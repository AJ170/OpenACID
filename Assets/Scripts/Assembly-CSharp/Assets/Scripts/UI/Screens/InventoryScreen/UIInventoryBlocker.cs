using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class UIInventoryBlocker : MonoBehaviour
	{
		public sealed class BlockerHandle
		{
			private readonly UIFadeController m_fade;

			private readonly BoxCollider m_collider;

			private int m_activatedCounter;

			public BlockerHandle(UIFadeController _fade, BoxCollider _collider)
			{
				m_fade = _fade;
				m_collider = _collider;
				m_fade.SetState(UIFadeController.EState.Off, true);
				m_collider.enabled = false;
			}

			public void On(bool _faded, bool _animated)
			{
				m_activatedCounter++;
				if (m_activatedCounter == 1)
				{
					m_collider.enabled = true;
					ShowFade(_faded, _animated);
				}
			}

			public void Off(bool _animated)
			{
				m_activatedCounter--;
				if (m_activatedCounter == 0)
				{
					m_collider.enabled = false;
					ShowFade(false, _animated);
				}
			}

			public void ShowFade(bool _show, bool _animated)
			{
				if (_show)
				{
					m_fade.SetState(UIFadeController.EState.On, !_animated);
				}
				else
				{
					m_fade.SetState(UIFadeController.EState.Off, true);
				}
			}

			public void Reset()
			{
				m_activatedCounter = 0;
				m_collider.enabled = false;
				ShowFade(false, false);
			}
		}

		public enum EBlocker
		{
			Full = 0
		}

		private const float k_alphaOn = 0.5f;

		private const float k_alphaOff = 0f;

		private const float k_alphaDuration = 0.15f;

		[SerializeField]
		private UIFadeController m_fullScreenFadeController;

		[SerializeField]
		private BoxCollider m_fullScreenCollider;

		private readonly BBDictionary<int, BlockerHandle> m_blockers = new BBDictionary<int, BlockerHandle>();

		private bool m_init;

		public BlockerHandle GetBlocker(EBlocker _blocker)
		{
			Init();
			BlockerHandle value;
			if (m_blockers.TryGetValue((int)_blocker, out value))
			{
				return value;
			}
			return null;
		}

		private void Awake()
		{
			Init();
		}

		private void Init()
		{
			if (!m_init)
			{
				m_init = true;
				m_fullScreenFadeController.Set(0f, 0.5f, 0.15f);
				m_blockers.Add(0, new BlockerHandle(m_fullScreenFadeController, m_fullScreenCollider));
			}
		}
	}
}
