using Assets.Scripts.Mission.Logic;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Motion.Hotspots
{
	public class SpotInspectionManager
	{
		private readonly BBDictionary<EntityId, EntityId> m_spotInspectedBy = new BBDictionary<EntityId, EntityId>();

		public void RegisterSpotInspector(EntityId _spotId, EntityId _inspector)
		{
			m_spotInspectedBy[_spotId] = _inspector;
		}

		public void DeregisterSpotInspector(EntityId _spotId, EntityId _inspector)
		{
			m_spotInspectedBy.Remove(_spotId);
		}

		public bool GetSpotInspector(EntityId _spotId, out EntityId _inspector)
		{
			if (m_spotInspectedBy.ContainsKey(_spotId))
			{
				_inspector = m_spotInspectedBy[_spotId];
				return true;
			}
			_inspector = EntityId.InvalidId;
			return false;
		}
	}
}
