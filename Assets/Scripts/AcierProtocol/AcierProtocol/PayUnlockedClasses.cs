using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "PayUnlockedClasses")]
	public class PayUnlockedClasses : IExtensible
	{
		private Assassin.EClassTypeId[] _classes = new Assassin.EClassTypeId[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "classes", DataFormat = DataFormat.TwosComplement)]
		public Assassin.EClassTypeId[] classes
		{
			get
			{
				return _classes;
			}
			set
			{
				Array.Resize(ref _classes, value.Length);
				value.CopyTo(_classes, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
