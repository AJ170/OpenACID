using SharedBaseLib.Serialization;

namespace Assets.Scripts.UI.Screens.CreditsScreen
{
	public class CreditsItem : ICreditsItem
	{
		public ECreditsItemType Type { get; private set; }

		public float Position { get; set; }

		public float Size { get; set; }

		public CreditsTab Tab { get; set; }

		public CreditsItem(ECreditsItemType _type)
		{
			Type = _type;
		}

		public static CreditsItem Create(ECreditsItemType _type)
		{
			switch (_type)
			{
			case ECreditsItemType.Section:
			case ECreditsItemType.Department:
			case ECreditsItemType.Job:
			case ECreditsItemType.Person:
			case ECreditsItemType.Text:
				return new CreditsTextItem(_type);
			default:
				return null;
			}
		}

		public static CreditsItem Serialize(DataSerializerBase _s, CreditsItem _item)
		{
			_s.SerializeBeginNode("item");
			if (_s.Mode == EMode.EMode_Read)
			{
				ECreditsItemType type = (ECreditsItemType)_s.SerializeEnum("type", typeof(ECreditsItemType), -1);
				_item = Create(type);
			}
			else
			{
				_s.SerializeEnum("type", _item.Type.GetType(), (int)_item.Type);
			}
			_item.SerializeInternal(_s);
			_s.SerializeEndNode();
			return _item;
		}

		protected virtual void SerializeInternal(DataSerializerBase _s)
		{
			Position = _s.SerializeFloat("position", Position);
			Size = _s.SerializeFloat("size", Size);
		}
	}
}
