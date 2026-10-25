using Assets.Scripts.Mission.Logic;
using UnityEngine;
using UnityEngine.AI;

namespace UbiCore.Acier
{
	public class MoveInterpreterDecals
	{
		private GameObject m_decalNegativeFeedback;

		private GameObject m_decalPositiveFeedback;

		private bool m_isSetUp;

		private int m_poolSize = 5;

		private GameObject[] m_indicatorPool;

		private GameObject[] m_effectPool;

		private int m_currentIndex;

		private Vector3 previousPosition = Vector3.zero;

		private bool m_Enabled;

		private bool m_AlreadyDisabled;

		public static MoveInterpreterDecals Instance { get; private set; }

		public bool Enabled
		{
			get
			{
				return m_Enabled;
			}
			set
			{
				m_Enabled = value;
				Update();
			}
		}

		private MoveInterpreterDecals()
		{
			m_decalPositiveFeedback = SpawnDecal("Environment/Indicators/MoveInterpreter/positive");
			m_decalNegativeFeedback = SpawnDecal("Environment/Indicators/MoveInterpreter/invalid");
		}

		private void SetUpIndicatorPool()
		{
			m_indicatorPool = new GameObject[m_poolSize];
			m_effectPool = new GameObject[m_poolSize];
			for (int i = 0; i < m_poolSize; i++)
			{
				m_indicatorPool[i] = SpawnDecal("Environment/Indicators/MoveInterpreter/TargetIndicator/Prefabs/target_indicator");
				m_indicatorPool[i].SetActive(false);
				m_effectPool[i] = SpawnDecal("Environment/Indicators/MoveInterpreter/TargetIndicator/Prefabs/target_indicator_appear");
				m_effectPool[i].SetActive(false);
				m_currentIndex = 0;
			}
		}

		private void ApplySortingOrder(GameObject[] gos, int order)
		{
			foreach (GameObject gameObject in gos)
			{
				Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
				Renderer[] array = componentsInChildren;
				foreach (Renderer renderer in array)
				{
					renderer.sortingOrder = order;
				}
			}
		}

		private GameObject SpawnDecal(string _resourceName)
		{
			return (GameObject)Object.Instantiate(Resources.Load(_resourceName, typeof(GameObject)));
		}

		public void Update()
		{
			UpdateDecal();
		}

		public void UpdateDecal()
		{
			if (!m_isSetUp)
			{
				m_isSetUp = true;
				SetUpIndicatorPool();
			}
			if (!(m_indicatorPool[m_currentIndex] != null))
			{
				return;
			}
			if (m_Enabled)
			{
				if (m_AlreadyDisabled)
				{
					m_AlreadyDisabled = false;
				}
				Vector3 vector = new Vector3(0f, -100f, 0f);
				if (Enabled && (PlayerHelper.IsInParcour || PlayerHelper.IsRoaming || PlayerHelper.IsBeingChased))
				{
					if (PlayerHelper.IsApproachingPosition)
					{
						vector = PlayerHelper.TargetPosition;
					}
					else
					{
						NavMeshAgent navMeshAgent = IconInterpreter.NavMeshAgent;
						if (navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.hasPath)
						{
							if (navMeshAgent.nextOffMeshLinkData.valid)
							{
								vector = navMeshAgent.nextOffMeshLinkData.startPos;
							}
							else if (navMeshAgent.hasPath)
							{
								NavMeshPath path = navMeshAgent.path;
								Vector3[] corners = path.corners;
								if (corners.Length > 0)
								{
									vector = corners[corners.Length - 1];
								}
							}
						}
						else
						{
							bool flag = false;
						}
					}
				}
				if (Vector3.Distance(vector, previousPosition) > 1E-05f)
				{
					m_effectPool[m_currentIndex].SetActive(true);
					m_indicatorPool[m_currentIndex].SetActive(true);
					m_effectPool[m_currentIndex].transform.position = vector + Vector3.up * 0.05f;
					m_indicatorPool[m_currentIndex].transform.position = vector + Vector3.up * 0.05f;
					m_indicatorPool[m_currentIndex].GetComponent<IndicatorEffect>().Play();
					m_effectPool[m_currentIndex].GetComponent<ParticleSystem>().Play();
					if (m_currentIndex == 0)
					{
						m_indicatorPool[m_poolSize - 1].GetComponent<IndicatorEffect>().Pause();
					}
					else
					{
						m_indicatorPool[m_currentIndex - 1].GetComponent<IndicatorEffect>().Pause();
					}
					if (m_currentIndex < m_poolSize - 1)
					{
						m_currentIndex++;
					}
					else
					{
						m_currentIndex = 0;
					}
					previousPosition = vector;
				}
			}
			else if (!m_AlreadyDisabled)
			{
				for (int i = 0; i < m_indicatorPool.Length; i++)
				{
					m_indicatorPool[i].GetComponent<IndicatorEffect>().Pause();
				}
				m_AlreadyDisabled = true;
			}
		}

		public void NotifyGoodPosition(Vector3 _position)
		{
			StartBillboardDecal(m_decalPositiveFeedback, _position);
		}

		private void StartBillboardDecal(GameObject _go, Vector3 _position)
		{
			_go.transform.localPosition = _position;
			ParticleSystem component = _go.GetComponent<ParticleSystem>();
			if (!component)
			{
				UnityEngine.Debug.LogError("ParticleSystem missing from decal: " + _go);
			}
			float startSize = component.startSize;
			startSize = Vector3.Distance(_go.transform.position, Camera.main.transform.position) / 10f;
			component.startSize = startSize;
			if (!component.isPlaying)
			{
				component.Play();
			}
		}

		public void NotifyInvalidPosition(Vector3 _position)
		{
			StartBillboardDecal(m_decalNegativeFeedback, _position);
		}

		public static void Create()
		{
			Instance = new MoveInterpreterDecals();
		}

		private void ClearPool()
		{
			for (int i = 0; i < m_indicatorPool.Length; i++)
			{
				Object.Destroy(m_indicatorPool[i]);
				Object.Destroy(m_effectPool[i]);
			}
			m_isSetUp = false;
		}

		public static void Destroy()
		{
			Instance.ClearPool();
			DestroyDecal(ref Instance.m_decalPositiveFeedback);
			DestroyDecal(ref Instance.m_decalNegativeFeedback);
			Instance = null;
		}

		private static void DestroyDecal(ref GameObject _go)
		{
			if (!(_go == null))
			{
				Object.DestroyImmediate(_go, true);
				_go = null;
			}
		}
	}
}
