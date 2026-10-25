using System.Collections.Generic;
using UbiCore;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class AudioSourceFactory
	{
		private class Item
		{
			public AudioSource Channel;

			public AudioThreeDSource ParentTracker;

			public bool IsUsed;
		}

		private readonly List<Item> m_items = new List<Item>();

		private readonly Transform m_unusedAudioSourcesParent;

		public AudioSourceFactory(Transform _unusedAudioSourcesParent)
		{
			m_unusedAudioSourcesParent = _unusedAudioSourcesParent;
		}

		public AudioSource GetFromTemplate(AudioSource _template)
		{
			for (int num = m_items.Count - 1; num >= 0; num--)
			{
				if (m_items[num].Channel == null)
				{
					m_items.RemoveAt(num);
				}
				else if (!m_items[num].IsUsed)
				{
					return Take(m_items[num], _template);
				}
			}
			Item item = Create();
			m_items.Add(item);
			return Take(item, _template);
		}

		public void GiveBack(AudioSource _audioSource)
		{
			for (int num = m_items.Count - 1; num >= 0; num--)
			{
				Item item = m_items[num];
				if (item.Channel == null)
				{
					m_items.RemoveAt(num);
				}
				else if (_audioSource == item.Channel)
				{
					item.IsUsed = false;
					item.Channel.transform.parent = m_unusedAudioSourcesParent;
					item.ParentTracker.ParentTransform = null;
					item.Channel.gameObject.SetActive(false);
					break;
				}
			}
		}

		public bool DetachFromParent(AudioSource _audioSource)
		{
			for (int i = 0; i < m_items.Count; i++)
			{
				Item item = m_items[i];
				if (item.Channel == null)
				{
					m_items.RemoveAt(i);
					return true;
				}
				if (_audioSource == item.Channel)
				{
					item.ParentTracker.ParentTransform = null;
					return true;
				}
			}
			return false;
		}

		private Item Create()
		{
			Item item = new Item();
			item.IsUsed = false;
			GameObject gameObject = new GameObject("AudioSource 3D");
			item.ParentTracker = gameObject.AddComponent<AudioThreeDSource>();
			item.Channel = gameObject.AddComponent<AudioSource>();
			item.Channel.gameObject.SetActive(false);
			return item;
		}

		private AudioSource Take(Item _item, AudioSource _template)
		{
			_item.IsUsed = true;
			_item.ParentTracker.ParentTransform = _template.transform;
			_item.Channel.gameObject.SetActive(true);
			CopyFromTemplate(_template, _item.Channel);
			return _item.Channel;
		}

		public static void CopyFromTemplate(AudioSource _template, AudioSource _copy)
		{
			_copy.mute = _template.mute;
			_copy.bypassEffects = _template.bypassEffects;
			_copy.bypassListenerEffects = _template.bypassListenerEffects;
			_copy.bypassReverbZones = _template.bypassReverbZones;
			_copy.dopplerLevel = _template.dopplerLevel;
			_copy.rolloffMode = _template.rolloffMode;
			_copy.minDistance = _template.minDistance;
			_copy.spatialBlend = _template.spatialBlend;
			_copy.spread = ((!MathHelper.Approximately(_template.spread, 0f)) ? _template.spread : 40f);
			_copy.maxDistance = _template.maxDistance;
			_copy.panStereo = _template.panStereo;
			_copy.ignoreListenerPause = _template.ignoreListenerPause;
			_copy.ignoreListenerVolume = _template.ignoreListenerVolume;
			_copy.velocityUpdateMode = _template.velocityUpdateMode;
			_copy.transform.position = _template.transform.position;
			_copy.transform.rotation = _template.transform.rotation;
		}
	}
}
