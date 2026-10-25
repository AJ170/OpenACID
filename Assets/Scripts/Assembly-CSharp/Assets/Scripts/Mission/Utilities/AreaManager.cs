using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.Mission.Interfaces;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Utilities
{
	public class AreaManager
	{
		public delegate void DirtyFlagDelegate();

		private readonly BBList<IMissionAreaBase> m_restrictedAreas = new BBList<IMissionAreaBase>();

		public event DirtyFlagDelegate OnDirtyFlagSet;

		public BBList<IMissionAreaBase> GetAreas()
		{
			BBList<IMissionAreaBase> bBList = new BBList<IMissionAreaBase>();
			for (int i = 0; i < m_restrictedAreas.Count; i++)
			{
				IMissionAreaBase missionAreaBase = m_restrictedAreas[i];
				if (missionAreaBase != null)
				{
					bBList.Add(missionAreaBase);
				}
			}
			return bBList;
		}

		public bool IsInArea(Vector3 _checkPosition, EMissionAreaType _areaType = EMissionAreaType.Restricted, bool hasToBeVisible = false)
		{
			for (int i = 0; i < m_restrictedAreas.Count; i++)
			{
				IMissionAreaBase missionAreaBase = m_restrictedAreas[i];
				if ((!hasToBeVisible || !missionAreaBase.AreaData.DoNotRenderOnMiniMap) && missionAreaBase.AreaData.Type == _areaType && missionAreaBase.IsInside(_checkPosition))
				{
					return true;
				}
			}
			return false;
		}

		public void Register(IMissionAreaBase _missionArea)
		{
			m_restrictedAreas.Add(_missionArea);
			InvokeOnAreasChanged();
		}

		public void Unregister(IMissionAreaBase _missionArea)
		{
			m_restrictedAreas.Remove(_missionArea);
			InvokeOnAreasChanged();
			if (_missionArea.IsDynamic)
			{
				UnityEngine.Object.Destroy((_missionArea as MonoBehaviour).gameObject);
			}
		}

		private void InvokeOnAreasChanged()
		{
			DirtyFlagDelegate onDirtyFlagSet = this.OnDirtyFlagSet;
			if (onDirtyFlagSet != null)
			{
				onDirtyFlagSet();
			}
		}

		public MissionArea CreateEscapeArea(Transform _parent, long _activateAtId, long _removeAtId)
		{
			GameObject gameObject = new GameObject("Escape area");
			gameObject.transform.parent = _parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			MissionArea missionArea = gameObject.AddComponent<MissionArea>();
			missionArea.Data.ActivateAtId = _activateAtId;
			missionArea.Data.RemoveAtId = _removeAtId;
			missionArea.Data.Type = EMissionAreaType.Escape_EMissionAreaType;
			missionArea.IsDynamic = true;
			return missionArea;
		}

		public MissionArea CreatePerimeter(Transform _parent, Vector3 _center, double _radius, long _activateAtId, long _removeAtId)
		{
			List<IMissionBase> list = new List<IMissionBase>();
			MissionArea missionArea = CreateEscapeArea(_parent, _activateAtId, _removeAtId);
			list.Add(missionArea);
			for (float num = 0f; (double)num < Math.PI * 2.0; num += (float)Math.PI / 10f)
			{
				AreaNode areaNode = missionArea.CreateNode();
				int num2 = (int)((double)_center.x + _radius * Math.Cos(num));
				int num3 = (int)((double)_center.z + _radius * Math.Sin(num));
				areaNode.transform.position = new Vector3(num2, _center.y, num3);
				list.Add(areaNode);
			}
			UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.AddMissionEntries(list);
			return missionArea;
		}

		public void DestroyPerimeter(MissionArea _perimeter)
		{
			MissionMonoBehaviour[] componentsInChildren = _perimeter.gameObject.GetComponentsInChildren<MissionMonoBehaviour>();
			UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.RemoveMissionEntries(componentsInChildren);
			UnityEngine.Object.Destroy(_perimeter.gameObject);
		}
	}
}
