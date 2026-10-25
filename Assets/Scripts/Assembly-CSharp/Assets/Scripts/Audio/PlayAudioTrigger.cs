using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(SphereCollider))]
	public class PlayAudioTrigger : MonoBehaviour
	{
		private SphereCollider m_collider;

		private AudioSource m_audio;

		public PlayAudio PlayAudio;

		private void Awake()
		{
			m_audio = GetComponent<AudioSource>();
			SetupHierarchy();
			SetupCollision();
		}

		private void SetupHierarchy()
		{
			if (PlayAudio == null)
			{
				GameObject gameObject = new GameObject("PlayAudio");
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = Vector3.zero;
				AudioSource copy = gameObject.AddComponent<AudioSource>();
				if (m_audio != null)
				{
					AudioSourceFactory.CopyFromTemplate(m_audio, copy);
					UnityEngine.Object.Destroy(m_audio);
				}
				PlayAudio = gameObject.AddComponent<PlayAudio>();
			}
		}

		private void SetupCollision()
		{
			m_collider = base.gameObject.AddComponentIfNotExist<SphereCollider>();
			m_collider.isTrigger = true;
			m_collider.radius = PlayAudio.Audio.maxDistance;
		}

		private void OnEnable()
		{
			if (!Application.isEditor)
			{
				PlayAudio.enabled = IsPlayerInsideTrigger();
			}
		}

		private void OnDisable()
		{
			if (!Application.isEditor)
			{
				PlayAudio.enabled = false;
			}
		}

		private bool IsPlayerInsideTrigger()
		{
			return Vector3.Distance(base.transform.position, PlayerHelper.Entity.Position) < m_collider.radius;
		}

		private bool IsPlayer(Collider _collider)
		{
			EntityWrapperComponent component = _collider.GetComponent<EntityWrapperComponent>();
			return component != null && component.Entity.Type == EntityTypes.EType.PlayerCharacter;
		}

		private void OnTriggerEnter(Collider _collider)
		{
			if (IsPlayer(_collider))
			{
				PlayAudio.enabled = true;
			}
		}

		private void OnTriggerExit(Collider _collider)
		{
			if (IsPlayer(_collider))
			{
				PlayAudio.enabled = false;
			}
		}
	}
}
