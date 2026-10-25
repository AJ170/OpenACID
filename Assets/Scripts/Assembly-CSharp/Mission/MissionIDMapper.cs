using System.Collections.Generic;
using Assets.Scripts.Mission.Mission.Interfaces;

namespace Mission
{
	public class MissionIDMapper
	{
		private SortedDictionary<long, IMissionBase> m_mappingIdInstance = new SortedDictionary<long, IMissionBase>();

		public void Register(IMissionBase _missionObject)
		{
			if (_missionObject.MissionData != null)
			{
				m_mappingIdInstance[_missionObject.MissionData.ID] = _missionObject;
			}
		}

		public IMissionBase GetMissionObject(long _id)
		{
			IMissionBase value;
			if (m_mappingIdInstance.TryGetValue(_id, out value))
			{
				return value;
			}
			return null;
		}

		public void Reset()
		{
			m_mappingIdInstance.Clear();
		}
	}
}
