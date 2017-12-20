using System.Windows.Controls;
using System.Windows.Data;

namespace GMCWPF
{
	/// <summary>
	/// DockPanelContent.xaml の相互作用ロジック
	/// </summary>
	public partial class DockPanelContent : UserControl
	{
		public DockPanelContent ()
		{
			InitializeComponent();
		}

		private void ContentBox_GotFocus (object sender, System.Windows.RoutedEventArgs e)
		{
			Dispatcher.InvokeAsync(() => ContentBox.SelectAll());
		}
	}
}
