using System.Text;
using AcierProtocol;
using AssetBundleData;
using Database;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Database
{
	[Database("TranslationsDefinition")]
	public class TranslationsDef : DBEntry
	{
		public ELanguages Language { get; set; }

		public int LocaID { get; set; }

		public UnityResource TranslationResource { get; set; }

		public TranslationsDef(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			TranslationResource = new UnityResource(_assetBundleInfo);
		}

		public override void Serialize(DataSerializerBase _s)
		{
			if (_s.Mode == EMode.EMode_Write)
			{
				base.Name = Language.ToString();
			}
			base.Serialize(_s);
			TranslationResource.Serialize("TranslationResource", _s, base.AssetBundleInfo);
			Language = (ELanguages)_s.SerializeEnum("Language", Language.GetType(), (int)Language);
			LocaID = _s.SerializeInt("LocaID", LocaID);
		}

		public new string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("TranslationsDef \n");
			stringBuilder.AppendFormat("Language: {0} \n", Language);
			stringBuilder.AppendFormat("LocaID: {0} \n", LocaID);
			stringBuilder.AppendFormat("TranslationResource: {0} \n", TranslationResource.GetResourcePath());
			return stringBuilder.ToString();
		}

		public override void ClearCache()
		{
			TranslationResource.ClearCache();
		}
	}
}
