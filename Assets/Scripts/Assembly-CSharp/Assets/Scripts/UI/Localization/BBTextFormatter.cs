using System;
using System.Collections.Generic;
using System.Globalization;

namespace Assets.Scripts.UI.Localization
{
	public class BBTextFormatter : IFormatProvider, ICustomFormatter
	{
		private static readonly List<Type> s_NumberTypes = new List<Type>
		{
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
			typeof(short),
			typeof(ushort),
			typeof(float),
			typeof(double),
			typeof(decimal)
		};

		private static readonly CultureInfo s_Culture = new CultureInfo(string.Empty);

		private static NumberFormatInfo s_nfiPlain;

		private static NumberFormatInfo s_nfiStartsWithT;

		public object GetFormat(Type _formatType)
		{
			return (_formatType != typeof(ICustomFormatter)) ? null : this;
		}

		public string Format(string _format, object _arg, IFormatProvider _formatProvider)
		{
			if (!Equals(_formatProvider))
			{
				return null;
			}
			if (s_NumberTypes.Contains(_arg.GetType()))
			{
				return HandleNumberFormatting(_format, _arg);
			}
			try
			{
				return HandleOtherFormats(_format, _arg);
			}
			catch (FormatException innerException)
			{
				throw new FormatException(string.Format("Invalid format: {0}", _format), innerException);
			}
		}

		private static string HandleNumberFormatting(string _format, object _arg)
		{
			if (LocalizationManager.Instance.IsFinalLanguageInitilized)
			{
				if (s_nfiPlain == null)
				{
					s_nfiPlain = AllocateNumberFormat();
				}
				if (s_nfiStartsWithT == null)
				{
					s_nfiStartsWithT = AllocateNumberFormat();
					s_nfiStartsWithT.NumberGroupSizes = new int[1];
					s_nfiStartsWithT.CurrencyGroupSizes = new int[1];
					s_nfiStartsWithT.PercentGroupSizes = new int[1];
				}
			}
			NumberFormatInfo numberFormatInfo = s_nfiPlain;
			if (string.IsNullOrEmpty(_format))
			{
				_format = "N0";
			}
			if (_format.StartsWith("t", true, CultureInfo.InvariantCulture) || _format.StartsWith("p", true, CultureInfo.InvariantCulture))
			{
				_format = "#.##";
				numberFormatInfo = s_nfiStartsWithT;
			}
			if (numberFormatInfo != null)
			{
				s_Culture.NumberFormat = numberFormatInfo;
			}
			return ((IFormattable)_arg).ToString(_format, s_Culture);
		}

		private static string HandleOtherFormats(string _format, object _args)
		{
			if (_args is IFormattable)
			{
				return ((IFormattable)_args).ToString(_format, CultureInfo.InvariantCulture);
			}
			if (_args != null)
			{
				return NGUIText.EscapeSymbols(_args.ToString());
			}
			return string.Empty;
		}

		private static NumberFormatInfo AllocateNumberFormat()
		{
			NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
			numberFormatInfo.CurrencyDecimalSeparator = LocalizationManager.Instance.Get(3471);
			numberFormatInfo.NumberDecimalSeparator = LocalizationManager.Instance.Get(3471);
			numberFormatInfo.PercentDecimalSeparator = LocalizationManager.Instance.Get(3471);
			numberFormatInfo.CurrencyGroupSeparator = LocalizationManager.Instance.Get(3470);
			numberFormatInfo.NumberGroupSeparator = LocalizationManager.Instance.Get(3470);
			numberFormatInfo.PercentGroupSeparator = LocalizationManager.Instance.Get(3470);
			return numberFormatInfo;
		}
	}
}
