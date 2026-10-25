using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "SkillTrainingTimeMapping")]
	public class SkillTrainingTimeMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private int _spentSkillPoints;

			private int _trainingTimeSeconds;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "spentSkillPoints", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int spentSkillPoints
			{
				get
				{
					return _spentSkillPoints;
				}
				set
				{
					_spentSkillPoints = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "trainingTimeSeconds", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int trainingTimeSeconds
			{
				get
				{
					return _trainingTimeSeconds;
				}
				set
				{
					_trainingTimeSeconds = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Entry[] _entries = new Entry[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "entries", DataFormat = DataFormat.Default)]
		public Entry[] entries
		{
			get
			{
				return _entries;
			}
			set
			{
				Array.Resize(ref _entries, value.Length);
				value.CopyTo(_entries, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
