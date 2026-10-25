using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveTimeLimitContent")]
	public class ObjectiveTimeLimitContent : IExtensible
	{
		private ObjectiveBaseContent _ObjectiveBaseContent;

		private bool _Invert;

		private bool _WaitForTimerEnd;

		private int _TimeInSeconds;

		private int _TimeForRetry;

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "ObjectiveBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveBaseContent ObjectiveBaseContent
		{
			get
			{
				return _ObjectiveBaseContent;
			}
			set
			{
				_ObjectiveBaseContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "Invert", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool Invert
		{
			get
			{
				return _Invert;
			}
			set
			{
				_Invert = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "WaitForTimerEnd", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool WaitForTimerEnd
		{
			get
			{
				return _WaitForTimerEnd;
			}
			set
			{
				_WaitForTimerEnd = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "TimeInSeconds", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int TimeInSeconds
		{
			get
			{
				return _TimeInSeconds;
			}
			set
			{
				_TimeInSeconds = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "TimeForRetry", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int TimeForRetry
		{
			get
			{
				return _TimeForRetry;
			}
			set
			{
				_TimeForRetry = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
