using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "EventsProgress")]
	public class EventsProgress : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "ChallengeEvent")]
		public class ChallengeEvent : IExtensible
		{
			private string _challengeId = "";

			private long _startCounterValue;

			private ECounterType _challengeCounter;

			private EEventChallengeStatus _status = EEventChallengeStatus.Challenge_InProgress;

			private int _timesToComplete;

			private int _rewardScore;

			private MissionGoalLoot _rewardLoot;

			private int _challengeIndex;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "challengeId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string challengeId
			{
				get
				{
					return _challengeId;
				}
				set
				{
					_challengeId = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "startCounterValue", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long startCounterValue
			{
				get
				{
					return _startCounterValue;
				}
				set
				{
					_startCounterValue = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "challengeCounter", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(ECounterType.InvalidCounter)]
			public ECounterType challengeCounter
			{
				get
				{
					return _challengeCounter;
				}
				set
				{
					_challengeCounter = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "status", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EEventChallengeStatus.Challenge_InProgress)]
			public EEventChallengeStatus status
			{
				get
				{
					return _status;
				}
				set
				{
					_status = value;
				}
			}

			[ProtoMember(5, IsRequired = false, Name = "timesToComplete", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int timesToComplete
			{
				get
				{
					return _timesToComplete;
				}
				set
				{
					_timesToComplete = value;
				}
			}

			[ProtoMember(6, IsRequired = false, Name = "rewardScore", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int rewardScore
			{
				get
				{
					return _rewardScore;
				}
				set
				{
					_rewardScore = value;
				}
			}

			[ProtoMember(7, IsRequired = false, Name = "rewardLoot", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public MissionGoalLoot rewardLoot
			{
				get
				{
					return _rewardLoot;
				}
				set
				{
					_rewardLoot = value;
				}
			}

			[ProtoMember(8, IsRequired = false, Name = "challengeIndex", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int challengeIndex
			{
				get
				{
					return _challengeIndex;
				}
				set
				{
					_challengeIndex = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "EventReward")]
		public class EventReward : IExtensible
		{
			private MissionGoalLoot _rewardLoot;

			private long _scoreMarker;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "rewardLoot", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public MissionGoalLoot rewardLoot
			{
				get
				{
					return _rewardLoot;
				}
				set
				{
					_rewardLoot = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "scoreMarker", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long scoreMarker
			{
				get
				{
					return _scoreMarker;
				}
				set
				{
					_scoreMarker = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private string _mainEventId = "";

		private ChallengeEvent[] _challengeEvents = new ChallengeEvent[0];

		private long _score;

		private EventReward[] _eventRewards = new EventReward[0];

		private int _leaderboardPosition;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "mainEventId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string mainEventId
		{
			get
			{
				return _mainEventId;
			}
			set
			{
				_mainEventId = value;
			}
		}

		[ProtoMember(2, Name = "challengeEvents", DataFormat = DataFormat.Default)]
		public ChallengeEvent[] challengeEvents
		{
			get
			{
				return _challengeEvents;
			}
			set
			{
				Array.Resize(ref _challengeEvents, value.Length);
				value.CopyTo(_challengeEvents, 0);
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "score", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long score
		{
			get
			{
				return _score;
			}
			set
			{
				_score = value;
			}
		}

		[ProtoMember(4, Name = "eventRewards", DataFormat = DataFormat.Default)]
		public EventReward[] eventRewards
		{
			get
			{
				return _eventRewards;
			}
			set
			{
				Array.Resize(ref _eventRewards, value.Length);
				value.CopyTo(_eventRewards, 0);
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "leaderboardPosition", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int leaderboardPosition
		{
			get
			{
				return _leaderboardPosition;
			}
			set
			{
				_leaderboardPosition = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
