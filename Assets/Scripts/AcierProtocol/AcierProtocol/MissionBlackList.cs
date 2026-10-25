using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionBlackList")]
	public class MissionBlackList : IExtensible
	{
		private string[] _missions = new string[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "missions", DataFormat = DataFormat.Default)]
		public string[] missions
		{
			get
			{
				return _missions;
			}
			set
			{
				Array.Resize(ref _missions, value.Length);
				value.CopyTo(_missions, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
