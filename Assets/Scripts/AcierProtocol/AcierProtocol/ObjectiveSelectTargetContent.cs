using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveSelectTargetContent")]
	public class ObjectiveSelectTargetContent : IExtensible
	{
		private ObjectiveTargetContent _ObjectiveTargetContent;

		private bool _ForceSelection;

		private bool _FailOnUnselect;

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

		[ProtoMember(1, IsRequired = false, Name = "ForceSelection", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool ForceSelection
		{
			get
			{
				return _ForceSelection;
			}
			set
			{
				_ForceSelection = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "FailOnUnselect", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool FailOnUnselect
		{
			get
			{
				return _FailOnUnselect;
			}
			set
			{
				_FailOnUnselect = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
