using AcierProtocol;
using Assets.Scripts.UI.Utils.Extensions;
using Database;

namespace Assets.Scripts.UI.MVC.Models.Assassin
{
	public class UIAssassinLeaderboardGeneratedUserDetailsModel : UIAssassinLeaderboardUserDetailsModel
	{
		public override string ClassName
		{
			get
			{
				if (base.LeaderBoardUser != null)
				{
					LeaderBoardUser.GeneratedLeaderBoardAssassin generatedLeaderBoardAssassin = base.LeaderBoardUser.generatedLeaderBoardAssassin;
					if (generatedLeaderBoardAssassin != null)
					{
						return AssassinExtensions.GetAssassinClassDisplayNames(generatedLeaderBoardAssassin.primaryClass, generatedLeaderBoardAssassin.secondaryClass);
					}
				}
				return string.Empty;
			}
		}

		public override AssassinSummary Summery
		{
			get
			{
				if (base.LeaderBoardUser != null)
				{
					LeaderBoardUser.GeneratedLeaderBoardAssassin generatedLeaderBoardAssassin = base.LeaderBoardUser.generatedLeaderBoardAssassin;
					if (generatedLeaderBoardAssassin != null)
					{
						AssassinSummary assassinSummary = new AssassinSummary();
						assassinSummary.Armour = generatedLeaderBoardAssassin.armorRating;
						assassinSummary.Damage = generatedLeaderBoardAssassin.damage;
						assassinSummary.Health = generatedLeaderBoardAssassin.health;
						assassinSummary.Power = (int)generatedLeaderBoardAssassin.itemPower;
						return assassinSummary;
					}
				}
				return null;
			}
		}

		public override void GetAssasssinPortraitData(ref GameID _headId, ref GameID _humanoidId, ref GameID _colorId, ref GameID _itemId)
		{
			if (base.LeaderBoardUser != null && base.LeaderBoardUser.generatedLeaderBoardAssassin != null)
			{
				GameDB gameDB = Main.Instance.GameDB;
				LeaderBoardUser.GeneratedLeaderBoardAssassin generatedLeaderBoardAssassin = base.LeaderBoardUser.generatedLeaderBoardAssassin;
				_headId = gameDB.FindGameIdFromGuid(generatedLeaderBoardAssassin.headDefGuid);
				_humanoidId = gameDB.FindGameIdFromGuid(generatedLeaderBoardAssassin.outfitId);
				_colorId = gameDB.FindGameIdFromGuid(generatedLeaderBoardAssassin.colorDefGuid);
			}
		}
	}
}
