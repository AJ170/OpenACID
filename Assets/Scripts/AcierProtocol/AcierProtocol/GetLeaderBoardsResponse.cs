using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetLeaderBoardsResponse")]
	public class GetLeaderBoardsResponse : IExtensible
	{
		private GlobalLeaderBoardResult _globalLeaderBoardResult;

		private EventLeaderBoardResult _latestEventLeaderBoardResult;

		private long _lastUpdatedSince;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "globalLeaderBoardResult", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public GlobalLeaderBoardResult globalLeaderBoardResult
		{
			get
			{
				return _globalLeaderBoardResult;
			}
			set
			{
				_globalLeaderBoardResult = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "latestEventLeaderBoardResult", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public EventLeaderBoardResult latestEventLeaderBoardResult
		{
			get
			{
				return _latestEventLeaderBoardResult;
			}
			set
			{
				_latestEventLeaderBoardResult = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "lastUpdatedSince", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long lastUpdatedSince
		{
			get
			{
				return _lastUpdatedSince;
			}
			set
			{
				_lastUpdatedSince = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
