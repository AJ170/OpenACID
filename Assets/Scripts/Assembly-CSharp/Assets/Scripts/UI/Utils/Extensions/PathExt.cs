using System.IO;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class PathExt
	{
		public static string Combine(params string[] _paths)
		{
			string text = null;
			bool flag = true;
			foreach (string text2 in _paths)
			{
				if (flag)
				{
					text = text2;
					flag = false;
				}
				else
				{
					text = Path.Combine(text, text2);
				}
			}
			return text;
		}
	}
}
