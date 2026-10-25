using System;
using System.Diagnostics;
using Assets.Scripts.Crowd;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using UbiCore.Utils;
using UnityEngine;

namespace UbiCore.Debug.UI
{
	public class DebugMenuLogic : DebugMenuItem
	{
		private LogicDebugOutput m_DebugOutput;

		private int CrowdSpawnCount = 25;

		private bool m_selectedShowStats;

		private bool m_selectedDebugSkills;

		private Vector2 m_statsScrollPosition = Vector2.zero;

		private int m_debugRulesSelected = -1;

		public override string GetName()
		{
			return "Logic";
		}

		public override void OnMenu()
		{
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			OnMenuToggleShowActiveStateMachines();
			OnMenuToggleShowActiveBuffs();
			OnMenuToggleShowTutorialStatus();
			OnMenuToggleShowMisc();
			OnMenuToggleShowSkillWheelStatus();
			OnMenuTogglePrototyping();
			OnMenuToggleShowBehavior();
			GUILayout.EndHorizontal();
			OnMenuTimeMultiplier();
			OnMenuOptimization();
			OnMenuCrowd();
			OnMenuStats();
			GUILayout.EndVertical();
		}

		private void Slider(string _title, float _in, out float _out, float _min, float _max)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(_title, GUILayout.Width(200f));
			float.TryParse(GUILayout.TextField(_in.ToString("F2"), 10, GUILayout.Width(50f)), out _out);
			_out = GUILayout.HorizontalSlider(_in, _min, _max);
			GUILayout.EndHorizontal();
		}

		private void Slider(string _title, int _in, out int _out, int _min, int _max)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(_title, GUILayout.Width(200f));
			int.TryParse(GUILayout.TextField(_in.ToString(), 10, GUILayout.Width(50f)), out _out);
			_out = Mathf.RoundToInt(GUILayout.HorizontalSlider(_in, _min, _max));
			GUILayout.EndHorizontal();
		}

		private void Field(string _title, int _value)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(_title, GUILayout.Width(200f));
			GUILayout.Label(_value.ToString(), GUILayout.Width(50f));
			GUILayout.EndHorizontal();
		}

		private void Field(string _title, float _value)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(_title, GUILayout.Width(200f));
			GUILayout.Label(_value.ToString("F2"), GUILayout.Width(50f));
			GUILayout.EndHorizontal();
		}

		private void OnMenuCrowd()
		{
			GUILayout.BeginVertical("box", GUILayout.MaxWidth(600f));
			GUILayout.Label("Crowd");
			if (CrowdGraph.Instance.CrowdNodes.Count <= 0)
			{
				GUILayout.Label("Can't spawn crowd: no CrowdGraph in level");
				GUI.enabled = false;
			}
			float _out;
			Slider("Quality", Crowd.CrowdQuality, out _out, 0f, 1f);
			Crowd.CrowdQuality = _out;
			Field("Crowd to spawn", Crowd.Instance.DesiredSpawnCount);
			Field("Update period", CrowdSettings.UpdatePeriod);
			Toggle(ref CrowdAvoidance.EnableCrowdAvoidance, "Enable RVO");
			Slider("Min speed", CrowdSettings.MinSpeed, out CrowdSettings.MinSpeed, 0f, 7f);
			Slider("Max speed", CrowdSettings.MaxSpeed, out CrowdSettings.MaxSpeed, 0f, 7f);
			Slider("Crowd density", Crowd.Instance.CrowdDensity, out Crowd.Instance.CrowdDensity, 0, 2000);
			Field("Crowd entities", Crowd.Instance.CrowdBehaviors.Count);
			Field("Perceived", (float)Crowd.Instance.CurrentCrowd);
			Field("Desired", (float)Crowd.Instance.DesiredCrowd);
			Field("Missing", (float)Crowd.Instance.MissingCrowd);
			Slider("Add entities:", CrowdSpawnCount, out CrowdSpawnCount, 0, 100);
			if (GUILayout.Button("Spawn"))
			{
				Crowd.Instance.SpawnCrowd(CrowdSpawnCount);
			}
			if (GUILayout.Button("Clear"))
			{
				Crowd.Instance.ClearCrowd();
			}
			if (GUILayout.Button("Rebuid index"))
			{
				Crowd.Instance.RebuildNodeIndex();
				Crowd.Instance.RebuildStationIndex();
			}
			GUILayout.EndVertical();
			GUI.enabled = true;
		}

		private void OnMenuStats()
		{
			GameEntity selectedGameEntity = GetSelectedGameEntity();
			IEntityStatsCarrier entityStatsCarrier = selectedGameEntity;
			if (entityStatsCarrier == null || entityStatsCarrier.EntityStats == null)
			{
				return;
			}
			IEntityStats entityStats = entityStatsCarrier.EntityStats;
			GUILayout.BeginVertical("box");
			m_selectedShowStats = GUILayout.Toggle(m_selectedShowStats, "Show stats for " + selectedGameEntity.Name);
			if (m_selectedShowStats)
			{
				GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
				GUIStyle gUIStyle2 = new GUIStyle(GUI.skin.label);
				gUIStyle.normal.textColor = new Color(0f, 0.5f, 1f);
				gUIStyle2.normal.textColor = new Color(0.5f, 0.8f, 1f);
				int num = 0;
				m_statsScrollPosition = GUILayout.BeginScrollView(m_statsScrollPosition);
				foreach (int value in Enum.GetValues(typeof(StatTypes.EStatType)))
				{
					if (value != -1 && value != 387 && entityStats.HasStat((StatTypes.EStatType)value))
					{
						GUIStyle style = ((num % 2 != 0) ? gUIStyle2 : gUIStyle);
						IReadonlyStatValueBase readonlyStatValueBase = entityStats.GetReadonlyStatValueBase((StatTypes.EStatType)value);
						GUILayout.BeginHorizontal();
						GUILayout.Label(((StatTypes.EStatType)value).ToString(), style, GUILayout.Width(300f));
						GUILayout.Label(readonlyStatValueBase.Category.ToString(), style, GUILayout.Width(70f));
						GUILayout.Label(readonlyStatValueBase.ValueType.Name, style, GUILayout.Width(90f));
						GUILayout.Label(readonlyStatValueBase.ToString(), style);
						GUILayout.EndHorizontal();
						GUILayout.Space(-10f);
						num++;
					}
				}
				GUILayout.EndScrollView();
			}
			GUILayout.EndVertical();
		}

		public override void OnOpen()
		{
			if (m_DebugOutput == null)
			{
				GameObject gameObject = new GameObject("LogicDebugOutput");
				m_DebugOutput = gameObject.AddComponent<LogicDebugOutput>();
			}
		}

		public override void OnApply()
		{
		}

		private void OnMenuOptimization()
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("Profiling & optimization");
			Main.ProfilingDetailed = GUILayout.Toggle(Main.ProfilingDetailed, "Detailed Profiling (important overhead on framerate)");
			GUILayout.EndVertical();
		}

		private void OnMenuTimeMultiplier()
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label(string.Format("Time scale: {0}", UbiCore.Utils.Singleton<Timer>.Instance.timeScale.ToString("F2")));
			UbiCore.Utils.Singleton<Timer>.Instance.timeScale = GUILayout.HorizontalSlider(UbiCore.Utils.Singleton<Timer>.Instance.timeScale, 0.1f, 3f);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("x0.1", GUILayout.MinHeight(40f)))
			{
				LogicMain.GlobalTimeScale = 0.1f;
			}
			if (GUILayout.Button("x0.2", GUILayout.MinHeight(40f)))
			{
				LogicMain.GlobalTimeScale = 0.2f;
			}
			if (GUILayout.Button("x0.5", GUILayout.MinHeight(40f)))
			{
				LogicMain.GlobalTimeScale = 0.5f;
			}
			if (GUILayout.Button("x1.0", GUILayout.MinHeight(40f)))
			{
				LogicMain.GlobalTimeScale = 1f;
			}
			if (GUILayout.Button("x2.0", GUILayout.MinHeight(40f)))
			{
				LogicMain.GlobalTimeScale = 2f;
			}
			if (GUILayout.Button("x3.0", GUILayout.MinHeight(40f)))
			{
				LogicMain.GlobalTimeScale = 3f;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}

		private void OnMenuToggleShowBehavior()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowBehavior;
				Toggle(ref value, "Show Behavior for Id(slot):");
				m_DebugOutput.ShowBehavior = value;
				m_DebugOutput.ShowBehaviorEntitySlot = GUILayout.TextField(m_DebugOutput.ShowBehaviorEntitySlot ?? "4");
			}
		}

		private void OnMenuToggleShowActiveStateMachines()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowActiveStateMachines;
				Toggle(ref value, "Show Active State-Machines");
				m_DebugOutput.ShowActiveStateMachines = value;
			}
		}

		private void OnMenuToggleShowActiveBuffs()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowActiveBuffs;
				Toggle(ref value, "Show Active Buffs");
				m_DebugOutput.ShowActiveBuffs = value;
			}
		}

		private void OnMenuToggleShowTutorialStatus()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowTutorialStatus;
				Toggle(ref value, "Show Tutorial Status");
				m_DebugOutput.ShowTutorialStatus = value;
			}
		}

		private void OnMenuToggleShowMisc()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowMisc;
				Toggle(ref value, "Show Misc");
				m_DebugOutput.ShowMisc = value;
			}
		}

		private void OnMenuToggleShowSkillWheelStatus()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowSkillWheelStatus;
				Toggle(ref value, "Show SkillWheel Status");
				m_DebugOutput.ShowSkillWheelStatus = value;
			}
		}

		private void OnMenuTogglePrototyping()
		{
			GUI.enabled = true;
			bool value = !LogicMain.LogicInterface.Prototype_BuffTimerDisabled;
			Toggle(ref value, "Enable Buff Timers");
			LogicMain.LogicInterface.Prototype_BuffTimerDisabled = !value;
		}

		[Conditional("UNITY_DEBUG")]
		private void OnMenuRuleTriggers()
		{
			if (m_DebugOutput == null)
			{
				return;
			}
			GUI.enabled = true;
			GameEntity selectedGameEntity = GetSelectedGameEntity();
			AssassinCharacter assassinCharacter = selectedGameEntity as AssassinCharacter;
			if (assassinCharacter == null)
			{
				return;
			}
			GUILayout.BeginVertical("box");
			m_selectedDebugSkills = GUILayout.Toggle(m_selectedDebugSkills, "Debug skills");
			if (m_selectedDebugSkills)
			{
				GUILayout.BeginHorizontal();
				int num = assassinCharacter.Skills.SkillSlots.Length;
				for (int i = 0; i < num; i++)
				{
					Enum<SkillTypes> skillToDebug = assassinCharacter.Skills.SkillSlots[i];
					if (GUILayout.Button(skillToDebug.Name))
					{
						m_DebugOutput.SkillToDebug = skillToDebug;
						m_debugRulesSelected = i;
					}
				}
				if (m_debugRulesSelected < 0 || m_debugRulesSelected >= num)
				{
					m_debugRulesSelected = -1;
					m_DebugOutput.SkillToDebug = Enum<SkillTypes>.Invalid;
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}

		private GameEntity GetSelectedGameEntity()
		{
			GameEntity gameEntity = null;
			if (gameEntity == null)
			{
				gameEntity = LogicMain.PlayerEntity;
			}
			return gameEntity;
		}
	}
}
