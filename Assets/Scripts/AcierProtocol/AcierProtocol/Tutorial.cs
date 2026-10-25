using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Tutorial")]
	public class Tutorial : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Category")]
		public class Category : IExtensible
		{
			private ETutorialCategory _category;

			private int _counter;

			private bool _finished;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "category", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(ETutorialCategory.TutCatIntroMission)]
			public ETutorialCategory category
			{
				get
				{
					return _category;
				}
				set
				{
					_category = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "counter", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int counter
			{
				get
				{
					return _counter;
				}
				set
				{
					_counter = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "finished", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool finished
			{
				get
				{
					return _finished;
				}
				set
				{
					_finished = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Category[] _categories = new Category[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "categories", DataFormat = DataFormat.Default)]
		public Category[] categories
		{
			get
			{
				return _categories;
			}
			set
			{
				Array.Resize(ref _categories, value.Length);
				value.CopyTo(_categories, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
