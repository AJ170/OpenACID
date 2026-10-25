using Database;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class DatabaseExtensions
	{
		public static bool Valid(this GameID _id)
		{
			return _id != GameID.Invalid;
		}
	}
}
