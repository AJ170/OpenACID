using System;
using System.Collections.Generic;
using Assets.Scripts.Engine;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class WrappingBuffGenerator : IBuffGenerator
	{
		private readonly BBList<IBuffGenerator> m_BuffGenerators;

		private static readonly BBList<IBuff> s_BuffList = new BBList<IBuff>();

		public Func<IEntity, IBuff, bool> IsActivePredicate { get; set; }

		public WrappingBuffGenerator(IEnumerable<IBuffGenerator> _buffGenerators)
		{
			m_BuffGenerators = new BBList<IBuffGenerator>(_buffGenerators);
		}

		public void MakeBuffs(BBList<IBuff> _result, IEntity _creator, IEntity _targetEntity, bool _doInversion)
		{
			this.ForceList(ref _result);
			_result.ClearIfNotEmpty();
			for (int i = 0; i < m_BuffGenerators.Count; i++)
			{
				m_BuffGenerators[i].IsActivePredicate = IsActivePredicate;
				m_BuffGenerators[i].MakeBuffs(s_BuffList, _creator, _targetEntity, _doInversion);
				_result.AddRange(s_BuffList);
			}
		}

		public void MakeBuffs(BBList<IBuff> _result, IEntity _creator, IEntity _targetEntity)
		{
			MakeBuffs(_result, _creator, _targetEntity, true);
		}
	}
}
