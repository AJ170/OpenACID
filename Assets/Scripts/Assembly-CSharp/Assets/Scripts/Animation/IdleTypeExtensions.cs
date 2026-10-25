using Assets.Scripts.Mission.Definitions;

namespace Assets.Scripts.Animation
{
	public static class IdleTypeExtensions
	{
		public static bool HasRootMotion(this IdleType _type)
		{
			bool result = true;
			if (_type == IdleType.CapturedNpcMale)
			{
				result = false;
			}
			return result;
		}

		public static AnimatorStateMachineStatic.StateHashValue GetStateHashValue(this IdleType _idletType)
		{
			return CharacterData.GetStateHashStatic(_idletType);
		}
	}
}
