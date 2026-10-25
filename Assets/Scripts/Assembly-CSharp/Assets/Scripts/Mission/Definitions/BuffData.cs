using System;
using AssetBundleData;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Database;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Definitions
{
	public class BuffData : UbiCore.Utils.Singleton<BuffData>
	{
		public interface IBuffDefinition : Data.IDefinition
		{
			EBuffType Type { get; }

			StatTypes.EStatType StatType { get; }

			string[] StatusEffectNames { get; }

			string[] StartEffectNames { get; }

			string[] EndEffectNames { get; }

			string DisplayName { get; }

			int IconSpriteIndex { get; }

			bool TriggerCharacterConditionChange { get; }

			bool CancelOnEnterCombat { get; }

			bool CancelOnDeath { get; }
		}

		[Database("BuffDefinition")]
		public class BuffDefinition : Data.Definition, IBuffDefinition, Data.IDefinition
		{
			public override Type InstanceType
			{
				get
				{
					return null;
				}
			}

			public EBuffType Type { get; protected set; }

			public StatTypes.EStatType StatType { get; protected set; }

			public string[] StatusEffectNames { get; protected set; }

			public string[] StartEffectNames { get; protected set; }

			public string[] EndEffectNames { get; protected set; }

			public string DisplayName { get; protected set; }

			public int IconSpriteIndex { get; protected set; }

			public bool TriggerCharacterConditionChange { get; protected set; }

			public bool CancelOnEnterCombat { get; protected set; }

			public bool CancelOnDeath { get; protected set; }

			public BuffDefinition(AssetBundleInfo _assetBundleInfo)
				: base(_assetBundleInfo, true)
			{
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
				StatType = (StatTypes.EStatType)_s.SerializeEnum("StatType", StatType.GetType(), (int)StatType);
				StatusEffectNames = _s.SerializeArrayGeneric("StatusEffectNames", StatusEffectNames);
				StartEffectNames = _s.SerializeArrayGeneric("StartEffectNames", StartEffectNames);
				EndEffectNames = _s.SerializeArrayGeneric("EndEffectNames", EndEffectNames);
				DisplayName = _s.SerializeString("DisplayName", DisplayName);
				IconSpriteIndex = _s.SerializeInt("IconSpriteIndex", IconSpriteIndex);
				TriggerCharacterConditionChange = _s.SerializeBool("TriggerCharacterConditionChange", TriggerCharacterConditionChange);
				CancelOnEnterCombat = _s.SerializeBool("CancelOnEnterCombat", CancelOnEnterCombat);
				CancelOnDeath = _s.SerializeBool("CancelOnDeath", CancelOnDeath);
				if (_s.Mode == EMode.EMode_Read)
				{
					PostConstructor();
				}
			}

			protected void PostConstructor()
			{
				Type = (EBuffType)(int)Enum.Parse(typeof(EBuffType), base.Name);
				if (UbiCore.Utils.Singleton<BuffData>.IsAvailable)
				{
					UbiCore.Utils.Singleton<BuffData>.Instance.AddDefinition(this);
				}
			}
		}

		private readonly BuffDefinition[] m_Definitions = new BuffDefinition[41];

		public IBuffDefinition GetDefinition(EBuffType _type)
		{
			if (_type == EBuffType.Invalid)
			{
				return null;
			}
			return m_Definitions[(int)_type];
		}

		private void AddDefinition(BuffDefinition _definition)
		{
			if (UbiCore.Utils.Singleton<DefinitionManager>.Instance.Status == DefinitionManager.EStatus.Fresh)
			{
				m_Definitions[(int)_definition.Type] = _definition;
			}
		}
	}
}
