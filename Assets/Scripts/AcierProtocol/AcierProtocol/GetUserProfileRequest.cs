using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetUserProfileRequest")]
	public class GetUserProfileRequest : IExtensible
	{
		private bool _needInventory = true;

		private bool _needAssassins = true;

		private bool _needResources = true;

		private bool _needMissions = true;

		private bool _needCounter = true;

		private bool _needAchievements = true;

		private bool _needTutorial = true;

		private bool _needEntitlements = true;

		private bool _needUserSettings = true;

		private bool _needProgressionQuests = true;

		private bool _needEventsProgress = true;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "needInventory", DataFormat = DataFormat.Default)]
		[DefaultValue(true)]
		public bool needInventory
		{
			get
			{
				return _needInventory;
			}
			set
			{
				_needInventory = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "needAssassins", DataFormat = DataFormat.Default)]
		[DefaultValue(true)]
		public bool needAssassins
		{
			get
			{
				return _needAssassins;
			}
			set
			{
				_needAssassins = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "needResources", DataFormat = DataFormat.Default)]
		[DefaultValue(true)]
		public bool needResources
		{
			get
			{
				return _needResources;
			}
			set
			{
				_needResources = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "needMissions", DataFormat = DataFormat.Default)]
		[DefaultValue(true)]
		public bool needMissions
		{
			get
			{
				return _needMissions;
			}
			set
			{
				_needMissions = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "needCounter", DataFormat = DataFormat.Default)]
		[DefaultValue(true)]
		public bool needCounter
		{
			get
			{
				return _needCounter;
			}
			set
			{
				_needCounter = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "needAchievements", DataFormat = DataFormat.Default)]
		[DefaultValue(true)]
		public bool needAchievements
		{
			get
			{
				return _needAchievements;
			}
			set
			{
				_needAchievements = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "needTutorial", DataFormat = DataFormat.Default)]
		[DefaultValue(true)]
		public bool needTutorial
		{
			get
			{
				return _needTutorial;
			}
			set
			{
				_needTutorial = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "needEntitlements", DataFormat = DataFormat.Default)]
		[DefaultValue(true)]
		public bool needEntitlements
		{
			get
			{
				return _needEntitlements;
			}
			set
			{
				_needEntitlements = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "needUserSettings", DataFormat = DataFormat.Default)]
		[DefaultValue(true)]
		public bool needUserSettings
		{
			get
			{
				return _needUserSettings;
			}
			set
			{
				_needUserSettings = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "needProgressionQuests", DataFormat = DataFormat.Default)]
		[DefaultValue(true)]
		public bool needProgressionQuests
		{
			get
			{
				return _needProgressionQuests;
			}
			set
			{
				_needProgressionQuests = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "needEventsProgress", DataFormat = DataFormat.Default)]
		[DefaultValue(true)]
		public bool needEventsProgress
		{
			get
			{
				return _needEventsProgress;
			}
			set
			{
				_needEventsProgress = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
