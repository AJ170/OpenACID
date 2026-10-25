using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveParkourContent")]
	public class ObjectiveParkourContent : IExtensible
	{
		private ObjectiveBaseContent _ObjectiveBaseContent;

		private long[] _Challengers = new long[0];

		private long[] _Waypoints = new long[0];

		private int _WaypointWaitingDuration;

		private string _TypeGuid = "";

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

		[ProtoMember(9, Name = "Challengers", DataFormat = DataFormat.TwosComplement)]
		public long[] Challengers
		{
			get
			{
				return _Challengers;
			}
			set
			{
				Array.Resize(ref _Challengers, value.Length);
				value.CopyTo(_Challengers, 0);
			}
		}

		[ProtoMember(1, Name = "Waypoints", DataFormat = DataFormat.TwosComplement)]
		public long[] Waypoints
		{
			get
			{
				return _Waypoints;
			}
			set
			{
				Array.Resize(ref _Waypoints, value.Length);
				value.CopyTo(_Waypoints, 0);
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "WaypointWaitingDuration", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int WaypointWaitingDuration
		{
			get
			{
				return _WaypointWaitingDuration;
			}
			set
			{
				_WaypointWaitingDuration = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "TypeGuid", DataFormat = DataFormat.Default)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
