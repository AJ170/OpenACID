using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionSpawnStaticObjectContent")]
	public class MissionSpawnStaticObjectContent : IExtensible
	{
		private MissionSpawnEntityContent _MissionSpawnEntityContent;

		private string _TypeGuid = "";

		private float _ScaleX;

		private float _ScaleY;

		private float _ScaleZ;

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

		[ProtoMember(1, IsRequired = false, Name = "ScaleX", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float ScaleX
		{
			get
			{
				return _ScaleX;
			}
			set
			{
				_ScaleX = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "ScaleY", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float ScaleY
		{
			get
			{
				return _ScaleY;
			}
			set
			{
				_ScaleY = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "ScaleZ", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float ScaleZ
		{
			get
			{
				return _ScaleZ;
			}
			set
			{
				_ScaleZ = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
