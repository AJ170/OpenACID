using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "TutorialCategoryMapping")]
	public class TutorialCategoryMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private ETutorialCategory _tutorialCategory;

			private string[] _grantedEntitlements = new string[0];

			private string[] _reqEntitlements = new string[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "tutorialCategory", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(ETutorialCategory.TutCatIntroMission)]
			public ETutorialCategory tutorialCategory
			{
				get
				{
					return _tutorialCategory;
				}
				set
				{
					_tutorialCategory = value;
				}
			}

			[ProtoMember(2, Name = "grantedEntitlements", DataFormat = DataFormat.Default)]
			public string[] grantedEntitlements
			{
				get
				{
					return _grantedEntitlements;
				}
				set
				{
					Array.Resize(ref _grantedEntitlements, value.Length);
					value.CopyTo(_grantedEntitlements, 0);
				}
			}

			[ProtoMember(3, Name = "reqEntitlements", DataFormat = DataFormat.Default)]
			public string[] reqEntitlements
			{
				get
				{
					return _reqEntitlements;
				}
				set
				{
					Array.Resize(ref _reqEntitlements, value.Length);
					value.CopyTo(_reqEntitlements, 0);
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
