using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

namespace Assets.Scripts.Crowd
{
	public class CrowdBehaviorRuntimeData : TreeRuntimeData
	{
		public CrowdBehavior CrowdBehavior { get; private set; }

		public void Init(CrowdBehavior _crowdBehavior)
		{
			CrowdBehavior = _crowdBehavior;
			Init();
		}

		public override void Init()
		{
			base.Init();
		}
	}
}
