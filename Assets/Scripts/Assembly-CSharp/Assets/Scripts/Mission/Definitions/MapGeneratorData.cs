using AcierProtocol;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Definitions
{
	public static class MapGeneratorData
	{
		public interface IMapGeneratorEntityDefinition
		{
			EEra Age { get; }

			int MapWeight { get; }

			int UnlockLevel { get; }
		}

		internal interface IMapGeneratorEntityDefinitionAccess : IMapGeneratorEntityDefinition
		{
			new EEra Age { get; set; }

			new int MapWeight { get; set; }

			new int UnlockLevel { get; set; }
		}

		internal static void FillFromStats(IMapGeneratorEntityDefinitionAccess _definition, StatValueCollection _stats)
		{
			_definition.Age = (EEra)_stats.Get_int(StatTypes.EStatType.MapGenerator_Age);
			_definition.MapWeight = _stats.Get_int(StatTypes.EStatType.MapGenerator_MapWeight);
			_definition.UnlockLevel = _stats.Get_int(StatTypes.EStatType.MapGenerator_UnlockLevel);
		}
	}
}
