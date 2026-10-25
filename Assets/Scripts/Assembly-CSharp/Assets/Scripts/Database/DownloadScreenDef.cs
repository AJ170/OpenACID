using AssetBundleData;
using Database;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Database
{
	[Database("DownloadScreenDefinition")]
	public class DownloadScreenDef : DBEntry
	{
		public enum EColorType
		{
			White = 0,
			Black = 1
		}

		public enum EAlignment
		{
			Top = 0,
			Center = 1,
			Bottom = 2
		}

		public UnityResource BackgroundScreen { get; set; }

		public EColorType LogoType { get; set; }

		public EColorType PrivacyTextType { get; set; }

		public EAlignment Alignment { get; set; }

		public DownloadScreenDef(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			BackgroundScreen = new UnityResource(_assetBundleInfo);
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			BackgroundScreen.Serialize("BackgroundScreen", _s, base.AssetBundleInfo);
			LogoType = (EColorType)_s.SerializeEnum("LogoType", LogoType.GetType(), (int)LogoType);
			PrivacyTextType = (EColorType)_s.SerializeEnum("PrivacyTextType", PrivacyTextType.GetType(), (int)PrivacyTextType);
			Alignment = (EAlignment)_s.SerializeEnum("Alignment", Alignment.GetType(), (int)Alignment);
		}

		public override void ClearCache()
		{
			BackgroundScreen.ClearCache();
		}
	}
}
