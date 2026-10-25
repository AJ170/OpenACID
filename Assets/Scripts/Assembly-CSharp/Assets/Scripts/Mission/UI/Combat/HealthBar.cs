using Assets.Scripts.Engine.ExtensionMethods;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.UI.Ingame;
using SharedBaseLib.Core;
using UbiCore;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.UI.Combat
{
	public class HealthBar : ICombatAnnotation
	{
		private const float FadeTime = 1f;

		public static bool ForceInvisible = true;

		private static GameObject Pool;

		private static Object Prefab = null;

		private static BBList<HealthBar> Instances;

		private static readonly float[] ReferenceBgSize = new float[2] { 512f, 128f };

		private static readonly float[] ReferenceBarSize = new float[2] { 485f, 25f };

		private static readonly float[] ReferenceBarOffset = new float[2] { 10f, 0f };

		private float m_alpha = 1f;

		private bool m_dirtyAlpha = true;

		private float m_scale = 1f;

		private float m_healthBarPercentage;

		private float m_healthBarPercentageOld;

		private float m_healthBarPercentageChangeTime;

		private IGameEntity m_target;

		private float m_fadeOutTime;

		private float m_creationTime;

		private bool m_destroyMaterials;

		private bool m_ownOverrideValue = !ForceInvisible;

		public GameObject m_background;

		public GameObject m_bar;

		public GameObject m_barHit;

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
				return m_scale * ReferenceBgSize[1];
			}
		}

		public override float StackOffset { get; set; }

		public float Alpha
		{
			get
			{
				return m_alpha;
			}
			set
			{
				if (m_alpha != value)
				{
					m_dirtyAlpha = true;
					m_alpha = value;
				}
			}
		}

		public float HealthBarPercentage
		{
			get
			{
				return m_healthBarPercentage;
			}
			set
			{
				float num = Mathf.Clamp01(value);
				if (!MathHelper.Approximately(num, m_healthBarPercentage))
				{
					m_healthBarPercentageOld = m_healthBarPercentage;
					m_healthBarPercentage = num;
					m_healthBarPercentageChangeTime = LogicMain.CurrentTime;
				}
			}
		}

		private Vector3 Position3d { get; set; }

		public static void Preload()
		{
			Prefab = Resources.Load("UI/Combat/HealthBar");
			GameObject target = GameObject.FindGameObjectWithTag("UiIconCamera");
			Pool = target.FindDirectChildByName(Assets.Scripts.UI.Ingame.Constants.IconPoolTag);
			int num = CombatController.Instance.MaxParticipantCount - 1;
			Instances = new BBList<HealthBar>(num);
			for (int i = 0; i < num; i++)
			{
				CreateHealthBar();
			}
		}

		public static void Unload()
		{
			Prefab = null;
			Instances = null;
			Pool = null;
		}

		public static HealthBar Get(IGameEntity _target, float _initialValue)
		{
			HealthBar healthBar = Instances.Find((HealthBar _d) => !_d.gameObject.activeSelf);
			if (healthBar == null)
			{
				healthBar = CreateHealthBar();
			}
			healthBar.UpdateScale();
			healthBar.m_target = _target;
			healthBar.gameObject.SetActive(true);
			healthBar.Reset(_initialValue);
			return healthBar;
		}

		private static HealthBar CreateHealthBar()
		{
			GameObject gameObject = Object.Instantiate(Prefab) as GameObject;
			gameObject.transform.parent = Pool.transform;
			HealthBar component = gameObject.GetComponent<HealthBar>();
			component.m_destroyMaterials = false;
			component.Alpha = 0f;
			Instances.Add(component);
			return component;
		}

		public override void Release()
		{
			base.gameObject.SetActive(false);
			m_target = null;
			TriggerOnRelease();
		}

		public void FadeOut()
		{
			m_fadeOutTime = LogicMain.CurrentTime;
		}

		private void Reset(float _initialValue)
		{
			m_healthBarPercentage = _initialValue;
			m_healthBarPercentageOld = _initialValue;
			m_healthBarPercentageChangeTime = (m_creationTime = LogicMain.CurrentTime);
			m_fadeOutTime = -1f;
			m_dirtyAlpha = true;
		}

		private void OnDestroy()
		{
			if (!SharedMain.IsShuttingDown && m_destroyMaterials)
			{
				m_barHit.GetComponent<Renderer>().material.DestroyAuto();
				m_bar.GetComponent<Renderer>().material.DestroyAuto();
				m_background.GetComponent<Renderer>().material.DestroyAuto();
			}
		}

		private void LateUpdate()
		{
			CombatUiData combatUiData = CombatController.Instance.CombatUiData;
			if (!combatUiData.m_ShowHealthBars || m_healthBarPercentage >= 1f || !m_target.IsAttached)
			{
				Alpha = 0f;
				SetAlpha((!ForceInvisible) ? Alpha : 0f);
				return;
			}
			float scale = m_scale;
			if (m_fadeOutTime > 0f)
			{
				float num = LogicMain.CurrentTime - m_fadeOutTime;
				if (!(num < 1f))
				{
					Release();
					return;
				}
				Alpha = 1f - Interpolate.Ease01(Interpolate.EaseType.EaseOutCubic, num / 1f);
			}
			else
			{
				float alpha = Interpolate.Ease01(Interpolate.EaseType.EaseInOutExpo, Mathf.Clamp01((LogicMain.CurrentTime - m_creationTime) / 1f));
				Alpha = alpha;
				Animator animator = m_target.UnityComponentCache.Animator;
				Position3d = animator.GetBoneTransform(HumanBodyBones.Head).position;
			}
			if (ForceInvisible != m_ownOverrideValue)
			{
				m_dirtyAlpha = true;
				m_ownOverrideValue = ForceInvisible;
			}
			SetAlpha((!ForceInvisible) ? Alpha : 0f);
			if (!ForceInvisible)
			{
				Vector3 vector = combatUiData.m_HealthBar_Offset2d * scale * ReferenceBgSize[0];
				Vector2 b = new Vector2(Screen.width, Screen.height);
				Vector3 vector2 = Vector3.Scale(UnityEngine.Camera.main.WorldToScreenPoint(Position3d), new Vector3(1f / (float)UnityEngine.Camera.main.pixelWidth, 1f / (float)UnityEngine.Camera.main.pixelHeight, 1f));
				vector2 = Vector2.Scale(vector2, b);
				Vector2 vector3 = default(Vector2);
				vector3.x = vector2.x + vector.x * combatUiData.m_HealthBar_CustomScale.x;
				vector3.y = vector2.y + vector.y * combatUiData.m_HealthBar_CustomScale.y;
				base.transform.position = new Vector3(vector3.x, vector3.y, 20f);
				base.transform.localScale = new Vector3(base.transform.localScale.x * combatUiData.m_HealthBar_CustomScale.x, base.transform.localScale.y * combatUiData.m_HealthBar_CustomScale.y, 1f);
				m_background.transform.localScale = new Vector3(scale * ReferenceBgSize[0], scale * ReferenceBgSize[1], 1f);
				float num2 = LogicMain.CurrentTime - m_healthBarPercentageChangeTime;
				float num3 = Interpolate.Ease01(combatUiData.m_HealthBar_InterpolationType, Mathf.Clamp01(num2 / combatUiData.m_HealthBar_HitTime));
				float num4 = num3 * m_healthBarPercentage + (1f - num3) * m_healthBarPercentageOld;
				float num5 = 0.5f * scale * ReferenceBarSize[0] * (1f - num4);
				m_bar.transform.localScale = new Vector3(num4 * scale * ReferenceBarSize[0], scale * ReferenceBarSize[1], 1f);
				m_bar.transform.localPosition = new Vector3(scale * ReferenceBarOffset[0] - num5, scale * ReferenceBarOffset[1], -2f);
				float num6 = 0f;
				if (num2 > combatUiData.m_HealthBar_HitTime)
				{
					num6 = Interpolate.Ease01(combatUiData.m_HealthBar_InterpolationType, Mathf.Clamp01((LogicMain.CurrentTime - m_healthBarPercentageChangeTime - combatUiData.m_HealthBar_HitTime) / combatUiData.m_HealthBar_HitFalloffTime));
				}
				float num7 = num6 * m_healthBarPercentage + (1f - num6) * m_healthBarPercentageOld;
				float num8 = 0.5f * scale * ReferenceBarSize[0] * (1f - num7);
				m_barHit.transform.localScale = new Vector3(num7 * scale * ReferenceBarSize[0], scale * ReferenceBarSize[1], 1f);
				m_barHit.transform.localPosition = new Vector3(scale * ReferenceBarOffset[0] - num8, scale * ReferenceBarOffset[1], -1f);
			}
		}

		private void UpdateScale()
		{
			CombatUiData combatUiData = CombatController.Instance.CombatUiData;
			m_scale = (float)Screen.height / (float)combatUiData.m_ReferenceScreenHeight;
		}

		private void SetAlpha(float _alpha)
		{
			if (m_dirtyAlpha)
			{
				SetAlpha(m_background, _alpha);
				SetAlpha(m_bar, _alpha);
				SetAlpha(m_barHit, _alpha);
				base.transform.localScale = new Vector3(1f, _alpha);
				m_destroyMaterials = true;
			}
		}

		private static void SetAlpha(GameObject _obj, float _value)
		{
			_obj.SetActive(_value >= 0.0019607844f);
			Material material = _obj.GetComponent<Renderer>().material;
			Color color = material.color;
			color.a = _value;
			material.color = color;
		}
	}
}
