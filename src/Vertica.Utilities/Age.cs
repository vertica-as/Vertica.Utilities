using System;
using System.Globalization;
using System.Text;
using Vertica.Utilities.Extensions.StringExt;

namespace Vertica.Utilities
{
	public struct Age : IFormattable, IEquatable<Age>, IComparable, IComparable<Age>, IComparable<TimeSpan>
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
		public int Years => _years;

		private readonly int _months;
		/// <summary>
		/// Gets the number of months old
		/// </summary>
		public int Months => _months;

		private readonly int _weeks;
		/// <summary>
		/// Gets the number of weeks old
		/// </summary>
		public int Weeks => _weeks;

		private readonly int _days;
		/// <summary>
		/// Gets the number of days old
		/// </summary>
		public int Days => _days;

		private readonly DateTime _advent;
		/// <summary>
		/// Gets/Sets the start of the age
		/// </summary>
		public DateTime Advent => _advent;

		private readonly DateTime _terminus;
		/// <summary>
		/// Gets/Sets the end of the age
		/// </summary>
		public DateTime Terminus => _terminus;

		/// <summary>
		/// Gets the elapsed time
		/// </summary>
		public TimeSpan Elapsed => new TimeSpan(_terminus.Ticks - _advent.Ticks);

		/// <summary>
		/// Gets a value indicating that the age is empty (both advent and terminus dates are MinValue)
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty => _advent == DateTime.MinValue
		                       && _terminus == DateTime.MinValue;

		#endregion

		#region ToString

		public override string ToString()
		{
			return ToString(0, true);
		}

		public string ToString(int significantPlaces)
		{
			return ToString(significantPlaces, true);
		}

		public string ToString(int significantPlaces, bool includeTime)
		{
			if (IsEmpty)
			{
				return string.Empty;
			}

			int max = significantPlaces < 1 ? 10 : significantPlaces;
			int parts = 0;

			var result = new StringBuilder();
			if (_years > 0 && parts < max)
			{
				result.AppendFormat(" {0} year{1}", _years, plural(_years));
				++parts;
			}
			if (_months > 0 && parts < max)
			{
				result.AppendFormat(" {0} month{1}", _months, plural(_months));
				++parts;
			}
			if (_weeks > 0 && parts < max)
			{
				result.AppendFormat(" {0} week{1}", _weeks, plural(_weeks));
				++parts;
			}
			if (_days > 0 && parts < max)
			{
				result.AppendFormat(" {0} day{1}", _days, plural(_days));
				++parts;
			}

			if (includeTime)
			{
				TimeSpan time = Elapsed;
				if (time.Hours != 0 && parts < max)
				{
					result.AppendFormat(" {0} hour{1}", time.Hours, plural(time.Hours));
					++parts;
				}
				if (time.Minutes != 0 && parts < max)
				{
					result.AppendFormat(" {0} minute{1}", time.Minutes, plural(time.Minutes));
					++parts;
				}
				if (time.Seconds != 0 && parts < max)
				{
					result.AppendFormat(" {0} second{1}", time.Seconds, plural(time.Seconds));
					++parts;
				}
			}

			return result.Length == 0 ?
				includeTime ? "less than a second" : "less than a day"
				: result.ToString().Trim();
		}

		private static string plural(int number)
		{
			return number == 1 ? string.Empty : "s";
		}

		public string ToString(string format)
		{
			return ToString(format, CultureInfo.InvariantCulture);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			if (format.IsEmpty()) format = "g";

			char first = format[0];
			if (char.ToLower(first) == 'g')
			{
				int parts = 0;
				if (format.Length > 1 && char.IsDigit(format[1]))
					parts = int.Parse(format[1].ToString());
				return ToString(parts);
			}

			if (char.IsDigit(first))
			{
				int parts = int.Parse(first.ToString());
				return ToString(parts);
			}

			throw new FormatException("Could not parse the Age format: " + format);
		}

		#endregion

		#region Equality

		public bool Equals(Age other)
		{
			return _advent.Equals(other._advent) && _terminus.Equals(other._terminus);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Age && Equals((Age)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (_advent.GetHashCode() * 397) ^ _terminus.GetHashCode();
			}
		}

		public static bool operator ==(Age left, Age right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Age left, Age right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region comparisons

		public int CompareTo(object obj)
		{
			if (obj is Age)
			{
				return CompareTo((Age)obj);
			}
			if (obj is TimeSpan)
			{
				return CompareTo((TimeSpan)obj);
			}

			throw new ArgumentException(string.Format("The object '{0}' is of the wrong type for comparison.", obj.GetType()), "obj");
		}

		public int CompareTo(Age other)
		{
			return CompareTo(other.Elapsed);
		}

		public int CompareTo(TimeSpan other)
		{
			return Elapsed.CompareTo(other);
		}

		public static bool operator >(Age x, Age y)
		{
			return x.Elapsed > y.Elapsed;
		}

		public static bool operator <(Age x, Age y)
		{
			return x.Elapsed < y.Elapsed;
		}

		public static bool operator >=(Age x, Age y)
		{
			return x.Elapsed >= y.Elapsed;
		}

		public static bool operator <=(Age x, Age y)
		{
			return x.Elapsed <= y.Elapsed;
		}

		#endregion
	}
}