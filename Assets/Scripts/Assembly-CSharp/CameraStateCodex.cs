using System.Collections.Generic;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Camera;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.UI;
using UbiCore.Utils;
using UnityEngine;

public class CameraStateCodex : CameraStateBase
{
	private const string LocatorName = "Cam_Locator";

	private Animation m_animation;

	private CodexEntry m_codexEntry;

	private float m_codexExitTime;

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.Codex;
		}
	}

	public override void CopyFrom(ICameraState _src)
	{
	}

	public override void StartState(ICameraState _previous)
	{
		base.StartState(_previous);
		EntityId currentInteractionObject = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.CurrentInteractionObject;
		GameObject gameObject = currentInteractionObject.GameEntity.GameObject;
		Transform transform = gameObject.transform.FindChildRecursive("Cam_Locator");
		InteractableObject entity = currentInteractionObject.GetEntity<InteractableObject>();
		if (entity is Codex)
		{
			m_codexEntry = entity.Cast<Codex>().CodexEntry;
		}
		if (m_codexEntry == null)
		{
		}
		m_codexExitTime = LogicMain.CurrentTime + 3f;
		if (transform != null)
		{
			m_animation = transform.GetComponent<Animation>();
			if (m_animation != null)
			{
				List<string> list = new List<string>();
				foreach (AnimationState item in m_animation)
				{
					list.Add(item.name);
				}
				if (list.Count > 0)
				{
					m_animation.Play(list[0]);
				}
				for (int i = 1; i < list.Count; i++)
				{
					m_animation.PlayQueued(list[i]);
				}
				m_animation.Sample();
				m_outputFieldOfView = transform.localScale.x;
			}
			m_outputPosition = transform.position;
			m_outputRotation = transform.rotation;
		}
		else if (_previous != null)
		{
			m_outputPosition = _previous.CurrentPosition;
			m_outputRotation = _previous.CurrentRotation;
		}
		else
		{
			m_outputPosition = base.transform.position;
			m_outputRotation = base.transform.rotation;
		}
		BarrierManager barrierManager = UnityEngine.Object.FindObjectOfType<BarrierManager>();
		if (barrierManager != null)
		{
			barrierManager.SetVisible(false);
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (m_animation != null)
		{
			m_outputPosition = m_animation.transform.position;
			m_outputRotation = m_animation.transform.rotation;
			float x = m_animation.transform.localScale.x;
			m_outputFieldOfView = ((!(x > 1f)) ? m_outputFieldOfView : x);
			if (!m_animation.isPlaying)
			{
				m_codexEntry.OnEndInteraction();
			}
		}
		else if (LogicMain.CurrentTime > m_codexExitTime)
		{
			m_codexEntry.OnEndInteraction();
		}
	}

	public override void EndState()
	{
		BarrierManager barrierManager = UnityEngine.Object.FindObjectOfType<BarrierManager>();
		if (barrierManager != null)
		{
			barrierManager.SetVisible(true);
		}
	}
}
