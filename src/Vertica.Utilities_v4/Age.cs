using System;

namespace Vertica.Utilities_v4
{
	/* based on: http://www.singular.co.nz/blog/archive/2007/05/01/building-an-age-class-in-csharp.aspx */
	public struct Age
	{
		#region construction

		public static readonly Age Empty = new Age(DateTime.MinValue, DateTime.MinValue);

		/// <summary>
		/// Creates a new instance of Age starting from the given date until now
		/// </summary>
		/// <param name="advent"></param>
		public Age(DateTime advent)
			: this(advent, getNowFromKind(advent))
		{
		}

		/// <summary>
		/// Creates a new instance of Age starting from the given date until now
		/// </summary>
		/// <param name="advent"></param>
		/// <param name="terminus"></param>
		// Implementation based on http://tommycarlier.blogspot.com/2006/02/years-months-and-days-between-2-dates.html#links
		public Age(DateTime advent, DateTime terminus)
		{
			_advent = advent;
			_terminus = terminus;

			// only extracts down to 'days', so the TimeOfDay needs to be 
			// normalised for this calculation
			TimeSpan time = advent.TimeOfDay;
			if (time > TimeSpan.Zero)
			{
				advent = advent.Subtract(time);
				terminus = terminus.Subtract(time);
			}

			_years = terminus.Year - advent.Year;
			_months = terminus.Month - advent.Month;
			_days = terminus.Day - advent.Day;
			_weeks = 0;

			if (_days < 0) _months -= 1;
			while (_months < 0)
			{
				_months += 12;
				_years -= 1;
			}

			TimeSpan span = terminus - advent.AddYears(_years).AddMonths(_months);
			_days = (int)Math.Floor(span.TotalDays);

			if (_days > 0)
			{
				_weeks = _days / 7;
				_days = _days % 7;
			}
		}

		/// <summary>
		/// Gets the current date/time for universal vrs local time
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		private static DateTime getNowFromKind(DateTime time)
		{
			return time.Kind == DateTimeKind.Utc ? Time.UtcNow.DateTime : Time.Now.DateTime;
		}

		#endregion

		#region properties

		private readonly int _years;
		/// <summary>
		/// Gets the number of years old
		/// </summary>
		public int Years
		{
			get { return _years; }
		}

		private readonly int _months;
		/// <summary>
		/// Gets the number of months old
		/// </summary>
		public int Months
		{
			get { return _months; }
		}

		private readonly int _weeks;
		/// <summary>
		/// Gets the number of weeks old
		/// </summary>
		public int Weeks
		{
			get { return _weeks; }
		}

		private readonly int _days;
		/// <summary>
		/// Gets the number of days old
		/// </summary>
		public int Days
		{
			get { return _days; }
		}

		private readonly DateTime _advent;
		/// <summary>
		/// Gets/Sets the start of the age
		/// </summary>
		public DateTime Advent
		{
			get { return _advent; }
		}

		private readonly DateTime _terminus;
		/// <summary>
		/// Gets/Sets the end of the age
		/// </summary>
		public DateTime Terminus
		{
			get { return _terminus; }
		}

		/// <summary>
		/// Gets the elapsed time
		/// </summary>
		public TimeSpan Elapsed
		{
			get { return new TimeSpan(_terminus.Ticks - _advent.Ticks); }
		}

		/// <summary>
		/// Gets a value indicating that the age is empty (both advent and terminus dates are MinValue)
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty
		{
			get
			{
				return _advent == DateTime.MinValue
					   && _terminus == DateTime.MinValue;
			}
		}

		#endregion
	}
}