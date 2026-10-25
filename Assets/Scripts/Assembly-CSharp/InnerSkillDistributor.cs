using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using UnityEngine;

public class InnerSkillDistributor : MonoBehaviour
{
	public List<SkillButton> SkillButtons;

	public SkillButton CallHireling;

	public SkillButton EagleVision;

	public void GetDeactivated()
	{
	}

	public void SelfUpdate()
	{
		for (int i = 0; i < SkillButtons.Count; i++)
		{
			SkillButton skillButton = SkillButtons[i];
			if (skillButton.available)
			{
				SkillData.SkillDefinition item = Main.Instance.GameDB.GetItem<SkillData.SkillDefinition>(Main.Instance.GameDB.FindGameIdFromGuid(skillButton.SkillID));
				skillButton.SetupSkillButton(item);
			}
			else
			{
				skillButton.SkillIcon.ClearTexture();
			}
		}
		CallHireling.available = true;
		EagleVision.available = true;
		CallHireling.SetupSkillButton("Call_Hireling".ToEnum<SkillTypes>().GetDefinition());
		EagleVision.SetupSkillButton("Eagle_Vision".ToEnum<SkillTypes>().GetDefinition());
	}
}
