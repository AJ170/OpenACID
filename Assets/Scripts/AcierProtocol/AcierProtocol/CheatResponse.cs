using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "CheatResponse")]
	public class CheatResponse : IExtensible
	{
		private string _param1 = "";

		private string _param2 = "";

		private MissionFile _missionFile;

		private MissionToken _missionToken;

		private IExtension extensionObject;

		[ProtoMember(2, IsRequired = false, Name = "param1", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string param1
		{
			get
			{
				return _param1;
			}
			set
			{
				_param1 = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "param2", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string param2
		{
			get
			{
				return _param2;
			}
			set
			{
				_param2 = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "missionFile", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionFile missionFile
		{
			get
			{
				return _missionFile;
			}
			set
			{
				_missionFile = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "missionToken", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionToken missionToken
		{
			get
			{
				return _missionToken;
			}
			set
			{
				_missionToken = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
