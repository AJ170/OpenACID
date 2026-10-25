using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveBaseContent")]
	public class ObjectiveBaseContent : IExtensible
	{
		private MissionBaseContent _MissionBaseContent;

		private bool _IsPrimaryObjective;

		private bool _IsOptionalObjective;

		private bool _IsEliteObjective;

		private bool _HideDescription;

		private bool _ShowFailMessageEvenIfHidden;

		private string _Description = "";

		private string _ShortDescription = "";

		private string _SecondaryDescription = "";

		private int _PhaseCompletedMessage;

		private long _OnSuccessId;

		private long _OnFailId;

		private bool _IsVisibleInEagleVision = true;

		private EObjectiveType _TrackerIcon;

		private EObjectiveResult _TerminationResult = EObjectiveResult.Failed_EObjectiveResult;

		private IExtension extensionObject;

		[ProtoMember(20, IsRequired = false, Name = "MissionBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionBaseContent MissionBaseContent
		{
			get
			{
				return _MissionBaseContent;
			}
			set
			{
				_MissionBaseContent = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "IsPrimaryObjective", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool IsPrimaryObjective
		{
			get
			{
				return _IsPrimaryObjective;
			}
			set
			{
				_IsPrimaryObjective = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "IsOptionalObjective", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool IsOptionalObjective
		{
			get
			{
				return _IsOptionalObjective;
			}
			set
			{
				_IsOptionalObjective = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "IsEliteObjective", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool IsEliteObjective
		{
			get
			{
				return _IsEliteObjective;
			}
			set
			{
				_IsEliteObjective = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "HideDescription", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool HideDescription
		{
			get
			{
				return _HideDescription;
			}
			set
			{
				_HideDescription = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "ShowFailMessageEvenIfHidden", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool ShowFailMessageEvenIfHidden
		{
			get
			{
				return _ShowFailMessageEvenIfHidden;
			}
			set
			{
				_ShowFailMessageEvenIfHidden = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Description", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string Description
		{
			get
			{
				return _Description;
			}
			set
			{
				_Description = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "ShortDescription", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string ShortDescription
		{
			get
			{
				return _ShortDescription;
			}
			set
			{
				_ShortDescription = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "SecondaryDescription", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string SecondaryDescription
		{
			get
			{
				return _SecondaryDescription;
			}
			set
			{
				_SecondaryDescription = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "PhaseCompletedMessage", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int PhaseCompletedMessage
		{
			get
			{
				return _PhaseCompletedMessage;
			}
			set
			{
				_PhaseCompletedMessage = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "OnSuccessId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long OnSuccessId
		{
			get
			{
				return _OnSuccessId;
			}
			set
			{
				_OnSuccessId = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "OnFailId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long OnFailId
		{
			get
			{
				return _OnFailId;
			}
			set
			{
				_OnFailId = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "IsVisibleInEagleVision", DataFormat = DataFormat.Default)]
		[DefaultValue(true)]
		public bool IsVisibleInEagleVision
		{
			get
			{
				return _IsVisibleInEagleVision;
			}
			set
			{
				_IsVisibleInEagleVision = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "TrackerIcon", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EObjectiveType.Deliver_EObjectiveType)]
		public EObjectiveType TrackerIcon
		{
			get
			{
				return _TrackerIcon;
			}
			set
			{
				_TrackerIcon = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "TerminationResult", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EObjectiveResult.Failed_EObjectiveResult)]
		public EObjectiveResult TerminationResult
		{
			get
			{
				return _TerminationResult;
			}
			set
			{
				_TerminationResult = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
