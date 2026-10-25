using System;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;

namespace Assets.Scripts.UI.Types
{
	public class UIPercentage : IFormattable
	{
		private static readonly StringId s_percentageId = new StringId(1902);

		public float Value { get; set; }

		public StringId OptionalStringId { get; set; }

		public UIPercentage(float _value, StringId _optionalStringId = null)
		{
			Value = _value;
			OptionalStringId = _optionalStringId;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override string ToString()
		{
			return ToString(string.Empty, null);
		}

		public string ToString(string _format)
		{
			return ToString(_format, null);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			StringId key = OptionalStringId ?? s_percentageId;
			int num = UIUtil.FormatPercentValue(Value);
			return LocalizationManager.Instance.Get(key, new StringMapper { 
			{
				"value",
				LocalizationManager.Instance.GetInteger(num)
			} });
		}
	}
}
