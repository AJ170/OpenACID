using System;

namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IEntityIdSkillPayload : ISkillPayload, IEquatable<ISkillPayload>
	{
		EntityId Id { get; }
	}
}
