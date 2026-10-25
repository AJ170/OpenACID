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
	public class CTADefinitionTooManyDeaths : CTADefinitionBase
	{
		public int DeathsWithoutDoctor { get; protected set; }

		public int TotalWithoutDoctor { get; protected set; }

		public int DeathsWithDoctor { get; protected set; }

		public int TotalWithDoctor { get; protected set; }

		public EModuleType[] ModuleTypes { get; protected set; }

		public CTADefinitionTooManyDeaths(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			ModuleTypes = new EModuleType[0];
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			DeathsWithoutDoctor = _s.SerializeInt("DeathsWithoutDoctor", DeathsWithoutDoctor);
			TotalWithoutDoctor = _s.SerializeInt("TotalWithoutDoctor", TotalWithoutDoctor);
			DeathsWithDoctor = _s.SerializeInt("DeathsWithDoctor", DeathsWithDoctor);
			TotalWithDoctor = _s.SerializeInt("TotalWithDoctor", TotalWithDoctor);
			ModuleTypes = (EModuleType[])_s.SerializeEnumArray("ModuleTypes", typeof(EModuleType), ModuleTypes);
		}

		protected override EResult CheckTrigger()
		{
			ReadOnlyCollection<LocalStorage.ICTAMissionData> missionData = UbiCore.Utils.Singleton<LocalStorage>.Instance.CTAMissionDataGet();
			int num = CountDeaths(missionData, TotalWithoutDoctor);
			int num2 = CountDeaths(missionData, TotalWithDoctor);
			int num3 = CountDoctor(missionData, TotalWithDoctor);
			if ((num < DeathsWithoutDoctor || num3 > 0) && num2 < DeathsWithDoctor)
			{
				return EResult.Proceed;
			}
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			if (currentAssassin == null)
			{
				return EResult.Proceed;
			}
			InventoryElement elementForSlot = currentAssassin.GetElementForSlot(EInventorySlot.SlotModule);
			if (elementForSlot == null)
			{
				return EResult.Fullfilled;
			}
			if (ModuleTypes.Contains(elementForSlot.moduleData.moduleType))
			{
				return EResult.Proceed;
			}
			return EResult.Fullfilled;
		}

		private static int CountDeaths(ReadOnlyCollection<LocalStorage.ICTAMissionData> _missionData, int _totalMissions)
		{
			if (_missionData.Count < _totalMissions)
			{
				return -1;
			}
			int num = 0;
			for (int i = Mathf.Max(_missionData.Count - _totalMissions, 0); i < _missionData.Count; i++)
			{
				num += _missionData[i].Died;
			}
			return num;
		}

		private static int CountDoctor(ReadOnlyCollection<LocalStorage.ICTAMissionData> _missionData, int _totalMissions)
		{
			if (_missionData.Count < _totalMissions)
			{
				return -1;
			}
			int num = 0;
			for (int i = Mathf.Max(_missionData.Count - _totalMissions, 0); i < _missionData.Count; i++)
			{
				if (_missionData[i].UsedDoctor)
				{
					num++;
				}
			}
			return num;
		}
	}
}
