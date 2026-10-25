using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveCanTriggerSkillContent")]
	public class ObjectiveCanTriggerSkillContent : IExtensible
	{
		private ObjectiveBaseContent _ObjectiveBaseContent;

		private string _SkillName = "";

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

		[ProtoMember(1, IsRequired = false, Name = "SkillName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string SkillName
		{
			get
			{
				return _SkillName;
			}
			set
			{
				_SkillName = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
