using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class AudioManagerInspector : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_channels;

		[SerializeField]
		private GameObject m_audioSourceFactory;

		[SerializeField]
		private AudioListener m_audioListener;

		[SerializeField]
		private float m_slowMotionFromZeroToOne;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_slowMotionTimeScaleMin = 0.3f;

		public GameObject Channels
		{
			get
			{
				return m_channels;
			}
		}

		public GameObject AudioSourceFactory
		{
			get
			{
				return m_audioSourceFactory;
			}
		}

		public AudioListener AudioListener
		{
			get
			{
				return m_audioListener;
			}
		}

		public float SlowMotionFromZeroToOne
		{
			get
			{
				return m_slowMotionFromZeroToOne;
			}
		}

		public float SlowMotionTimeScaleMin
		{
			get
			{
				return m_slowMotionTimeScaleMin;
			}
		}
	}
}
