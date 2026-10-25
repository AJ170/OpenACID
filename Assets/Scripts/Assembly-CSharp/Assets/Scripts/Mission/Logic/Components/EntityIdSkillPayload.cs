using System;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class EntityIdSkillPayload : ISkillPayload, IEntityIdSkillPayload, IEquatable<ISkillPayload>
	{
		public EntityId Id { get; private set; }

		public EntityIdSkillPayload(EntityId _id)
		{
			Id = _id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			ISkillPayload skillPayload = obj as ISkillPayload;
			if (skillPayload == null)
			{
				return false;
			}
			return Equals(skillPayload);
		}

		public virtual bool Equals(ISkillPayload _other)
		{
			EntityIdSkillPayload entityIdSkillPayload = _other as EntityIdSkillPayload;
			return entityIdSkillPayload != null && Id == entityIdSkillPayload.Id;
		}
	}
}
