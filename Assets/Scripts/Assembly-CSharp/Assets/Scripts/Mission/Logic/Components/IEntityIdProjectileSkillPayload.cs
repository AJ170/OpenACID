using System;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IEntityIdProjectileSkillPayload : ISkillPayload, IEntityIdSkillPayload, IEquatable<ISkillPayload>
	{
		Projectile Projectile { get; }
	}
}
