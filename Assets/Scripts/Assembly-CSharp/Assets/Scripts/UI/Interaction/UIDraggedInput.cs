using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI.Interaction
{
	public class UIDraggedInput : UIInput
	{
		private bool m_wasDrag;

		private void OnDisable()
		{
			StopAllCoroutines();
			ClearFlag();
		}

		private void OnDestroy()
		{
			StopAllCoroutines();
			ClearFlag();
		}

		protected override void OnSelect(bool isSelected)
		{
			if (!m_wasDrag)
			{
				if (isSelected)
				{
					OnSelectEvent();
				}
				else
				{
					OnDeselectEvent();
				}
			}
			else
			{
				UICamera.selectedObject = null;
			}
		}

		protected virtual void OnDragStart()
		{
			m_wasDrag = true;
		}

		protected virtual void OnDragEnd()
		{
			StartCoroutine(co_ClearDrag());
		}

		private IEnumerator co_ClearDrag()
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			ClearFlag();
		}

		private void ClearFlag()
		{
			m_wasDrag = false;
		}
	}
}
