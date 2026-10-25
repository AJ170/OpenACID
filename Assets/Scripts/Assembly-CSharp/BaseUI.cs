using System;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.UI.Ingame;
using UnityEngine;

public abstract class BaseUI : UISubSystem
{
	private float m_lastAspect;

	private float m_lastOrthographicSize;

	public override void UpdateFrame(float _deltaTime)
	{
		UpdateTransformations();
	}

	protected abstract void FixLayout();

	protected void UpdateTransformations()
	{
		if (!(Math.Abs(base.UICamera.orthographicSize - m_lastOrthographicSize) < float.Epsilon) || !(Math.Abs(base.UICamera.aspect - m_lastAspect) < float.Epsilon))
		{
			m_lastOrthographicSize = base.UICamera.orthographicSize;
			m_lastAspect = base.UICamera.aspect;
			FixLayout();
		}
	}

	protected void FixSpriteLayout(ISpriteControlDefinition _buttonDefinition)
	{
		FixSpriteLayout(_buttonDefinition, BorderOffset);
	}

	protected void FixSpriteLayout(ISpriteControlDefinition _buttonDefinition, Vector2 _borderOffset)
	{
		float _pixelWidth;
		Vector2 offset = ComputePosition(_buttonDefinition, ScreenAnchor, _borderOffset, out _pixelWidth);
		SpriteUtils.TransformPixelExact(_buttonDefinition, offset, _pixelWidth, base.UICamera);
	}

	private Vector2 ComputePosition(ISpriteControlDefinition _buttonDefinition, ScreenAnchor _screenAnchor, Vector2 _borderOffset, out float _pixelWidth)
	{
		if (_buttonDefinition == null)
		{
			_pixelWidth = 1f;
			return Vector2.zero;
		}
		ISpriteControlBehaviour spriteBehaviour = _buttonDefinition.SpriteBehaviour;
		float screenWidth = spriteBehaviour.ScreenWidth;
		float num = screenWidth * spriteBehaviour.Aspect;
		_pixelWidth = SpriteUtils.ComputePixelWidth(screenWidth, base.UICamera);
		float aspect = base.UICamera.aspect;
		float num2 = _borderOffset.x * 1f;
		if (_screenAnchor.HasFlag(Direction.Right))
		{
			num2 = 1f - screenWidth * 1f - num2;
		}
		else if (_screenAnchor.HasFlag(Direction.Middle))
		{
			num2 = (1f - screenWidth * 1f + num2) / 2f;
		}
		float num3 = _borderOffset.y * 1f;
		if (_screenAnchor.HasFlag(Direction.Top))
		{
			num3 = 1f / aspect - num * 1f - num3;
		}
		else if (_screenAnchor.HasFlag(Direction.Center))
		{
			num3 = (1f / aspect - num * 1f - num3) / 2f;
		}
		return new Vector2(num2, num3);
	}
}
