using System;
using System.Collections.Generic;
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

		public Button GetPlusBtn { get{ return PlusBtn_Content; } }
		public Button GetMinusBtn { get { return MinusBtn_Content; } }

		public void setBindContentBox (Manhours manhours, int index)
		{
			var binding = new Binding(string.Format($"[{index}]")) { Source = manhours };
			ContentBox.SetBinding(TextBox.TextProperty, binding);
		}
	}
}
