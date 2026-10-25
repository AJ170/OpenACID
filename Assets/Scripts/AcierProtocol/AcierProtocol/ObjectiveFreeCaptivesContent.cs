using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveFreeCaptivesContent")]
	public class ObjectiveFreeCaptivesContent : IExtensible
	{
		private ObjectiveBaseContent _ObjectiveBaseContent;

		private int _MaximumCasualties;

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

		[ProtoMember(1, IsRequired = false, Name = "MaximumCasualties", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int MaximumCasualties
		{
			get
			{
				return _MaximumCasualties;
			}
			set
			{
				_MaximumCasualties = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
