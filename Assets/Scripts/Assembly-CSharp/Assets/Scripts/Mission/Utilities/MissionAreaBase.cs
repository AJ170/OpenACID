using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Utilities
{
	public abstract class MissionAreaBase<TData> : MissionBase<TData>, IMissionBase, IMissionAreaBase where TData : MissionAreaBaseData, new()
	{
		public MissionAreaBaseData AreaData
		{
			get
			{
				return base.Data;
			}
		}

		public abstract bool IsInside(Vector3 _checkPosition);

		public virtual void OnDrawGizmosSelected()
		{
		}

		public override void OnPreActivate()
		{
			base.OnPreActivate();
			UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.Register(this);
		}

		public override void OnDeactivate()
		{
			UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.Unregister(this);
			base.OnDeactivate();
		}
	}
}
