using System.Text;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Network.Missions.Helpers;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.UI.Combat
{
	public class CombatAnnotation : ICombatAnnotation
	{
		public enum Type
		{
			Damage = 0,
			CriticalDamage = 1,
			Heal = 2,
			Timer = 3,
			DodgedNormal = 4,
			DodgedMiss = 5,
			KillMoveDeflect = 6,
			SoftCurrency = 7,
			SkillAvailable = 8,
			SkillUnavailable = 9,
			MissionGoalLootMundane = 10,
			MissionGoalLootCommon = 11,
			MissionGoalLootUncommon = 12,
			MissionGoalLootRare = 13,
			MissionGoalLootPerfect = 14
		}

		private const float PositionZ = 0f;

		private const int c_TickSoundTimer = 10;

		private static GameObject Pool;

		private static UnityEngine.Object Prefab;

		private static Vector2 RootDimensions;

		private static BBList<CombatAnnotation> Instances;

		private static int s_backgroundHitIndex = -1;

		private IGameEntity m_target;

		private float m_creationTime;

		private float m_lifeTime;

		private float m_scale = 1f;

		private Vector3 m_positionAnchor;

		private bool m_positionInitialized;

		private Vector2 m_stackOffset = Vector2.zero;

		public UIWidget m_container;

		public UIExtendedLabel m_label;

		public UISprite m_icon;

		public UISprite m_background;

		private float Alpha
		{
			get
			{
				return m_container.alpha;
			}
			set
			{
				m_container.alpha = value;
			}
		}

		public override IGameEntity Target
		{
			get
			{
				return m_target;
			}
		}

		public override float Height
		{
			get
			{
				return m_container.transform.localScale.y * (float)m_label.height;
			}
		}

		public override float StackOffset
		{
			get
			{
				return m_stackOffset.y;
			}
			set
			{
				m_stackOffset.y = value;
			}
		}

		public static void Preload()
		{
			Prefab = UnityEngine.Resources.Load("UI/Combat/FloatingAnnotation");
			Pool = GameObject.Find("CombatAnnotationPool");
			UIPanel component = Pool.GetComponent<UIPanel>();
			RootDimensions = new Vector2(component.width, component.height);
			component.gameObject.transform.localPosition = new Vector3((0f - RootDimensions.x) / 2f, (0f - RootDimensions.y) / 2f, 0f);
			Instances = new BBList<CombatAnnotation>(5);
			for (int i = 0; i < 5; i++)
			{
				CreateAnnotation();
			}
		}

		public static void Unload()
		{
			Prefab = null;
			Instances = null;
			Pool = null;
		}

		private static CombatAnnotation Get()
		{
			CombatAnnotation combatAnnotation = Instances.Find((CombatAnnotation _d) => !_d.gameObject.activeSelf);
			if (combatAnnotation == null)
			{
				combatAnnotation = CreateAnnotation();
			}
			return combatAnnotation;
		}

		private static CombatAnnotation CreateAnnotation()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Prefab) as GameObject;
			gameObject.SetActive(false);
			gameObject.transform.parent = Pool.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			CombatAnnotation component = gameObject.GetComponent<CombatAnnotation>();
			Instances.Add(component);
			return component;
		}

		public static ICombatAnnotation GetNumber(CombatUiEvent _event, float _initialValue, Type _type)
		{
			return GetNumber(_event.Target, _initialValue, _type);
		}

		public static ICombatAnnotation GetNumber(IGameEntity _target, float _initialValue, Type _type)
		{
			CombatAnnotation combatAnnotation = Get();
			combatAnnotation.m_target = _target;
			combatAnnotation.m_label.UseLocalizedText = false;
			combatAnnotation.ResetToNumber(_initialValue, _type);
			combatAnnotation.gameObject.SetActive(true);
			return combatAnnotation;
		}

		public static ICombatAnnotation GetLocalizationKey(IGameEntity _target, Type _type, int _locaKey, StringMapper _variables)
		{
			CombatAnnotation combatAnnotation = Get();
			combatAnnotation.m_target = _target;
			combatAnnotation.ResetToLocalizationKey(_type, _locaKey, _variables);
			combatAnnotation.gameObject.SetActive(true);
			return combatAnnotation;
		}

		public static ICombatAnnotation GetLocalizationKey(CombatUiEvent _event, Type _type)
		{
			int locaKey = -1;
			StringMapper variables = null;
			switch (_type)
			{
			case Type.DodgedNormal:
				locaKey = 5069;
				break;
			case Type.DodgedMiss:
				locaKey = 6454;
				break;
			case Type.KillMoveDeflect:
				locaKey = 5274;
				break;
			case Type.SkillAvailable:
			{
				locaKey = 5640;
				StringMapper stringMapper = new StringMapper();
				stringMapper.Add("Skill_name", GetLocalizedSkillName(_event));
				variables = stringMapper;
				break;
			}
			case Type.SkillUnavailable:
			{
				locaKey = 5641;
				StringMapper stringMapper = new StringMapper();
				stringMapper.Add("Skill_name", GetLocalizedSkillName(_event));
				variables = stringMapper;
				break;
			}
			}
			return GetLocalizationKey(_event.Target, _type, locaKey, variables);
		}

		public static Type GetType(Quality.EQuality _quality)
		{
			return (Type)(10 + _quality - 1);
		}

		private static string GetLocalizedSkillName(CombatUiEvent _event)
		{
			return LocalizationManager.Instance.Get(_event.SkillType.GetDefinition().NameLocaKey.Key);
		}

		private static int GetNextBackgroundHitIndex()
		{
			s_backgroundHitIndex++;
			return s_backgroundHitIndex % 3;
		}

		public override void Release()
		{
			base.gameObject.SetActive(false);
			TriggerOnRelease();
			m_target = null;
		}

		private void ResetToNumber(float _initialValue, Type _type)
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (_type)
			{
			case Type.CriticalDamage:
				AppendNumber(_initialValue, stringBuilder);
				stringBuilder.Append('!');
				break;
			case Type.Timer:
				if (_initialValue <= 10f)
				{
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_timer_1s_ping);
				}
				AppendNumber(_initialValue, stringBuilder);
				break;
			default:
				AppendNumber(_initialValue, stringBuilder);
				break;
			case Type.DodgedNormal:
			case Type.DodgedMiss:
			case Type.KillMoveDeflect:
				break;
			}
			m_label.text = stringBuilder.ToString();
			ResetTo(_type);
		}

		private static void AppendNumber(float _initialValue, StringBuilder _sb)
		{
			_sb.Append(MissionMathHelper.RoundToInt(_initialValue));
		}

		private void ResetToLocalizationKey(Type _type, int _locaKey, StringMapper _variables = null)
		{
			m_label.SetLocalizationKey(_locaKey, _variables);
			ResetTo(_type);
		}

		private void ResetTo(Type _type)
		{
			m_positionInitialized = false;
			m_creationTime = LogicMain.CurrentTime;
			m_stackOffset = Vector2.zero;
			CombatUiData combatUiData = CombatController.Instance.CombatUiData;
			switch (_type)
			{
			case Type.Damage:
				m_label.color = combatUiData.m_Digits_ColorHit;
				m_label.effectStyle = UILabel.Effect.None;
				m_scale = combatUiData.m_Digits_ScaleHit;
				m_background.spriteName = combatUiData.m_Digits_BackgroundHit[GetNextBackgroundHitIndex()];
				m_background.leftAnchor.target = m_label.transform;
				m_lifeTime = combatUiData.m_Digits_LifeTime;
				m_icon.gameObject.SetActive(false);
				break;
			case Type.CriticalDamage:
				m_label.color = combatUiData.m_Digits_ColorCrit;
				m_label.effectStyle = UILabel.Effect.None;
				m_scale = combatUiData.m_Digits_ScaleCrit;
				m_background.spriteName = combatUiData.m_Digits_BackgroundHit[GetNextBackgroundHitIndex()];
				m_background.leftAnchor.target = m_label.transform;
				m_lifeTime = combatUiData.m_Digits_LifeTime;
				m_icon.gameObject.SetActive(false);
				break;
			case Type.Heal:
				m_label.color = combatUiData.m_Digits_ColorHeal;
				m_label.effectStyle = UILabel.Effect.None;
				m_scale = combatUiData.m_Digits_ScaleHeal;
				m_background.spriteName = combatUiData.m_Digits_BackgroundHeal;
				m_background.leftAnchor.target = m_label.transform;
				m_lifeTime = combatUiData.m_Digits_LifeTime;
				m_icon.gameObject.SetActive(false);
				break;
			case Type.Timer:
				m_label.color = combatUiData.m_Digits_ColorTimer;
				m_label.effectStyle = UILabel.Effect.None;
				m_scale = combatUiData.m_Digits_ScaleTimer;
				m_background.spriteName = combatUiData.m_Digits_BackgroundTimer;
				m_background.leftAnchor.target = m_label.transform;
				m_lifeTime = combatUiData.m_Digits_LifeTime;
				m_icon.gameObject.SetActive(false);
				break;
			case Type.DodgedNormal:
				m_label.color = combatUiData.m_Digits_ColorDodged;
				m_label.effectStyle = UILabel.Effect.None;
				m_scale = combatUiData.m_Digits_ScaleDodged;
				m_background.spriteName = combatUiData.m_Digits_BackgroundGeneral;
				m_background.leftAnchor.target = m_label.transform;
				m_lifeTime = combatUiData.m_Digits_LifeTime;
				m_icon.gameObject.SetActive(false);
				break;
			case Type.DodgedMiss:
				m_label.color = combatUiData.m_Digits_ColorDodgedMiss;
				m_label.effectStyle = UILabel.Effect.None;
				m_scale = combatUiData.m_Digits_ScaleDodgedMiss;
				m_background.spriteName = combatUiData.m_Digits_BackgroundGeneral;
				m_background.leftAnchor.target = m_label.transform;
				m_lifeTime = combatUiData.m_Digits_LifeTime;
				m_icon.gameObject.SetActive(false);
				break;
			case Type.KillMoveDeflect:
				m_label.color = combatUiData.m_Digits_ColorKillMoveDeflect;
				m_label.effectStyle = UILabel.Effect.None;
				m_scale = combatUiData.m_Digits_ScaleKillMoveDeflect;
				m_background.spriteName = combatUiData.m_Digits_BackgroundGeneral;
				m_background.leftAnchor.target = m_label.transform;
				m_lifeTime = combatUiData.m_Digits_LifeTime;
				m_icon.gameObject.SetActive(false);
				break;
			case Type.SoftCurrency:
			{
				m_label.color = combatUiData.m_Digits_ColorCoins;
				m_label.effectStyle = UILabel.Effect.None;
				m_scale = combatUiData.m_Digits_ScaleCoins;
				m_background.spriteName = combatUiData.m_Digits_BackgroundGeneral;
				m_lifeTime = combatUiData.m_Digits_LifeTime;
				CurrencyDescription value = null;
				if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions.TryGetValue(2, out value))
				{
					m_icon.spriteName = value.SmallIconName;
					m_icon.gameObject.SetActive(true);
					m_background.leftAnchor.target = m_icon.transform;
				}
				else
				{
					m_icon.gameObject.SetActive(false);
					m_background.leftAnchor.target = m_label.transform;
				}
				break;
			}
			case Type.SkillAvailable:
			case Type.SkillUnavailable:
				m_label.color = combatUiData.m_Digits_ColorDefault;
				m_label.effectStyle = UILabel.Effect.None;
				m_scale = combatUiData.m_Digits_ScaleDefault;
				m_background.spriteName = combatUiData.m_Digits_BackgroundGeneral;
				m_background.leftAnchor.target = m_label.transform;
				m_lifeTime = combatUiData.m_Digits_LifeTime;
				m_icon.gameObject.SetActive(false);
				break;
			case Type.MissionGoalLootCommon:
			{
				ColorDef item = Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_commonColor);
				m_label.color = item.Color0;
				m_label.effectStyle = UILabel.Effect.Outline;
				m_label.effectDistance = new Vector2(1f, 1f);
				m_scale = combatUiData.m_Digits_ScaleItems;
				m_background.spriteName = combatUiData.m_Digits_BackgroundGeneral;
				m_background.leftAnchor.target = m_label.transform;
				m_lifeTime = combatUiData.m_Digits_LootLifetime;
				m_icon.gameObject.SetActive(false);
				break;
			}
			case Type.MissionGoalLootMundane:
			{
				ColorDef item = Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_mundaneColor);
				m_label.color = item.Color0;
				m_label.effectStyle = UILabel.Effect.Outline;
				m_label.effectDistance = new Vector2(1f, 1f);
				m_scale = combatUiData.m_Digits_ScaleItems;
				m_background.spriteName = combatUiData.m_Digits_BackgroundGeneral;
				m_background.leftAnchor.target = m_label.transform;
				m_lifeTime = combatUiData.m_Digits_LootLifetime;
				m_icon.gameObject.SetActive(false);
				break;
			}
			case Type.MissionGoalLootPerfect:
			{
				ColorDef item = Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_perfectColor);
				m_label.color = item.Color0;
				m_label.effectStyle = UILabel.Effect.Outline;
				m_label.effectDistance = new Vector2(1f, 1f);
				m_scale = combatUiData.m_Digits_ScaleItems;
				m_background.spriteName = combatUiData.m_Digits_BackgroundGeneral;
				m_background.leftAnchor.target = m_label.transform;
				m_lifeTime = combatUiData.m_Digits_LootLifetime;
				m_icon.gameObject.SetActive(false);
				break;
			}
			case Type.MissionGoalLootRare:
			{
				ColorDef item = Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_rareColor);
				m_label.color = item.Color0;
				m_label.effectStyle = UILabel.Effect.Outline;
				m_label.effectDistance = new Vector2(1f, 1f);
				m_scale = combatUiData.m_Digits_ScaleItems;
				m_background.spriteName = combatUiData.m_Digits_BackgroundGeneral;
				m_background.leftAnchor.target = m_label.transform;
				m_lifeTime = combatUiData.m_Digits_LootLifetime;
				m_icon.gameObject.SetActive(false);
				break;
			}
			case Type.MissionGoalLootUncommon:
			{
				ColorDef item = Main.Instance.GameDB.GetItem<ColorDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_uncommonColor);
				m_label.color = item.Color0;
				m_label.effectStyle = UILabel.Effect.Outline;
				m_label.effectDistance = new Vector2(1f, 1f);
				m_scale = combatUiData.m_Digits_ScaleItems;
				m_background.spriteName = combatUiData.m_Digits_BackgroundGeneral;
				m_background.leftAnchor.target = m_label.transform;
				m_lifeTime = combatUiData.m_Digits_LootLifetime;
				m_icon.gameObject.SetActive(false);
				break;
			}
			}
			if (Singleton<EffectManager>.Instance.HasCondition(EffectManager.ECondition.ReducedGore) && combatUiData.m_Digits_BackgroundHit.Contains(m_background.spriteName))
			{
				m_background.spriteName = combatUiData.m_Digits_BackgroundGeneral;
			}
			Alpha = 1f;
			UpdatePosAndAlpha();
		}

		private void LateUpdate()
		{
			UpdatePosAndAlpha();
		}

		private void UpdatePosAndAlpha()
		{
			if (CombatController.Instance == null)
			{
				Release();
				return;
			}
			CombatUiData combatUiData = CombatController.Instance.CombatUiData;
			float num = LogicMain.CurrentTime - m_creationTime;
			float a = Mathf.InverseLerp(0f, m_lifeTime, num);
			a = Mathf.Max(a, 0f);
			if (a >= 1f || Target.Id == EntityId.InvalidId || !Target.IsAttached)
			{
				Release();
				return;
			}
			float scale = Interpolate.Ease01(combatUiData.m_Digits_ScaleInterpolation, a);
			UpdateScale(scale);
			float num2 = Interpolate.Ease01(combatUiData.m_Digits_MoveInterpolation, a);
			Vector3 vector = CalcTargetPosition3D();
			if (m_positionInitialized)
			{
				float num3 = Mathf.Min(LogicMain.FrameTime * combatUiData.m_Digits_PositionBlendFactor, 1f);
				m_positionAnchor = num3 * vector + (1f - num3) * m_positionAnchor;
			}
			else
			{
				m_positionAnchor = vector;
				m_positionInitialized = true;
			}
			Vector2 vector2 = UnityEngine.Camera.main.WorldToScreenPoint(m_positionAnchor);
			vector2 = new Vector2(vector2.x * RootDimensions.x / (float)Screen.width, vector2.y * RootDimensions.y / (float)Screen.height);
			Vector2 vector3 = m_stackOffset + combatUiData.m_Digits_Offset2d + combatUiData.m_Digits_TargetOffset2d * num2;
			vector2 += vector3;
			base.transform.localPosition = new Vector3(vector2.x, vector2.y, 0f);
			float num4 = (num - m_lifeTime + combatUiData.m_Digits_FadeOutTime) / combatUiData.m_Digits_FadeOutTime;
			if (num4 > 0f)
			{
				num4 = Mathf.Min(num4, 1f);
				Alpha = 1f - Interpolate.Ease01(combatUiData.m_Digits_FadeInterpolation, num4);
			}
		}

		private Vector3 CalcTargetPosition3D()
		{
			Character character = Target as Character;
			if (character != null)
			{
				if (character.IsAttached)
				{
					Animator animator = character.UnityComponentCache.Animator;
					return animator.GetBoneTransform(HumanBodyBones.Head).position;
				}
				return Target.Position + new Vector3(0f, 1.7f, 0f);
			}
			return Target.Position;
		}

		private void UpdateScale(float _scale = 1f)
		{
			_scale *= CalcNonAnimScale();
			m_container.transform.localScale = new Vector3(_scale, _scale, 1f);
		}

		private float CalcNonAnimScale()
		{
			CombatUiData combatUiData = CombatController.Instance.CombatUiData;
			return combatUiData.m_Digits_BaseScaleFactor * m_scale;
		}

		private static void DestroySafe(UnityEngine.Object _object)
		{
			UnityEngine.Object.Destroy(_object);
		}
	}
}
