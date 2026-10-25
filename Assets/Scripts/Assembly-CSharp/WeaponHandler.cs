using Assets.Scripts.Animation;
using Assets.Scripts.Audio;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

[RequireComponent(typeof(AnimatorDataController))]
[RequireComponent(typeof(Animator))]
public class WeaponHandler : MonoBehaviour, IAnimatorEventListener
{
	public class WeaponDesc
	{
		public ItemDef.EWeaponTypes Type { get; private set; }

		public EWeaponCategory Category { get; private set; }

		public EAudioWeaponType AudioType { get; private set; }

		public string DrawBone { get; private set; }

		public string HideBone { get; private set; }

		public string HideOffsetBone { get; private set; }

		public Vector3 Rotation { get; private set; }

		public AnimatorStateMachineStatic.StateMachineHashValue UnsheathedAnimStateMachine { get; private set; }

		public AnimatorStateMachineStatic.StateHashValue DropState { get; private set; }

		public AnimatorStateMachineStatic.StateHashValue DropStateWeapon { get; private set; }

		public AnimatorStateMachineStatic.StateHashValue AimState { get; private set; }

		public AnimatorStateMachineStatic.StateHashValue AimStateWeapon { get; private set; }

		public AnimatorStateMachineStatic.StateHashValue ShootState { get; private set; }

		public WeaponDesc(ItemDef.EWeaponTypes _type, EWeaponCategory _category, EAudioWeaponType _audioType, string _drawBone, string _hideBone, string _hideOffsetBone, Vector3 _rotation, AnimatorStateMachineStatic.StateMachineHashValue _unsheathedAnimStateMachine, AnimatorStateMachineStatic.StateHashValue _dropState, AnimatorStateMachineStatic.StateHashValue _dropStateWeapon, AnimatorStateMachineStatic.StateHashValue _aimState, AnimatorStateMachineStatic.StateHashValue _aimStateWeapon, AnimatorStateMachineStatic.StateHashValue _shootState)
		{
			Type = _type;
			Category = _category;
			AudioType = _audioType;
			DrawBone = _drawBone;
			HideBone = _hideBone;
			HideOffsetBone = _hideOffsetBone;
			Rotation = _rotation;
			UnsheathedAnimStateMachine = _unsheathedAnimStateMachine;
			DropState = _dropState;
			DropStateWeapon = _dropStateWeapon;
			AimState = _aimState;
			AimStateWeapon = _aimStateWeapon;
			ShootState = _shootState;
		}
	}

	public delegate void WeaponDescDelegate(WeaponDesc _desc);

	private const float DropStateTransitionDuration = 0.1f;

	private static readonly BBDictionary<int, WeaponDesc> WeaponDescs = new BBDictionary<int, WeaponDesc>
	{
		{
			1,
			new WeaponDesc(ItemDef.EWeaponTypes.Dagger, EWeaponCategory.Melee, EAudioWeaponType.Stab, "RightHand_Weapon_Ref_Jnt", "P_Dagger_Tag_Jnt", null, new Vector3(-180f, 0f, 0f), null, null, null, null, null, null)
		},
		{
			2,
			new WeaponDesc(ItemDef.EWeaponTypes.Bow, EWeaponCategory.Ranged, EAudioWeaponType.Arrow, "LeftHand_Weapon_Ref_Jnt", "Bow_Tag_Jnt", null, Vector3.zero, AnimationStateMachineHashes.StateMachine_CombatRangeBow, AnimationStateHashes.State_LowProfile_Idle_Footm, AnimationStateHashes.State_Bow_Idle, AnimationStateHashes.State_AimLoopBow, AnimationStateHashes.BowAnimationStateHashes.State_AimLoopBow, AnimationStateHashes.State_ShootBow)
		},
		{
			4,
			new WeaponDesc(ItemDef.EWeaponTypes.Crossbow, EWeaponCategory.Ranged, EAudioWeaponType.Arrow, "RightHand_Weapon_Ref_Jnt", "crossbow_Tag_Jnt", "offset", Vector3.zero, null, null, null, AnimationStateHashes.State_AimLoopCrossbow, AnimationStateHashes.CrossbowAnimationStateHashes.State_LoadedCrossbow, AnimationStateHashes.State_ShootCrossbow)
		},
		{
			8,
			new WeaponDesc(ItemDef.EWeaponTypes.Sword, EWeaponCategory.Melee, EAudioWeaponType.Slash, "RightHand_Weapon_Ref_Jnt", "P_LeftWaist_Tag_Jnt", null, Vector3.zero, null, null, null, null, null, null)
		},
		{
			16,
			new WeaponDesc(ItemDef.EWeaponTypes.TwoHand, EWeaponCategory.Melee, EAudioWeaponType.Slash, "RightHand_Weapon_Ref_Jnt", "P_LeftWaist_Tag_Jnt", null, Vector3.zero, null, null, null, null, null, null)
		},
		{
			32,
			new WeaponDesc(ItemDef.EWeaponTypes.Quiver, EWeaponCategory.Ranged, EAudioWeaponType.Arrow, "Quiver_Tag_Jnt", "Quiver_Tag_Jnt", null, Vector3.zero, null, null, null, null, null, null)
		},
		{
			64,
			new WeaponDesc(ItemDef.EWeaponTypes.Arrow, EWeaponCategory.Ranged, EAudioWeaponType.Arrow, string.Empty, string.Empty, null, Vector3.zero, null, null, null, null, null, null)
		},
		{
			128,
			new WeaponDesc(ItemDef.EWeaponTypes.Bolt, EWeaponCategory.Ranged, EAudioWeaponType.Arrow, string.Empty, string.Empty, null, Vector3.zero, null, null, null, null, null, null)
		},
		{
			256,
			new WeaponDesc(ItemDef.EWeaponTypes.Polearm, EWeaponCategory.Melee, EAudioWeaponType.Stab, "RightHand_Weapon_Ref_Jnt", string.Empty, null, Vector3.zero, null, null, null, null, null, null)
		}
	};

	private GameObject m_goWeapon;

	private GameObject m_hideBone;

	private GameObject m_drawBone;

	private Vector3 m_hideOffset;

	private WeaponDesc m_weaponDesc;

	private bool m_weaponDrawn;

	private Quaternion m_fallStartRotation;

	private Quaternion m_fallEndRotation;

	private Vector3 m_fallStartPosition;

	private Vector3 m_fallEndPosition;

	private float m_fallStartTime;

	private bool m_falling;

	private bool m_attached = true;

	private bool m_shown = true;

	public WeaponDesc CurrentWeaponDesc
	{
		get
		{
			return m_weaponDesc;
		}
		private set
		{
			WeaponDesc weaponDesc = m_weaponDesc;
			m_weaponDesc = value;
			if (this.OnCurrentWeaponChanged != null)
			{
				this.OnCurrentWeaponChanged(weaponDesc);
			}
		}
	}

	public Animator Animator { get; private set; }

	public bool WeaponDrawn
	{
		get
		{
			return m_weaponDrawn;
		}
		set
		{
			if (m_weaponDrawn != value && Assets.Scripts.Engine.Helpers.Verify(m_goWeapon != null, "Invalid usage - WeaponHandler has no Weapon to Draw/Hide"))
			{
				GameObject gameObject = ((!value) ? m_hideBone : m_drawBone);
				m_goWeapon.transform.parent = gameObject.transform;
				m_goWeapon.transform.localPosition = ((!value) ? m_hideOffset : Vector3.zero);
				m_goWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
				m_weaponDrawn = value;
			}
		}
	}

	public event WeaponDescDelegate OnCurrentWeaponChanged;

	public static EWeaponType WeaponTypesToType(ItemDef.EWeaponTypes _types)
	{
		EWeaponType result = EWeaponType.Invalid;
		switch (_types)
		{
		case ItemDef.EWeaponTypes.Bow:
			result = EWeaponType.Bow;
			break;
		case ItemDef.EWeaponTypes.Crossbow:
			result = EWeaponType.Crossbow;
			break;
		}
		return result;
	}

	private void Start()
	{
	}

	public void Initialize(ItemDef _def)
	{
		if (_def.ItemType != 0)
		{
			_def = Main.Instance.GameDB.GetItem<ItemDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_fallbackWeapon);
		}
		CurrentWeaponDesc = WeaponDescs[(int)_def.WeaponType];
		m_goWeapon = ItemHelper.CreateItem(_def);
		m_hideBone = base.gameObject.FindChildByName(CurrentWeaponDesc.HideBone);
		if (m_hideBone == null)
		{
			m_hideBone = base.gameObject;
		}
		m_drawBone = base.gameObject.FindChildByName(CurrentWeaponDesc.DrawBone);
		if (m_drawBone == null)
		{
			m_drawBone = base.gameObject;
		}
		GameObject gameObject = ((CurrentWeaponDesc.HideOffsetBone != null) ? m_goWeapon.FindChildByName(CurrentWeaponDesc.HideOffsetBone) : null);
		if (gameObject != null)
		{
			m_hideOffset = -gameObject.transform.localPosition;
		}
		m_goWeapon.transform.parent = m_hideBone.transform;
		m_goWeapon.transform.localPosition = Vector3.zero;
		m_goWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
		Renderer[] componentsInChildren = m_goWeapon.GetComponentsInChildren<Renderer>();
		Rendering.ConfigureLightProbes(m_goWeapon);
		Animator = m_goWeapon.GetComponent<Animator>();
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		if (CurrentWeaponDesc == null || m_goWeapon == null)
		{
			return false;
		}
		switch (_event.Type)
		{
		case AnimatorEventType.DrawWeapon:
			WeaponDrawn = true;
			break;
		case AnimatorEventType.HideWeapon:
		{
			WeaponDrawn = false;
			PlayerPreviewIdleBehaviour component = GetComponent<PlayerPreviewIdleBehaviour>();
			if (component != null)
			{
				component.IsPlayingWeaponEquipAnimation = false;
			}
			break;
		}
		}
		return false;
	}

	public void Detach()
	{
		StopListenForAnimatorEvents();
		WeaponDrawn = false;
	}

	public void StartListenForAnimatorEvents()
	{
		base.gameObject.GetComponent<AnimatorDataController>().RegisterEventListener(this);
	}

	public void StopListenForAnimatorEvents()
	{
		base.gameObject.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
	}

	public void ShowWeapon(bool _show)
	{
		m_goWeapon.SetActive(_show);
		Renderer[] componentsInChildren = m_goWeapon.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = _show && m_attached;
		}
		m_shown = _show;
	}

	public void ForceAttach(bool _attach)
	{
		m_attached = _attach;
		ShowWeapon(_attach && m_shown);
	}

	public void DropToFloor(Character _owner)
	{
		NavMeshAgent component = base.gameObject.GetComponent<NavMeshAgent>();
		if ((bool)component)
		{
			Vector3 position = component.transform.position;
			Vector3 position2 = m_goWeapon.transform.position;
			Vector3 vector = position2 - position;
			vector.y = 0f;
			Vector3 normalized = vector.normalized;
			Vector3 vector2 = new Vector3(position2.x, position.y, position2.z) + normalized * 0.1f;
			NavMeshHit _hit;
			if (NavigationHelper.CascadingSamplePosition(vector2, out _hit, -1))
			{
				vector2 = _hit.position;
			}
			Vector3 forward = m_goWeapon.transform.forward;
			Vector3 vector3 = Vector3.Lerp(forward, normalized, 0.2f);
			if (NavigationHelper.CascadingSamplePosition(vector2 + vector3 * 0.1f, out _hit, -1))
			{
				Vector3 normalized2 = (_hit.position - vector2).normalized;
				if (normalized2.sqrMagnitude > float.Epsilon)
				{
					vector3 = normalized2;
				}
			}
			RaycastHit hitInfo;
			if (Physics.Raycast(vector2 + Vector3.up * 0.3f, Vector3.down, out hitInfo, 0.6f))
			{
				vector2 = hitInfo.point;
			}
			m_fallStartPosition = position2;
			m_fallEndPosition = vector2;
			if (m_fallEndPosition.y > m_fallStartPosition.y)
			{
				m_fallEndPosition.y = m_fallStartPosition.y;
			}
			m_fallStartRotation = m_goWeapon.transform.rotation;
			m_fallEndRotation = Quaternion.LookRotation(vector3, Vector3.up) * Quaternion.Euler(0f, 0f, 90f * Mathf.Sign(m_goWeapon.transform.right.y));
			m_fallStartTime = LogicMain.CurrentTime;
			m_goWeapon.transform.parent = null;
			m_falling = true;
			m_attached = false;
		}
		AnimatorStateMachineStatic.StateHashValue dropStateWeapon = CurrentWeaponDesc.DropStateWeapon;
		if (dropStateWeapon != null)
		{
			Animator.Play(dropStateWeapon.NameHash);
			Animator.SetCurrentStopAction(true);
		}
		AnimatorStateMachineStatic.StateMachineHashValue unsheathedAnimStateMachine = CurrentWeaponDesc.UnsheathedAnimStateMachine;
		if (unsheathedAnimStateMachine != null && CurrentWeaponDesc.DropState != null)
		{
			AnimationController component2 = base.gameObject.GetComponent<AnimationController>();
			AnimatorStateMachineStatic.StateMachine currentStateMachine = component2.StateMachine.GetCurrentStateMachine();
			if (currentStateMachine.CompareNameHash(unsheathedAnimStateMachine))
			{
				component2.Animator.CrossFade(CurrentWeaponDesc.DropState.NameHash, 0.1f);
				Animator.SetCurrentStopAction(true);
			}
		}
	}

	private void LateUpdate()
	{
		if (m_falling)
		{
			float f = LogicMain.CurrentTime - m_fallStartTime;
			float num = -9.81f * Mathf.Pow(f, 2f) / 2f;
			float num2 = Mathf.Clamp01(num / (m_fallEndPosition.y - m_fallStartPosition.y));
			m_goWeapon.transform.localRotation = Quaternion.Slerp(m_fallStartRotation, m_fallEndRotation, num2);
			m_goWeapon.transform.localPosition = Vector3.Lerp(m_fallStartPosition, m_fallEndPosition, num2);
			if (num2 >= 1f)
			{
				m_falling = false;
			}
		}
	}
}
