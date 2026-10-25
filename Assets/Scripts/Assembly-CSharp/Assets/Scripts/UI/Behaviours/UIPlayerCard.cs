using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using UbiCore;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIPlayerCard : MonoBehaviour
	{
		public enum EPositionType
		{
			Default = 0,
			CreateAccountAccept = 1,
			OffScreenLeft = 2,
			OffScreenRight = 3
		}

		private const float AspectPositionFactor = -221.72f;

		private static readonly int TriggerTurnFront = Animator.StringToHash("trig_turn_to_front");

		private static readonly int TriggerTurnBack = Animator.StringToHash("trig_turn_to_back");

		[SerializeField]
		private Light m_cardLight;

		[SerializeField]
		private UIRoot m_root;

		[SerializeField]
		private BBTweenPosition m_positionTween;

		[SerializeField]
		private BBTweenRotation m_rotationTween;

		[SerializeField]
		private Transform m_defaultTrans;

		[SerializeField]
		private Transform m_confirmTrans;

		[SerializeField]
		private Animator m_animator;

		[SerializeField]
		private float m_offScreenX = 2000f;

		private bool m_faceFront = true;

		public PlayerIdCard Card;

		private Vector3 CenterCardPos(Transform _t)
		{
			float num = (float)Screen.width / (float)Screen.height;
			float num2 = (num - 1.3333334f) * -221.72f;
			return (!MathHelper.Approximately(_t.localPosition.x, 0f)) ? new Vector3(num2 + _t.localPosition.x, _t.localPosition.y, _t.localPosition.z) : _t.localPosition;
		}

		private void OnEnable()
		{
			m_faceFront = true;
			m_animator.SetTrigger(TriggerTurnFront);
		}

		private void OnClick()
		{
			m_faceFront = !m_faceFront;
			int trigger = ((!m_faceFront) ? TriggerTurnBack : TriggerTurnFront);
			m_animator.SetTrigger(trigger);
		}

		public void SetVisibilityByScreen()
		{
			SetVisibility(VisibilityByScreen());
		}

		private bool VisibilityByScreen()
		{
			UIScreenManager instance = Singleton<UIScreenManager>.Instance;
			if (instance == null || !instance.IsInitialized)
			{
				return false;
			}
			if (instance.CurrentScreen != null)
			{
				EScreenID screenId = instance.CurrentScreen.ScreenId;
				if (screenId == EScreenID.SocialSelectionScreen)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		public void SetVisibility(bool _show)
		{
			if (base.gameObject != null)
			{
				base.gameObject.SetActive(_show);
			}
			if (m_root != null)
			{
				m_root.gameObject.SetActive(_show);
			}
			if (m_cardLight != null)
			{
				m_cardLight.gameObject.SetActive(_show);
			}
		}

		public void SetPosition(EPositionType _type)
		{
			base.transform.localPosition = TypeToPosition(_type);
		}

		public void SetRotation(EPositionType _type)
		{
			m_faceFront = true;
			base.transform.localEulerAngles = TypeToRotation(_type);
		}

		public void Tween(EPositionType _from, EPositionType _to)
		{
			TweenPosition(TypeToPosition(_from), TypeToPosition(_to));
			TweenRotation(TypeToRotation(_from), TypeToRotation(_to));
		}

		private void TweenPosition(Vector3 _from, Vector3 _to)
		{
			m_positionTween.from = _from;
			m_positionTween.to = _to;
			m_positionTween.StartTweenPlayForward();
		}

		private void TweenRotation(Vector3 _from, Vector3 _to)
		{
			m_faceFront = true;
			m_rotationTween.from = _from;
			m_rotationTween.to = _to;
			m_rotationTween.StartTweenPlayForward();
		}

		private Vector3 TypeToPosition(EPositionType _type)
		{
			switch (_type)
			{
			case EPositionType.Default:
				return CenterCardPos(m_defaultTrans);
			case EPositionType.CreateAccountAccept:
				return CenterCardPos(m_confirmTrans);
			case EPositionType.OffScreenLeft:
				return new Vector3(0f - m_offScreenX, m_confirmTrans.localPosition.y, m_confirmTrans.localPosition.z);
			case EPositionType.OffScreenRight:
				return new Vector3(m_offScreenX, m_confirmTrans.localPosition.y, m_confirmTrans.localPosition.z);
			default:
				return CenterCardPos(m_defaultTrans);
			}
		}

		private Vector3 TypeToRotation(EPositionType _type)
		{
			switch (_type)
			{
			case EPositionType.Default:
				return m_defaultTrans.localEulerAngles;
			case EPositionType.CreateAccountAccept:
				return m_confirmTrans.localEulerAngles;
			case EPositionType.OffScreenLeft:
				return m_confirmTrans.localEulerAngles;
			case EPositionType.OffScreenRight:
				return m_confirmTrans.localEulerAngles;
			default:
				return m_defaultTrans.localEulerAngles;
			}
		}
	}
}
