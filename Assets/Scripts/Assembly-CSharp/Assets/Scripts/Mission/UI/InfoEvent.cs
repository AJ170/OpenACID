using AcierProtocol;
using Assets.Scripts.UI.Localization;
using UnityEngine;

namespace Assets.Scripts.Mission.UI
{
	public class InfoEvent : LogicUiEvent
	{
		public enum ETextAnnotationMode
		{
			Invalid = 0,
			MessageOpen = 1,
			MessageOpenExternal = 2,
			MessageCloseExternal = 3,
			PopupOpenConfirm = 4,
			PopupOpenExternal = 5,
			PopupCloseExternal = 6,
			ImageOpen = 7,
			ImageOpenExternal = 8,
			ImageCloseExternal = 9
		}

		public bool Immediate { get; private set; }

		public EInfoEventType InfoType { get; private set; }

		public int TextId { get; private set; }

		public ETextAnnotationMode AnnotationMode { get; private set; }

		public int HeaderTextId { get; private set; }

		public StringMapper StringMapper { get; private set; }

		public int UniqueId { get; private set; }

		public Texture Texture { get; private set; }

		public InfoEvent(EInfoEventType _infoType, int _textId, ETextAnnotationMode _annotationMode, int _headerTextId, Texture _texture, StringMapper _stringMapper, int _uniqueId, bool _immediate = false)
		{
			StringMapper = _stringMapper;
			InfoType = _infoType;
			TextId = _textId;
			AnnotationMode = _annotationMode;
			HeaderTextId = _headerTextId;
			UniqueId = _uniqueId;
			Texture = _texture;
			Immediate = _immediate;
		}
	}
}
