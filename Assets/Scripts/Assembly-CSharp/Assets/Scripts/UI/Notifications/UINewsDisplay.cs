using System;
using System.Collections;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Notifications
{
	public class UINewsDisplay : MonoBehaviour
	{
		private static readonly float[] InTangents = new float[3] { 30.65112f, 0f, -0.009468615f };

		private static readonly float[] OutTangents = new float[3] { 30.65112f, 0f, -0.009468615f };

		private static readonly float[] Times = new float[3] { 0f, 0.07f, 0.5f };

		private static readonly float[] Values = new float[3] { 0f, 0.6f, 0.25f };

		[SerializeField]
		private UISprite m_icon;

		[SerializeField]
		private ENewsType m_type;

		private UINewsEvaluatorBase m_evaluator;

		private BBTweenScale m_tweener;

		public ENewsType Type
		{
			set
			{
				if (m_type != value)
				{
					m_type = value;
					SetupDisplay();
				}
			}
		}

		private void Start()
		{
			if (m_evaluator == null)
			{
				SetupDisplay();
			}
		}

		private void SetupDisplay()
		{
			if (m_evaluator != null)
			{
				m_evaluator.Destroy();
			}
			m_evaluator = CreateEvaluator(m_type, OnValueChanged);
			m_icon.enabled = false;
			if (m_type != 0)
			{
				UIAtlas atlasByType = Singleton<UIScreenManager>.Instance.GetAtlasByType(UIScreenManager.EAtlasType.Animus);
				m_icon.atlas = atlasByType;
				m_icon.spriteName = m_evaluator.DisplayIcon;
				AnimationCurve animationCurve = new AnimationCurve();
				Keyframe[] array = new Keyframe[InTangents.Length];
				for (int i = 0; i < InTangents.Length; i++)
				{
					array[i].inTangent = InTangents[i];
					array[i].outTangent = OutTangents[i];
					array[i].time = Times[i];
					array[i].value = Values[i];
				}
				animationCurve.keys = array;
				m_tweener = m_icon.gameObject.AddComponent<BBTweenScale>();
				m_tweener.animationCurve = animationCurve;
				m_tweener.ActivateOnPlay = true;
				m_tweener.Target = m_icon.gameObject;
				m_tweener.from = Vector3.zero;
				m_tweener.to = new Vector3(4f, 4f, 4f);
			}
			m_evaluator.Update();
		}

		private void OnDestroy()
		{
			if (m_evaluator != null)
			{
				m_evaluator.OnValueChanged -= OnValueChanged;
				m_evaluator.Destroy();
				m_evaluator = null;
			}
		}

		private void OnEnable()
		{
			StartCoroutine("WaitForShow", true);
		}

		private IEnumerator WaitForShow(bool _includeWait)
		{
			if (m_evaluator == null)
			{
				yield break;
			}
			bool visible = m_evaluator.Value && m_type != 0 && IsNewsTypeUnlocked(m_type);
			if (visible)
			{
				m_icon.gameObject.SetActive(false);
				m_icon.enabled = false;
				if (_includeWait)
				{
					yield return new WaitForSeconds(1f);
				}
				PlayTweenDelayed(0f);
				m_icon.gameObject.SetActive(visible);
				m_icon.enabled = visible;
			}
			else
			{
				m_icon.gameObject.SetActive(visible);
				m_icon.enabled = visible;
			}
		}

		public void PlayTweenDelayed(float _delay)
		{
			m_tweener.delay = _delay;
			m_tweener.StartTweenPlayForward();
		}

		private void OnValueChanged(bool _value)
		{
			StopCoroutine("WaitForShow");
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine("WaitForShow", true);
				return;
			}
			IEnumerator enumerator = WaitForShow(false);
			while (enumerator.MoveNext())
			{
			}
		}

		private static UINewsEvaluatorBase CreateEvaluator(ENewsType _type, UINewsEvaluatorBase.HandleValueChange _callback)
		{
			UINewsEvaluatorBase uINewsEvaluatorBase;
			switch (_type)
			{
			case ENewsType.Social:
			{
				UIAggregatedNewsEvaluator uIAggregatedNewsEvaluator2 = new UIAggregatedNewsEvaluator();
				uIAggregatedNewsEvaluator2.Add(new UIMailsEvaluator());
				uIAggregatedNewsEvaluator2.Add(new UIFriendsEvaluator());
				uIAggregatedNewsEvaluator2.Add(new UIAchievementsEvaluator());
				uINewsEvaluatorBase = uIAggregatedNewsEvaluator2;
				break;
			}
			case ENewsType.Mail:
				uINewsEvaluatorBase = new UIMailsEvaluator();
				break;
			case ENewsType.Friends:
				uINewsEvaluatorBase = new UIFriendsEvaluator();
				break;
			case ENewsType.Assassins:
			{
				UIAggregatedNewsEvaluator uIAggregatedNewsEvaluator = new UIAggregatedNewsEvaluator();
				uIAggregatedNewsEvaluator.Add(new UIMysteryBoxEvaluator());
				uIAggregatedNewsEvaluator.Add(new UIInventoryEvaluator());
				uIAggregatedNewsEvaluator.Add(new UIAllAssassinsEvaluator());
				uIAggregatedNewsEvaluator.Add(new UIModulesEvaluator());
				uINewsEvaluatorBase = uIAggregatedNewsEvaluator;
				break;
			}
			case ENewsType.Inventory:
				uINewsEvaluatorBase = new UIInventoryEvaluator();
				break;
			case ENewsType.Forge:
				uINewsEvaluatorBase = new UIForgeEvaluator();
				break;
			case ENewsType.Codex:
				uINewsEvaluatorBase = new UICodexEvaluator();
				break;
			case ENewsType.Achievements:
				uINewsEvaluatorBase = new UIAchievementsEvaluator();
				break;
			case ENewsType.Invalid:
				uINewsEvaluatorBase = new UINullnewsEvaluator();
				break;
			case ENewsType.SkillPointsCurrent:
				uINewsEvaluatorBase = new UICurrentAssasinEvaluator();
				break;
			case ENewsType.Modules:
				uINewsEvaluatorBase = new UIModulesEvaluator();
				break;
			case ENewsType.Shop:
				uINewsEvaluatorBase = new UIShopEvaluator();
				break;
			case ENewsType.MysteryBox:
				uINewsEvaluatorBase = new UIMysteryBoxEvaluator();
				break;
			case ENewsType.Campaign:
				uINewsEvaluatorBase = new UICampaignEvaluator();
				break;
			case ENewsType.Event:
				uINewsEvaluatorBase = new UIEventChallengesEvaluator();
				break;
			default:
				throw new ArgumentOutOfRangeException("_type");
			}
			uINewsEvaluatorBase.Init();
			uINewsEvaluatorBase.OnValueChanged += _callback;
			return uINewsEvaluatorBase;
		}

		private static bool IsNewsTypeUnlocked(ENewsType _type)
		{
			switch (_type)
			{
			case ENewsType.Social:
				return UIButtonHeaderUtil.IsButtonHeaderTypeUnlocked(UIButtonHeader.EHeaderType.Social);
			case ENewsType.Assassins:
				return UIButtonHeaderUtil.IsButtonHeaderTypeUnlocked(UIButtonHeader.EHeaderType.Assassin);
			case ENewsType.Codex:
				return UIButtonHeaderUtil.IsButtonHeaderTypeUnlocked(UIButtonHeader.EHeaderType.Codex);
			case ENewsType.Shop:
				return UIButtonHeaderUtil.IsButtonHeaderTypeUnlocked(UIButtonHeader.EHeaderType.Shop);
			case ENewsType.Event:
				return UIButtonHeaderUtil.IsButtonHeaderTypeUnlocked(UIButtonHeader.EHeaderType.Events);
			default:
				return true;
			}
		}

		private void Update()
		{
			if (base.gameObject.activeInHierarchy && m_evaluator != null)
			{
				m_icon.spriteName = m_evaluator.DisplayIcon;
				m_evaluator.Update();
			}
		}
	}
}
