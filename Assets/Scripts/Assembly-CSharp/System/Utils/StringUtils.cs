using System.Text;

namespace System.Utils
{
	public class StringUtils
	{
		public static string Wrap(string _text, int _width, string _newline)
		{
			if (string.IsNullOrEmpty(_text))
			{
				return "-";
			}
			_text = _text.Replace("\r\n", _newline);
			_text = _text.Replace("\n", _newline);
			StringBuilder stringBuilder = new StringBuilder();
			if (_width < 1)
			{
				return _text;
			}
			int i = 0;
			while (i < _text.Length)
			{
				int num = _text.IndexOf(_newline, i, StringComparison.Ordinal);
				int num2 = ((num != -1) ? (num + _newline.Length) : (num = _text.Length));
				if (num > i)
				{
					do
					{
						int num3 = num - i;
						if (num3 > _width)
						{
							num3 = BreakLine(_text, i, _width);
						}
						stringBuilder.Append(_text, i, num3);
						stringBuilder.Append(_newline);
						for (i += num3; i < num && char.IsWhiteSpace(_text[i]); i++)
						{
						}
					}
					while (num > i);
				}
				else
				{
					stringBuilder.Append(_newline);
				}
				i = num2;
			}
			return stringBuilder.ToString();
		}

		public static int BreakLine(string _text, int _pos, int _max)
		{
			int num = _max - 1;
			while (num >= 0 && !char.IsWhiteSpace(_text[_pos + num]))
			{
				num--;
			}
			if (num < 0)
			{
				return _max;
			}
			while (num >= 0 && char.IsWhiteSpace(_text[_pos + num]))
			{
				num--;
			}
			return num + 1;
		}
	}
}
