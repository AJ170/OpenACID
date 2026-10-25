using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class PigeonryComponent : Component
	{
		private class PigeonContainer
		{
			private const int s_NumberOfTakeOffs = 3;

			private static readonly AnimatorStateMachineStatic.StateHashValue s_Idle = new AnimatorStateMachineStatic.StateHashValue("Base Layer.PigeonIdle");

			private static int s_TakeOffId = Animator.StringToHash("TakeOffID");

			public GameObject Locator { get; private set; }

			public GameObject Pigeon { get; private set; }

			public Animator Animator { get; private set; }

			private float TakeoffStartTime { get; set; }

			private float TakeoffOffsetTime { get; set; }

			private bool TakeoffStarted { get; set; }

			public PigeonContainer(GameObject _locator)
			{
				Locator = _locator;
				ItemDef item = Main.Instance.GameDB.GetItem<ItemDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_envInPigeonDynamic);
				Object @object = item.ItemPrefab.GetObject<Object>();
				Pigeon = Object.Instantiate(@object) as GameObject;
				ItemHelper.SetupLOD(Pigeon, 0.05f);
				Pigeon.transform.parent = _locator.transform;
				Pigeon.transform.localPosition = Vector3.zero;
				Pigeon.transform.localRotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
				Rendering.ConfigureLightProbes(Pigeon);
				Animator = Pigeon.GetComponent<Animator>();
				Animator.Play(s_Idle.NameHash, 0, Random.Range(0f, 1f));
			}

			public void Takeoff()
			{
				TakeoffStartTime = LogicMain.CurrentTime;
				TakeoffOffsetTime = LogicMain.Random.Rand(0f, 1f);
			}

			public void Update()
			{
				if (!TakeoffStarted && LogicMain.CurrentTime > TakeoffStartTime + TakeoffOffsetTime)
				{
					TakeoffStarted = true;
					Animator.SetInteger(s_TakeOffId, LogicMain.Random.Rand(1, 3));
				}
			}

			public void Reset()
			{
				Animator.SetInteger(s_TakeOffId, 0);
			}
		}

		private BBList<PigeonContainer> m_Pigeons = new BBList<PigeonContainer>();

		private float InteractionStartTime { get; set; }

		public override int Priority
		{
			get
			{
				return 26;
			}
		}

		public PigeonryComponent(IEntity _entity)
			: base(_entity)
		{
		}

		private void InitializePigeons()
		{
			GameObject gameObject = base.GameObjectEntity.GameObject.FindChildByName("PigeonLocators");
			if (gameObject != null)
			{
				for (int i = 0; i < gameObject.transform.childCount; i++)
				{
					m_Pigeons.Add(new PigeonContainer(gameObject.transform.GetChild(i).gameObject));
				}
			}
		}

		public override void OnActivate()
		{
			base.OnActivate();
			Rendering.ConfigureLightProbes(base.GameObjectEntity.GameObject);
			InitializePigeons();
		}

		public void OnStartInteraction()
		{
			InteractionStartTime = LogicMain.CurrentTime;
			for (int i = 0; i < m_Pigeons.Count; i++)
			{
				m_Pigeons[i].Takeoff();
			}
		}

		public void Update()
		{
			for (int i = 0; i < m_Pigeons.Count; i++)
			{
				m_Pigeons[i].Update();
			}
		}

		public void OnEndInteraction()
		{
			for (int i = 0; i < m_Pigeons.Count; i++)
			{
				m_Pigeons[i].Reset();
				m_Pigeons[i].Pigeon.SetActive(false);
			}
		}
	}
}
