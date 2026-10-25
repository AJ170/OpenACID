using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveIdentifyContent")]
	public class ObjectiveIdentifyContent : IExtensible
	{
		private ObjectiveTargetContent _ObjectiveTargetContent;

		private bool _FailIfTargetDies;

		private bool _UsePlainSight;

		private int _Minimum;

		private long[] _TargetAreas = new long[0];

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "ObjectiveTargetContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveTargetContent ObjectiveTargetContent
		{
			get
			{
				return _ObjectiveTargetContent;
			}
			set
			{
				_ObjectiveTargetContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "FailIfTargetDies", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool FailIfTargetDies
		{
			get
			{
				return _FailIfTargetDies;
			}
			set
			{
				_FailIfTargetDies = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "UsePlainSight", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool UsePlainSight
		{
			get
			{
				return _UsePlainSight;
			}
			set
			{
				_UsePlainSight = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Minimum", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int Minimum
		{
			get
			{
				return _Minimum;
			}
			set
			{
				_Minimum = value;
			}
		}

		[ProtoMember(1, Name = "TargetAreas", DataFormat = DataFormat.TwosComplement)]
		public long[] TargetAreas
		{
			get
			{
				return _TargetAreas;
			}
			set
			{
				Array.Resize(ref _TargetAreas, value.Length);
				value.CopyTo(_TargetAreas, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
