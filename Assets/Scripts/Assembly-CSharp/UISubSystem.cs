using System.Collections.Generic;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Ingame;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

public abstract class UISubSystem : UISubSystemLayout, IUISubSystemLayout, IUiSubSystem
{
	private readonly List<ISpriteControlDefinition> m_Sprites = new List<ISpriteControlDefinition>();

	public bool IsActive { get; protected set; }

	public virtual bool InitiallyVisible
	{
		get
		{
			return true;
		}
	}

	public int SpriteCount
	{
		get
		{
			return m_Sprites.Count;
		}
	}

	protected Camera UICamera
	{
		get
		{
			return UbiCore.Utils.Singleton<UiMain>.Instance.UICamera;
		}
	}

	public UISubSystem()
	{
		ScreenAnchor = ScreenAnchor.BottomLeft;
		IsActive = false;
	}

	public ISpriteControlDefinition GetSprite(int _idx)
	{
		return m_Sprites[_idx];
	}

	public void AddSprite(ISpriteControlDefinition _val)
	{
		m_Sprites.Add(_val);
	}

	public virtual void TearDownSprites()
	{
		for (int i = 0; i < m_Sprites.Count; i++)
		{
			m_Sprites[i].SpriteBehaviour.Dispose();
			Object.Destroy(m_Sprites[i].SpriteInstance);
		}
		m_Sprites.Clear();
	}

	public virtual bool IsOnUi(Vector2 _position)
	{
		if (SpriteCount <= 0)
		{
			return false;
		}
		Ray ray = UICamera.ScreenPointToRay(_position);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 1 << GetSprite(0).SpriteInstance.layer))
		{
			for (int i = 0; i < SpriteCount; i++)
			{
				ISpriteControlDefinition sprite = GetSprite(i);
				if (sprite.SpriteBehaviour.Collider != null && sprite.SpriteBehaviour.Collider.Contains(hitInfo.collider))
				{
					return true;
				}
			}
		}
		return false;
	}

	public virtual void OnSceneLoaded()
	{
	}

	public virtual void OnSceneUnload()
	{
	}

	public virtual void OnCameraCreated()
	{
	}

	public virtual void OnSharedUILoaded()
	{
	}

	public virtual void OnMissionEnded()
	{
	}

	public virtual void OnMissionOutroStarted()
	{
	}

	public virtual void OnUIAlignmentChanged(bool _isRightHanded)
	{
	}

	public virtual void OnUIReadyToUse()
	{
	}

	public virtual void OnMissionLoad()
	{
	}

	public virtual void OnMissionLoaded()
	{
		IsActive = !Main.Instance.SceneLoader.InAnimus;
	}

	public virtual void OnMissionUnload()
	{
	}

	public virtual void OnMissionUnloaded()
	{
		TearDownSprites();
		IsActive = false;
	}

	public abstract bool Tap(TapEvent _gesture);

	public abstract void UpdateFrame(float _deltaTime);

	public virtual void SetVisible(bool _visible)
	{
		for (int i = 0; i < SpriteCount; i++)
		{
			ISpriteControlDefinition sprite = GetSprite(i);
			sprite.SpriteBehaviour.IsVisible = _visible;
		}
	}
}
