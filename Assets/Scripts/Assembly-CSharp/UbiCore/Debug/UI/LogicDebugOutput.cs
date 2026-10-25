using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using AcierProtocol;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion.WithoutAgent;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.Ingame.Implementation;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Tutorial;
using Assets.Scripts.UI.Tutorial.Flow;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace UbiCore.Debug.UI
{
	[RequireComponent(typeof(GUIText))]
	public class LogicDebugOutput : MonoBehaviour
	{
		private const string TutorialCurrentMark = "=>";

		private const string TutorialNotLoaded = "not loaded";

		private readonly StringBuilder m_debugTriggersBuilder = new StringBuilder(500);

		public bool ShowBehavior { get; set; }

		public string ShowBehaviorEntitySlot { get; set; }

		public bool ShowActiveStateMachines { get; set; }

		public bool ShowActiveBuffs { get; set; }

		public bool ShowTutorialStatus { get; set; }

		public bool ShowSkillWheelStatus { get; set; }

		public bool ShowMisc { get; set; }

		public Enum<SkillTypes> SkillToDebug { get; set; }

		public static LogicDebugOutput Instance { get; private set; }

		public EntityId CurrentEntity
		{
			get
			{
				EntityId entityId = EntityId.InvalidId;
				if (entityId == EntityId.InvalidId && LogicMain.PlayerEntity != null)
				{
					entityId = LogicMain.PlayerEntity.Id;
				}
				return entityId;
			}
		}

		public LogicDebugOutput()
		{
			Instance = this;
		}

		private void OnGUI()
		{
			GUI.depth = -1;
			OnRenderActiveStateMachines();
			OnRenderActiveBuffs();
			OnRenderTutorialStatus();
			OnRenderMisc();
			OnRenderShowSkillWheelStatus();
			OnRenderBehavior();
		}

		private void OnRenderBehavior()
		{
			if (!ShowBehavior || string.IsNullOrEmpty(ShowBehaviorEntitySlot))
			{
				return;
			}
			uint slot = uint.Parse(ShowBehaviorEntitySlot);
			EntityId entityId = LogicMain.EntityManager.GetEntityId(slot, 1);
			GameObject gameObject = ((entityId == EntityId.InvalidId) ? null : entityId.GameEntity.GameObject);
			string text = string.Format("Npc ({0}):\n\n", (!(gameObject != null)) ? "?" : gameObject.name);
			if (entityId == EntityId.InvalidId || !(entityId.Entity is NpcCharacter))
			{
				return;
			}
			NpcCharacter npcCharacter = entityId.Entity as NpcCharacter;
			NpcRuntimeData treeRuntimeData = npcCharacter.CharacterAi.TreeRuntimeData;
			BBList<BehaviorComponent> lastTicked = treeRuntimeData.LastTicked;
			for (int i = 0; i < lastTicked.Count; i++)
			{
				BehaviorComponent behaviorComponent = lastTicked[i];
				text += string.Format("{0}({1}) {2}\n", behaviorComponent.Name, behaviorComponent.BehaviorType, treeRuntimeData.RequestBehaviorRuntimeData(behaviorComponent).ReturnCode);
			}
			text += "\nPatrol:\n";
			Assets.Scripts.Mission.Logic.Entities.Patrol patrol = npcCharacter.Patrol;
			text += string.Format("PatrolP {0} NpcP {1}\n", patrol.Position, npcCharacter.Position);
			Assets.Scripts.Mission.Logic.Components.Patrol patrolComponent = patrol.PatrolComponent;
			text += string.Format("WIndex {0} WIdleT {1} WP {2} idles {3} reloadTrig {4} iterationT {5} remainingIdle {6} iterDir {7} iterDirCast {8}\n", patrolComponent.CurrentWaypointIndex, patrolComponent.CurrentWaypoint.IdleTime, patrolComponent.CurrentWaypoint.Position, patrolComponent.Idles, patrolComponent.IsTriggerReloadRoute, patrolComponent.IterationType, patrolComponent.RemainingIdleTime, patrolComponent.MissionData.Data.IterationDirection, (int)patrolComponent.MissionData.Data.IterationDirection);
			PatrolTargetApproacher approacher = patrolComponent.Approacher;
			text += string.Format("MinMemberDSqr {0} Speed {1}\n", approacher.MinMemberDeviationSqr, approacher.Speed);
			text += "\nRoute:\n";
			if (patrolComponent.CurrentRoute != null)
			{
				for (int j = 0; j < patrolComponent.CurrentRoute.Count; j++)
				{
					Assets.Scripts.Mission.Logic.Components.Patrol.Waypoint waypoint = patrolComponent.CurrentRoute[j];
					text += string.Format("{0} - p {1} minS {2} maxS {3} ic {4} it {5} rot {6} rt {7} warp {8} id {9}\n", j, waypoint.Position, waypoint.MinSpeed, waypoint.MaxSpeed, waypoint.IdleCategory, waypoint.IdleTime, waypoint.Rotation, waypoint.RotationType, waypoint.WarpTarget, waypoint.ID);
				}
			}
			DebugHelper.DrawOutlineLabel(new Rect(0f, 0f, Screen.width, Screen.height / 2), text.ToString());
		}

		private void OnRenderActiveStateMachines()
		{
			if (!ShowActiveStateMachines)
			{
				return;
			}
			EntityId currentEntity = CurrentEntity;
			GameObject gameObject = ((currentEntity == EntityId.InvalidId) ? null : currentEntity.GameEntity.GameObject);
			string text = string.Format("Active State-Machines ({0}):\n\n", (!(gameObject != null)) ? "?" : gameObject.name);
			if (currentEntity == EntityId.InvalidId || !(currentEntity.Entity is IStateMachinesCarrier))
			{
				return;
			}
			IStateMachinesCarrier entity = currentEntity.GetEntity<IStateMachinesCarrier>();
			if (entity != null)
			{
				ReadOnlyCollection<IStateMachine> stateMachinesAccess = entity.StateMachines.StateMachinesAccess;
				int count = stateMachinesAccess.Count;
				for (int i = 0; i < count; i++)
				{
					text += string.Format("\t{0}\n", stateMachinesAccess[i].GetType().Name);
					text += string.Format("\t\t{0} {1}\n", stateMachinesAccess[i].CurrentStateType.ToString(), (stateMachinesAccess[i].ScheduledStateType == EStateType.Invalid) ? string.Empty : string.Format(" -> {0}", stateMachinesAccess[i].ScheduledStateType));
					text += "\n";
				}
				DebugHelper.DrawOutlineLabel(new Rect(0f, 0f, Screen.width, Screen.height / 2), text);
			}
		}

		private void OnRenderActiveBuffs()
		{
			if (!ShowActiveBuffs)
			{
				return;
			}
			EntityId currentEntity = CurrentEntity;
			GameObject gameObject = ((currentEntity == EntityId.InvalidId) ? null : currentEntity.GameEntity.GameObject);
			StringBuilder stringBuilder = new StringBuilder(string.Format("Active Buffs ({0}):\n\n", (!(gameObject != null)) ? "?" : gameObject.name), 1000);
			if (currentEntity == EntityId.InvalidId)
			{
				return;
			}
			IBuffsCarrier entity = currentEntity.GetEntity<IBuffsCarrier>();
			if (entity == null)
			{
				return;
			}
			int currentTurn = LogicMain.CurrentTurn;
			Buffs buffs = entity.Buffs;
			ReadOnlyCollection<IBuff> sortedBuffs = buffs.SortedBuffs;
			foreach (IBuff item in sortedBuffs)
			{
				stringBuilder.AppendFormat("\t{0}\n", item.Type);
				int num = currentTurn - item.StartTurn;
				if (item.DurationType == EBuffDurationType.Finite)
				{
					stringBuilder.AppendFormat("\t\t duration type: {0} {1}/{2}\n", item.DurationType, num, item.TurnDuration);
				}
				else
				{
					stringBuilder.AppendFormat("\t\t duration type: {0} {1}\n", item.DurationType, num);
				}
				stringBuilder.Append("\n");
			}
			DebugHelper.DrawOutlineLabel(new Rect(Screen.width / 2, 0f, Screen.width, Screen.height / 2), stringBuilder.ToString());
		}

		private void OnRenderTutorialStatus()
		{
			if (!ShowTutorialStatus)
			{
				return;
			}
			UITutorialManager tutorialManager = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager;
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			StringBuilder stringBuilder = new StringBuilder(string.Format("TutorialActive:{0}\n", tutorialManager.IsTutorialActive, 1000));
			stringBuilder.Append(string.Format("Category\n\n"));
			StringBuilder stringBuilder2 = new StringBuilder(string.Format("\nTutorial\tProfile\n\n"), 1000);
			TutorialCategory currentCategory = tutorialManager.CurrentCategory;
			for (int i = 0; i < tutorialManager.Categories.Length; i++)
			{
				TutorialCategory tutorialCategory = tutorialManager.Categories[i];
				string arg = "=>";
				if (i != currentCategory.Index)
				{
					arg = ((int)tutorialCategory.Category).ToString("d2");
				}
				stringBuilder.AppendFormat("{0} {1}:\n", arg, tutorialCategory.Category);
				stringBuilder2.AppendFormat("{0}-{1}", (int)tutorialCategory.CurrentFlowId, tutorialCategory.Finished);
				if (profile != null)
				{
					Tutorial.Category profileTutorialCategory = TutorialHelper.GetProfileTutorialCategory(profile, tutorialCategory.Category);
					int num = 0;
					bool flag = TutorialHelper.OwningFinishedEntitlements(tutorialCategory.Category);
					bool flag2 = false;
					if (profileTutorialCategory != null)
					{
						num = profileTutorialCategory.counter;
						flag2 = profileTutorialCategory.finished;
					}
					stringBuilder2.AppendFormat("\t{0}-{1}-{2}\n", num, flag, flag2);
				}
				else
				{
					stringBuilder2.Append("\n");
				}
			}
			stringBuilder.AppendFormat("\nFlowables (active-allowed) Name ScreenId:{0}\n", Singleton<UIScreenManager>.Instance.CurrentScreenId);
			TutorialCategory tutorialCategory2 = tutorialManager.Categories[currentCategory.Index];
			EFlowId maxOfCategory = FlowIdHelper.GetMaxOfCategory(currentCategory.Category);
			for (EFlowId eFlowId = FlowIdHelper.GetMinOfCategory(currentCategory.Category); eFlowId <= maxOfCategory; eFlowId++)
			{
				int num2 = (int)eFlowId;
				object obj;
				if (num2 < tutorialCategory2.Tutorials.Count)
				{
					ITutorialFlowable tutorialFlowable = tutorialCategory2.Tutorials[num2];
					obj = tutorialFlowable;
				}
				else
				{
					obj = null;
				}
				ITutorialFlowable tutorialFlowable2 = (ITutorialFlowable)obj;
				string text = "not loaded";
				string text2 = "not loaded";
				string text3 = "not loaded";
				if (tutorialFlowable2 != null)
				{
					text = tutorialFlowable2.IsActive.ToString();
					text2 = tutorialFlowable2.ConditionAllowShow().ToString();
					text3 = tutorialFlowable2.GetType().Name;
				}
				stringBuilder.AppendFormat("{0}: {1}-{2} {3}\n", num2, text, text2, text3);
			}
			float num3 = (float)Screen.width / 2f;
			DebugHelper.DrawOutlineLabel(new Rect(num3, 0f, 200f, Screen.height), stringBuilder.ToString());
			DebugHelper.DrawOutlineLabel(new Rect(num3 + 200f, 0f, Screen.width / 2 - 200, Screen.height), stringBuilder2.ToString());
		}

		private void OnRenderMisc()
		{
			if (!ShowMisc)
			{
				return;
			}
			float x = (float)Screen.width / 2f;
			int num = 0;
			for (int i = 0; i < LogicMain.EntityManager.EnemyCharacterCount; i++)
			{
				if (!LogicMain.EntityManager.GetEnemyCharacter(i).GetDeadOrAboutToDie())
				{
					num++;
				}
			}
			DebugHelper.DrawOutlineLabel(new Rect(x, 0f, 200f, Screen.height), string.Format("Enemy count: {0}", num));
		}

		private void OnRenderShowSkillWheelStatus()
		{
			if (!ShowSkillWheelStatus)
			{
				return;
			}
			UISkills subSystemByType = UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UISkills>();
			if (subSystemByType != null)
			{
				StringBuilder stringBuilder = new StringBuilder("Skill Wheel Buttons:\n");
				stringBuilder.Append("Name\n\n");
				StringBuilder stringBuilder2 = new StringBuilder("\nUIStatus\n\n", 1000);
				StringBuilder stringBuilder3 = new StringBuilder("\nLogic Status\n\n", 1000);
				List<UISkillButtonBehaviour> allSkillButtons = subSystemByType.GetAllSkillButtons();
				for (int i = 0; i < allSkillButtons.Count; i++)
				{
					UISkillButtonBehaviour uISkillButtonBehaviour = allSkillButtons[i];
					stringBuilder.AppendFormat("{0}\n", uISkillButtonBehaviour.Direction);
					stringBuilder2.AppendFormat("{0}\n", uISkillButtonBehaviour.CurrentSkillStatus);
					stringBuilder3.AppendFormat("{0}\n", uISkillButtonBehaviour.LastLogicSubType);
				}
				float num = (float)Screen.width / 2f;
				DebugHelper.DrawOutlineLabel(new Rect(num, 0f, 100f, Screen.height), stringBuilder.ToString());
				DebugHelper.DrawOutlineLabel(new Rect(num + 150f, 0f, Screen.width / 2 - 100, Screen.height), stringBuilder2.ToString());
				DebugHelper.DrawOutlineLabel(new Rect(num + 300f, 0f, Screen.width / 2 - 100, Screen.height), stringBuilder3.ToString());
			}
		}

		[Conditional("UNITY_DEBUG")]
		private void OnShowRuleTriggers()
		{
			if (!(SkillToDebug == Enum<SkillTypes>.Invalid))
			{
				string text = m_debugTriggersBuilder.ToString();
				DebugHelper.DrawOutlineLabel(new Rect(0f, (float)Screen.height / 2f, Screen.width, (float)Screen.height / 2f), text);
			}
		}

		private static bool DoDebugTriggers(IRule _rule)
		{
			if (Instance == null)
			{
				return false;
			}
			ISkillRule skillRule = _rule as ISkillRule;
			return skillRule != null && skillRule.SkillType == Instance.SkillToDebug;
		}

		[Conditional("UNITY_DEBUG")]
		public static void DebugTriggersReset(IRule _rule)
		{
			if (DoDebugTriggers(_rule))
			{
				Instance.m_debugTriggersBuilder.Length = 0;
				Instance.m_debugTriggersBuilder.Append("Rule: ").Append(Instance.SkillToDebug.Name).Append("\n");
			}
		}

		[Conditional("UNITY_DEBUG")]
		public static void DebugTriggersSubmit(IRule _rule, ITrigger _trigger, bool _checked, bool _fullfilled)
		{
			if (DoDebugTriggers(_rule))
			{
				StringBuilder debugTriggersBuilder = Instance.m_debugTriggersBuilder;
				string value = _trigger.GetType().Name;
				debugTriggersBuilder.Append(value);
				if (_checked)
				{
					debugTriggersBuilder.Append((!_fullfilled) ? "  FAILED" : "  ACTIVE");
				}
				else
				{
					debugTriggersBuilder.Append("  NOT CHECKED");
				}
				debugTriggersBuilder.Append("\n");
			}
		}
	}
}
