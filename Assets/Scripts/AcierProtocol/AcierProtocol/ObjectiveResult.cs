using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveResult")]
	public class ObjectiveResult : IExtensible
	{
		private EObjectiveResult _Result;

		private EObjectiveProgress _Tracker;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EObjectiveResult.Undefined_EObjectiveResult)]
		public EObjectiveResult Result
		{
			get
			{
				return _Result;
			}
			set
			{
				_Result = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Tracker", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EObjectiveProgress.NotStarted_EObjectiveProgress)]
		public EObjectiveProgress Tracker
		{
			get
			{
				return _Tracker;
			}
			set
			{
				_Tracker = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
