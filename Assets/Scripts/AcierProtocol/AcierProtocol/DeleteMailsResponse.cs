using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "DeleteMailsResponse")]
	public class DeleteMailsResponse : IExtensible
	{
		private string[] _failedIds = new string[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "failedIds", DataFormat = DataFormat.Default)]
		public string[] failedIds
		{
			get
			{
				return _failedIds;
			}
			set
			{
				Array.Resize(ref _failedIds, value.Length);
				value.CopyTo(_failedIds, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
