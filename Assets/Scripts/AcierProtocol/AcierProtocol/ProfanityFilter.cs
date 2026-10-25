using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ProfanityFilter")]
	public class ProfanityFilter : IExtensible
	{
		private string[] _profanityList = new string[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "profanityList", DataFormat = DataFormat.Default)]
		public string[] profanityList
		{
			get
			{
				return _profanityList;
			}
			set
			{
				Array.Resize(ref _profanityList, value.Length);
				value.CopyTo(_profanityList, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
