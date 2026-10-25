using System;
using System.Collections;
using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Database;
using Humanoids;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.CharacterPooling
{
	public class DefaultCharacterManager : ICharacterManager
	{
		private enum EDistances
		{
			DeadOffset = 40,
			Cutoff = 50,
			Cutoff_Sq = 2500
		}

		public class CacheEntry
		{
			private bool m_attached;

			private ObjectFade m_fadeCmp;

			public GameObject Instance { get; internal set; }

			public bool Attached
			{
				get
				{
					return m_attached;
				}
				set
				{
					if (value)
					{
						if (Instance == null)
						{
							Instance = SpawnHelper.CreateNpc(EntityData);
							Renderers = Instance.GetComponentsInChildren<Renderer>(true);
							AnimationController = Instance.GetComponent<AnimationController>();
							NavMeshAgent = Instance.GetComponent<NavMeshAgent>();
							Rendering.Instance.OnRegisterShadows(Instance);
						}
						else
						{
							NpcData.INpcDefinition definition = UbiCore.Utils.Singleton<NpcData>.Instance.GetDefinition(NpcType);
							HumanoidDef item = Main.Instance.GameDB.GetItem<HumanoidDef>(definition.HumanoidGameID);
							Singleton<HumanoidFactory>.Instance.AdjustAppearance(item, EntityData.Entity.FindComponent<CharacterAppearance>(), Instance);
						}
						SetFade(true);
						int currentTurn = LogicMain.CurrentTurn;
						int num = EntityData.Entity.EntityStats.Get_int(StatTypes.EStatType.Character_BirthTurn);
						if (currentTurn - num < 10)
						{
							float num2 = EntityData.Entity.EntityStats.Get_float(StatTypes.EStatType.Entity_FadeDuration);
							if (num2 > 0f)
							{
								EFadeMode fadeMode = (EFadeMode)EntityData.Entity.EntityStats.Get_int(StatTypes.EStatType.Entity_FadeMode);
								m_fadeCmp.FadeMode = fadeMode;
								m_fadeCmp.FadeDuration = num2;
								m_fadeCmp.Reset(true, true);
							}
						}
						else
						{
							m_fadeCmp.FadeMode = EFadeMode.Simple_EFadeMode;
							m_fadeCmp.FadeDuration = 0.4f;
							m_fadeCmp.Reset(true, true);
						}
						UpdateWrapper();
					}
					else if ((bool)Instance)
					{
						Instance.GetComponent<EntityWrapperComponent>().Entity = null;
						Fade = null;
					}
					if (m_attached != value)
					{
						AnimatorSoundStart component = Instance.GetComponent<AnimatorSoundStart>();
						if (value)
						{
							component.Attach();
						}
						else
						{
							component.Detach();
						}
					}
					m_attached = value;
				}
			}

			public AnimationController AnimationController { get; internal set; }

			public Renderer[] Renderers { get; internal set; }

			public NavMeshAgent NavMeshAgent { get; internal set; }

			public LogicAdapter.EntityData EntityData { get; internal set; }

			public NpcData.NpcType NpcType { get; private set; }

			public ObjectFade Fade
			{
				get
				{
					return m_fadeCmp;
				}
				set
				{
					if (m_fadeCmp != null)
					{
						UnityEngine.Object.Destroy(m_fadeCmp, (!m_fadeCmp.FadeDone) ? m_fadeCmp.FadeDuration : 0f);
					}
				}
			}

			internal CacheEntry(LogicAdapter.EntityData _ed)
			{
				EntityData = _ed;
				NpcType = _ed.NpcType;
			}

			public void SetFade(bool _fadeIn)
			{
				if (m_fadeCmp == null)
				{
					m_fadeCmp = Instance.AddComponentIfNotExist<ObjectFade>();
					m_fadeCmp.OnFadeDoneCallback = ObjectFadeCallbacks.OnFadeFinishedDestroySelf;
				}
				if (_fadeIn)
				{
					m_fadeCmp.FadeIn();
				}
				else
				{
					m_fadeCmp.FadeOut();
				}
			}

			internal void Clear()
			{
				Attached = false;
				if (Instance != null)
				{
					Rendering.Instance.OnUnregisterShadows(Instance);
					Singleton<HumanoidFactory>.Instance.DestroyHumanoid(Instance);
				}
				AnimationController = null;
				Instance = null;
				NavMeshAgent = null;
				Renderers = null;
				Fade = null;
			}

			private void UpdateWrapper()
			{
				GameObject instance = Instance;
				instance.SetActive(true);
				LogicAdapter.OnSetActive(instance, NavMeshAgent, true);
				instance.GetComponent<EntityWrapperComponent>().Entity = EntityData.Entity;
			}

			internal void Assign(LogicAdapter.EntityData _ed)
			{
				if (_ed.NpcType != NpcType)
				{
					Clear();
				}
				EntityData = _ed;
				NpcType = _ed.NpcType;
			}
		}

		private const float PrioScale_MissionNpc = 0.1f;

		private const float PrioScale_Attached = 0.5f;

		private const float PrioScale_AwareInteractionCharacter = 2f;

		private const float PrioScale_EnemyCharacter = 2f;

		private const float PrioScale_InteractionCharacter = 4f;

		private const float PrioScale_CrowdPenalty = 6f;

		private const bool StartAttached = false;

		private static int ourMaxVisibles = 10;

		private static int ourMaxCached = 15;

		private BBList<LogicAdapter.EntityData> m_WasVisible;

		private BBList<LogicAdapter.EntityData> m_IsVisible;

		private BBHashSet<LogicAdapter.EntityData> m_WasInFrustum;

		private BBHashSet<LogicAdapter.EntityData> m_IsInFrustum;

		private readonly BBList<CacheEntry> m_FadeOut = new BBList<CacheEntry>();

		private readonly BBList<CacheEntry> m_Active = new BBList<CacheEntry>();

		private readonly BBList<CacheEntry> m_Free = new BBList<CacheEntry>();

		private readonly BBList<LogicAdapter.EntityData> m_PotentialVisibles = new BBList<LogicAdapter.EntityData>();

		private readonly LogicAdapter.OrderByPriorityComparator m_Comparator = new LogicAdapter.OrderByPriorityComparator();

		private readonly BBList<LogicAdapter.EntityData> m_leftOvers = new BBList<LogicAdapter.EntityData>();

		private int MaxVisibles
		{
			get
			{
				return ourMaxVisibles;
			}
		}

		private int MaxCached
		{
			get
			{
				return ourMaxCached;
			}
		}

		public GameObject CharacterPoolNode { get; private set; }

		public ReadOnlyCollection<CacheEntry> ActiveCacheEntries
		{
			get
			{
				return m_Active.AsReadOnly();
			}
		}

		public DefaultCharacterManager()
		{
			m_WasVisible = new BBList<LogicAdapter.EntityData>();
			m_IsVisible = new BBList<LogicAdapter.EntityData>();
			m_WasInFrustum = new BBHashSet<LogicAdapter.EntityData>();
			m_IsInFrustum = new BBHashSet<LogicAdapter.EntityData>();
		}

		public static void SetupCharacterStats(int aCacheMaximum)
		{
			ourMaxVisibles = aCacheMaximum * 2 / 3;
			ourMaxCached = aCacheMaximum;
		}

		public static int CacheMaximum()
		{
			return ourMaxCached;
		}

		public void OnMissionLoad()
		{
			CharacterPoolNode = new GameObject("CharacterPool");
			LogicAdapter.OnMissionLoad();
			Main.Instance.StartCoroutine(Activator());
			LogicMain.EntityManager.OnEntityUnregistered += OnEntityUnregistered;
		}

		private void OnEntityUnregistered(IEntity _entity)
		{
			CacheEntry cacheEntry = m_Active.Find((CacheEntry entry) => entry.EntityData.Entity.Id == _entity.Id);
			if (cacheEntry != null && cacheEntry.Attached)
			{
				Detach(cacheEntry);
			}
		}

		private IEnumerator Activator()
		{
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (CharacterPoolNode != null)
			{
				for (int i = 0; i < m_Active.Count; i++)
				{
					if (!m_Active[i].Attached)
					{
						m_Active[i].Attached = true;
						break;
					}
				}
				yield return wait;
			}
		}

		public bool Update(float _dt)
		{
			Profiling.Start("DefaultCharacterManager::Update()", float.MaxValue);
			if (LogicAdapter.Active)
			{
				RefreshInFrustum();
				m_PotentialVisibles.Clear();
				m_PotentialVisibles.AddRange(m_IsInFrustum);
				m_PotentialVisibles.Sort(m_Comparator);
				SpawnDespawnEntities();
			}
			SwapLists();
			Profiling.Stop();
			return true;
		}

		public void OnMissionUnloaded()
		{
			LogicMain.EntityManager.OnEntityUnregistered -= OnEntityUnregistered;
			m_FadeOut.Clear();
			m_WasVisible.Clear();
			m_IsVisible.Clear();
			m_WasInFrustum.Clear();
			m_IsInFrustum.Clear();
			foreach (CacheEntry item in m_Active)
			{
				item.Clear();
			}
			m_Active.Clear();
			foreach (CacheEntry item2 in m_Free)
			{
				item2.Clear();
			}
			m_Free.Clear();
			UnityEngine.Object.Destroy(CharacterPoolNode);
			CharacterPoolNode = null;
			LogicAdapter.OnMissionUnload();
		}

		private float CalculateDistanceSq(LogicAdapter.EntityData _entity, Vector3 _position)
		{
			Vector3 position = _entity.Position;
			Vector3 vector = position - _position;
			vector.y = 0f;
			return vector.sqrMagnitude;
		}

		private void RefreshInFrustum()
		{
			Profiling.Start("DefaultCharacterManager::RefreshInFrustum()", float.MaxValue);
			int count = LogicAdapter.Characters.Count;
			Vector3 playerPosition = LogicAdapter.PlayerPosition;
			for (int i = 0; count > i; i++)
			{
				LogicAdapter.EntityData _entity_data = LogicAdapter.Characters[i];
				if (!_entity_data.Valid)
				{
					continue;
				}
				if (_entity_data.CanBePooled)
				{
					float num = CalculateDistanceSq(_entity_data, playerPosition);
					if (num < 2500f)
					{
						Vector3 normalized = (_entity_data.Position - AcierCameraController.Instance.LastCameraPosition).normalized;
						if (Vector3.Dot(normalized, AcierCameraController.Instance.LastCameraForward) > 0.1f)
						{
							CalculatePriority(ref _entity_data, num);
							m_IsInFrustum.Add(_entity_data);
						}
					}
				}
				else
				{
					_entity_data.Priority = 0f;
					m_IsInFrustum.Add(_entity_data);
				}
			}
			Profiling.Stop();
		}

		private static bool Contains(BBList<LogicAdapter.EntityData> _list, LogicAdapter.EntityData _data)
		{
			for (int i = 0; i < _list.Count; i++)
			{
				LogicAdapter.EntityData entityData = _list[i];
				if (entityData.Entity == _data.Entity)
				{
					return true;
				}
			}
			return false;
		}

		private void SpawnDespawnEntities()
		{
			Profiling.Start("DefaultCharacterManager::SpawnDespawnEntities()", float.MaxValue);
			m_IsVisible.AddRange(m_PotentialVisibles.GetRange(0, Math.Min(m_PotentialVisibles.Count, MaxVisibles)));
			int num = m_FadeOut.Count;
			int num2 = 0;
			while (num > num2)
			{
				Profiling.Start("Update Fadeout", float.MaxValue);
				CacheEntry cacheEntry = m_FadeOut[num2];
				if (Contains(m_IsVisible, cacheEntry.EntityData))
				{
					cacheEntry.SetFade(true);
					m_FadeOut.RemoveAt(num2);
					num--;
				}
				else if (cacheEntry.Fade == null || cacheEntry.Fade.FadeDone)
				{
					m_WasVisible.Remove(cacheEntry.EntityData);
					Detach(cacheEntry);
					num--;
				}
				else
				{
					num2++;
				}
				Profiling.Stop();
			}
			num = m_WasVisible.Count;
			for (int i = 0; num > i; i++)
			{
				LogicAdapter.EntityData ed = m_WasVisible[i];
				CacheEntry cacheEntry2 = m_Active.Find((CacheEntry entry) => entry.EntityData == ed);
				if (cacheEntry2 == null || !cacheEntry2.Attached || Contains(m_IsVisible, ed))
				{
					continue;
				}
				Profiling.Start("Despawn", float.MaxValue);
				if (m_WasInFrustum.Contains(ed))
				{
					if (!m_FadeOut.Contains(cacheEntry2))
					{
						cacheEntry2.SetFade(false);
						m_FadeOut.Add(cacheEntry2);
					}
				}
				else
				{
					Detach(cacheEntry2);
				}
				Profiling.Stop();
			}
			m_leftOvers.ClearIfNotEmpty();
			num = m_IsVisible.Count;
			for (int j = 0; num > j; j++)
			{
				LogicAdapter.EntityData ed2 = m_IsVisible[j];
				CacheEntry cacheEntry3 = m_Active.Find((CacheEntry entry) => entry.EntityData == ed2);
				if (cacheEntry3 == null)
				{
					Profiling.Start("Spawn", float.MaxValue);
					if (!Spawn(ed2, false))
					{
						m_leftOvers.Add(ed2);
					}
					Profiling.Stop();
				}
				else
				{
					m_FadeOut.Remove(cacheEntry3);
					Profiling.Start("Checking Fade In", float.MaxValue);
					cacheEntry3.Fade = null;
					Profiling.Stop();
				}
			}
			if (m_leftOvers.Count > 0)
			{
				Profiling.Start("DefaultCharacterManager::SpawnDespawnEntities()-Spawn 2nd pass", float.MaxValue);
				for (int k = 0; k < m_leftOvers.Count; k++)
				{
					Spawn(m_leftOvers[k], true);
				}
				Profiling.Stop();
			}
			Profiling.Stop();
		}

		private void SetEntryActive(CacheEntry _entry, LogicAdapter.EntityData _data)
		{
			_entry.Assign(_data);
			_entry.Attached = false;
			m_Active.Add(_entry);
		}

		private bool Spawn(LogicAdapter.EntityData _ed, bool _clear)
		{
			NpcData.NpcType npcType = _ed.NpcType;
			CacheEntry cacheEntry = m_Free.Find((CacheEntry x) => x.NpcType == _ed.NpcType);
			if (cacheEntry != null)
			{
				m_Free.Remove(cacheEntry);
				SetEntryActive(cacheEntry, _ed);
			}
			else if (m_Active.Count + m_Free.Count < MaxCached)
			{
				cacheEntry = new CacheEntry(_ed);
				SetEntryActive(cacheEntry, _ed);
			}
			else if (_clear && 0 < m_Free.Count)
			{
				cacheEntry = m_Free[0];
				m_Free.RemoveAt(0);
				SetEntryActive(cacheEntry, _ed);
			}
			return cacheEntry != null;
		}

		private void Detach(CacheEntry _entry)
		{
			m_FadeOut.Remove(_entry);
			_entry.Fade = null;
			LogicAdapter.OnSetActive(_entry.Instance, _entry.NavMeshAgent, false);
			m_Free.Add(_entry);
			m_Active.Remove(_entry);
			_entry.Attached = false;
		}

		private void SwapLists()
		{
			BBHashSet<LogicAdapter.EntityData> wasInFrustum = m_WasInFrustum;
			m_WasInFrustum = m_IsInFrustum;
			m_IsInFrustum = wasInFrustum;
			BBList<LogicAdapter.EntityData> wasVisible = m_WasVisible;
			m_WasVisible = m_IsVisible;
			m_IsVisible = wasVisible;
			m_IsInFrustum.Clear();
			m_IsVisible.Clear();
		}

		private void CalculatePriority(ref LogicAdapter.EntityData _entity_data, float _distance_sq)
		{
			_entity_data.Priority = Mathf.Sqrt(_distance_sq);
			Character character = _entity_data.Entity.Cast<Character>();
			if (character.GetIsDead())
			{
				_entity_data.Priority += 40f;
			}
			if (LogicMain.PlayerEntity.Vision.IsRelevant(_entity_data.Entity.Id))
			{
				_entity_data.Priority *= 0.1f;
			}
			if (character.IsAttached)
			{
				_entity_data.Priority *= 0.5f;
			}
			switch (_entity_data.EntityType)
			{
			case EntityTypes.EType.EnemyCharacter:
				_entity_data.Priority *= 2f;
				break;
			case EntityTypes.EType.InteractionCharacter:
				_entity_data.Priority *= 4f;
				break;
			case EntityTypes.EType.AwareInteractionCharacter:
				_entity_data.Priority *= 2f;
				break;
			case EntityTypes.EType.CrowdCharacter:
				_entity_data.Priority *= 6f;
				break;
			}
		}
	}
}
