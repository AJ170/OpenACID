using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Engine;
using Assets.Scripts.Engine.ExtensionMethods;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Ingame;
using UbiCore.Utils;
using UnityEngine;

public abstract class UIControlBehaviour : MonoBehaviour, ISpriteControlBehaviour
{
	public delegate void AnimationCompleteCallback();

	[SerializeField]
	private List<Collider> m_collider;

	[SerializeField]
	private float m_screenWidth;

	private bool m_isPlayingAnimationBlocked;

	private bool m_isScaleListenerAttached;

	public float Aspect
	{
		get
		{
			return MeshSize.y / MeshSize.x;
		}
	}

	public List<Collider> Collider
	{
		get
		{
			return m_collider;
		}
		private set
		{
			m_collider = value;
		}
	}

	public Vector2 MeshCenterOffset { get; private set; }

	public Vector2 MeshSize { get; private set; }

	public float ScreenWidth
	{
		get
		{
			return m_screenWidth;
		}
		set
		{
			m_screenWidth = value;
		}
	}

	public virtual bool IsVisible { get; set; }

	public virtual bool IsRendered { get; set; }

	public bool IsEnabled { get; set; }

	protected Camera UICamera
	{
		get
		{
			if (UbiCore.Utils.Singleton<UiMain>.Instance != null)
			{
				return UbiCore.Utils.Singleton<UiMain>.Instance.UICamera;
			}
			return null;
		}
	}

	public void OnLevelHasBeenLoaded(int _index)
	{
	}

	public virtual void Dispose()
	{
		m_collider.ClearIfNotEmpty();
		if (m_isScaleListenerAttached)
		{
			Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
			instance.OnScaleChanged = (Timer.OnScaleChangeHandler)Delegate.Remove(instance.OnScaleChanged, new Timer.OnScaleChangeHandler(OnScaleChanged));
			m_isScaleListenerAttached = false;
		}
	}

	public virtual void Construct()
	{
		Bounds bounds = base.gameObject.ComputeMeshBounds();
		MeshSize = new Vector2(bounds.extents.x * 2f, bounds.extents.y * 2f);
		MeshCenterOffset = new Vector2(bounds.min.x, bounds.min.y);
	}

	public bool IsOnUi(Vector2 _position)
	{
		Ray ray = UICamera.ScreenPointToRay(_position);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 1 << base.gameObject.layer) && Collider != null && Collider.Contains(hitInfo.collider))
		{
			return true;
		}
		return false;
	}

	public void SetCollidersEnabledState(bool _state)
	{
		for (int i = 0; i < Collider.Count; i++)
		{
			Collider[i].enabled = _state;
		}
	}

	protected virtual void SetVisibility(bool _isVisible)
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			renderer.enabled = _isVisible;
		}
	}

	protected void PlayAnimation(string _animationName, bool _playQueued = false, bool _playBlocking = false, AnimationCompleteCallback _animationCompleteCallback = null)
	{
		if (!m_isPlayingAnimationBlocked && !(GetComponent<Animation>() == null))
		{
			if (!m_isScaleListenerAttached)
			{
				Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
				instance.OnScaleChanged = (Timer.OnScaleChangeHandler)Delegate.Combine(instance.OnScaleChanged, new Timer.OnScaleChangeHandler(OnScaleChanged));
				OnScaleChanged(UbiCore.Utils.Singleton<Timer>.Instance.timeScale);
				m_isScaleListenerAttached = true;
			}
			if (_playQueued)
			{
				GetComponent<Animation>().PlayQueued(_animationName);
			}
			else
			{
				GetComponent<Animation>().Play(_animationName);
			}
			if (_playBlocking)
			{
				StartCoroutine(BlockTillAnimationEnd(_animationName, _animationCompleteCallback));
			}
		}
	}

	protected virtual void OnScaleChanged(float _new)
	{
		if (!(GetComponent<Animation>() != null))
		{
			return;
		}
		float speed = ((!(_new > 0f)) ? 0f : (1f / _new));
		foreach (AnimationState item in GetComponent<Animation>())
		{
			item.speed = speed;
		}
	}

	private IEnumerator BlockTillAnimationEnd(string _clipName, AnimationCompleteCallback _animationCompleteCallback)
	{
		m_isPlayingAnimationBlocked = true;
		while (GetComponent<Animation>().IsPlaying(_clipName))
		{
			yield return StartCoroutine(Timer.WaitForSecondsUnscaled(0.05f));
		}
		m_isPlayingAnimationBlocked = false;
		if (_animationCompleteCallback != null)
		{
			_animationCompleteCallback();
		}
	}
}
