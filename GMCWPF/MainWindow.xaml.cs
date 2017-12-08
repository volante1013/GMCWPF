using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace GMCWPF
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private StringBuilder builder;

		private XmlSerializer serializer;

		private List<Manhours> manhoursList;


		private string initName = "ここに工数名を入力";

		public MainWindow ()
		{
			InitializeComponent();

			builder = new StringBuilder();

			manhoursList = new List<Manhours>();
			var mh = new Manhours(initName, 100);
			manhoursList.Add(mh);
			addManhoursMain(mh);
		}

		private void CompleteBtn_Click (object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("クリック！");
			setTextToClipBoard();
		}

		private void PlusBtn_Click (object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			if (button.Name.Contains("Main"))
			{
				var mh = new Manhours(initName, 0);
				manhoursList.Add(mh);
				addManhoursMain(mh);
			}
			else if (button.Name.Contains("Content"))
			{
				var parent = ( ( ( button.Parent as DockPanel ).Parent as DockPanelContent ).Parent as TreeViewItem ).Parent as TreeViewItem;
				var index = treeView.Items.IndexOf(parent);
				addManhoursContent(manhoursList[index], parent);
			}
		}

		private void MinusBtn_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			if (button.Name.Contains("Main"))
			{
				if(treeView.Items.Count <= 1)
				{
					return;
				}

				var parent = ( ( button.Parent as DockPanel ).Parent as DockPanelMain ).Parent as TreeViewItem;
				manhoursList.RemoveAt(treeView.Items.IndexOf(parent));
				treeView.Items.Remove(parent);
			}
			else if (button.Name.Contains("Content"))
			{
				var item = ( ( button.Parent as DockPanel ).Parent as DockPanelContent ).Parent as TreeViewItem;
				var parent = item.Parent as TreeViewItem;
				if(parent.Items.Count <= 1)
				{
					return;
				}

				var index = treeView.Items.IndexOf(parent);
				manhoursList[index].Contents.RemoveAt(parent.Items.IndexOf(item));
				parent.Items.Remove(item);
			}
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

		private void addManhoursMain(Manhours manhours)
		{
			var dpMain = new DockPanelMain();
			dpMain.GetPlusBtn.Click += PlusBtn_Click;
			dpMain.GetMinusBtn.Click += MinusBtn_Click;
			dpMain.setBindPerBox(manhours);
			dpMain.setBindNameBox(manhours);

			var treeViewItem = new TreeViewItem();
			treeViewItem.IsExpanded = true;
			treeViewItem.Header = dpMain as UIElement;
			treeView.Items.Add(treeViewItem);

			addManhoursContent(manhours, treeViewItem);
		}

		private void addManhoursContent (Manhours manhours, TreeViewItem parentTreeViewItem)
		{

			var dpContent = new DockPanelContent();

			var treeViewItem = new TreeViewItem();
			treeViewItem.Header = dpContent;
			parentTreeViewItem.Items.Add(treeViewItem);
			var index = parentTreeViewItem.Items.IndexOf(treeViewItem);

			dpContent.GetPlusBtn.Click += PlusBtn_Click;
			dpContent.GetMinusBtn.Click += MinusBtn_Click;
			manhours.Contents.Add("ここに工数の内容を入力");
			dpContent.setBindContentBox(manhours, index);
		}
	}
}
