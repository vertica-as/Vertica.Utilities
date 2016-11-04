using System;
using System.Configuration;
using System.Globalization;

namespace Vertica.Utilities_v4.Configuration
{
	public class CollectionCountValidator : ConfigurationValidatorBase
	{
		private readonly uint _minimumCount;

		public CollectionCountValidator(uint minCount)
		{
			_minimumCount = minCount;
		}

		public override bool CanValidate(Type type)
		{
			return type.IsSubclassOf(typeof(ConfigurationElementCollection));
		}

		public override void Validate(object value)
		{
			var collection = (ConfigurationElementCollection)value;

			Guard.Against<ConfigurationErrorsException>(
				collection.Count < _minimumCount,
				Resources.Exceptions.CollectionCountValidator_MessageTemplate,
				collection.GetType().Name,
				_minimumCount.ToString(CultureInfo.InvariantCulture),
				collection.Count.ToString(CultureInfo.InvariantCulture));
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class CollectionCountValidatorAttribute : ConfigurationValidatorAttribute
	{
		public uint MinCount { get; set; }

		public override ConfigurationValidatorBase ValidatorInstance => new CollectionCountValidator(MinCount);
	}
}
