using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.UI.Ingame.Behaviours;
using UnityEngine;

namespace UbiCore.Debug.UI
{
	public class DebugMenuCombat : DebugMenuItem
	{
		public override string GetName()
		{
			return "Combat";
		}

		public override void OnOpen()
		{
		}

		public override void OnApply()
		{
		}

		public override void OnMenu()
		{
			float width = 150f;
			float width2 = 50f;
			if (Main.Instance.SceneLoader.InAnimus)
			{
				OnMenuAnimus();
				return;
			}
			CombatController instance = CombatController.Instance;
			if (instance == null)
			{
				GUILayout.Label("Could not find combat controller!");
				return;
			}
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			GUILayout.BeginVertical(GUI.skin.box);
			GUILayout.Label("Debug");
			instance.DebugShowInfo = GUILayout.Toggle(instance.DebugShowInfo, "Show debug info");
			instance.CombatUiData.m_ShowHealthBars = GUILayout.Toggle(instance.CombatUiData.m_ShowHealthBars, "Show health bars");
			GUILayout.EndVertical();
			GUILayout.BeginVertical(GUI.skin.box);
			GUILayout.Label("Cheats");
			instance.DebugPlayerImmortal = GUILayout.Toggle(instance.DebugPlayerImmortal, "Player immortal");
			instance.DebugEnemiesImmortal = GUILayout.Toggle(instance.DebugEnemiesImmortal, "Enemies immortal");
			instance.DebugDisableCriticalDamage = GUILayout.Toggle(instance.DebugDisableCriticalDamage, "Disable Critical Damage");
			if (GUILayout.Button("Heal Player"))
			{
				LogicMain.PlayerEntity.RefillHealth();
			}
			GUILayout.EndVertical();
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			CombatInterpreter.AutoAttack = GUILayout.Toggle(CombatInterpreter.AutoAttack, "Player auto attacks");
			GUILayout.BeginVertical();
			instance.EnableEnemyAttack = GUILayout.Toggle(instance.EnableEnemyAttack, "Enable enemy attack");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Attack period", GUILayout.Width(width));
			float.TryParse(GUILayout.TextField(instance.SchedulerGlobalCooldown.ToString("F2"), 10, GUILayout.Width(width2)), out instance.SchedulerGlobalCooldown);
			instance.SchedulerGlobalCooldown = GUILayout.HorizontalSlider(instance.SchedulerGlobalCooldown, 0f, 10f);
			GUILayout.EndHorizontal();
			bool flag = GUI.enabled;
			GUILayout.BeginHorizontal();
			GUI.enabled = (instance.ForceAttackAnimationId = GUILayout.Toggle(instance.ForceAttackAnimationId, "Force Attack Animation"));
			string s = GUILayout.TextField(string.Empty + instance.AttackAnimationId);
			int result;
			if (int.TryParse(s, out result) && result >= 1 && result <= 4)
			{
				instance.AttackAnimationId = result;
			}
			GUI.enabled = flag;
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUI.enabled = (instance.ForceComboAnimationId = GUILayout.Toggle(instance.ForceComboAnimationId, "Force Combo Animation"));
			s = GUILayout.TextField(string.Empty + instance.ComboAnimationId);
			if (int.TryParse(s, out result) && result >= 1 && result <= 6 && result != 5)
			{
				instance.ComboAnimationId = result;
			}
			GUI.enabled = flag;
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUI.enabled = (instance.ForceRandomAnimationParam = GUILayout.Toggle(instance.ForceRandomAnimationParam, "Force Random Animation"));
			s = GUILayout.TextField(instance.RandomAnimationParam.ToString("0.00"));
			float result2;
			if (GUI.changed && float.TryParse(s, out result2) && result2 >= 0f && result2 <= 1f)
			{
				instance.RandomAnimationParam = result2;
			}
			GUI.enabled = flag;
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginVertical(GUI.skin.box);
			GUILayout.Label("Player behavior");
			instance.EnableComboChangeTarget = GUILayout.Toggle(instance.EnableComboChangeTarget, "Allow changing targets during combos");
			instance.EnableKillTokenKillRuntime = GUILayout.Toggle(instance.EnableKillTokenKillRuntime, "Enable combo kill");
			if (instance.EnableKillTokenKillRuntime)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label("Tokens needed", GUILayout.Width(width));
				int result3 = instance.KillTokenCountRequired;
				int.TryParse(GUILayout.TextField(result3.ToString("D"), 10, GUILayout.Width(width2)), out result3);
				instance.KillTokenCountRequired = Mathf.RoundToInt(GUILayout.HorizontalSlider(result3, 0f, 10f));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				GUILayout.Label("Decay in combat after (s)", GUILayout.Width(width));
				float.TryParse(GUILayout.TextField(instance.KillTokenDecayDurationCombat.ToString("F2"), 10, GUILayout.Width(width2)), out instance.KillTokenDecayDurationCombat);
				instance.KillTokenDecayDurationCombat = GUILayout.HorizontalSlider(instance.KillTokenDecayDurationCombat, 0f, 10f);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				GUILayout.Label("Decay out of combat after (s)", GUILayout.Width(width));
				float.TryParse(GUILayout.TextField(instance.KillTokenDecayDurationNormal.ToString("F2"), 10, GUILayout.Width(width2)), out instance.KillTokenDecayDurationNormal);
				instance.KillTokenDecayDurationNormal = GUILayout.HorizontalSlider(instance.KillTokenDecayDurationNormal, 0f, 10f);
				GUILayout.EndHorizontal();
				UIKillTokenBarBehavior.AttachToCharacter = GUILayout.Toggle(UIKillTokenBarBehavior.AttachToCharacter, "Token bar follows player");
				CombatHelper.LoseAllTokensOnHit = GUILayout.Toggle(CombatHelper.LoseAllTokensOnHit, "Lost all tokens on hit");
				GUILayout.BeginHorizontal();
				GUILayout.Label("Parry delay", GUILayout.Width(width));
				float.TryParse(GUILayout.TextField(CombatInterpreter.ParryDelay.ToString("F2"), 10, GUILayout.Width(width2)), out CombatInterpreter.ParryDelay);
				CombatInterpreter.ParryDelay = GUILayout.HorizontalSlider(CombatInterpreter.ParryDelay, 0f, 1f);
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}

		private void OnMenuAnimus()
		{
		}
	}
}
