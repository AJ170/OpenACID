using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionTextAnnotationContent")]
	public class MissionTextAnnotationContent : IExtensible
	{
		private MissionBaseContent _MissionBaseContent;

		private int _TextId;

		private int _HeaderTextId;

		private bool _WaitForConfirmation;

		private EMissionTextAnnotationType _Type;

		private string _Texture = "";

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "MissionBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionBaseContent MissionBaseContent
		{
			get
			{
				return _MissionBaseContent;
			}
			set
			{
				_MissionBaseContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "TextId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int TextId
		{
			get
			{
				return _TextId;
			}
			set
			{
				_TextId = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "HeaderTextId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int HeaderTextId
		{
			get
			{
				return _HeaderTextId;
			}
			set
			{
				_HeaderTextId = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "WaitForConfirmation", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool WaitForConfirmation
		{
			get
			{
				return _WaitForConfirmation;
			}
			set
			{
				_WaitForConfirmation = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMissionTextAnnotationType.Message)]
		public EMissionTextAnnotationType Type
		{
			get
			{
				return _Type;
			}
			set
			{
				_Type = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Texture", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string Texture
		{
			get
			{
				return _Texture;
			}
			set
			{
				_Texture = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
