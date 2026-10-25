using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "UpdateSkillWheelRequest")]
	public class UpdateSkillWheelRequest : IExtensible
	{
		private string _assassinId = "";

		private SkillWheel _skillWheel;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "assassinId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string assassinId
		{
			get
			{
				return _assassinId;
			}
			set
			{
				_assassinId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "skillWheel", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public SkillWheel skillWheel
		{
			get
			{
				return _skillWheel;
			}
			set
			{
				_skillWheel = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
