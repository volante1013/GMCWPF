using System.Windows;

namespace GMCWPF
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private ManhoursViewModel manhoursViewModel;

		public MainWindow ()
		{
			InitializeComponent();

			this.MouseLeftButtonDown += (sender, e) => this.DragMove();
			RootStackPanel.MouseLeftButtonDown += (sender, e) => this.DragMove();

			manhoursViewModel = new ManhoursViewModel(RootStackPanel);
		}

		private void CompleteBtn_Click (object sender, RoutedEventArgs e)
		{
			manhoursViewModel.CompleteBtn_Click();
		}
	}
}
