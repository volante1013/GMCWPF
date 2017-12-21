﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Serialization;

namespace GMCWPF
{
	public class ManhoursViewModel
	{
		private StringBuilder builder = new StringBuilder();

		private XmlSerializer serializer = new XmlSerializer(typeof(List<Manhours>));

		private List<Manhours> manhoursList;

		private static readonly string dicPath = Path.GetFullPath("..\\..\\Serialize");
		private static readonly string filePath = dicPath + "\\Manhours.xml";
		private const string initName = "ここに工数名を入力";
		private const string initContent = "ここに工数の内容を入力";

		private StackPanel RootStackPanel;

		public ManhoursViewModel (StackPanel stackPanel)
		{
			RootStackPanel = stackPanel;

			SetManhoursList();
		}

		#region イベント
		public void CompleteBtn_Click ()
		{
			SetTextToClipBoard();
			SerializeManhoursList();
		}

		private void PlusBtn_Click (object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			if (button.Name.Contains("Main"))
			{
				var mh = new Manhours(initName, 0, initContent);
				manhoursList.Add(mh);
				AddManhoursMain(mh);
			}
			else if (button.Name.Contains("Content"))
			{
				var stackPanel = ( ( button.Parent as DockPanel ).Parent as DockPanelContent ).Parent as StackPanel;
				var index = RootStackPanel.Children.IndexOf(stackPanel);
				manhoursList[index].Contents.Add(initContent);
				AddManhoursContent(manhoursList[index], stackPanel);
			}
		}

		private void MinusBtn_Click (object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			if (button.Name.Contains("Main"))
			{
				if (RootStackPanel.Children.Count <= 1)
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
				if (stackPanel.Children.Count <= 2)
				{
					return;
				}
				var index = RootStackPanel.Children.IndexOf(stackPanel);
				manhoursList[index].Contents.RemoveAt(stackPanel.Children.IndexOf(dockPanel) - 1);
				stackPanel.Children.Remove(dockPanel);
			}
		}

		private void UpDownBtn_Click (object sender, RoutedEventArgs e)
		{
			int UpOrDownIndex = 1;

			var button = sender as Button;
			var dp = button.Parent as DockPanel;
			var dpContent = dp.Parent as DockPanelContent;
			var stackPanel = dpContent.Parent as StackPanel;
			var index = stackPanel.Children.IndexOf(dpContent);
			if (button.Name.Contains("Up"))
			{
				if (index <= 1)
					return;

				UpOrDownIndex *= -1;
			}
			else if (button.Name.Contains("Down"))
			{
				if (index >= stackPanel.Children.Count - 1)
					return;
			}

			var mh = manhoursList[RootStackPanel.Children.IndexOf(stackPanel)];
			index -= 1;
			var tmp = mh[index];
			mh[index] = mh[index + UpOrDownIndex];
			mh[index + UpOrDownIndex] = tmp;
		}

		private void PercentBox_LostFocus (object sender, RoutedEventArgs e)
		{
			var stackPanel = ( ( ( sender as TextBox ).Parent as DockPanel ).Parent as DockPanelMain ).Parent as StackPanel;
			var mh = manhoursList[RootStackPanel.Children.IndexOf(stackPanel)];
			mh.IsChangedPercent = true;
			if (manhoursList.All(man => man.IsChangedPercent))
			{
				manhoursList.ForEach(man => man.IsChangedPercent = false);
				mh.IsChangedPercent = true;
			}

			var NotChangedMh = manhoursList.Where(man => !man.IsChangedPercent);
			if (NotChangedMh.Count() != 1)
			{
				return;
			}

			NotChangedMh.Single().Percent = 100 - manhoursList.Where(man => man.IsChangedPercent).Select(man => man.Percent).Sum();
		}
		#endregion

		private void SetManhoursList ()
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
					DeserializeManhoursList();
					return;
				}
			}

			MessageBox.Show("工数コメントを新規作成します", "New Create", MessageBoxButton.OK, MessageBoxImage.Information);
			manhoursList = new List<Manhours>();
			var mh = new Manhours(initName, 100, initContent);
			manhoursList.Add(mh);
			AddManhoursMain(mh);
		}

		private void SetTextToClipBoard ()
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

		private void SerializeManhoursList ()
		{
			using (StreamWriter sw = new StreamWriter(filePath, false, new UTF8Encoding(false)))
			{
				serializer.Serialize(sw, manhoursList);
				sw.Close();
			}
		}

		private void DeserializeManhoursList ()
		{
			using (StreamReader sr = new StreamReader(filePath, new UTF8Encoding(false)))
			{
				manhoursList = serializer.Deserialize(sr) as List<Manhours>;
				sr.Close();
			}
			manhoursList?.ForEach(mh => AddManhoursMain(mh));
		}

		private void AddManhoursMain (Manhours manhours)
		{
			var dpMain = new DockPanelMain(manhours);
			dpMain.PlusBtn_Main.Click += PlusBtn_Click;
			dpMain.MinusBtn_Main.Click += MinusBtn_Click;
			dpMain.PercentBox.LostFocus += PercentBox_LostFocus;

			var stackPanel = new StackPanel();
			RootStackPanel.Children.Add(stackPanel);
			stackPanel.Children.Add(dpMain);

			manhours.Contents.ForEach(content => AddManhoursContent(manhours, stackPanel));
		}

		private void AddManhoursContent (Manhours manhours, StackPanel stackPanel)
		{
			var dpContent = new DockPanelContent();
			stackPanel.Children.Add(dpContent);
			var index = stackPanel.Children.IndexOf(dpContent);

			dpContent.PlusBtn_Content.Click += PlusBtn_Click;
			dpContent.MinusBtn_Content.Click += MinusBtn_Click;
			dpContent.UpBtn_Content.Click += UpDownBtn_Click;
			dpContent.DownBtn_Content.Click += UpDownBtn_Click;
			dpContent.ContentBox.SetBinding(TextBox.TextProperty,
				new Binding($"[{index - 1}]")
				{
					Source = manhours,
					UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
				});
		}

	}
}
