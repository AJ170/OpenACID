using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionPreventMovementContent")]
	public class MissionPreventMovementContent : IExtensible
	{
		private MissionBaseContent _MissionBaseContent;

		private PreventMovementExceptionTypes _PreventMovementException;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "MissionBaseContent", DataFormat = DataFormat.Default)]
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

		[ProtoMember(2, IsRequired = false, Name = "PreventMovementException", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(PreventMovementExceptionTypes.PreventMovementException_Invalid)]
		public PreventMovementExceptionTypes PreventMovementException
		{
			get
			{
				return _PreventMovementException;
			}
			set
			{
				_PreventMovementException = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
