using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ReachNewAchievementsRequest")]
	public class ReachNewAchievementsRequest : IExtensible
	{
		private string[] _ids = new string[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "ids", DataFormat = DataFormat.Default)]
		public string[] ids
		{
			get
			{
				return _ids;
			}
			set
			{
				Array.Resize(ref _ids, value.Length);
				value.CopyTo(_ids, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
