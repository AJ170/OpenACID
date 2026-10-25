using System.Collections.Generic;

namespace Assets.Scripts.Mission.Mission.Interfaces
{
	public interface IObjectiveTarget : IObjectiveValue
	{
		List<IMissionEntity> Targets { get; }
	}
}
