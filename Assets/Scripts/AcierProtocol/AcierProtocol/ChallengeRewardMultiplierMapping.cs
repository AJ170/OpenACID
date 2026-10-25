using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ChallengeRewardMultiplierMapping")]
	public class ChallengeRewardMultiplierMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private int _challengeObjectiveId;

			private float _multiplier;

			private bool _forced;

			private bool _hidden;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "challengeObjectiveId", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int challengeObjectiveId
			{
				get
				{
					return _challengeObjectiveId;
				}
				set
				{
					_challengeObjectiveId = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "multiplier", DataFormat = DataFormat.FixedSize)]
			[DefaultValue(0f)]
			public float multiplier
			{
				get
				{
					return _multiplier;
				}
				set
				{
					_multiplier = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "forced", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool forced
			{
				get
				{
					return _forced;
				}
				set
				{
					_forced = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "hidden", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool hidden
			{
				get
				{
					return _hidden;
				}
				set
				{
					_hidden = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Entry[] _entries = new Entry[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "entries", DataFormat = DataFormat.Default)]
		public Entry[] entries
		{
			get
			{
				return _entries;
			}
			set
			{
				Array.Resize(ref _entries, value.Length);
				value.CopyTo(_entries, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
