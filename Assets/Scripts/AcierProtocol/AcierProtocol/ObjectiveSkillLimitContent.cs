using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveSkillLimitContent")]
	public class ObjectiveSkillLimitContent : IExtensible
	{
		private ObjectiveBaseContent _ObjectiveBaseContent;

		private int _Limit;

		private string _SkillName = "";

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

		[ProtoMember(9, IsRequired = false, Name = "Limit", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int Limit
		{
			get
			{
				return _Limit;
			}
			set
			{
				_Limit = value;
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

		[ProtoMember(2, IsRequired = false, Name = "Invert", DataFormat = DataFormat.Default)]
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
