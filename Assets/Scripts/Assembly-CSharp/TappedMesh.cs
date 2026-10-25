using System;
using UbiCore.Messenger;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TappedMesh : MonoBehaviour
{
	public enum eTriggerType
	{
		None = -1,
		MoveTo = 0,
		MoveToBench = 1
	}

	[Serializable]
	public class TriggerData
	{
		public Transform mStart;

		public Transform mEnd;
	}

	[SerializeField]
	private eTriggerType m_Type = eTriggerType.None;

	[SerializeField]
	private TriggerData m_Data;

	public eTriggerType Type
	{
		get
		{
			return m_Type;
		}
		set
		{
			m_Type = value;
		}
	}

	public TriggerData Data
	{
		get
		{
			return m_Data;
		}
		set
		{
			m_Data = value;
		}
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
		if (m_Data == null)
		{
			m_Data = new TriggerData();
		}
	}

	public bool OnTap()
	{
		bool result = false;
		switch (m_Type)
		{
		case eTriggerType.MoveTo:
			Messenger.Instance.Broadcast(new MoveToEvent(null, MoveToEvent.EInputType.Hotspot));
			result = true;
			break;
		case eTriggerType.MoveToBench:
			result = true;
			break;
		case eTriggerType.None:
			result = true;
			break;
		}
		return result;
	}
}
