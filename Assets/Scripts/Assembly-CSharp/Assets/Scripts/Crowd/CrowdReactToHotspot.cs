using Assets.Scripts.Animation;
using UnityEngine;

namespace Assets.Scripts.Crowd
{
	public class CrowdReactToHotspot : MonoBehaviour
	{
		public static float Cooldown = 10f;

		[HideInInspector]
		public CrowdBehavior CrowdBehavior;

		public float MinEnterTime;

		public float LeaveTime;

		public CrowdHotspotData Hotspot;

		public CrowdHotspotData NextHotspot;

		public void Reset()
		{
			if (Hotspot != null)
			{
				Animator animator = CrowdBehavior.Animator;
				if (Hotspot.TriggerReactionType == 14)
				{
					animator.SetCurrentStopAction(true);
				}
				animator.SetCurrentReactionType(ReactionType.Invalid);
				Hotspot = null;
				NextHotspot = null;
			}
		}

		public void PlayHotspotReactionInit()
		{
			if (!(LogicMain.CurrentTime > MinEnterTime) || CrowdBehavior.CrowdFollowCrowdGraph.NodeData == null)
			{
				return;
			}
			Animator animator = CrowdBehavior.Animator;
			CrowdHotspotData hotspot = CrowdBehavior.CrowdFollowCrowdGraph.NodeData.Hotspot;
			if (hotspot == null)
			{
				return;
			}
			float magnitude = (CrowdBehavior.Position - hotspot.Position).magnitude;
			if (magnitude < hotspot.TriggerRadius)
			{
				if (NextHotspot == null || (Hotspot != null && NextHotspot != Hotspot))
				{
					NextHotspot = hotspot;
					MinEnterTime = LogicMain.CurrentTime + Random.value * 1f;
					return;
				}
				animator.SetCurrentReactionType((ReactionType)hotspot.TriggerReactionType);
				Hotspot = hotspot;
				MinEnterTime = LogicMain.CurrentTime + hotspot.TriggerDuration + Cooldown;
				LeaveTime = LogicMain.CurrentTime + hotspot.TriggerDuration;
			}
		}

		public bool PlayHotspotReactionUpdate()
		{
			if (LogicMain.CurrentTime > LeaveTime)
			{
				Reset();
				return false;
			}
			return true;
		}
	}
}
