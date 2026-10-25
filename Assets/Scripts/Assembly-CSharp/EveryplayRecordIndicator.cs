using Assets.Scripts;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils;
using UnityEngine;

public class EveryplayRecordIndicator : MonoBehaviour
{
	[SerializeField]
	private UIButtonProxy m_recordButton;

	[SerializeField]
	private GameObject m_recordIcon;

	[SerializeField]
	private UIExtendedLabel m_recordLabel;

	private UIWidget m_widget;

	private BBTweenPosition m_tweenPosition;

	public UIButton RecordButton
	{
		get
		{
			return m_recordButton.Button;
		}
	}

	private void Awake()
	{
		m_widget = GetComponent<UIWidget>();
		m_tweenPosition = GetComponent<BBTweenPosition>();
		EveryplayManager.RecordingStarted += OnRecordingStarted;
		EveryplayManager.RecordingStopped += OnRecordingStopped;
	}

	private void Start()
	{
		UpdateTweenPosition();
	}

	private void OnDestroy()
	{
		EveryplayManager.RecordingStarted -= OnRecordingStarted;
		EveryplayManager.RecordingStopped -= OnRecordingStopped;
	}

	private void OnRecordingStarted()
	{
		m_recordLabel.LocalizationKey = 6494;
		MoveIn();
	}

	private void OnRecordingStopped()
	{
		MoveOut();
	}

	private void MoveIn()
	{
		m_tweenPosition.PlayForward();
	}

	private void MoveOut()
	{
		m_tweenPosition.PlayReverse();
	}

	private void UpdateTweenPosition()
	{
		float offscreenPositionY = UIUtil.GetOffscreenPositionY(m_widget, m_widget.height, UIUtil.ESide.Bottom);
		Vector3 localPosition = base.gameObject.transform.localPosition;
		Vector3 from = localPosition;
		from.y = offscreenPositionY;
		m_tweenPosition.from = from;
		m_tweenPosition.to = localPosition;
		m_tweenPosition.ResetToBeginning();
	}
}
