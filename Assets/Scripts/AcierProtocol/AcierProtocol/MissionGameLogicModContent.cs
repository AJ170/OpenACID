using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionGameLogicModContent")]
	public class MissionGameLogicModContent : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "MissionCombatSettings")]
		public class MissionCombatSettings : IExtensible
		{
			private EBool _KillTokenKill = EBool.Null;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "KillTokenKill", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EBool.Null)]
			public EBool KillTokenKill
			{
				get
				{
					return _KillTokenKill;
				}
				set
				{
					_KillTokenKill = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "MissionHealthSettings")]
		public class MissionHealthSettings : IExtensible
		{
			private EBool _SetHealthPercentage = EBool.Null;

			private int _Percentage;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "SetHealthPercentage", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EBool.Null)]
			public EBool SetHealthPercentage
			{
				get
				{
					return _SetHealthPercentage;
				}
				set
				{
					_SetHealthPercentage = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "Percentage", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int Percentage
			{
				get
				{
					return _Percentage;
				}
				set
				{
					_Percentage = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[ProtoContract(Name = "EBool")]
		public enum EBool
		{
			[ProtoEnum(Name = "False", Value = 0)]
			False = 0,
			[ProtoEnum(Name = "True", Value = 1)]
			True = 1,
			[ProtoEnum(Name = "Null", Value = 2)]
			Null = 2
		}

		private MissionBaseContent _MissionBaseContent;

		private MissionCombatSettings _Combat;

		private MissionHealthSettings _Health;

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "MissionBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionBaseContent MissionBaseContent
		{
			get
			{
				return _MissionBaseContent;
			}
			set
			{
				_MissionBaseContent = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "Combat", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionCombatSettings Combat
		{
			get
			{
				return _Combat;
			}
			set
			{
				_Combat = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Health", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionHealthSettings Health
		{
			get
			{
				return _Health;
			}
			set
			{
				_Health = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
