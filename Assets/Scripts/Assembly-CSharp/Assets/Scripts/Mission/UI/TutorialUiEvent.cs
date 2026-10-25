namespace Assets.Scripts.Mission.UI
{
	public class TutorialUiEvent : LogicUiEvent
	{
		public enum EType
		{
			ShowWidget = 0,
			HideWidget = 1,
			SetHighlight = 2,
			RemoveHighlight = 3,
			ActivateSystem = 4,
			DeactivateSystem = 5,
			BlockWidget = 6,
			UnBlockWidget = 7
		}

		public EType Type { get; private set; }

		public string Path { get; private set; }

		public TutorialUiEvent(string _path, EType _type)
		{
			Type = _type;
			Path = _path;
		}

		public override string ToString()
		{
			return string.Format("TutorialUiEvent {0}:{1}", Path, Type);
		}
	}
}
