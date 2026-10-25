using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Motion.Hotspots;
using UbiCore.Messenger;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace UbiCore.Acier
{
	public class InteractWithObjectInterpreter : IconInterpreter
	{
		private Plane m_dragPlane = new Plane(Vector3.up, 0f);

		public override bool OnSecondTap(TapEvent _event)
		{
			return OnTap(_event);
		}

		public override bool OnTap(TapEvent _event)
		{
			bool flag = base.OnTap(_event);
			if (!flag)
			{
				flag = EvaluateGetOutTap(_event.Position);
			}
			return flag;
		}

		public override IconMaterial GetDefaultActionIcon()
		{
			IconMaterial defaultActionIcon = base.GetDefaultActionIcon();
			if (defaultActionIcon != 0)
			{
				return defaultActionIcon;
			}
			return IconMaterial.Invalid;
		}

		public override bool OnTriggerDefaultAction()
		{
			if (base.OnTriggerDefaultAction())
			{
				return true;
			}
			return false;
		}

		public override bool OnDrag(DragEvent _event)
		{
			return base.OnDrag(_event);
		}

		public override bool OnSwipe(SwipeEvent _event)
		{
			return base.OnSwipe(_event);
		}

		public override bool OnTwist(TwistEvent _event)
		{
			return base.OnTwist(_event);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
		}

		private bool EvaluateGetOutTap(Vector2 _position)
		{
			NavMeshAgent navMeshAgent = IconInterpreter.NavMeshAgent;
			Vector3 position = navMeshAgent.transform.position;
			m_dragPlane.distance = 0f - position.y;
			Ray ray = Camera.main.ScreenPointToRay(_position);
			Vector3 _hitPoint;
			if (m_dragPlane.RayCast(ray, out _hitPoint))
			{
				return UbiCore.Utils.Singleton<HotspotService>.Instance.InteractableMissionObjectManager.LeaveInteraction(PlayerHelper.Entity, _hitPoint);
			}
			return false;
		}
	}
}
