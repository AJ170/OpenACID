using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveKillEnemyTypeContent")]
	public class ObjectiveKillEnemyTypeContent : IExtensible
	{
		private ObjectiveBaseContent _ObjectiveBaseContent;

		private bool _ForbidToKill;

		private string _TypeGuid = "";

		private int _KillAmount;

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "ObjectiveBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveBaseContent ObjectiveBaseContent
		{
			get
			{
				return _ObjectiveBaseContent;
			}
			set
			{
				_ObjectiveBaseContent = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "ForbidToKill", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool ForbidToKill
		{
			get
			{
				return _ForbidToKill;
			}
			set
			{
				_ForbidToKill = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "TypeGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string TypeGuid
		{
			get
			{
				return _TypeGuid;
			}
			set
			{
				_TypeGuid = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "KillAmount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int KillAmount
		{
			get
			{
				return _KillAmount;
			}
			set
			{
				_KillAmount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
