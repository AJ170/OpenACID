namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueWithOwner<TValue> : StatValue<TValue>, IStatValueCollectionCarrier, IReadonlyStatValueBase, IStatValueBase, IScalableStat where TValue : class, IStatValueWrapper, new()
	{
		private readonly StatValueCollection m_StatValueCollection;

		public override StatValueCollection StatValueCollection
		{
			get
			{
				return m_StatValueCollection;
			}
		}

		public float LevelIncrement
		{
			get
			{
				IScalableNumberStatValueWrapper scalableNumberStatValueWrapper = m_ValueWrapper as IScalableNumberStatValueWrapper;
				return (scalableNumberStatValueWrapper != null) ? scalableNumberStatValueWrapper.LevelIncrement : 0f;
			}
			set
			{
				IScalableNumberStatValueWrapper scalableNumberStatValueWrapper = (IScalableNumberStatValueWrapper)m_ValueWrapper;
				scalableNumberStatValueWrapper.LevelIncrement = value;
			}
		}

		public override int Value_int
		{
			get
			{
				return (!IsScaled) ? base.Value_int : ((int)((float)base.Value_int + StatValueCollection.IncrementFactor * LevelIncrement));
			}
			set
			{
				base.Value_int = value;
			}
		}

		public override float Value_float
		{
			get
			{
				return (!IsScaled) ? base.Value_float : (base.Value_float + StatValueCollection.IncrementFactor * LevelIncrement);
			}
			set
			{
				base.Value_float = value;
			}
		}

		public override float Value_number
		{
			get
			{
				float num = ((!IsScaled) ? base.Value_number : (base.Value_number + StatValueCollection.IncrementFactor * LevelIncrement));
				if (m_ValueWrapper is StatValueWrapperScalable_Int)
				{
					num = (int)num;
				}
				return num;
			}
			set
			{
				base.Value_number = value;
			}
		}

		public override bool HasDefaultValue
		{
			get
			{
				return base.HasDefaultValue && LevelIncrement == 0f;
			}
		}

		public StatValueWithOwner(StatTypes.EStatType _statType, StatValueCollection _statValueCollection)
			: base(_statType, _statValueCollection)
		{
			m_StatValueCollection = _statValueCollection;
		}

		public override void CopyFrom(IReadonlyStatValueBase _other)
		{
			IScalableStat scalableStat = _other as IScalableStat;
			if (scalableStat != null && !IsScaled && _other.IsScaled)
			{
				TValue val = StatValue<TValue>.s_TmpNewValueWrapper;
				val.Number = ((IReadonlyStatValue)scalableStat).Value_number;
			}
			else
			{
				IStatValueWrapperContainer statValueWrapperContainer = (IStatValueWrapperContainer)_other;
				TValue val2 = StatValue<TValue>.s_TmpNewValueWrapper;
				val2.CopyFrom(statValueWrapperContainer.ValueWrapper);
			}
			SetValue();
		}

		public override IReadonlyStatValueBase Clone(StatValueCollection _newStatValueCollection)
		{
			StatValueWithOwner<TValue> statValueWithOwner = new StatValueWithOwner<TValue>(StatType, _newStatValueCollection);
			statValueWithOwner.CopyFrom(this);
			return statValueWithOwner;
		}
	}
}
