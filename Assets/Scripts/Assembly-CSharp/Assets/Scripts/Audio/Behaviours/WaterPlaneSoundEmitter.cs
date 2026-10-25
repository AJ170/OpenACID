using Assets.Scripts.Engine.ExtensionMethods;
using Assets.Scripts.Network.Missions.Helpers;
using UnityEngine;

namespace Assets.Scripts.Audio.Behaviours
{
	public class WaterPlaneSoundEmitter : MonoBehaviour
	{
		public delegate bool WaterPlaneTriggerDelegate(WaterPlaneSoundEmitter _sender, bool _activated);

		private float m_enterRadius;

		private bool m_isActive;

		private WaterPlaneSoundSender m_sender;

		private Vector2[] m_corners;

		private GameObject m_instance;

		private float m_height;

		private Vector3 m_targetPos;

		public AudioSource AudioSource { get; private set; }

		public float Distance { get; private set; }

		public bool IsActive
		{
			get
			{
				return m_isActive;
			}
			set
			{
				if (m_isActive != value && this.Trigger != null && this.Trigger(this, value))
				{
					m_isActive = value;
				}
			}
		}

		public event WaterPlaneTriggerDelegate Trigger;

		public void OnDrawGizmosSelected()
		{
			if (m_sender.Border != null)
			{
				for (int i = 1; i <= m_sender.Border.Count; i++)
				{
					Vector3 position = m_sender.Border[(i - 1) % m_sender.Border.Count].position;
					Vector3 position2 = m_sender.Border[i % m_sender.Border.Count].position;
					Gizmos.DrawLine(position, position2);
				}
				Gizmos.DrawSphere(m_targetPos, 3f);
			}
		}

		public void OnEnable()
		{
			m_sender = GetComponent<WaterPlaneSoundSender>();
			m_instance = Object.Instantiate(m_sender.AudioConfiguration);
			m_instance.transform.parent = m_sender.transform;
			m_sender.transform.localPosition = Vector3.zero;
			m_corners = GetPositions();
			AudioSource = m_instance.GetComponent<AudioSource>();
			m_enterRadius = AudioSource.maxDistance * AudioSource.maxDistance;
		}

		private Vector2[] GetPositions()
		{
			int count = m_sender.Border.Count;
			Vector2[] array = new Vector2[count];
			m_height = 0f;
			for (int i = 0; i < count; i++)
			{
				array[i] = m_sender.Border[i].position.To2D();
				m_height += m_sender.Border[i].position.y;
			}
			m_height /= count;
			return array;
		}

		public void Update()
		{
			if (LogicMain.IsRunning && LogicMain.PlayerEntity.IsActive)
			{
				Vector3 position = LogicMain.PlayerEntity.Position;
				if (MissionMathHelper.IsInside(m_corners, position))
				{
					m_targetPos = new Vector3(position.x, m_height, position.z);
				}
				else
				{
					m_targetPos = MissionMathHelper.ClosestPointOnPolygon(m_corners, position).To3D(m_height);
				}
				m_instance.transform.position = m_targetPos;
				Distance = (position - m_targetPos).sqrMagnitude;
				IsActive = Distance < m_enterRadius;
			}
		}
	}
}
