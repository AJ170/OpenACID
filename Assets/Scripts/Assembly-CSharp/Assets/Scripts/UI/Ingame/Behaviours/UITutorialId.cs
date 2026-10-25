using System;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.UI;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	[Serializable]
	public class UITutorialId : MonoBehaviour
	{
		public string Identifier = string.Empty;

		[EnumFlags]
		public ETutorialIdCondition Conditions;

		public string FullPath { get; private set; }

		public bool ConditionFulfilled { get; private set; }

		private void Awake()
		{
			ConditionFulfilled = true;
			string text = Identifier;
			Transform parent = base.gameObject.transform.parent;
			while (parent != null)
			{
				UITutorialId component = parent.GetComponent<UITutorialId>();
				if (component != null)
				{
					text = component.Identifier + "/" + text;
				}
				parent = parent.transform.parent;
			}
			FullPath = text;
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.RegisterId(this);
		}

		private void Stop()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.UnRegisterId(this);
		}

		public void Update()
		{
			UpdateVisibilityByCondition();
		}

		private void UpdateVisibilityByCondition()
		{
			ConditionFulfilled = true;
			if (ConditionFulfilled && (Conditions & ETutorialIdCondition.PlayerIsInLowProfile) == ETutorialIdCondition.PlayerIsInLowProfile && PlayerHelper.Entity != null)
			{
				ConditionFulfilled = PlayerHelper.PlayerProfile.CurrentStateType == EStateType.PlayerProfile_Low;
			}
			if (ConditionFulfilled && (Conditions & ETutorialIdCondition.PlayerIsInHighProfile) == ETutorialIdCondition.PlayerIsInHighProfile && PlayerHelper.Entity != null)
			{
				ConditionFulfilled = PlayerHelper.PlayerProfile.CurrentStateType == EStateType.PlayerProfile_High;
			}
			if (ConditionFulfilled && (Conditions & ETutorialIdCondition.PlayerIsInCombat) == ETutorialIdCondition.PlayerIsInCombat && PlayerHelper.Entity != null)
			{
				ConditionFulfilled = PlayerHelper.IsInCombat;
			}
			if (ConditionFulfilled && (Conditions & ETutorialIdCondition.PlayerIsNotInCombat) == ETutorialIdCondition.PlayerIsNotInCombat && PlayerHelper.Entity != null)
			{
				ConditionFulfilled = !PlayerHelper.IsInCombat;
			}
		}
	}
}
