using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.InfoPopup
{
	[RequireComponent(typeof(UIWidget))]
	public abstract class InfoPopUpContentView : MonoBehaviour
	{
		[SerializeField]
		protected UIPanel[] m_childPannels;

		[HideInInspector]
		public List<EventDelegate> onLayoutFinished = new List<EventDelegate>();

		public void SetData(InfoPopUpContentData _data)
		{
			Render(_data);
		}

		public abstract UIPortraitTexture GetPortraitTexture();

		public virtual void Purge()
		{
			onLayoutFinished.Clear();
		}

		public void UpdateChildPannelsDepth(int _depth)
		{
			for (int i = 0; i < m_childPannels.Length; i++)
			{
				m_childPannels[i].depth += _depth;
			}
		}

		public virtual void UpdateLayout()
		{
			EventDelegate.Execute(onLayoutFinished);
		}

		protected abstract void Render(InfoPopUpContentData _data);

		public virtual void OnLayoutFinished()
		{
		}
	}
	public abstract class InfoPopUpContentView<T> : InfoPopUpContentView where T : InfoPopUpContentData
	{
		protected abstract void Render(T _data);

		protected sealed override void Render(InfoPopUpContentData _data)
		{
			T data = _data as T;
			Render(data);
		}
	}
}
