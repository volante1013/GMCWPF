using System.Windows.Controls;

namespace GMCWPF
{
	/// <summary>
	/// DockPanelMain.xaml の相互作用ロジック
	/// </summary>
	public partial class DockPanelMain : UserControl
	{
		public DockPanelMain (Manhours manhours)
		{
			InitializeComponent();

			this.DataContext = manhours;
		}

		private void TextBox_GotFocus (object sender, System.Windows.RoutedEventArgs e)
		{
			Dispatcher.InvokeAsync(() => ( sender as TextBox ).SelectAll());
		}
	}
}
