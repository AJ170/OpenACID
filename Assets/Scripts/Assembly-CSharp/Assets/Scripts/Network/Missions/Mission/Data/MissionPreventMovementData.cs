using System;
using AcierProtocol;
using ProtoBuf;
using UnityEngine;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public class MissionPreventMovementData : MissionBaseData
	{
		[SerializeField]
		private PreventMovementExceptionTypes m_PreventMovementException;

		public PreventMovementExceptionTypes PreventMovementException
		{
			get
			{
				return m_PreventMovementException;
			}
			private set
			{
				m_PreventMovementException = value;
			}
		}

		public MissionPreventMovementData()
		{
		}

		public MissionPreventMovementData(MissionPreventMovementContent _missionPreventMovementContent)
			: base(_missionPreventMovementContent.MissionBaseContent)
		{
			PreventMovementException = _missionPreventMovementContent.PreventMovementException;
		}

		public override IExtensible SerializeProto()
		{
			MissionPreventMovementContent missionPreventMovementContent = new MissionPreventMovementContent();
			missionPreventMovementContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			missionPreventMovementContent.PreventMovementException = PreventMovementException;
			return missionPreventMovementContent;
		}
	}
}
