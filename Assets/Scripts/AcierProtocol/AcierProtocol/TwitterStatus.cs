using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "TwitterStatus")]
	public class TwitterStatus : IExtensible
	{
		private long _createdAt;

		private string _text = "";

		private int _favoriteCount;

		private int _retweetCount;

		private TwitterUrlEntity[] _urlEntity = new TwitterUrlEntity[0];

		private TwitterHashTagEntity[] _hashTagEntity = new TwitterHashTagEntity[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "createdAt", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long createdAt
		{
			get
			{
				return _createdAt;
			}
			set
			{
				_createdAt = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "text", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "favoriteCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int favoriteCount
		{
			get
			{
				return _favoriteCount;
			}
			set
			{
				_favoriteCount = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "retweetCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int retweetCount
		{
			get
			{
				return _retweetCount;
			}
			set
			{
				_retweetCount = value;
			}
		}

		[ProtoMember(5, Name = "urlEntity", DataFormat = DataFormat.Default)]
		public TwitterUrlEntity[] urlEntity
		{
			get
			{
				return _urlEntity;
			}
			set
			{
				Array.Resize(ref _urlEntity, value.Length);
				value.CopyTo(_urlEntity, 0);
			}
		}

		[ProtoMember(6, Name = "hashTagEntity", DataFormat = DataFormat.Default)]
		public TwitterHashTagEntity[] hashTagEntity
		{
			get
			{
				return _hashTagEntity;
			}
			set
			{
				Array.Resize(ref _hashTagEntity, value.Length);
				value.CopyTo(_hashTagEntity, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
