using System.Collections.Generic;
using Assets.Scripts.UI.ScreenManager;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class TweenManager : MonoBehaviour
	{
		[SerializeField]
		private float m_overWritePositionTime;

		[SerializeField]
		private float m_startTweenDelay;

		[SerializeField]
		public List<UITweenedScreenComponent> m_tweenedComponents = new List<UITweenedScreenComponent>();

		private UITweener m_refTween;

		private readonly TweenManagerEventHandler m_eventHandler = new TweenManagerEventHandler();

		public List<GameObject> ScreenElements;

		public UITweener RefTweener
		{
			get
			{
				UITweener obj = m_refTween ?? GetReferenceTween();
				UITweener result = obj;
				m_refTween = obj;
				return result;
			}
		}

		public void CollectAndSetupTweenedComponents(bool _updatePosition)
		{
			if (RefTweener == null)
			{
				return;
			}
			UIScreen component = GetComponent<UIScreen>();
			if (ScreenElements != null)
			{
				for (int i = 0; i < ScreenElements.Count; i++)
				{
					GameObject gameObject = ScreenElements[i];
					if (gameObject == null)
					{
						continue;
					}
					UITweenedScreenComponent component2 = gameObject.GetComponent<UITweenedScreenComponent>();
					if (component2 != null)
					{
						if (component != null)
						{
							component2.SetScreenParent(component);
						}
						m_eventHandler.AddToTweenComponentsByTrigger(component2);
						SetupTweenedScreenComponent(component2, _updatePosition);
					}
				}
			}
			for (int j = 0; j < m_tweenedComponents.Count; j++)
			{
				UITweenedScreenComponent uITweenedScreenComponent = m_tweenedComponents[j];
				if (uITweenedScreenComponent != null)
				{
					if (component != null)
					{
						uITweenedScreenComponent.SetScreenParent(component);
					}
					m_eventHandler.AddToTweenComponentsByTrigger(uITweenedScreenComponent);
					SetupTweenedScreenComponent(uITweenedScreenComponent, _updatePosition);
				}
			}
		}

		public void AddTweenedComponent(UITweenedScreenComponent _tweenedComponent)
		{
			if (ScreenElements != null)
			{
				for (int i = 0; i < ScreenElements.Count; i++)
				{
					GameObject gameObject = ScreenElements[i];
					if (gameObject != null)
					{
						UITweenedScreenComponent component = gameObject.GetComponent<UITweenedScreenComponent>();
						if (component != null && component == _tweenedComponent)
						{
							return;
						}
					}
				}
			}
			if (!m_tweenedComponents.Contains(_tweenedComponent))
			{
				m_tweenedComponents.Add(_tweenedComponent);
				SetupTweenedScreenComponent(_tweenedComponent, true);
			}
		}

		public void AddEventListener(TweenManagerEventHandler.EventType _type, UITweenedScreenComponent.ETriggerPhase _phase, EventDelegate.Callback _callback)
		{
			m_eventHandler.AddEventListener(_type, _phase, _callback);
		}

		public void RemoveEventListener(TweenManagerEventHandler.EventType _type, UITweenedScreenComponent.ETriggerPhase _phase, EventDelegate.Callback _callback)
		{
			m_eventHandler.RemoveEventListener(_type, _phase, _callback);
		}

		public bool IsPlaying(UITweenedScreenComponent.ETriggerPhase _phase)
		{
			return m_eventHandler.IsPlaying(_phase);
		}

		private void Start()
		{
			CollectAndSetupTweenedComponents(true);
		}

		private void OnDestroy()
		{
			if (ScreenElements != null)
			{
				ScreenElements.Clear();
			}
			if (m_tweenedComponents != null)
			{
				m_tweenedComponents.Clear();
			}
			m_refTween = null;
			m_eventHandler.ClearAllEvents();
		}

		private TweenPosition GetReferenceTween()
		{
			UIRoot mainUIRoot = Singleton<UIScreenManager>.Instance.SceneData.MainUIRoot;
			if (mainUIRoot != null)
			{
				return mainUIRoot.GetComponent<TweenPosition>();
			}
			return null;
		}

		private UITweener SetupTweenedScreenComponent(UITweenedScreenComponent _tweenedScreenComponent, bool _updatePosition)
		{
			UITweener uITweener = null;
			_tweenedScreenComponent.enabled = true;
			if (_tweenedScreenComponent.Phase == UITweenedScreenComponent.ETriggerPhase.OnScreenActivate || _tweenedScreenComponent.Phase == UITweenedScreenComponent.ETriggerPhase.OnScreenEnter || _tweenedScreenComponent.Phase == UITweenedScreenComponent.ETriggerPhase.ManuallyStart)
			{
				switch (_tweenedScreenComponent.TranititionMode)
				{
				case UITweenedScreenComponent.ETransitionMode.Position:
					uITweener = SetupPositionTween(_tweenedScreenComponent, _updatePosition);
					break;
				case UITweenedScreenComponent.ETransitionMode.Alpha:
					uITweener = SetupAlphaTween(_tweenedScreenComponent);
					break;
				case UITweenedScreenComponent.ETransitionMode.Scale:
					uITweener = SetupScaleTween(_tweenedScreenComponent);
					break;
				default:
					uITweener = _tweenedScreenComponent.GetComponent<UITweener>();
					break;
				}
			}
			else
			{
				uITweener = _tweenedScreenComponent.GetComponent<UITweener>();
			}
			if (uITweener != null)
			{
				if (RefTweener != null)
				{
					CopyReferenceTweenValues(RefTweener, uITweener, _tweenedScreenComponent.PlaceinLine);
				}
				if (m_overWritePositionTime > 0f)
				{
					uITweener.duration = m_overWritePositionTime;
				}
			}
			return _tweenedScreenComponent.Tweener = uITweener;
		}

		private void CopyReferenceTweenValues(UITweener _refTween, UITweener _tweener, int _placeInLine)
		{
			float num = ((!(_refTween.delay > 0f)) ? 0f : 0.1f);
			_tweener.animationCurve = _refTween.animationCurve;
			_tweener.duration = _refTween.duration;
			_tweener.delay = m_startTweenDelay + _refTween.delay + num * (float)_placeInLine;
			_tweener.ignoreTimeScale = _refTween.ignoreTimeScale;
		}

		private UITweener SetupPositionTween(UITweenedScreenComponent _tweenedComponent, bool _updatePosition)
		{
			TweenPosition tweenPosition = _tweenedComponent.GetComponent<TweenPosition>();
			if (tweenPosition == null || _updatePosition)
			{
				if (tweenPosition == null)
				{
					tweenPosition = _tweenedComponent.gameObject.AddComponent<TweenPosition>();
					tweenPosition.enabled = false;
				}
				UIWidget uIWidget = NGUITools.FindInParents<UIWidget>(_tweenedComponent.gameObject);
				uIWidget.Update();
				Vector3[] sides = Singleton<UIScreenManager>.Instance.SceneData.MainUICamera.GetSides(uIWidget.transform);
				int num = 0;
				switch (_tweenedComponent.StartingSide)
				{
				case UITweenedScreenComponent.EStartingSide.Left:
				{
					float num3 = (float)uIWidget.width * Mathf.Abs(uIWidget.pivotOffset.x - 1f);
					num = (int)Mathf.Ceil(uIWidget.transform.localPosition.x + sides[0].x - num3);
					break;
				}
				case UITweenedScreenComponent.EStartingSide.Right:
				{
					float num2 = (float)uIWidget.width * uIWidget.pivotOffset.x;
					num = (int)Mathf.Ceil(uIWidget.transform.localPosition.x + sides[2].x + num2);
					break;
				}
				}
				Vector3 localPosition = tweenPosition.transform.localPosition;
				tweenPosition.from = new Vector3(num, localPosition.y, localPosition.z);
				tweenPosition.SetEndToCurrentValue();
			}
			return tweenPosition;
		}

		private UITweener SetupAlphaTween(UITweenedScreenComponent _tweenedComponent)
		{
			TweenAlpha tweenAlpha = _tweenedComponent.gameObject.AddComponentIfNotExist<TweenAlpha>();
			tweenAlpha.from = 0f;
			tweenAlpha.to = 1f;
			return tweenAlpha;
		}

		private UITweener SetupScaleTween(UITweenedScreenComponent _tweenedComponent)
		{
			TweenScale tweenScale = _tweenedComponent.gameObject.AddComponentIfNotExist<TweenScale>();
			tweenScale.from = Vector3.zero;
			tweenScale.to = Vector3.one;
			return tweenScale;
		}
	}
}
