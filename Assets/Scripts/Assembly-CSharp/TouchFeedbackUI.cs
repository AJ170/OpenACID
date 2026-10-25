using System;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Ingame;
using UbiCore.Acier;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

public class TouchFeedbackUI : BaseUI
{
	protected TouchFeedbackBehavior TouchFeedbackBehavior;

	private SpriteControlDefinition m_touchFeedbackDefinition;

	private AcierInputHandler m_inputHandler;

	private float m_stickMaxDistance;

	public override void OnMissionLoaded()
	{
		base.OnMissionLoaded();
		if (IsActive)
		{
			GameObject buttonTemplate = Resources.Load<GameObject>("UI/TouchFeedback/TouchFeedback");
			float scale = UbiCore.Utils.Singleton<UiMain>.Instance.GetScale();
			m_touchFeedbackDefinition = SpriteUtils.ConstructControl<SpriteControlDefinition, TouchFeedbackBehavior>(buttonTemplate, scale, base.UICamera);
			TouchFeedbackBehavior = m_touchFeedbackDefinition.SpriteBehaviour as TouchFeedbackBehavior;
			TouchFeedbackBehavior.Init();
			AddSprite(m_touchFeedbackDefinition);
		}
	}

	public override void UpdateFrame(float _deltaTime)
	{
		UpdateTransformations();
		Update();
	}

	public override void OnMissionUnloaded()
	{
		base.OnMissionUnloaded();
		m_touchFeedbackDefinition = null;
	}

	protected override void FixLayout()
	{
	}

	public override bool Tap(TapEvent _gesture)
	{
		return false;
	}

	public void Update()
	{
		m_inputHandler = AcierInputHandler.Instance;
		if (m_inputHandler == null || !m_inputHandler.EnableTouchFeedback || m_inputHandler.ActiveInterpreter == null)
		{
			TouchFeedbackBehavior.SetVisible(false);
			return;
		}
		TouchFeedbackBehavior.SetVisible(false);
		for (int i = 0; i < Math.Min(m_inputHandler.TouchDataCount, 2); i++)
		{
			TouchData touchData = m_inputHandler.GetTouchData(i);
			m_stickMaxDistance = m_inputHandler.StickMaxDistance;
			TouchFeedbackBehavior.UpdateControl(touchData, i, m_inputHandler.StickMinDistance, m_stickMaxDistance, m_inputHandler.TapMaxDuration);
		}
	}
}
