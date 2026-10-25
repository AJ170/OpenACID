using System.Collections.Generic;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public abstract class UIShopBaseItem : MonoBehaviour
	{
		public enum EShopNodeType
		{
			Category = 0,
			Item = 1,
			Advertisement = 2,
			AdColony = 3,
			HappyHourBanner = 4
		}

		public enum EShopItemViewType
		{
			ItemMain = 0,
			ItemInfo = 1,
			Category = 2
		}

		public static UIShopBaseItem Current;

		[SerializeField]
		private Transform m_mainContainerTransform;

		[SerializeField]
		private UISprite m_newIndicatorSprite;

		[SerializeField]
		private BoxCollider m_boxCollider;

		[SerializeField]
		protected GameObject m_onEnterTweensGameObject;

		[HideInInspector]
		public List<EventDelegate> OnSelected = new List<EventDelegate>();

		public bool IsSelected { get; protected set; }

		public ShopBaseNode CurrentShopBaseNode { get; protected set; }

		public BBTargetedTween[] EnterScreenTweens { get; private set; }

		public Transform MainContainer
		{
			get
			{
				return m_mainContainerTransform;
			}
		}

		public abstract EShopNodeType ShopDataNodeType { get; }

		public bool IsValid
		{
			get
			{
				return CurrentShopBaseNode != null;
			}
		}

		public void ResetScale()
		{
			m_mainContainerTransform.localScale = Vector3.one;
		}

		public virtual void UpdateData(ShopBaseNode _baseNode)
		{
			CurrentShopBaseNode = _baseNode;
			ActivateColliders(IsValid);
			m_mainContainerTransform.gameObject.SetActive(IsValid);
			if (m_newIndicatorSprite != null)
			{
				m_newIndicatorSprite.gameObject.SetActive(false);
			}
		}

		public void DisableItem()
		{
			UpdateData(null);
		}

		public virtual void ResetItemContainer()
		{
		}

		public void ActivateColliders(bool _status)
		{
			m_boxCollider.enabled = _status;
			OnItemPress(false);
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnDestroyItem()
		{
		}

		protected virtual void OnItemClicked()
		{
		}

		protected virtual void OnItemPress(bool _pressed)
		{
		}

		protected virtual void OnItemDragStart()
		{
		}

		private void Awake()
		{
			EnterScreenTweens = m_onEnterTweensGameObject.GetComponents<BBTargetedTween>();
			OnAwake();
		}

		private void OnDestroy()
		{
			Current = null;
			EnterScreenTweens = null;
			CurrentShopBaseNode = null;
			OnSelected.Clear();
			OnDestroyItem();
		}

		private void OnPress(bool _pressed)
		{
			OnItemPress(_pressed);
			if (!_pressed && IsSelected)
			{
				Current = this;
				OnItemClicked();
				Current = null;
			}
			IsSelected = _pressed;
		}

		private void OnSimulateTap()
		{
			OnPress(true);
			OnPress(false);
		}

		private void OnDragStart()
		{
			OnItemDragStart();
			IsSelected = false;
		}
	}
}
