using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class TextColors
	{
		public Color32 NegativeColor = new Color32(byte.MaxValue, 0, 6, byte.MaxValue);

		public Color32 PositiveColor = new Color32(120, byte.MaxValue, 45, byte.MaxValue);

		public Color32 ShopMundaneOverideColor = new Color32(180, 180, 180, byte.MaxValue);

		public Color32 ShopMundaneInsigniaOverideColor = new Color32(119, 119, 119, byte.MaxValue);

		public Color32 ShopMundaneInsigniaOverlayOverideColor = new Color32(119, 119, 119, byte.MaxValue);

		public Color32 ShopCategoryNameColor = new Color32(36, 106, 119, byte.MaxValue);

		public Color32 BlueHighLightColor = new Color32(6, 88, 102, byte.MaxValue);

		public Color32 ResourceInfoPopupHeaderColor = new Color32(50, 150, 210, byte.MaxValue);

		public Color32 TimerColor = new Color32(byte.MaxValue, 121, 49, byte.MaxValue);

		public Color32 SkillColor = new Color32(50, 150, 210, byte.MaxValue);

		public Color32 EnergyAffortable = new Color32(50, 150, 210, byte.MaxValue);

		public Color32 EnergyToLow = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

		public Color LegendaryColor = new Color32(240, 184, 105, byte.MaxValue);

		private Color eliteInactiveHeaderColor;

		public Color32 ShopNoQualityColor
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.NoItemQualityColor).Color0;
			}
		}

		public Color32 BluePrintQualityColor
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.BlueprintGlowColor).Color0;
			}
		}

		public Color32 BluePrintFrameQualityColor
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.BlueprintFrameColor).Color0;
			}
		}

		public Color32 LockPickQualityColor
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.LockpicksGlowColor).Color0;
			}
		}

		public Color32 LockPickFrameQualityColor
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.LockpicksFrameColor).Color0;
			}
		}

		public Color32 EliteMissionTitleColor
		{
			get
			{
				return new Color32(byte.MaxValue, 191, 90, byte.MaxValue);
			}
		}

		public Color32 HeroicShopCategoryNameColor
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HeroicShopCategoryNameColor).Color0;
			}
		}

		public Color32 HeroicShopFrameColor
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HeroicShopFrameColor).Color0;
			}
		}

		public Color32 HeroicShopGlowColor
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HeroicShopGlowColor).Color0;
			}
		}

		public Color32 HeroicShopBackgroundColor
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HeroicShopBackgroundColor).Color0;
			}
		}

		public Color32 DiscountOldPriceColorBright
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.DiscountOldPriceColorBright).Color0;
			}
		}

		public Color32 DiscountCrossOutLineColorBright
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.DiscountCrossOutLineColorBright).Color0;
			}
		}

		public Color32 DiscountOldPriceColorDark
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.DiscountOldPriceColorDark).Color0;
			}
		}

		public Color32 DiscountCrossOutLineColorDark
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.DiscountCrossOutLineColorDark).Color0;
			}
		}

		public Color32 CampaignUnlockBackgroundColor
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.CampaignUnlockBackgroundColor).Color0;
			}
		}

		public Color32 CampaignUnlockTitleColor
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.CampaignUnlockTitleColor).Color0;
			}
		}

		public Color32 CampaignUnlockExtraTitleColor
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.CampaignUnlockExtraTitleColor).Color0;
			}
		}

		public Color LeaderboardOwnPlayerColor
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HeroicShopFrameColor).Color0;
			}
		}

		public Color AssassinPreviewPopupBackgroundColor
		{
			get
			{
				return Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.AssassinPreviewPopupBackgroundColor).Color0;
			}
		}

		public Color LeaderboardDeselectedColor
		{
			get
			{
				return new Color32(150, 150, 150, byte.MaxValue);
			}
		}

		public Color LeaderboardOwnPlayerDeselectedColor
		{
			get
			{
				return new Color32(213, 160, 77, byte.MaxValue);
			}
		}

		public Color32 InactiveHeaderText
		{
			get
			{
				return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}

		public Color32 SelectedHeaderText
		{
			get
			{
				return new Color32(17, 93, 106, byte.MaxValue);
			}
		}
	}
}
