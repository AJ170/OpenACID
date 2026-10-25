using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Audio.Behaviours
{
	public class BonFireSoundEmitter : MonoBehaviour
	{
		public delegate bool BonFireTriggerDelegate(BonFireSoundEmitter _sender, bool _activated);

		private float m_enterRadius;

		private bool m_isActive;

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

		public event BonFireTriggerDelegate Trigger;

		public void OnEnable()
		{
			AudioSource = base.gameObject.AddComponentIfNotExist<AudioSource>();
			m_enterRadius = AudioSource.maxDistance * AudioSource.maxDistance;
		}

		public void Update()
		{
			if (LogicMain.IsRunning && LogicMain.PlayerEntity.IsActive)
			{
				Distance = (LogicMain.PlayerEntity.Position - base.transform.position).sqrMagnitude;
				IsActive = Distance < m_enterRadius;
			}
		}
	}
}
