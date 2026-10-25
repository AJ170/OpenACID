using System.Collections.ObjectModel;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Serialization;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Database
{
	public class CTADefinitionDefenseModule : CTADefinitionBase
	{
		public float HealthThreshold { get; set; }

		public int MissionsCount { get; set; }

		public int MissionsTotal { get; set; }

		public CTADefinitionDefenseModule(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			HealthThreshold = _s.SerializeFloat("HealthThreshold", HealthThreshold);
			MissionsCount = _s.SerializeInt("MissionsCount", MissionsCount);
			MissionsTotal = _s.SerializeInt("MissionsTotal", MissionsTotal);
		}

		protected override EResult CheckTrigger()
		{
			ReadOnlyCollection<LocalStorage.ICTAMissionData> readOnlyCollection = UbiCore.Utils.Singleton<LocalStorage>.Instance.CTAMissionDataGet();
			int num = 0;
			for (int i = Mathf.Max(readOnlyCollection.Count - MissionsTotal, 0); i < readOnlyCollection.Count; i++)
			{
				if (readOnlyCollection[i].LowHealth)
				{
					num++;
				}
			}
			if (num < MissionsCount)
			{
				return EResult.Proceed;
			}
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			if (currentAssassin == null)
			{
				return EResult.Proceed;
			}
			InventoryElement elementForSlot = currentAssassin.GetElementForSlot(EInventorySlot.SlotModule);
			if (elementForSlot != null)
			{
				return EResult.Proceed;
			}
			return EResult.Fullfilled;
		}
	}
}
