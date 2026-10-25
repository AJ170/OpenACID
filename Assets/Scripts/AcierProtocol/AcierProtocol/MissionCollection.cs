using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionCollection")]
	public class MissionCollection : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "RankMissions")]
		public class RankMissions : IExtensible
		{
			private int _rankIdx;

			private Mission[] _missions = new Mission[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "rankIdx", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int rankIdx
			{
				get
				{
					return _rankIdx;
				}
				set
				{
					_rankIdx = value;
				}
			}

			[ProtoMember(2, Name = "missions", DataFormat = DataFormat.Default)]
			public Mission[] missions
			{
				get
				{
					return _missions;
				}
				set
				{
					Array.Resize(ref _missions, value.Length);
					value.CopyTo(_missions, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "AssassinMissions")]
		public class AssassinMissions : IExtensible
		{
			private string _assassinId = "";

			private RankMissions[] _rankMissions = new RankMissions[0];

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

			[ProtoMember(2, Name = "rankMissions", DataFormat = DataFormat.Default)]
			public RankMissions[] rankMissions
			{
				get
				{
					return _rankMissions;
				}
				set
				{
					Array.Resize(ref _rankMissions, value.Length);
					value.CopyTo(_rankMissions, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private AssassinMissions[] _assassinMissions = new AssassinMissions[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "assassinMissions", DataFormat = DataFormat.Default)]
		public AssassinMissions[] assassinMissions
		{
			get
			{
				return _assassinMissions;
			}
			set
			{
				Array.Resize(ref _assassinMissions, value.Length);
				value.CopyTo(_assassinMissions, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
