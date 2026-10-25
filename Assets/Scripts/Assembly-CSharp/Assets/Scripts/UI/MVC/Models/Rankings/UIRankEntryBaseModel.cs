using AcierProtocol;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Models.Rankings
{
	public class UIRankEntryBaseModel : UIModelBase
	{
		public LeaderBoardUser User;

		public int HeroicRank;

		public int Position;

		public string Name;

		public long Score;

		public string RelationIconName;

		public Color Color;
	}
}
