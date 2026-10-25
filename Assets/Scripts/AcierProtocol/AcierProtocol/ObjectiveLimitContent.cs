using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveLimitContent")]
	public class ObjectiveLimitContent : IExtensible
	{
		private ObjectiveBaseContent _ObjectiveBaseContent;

		private float _Limit;

		private ELimitType _LimitType;

		private bool _Invert;

		private bool _InvertedResult;

		private bool _PreventFailingOnIncrease = true;

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

		[ProtoMember(1, IsRequired = false, Name = "Limit", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float Limit
		{
			get
			{
				return _Limit;
			}
			set
			{
				_Limit = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "LimitType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ELimitType.Roof)]
		public ELimitType LimitType
		{
			get
			{
				return _LimitType;
			}
			set
			{
				_LimitType = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Invert", DataFormat = DataFormat.Default)]
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

		[ProtoMember(4, IsRequired = false, Name = "InvertedResult", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool InvertedResult
		{
			get
			{
				return _InvertedResult;
			}
			set
			{
				_InvertedResult = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "PreventFailingOnIncrease", DataFormat = DataFormat.Default)]
		[DefaultValue(true)]
		public bool PreventFailingOnIncrease
		{
			get
			{
				return _PreventFailingOnIncrease;
			}
			set
			{
				_PreventFailingOnIncrease = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
