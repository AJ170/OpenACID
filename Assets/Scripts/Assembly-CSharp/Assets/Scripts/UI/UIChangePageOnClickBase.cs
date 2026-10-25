using UnityEngine;

namespace Assets.Scripts.UI
{
	public class UIChangePageOnClickBase : MonoBehaviour
	{
		public enum EDirection
		{
			Next = 0,
			Previous = 1
		}

		public EDirection Direction;

		public bool ShowOnlyIfNeeded = true;

		public bool HideButtonIfOnLastPage = true;

		protected bool IsLooping;

		private UIButton m_button;

		public bool Initialized { get; protected set; }

		public virtual int PageCount
		{
			get
			{
				return 0;
			}
		}

		public virtual void MoveToNextPage()
		{
		}

		public virtual void MoveToPreviousPage()
		{
		}

		protected virtual void Init()
		{
			Initialized = false;
			if (m_button != null)
			{
				m_button = base.gameObject.GetComponent<UIButton>();
			}
			base.gameObject.SetActive(!ShowOnlyIfNeeded || PageCount > 1);
			Initialized = true;
		}

		protected void OnCurrentPageChanged(int _oldPage, int _newPage)
		{
			if (!Initialized)
			{
				Init();
			}
			if (!Initialized || IsLooping)
			{
				return;
			}
			bool active = false;
			if (!ShowOnlyIfNeeded || PageCount > 1)
			{
				active = true;
				bool flag = true;
				flag = ((Direction != 0) ? (_newPage > 0) : (_newPage < PageCount - 1));
				if (m_button != null)
				{
					m_button.isEnabled = flag;
				}
				if (!flag && HideButtonIfOnLastPage)
				{
					active = false;
				}
			}
			base.gameObject.SetActive(active);
		}

		private void Start()
		{
			Init();
		}

		private void OnClick()
		{
			if (!Initialized)
			{
				Init();
			}
			if (Initialized)
			{
				if (Direction == EDirection.Next)
				{
					MoveToNextPage();
				}
				else
				{
					MoveToPreviousPage();
				}
			}
		}
	}
}
