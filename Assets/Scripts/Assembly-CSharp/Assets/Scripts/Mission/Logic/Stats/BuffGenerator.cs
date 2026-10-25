using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.Stats
{
	internal static class BuffGenerator
	{
		private static readonly BBList<IBuff> s_BuffList = new BBList<IBuff>();

		public static void ForceList(this IBuffGenerator _buffGenerator, ref BBList<IBuff> _list)
		{
			if (_list == null)
			{
				_list = s_BuffList;
			}
		}
	}
}
