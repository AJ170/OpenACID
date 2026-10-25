using Assets.Scripts.Engine;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class CachedBuffsCollection : BuffsCollection
	{
		private readonly BBDictionary<int, BBList<IBuff>> m_BuffCache = new BBDictionary<int, BBList<IBuff>>();

		public CachedBuffsCollection(BuffEventHandler _buffEventHandler)
			: base(_buffEventHandler)
		{
		}

		protected override void AddBuffInternal(IBuff _buff)
		{
			base.AddBuffInternal(_buff);
			BBList<IBuff> value;
			if (!m_BuffCache.TryGetValue((int)_buff.StatType, out value))
			{
				value = (m_BuffCache[(int)_buff.StatType] = new BBList<IBuff>());
			}
			value.Add(_buff);
		}

		protected override void RemoveBuffAtInternal(int _idx)
		{
			IBuff buffAt = GetBuffAt(_idx);
			BBList<IBuff> bBList = m_BuffCache[(int)buffAt.StatType];
			bBList.Remove(buffAt);
			base.RemoveBuffAtInternal(_idx);
		}

		protected override bool RemoveBuffInternal(IBuff _buff)
		{
			BBList<IBuff> value;
			if (!m_BuffCache.TryGetValue((int)_buff.StatType, out value))
			{
				return false;
			}
			if (!value.Remove(_buff))
			{
				return false;
			}
			return base.RemoveBuffInternal(_buff);
		}

		public override bool HasBuffs(StatTypes.EStatType _statType)
		{
			BBList<IBuff> value;
			return m_BuffCache.TryGetValue((int)_statType, out value) && value.Count > 0;
		}

		public override bool GetBuffs(StatTypes.EStatType _statType, BBList<IBuff> _result)
		{
			if (_result != null)
			{
				_result.ClearIfNotEmpty();
			}
			BBList<IBuff> value;
			if (m_BuffCache.TryGetValue((int)_statType, out value))
			{
				if (_result != null && value.Count > 0)
				{
					_result.AddRange(value);
				}
				return true;
			}
			return false;
		}

		public override void Clear()
		{
			m_BuffCache.Clear();
			ClearInternal(false);
		}
	}
}
