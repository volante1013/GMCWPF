using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Serialization;

namespace GMCWPF
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private StringBuilder builder = new StringBuilder();

		private XmlSerializer serializer = new XmlSerializer(typeof(List<Manhours>));

		private List<Manhours> manhoursList;

		private static readonly string dicPath = Path.GetFullPath("..\\..\\Serialize");
		private static readonly string filePath = dicPath + "\\Manhours.xml";
		private const string initName = "ここに工数名を入力";
		private const string initContent = "ここに工数の内容を入力";

		public MainWindow ()
		{
			InitializeComponent();
			setManhoursList();
		}

		#region イベント
		private void OnMouseLeftButtonDown (object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void CompleteBtn_Click (object sender, RoutedEventArgs e)
		{
			setTextToClipBoard();
			serializeManhoursList();
		}

		private void PlusBtn_Click (object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			if (button.Name.Contains("Main"))
			{
				var mh = new Manhours(initName, 0, initContent);
				manhoursList.Add(mh);
				addManhoursMain(mh);
			}
			else if (button.Name.Contains("Content"))
			{
				var stackPanel = ( ( button.Parent as DockPanel ).Parent as DockPanelContent ).Parent as StackPanel;
				var index = RootStackPanel.Children.IndexOf(stackPanel);
				manhoursList[index].Contents.Add(initContent);
				addManhoursContent(manhoursList[index], stackPanel);
			}
		}

		private void MinusBtn_Click (object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			if (button.Name.Contains("Main"))
			{
				if(RootStackPanel.Children.Count <= 1)
				{
					return;
				}

				var stackPanel = ( ( button.Parent as DockPanel ).Parent as DockPanelMain ).Parent as StackPanel;
				manhoursList.RemoveAt(RootStackPanel.Children.IndexOf(stackPanel));
				RootStackPanel.Children.Remove(stackPanel);
			}
			else if (button.Name.Contains("Content"))
			{
				var dockPanel = ( button.Parent as DockPanel ).Parent as DockPanelContent;
				var stackPanel = dockPanel.Parent as StackPanel;
				if(stackPanel.Children.Count <= 2)
				{
					return;
				}
				var index = RootStackPanel.Children.IndexOf(stackPanel);
				manhoursList[index].Contents.RemoveAt(stackPanel.Children.IndexOf(dockPanel) - 1);
				stackPanel.Children.Remove(dockPanel);
			}
		}
		#endregion

		private void setManhoursList ()
		{
			if (!Directory.Exists(dicPath))
			{
				Directory.CreateDirectory(dicPath);
			}

			if (File.Exists(filePath))
			{
				var doDeserialize = MessageBox.Show("前回の工数コメントを復元しますか", "Select", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (doDeserialize == MessageBoxResult.Yes)
				{
					deserializeManhoursList();
					return;
				}
			}

			MessageBox.Show("工数コメントを新規作成します", "New Create", MessageBoxButton.OK, MessageBoxImage.Information);
			manhoursList = new List<Manhours>();
			var mh = new Manhours(initName, 100, initContent);
			manhoursList.Add(mh);
			addManhoursMain(mh);
		}

		private void setTextToClipBoard ()
		{
			manhoursList.ForEach((mh) =>
			{
				builder.AppendLine("■" + mh.Name + "：" + mh.Percent.ToString() + "%");
				mh.Contents.ForEach((content) => builder.AppendLine(" - " + content));
			});

			System.Windows.Forms.Clipboard.SetText(builder.ToString());
			MessageBox.Show("以下の内容をクリップボードに貼り付けました\n" + builder.ToString(),
							"Result", MessageBoxButton.OK, MessageBoxImage.Information);

			builder.Clear();
		}

		private void serializeManhoursList ()
		{
			using (StreamWriter sw = new StreamWriter(filePath, false, new UTF8Encoding(false)))
			{
				serializer.Serialize(sw, manhoursList);
				sw.Close();
			}
		}

		private void deserializeManhoursList ()
		{
			using (StreamReader sr = new StreamReader(filePath, new UTF8Encoding(false)))
			{
				manhoursList = serializer.Deserialize(sr) as List<Manhours>;
				sr.Close();
			}
			manhoursList?.ForEach(mh => addManhoursMain(mh));
		}

		private void addManhoursMain(Manhours manhours)
		{
			var dpMain = new DockPanelMain(manhours);
			dpMain.PlusBtn_Main.Click += PlusBtn_Click;
			dpMain.MinusBtn_Main.Click += MinusBtn_Click;

			var stackPanel = new StackPanel();
			RootStackPanel.Children.Add(stackPanel);
			stackPanel.Children.Add(dpMain);

			manhours.Contents.ForEach(content => addManhoursContent(manhours, stackPanel));
		}

		private void addManhoursContent (Manhours manhours, StackPanel stackPanel)
		{
			var dpContent = new DockPanelContent();
			stackPanel.Children.Add(dpContent);
			var index = stackPanel.Children.IndexOf(dpContent);

			dpContent.PlusBtn_Content.Click += PlusBtn_Click;
			dpContent.MinusBtn_Content.Click += MinusBtn_Click;
			dpContent.ContentBox.SetBinding(TextBox.TextProperty, new Binding($"[{index - 1}]") { Source = manhours });
		}

	}
}
