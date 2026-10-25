using System.Collections.Generic;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.Buttons
{
	public abstract class UIButtonHeader : MonoBehaviour
	{
		public enum EHeaderType
		{
			Invalid = -1,
			MissionsCampaign = 0,
			MissionsContracts = 1,
			Assassin = 2,
			Social = 3,
			Shop = 4,
			Codex = 5,
			Events = 6
		}

		[SerializeField]
		protected EHeaderType m_eHeaderType;

		[CheckAssigned]
		[SerializeField]
		protected UIWidget m_centerPlaceHolder;

		protected bool m_isSelected;

		protected bool m_isUnlocked = true;

		protected readonly List<EventDelegate> m_unlockChangeCompleted = new List<EventDelegate>();

		private UIScreenChanger m_screenChanger;

		public List<EventDelegate> OnUnlockChangeCompleted
		{
			get
			{
				return m_unlockChangeCompleted;
			}
		}

		public UIScreenChanger ScreenChanger
		{
			get
			{
				if (m_screenChanger == null)
				{
					m_screenChanger = GetComponent<UIScreenChanger>();
				}
				return m_screenChanger;
			}
		}

		public EScreenID ScreenID
		{
			get
			{
				return ScreenChanger.ScreenID;
			}
		}

		public EHeaderType HeaderType
		{
			get
			{
				return m_eHeaderType;
			}
		}

		public virtual UIWidget CenterPlaceHolder
		{
			get
			{
				return m_centerPlaceHolder;
			}
		}

		public abstract bool IsEnabled { get; set; }

		public abstract bool IsAnimating { get; protected set; }

		public abstract bool IsUnlocked { get; set; }

		public virtual void InitButton()
		{
		}

		public virtual void SelectButton()
		{
		}

		public virtual void DeselectButton()
		{
		}

		public virtual void ChangeLockedState(bool _isUnlocked, bool _changeInstantly, bool _forceUpdate = false)
		{
		}

		private void Awake()
		{
			InitButton();
		}

		private void OnDestroy()
		{
			OnDestroyButton();
		}

		protected virtual void OnDestroyButton()
		{
		}

		protected virtual bool DoPress(bool _pressed)
		{
			return true;
		}

		private bool OnPress(bool _pressed)
		{
			return DoPress(_pressed);
		}
	}
}
