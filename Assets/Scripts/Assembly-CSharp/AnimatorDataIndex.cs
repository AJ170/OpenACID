using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assets.Scripts.Engine;
using SharedBaseLib.Core;
using UnityEngine;

public class AnimatorDataIndex : ScriptableObject
{
	[SerializeField]
	private List<AnimatorData> m_data;

	private readonly BBDictionary<int, AnimatorData.MotionData> m_motionIndex = new BBDictionary<int, AnimatorData.MotionData>();

	private readonly BBDictionary<int, AnimatorData.MotionData> m_clipCache = new BBDictionary<int, AnimatorData.MotionData>();

	private bool m_loaded;

	private static readonly BBList<AnimatorDataIndex> Instances = new BBList<AnimatorDataIndex>(2);

	private void OnEnable()
	{
		m_motionIndex.Clear();
		m_clipCache.Clear();
		m_loaded = false;
	}

	public void LoadIfNotLoaded()
	{
		if (m_loaded)
		{
			return;
		}
		if (!Instances.Contains(this))
		{
			Instances.Add(this);
		}
		for (int i = 0; i < m_data.Count; i++)
		{
			ReadOnlyCollection<AnimatorData.MotionData> motionDataList = m_data[i].MotionDataList;
			for (int j = 0; j < motionDataList.Count; j++)
			{
				AnimatorData.MotionData motionData = motionDataList[j];
				int crc = motionData.m_crc32;
				AnimatorData.MotionData value;
				if (m_motionIndex.TryGetValue(crc, out value))
				{
					value.Merge(motionData);
				}
				else
				{
					m_motionIndex.Add(crc, new AnimatorData.MotionData(motionData));
				}
				UnifyBlendIds(ref m_motionIndex[crc].m_events);
			}
		}
		m_loaded = true;
	}

	private void UnifyBlendIds(ref List<AnimatorData.Event> _events)
	{
		for (int i = 0; i < _events.Count; i++)
		{
			AnimatorData.Event @event = _events[i];
			bool flag = false;
			switch (@event.m_type)
			{
			case AnimatorEventType.SoundFootCrouch:
			case AnimatorEventType.SoundFootWalk:
			case AnimatorEventType.SoundFootRun:
			case AnimatorEventType.SoundFootSprint:
			case AnimatorEventType.SoundFootSlide:
			case AnimatorEventType.SoundFootStop:
				flag = true;
				break;
			default:
				flag = false;
				break;
			}
			if (flag)
			{
				int num = @event.m_blendID & 0xFFF;
				@event.m_blendID = num | 0x210000;
			}
		}
	}

	public static void Shutdown()
	{
		for (int i = 0; i < Instances.Count; i++)
		{
			Instances[i].m_clipCache.Clear();
		}
		Instances.ClearIfNotEmpty();
	}

	public AnimatorData.MotionData GetMotionData(AnimationClip _clip)
	{
		AnimatorData.MotionData value = null;
		int hashCode = _clip.GetHashCode();
		if (!m_clipCache.TryGetValue(hashCode, out value))
		{
			m_motionIndex.TryGetValue((int)StringCRC32.Compute(_clip.name), out value);
			m_clipCache[hashCode] = value;
		}
		return value;
	}

	public AnimatorData.Event GetEventByMotion(AnimationClip _motion, AnimatorEventType _eventType)
	{
		AnimatorData.MotionData motionData = GetMotionData(_motion);
		if (motionData != null)
		{
			for (int i = 0; i < motionData.m_events.Count; i++)
			{
				AnimatorData.Event @event = motionData.m_events[i];
				if (@event.m_type == _eventType)
				{
					return @event;
				}
			}
		}
		return null;
	}

	public List<AnimatorData.Event> GetEventsByMotion(AnimationClip _motion, AnimatorEventType _eventType)
	{
		List<AnimatorData.Event> list = new List<AnimatorData.Event>();
		AnimatorData.MotionData motionData = GetMotionData(_motion);
		if (motionData != null)
		{
			for (int i = 0; i < motionData.m_events.Count; i++)
			{
				AnimatorData.Event @event = motionData.m_events[i];
				if (@event.m_type == _eventType)
				{
					list.Add(@event);
				}
			}
		}
		return list;
	}
}
