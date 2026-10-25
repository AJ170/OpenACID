using AcierProtocol;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class ModuleQualityMappingExtensions
	{
		public static Quality.EQuality GetModuleDisplayItemQuality(this ModuleQualityMapping _mapping, Quality.EQuality _quality)
		{
			for (int i = 0; i < _mapping.entries.Length; i++)
			{
				ModuleQualityMapping.Entry entry = _mapping.entries[i];
				if (entry.moduleQuality == _quality)
				{
					return entry.displayQuality;
				}
			}
			return _quality;
		}
	}
}
