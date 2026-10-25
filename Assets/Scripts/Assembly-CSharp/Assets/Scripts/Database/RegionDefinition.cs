using System.Collections.Generic;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Database
{
	[Database("RegionDefinition")]
	public class RegionDefinition : DBEntry
	{
		public class AppearanceOption : IDataSerializable
		{
			public string Name { get; set; }

			public LocaKey NameLocaKey { get; set; }

			public LocaKey NameShortLocaKey { get; set; }

			public LocaKey DateLocaKey { get; set; }

			public LocaKey EarthPinLocaKey { get; set; }

			public UnityResource Banner { get; private set; }

			public UnityResource Map { get; private set; }

			public UnityResource EarthPinImg { get; private set; }

			public AppearanceOption(AssetBundleInfo _assetBundleInfo)
			{
				NameLocaKey = new LocaKey();
				NameShortLocaKey = new LocaKey();
				DateLocaKey = new LocaKey();
				EarthPinLocaKey = new LocaKey();
				Banner = new UnityResource(_assetBundleInfo);
				Map = new UnityResource(_assetBundleInfo);
				EarthPinImg = new UnityResource(_assetBundleInfo);
			}

			public void ClearCache()
			{
				Banner.ClearCache();
				Map.ClearCache();
				EarthPinImg.ClearCache();
			}

			public void Serialize(string _name, DataSerializerBase _s, object _customData = null)
			{
				AssetBundleInfo customData = (AssetBundleInfo)_customData;
				Name = _s.SerializeString("Name", Name);
				NameLocaKey.Key = _s.SerializeInt("NameLocaKey", NameLocaKey.Key);
				NameShortLocaKey.Key = _s.SerializeInt("NameShortLocaKey", NameShortLocaKey.Key);
				DateLocaKey.Key = _s.SerializeInt("DateLocaKey", DateLocaKey.Key);
				EarthPinLocaKey.Key = _s.SerializeInt("EarthPinLocaKey", EarthPinLocaKey.Key);
				Banner.Serialize("Banner", _s, customData);
				Map.Serialize("Map", _s, customData);
				EarthPinImg.Serialize("EarthPinImage", _s, customData);
			}
		}

		private CityExtensions.CityIdentifier m_cityIdentifier;

		public LocaKey NameLocaKey { get; set; }

		public LocaKey NameShortLocaKey { get; set; }

		public LocaKey DateLocaKey { get; set; }

		public LocaKey EarthPinLocaKey { get; set; }

		public UnityResource Banner { get; set; }

		public UnityResource Map { get; set; }

		public UnityResource EarthPinImg { get; set; }

		public UnityResource CharacterLighting { get; set; }

		public List<AppearanceOption> AppearanceOptions { get; private set; }

		public int PlaceInList { get; set; }

		public int GeneratedYearMin { get; set; }

		public int GeneratedYearMax { get; set; }

		public bool Locked { get; set; }

		public bool SelectedOnStartup { get; set; }

		public bool Hidden { get; set; }

		public CityExtensions.CityIdentifier CityIdentifier
		{
			get
			{
				return m_cityIdentifier;
			}
			set
			{
				m_cityIdentifier = value;
			}
		}

		public bool Globe { get; set; }

		public ERegion Region { get; set; }

		public EMood Mood { get; set; }

		public RegionDefinition(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			Banner = new UnityResource(_assetBundleInfo);
			Map = new UnityResource(_assetBundleInfo);
			EarthPinImg = new UnityResource(_assetBundleInfo);
			CharacterLighting = new UnityResource(_assetBundleInfo);
			NameLocaKey = new LocaKey();
			NameShortLocaKey = new LocaKey();
			DateLocaKey = new LocaKey();
			EarthPinLocaKey = new LocaKey();
			PlaceInList = 0;
			AppearanceOptions = new List<AppearanceOption>();
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			NameLocaKey.Key = _s.SerializeInt("NameLocaKey", NameLocaKey.Key);
			NameShortLocaKey.Key = _s.SerializeInt("NameShortLocaKey", NameShortLocaKey.Key);
			DateLocaKey.Key = _s.SerializeInt("DateLocaKey", DateLocaKey.Key);
			EarthPinLocaKey.Key = _s.SerializeInt("EarthPinLocaKey", EarthPinLocaKey.Key);
			Banner.Serialize("Banner", _s, base.AssetBundleInfo);
			Map.Serialize("Map", _s, base.AssetBundleInfo);
			EarthPinImg.Serialize("EarthPinImage", _s, base.AssetBundleInfo);
			PlaceInList = _s.SerializeInt("PlaceInList", PlaceInList);
			Locked = _s.SerializeBool("Locked", Locked);
			Hidden = _s.SerializeBool("Hidden", Hidden);
			SelectedOnStartup = _s.SerializeBool("SelectedOnStartup", SelectedOnStartup);
			CityIdentifier = (CityExtensions.CityIdentifier)_s.SerializeEnum("CityIdentifier", CityIdentifier.GetType(), (int)CityIdentifier);
			Globe = _s.SerializeBool("Globe", Globe);
			Region = (ERegion)_s.SerializeEnum("Region", Region.GetType(), (int)Region);
			Mood = (EMood)_s.SerializeEnum("Mood", Mood.GetType(), (int)Mood);
			GeneratedYearMin = _s.SerializeInt("GeneratedYearMin", GeneratedYearMin);
			GeneratedYearMax = _s.SerializeInt("GeneratedYearMax", GeneratedYearMax);
			_s.SerializeBeginNode("AppearanceOptions");
			int num = _s.SerializeItemCount(AppearanceOptions.Count);
			if (_s.Mode == EMode.EMode_Read)
			{
				AppearanceOptions.Clear();
				for (int i = 0; i < num; i++)
				{
					_s.SerializeBeginNode("Value");
					AppearanceOption appearanceOption = new AppearanceOption(base.AssetBundleInfo);
					AppearanceOptions.Add(appearanceOption);
					appearanceOption.Serialize("AppearanceOption", _s, base.AssetBundleInfo);
					_s.SerializeEndNode();
				}
			}
			else
			{
				for (int j = 0; j < num; j++)
				{
					_s.SerializeBeginNode("Value");
					AppearanceOptions[j].Serialize("AppearanceOption", _s, base.AssetBundleInfo);
					_s.SerializeEndNode();
				}
			}
			_s.SerializeEndNode();
			if (_s.Mode == EMode.EMode_Read)
			{
				GeneratedYearMin = ((GeneratedYearMin <= 0) ? 1000 : GeneratedYearMin);
				GeneratedYearMax = ((GeneratedYearMax <= GeneratedYearMin) ? 1050 : GeneratedYearMax);
			}
			CharacterLighting.Serialize("CharacterLighting", _s, base.AssetBundleInfo);
		}

		public override void ClearCache()
		{
			Banner.ClearCache();
			Map.ClearCache();
			EarthPinImg.ClearCache();
			CharacterLighting.ClearCache();
			for (int i = 0; i < AppearanceOptions.Count; i++)
			{
				AppearanceOptions[i].ClearCache();
			}
		}
	}
}
