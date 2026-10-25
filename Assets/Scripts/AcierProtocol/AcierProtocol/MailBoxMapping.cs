using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MailBoxMapping")]
	public class MailBoxMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "PriorityLimit")]
		public class PriorityLimit : IExtensible
		{
			private EMailPriority _emailPriority;

			private int _limit;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = true, Name = "emailPriority", DataFormat = DataFormat.TwosComplement)]
			public EMailPriority emailPriority
			{
				get
				{
					return _emailPriority;
				}
				set
				{
					_emailPriority = value;
				}
			}

			[ProtoMember(2, IsRequired = true, Name = "limit", DataFormat = DataFormat.TwosComplement)]
			public int limit
			{
				get
				{
					return _limit;
				}
				set
				{
					_limit = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private PriorityLimit[] _priorityLimitMapping = new PriorityLimit[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "priorityLimitMapping", DataFormat = DataFormat.Default)]
		public PriorityLimit[] priorityLimitMapping
		{
			get
			{
				return _priorityLimitMapping;
			}
			set
			{
				Array.Resize(ref _priorityLimitMapping, value.Length);
				value.CopyTo(_priorityLimitMapping, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
