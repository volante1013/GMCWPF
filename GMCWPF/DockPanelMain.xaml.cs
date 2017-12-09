using System.Windows.Controls;
using System.Windows.Data;

namespace GMCWPF
{
	/// <summary>
	/// DockPanelMain.xaml の相互作用ロジック
	/// </summary>
	public partial class DockPanelMain : UserControl
	{
		public DockPanelMain ()
		{
			InitializeComponent();
		}

		public Button GetPlusBtn { get { return PlusBtn_Main; } }
		public Button GetMinusBtn { get { return MinusBtn_Main; } }

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
	}
}
