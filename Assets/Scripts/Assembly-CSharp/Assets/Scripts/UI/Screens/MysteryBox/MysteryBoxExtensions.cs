using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Screens.MysteryBox
{
	public static class MysteryBoxExtensions
	{
		public static MysteryBoxMapping.Entry GetEntryByQualityAndType(this MysteryBoxMapping _mapping, Quality.EQuality _quality, EMysteryBoxType _type)
		{
			MysteryBoxMapping.Entry entry = null;
			for (int i = 0; i < _mapping.entries.Length; i++)
			{
				entry = _mapping.entries[i];
				if (_type == entry.type && _quality == entry.quality)
				{
					return entry;
				}
			}
			return null;
		}

		public static MysteryBoxMapping.Entry Info(this MysteryBoxChain _chain)
		{
			return Singleton<StateManager>.Instance.TemplateManager.MysteryBoxMapping.GetEntryByQualityAndType(_chain.quality, _chain.mysteryBoxType);
		}

		public static void FillMysteryBoxStringMapper(this EMysteryBoxType _type, Quality.EQuality _quality, ref StringMapper _mapper)
		{
			MysteryBoxMapping.Entry entryByQualityAndType = Singleton<StateManager>.Instance.TemplateManager.MysteryBoxMapping.GetEntryByQualityAndType(_quality, _type);
			_mapper.Add("mysterybox_chain", entryByQualityAndType.chainLength);
			ItemQualityDescription value = null;
			UINGUIFormattedText formattedColoredText = UIItemUtil.GetFormattedColoredText(_quality);
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions.TryGetValue((int)_quality, out value))
			{
				_mapper.Add("mysterybox_quality", formattedColoredText);
			}
			formattedColoredText = UIItemUtil.GetFormattedColoredText(entryByQualityAndType.bonusQuality);
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions.TryGetValue((int)entryByQualityAndType.bonusQuality, out value))
			{
				_mapper.Add("mysterybox_bonus_quality", formattedColoredText);
			}
		}

		public static int GetInventorySpaceRequiredForClaim(this MysteryBoxChain _chain)
		{
			if (_chain == null)
			{
				return 0;
			}
			int num = ((_chain.mysteryBoxType != EMysteryBoxType.MysteryBoxRessourceType) ? 1 : 0);
			int num2 = _chain.slots.Length;
			int chainLength = _chain.Info().chainLength;
			if (num2 == chainLength - 1)
			{
				num++;
			}
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			int amountOfBoxesByTypeAndQuality = profile.Inventory.GetAmountOfBoxesByTypeAndQuality(_chain.mysteryBoxType, _chain.quality);
			if (amountOfBoxesByTypeAndQuality == 1)
			{
				num--;
			}
			return num;
		}

		public static int GetInventorySpaceRequiredForClaim(this EMysteryBoxType _mysteryBoxType, Quality.EQuality _quality)
		{
			int num = ((_mysteryBoxType != EMysteryBoxType.MysteryBoxRessourceType) ? 1 : 0);
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			int amountOfBoxesByTypeAndQuality = profile.Inventory.GetAmountOfBoxesByTypeAndQuality(_mysteryBoxType, _quality);
			if (amountOfBoxesByTypeAndQuality == 1)
			{
				num--;
			}
			return num;
		}
	}
}
