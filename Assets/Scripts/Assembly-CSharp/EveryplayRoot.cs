using Assets.Scripts;
using UnityEngine;

public class EveryplayRoot : MonoBehaviour
{
	[SerializeField]
	public EveryplayRecordIndicator RecordIndicator;

	private void Start()
	{
		EventDelegate.Add(RecordIndicator.RecordButton.onClick, EveryplayManager.OnRecordButtonClicked);
	}

	private void OnDestroy()
	{
		EventDelegate.Remove(RecordIndicator.RecordButton.onClick, EveryplayManager.OnRecordButtonClicked);
	}
}
