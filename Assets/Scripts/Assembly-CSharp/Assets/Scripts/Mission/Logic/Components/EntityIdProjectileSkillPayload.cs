using System;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class EntityIdProjectileSkillPayload : EntityIdSkillPayload, ISkillPayload, IEntityIdSkillPayload, IEntityIdProjectileSkillPayload, IEquatable<ISkillPayload>
	{
		public Projectile Projectile { get; private set; }

		public EntityIdProjectileSkillPayload(EntityId _id, Projectile _projectile)
			: base(_id)
		{
			Projectile = _projectile;
		}

		public EntityIdProjectileSkillPayload(EntityIdSkillPayload _sourcePayload, Projectile _projectile)
			: base(_sourcePayload.Id)
		{
			Projectile = _projectile;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode() + Projectile.GetHashCode();
		}

		public override bool Equals(ISkillPayload _other)
		{
			EntityIdProjectileSkillPayload entityIdProjectileSkillPayload = _other as EntityIdProjectileSkillPayload;
			return entityIdProjectileSkillPayload != null && Id == entityIdProjectileSkillPayload.Id && Projectile == entityIdProjectileSkillPayload.Projectile;
		}
	}
}
