using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "TemplateTargetContent")]
	public class TemplateTargetContent : IExtensible
	{
		private MissionTargetContent _MissionTargetContent;

		private EObjectSubCategory[] _SubCategories = new EObjectSubCategory[0];

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "MissionTargetContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionTargetContent MissionTargetContent
		{
			get
			{
				return _MissionTargetContent;
			}
			set
			{
				_MissionTargetContent = value;
			}
		}

		[ProtoMember(2, Name = "SubCategories", DataFormat = DataFormat.TwosComplement)]
		public EObjectSubCategory[] SubCategories
		{
			get
			{
				return _SubCategories;
			}
			set
			{
				Array.Resize(ref _SubCategories, value.Length);
				value.CopyTo(_SubCategories, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
