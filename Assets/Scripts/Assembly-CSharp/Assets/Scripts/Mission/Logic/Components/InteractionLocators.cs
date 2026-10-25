using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assets.Scripts.Mission.Motion;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class InteractionLocators : Component, IComponent, IIconPositionCarrier
	{
		public class Locator
		{
			public bool Enabled
			{
				get
				{
					bool result = false;
					if (Position != null)
					{
						result = Position.activeSelf;
					}
					return result;
				}
			}

			public GameObject Position { get; private set; }

			public GameObject Step { get; private set; }

			public Quaternion Orientation { get; private set; }

			public Plane Plane { get; private set; }

			public float MaxOffset { get; internal set; }

			public Locator(GameObject _position, GameObject _step, GameObject _center)
			{
				Position = _position;
				Step = _step;
				if (Step != null)
				{
					Vector3 forward = Step.transform.position - Position.transform.position;
					forward.y = 0f;
					forward.Normalize();
					Orientation = Quaternion.LookRotation(forward);
					if (_center != null)
					{
						Vector3 up = Vector3.up;
						up = Step.transform.position - _center.transform.position;
						up.y = 0f;
						up.Normalize();
						Plane = new Plane(up, Step.transform.position);
					}
				}
			}
		}

		public static string LocatorRootName = "Locators";

		private GameObject m_Base;

		private GameObject m_Center;

		private GameObject m_Rest;

		private readonly BBList<Locator> m_Locators = new BBList<Locator>();

		private readonly ReadOnlyCollection<Locator> m_LocatorsAccess;

		public ReadOnlyCollection<Locator> Locators
		{
			get
			{
				return m_LocatorsAccess;
			}
		}

		public GameObject Rest
		{
			get
			{
				return m_Rest;
			}
			private set
			{
				m_Rest = value;
			}
		}

		public GameObject Base
		{
			get
			{
				return m_Base;
			}
			private set
			{
				m_Base = value;
			}
		}

		public GameObject Center
		{
			get
			{
				return m_Center;
			}
			private set
			{
				m_Center = value;
			}
		}

		public override int Priority
		{
			get
			{
				return 15;
			}
		}

		public Vector3 IconPosition
		{
			get
			{
				return m_Center.transform.position;
			}
		}

		public InteractionLocators(IEntity _entity)
			: base(_entity)
		{
			m_LocatorsAccess = new ReadOnlyCollection<Locator>(m_Locators);
		}

		public float GetSimpleDistanceToTarget(GameObject _target)
		{
			float num = float.MaxValue;
			Vector3 position = _target.transform.position;
			for (int i = 0; i < m_Locators.Count; i++)
			{
				float magnitude = (position - m_Locators[i].Position.transform.position).magnitude;
				if (magnitude > 0f)
				{
					num = Mathf.Min(num, magnitude);
				}
			}
			return num;
		}

		public Locator GetLocatorByPosition(Vector3 _pos)
		{
			Locator _locator = null;
			GetDistanceToTarget(_pos, out _locator);
			return _locator;
		}

		public float GetDistanceToTarget(GameObject _target)
		{
			Locator _locator = null;
			return GetDistanceToTarget(_target, out _locator);
		}

		public float GetDistanceToTarget(GameObject _target, out Locator _locator)
		{
			return GetDistanceToTarget(_target.transform.position, out _locator);
		}

		public float GetDistanceToTarget(Vector3 _pos, out Locator _locator)
		{
			float num = float.MaxValue;
			_locator = null;
			for (int i = 0; i < m_Locators.Count; i++)
			{
				float distanceToPoint = m_Locators[i].Plane.GetDistanceToPoint(_pos);
				if (distanceToPoint > 0f && distanceToPoint < num)
				{
					num = distanceToPoint;
					_locator = m_Locators[i];
				}
			}
			return num;
		}

		public override void OnActivate()
		{
			InitializeLocators();
		}

		public bool ChooseClosestLocator(Vector3 _position, int _layerMask, out Locator _locator, out NavMeshPath _path)
		{
			List<Locator> list = Locators.OrderBy((Locator loc) => Vector3.Distance(loc.Position.transform.position, _position), (float d1, float d2) => d1.CompareTo(d2));
			_path = new NavMeshPath();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Enabled && NavMesh.CalculatePath(_position, list[i].Position.transform.position, _layerMask, _path) && _path.status == NavMeshPathStatus.PathComplete)
				{
					_locator = list[i];
					return true;
				}
			}
			_locator = null;
			return false;
		}

		public bool ChooseClosestLocatorTryDirect(Vector3 _position, int _layerMask, Vector3 _assassin, out Locator _locator, out NavMeshPath _path)
		{
			List<Locator> list = Locators.OrderBy((Locator loc) => Vector3.Distance(loc.Position.transform.position, _position), (float d1, float d2) => d1.CompareTo(d2));
			_path = new NavMeshPath();
			Locator locator = null;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Enabled && NavMesh.CalculatePath(_position, list[i].Position.transform.position, _layerMask, _path) && _path.status == NavMeshPathStatus.PathComplete)
				{
					if (ParcourHelper.CanUseLocatorDirectly(_assassin, list[i]))
					{
						_locator = list[i];
						return true;
					}
					if (locator == null)
					{
						locator = list[i];
					}
				}
			}
			_locator = locator;
			return locator != null;
		}

		private void InitializeLocators()
		{
			Base = GameObject.FindChildByName("Center");
			Center = GameObject.FindChildByName("Middle");
			Rest = GameObject.FindChildByName("Rest");
			GameObject gameObject = GameObject.FindChildByName(LocatorRootName);
			if (gameObject != null)
			{
				for (int i = 0; i < gameObject.transform.childCount; i++)
				{
					Transform child = gameObject.transform.GetChild(i);
					if (!(child != null) || !(child.gameObject != null))
					{
						continue;
					}
					GameObject gameObject2 = child.gameObject;
					GameObject step = Center;
					for (int j = 0; j < gameObject2.transform.childCount; j++)
					{
						Transform child2 = gameObject2.transform.GetChild(j);
						if (child2 != null && child2.gameObject != null)
						{
							step = child2.gameObject;
							break;
						}
					}
					m_Locators.Add(new Locator(gameObject2, step, Center));
				}
			}
			for (int k = 0; k < m_Locators.Count; k++)
			{
				Vector3 position = m_Locators[k].Step.transform.position;
				Vector3 rhs = Vector3.Cross(m_Locators[k].Plane.normal, Vector3.up);
				float num = 0f;
				float num2 = 0f;
				for (int l = 0; l < m_Locators.Count; l++)
				{
					if (k != l)
					{
						Vector3 lhs = m_Locators[l].Step.transform.position - position;
						float b = Vector3.Dot(lhs, rhs);
						num = Mathf.Min(num, b);
						num2 = Mathf.Max(num2, b);
					}
				}
				m_Locators[k].MaxOffset = Mathf.Min(0f - num, num2);
			}
		}
	}
}
