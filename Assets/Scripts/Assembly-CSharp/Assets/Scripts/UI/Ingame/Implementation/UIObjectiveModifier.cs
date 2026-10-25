using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Implementation
{
	public class UIObjectiveModifier : MonoBehaviour
	{
		public UISprite FailedMarker;

		public UIExtendedLabel ModifierText;

		public long LastDataFromObjectiveId { get; private set; }

		public bool IsActive
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		public bool UpdateData(string _text, long _objectiveId, bool _missionFailed)
		{
			bool result = LastDataFromObjectiveId != _objectiveId || _missionFailed;
			ModifierText.text = _text;
			FailedMarker.gameObject.SetActive(_missionFailed);
			LastDataFromObjectiveId = _objectiveId;
			base.gameObject.SetActive(true);
			return result;
		}

		public virtual void Hide()
		{
			base.gameObject.SetActive(false);
		}

		private void Awake()
		{
		}
	}
}
