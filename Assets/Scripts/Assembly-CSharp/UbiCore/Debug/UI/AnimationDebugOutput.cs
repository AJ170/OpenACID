using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Selection;
using SharedBaseLib.System.Extensions;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace UbiCore.Debug.UI
{
	[RequireComponent(typeof(GUIText))]
	public class AnimationDebugOutput : MonoBehaviour
	{
		private bool m_ShowPendingAnimationTasks;

		private bool m_ShowActiveTasks;

		private bool m_ShowStateMachineInfo;

		private bool m_ShowGeneralStateInfo;

		private bool m_ShowBoneLocation;

		private bool m_ShowCurrentParameters;

		private bool m_ShowGameObjectAndAgentPositions;

		private Rect m_Rect = default(Rect);

		private Vector3 m_LastRootMotionTranslate = Vector3.zero;

		private Vector3 m_CurrentRootMotionTranslate = Vector3.zero;

		private AnimationController m_PlayerAnimationController;

		private AnimationController m_PreviousController;

		private List<AnimatorStateMachineStatic.Parameter> m_Parameters = new List<AnimatorStateMachineStatic.Parameter>();

		public bool ShowActiveTasks
		{
			get
			{
				return m_ShowActiveTasks;
			}
			set
			{
				m_ShowActiveTasks = value;
				base.enabled = GetIsEnabled();
			}
		}

		public bool ShowPendingAnimationTasks
		{
			get
			{
				return m_ShowPendingAnimationTasks;
			}
			set
			{
				m_ShowPendingAnimationTasks = value;
				base.enabled = GetIsEnabled();
			}
		}

		public bool ShowStateMachineInfo
		{
			get
			{
				return m_ShowStateMachineInfo;
			}
			set
			{
				m_ShowStateMachineInfo = value;
				base.enabled = GetIsEnabled();
			}
		}

		public bool ShowGeneralStateInfo
		{
			get
			{
				return m_ShowGeneralStateInfo;
			}
			set
			{
				m_ShowGeneralStateInfo = value;
				base.enabled = GetIsEnabled();
			}
		}

		public bool ShowBoneLocation
		{
			get
			{
				return m_ShowBoneLocation;
			}
			set
			{
				m_ShowBoneLocation = value;
				base.enabled = GetIsEnabled();
			}
		}

		public bool ShowGameObjectAndAgentPositions
		{
			get
			{
				return m_ShowGameObjectAndAgentPositions;
			}
			set
			{
				m_ShowGameObjectAndAgentPositions = value;
				base.enabled = GetIsEnabled();
			}
		}

		public bool ShowCurrentParameters
		{
			get
			{
				return m_ShowCurrentParameters;
			}
			set
			{
				m_ShowCurrentParameters = value;
				base.enabled = GetIsEnabled();
			}
		}

		public bool UseLogicSelection { get; set; }

		public AnimationController AnimationController
		{
			get
			{
				AnimationController animationController = null;
				if (UseLogicSelection && UbiCore.Utils.Singleton<SelectionManager>.IsAvailable)
				{
					ISelectable selection = UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection;
					GameObjectEntity gameObjectEntity = selection as GameObjectEntity;
					if (gameObjectEntity != null)
					{
						animationController = gameObjectEntity.UnityComponentCache.AnimationController;
					}
				}
				if (animationController == null && LogicMain.PlayerEntity != null)
				{
					if (m_PlayerAnimationController == null && LogicMain.PlayerEntity != null && LogicMain.PlayerEntity.GameObjectProxy != null && LogicMain.PlayerEntity.GameObjectProxy.GameObject != null)
					{
						m_PlayerAnimationController = LogicMain.PlayerEntity.GameObjectProxy.GameObject.GetComponent<AnimationController>();
					}
					animationController = m_PlayerAnimationController;
				}
				return animationController;
			}
		}

		private void Start()
		{
			base.transform.position = new Vector3(0.025f, 0.975f, 0f);
			base.transform.parent = base.transform;
			GetComponent<GUIText>().fontSize = 12;
			GetComponent<GUIText>().font = Resources.Load("UBIGAME TEXT L BOLD 32", typeof(Font)) as Font;
		}

		private void OnGUI()
		{
			GUI.depth = -1;
			if (AnimationController != null && AnimationController.gameObject != null && AnimationController.gameObject.activeInHierarchy)
			{
				OnRenderActiveTasks();
				OnRenderStateMachineInfo();
				OnRenderGeneralStateInfo();
				OnRenderCurrentParameters();
				OnRenderPendingAnimationTasks();
			}
		}

		private void OnRenderActiveTasks()
		{
			if (!ShowActiveTasks)
			{
				return;
			}
			string text = string.Format("Active Tasks ({0}):\n\n", AnimationController.gameObject.name);
			if (AnimationController.TaskManager != null)
			{
				for (int i = 0; i < AnimationController.Animator.layerCount; i++)
				{
					text += string.Format("Layer {0}:\n", i);
					ReadOnlyCollection<AnimationTaskBase> currentTasks = AnimationController.TaskManager.GetCurrentTasks((AnimationTaskManager.ELayer)i);
					for (int j = 0; j < currentTasks.Count; j++)
					{
						text += string.Format("{0}\n", currentTasks[j].ToString());
						for (int k = 0; k < currentTasks[j].StateCount; k++)
						{
							text += string.Format("  {0} {1} {2}\n", currentTasks[j].GetStateName(k), (currentTasks[j].CurrentStateIndex != k) ? string.Empty : "*", currentTasks[j].GetIsStateEnabled(k) ? string.Empty : " - Disabled");
						}
					}
				}
			}
			IStateMachinesCarrier stateMachinesCarrier = AnimationController.TryGetEntity<IStateMachinesCarrier>();
			if (stateMachinesCarrier != null)
			{
				ICharacterConditionCarrier characterConditionCarrier = stateMachinesCarrier.StateMachines as ICharacterConditionCarrier;
				if (characterConditionCarrier != null)
				{
					text += string.Format("\n\nCondition: {0}\n\n", characterConditionCarrier.CharacterCondition.CurrentStateType);
				}
			}
			DebugHelper.DrawOutlineLabel(new Rect(0f, 0f, Screen.width, Screen.height / 2), text);
		}

		private void OnRenderStateMachineInfo()
		{
			if (!ShowStateMachineInfo)
			{
				return;
			}
			string text = string.Format("State Machine ({0}):\n\n", AnimationController.gameObject.name);
			Animator component = AnimationController.GetComponent<Animator>();
			if (component != null && AnimationController.StateMachine != null)
			{
				for (int i = 0; i < component.layerCount; i++)
				{
					AnimatorStateInfo currentAnimatorStateInfo = component.GetCurrentAnimatorStateInfo(i);
					AnimatorClipInfo[] currentAnimationClipState = AnimationController.DataController.GetCurrentAnimationClipState(i);
					text += string.Format("Current State: {0}\n", AnimationController.StateMachine.GetStateNameByNameHash(currentAnimatorStateInfo.fullPathHash));
					text += string.Format("  Normalized Time: {0:0.00}\n", currentAnimatorStateInfo.normalizedTime - (float)(int)currentAnimatorStateInfo.normalizedTime);
					text += string.Format("  Duration (s): {0:0.00}\n", currentAnimatorStateInfo.length);
					text += string.Format("  Loop Count: {0}\n", (int)currentAnimatorStateInfo.normalizedTime);
					text += string.Format("  Clips:\n", (int)currentAnimatorStateInfo.normalizedTime);
					for (int j = 0; j < currentAnimationClipState.Length; j++)
					{
						text += string.Format("    {0} - Weight: {1:0.0}\n", currentAnimationClipState[j].clip.name, currentAnimationClipState[j].weight);
					}
					if (component.IsInTransition(i))
					{
						AnimatorTransitionInfo animatorTransitionInfo = component.GetAnimatorTransitionInfo(i);
						AnimatorStateMachineStatic.Transition transition = AnimationController.StateMachine.GetTransition(animatorTransitionInfo.fullPathHash);
						text = ((transition == null) ? (text + string.Format("\nTransition unknown: Hash {0} {1}\n", animatorTransitionInfo.fullPathHash, (animatorTransitionInfo.fullPathHash != 0) ? string.Empty : "(CrossFade/Play active?)")) : (text + string.Format("\nTransition ({1}): {0:0.00}\n\tName: {2}\n", animatorTransitionInfo.normalizedTime, (!transition.Atomic) ? "Non-Atomic" : "Atomic", transition.Name)));
					}
					else
					{
						text += "\nTransition: -\n";
					}
					AnimatorStateInfo nextAnimatorStateInfo = component.GetNextAnimatorStateInfo(i);
					AnimatorClipInfo[] nextAnimatorClipInfo = component.GetNextAnimatorClipInfo(i);
					text += string.Format("\nNext State: {0}\n", AnimationController.StateMachine.GetStateNameByNameHash(nextAnimatorStateInfo.fullPathHash));
					text += string.Format("  Normalized Time: {0:0.00}\n", nextAnimatorStateInfo.normalizedTime - (float)(int)nextAnimatorStateInfo.normalizedTime);
					text += string.Format("  Duration (s): {0:0.00}\n", nextAnimatorStateInfo.length);
					text += string.Format("  Loop Count: {0}\n", (int)nextAnimatorStateInfo.normalizedTime);
					text += string.Format("  Clips:\n", (int)nextAnimatorStateInfo.normalizedTime);
					for (int k = 0; k < nextAnimatorClipInfo.Length; k++)
					{
						text += string.Format("    {0} - Weight: {1:0.0}\n", nextAnimatorClipInfo[k].clip.name, nextAnimatorClipInfo[k].weight);
					}
					if (i < component.layerCount - 1)
					{
						text += string.Format("---\n", (int)nextAnimatorStateInfo.normalizedTime);
					}
				}
			}
			DebugHelper.DrawOutlineLabel(new Rect(Screen.width / 2, 1f, Screen.width / 2, Screen.height), text);
		}

		private void OnRenderGeneralStateInfo()
		{
			if (ShowGeneralStateInfo && AnimationController.Animator != null)
			{
				string text = string.Format("General State ({0}):\n\n", AnimationController.gameObject.name);
				Vector3 vector = AnimationController.Animator.rootPosition - m_LastRootMotionTranslate;
				text += string.Format("Match Target: {0}\n", AnimationController.Animator.isMatchingTarget);
				text += string.Format("Root Motion (Animator): {0}\n", AnimationController.Animator.applyRootMotion);
				text += string.Format("Root Motion (AnimationController): {0}\n", AnimationController.ApplyRootMotion);
				text += string.Format("\tTranslation: {0} Delta: {1}\n", AnimationController.Animator.rootPosition, vector);
				text += string.Format("\tRotation: {0}\n", AnimationController.Animator.rootRotation.eulerAngles);
				text += string.Format("Auto Traverse: {0}\n", AnimationController.Agent.autoTraverseOffMeshLink);
				text += string.Format("Agent enabled: {0}\n", AnimationController.Agent.enabled);
				text += string.Format("Update Position: {0}\n", AnimationController.Agent.updatePosition);
				text += string.Format("Update Rotation: {0}\n", AnimationController.Agent.updateRotation);
				text += string.Format("Upper Body Layer Weight: {0}\n", AnimationController.Animator.GetLayerWeight(1));
				text += string.Format("On link: {0}\n", AnimationController.Agent.isOnOffMeshLink);
				text += string.Format("Current link: {0} - Start {1} - End: {2}\n", AnimationController.Agent.currentOffMeshLinkData.offMeshLink, AnimationController.Agent.currentOffMeshLinkData.startPos, AnimationController.Agent.currentOffMeshLinkData.endPos);
				text += string.Format("Next link: {0} - Start {0} - End: {1}\n", AnimationController.Agent.nextOffMeshLinkData.offMeshLink, AnimationController.Agent.nextOffMeshLinkData.startPos, AnimationController.Agent.nextOffMeshLinkData.endPos);
				text += string.Format("GameObject Pos.: {0}\n", AnimationController.gameObject.transform.position);
				text += string.Format("Agent Pos.: {0}\n", AnimationController.Agent.transform.position);
				text += string.Format("Agent Speed: {0}\n", AnimationController.Agent.speed);
				text += string.Format("Agent Velocity: {0}\n", AnimationController.Agent.velocity);
				text += string.Format("Steering Target: {0}\n", AnimationController.Agent.steeringTarget);
				text += string.Format("Has Path: {0}\n", AnimationController.Agent.hasPath);
				NavMeshPath path = AnimationController.Agent.path;
				Vector3[] corners = path.corners;
				for (int i = 0; i < corners.Length; i++)
				{
					text += string.Format("\t{0}: {1}\n", i, corners[i]);
				}
				DebugHelper.DrawOutlineLabel(new Rect(0f, Screen.height / 2, Screen.width, Screen.height / 2), text);
			}
		}

		private void OnRenderCurrentParameters()
		{
			if (!ShowCurrentParameters || AnimationController.StateMachine == null)
			{
				return;
			}
			float num = Screen.width / 2;
			float num2 = Screen.width / 6;
			int num3 = 0;
			string text = "Parameters:\n\n";
			if (AnimationController != m_PreviousController)
			{
				Dictionary<int, AnimatorStateMachineStatic.Parameter> defaultParameters = AnimationController.StateMachine.DefaultParameters;
				m_Parameters.Clear();
				foreach (AnimatorStateMachineStatic.Parameter value in defaultParameters.Values)
				{
					m_Parameters.Add(value);
				}
				m_Parameters.OrderBy((AnimatorStateMachineStatic.Parameter p) => p.Name, (string a, string b) => string.Compare(a, b, StringComparison.Ordinal));
				m_PreviousController = AnimationController;
			}
			int count = m_Parameters.Count;
			for (int i = 0; i < count; i++)
			{
				AnimatorStateMachineStatic.Parameter parameter = m_Parameters[i];
				switch (parameter.Type)
				{
				case AnimatorStateMachineStatic.Parameter.EType.Bool:
					text += string.Format("{0}: {1}\n", parameter.Name, AnimationController.Animator.GetBool(parameter.NameHash));
					break;
				case AnimatorStateMachineStatic.Parameter.EType.Float:
					text += string.Format("{0}: {1:0.00}\n", parameter.Name, AnimationController.Animator.GetFloat(parameter.NameHash));
					break;
				case AnimatorStateMachineStatic.Parameter.EType.Integer:
					text += string.Format(arg1: (parameter.NameHash == AnimatorParameters.MovementState) ? ((MovementState)AnimationController.Animator.GetInteger(parameter.NameHash)).ToString() : ((parameter.NameHash == AnimatorParameters.GeneralState) ? ((GeneralState)AnimationController.Animator.GetInteger(parameter.NameHash)).ToString() : ((parameter.NameHash == AnimatorParameters.ActionType) ? ((ActionType)AnimationController.Animator.GetInteger(parameter.NameHash)).ToString() : ((parameter.NameHash == AnimatorParameters.ClimbDirectionHash) ? ((eDirection)AnimationController.Animator.GetInteger(parameter.NameHash)).ToString() : ((parameter.NameHash == AnimatorParameters.CurrentLinkTypeHash) ? ((eLinkType)AnimationController.Animator.GetInteger(parameter.NameHash)).ToString() : ((parameter.NameHash == AnimatorParameters.ReactionType) ? ((ReactionType)AnimationController.Animator.GetInteger(parameter.NameHash)).ToString() : ((parameter.NameHash == AnimatorParameters.TurnDirectionHash) ? ((TurnDirection)AnimationController.Animator.GetInteger(parameter.NameHash)).ToString() : ((parameter.NameHash != AnimatorParameters.ActionTypeUpperBody) ? AnimationController.Animator.GetInteger(parameter.NameHash).ToString() : ((ActionTypeUpperBody)AnimationController.Animator.GetInteger(parameter.NameHash)).ToString()))))))), format: "{0}: {1}\n", arg0: parameter.Name);
					break;
				}
				if (num3 >= 20)
				{
					m_Rect.x = num;
					m_Rect.y = Screen.height / 2;
					m_Rect.width = Screen.width;
					m_Rect.height = Screen.height / 2;
					DebugHelper.DrawOutlineLabel(m_Rect, text);
					text = "\n\n";
					num3 = 0;
					num += num2;
				}
				else
				{
					num3++;
				}
			}
			m_Rect.x = num;
			m_Rect.y = Screen.height / 2;
			m_Rect.width = Screen.width;
			m_Rect.height = Screen.height / 2;
			DebugHelper.DrawOutlineLabel(m_Rect, text);
		}

		private void OnRenderPendingAnimationTasks()
		{
			if (ShowPendingAnimationTasks)
			{
				List<AnimationPendingTasks.PendingAnimation> list = AnimationPendingTasks.Pending.ToList();
				list.Sort(SortByAnimationTaskIdAsc);
				string text = string.Format("# Pending Animation Tasks ({0}):\n\n", list.Count);
				for (int i = 0; i < list.Count; i++)
				{
					text += string.Format("Task: Id: {0} - Type: {1} - Owner: {2} ({3}) - Exited: {4} - Disposed: {5}\n", list[i].Id, list[i].Type, list[i].Owner, list[i].OwnerType, list[i].Exited, list[i].Disposed);
				}
				DebugHelper.DrawOutlineLabel(new Rect(0f, 0f, Screen.width, Screen.height), text);
			}
		}

		private static int SortByAnimationTaskIdAsc(AnimationPendingTasks.PendingAnimation _left, AnimationPendingTasks.PendingAnimation _right)
		{
			return _left.Id.CompareTo(_right.Id);
		}

		private void OnDrawGizmos()
		{
			if (ShowBoneLocation)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(AnimationController.Animator.GetBoneTransform(HumanBodyBones.LeftHand).position, 0.15f);
				Gizmos.DrawSphere(AnimationController.Animator.GetBoneTransform(HumanBodyBones.RightHand).position, 0.15f);
				Gizmos.DrawSphere(AnimationController.Animator.GetBoneTransform(HumanBodyBones.LeftFoot).position, 0.15f);
				Gizmos.DrawSphere(AnimationController.Animator.GetBoneTransform(HumanBodyBones.RightFoot).position, 0.15f);
			}
			if (ShowGameObjectAndAgentPositions)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawCube(AnimationController.Agent.transform.position, new Vector3(0.4f, 0.4f, 0.4f));
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(AnimationController.gameObject.transform.position, 0.25f);
			}
		}

		private bool GetIsEnabled()
		{
			return ShowActiveTasks || ShowStateMachineInfo || ShowGeneralStateInfo || ShowBoneLocation || ShowCurrentParameters || ShowGameObjectAndAgentPositions || m_ShowPendingAnimationTasks;
		}

		private void Update()
		{
			m_LastRootMotionTranslate = m_CurrentRootMotionTranslate;
			if (AnimationController != null && AnimationController.Animator != null)
			{
				m_CurrentRootMotionTranslate = AnimationController.Animator.rootPosition;
			}
		}
	}
}
