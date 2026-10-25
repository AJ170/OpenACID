using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MissionScreen.EarthPins
{
	public class EarthPinManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_earthPinPrefab;

		private GameObjectCache m_objectCache;

		private readonly Dictionary<string, EarthPinBase> m_usedCityPins = new Dictionary<string, EarthPinBase>();

		private readonly List<string> m_citiesToBeRemoved = new List<string>();

		public EEra CurrentSelectedEra { get; private set; }

		public bool IsPinsCreated
		{
			get
			{
				return m_usedCityPins.Count > 0;
			}
		}

		public void OnInit()
		{
			m_objectCache = new GameObjectCache(m_earthPinPrefab, base.transform);
			m_objectCache.Reserve(10);
		}

		public GameObject GetPin()
		{
			return m_objectCache.Dequeue().Item;
		}

		public void ReleaseObjects()
		{
			m_usedCityPins.Clear();
			m_objectCache.ReleaseAll();
		}

		public EarthPinBase GetUsedPinByEarthPinLocaKey(RegionDefinition _regiondef, string _regionAppearanceOption)
		{
			EarthPinBase value = null;
			string key = GeneratedEarthPinKey(_regiondef, _regionAppearanceOption);
			m_usedCityPins.TryGetValue(key, out value);
			return value;
		}

		public void CreateRegionPinsForCampaignMissions(MissionsAtLocation[] _missionsAtLocation, LocationSet _selectedLocationSet, MissionInfo _currentMission, bool _autoSelectPin = true)
		{
			RegionDefinition regionDefinition = _selectedLocationSet.GetRegionDefinition();
			LocaKey regionEarthPinLoca = regionDefinition.GetRegionEarthPinLoca(string.Empty);
			SetCurrentSelectedEra(_selectedLocationSet.era);
			foreach (MissionsAtLocation missionsAtLocation in _missionsAtLocation)
			{
				if (missionsAtLocation.location.era == _selectedLocationSet.era)
				{
					RegionDefinition regionDefinition2 = missionsAtLocation.location.GetRegionDefinition();
					LocaKey regionEarthPinLoca2 = regionDefinition2.GetRegionEarthPinLoca(string.Empty);
					bool flag = false;
					for (int j = 0; j < regionDefinition2.AppearanceOptions.Count; j++)
					{
						string text = regionDefinition2.AppearanceOptions[j].Name;
						flag = _autoSelectPin && regionEarthPinLoca2.Key == regionEarthPinLoca.Key && _currentMission.regionAppearanceOption == text;
						RegisterPinForRegionDef(regionDefinition2, text, flag);
					}
					flag = _autoSelectPin && regionEarthPinLoca2.Key == regionEarthPinLoca.Key && string.IsNullOrEmpty(_currentMission.regionAppearanceOption);
					RegisterPinForRegionDef(regionDefinition2, string.Empty, flag);
				}
			}
			string key = GeneratedEarthPinKey(regionDefinition, _currentMission.regionAppearanceOption);
			EarthPinBase value;
			if (m_usedCityPins.TryGetValue(key, out value))
			{
				value.SetTargetRegion(regionDefinition, _currentMission.regionAppearanceOption);
			}
			RemoveUnUsedPins();
		}

		public void CreateRegionPinsForContractMissions(List<AcierProtocol.Mission> _missions, int _selectedIndex, bool _autoSelectPin = true)
		{
			if (_missions.Count > _selectedIndex)
			{
				AcierProtocol.Mission mission = _missions[_selectedIndex];
				SetCurrentSelectedEra(mission.locationSet.era);
				RegionDefinition regionDefinition = mission.GetRegionDefinition();
				LocaKey regionEarthPinLoca = regionDefinition.GetRegionEarthPinLoca(string.Empty);
				RegisterPinForRegionDef(regionDefinition, string.Empty, _autoSelectPin);
				for (int i = 0; i < _missions.Count; i++)
				{
					AcierProtocol.Mission mission2 = _missions[i];
					if (i != _selectedIndex && mission2.locationSet.era == mission.locationSet.era)
					{
						RegionDefinition regionDefinition2 = mission2.GetRegionDefinition();
						LocaKey regionEarthPinLoca2 = regionDefinition2.GetRegionEarthPinLoca(string.Empty);
						if (regionEarthPinLoca2.Key != regionEarthPinLoca.Key)
						{
							RegisterPinForRegionDef(mission2.GetRegionDefinition(), string.Empty, false);
						}
					}
				}
				string key = GeneratedEarthPinKey(regionDefinition, string.Empty);
				EarthPinBase value;
				if (m_usedCityPins.TryGetValue(key, out value))
				{
					value.SetTargetRegion(regionDefinition, string.Empty);
				}
				RemoveUnUsedPins();
			}
			else
			{
				ReleaseObjects();
			}
		}

		private void SetCurrentSelectedEra(EEra _eraLocation)
		{
			if (CurrentSelectedEra != _eraLocation)
			{
				ReleaseObjects();
			}
			else
			{
				m_citiesToBeRemoved.AddRange(m_usedCityPins.Keys);
			}
			CurrentSelectedEra = _eraLocation;
		}

		public void RemoveUnUsedPins()
		{
			for (int i = 0; i < m_citiesToBeRemoved.Count; i++)
			{
				string key = m_citiesToBeRemoved[i];
				EarthPinBase earthPinBase = m_usedCityPins[key];
				m_objectCache.Release(earthPinBase.gameObject);
				m_usedCityPins.Remove(key);
			}
			m_citiesToBeRemoved.Clear();
		}

		private string GeneratedEarthPinKey(RegionDefinition _region, string _appearanceOptionName)
		{
			LocaKey regionEarthPinLoca = _region.GetRegionEarthPinLoca(_appearanceOptionName);
			return regionEarthPinLoca.Key + _appearanceOptionName.ToLower();
		}

		private void RegisterPinForRegionDef(RegionDefinition _region, string _appearanceOptionName, bool _isSelected)
		{
			string text = GeneratedEarthPinKey(_region, _appearanceOptionName);
			if (m_citiesToBeRemoved != null)
			{
				for (int i = 0; i < m_citiesToBeRemoved.Count; i++)
				{
					string text2 = m_citiesToBeRemoved[i];
					if (text2 == text)
					{
						m_citiesToBeRemoved.RemoveAt(i);
						break;
					}
				}
			}
			EarthPinBase value = null;
			if (!m_usedCityPins.TryGetValue(text, out value))
			{
				GameObject pin = GetPin();
				value = pin.GetComponent<EarthPinBase>();
			}
			value.name = text;
			value.gameObject.SetActive(true);
			value.SetTargetRegion(_region, _appearanceOptionName);
			value.IsSelected = _isSelected;
			m_usedCityPins[text] = value;
		}

		private void OnDestroy()
		{
			m_citiesToBeRemoved.Clear();
			m_usedCityPins.Clear();
			m_objectCache.Destroy();
		}
	}
}
