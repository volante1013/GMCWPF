using System.Windows.Controls;
using System.Windows.Data;

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

		public void setBindPerBox (Manhours manhours)
		{
			var binding = new Binding(nameof(manhours.Percent)) { Source = manhours };
			PercentBox.SetBinding(TextBox.TextProperty, binding);
		}

		public void setBindNameBox (Manhours manhours)
		{
			var binding = new Binding(nameof(manhours.Name)) { Source = manhours };
			NameBox.SetBinding(TextBox.TextProperty, binding);
		}

		private void TextBox_GotFocus (object sender, System.Windows.RoutedEventArgs e)
		{
			Dispatcher.InvokeAsync(() => ( sender as TextBox ).SelectAll());
		}
	}
}
