using System;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IBuffGenerator
	{
		Func<IEntity, IBuff, bool> IsActivePredicate { get; set; }

		void MakeBuffs(BBList<IBuff> _result, IEntity _creator, IEntity _targetEntity, bool _doInversion);

		void MakeBuffs(BBList<IBuff> _result, IEntity _creator, IEntity _targetEntity);
	}
}
