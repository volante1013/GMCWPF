using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GMCWPF
{
	public abstract class BindableBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void SetProperty<T> (ref T storage, T value, [CallerMemberName]string propertyName = null)
		{
			if(object.Equals(storage, value))
			{
				return;
			}

			storage = value;
			NotifyPropertyChanged(propertyName);
		}

		protected void NotifyPropertyChanged ([CallerMemberName]string propertyName = null)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
