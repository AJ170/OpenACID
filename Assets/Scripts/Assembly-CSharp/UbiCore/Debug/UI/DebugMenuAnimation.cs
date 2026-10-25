using Assets.Scripts.Mission.Logic;
using UnityEngine;
using UnityEngine.AI;

namespace UbiCore.Debug.UI
{
	public class DebugMenuAnimation : DebugMenuItem
	{
		private AnimationDebugOutput m_DebugOutput;

		public override string GetName()
		{
			return "Animation";
		}

		public override void OnApply()
		{
		}

		public override void OnMenu()
		{
			GUILayout.BeginVertical();
			OnMenuToggleShowActiveTasks();
			OnMenuToggleShowStateMachineInfo();
			OnMenuToggleShowGeneralStateInfo();
			OnMenuToggleShowCurrentParameterInfo();
			OnMenuToggleShowBoneLocation();
			OnMenuToggleShowGameObjectAndAgentPositions();
			OnMenuToggleUseLogicSelection();
			OnMenuAnimationSettings();
			OnMenuToggleShowPendingAnimationTasks();
			GUILayout.EndVertical();
		}

		private void OnMenuAnimationSettings()
		{
			GUI.enabled = true;
			GUILayout.BeginVertical("Box");
			GUILayout.Label("Settings");
			Toggle(ref AnimationStateJumpDown.CompleteOnTaskEndEvent, "Exit JumpDown on TaskEndEvent");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Ground height offset:", GUILayout.Width(300f));
			float.TryParse(GUILayout.TextField(AnimationController.GroundHeightOffset.ToString("F2"), 10, GUILayout.Width(50f)), out AnimationController.GroundHeightOffset);
			AnimationController.GroundHeightOffset = GUILayout.HorizontalSlider(AnimationController.GroundHeightOffset, -0.5f, 0.5f);
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Last used OffMeshLink: ");
			GUILayout.TextField(AssassinExtensions.LastUsedLinkID.ToString());
			GUILayout.EndHorizontal();
		}

		public override void OnOpen()
		{
			if (m_DebugOutput == null)
			{
				GameObject gameObject = new GameObject("AnimationDebugOutput");
				m_DebugOutput = gameObject.AddComponent<AnimationDebugOutput>();
			}
		}

		private void OnMenuToggleShowActiveTasks()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowActiveTasks;
				Toggle(ref value, "Show Active Tasks");
				m_DebugOutput.ShowActiveTasks = value;
			}
		}

		private void OnMenuToggleShowPendingAnimationTasks()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowPendingAnimationTasks;
				Toggle(ref value, "Show Pending Animation Tasks");
				m_DebugOutput.ShowPendingAnimationTasks = value;
			}
		}

		private void OnMenuToggleShowStateMachineInfo()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowStateMachineInfo;
				Toggle(ref value, "Show State Machine Info");
				m_DebugOutput.ShowStateMachineInfo = value;
			}
		}

		private void OnMenuToggleShowGeneralStateInfo()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowGeneralStateInfo;
				Toggle(ref value, "Show General State Info");
				m_DebugOutput.ShowGeneralStateInfo = value;
			}
		}

		private void OnMenuToggleShowCurrentParameterInfo()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowCurrentParameters;
				Toggle(ref value, "Show Current parameters");
				m_DebugOutput.ShowCurrentParameters = value;
			}
		}

		private void OnMenuToggleShowBoneLocation()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowBoneLocation;
				Toggle(ref value, "Show Bone Location");
				m_DebugOutput.ShowBoneLocation = value;
			}
		}

		private void OnMenuToggleShowGameObjectAndAgentPositions()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowGameObjectAndAgentPositions;
				Toggle(ref value, "Show GameObject and NavMeshAgent Positions");
				m_DebugOutput.ShowGameObjectAndAgentPositions = value;
			}
		}

		private void OnMenuToggleUseLogicSelection()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.UseLogicSelection;
				Toggle(ref value, "Use Logic Selection");
				m_DebugOutput.UseLogicSelection = value;
			}
		}

		public bool GetShowBoneLocation()
		{
			bool result = false;
			if (m_DebugOutput != null)
			{
				result = m_DebugOutput.ShowBoneLocation;
			}
			return result;
		}
	}
}
