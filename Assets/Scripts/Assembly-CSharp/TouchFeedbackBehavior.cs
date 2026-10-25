using UbiCore.Acier;
using UbiCore.Utils;
using UnityEngine;

public class TouchFeedbackBehavior : UIControlBehaviour
{
	public static float GlobalScaleFactor = 2f;

	public SpriteRenderer[] TouchSprite;

	public SpriteRenderer[] SelectSprite;

	public SpriteRenderer[] StickDeadzoneSprite;

	public SpriteRenderer[] StickSizeSprite;

	public SpriteRenderer[] StickSprite;

	public void Init()
	{
		float virtualStickScale = Main.Instance.Startup.QualitySettings.virtualStickScale;
		if (virtualStickScale > float.Epsilon)
		{
			GlobalScaleFactor = virtualStickScale;
		}
		else if (Screen.dpi > 25f)
		{
			float num = (float)Mathf.Min(Screen.width, Screen.height) / Screen.dpi;
			float num2 = 5f;
			float num3 = 1f;
			float t = Mathf.Clamp01((num - num3) / (num2 - num3));
			GlobalScaleFactor = Mathf.Lerp(4f, 2f, t);
		}
	}

	public void SetVisible(bool _visible)
	{
		for (int i = 0; i < 2; i++)
		{
			TouchSprite[i].enabled = _visible;
			SelectSprite[i].enabled = _visible;
			StickDeadzoneSprite[i].enabled = _visible;
			StickSizeSprite[i].enabled = _visible;
			StickSprite[i].enabled = _visible;
		}
	}

	public void UpdateControl(TouchData _touch, int _index, float _stickMinDistance, float _stickMaxDistance, float _tapMaxDuration)
	{
		if (_index == 0)
		{
			UpdateStickFeedback(_touch, _index, _stickMinDistance, _stickMaxDistance, _tapMaxDuration);
		}
		else
		{
			UpdateCameraFeedback(_touch, _index, _stickMinDistance, _stickMaxDistance, _tapMaxDuration);
		}
	}

	public void UpdateStickFeedback(TouchData _touch, int _index, float _stickMinDistance, float _stickMaxDistance, float _tapMaxDuration)
	{
		switch (_touch.state)
		{
		case TouchData.ETouchState.None:
			break;
		case TouchData.ETouchState.Touch:
		{
			float a = (UbiCore.Utils.Singleton<Timer>.Instance.realTime - _touch.startTime) / _tapMaxDuration;
			UpdateTouchSprite(_color: new Color(1f, 1f, 1f, a), _position: _touch.startPosition, _stickInput: Vector2.zero, _sizeFactor: _stickMaxDistance / 1.875f, _sprite: TouchSprite[_index], _stickMinDistance: _stickMinDistance, _stickMaxDistance: _stickMaxDistance, layer: 0f);
			break;
		}
		case TouchData.ETouchState.Drag:
		{
			UpdateTouchSprite(_touch.stickBasePosition, Vector2.zero, _stickMaxDistance, Color.white, StickSizeSprite[_index], _stickMinDistance, _stickMaxDistance, 0f);
			float sqrMagnitude = _touch.stickInput.sqrMagnitude;
			if (sqrMagnitude > 0f)
			{
				UpdateTouchSprite(_touch.stickBasePosition, _touch.stickInput, _stickMaxDistance * 0.35f, Color.white, StickSprite[_index], _stickMaxDistance + _stickMaxDistance * 0.35f * 0.2f, _stickMaxDistance + _stickMaxDistance * 0.35f * 0.2f, -1f, true);
			}
			else
			{
				UpdateTouchSprite(_touch.stickBasePosition, _touch.stickInput, _stickMaxDistance / 1.875f, Color.white, StickDeadzoneSprite[_index], _stickMaxDistance, _stickMaxDistance, -1f);
			}
			break;
		}
		case TouchData.ETouchState.Select:
			UpdateTouchSprite(_touch.position, Vector2.zero, _stickMaxDistance / 1.875f, Color.white, SelectSprite[_index], _stickMinDistance, _stickMaxDistance, 0f);
			break;
		}
	}

	public void UpdateCameraFeedback(TouchData _touch, int _index, float _stickMinDistance, float _stickMaxDistance, float _tapMaxDuration)
	{
		switch (_touch.state)
		{
		case TouchData.ETouchState.None:
			break;
		case TouchData.ETouchState.Touch:
		{
			float a = (UbiCore.Utils.Singleton<Timer>.Instance.realTime - _touch.startTime) / _tapMaxDuration;
			UpdateTouchSprite(_color: new Color(1f, 1f, 1f, a), _position: _touch.startPosition, _stickInput: Vector2.zero, _sizeFactor: _stickMaxDistance / 1.875f, _sprite: TouchSprite[_index], _stickMinDistance: _stickMinDistance, _stickMaxDistance: _stickMaxDistance, layer: 0f);
			break;
		}
		case TouchData.ETouchState.Drag:
			UpdateTouchSprite(_touch.stickBasePosition, Vector2.zero, _stickMaxDistance, Color.white, StickSizeSprite[_index], _stickMinDistance, _stickMaxDistance, 0f);
			UpdateTouchSprite(_touch.stickBasePosition, _touch.stickInput, _stickMaxDistance, Color.white, StickSprite[_index], 0f, _stickMaxDistance * 0.35f, -1f);
			break;
		case TouchData.ETouchState.Select:
			UpdateTouchSprite(_touch.position, Vector2.zero, _stickMaxDistance / 1.875f, Color.white, SelectSprite[_index], _stickMinDistance, _stickMaxDistance, 0f);
			break;
		}
	}

	private void UpdateTouchSprite(Vector2 _position, Vector2 _stickInput, float _sizeFactor, Color _color, SpriteRenderer _sprite, float _stickMinDistance, float _stickMaxDistance, float layer = 0f, bool rotate = false)
	{
		float pixelScaleFactor = AcierInputHandler.Instance.PixelScaleFactor;
		Vector2 vector = new Vector2(2f, 2f) * _sizeFactor * pixelScaleFactor * GlobalScaleFactor;
		Vector2 normalized = _stickInput.normalized;
		float magnitude = _stickInput.magnitude;
		_position += normalized * Mathf.Lerp(_stickMinDistance, _stickMaxDistance, magnitude) * pixelScaleFactor * GlobalScaleFactor;
		_sprite.enabled = true;
		_sprite.color = _color;
		_sprite.transform.localPosition = new Vector3(_position.x, _position.y, layer * 0.001f);
		_sprite.transform.localScale = vector;
		if (rotate)
		{
			_sprite.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Acos(normalized.x) * Mathf.Sign(normalized.y) * 57.29578f - 90f);
		}
	}
}
