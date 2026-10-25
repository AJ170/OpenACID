using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "DeleteMailsRequest")]
	public class DeleteMailsRequest : IExtensible
	{
		private string[] _mailIds = new string[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "mailIds", DataFormat = DataFormat.Default)]
		public string[] mailIds
		{
			get
			{
				return _mailIds;
			}
			set
			{
				Array.Resize(ref _mailIds, value.Length);
				value.CopyTo(_mailIds, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
