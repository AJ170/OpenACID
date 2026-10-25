using System;
using System.Collections;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIDropDown : MonoBehaviour, IInitialized<UIDropDownConfiguration>
	{
		public enum EState
		{
			Opened = 0,
			Closed = 1
		}

		private const string k_openTrack = "Open";

		private const string k_closeTrack = "Close";

		[SerializeField]
		private UITable m_table;

		[SerializeField]
		private BBTweenHeight m_tweenHeight;

		[SerializeField]
		private UIScrollView m_scrollView;

		[SerializeField]
		private UnityEngine.Animation m_animation;

		[SerializeField]
		private GameObject m_listItem;

		[SerializeField]
		private UIWidget m_bounds;

		[SerializeField]
		private bool m_selectMultiple;

		[SerializeField]
		private UISprite m_arrowSprite;

		private GameObjectCache m_cache;

		private bool m_inTransition;

		private readonly BBList<UIDropDownItem> m_selectedItems = new BBList<UIDropDownItem>(6);

		private readonly BBList<UIDropDownItem> m_items = new BBList<UIDropDownItem>(6);

		public EState State { get; private set; }

		public void Initialize(UIDropDownConfiguration _data)
		{
			InitObjectPool();
			Clear();
			for (int i = 0; i < _data.Items.Count; i++)
			{
				GameObjectCache.GameObjectCacheEntry gameObjectCacheEntry = m_cache.Dequeue();
				UIDropDownItem component = gameObjectCacheEntry.Item.GetComponent<UIDropDownItem>();
				gameObjectCacheEntry.Item.gameObject.name = string.Format("{0:00}", i);
				component.UpdateItem(_data.Items[i], m_bounds);
				component.OnClicked += OnItemClicked;
				m_items.Add(component);
				if (_data.Items[i].Selected)
				{
					m_selectedItems.Add(component);
				}
			}
			base.gameObject.SetActive(true);
			m_table.Reposition();
		}

		public void Clear()
		{
			m_selectedItems.Clear();
			m_items.Clear();
			if (m_cache != null)
			{
				m_cache.ReleaseAll(OnItemRelease);
			}
		}

		public void SetState(EState _state)
		{
			m_inTransition = false;
			StopAllCoroutines();
			State = _state;
			SetupTween(State);
			string stateName = ((State != 0) ? "Close" : "Open");
			m_animation.SampleAnimation(1f, stateName);
			OnTweening(1f);
			if (State == EState.Closed)
			{
				base.gameObject.GetComponent<UIPanel>().alpha = 0f;
			}
		}

		public void Open(Action _finished = null, Action<float> _animationProgress = null)
		{
			if (State != 0 && !m_inTransition)
			{
				State = EState.Opened;
				base.gameObject.SetActive(true);
				SetupTween(State);
				UpdateSelection();
				StartCoroutine(co_ChangeState("Open", _finished, OnTweening, _animationProgress));
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_whoosh_small);
			}
		}

		public void Close(Action _finished = null, Action<float> _animationProgress = null)
		{
			if (State == EState.Closed || m_inTransition)
			{
				return;
			}
			State = EState.Closed;
			SetupTween(State);
			StartCoroutine(co_ChangeState("Close", delegate
			{
				base.gameObject.SetActive(false);
				UpdateSelection();
				if (_finished != null)
				{
					_finished();
				}
			}, OnTweening, _animationProgress));
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_whoosh_small);
		}

		private void UpdateSelection()
		{
			for (int i = 0; i < m_items.Count; i++)
			{
				m_items[i].Select(m_selectedItems.Contains(m_items[i]), true);
			}
		}

		private void OnTweening(float _factor)
		{
			float z = ((State != 0) ? (180f - 180f * _factor) : (180f * _factor));
			if (m_arrowSprite != null && m_arrowSprite.transform != null)
			{
				m_arrowSprite.transform.eulerAngles = new Vector3(0f, 0f, z);
			}
		}

		private void Awake()
		{
			InitObjectPool();
			SetState(EState.Closed);
		}

		private void OnDisable()
		{
			SetState(EState.Closed);
		}

		private void OnDestroy()
		{
			if (m_cache != null)
			{
				m_cache.ReleaseAll(OnItemRelease);
				m_cache.Destroy();
			}
		}

		private Vector2 GetSize()
		{
			UIPanel component = GetComponent<UIPanel>();
			Vector2 viewSize = component.GetViewSize();
			viewSize.x = 0f;
			return viewSize;
		}

		private void InitObjectPool()
		{
			if (m_cache == null)
			{
				m_cache = new GameObjectCache(m_listItem, m_table.transform);
				m_cache.Reserve(6);
			}
		}

		private void SetupTween(EState _toState)
		{
			Vector2 size = GetSize();
			switch (_toState)
			{
			case EState.Opened:
				m_tweenHeight.from = (int)size.x;
				m_tweenHeight.to = (int)size.y;
				break;
			case EState.Closed:
				m_tweenHeight.from = (int)size.y;
				m_tweenHeight.to = (int)size.x;
				break;
			default:
				throw new ArgumentOutOfRangeException("_toState");
			}
		}

		private void OnItemRelease(GameObject _obj)
		{
			UIDropDownItem component = _obj.GetComponent<UIDropDownItem>();
			component.OnClicked -= OnItemClicked;
		}

		private void OnItemClicked(UIDropDownItem _sender)
		{
			if (m_selectMultiple)
			{
				UpdateItemSelectionMultiple(_sender);
			}
			else
			{
				UpdateItemSelectionSingle(_sender);
			}
		}

		private void UpdateItemSelectionSingle(UIDropDownItem _item)
		{
			UIDropDownItem uIDropDownItem = m_selectedItems.Pop();
			if (uIDropDownItem != null)
			{
				UIDropDownItemConfiguration data = uIDropDownItem.Data;
				SelectItem(uIDropDownItem, data, false);
			}
			m_selectedItems.Add(_item);
			UIDropDownItemConfiguration data2 = _item.Data;
			SelectItem(_item, data2, true);
		}

		private void SelectItem(UIDropDownItem _item, UIDropDownItemConfiguration _data, bool _select)
		{
			if (_data.OnSelectionChange != null)
			{
				_data.OnSelectionChange(_select, _data);
			}
			_item.Select(_select, false, delegate(UIDropDownItem _downItem, bool _selected)
			{
				if (_data.OnStateChangedFinished != null)
				{
					_data.OnStateChangedFinished(_selected, _data);
				}
			});
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_navigate);
		}

		private void UpdateItemSelectionMultiple(UIDropDownItem _item)
		{
			UIDropDownItemConfiguration data = _item.Data;
			if (data == null)
			{
				return;
			}
			if (m_selectedItems.Remove(_item))
			{
				if (_item.Selected)
				{
					SelectItem(_item, data, false);
				}
			}
			else if (!_item.Selected)
			{
				m_selectedItems.Add(_item);
				SelectItem(_item, data, true);
			}
		}

		private IEnumerator co_ChangeState(string _clip, Action _finished = null, Action<float> _tweenProgress = null, Action<float> _animationProgress = null)
		{
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			m_animation.SampleAnimation(0f, _clip);
			yield return wait;
			yield return wait;
			base.gameObject.GetComponent<UIPanel>().alpha = 1f;
			bool tweenPlaying = true;
			Action<float> _tweenProgress2 = default(Action<float>);
			EventDelegate.Add(m_tweenHeight.onFinished, delegate
			{
				tweenPlaying = false;
				if (_tweenProgress2 != null)
				{
					_tweenProgress2(1f);
				}
			}, true);
			m_inTransition = true;
			m_animation.Play(_clip);
			while (m_animation.IsPlaying(_clip))
			{
				if (tweenPlaying && _tweenProgress != null)
				{
					_tweenProgress(m_tweenHeight.tweenFactor);
				}
				if (_animationProgress != null)
				{
					_animationProgress(m_animation[_clip].normalizedTime);
				}
				yield return wait;
			}
			m_inTransition = false;
			if (_finished != null)
			{
				_finished();
			}
		}

		public void Toggle()
		{
			switch (State)
			{
			case EState.Opened:
				Close();
				break;
			case EState.Closed:
				Open();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
