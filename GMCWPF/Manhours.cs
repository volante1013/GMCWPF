using System.Collections.Generic;
using System.Windows.Data;

namespace GMCWPF
{
	public class Manhours : BindableBase
	{
		private string _Name;
		public string Name
		{
			get { return _Name; }
			set { SetProperty(ref _Name, value); }
		}

		private int _Percent;
		public int Percent
		{
			get { return _Percent; }
			set { SetProperty(ref _Percent, value); }
		}

		public bool IsChangedPercent { get; set; } = false;

		public List<string> Contents { get; }

		public string this[int index]
		{
			get { return Contents[index]; }
			set
			{
				if (Contents.Count <= index || Contents[index] == value)
				{
					return;
				}

				Contents[index] = value;
				NotifyPropertyChanged(Binding.IndexerName);
			}
		}

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
			Contents = new List<string> { content };
		}
	}
}
