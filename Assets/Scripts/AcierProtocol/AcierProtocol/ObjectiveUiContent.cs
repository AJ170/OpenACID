using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveUiContent")]
	public class ObjectiveUiContent : IExtensible
	{
		private ObjectiveBaseContent _ObjectiveBaseContent;

		private string _TargetPath = "";

		private EAction _Action;

		private bool _WaitForInput;

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

		[ProtoMember(9, IsRequired = false, Name = "TargetPath", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string TargetPath
		{
			get
			{
				return _TargetPath;
			}
			set
			{
				_TargetPath = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "Action", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EAction.Hide)]
		public EAction Action
		{
			get
			{
				return _Action;
			}
			set
			{
				_Action = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "WaitForInput", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool WaitForInput
		{
			get
			{
				return _WaitForInput;
			}
			set
			{
				_WaitForInput = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
