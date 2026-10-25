using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "HirelingAssassins")]
	public class HirelingAssassins : IExtensible
	{
		private HirelingAssassin[] _hirelingAssassins = new HirelingAssassin[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "hirelingAssassins", DataFormat = DataFormat.Default)]
		public HirelingAssassin[] hirelingAssassins
		{
			get
			{
				return _hirelingAssassins;
			}
			set
			{
				Array.Resize(ref _hirelingAssassins, value.Length);
				value.CopyTo(_hirelingAssassins, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
