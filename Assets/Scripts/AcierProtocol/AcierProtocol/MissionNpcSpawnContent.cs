using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionNpcSpawnContent")]
	public class MissionNpcSpawnContent : IExtensible
	{
		private MissionSpawnEntityContent _MissionSpawnEntityContent;

		private string _TypeGuid = "";

		private float _ScalingMultiplier = 1f;

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "MissionSpawnEntityContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionSpawnEntityContent MissionSpawnEntityContent
		{
			get
			{
				return _MissionSpawnEntityContent;
			}
			set
			{
				_MissionSpawnEntityContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "TypeGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string TypeGuid
		{
			get
			{
				return _TypeGuid;
			}
			set
			{
				_TypeGuid = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "ScalingMultiplier", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(1f)]
		public float ScalingMultiplier
		{
			get
			{
				return _ScalingMultiplier;
			}
			set
			{
				_ScalingMultiplier = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
