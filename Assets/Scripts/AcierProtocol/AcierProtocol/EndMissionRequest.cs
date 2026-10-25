using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "EndMissionRequest")]
	public class EndMissionRequest : IExtensible
	{
		private string _missionToken = "";

		private bool _won;

		private MissionResult _missionResult;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "missionToken", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string missionToken
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

		[ProtoMember(2, IsRequired = false, Name = "won", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool won
		{
			get
			{
				return _won;
			}
			set
			{
				_won = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "missionResult", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionResult missionResult
		{
			get
			{
				return _missionResult;
			}
			set
			{
				_missionResult = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
