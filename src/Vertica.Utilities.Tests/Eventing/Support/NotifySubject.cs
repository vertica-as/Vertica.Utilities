using System.ComponentModel;
using Vertica.Utilities.Eventing;

namespace Vertica.Utilities.Tests.Eventing.Support
{
	internal class NotifySubject : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string _s;
		public string S
		{
			get { return _s; }
			set
			{
				_s = value;
				this.Notify(PropertyChanged, i => i.S);
			}
		}

		private int _i;
		public int I
		{
			get { return _i; }
			set
			{
				int old = _i;
				_i = value;
				this.Notify(PropertyChanged, i => i.I, old, value);
			}
		}


		private decimal _d;
		public decimal D
		{
			get { return _d; }
			set
			{
				_d = value;
				PropertyChanged.Raise(this, "D");
			}
		}
	}
}
