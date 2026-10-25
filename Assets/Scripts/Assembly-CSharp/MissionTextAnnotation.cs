using System.Collections;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using UnityEngine;

public class MissionTextAnnotation : MissionBase<MissionTextAnnotationData>
{
	private const string TextureBasePath = "UI/Texture/Tutorial/";

	private int m_uniqueInfoId;

	private ResourceRequest m_resourceRequest;

	private InfoEvent.ETextAnnotationMode m_openMode;

	private InfoEvent.ETextAnnotationMode m_closeMode;

	public override void OnActivate()
	{
		base.OnActivate();
		switch (base.Data.Type)
		{
		default:
			m_openMode = InfoEvent.ETextAnnotationMode.MessageOpen;
			m_closeMode = InfoEvent.ETextAnnotationMode.Invalid;
			break;
		case EMissionTextAnnotationType.MessageExternal:
			m_openMode = InfoEvent.ETextAnnotationMode.MessageOpenExternal;
			m_closeMode = InfoEvent.ETextAnnotationMode.MessageCloseExternal;
			break;
		case EMissionTextAnnotationType.PopupConfirm:
			m_openMode = InfoEvent.ETextAnnotationMode.PopupOpenConfirm;
			m_closeMode = InfoEvent.ETextAnnotationMode.Invalid;
			break;
		case EMissionTextAnnotationType.PopupExternal:
			m_openMode = InfoEvent.ETextAnnotationMode.PopupOpenExternal;
			m_closeMode = InfoEvent.ETextAnnotationMode.PopupCloseExternal;
			break;
		case EMissionTextAnnotationType.Image:
			m_openMode = InfoEvent.ETextAnnotationMode.ImageOpen;
			m_closeMode = InfoEvent.ETextAnnotationMode.Invalid;
			break;
		case EMissionTextAnnotationType.ImageExternal:
			m_openMode = InfoEvent.ETextAnnotationMode.ImageOpenExternal;
			m_closeMode = InfoEvent.ETextAnnotationMode.ImageCloseExternal;
			break;
		}
		if (m_resourceRequest != null && !m_resourceRequest.isDone)
		{
			StartCoroutine(WaitForResource());
		}
		else
		{
			SendInfoEvent();
		}
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		if (m_closeMode != 0)
		{
			LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, -1, m_closeMode, 0, null, m_uniqueInfoId);
		}
	}

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		base.OnPostRead(_nodeData);
		m_resourceRequest = null;
		if ((base.Data.Type == EMissionTextAnnotationType.Image || base.Data.Type == EMissionTextAnnotationType.ImageExternal) && !string.IsNullOrEmpty(base.Data.Texture))
		{
			m_resourceRequest = UnityEngine.Resources.LoadAsync<Texture2D>("UI/Texture/Tutorial/" + base.Data.Texture);
		}
	}

	private IEnumerator WaitForResource()
	{
		yield return m_resourceRequest;
		SendInfoEvent();
	}

	private void SendInfoEvent()
	{
		Texture texture = null;
		if (m_resourceRequest != null)
		{
			texture = m_resourceRequest.asset as Texture;
		}
		m_uniqueInfoId = LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, base.Data.TextId, m_openMode, base.Data.HeaderTextId, texture, null, true);
	}
}
