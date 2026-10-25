using System;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IStatValueBase : IReadonlyStatValueBase
	{
		bool IsWritingAllowed { get; }

		Type EnumValueType { get; }

		void ApplyBuff(IBuff _buff, IEntity _entity, float _payload);

		void CopyFrom(IReadonlyStatValueBase _other);
	}
}
