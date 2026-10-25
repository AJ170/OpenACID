using System.Collections.Generic;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.UI.Ingame.MiniMap;
using UbiCore.Debug;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIMiniMapBehaviour : UIBaseBehaviour
	{
		private UIMiniMapEntities m_miniMapEntities;

		private UIMiniMapMissionNodes m_miniMapMissionNodes;

		private UIMiniMapMovingAreas m_miniMapMovingAreas;

		public UICustomTexture Background;

		public UICustomTexture RestrictedOverlay;

		public GameObject EntityContainer;

		public GameObject EntityClippedContainer;

		[SerializeField]
		private GameObject m_mapMainContainer;

		private UIMapData m_uiMapData;

		public float m_visibleSize = 0.25f;

		public float CameraRotation { get; set; }

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.MiniMap;
			}
		}

		public void Init()
		{
			m_mapMainContainer.gameObject.SetActive(true);
		}

		public void OnMissionLoaded()
		{
			GameObject gameObject = GameObject.Find("EnvScenario/MapScenario");
			if (gameObject != null)
			{
				m_uiMapData = gameObject.GetComponent<UIMapData>();
				Object @object = Resources.Load("UI/Minimap/MiniMapEntity");
				m_miniMapEntities = new UIMiniMapEntities(EntityClippedContainer, @object, m_visibleSize);
				m_miniMapMissionNodes = new UIMiniMapMissionNodes(EntityContainer, @object, m_visibleSize);
				m_miniMapMovingAreas = new UIMiniMapMovingAreas(EntityClippedContainer, @object, m_visibleSize);
				m_miniMapEntities.OnMissionLoaded(m_uiMapData);
				m_miniMapMissionNodes.OnMissionLoaded(m_uiMapData);
				m_miniMapMovingAreas.OnMissionLoaded(m_uiMapData);
				AudioManager.Instance.EventManager.OnMissionLoadedUiMapData(m_uiMapData);
				Background.mainTexture = m_uiMapData.m_miniMap;
				OnAreasChanged();
			}
		}

		public void OnAreasChanged()
		{
			Texture2D texture2D = RestrictedOverlay.mainTexture as Texture2D;
			if (texture2D != null)
			{
				MissionAreaPlotter.Clear(texture2D);
			}
			List<IMissionAreaBase> areas = UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.GetAreas();
			for (int i = 0; i < areas.Count; i++)
			{
				MissionArea missionArea = areas[i] as MissionArea;
				if (!(missionArea == null) && !missionArea.Data.DoNotRenderOnMiniMap)
				{
					texture2D = ((!(texture2D == null)) ? MissionAreaPlotter.AddAreaOverlay(texture2D, missionArea.ComputeAreaNodePositions(), m_uiMapData, missionArea.FillColor[(int)missionArea.Data.Type], missionArea.OtherFillColor, missionArea.LineShift, missionArea.LineWidth, false) : MissionAreaPlotter.CreateAreaOverlay(missionArea.ComputeAreaNodePositions(), m_uiMapData, missionArea.FillColor[(int)missionArea.Data.Type], missionArea.OtherFillColor, missionArea.LineShift, missionArea.LineWidth));
				}
			}
			RestrictedOverlay.mainTexture = texture2D;
			RestrictedOverlay.depth = 1;
			RestrictedOverlay.gameObject.SetActive(texture2D != null);
		}

		public void OnMissionUnload()
		{
			m_uiMapData = null;
			m_miniMapEntities.OnMissionUnload();
			m_miniMapEntities = null;
			m_miniMapMissionNodes.OnMissionUnload();
			m_miniMapMissionNodes = null;
			m_miniMapMovingAreas.OnMissionUnload();
			m_miniMapMovingAreas = null;
		}

		public void UpdateFrame(float _deltaTime)
		{
			if (!(LogicMain.Player == null))
			{
				UpdateCameraRotation();
				Vector2 _mapSize = m_uiMapData.m_endPos - m_uiMapData.m_startPos;
				Vector2 _playerPosition = new Vector2((LogicMain.Player.transform.position.x - m_uiMapData.m_startPos.x) / _mapSize.x, (LogicMain.Player.transform.position.z - m_uiMapData.m_startPos.y) / _mapSize.y);
				Vector4 mapUV = new Vector4(_playerPosition.x - m_visibleSize / 2f, _playerPosition.y - m_visibleSize / 2f, m_visibleSize, m_visibleSize);
				UpdateMaterialRegion(Background, mapUV);
				UpdateMaterialRegion(RestrictedOverlay, mapUV);
				UIWidget component = base.gameObject.GetComponent<UIWidget>();
				Vector2 _bgSize = component.localSize;
				float bgRadius = _bgSize.x / 2f * 0.95f;
				Profiling.Start("m_miniMapEntities.Update", float.MaxValue);
				m_miniMapEntities.Update(ref _mapSize, ref _playerPosition, ref _bgSize, bgRadius, CameraRotation);
				Profiling.Stop();
				Profiling.Start("m_miniMapMissionNodes.Update", float.MaxValue);
				m_miniMapMissionNodes.Update(ref _mapSize, ref _playerPosition, ref _bgSize, bgRadius, CameraRotation);
				Profiling.Stop();
				Profiling.Start("m_miniMapMovingAreas.Update", float.MaxValue);
				m_miniMapMovingAreas.Update(ref _mapSize, ref _playerPosition, ref _bgSize, bgRadius);
				Profiling.Stop();
			}
		}

		private void UpdateMaterialRegion(UICustomTexture _texture, Vector4 _mapUV)
		{
			_texture.material.SetVector("_Region", _mapUV);
			UIDrawCall drawCall = _texture.drawCall;
			if (drawCall != null && drawCall.dynamicMaterial != null)
			{
				drawCall.dynamicMaterial.SetVector("_Region", _mapUV);
			}
		}

		private void UpdateCameraRotation()
		{
			if (AcierCameraController.Instance.CurrentStateType != AcierCameraController.ECameraState.Codex)
			{
				Vector3 forward = Camera.main.transform.forward;
				forward.y = 0f;
				forward.Normalize();
				CameraRotation = Vector3.Angle(forward, Vector3.forward) * Mathf.Sign(0f - forward.x);
				base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f - CameraRotation);
			}
		}
	}
}
