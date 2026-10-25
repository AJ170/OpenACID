using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveKillContent")]
	public class ObjectiveKillContent : IExtensible
	{
		private ObjectiveTargetContent _ObjectiveTargetContent;

		private bool _DoNotKill;

		private int _Minimum;

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

		[ProtoMember(9, IsRequired = false, Name = "DoNotKill", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool DoNotKill
		{
			get
			{
				return _DoNotKill;
			}
			set
			{
				_DoNotKill = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "Minimum", DataFormat = DataFormat.TwosComplement)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
