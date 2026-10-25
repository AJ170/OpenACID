using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Achievement")]
	public class Achievement : IExtensible
	{
		private string _definitionId = "";

		private AchievementRank[] _ranks = new AchievementRank[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "definitionId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string definitionId
		{
			get
			{
				return _definitionId;
			}
			set
			{
				_definitionId = value;
			}
		}

		[ProtoMember(2, Name = "ranks", DataFormat = DataFormat.Default)]
		public AchievementRank[] ranks
		{
			get
			{
				return _ranks;
			}
			set
			{
				Array.Resize(ref _ranks, value.Length);
				value.CopyTo(_ranks, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
