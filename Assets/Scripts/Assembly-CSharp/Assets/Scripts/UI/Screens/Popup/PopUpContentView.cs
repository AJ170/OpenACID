using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup
{
	[RequireComponent(typeof(UIWidget))]
	public abstract class PopUpContentView : MonoBehaviour
	{
		public List<EventDelegate> onLayoutFinished = new List<EventDelegate>();

		private UIWidget m_widget;

		public UIWidget Widget
		{
			get
			{
				if (m_widget == null)
				{
					m_widget = GetComponent<UIWidget>();
				}
				return m_widget;
			}
		}

		public virtual Vector2 Size
		{
			get
			{
				return new Vector2(Widget.width, Widget.height);
			}
		}

		public void SetData(PopUpContentData _data)
		{
			Render(_data);
		}

		public virtual void Purge()
		{
			onLayoutFinished.Clear();
		}

		public virtual void UpdateLayout()
		{
			EventDelegate.Execute(onLayoutFinished);
		}

		protected abstract void Render(PopUpContentData _data);

		public virtual void OnLayoutFinished()
		{
		}
	}
	public abstract class PopUpContentView<T> : PopUpContentView where T : PopUpContentData
	{
		protected abstract void Render(T _data);

		protected sealed override void Render(PopUpContentData _data)
		{
			T data = _data as T;
			Render(data);
		}
	}
}
