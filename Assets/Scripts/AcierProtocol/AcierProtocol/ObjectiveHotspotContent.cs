using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveHotspotContent")]
	public class ObjectiveHotspotContent : IExtensible
	{
		private ObjectiveBaseContent _ObjectiveBaseContent;

		private int[] _TargetHotspots = new int[0];

		private bool _Invert;

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

		[ProtoMember(9, Name = "TargetHotspots", DataFormat = DataFormat.TwosComplement)]
		public int[] TargetHotspots
		{
			get
			{
				return _TargetHotspots;
			}
			set
			{
				Array.Resize(ref _TargetHotspots, value.Length);
				value.CopyTo(_TargetHotspots, 0);
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "Invert", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool Invert
		{
			get
			{
				return _Invert;
			}
			set
			{
				_Invert = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
