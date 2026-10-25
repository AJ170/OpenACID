using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SharedBaseLib.Core;
using UnityEngine;

public class AnimatorData : ScriptableObject
{
	[Serializable]
	public class Event : IComparable<Event>
	{
		public float m_time;

		public AnimatorEventType m_type;

		public int m_blendID;

		public bool m_OneTimeEvent;

		public UnityEngine.Object m_payload;

		public Event(Event _e)
		{
			m_time = _e.m_time;
			m_type = _e.m_type;
			m_blendID = _e.m_blendID;
			m_OneTimeEvent = _e.m_OneTimeEvent;
			m_payload = _e.m_payload;
		}

		public Event(float _time, AnimatorEventType _type, int _blendID, UnityEngine.Object _payload)
		{
			m_time = _time;
			m_type = _type;
			m_blendID = _blendID;
			m_payload = _payload;
		}

		public int CompareTo(Event other)
		{
			return m_time.CompareTo(other.m_time);
		}
	}

	[Serializable]
	public class MotionData
	{
		public int m_crc32;

		public List<Event> m_events = new List<Event>();

		public float m_rootMotionRotation;

		public float m_rootMotionLength;

		public MotionData(string _motion)
		{
			m_crc32 = (int)StringCRC32.Compute(_motion);
		}

		public MotionData(MotionData _other)
		{
			m_crc32 = _other.m_crc32;
			m_events.AddRange(_other.m_events);
			m_rootMotionRotation = _other.m_rootMotionRotation;
			m_rootMotionLength = _other.m_rootMotionLength;
		}

		public void Merge(MotionData _other)
		{
			m_events.AddRange(_other.m_events);
		}

		public bool IsEmpty()
		{
			return m_events.Count == 0 && m_rootMotionRotation == 0f;
		}

		public void Sort()
		{
			m_events.Sort();
		}
	}

	[SerializeField]
	private List<MotionData> m_motionData = new List<MotionData>();

	[SerializeField]
	[HideInInspector]
	private List<int> m_allowedEventTypes = new List<int>();

	public ReadOnlyCollection<MotionData> MotionDataList
	{
		get
		{
			return new ReadOnlyCollection<MotionData>(m_motionData);
		}
	}
}
