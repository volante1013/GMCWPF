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

		public void setBindContentBox (Manhours manhours, int index)
		{
			var binding = new Binding(string.Format($"[{index}]")) { Source = manhours };
			ContentBox.SetBinding(TextBox.TextProperty, binding);
		}

		private void ContentBox_GotFocus (object sender, System.Windows.RoutedEventArgs e)
		{
			Dispatcher.InvokeAsync(() => ContentBox.SelectAll());
		}
	}
}
