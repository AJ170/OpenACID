using System.Collections.ObjectModel;

namespace Assets.Scripts.Mission.Utilities
{
	public interface IEnumInfos
	{
		ReadOnlyCollection<string> Values { get; }

		int Count { get; }
	}
}
