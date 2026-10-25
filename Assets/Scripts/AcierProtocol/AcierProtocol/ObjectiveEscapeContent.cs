using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveEscapeContent")]
	public class ObjectiveEscapeContent : IExtensible
	{
		private ObjectiveTargetContent _ObjectiveTargetContent;

		private float _Distance;

		private bool _HideOutside;

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "ObjectiveTargetContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveTargetContent ObjectiveTargetContent
		{
			get
			{
				return _ObjectiveTargetContent;
			}
			set
			{
				_ObjectiveTargetContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "Distance", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float Distance
		{
			get
			{
				return _Distance;
			}
			set
			{
				_Distance = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "HideOutside", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool HideOutside
		{
			get
			{
				return _HideOutside;
			}
			set
			{
				_HideOutside = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
