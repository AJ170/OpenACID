using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Models.Misc;
using Assets.Scripts.UI.Utils;

namespace Assets.Scripts.UI.MVC.Models.DailyQuest
{
	public class UIDailyQuestModel : UITabButtonModel
	{
		public ProgressionQuests.RankQuests RankQuest { get; set; }

		public HeroicRankDescription RankDescription { get; set; }

		public override long EndTimeUTC
		{
			get
			{
				return (long)((RankQuest == null) ? 0 : RankQuest.expiresAtTimeStamp);
			}
		}

		public override StringId TitleLoca
		{
			get
			{
				return 8291;
			}
		}

		public override StringId SubTitleLoca
		{
			get
			{
				return (RankDescription == null) ? StringId.Invalid : RankDescription.NameId;
			}
		}

		public override bool IsEnabled
		{
			get
			{
				return true;
			}
		}
	}
}
