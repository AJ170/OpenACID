using AcierProtocol;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class TierExtensions
	{
		public static bool Locked(this AssassinSkillTier_ _tier)
		{
			return _tier.pointsRemaining > 0;
		}
	}
}
