using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveTargetContent")]
	public class ObjectiveTargetContent : IExtensible
	{
		private ObjectiveBaseContent _ObjectiveBaseContent;

		private long[] _Targets = new long[0];

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

		[ProtoMember(9, Name = "Targets", DataFormat = DataFormat.TwosComplement)]
		public long[] Targets
		{
			get
			{
				return _Targets;
			}
			set
			{
				Array.Resize(ref _Targets, value.Length);
				value.CopyTo(_Targets, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
