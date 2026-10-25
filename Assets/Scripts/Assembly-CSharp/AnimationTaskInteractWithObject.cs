using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Network.Missions.Mission.Enums;
using UnityEngine;

public class AnimationTaskInteractWithObject : AnimationTaskBase, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public EntityId Id { get; set; }

		public InteractionLocators.Locator SelectedLocator { get; set; }

		public int LoopCount { get; set; }

		public Parameters()
			: base(AnimationTaskType.InteractableObject)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.InteractableObject;

	private readonly EObjectCategory m_ObjectCategory;

	private readonly EObjectSubCategory m_objectSubCategory;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.InteractableObject;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	private GameObject TargetGameObject
	{
		get
		{
			return InteractableObjectId.GetEntity<IGameEntity>().GameObject;
		}
	}

	public EntityId InteractableObjectId { get; private set; }

	public AnimationTaskInteractWithObject()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskInteractWithObject(Parameters _parameters)
		: base(_parameters)
	{
		InteractableObjectId = _parameters.Id;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Invalid);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		InteractableObject entity = InteractableObjectId.GetEntity<InteractableObject>();
		m_ObjectCategory = entity.InteractableObjectDefinition.Definition.Category;
		m_objectSubCategory = entity.InteractableObjectDefinition.Definition.SubCategory;
		InteractionLocators.Locator locator = _parameters.SelectedLocator;
		bool flag = AnimationController.Animator.IsCurrentRightFootFront();
		if ((flag && locator.Position.name == "Front_locator_footl") || (!flag && locator.Position.name != "Front_locator_footl"))
		{
			ReadOnlyCollection<InteractionLocators.Locator> locators = entity.InteractionLocators.Locators;
			for (int i = 0; i < locators.Count; i++)
			{
				if (locator != locators[i])
				{
					locator = locators[i];
					break;
				}
			}
		}
		OnAddState(new AnimationStateLootChest(_parameters.AnimationController, m_ObjectCategory, m_objectSubCategory, locator, _parameters.LoopCount));
	}

	public override bool CanCancel()
	{
		return true;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.GetComponent<AnimationController>().DataController.RegisterEventListener(this);
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		if (_isCanceled)
		{
			SetChestOpened(false);
		}
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.GetComponent<AnimationController>().DataController.UnregisterEventListener(this);
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		AnimatorEventType type = _event.Type;
		if (type == AnimatorEventType.InteractWithObject)
		{
			OnInteractWithObjectEvent();
		}
		return false;
	}

	private void OnInteractWithObjectEvent()
	{
		SetChestOpened(true);
	}

	private void SetChestOpened(bool _opened)
	{
		if (!(TargetGameObject != null))
		{
			return;
		}
		Transform transform = TargetGameObject.transform.FindChildRecursive("Chest_Loot_Ground");
		if (transform != null)
		{
			ChestType value = ChestType.OpenChestGround;
			EObjectCategory objectCategory = m_ObjectCategory;
			if (objectCategory == EObjectCategory.Mission_EObjectCategory && m_objectSubCategory.HasFlag(EObjectSubCategory.Chest))
			{
				value = (m_objectSubCategory.HasFlag(EObjectSubCategory.Elevated) ? ChestType.OpenChestElevated : ChestType.OpenChestGround);
			}
			Animator component = transform.GetComponent<Animator>();
			component.SetBool("OpenChest", _opened);
			component.SetInteger("ChestType", (int)value);
			if (!_opened)
			{
				component.CrossFade(AnimationStateHashes.State_ChestIdle.NameHash, 0.1f);
			}
		}
	}
}
