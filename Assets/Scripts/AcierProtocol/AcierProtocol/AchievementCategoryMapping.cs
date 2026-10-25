using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "AchievementCategoryMapping")]
	public class AchievementCategoryMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Category")]
		public class Category : IExtensible
		{
			private string _id = "";

			private Category[] _subCategories = new Category[0];

			private string _name = "";

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string id
			{
				get
				{
					return _id;
				}
				set
				{
					_id = value;
				}
			}

			[ProtoMember(2, Name = "subCategories", DataFormat = DataFormat.Default)]
			public Category[] subCategories
			{
				get
				{
					return _subCategories;
				}
				set
				{
					Array.Resize(ref _subCategories, value.Length);
					value.CopyTo(_subCategories, 0);
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string name
			{
				get
				{
					return _name;
				}
				set
				{
					_name = value;
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
