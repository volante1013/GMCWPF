using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace GMCWPF
{
	public class Manhours : INotifyPropertyChanged
	{
		private string _Name;
		public string Name
		{
			get { return _Name; }
			set { _Name = value; NotifyPropertyChanged(); }
		}

		private int _Percent;

		public int Percent
		{
			get { return _Percent; }
			set { _Percent = value; NotifyPropertyChanged(); }
		}

		public List<string> Contents { get; }

		public string this[int index]
		{
			get { return Contents[index]; }
			set
			{
				if (Contents.Count <= index || Contents[index] == value)
					return;

				Contents[index] = value;
				NotifyPropertyChanged(Binding.IndexerName);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void NotifyPropertyChanged ([CallerMemberName]string propertyName = "")
			=> this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		public Manhours ()
		{
			_Name = "";
			_Percent = 0;
			Contents = new List<string>();
		}

		public Manhours(string name, int percent, string content)
		{
			_Name = name;
			_Percent = percent;
			Contents = new List<string>();
			Contents.Add(content);
		}
	}
}
