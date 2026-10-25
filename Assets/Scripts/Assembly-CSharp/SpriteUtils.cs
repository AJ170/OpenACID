using Assets.Scripts.Engine;
using Assets.Scripts.UI.Ingame;
using UbiCore.Utils.Extensions;
using UnityEngine;

public class SpriteUtils
{
	public static TDefinition ConstructButton<TDefinition, TButton>(GameObject _buttonTemplate, bool _isEnabled, float _uiScale, Camera _uiCamera) where TDefinition : ISpriteButtonDefinition, new() where TButton : Component, ISpriteButtonBehaviour
	{
		Helpers.Verify(_buttonTemplate != null, "Template has to be set.");
		Helpers.Verify(_buttonTemplate.transform.localScale == Vector3.one, "Template must have a scale of (1,1,1).");
		TDefinition val = ConstructButtonDefinition<TDefinition, TButton>(_buttonTemplate, _uiCamera.gameObject, _isEnabled);
		TransformPixelExact(val, Vector2.one, _uiScale, _uiCamera);
		return val;
	}

	public static TDefinition ConstructControl<TDefinition, TControl>(GameObject _buttonTemplate, float _uiScale, Camera _uiCamera) where TDefinition : ISpriteControlDefinition, new() where TControl : Component, ISpriteControlBehaviour
	{
		Helpers.Verify(_buttonTemplate != null, "Template has to be set.");
		Helpers.Verify(_buttonTemplate.transform.localScale == Vector3.one, "Template must have a scale of (1,1,1).");
		return ConstructControlDefinition<TDefinition, TControl>(_buttonTemplate, _uiCamera.gameObject);
	}

	public static float ScalePixelExact(GameObject _gameObjectInstance, float _screenPixelWidth, Vector2 _meshSize)
	{
		float x = _meshSize.x;
		float num = 1f / x;
		float num2 = _screenPixelWidth * num;
		_gameObjectInstance.transform.localScale = new Vector3(num2, num2, 1f);
		return num2;
	}

	public static void TransformPixelExact(ISpriteControlDefinition _spriteButtonDefinition, Vector2 _offset, float _screenPixelWidth, Camera _uiCamera)
	{
		Helpers.Verify(_uiCamera != null, "UI Camera has to be set.");
		Helpers.Verify(_spriteButtonDefinition != null, "Definition has to be set.");
		GameObject spriteInstance = _spriteButtonDefinition.SpriteInstance;
		ISpriteControlBehaviour spriteBehaviour = _spriteButtonDefinition.SpriteBehaviour;
		Vector2 meshCenterOffset = spriteBehaviour.MeshCenterOffset;
		Vector2 meshSize = spriteBehaviour.MeshSize;
		TransformPixelExact(spriteInstance, _screenPixelWidth, _offset, meshSize, meshCenterOffset, _uiCamera);
	}

	private static TDefinition ConstructButtonDefinition<TDefinition, TSpriteType>(GameObject _template, GameObject _parent, bool _isEnabled) where TDefinition : ISpriteButtonDefinition, new() where TSpriteType : Component, ISpriteButtonBehaviour
	{
		GameObject gameObject = Object.Instantiate(_template);
		ISpriteButtonBehaviour component = gameObject.GetComponent<TSpriteType>();
		component.Construct();
		component.IsEnabled = _isEnabled;
		gameObject.transform.parent = _parent.transform;
		gameObject.SetLayerRecursively(LayerMask.NameToLayer("UI_Interactable"));
		SpriteRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<SpriteRenderer>();
		return new TDefinition
		{
			SpriteInstance = gameObject,
			SpriteBehaviour = component,
			SpriteRenderers = componentsInChildren
		};
	}

	private static TDefinition ConstructControlDefinition<TDefinition, TSpriteType>(GameObject _template, GameObject _parent) where TDefinition : ISpriteControlDefinition, new() where TSpriteType : Component, ISpriteControlBehaviour
	{
		GameObject gameObject = Object.Instantiate(_template);
		TSpriteType component = gameObject.GetComponent<TSpriteType>();
		if ((Object)component == (Object)null)
		{
			return default(TDefinition);
		}
		component.Construct();
		gameObject.transform.parent = _parent.transform;
		gameObject.SetLayerRecursively(LayerMask.NameToLayer("UI_Interactable"));
		SpriteRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<SpriteRenderer>();
		return new TDefinition
		{
			SpriteInstance = gameObject,
			SpriteBehaviour = component,
			SpriteRenderers = componentsInChildren
		};
	}

	private static void TransformPixelExact(GameObject _gameObjectInstance, float _screenPixelWidth, Vector2 _screenOffset, Vector2 _meshSize, Vector2 _meshCenterOffset, Camera _uiCamera)
	{
		Helpers.Verify(_uiCamera != null, "UI Camera has to be set.");
		float num = _uiCamera.orthographicSize * 2f * _uiCamera.aspect;
		float num2 = _uiCamera.orthographicSize * 2f;
		float num3 = _screenOffset.x * num;
		float num4 = _screenOffset.y * num;
		float num5 = ScalePixelExact(_gameObjectInstance, _screenPixelWidth, _meshSize);
		float num6 = (0f - _meshCenterOffset.x) * num5;
		float num7 = (0f - _meshCenterOffset.y) * num5;
		_gameObjectInstance.transform.localPosition = new Vector3(-0.5f * num + num6 + num3, -0.5f * num2 + num7 + num4, (_uiCamera.nearClipPlane + _uiCamera.farClipPlane) / 2f);
	}

	public static float ComputePixelWidth(float _relativeScreenWidth, Camera _uiCamera)
	{
		Helpers.Verify(_uiCamera != null, "UI Camera has to be set.");
		float num = (float)Screen.height * _uiCamera.aspect;
		return num * _relativeScreenWidth;
	}
}
