using System;
using System.Collections.Generic;
using SharedBaseLib.Serialization;

namespace Database
{
	public class EffectPackageElement : DataSerializableBase
	{
		public enum ETargetBone
		{
			Reference = 0,
			LeftHand = 1,
			RightHand = 2,
			Head = 3,
			Spine1 = 4,
			Spine2 = 5,
			RightHand_Weapon_Ref_Jnt = 6,
			Root = 7,
			LeftFoot = 8,
			LeftForeArm = 9,
			RightForeArm = 10
		}

		public enum ETargetCharType
		{
			Player = 0,
			Enemy = 1,
			Male = 2,
			Female = 3,
			Courtesan = 4,
			Doctor = 5,
			SpecialNPC = 6,
			World = 7
		}

		public static readonly string[] s_TargetBonesNames;

		public string Marker { get; set; }

		public ETargetBone TargetBone { get; set; }

		public UnityResource PreFab { get; set; }

		public UnityResource Parameter { get; set; }

		public ETargetCharType TargetCharType { get; set; }

		public bool ShowCondition { get; set; }

		public bool UseCondition { get; set; }

		public EffectManager.EConditionMode ConditionMode { get; set; }

		public List<EffectManager.Condition> Conditions { get; set; }

		public EffectPackageElement()
		{
			PreFab = new UnityResource();
			Parameter = new UnityResource();
			Conditions = new List<EffectManager.Condition>();
		}

		static EffectPackageElement()
		{
			Array values = Enum.GetValues(typeof(ETargetBone));
			s_TargetBonesNames = new string[values.Length];
			for (int i = 0; i < values.Length; i++)
			{
				ETargetBone eTargetBone = (ETargetBone)(int)values.GetValue(i);
				s_TargetBonesNames[i] = Enum.GetName(typeof(ETargetBone), eTargetBone);
			}
		}

		protected override void SerializeInternal(DataSerializerBase _s, object _customData = null)
		{
			Marker = _s.SerializeString("Marker", Marker);
			TargetBone = (ETargetBone)_s.SerializeEnum("TargetBone", TargetBone.GetType(), (int)TargetBone);
			PreFab.Serialize("PreFab", _s, _customData);
			Parameter.Serialize("Parameter", _s, _customData);
			TargetCharType = (ETargetCharType)_s.SerializeEnum("TargetCharType", TargetCharType.GetType(), (int)TargetCharType);
			ShowCondition = _s.SerializeBool("ShowCondition", ShowCondition);
			UseCondition = _s.SerializeBool("UseCondition", UseCondition);
			ConditionMode = (EffectManager.EConditionMode)_s.SerializeEnum("ConditionMode", ConditionMode.GetType(), (int)ConditionMode);
			Conditions = _s.SerializeList("Conditions", Conditions);
		}
	}
}
