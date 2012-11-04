using System.ComponentModel;
using Vertica.Utilities_v4.Eventing;

namespace Vertica.Utilities_v4.Tests.Eventing.Support
{
	internal class NotifySubject : INotifyPropertyChanged, INotifyPropertyChanging
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public event PropertyChangingEventHandler PropertyChanging;

		private string _s;
		public string S
		{
			get { return _s; }
			set
			{
				this.Notify(PropertyChanging, i => i.S);
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
				this.Notify(PropertyChanging, i => i.I, old, value);
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
				PropertyChanging.Raise(this, "D");
				_d = value;
				PropertyChanged.Raise(this, "D");
			}
		}

		private float _f;
		public float F
		{
			get { return _f; }
			set
			{
				float  old = _f;
				bool cancelled = this.Notify(PropertyChanging, i => i.F, old, value);
				if (!cancelled)
				{
					_f = value;
					this.Notify(PropertyChanged, i => i.F, old, value);	
				}
			}
		}
	}
}
