using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Motion.Hotspots
{
	public abstract class HotspotManager<T> : IHotspotManager where T : class, IEntity
	{
		protected readonly string m_rootNodeName;

		public string RootNodeName
		{
			get
			{
				return m_rootNodeName;
			}
		}

		protected HotspotManager(string _rootNodeName)
		{
			m_rootNodeName = _rootNodeName;
		}

		public virtual T GetEntity(GameObject _gameObject)
		{
			EntityWrapperComponent component = _gameObject.GetComponent<EntityWrapperComponent>();
			if (component != null)
			{
				if (component.Entity == null)
				{
					Transform parent = _gameObject.transform.parent;
					if (parent != null)
					{
						return GetEntity(parent.gameObject);
					}
				}
				return component.Entity as T;
			}
			Transform parent2 = _gameObject.transform.parent;
			if (parent2 != null)
			{
				return GetEntity(parent2.gameObject);
			}
			return (T)null;
		}

		public bool Interact(AssassinCharacter _assassin, GameObject _gameObject, NavMeshAgent _agent)
		{
			T entity = GetEntity(_gameObject);
			if (entity != null)
			{
				return Interact(_assassin, entity, _agent);
			}
			return false;
		}

		public virtual bool LeaveInteraction(AssassinCharacter _assassin, Vector3 _destination)
		{
			if (_assassin.PlayerMainCarrier.PlayerMain.LastStateChangeTurn >= LogicMain.CurrentTurn)
			{
				return false;
			}
			return _assassin.GetIsMovementAllowed();
		}

		public virtual void Initialize()
		{
			GameObject gameObject = GameObject.Find(m_rootNodeName);
			if (!(gameObject != null))
			{
				return;
			}
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				Transform child = gameObject.transform.GetChild(i);
				if (child != null)
				{
					GameObject gameObject2 = child.gameObject;
					if (gameObject2 != null)
					{
						CreateObject(gameObject2);
					}
				}
			}
		}

		protected abstract void CreateObject(GameObject _go);

		protected abstract bool Interact(AssassinCharacter _assassin, T _entity, NavMeshAgent _agent);

		protected bool InteractAction(NavMeshAgent navMeshAgent, IconAttachment iconAttachment)
		{
			T val = ((EntityIcon)iconAttachment).HeadIcon.Entity as T;
			return val != null && Interact(PlayerHelper.Entity, val, navMeshAgent);
		}
	}
}
