using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using UnityEngine;

public class OuterSkillDistributor : MonoBehaviour
{
	public List<SkillButton> SkillButtons;

	public bool locked;

	public void GetDeactivated()
	{
		for (int i = 0; i < SkillButtons.Count; i++)
		{
			SkillButtons[i].Background.color = new Color32(96, 96, 96, byte.MaxValue);
		}
		locked = true;
	}

	public void GetActivated()
	{
		locked = true;
		for (int i = 0; i < SkillButtons.Count; i++)
		{
			SkillButtons[i].Background.color = Color.white;
		}
		locked = false;
	}

	public void SelfUpdate()
	{
		if (!locked)
		{
			for (int i = 0; i < SkillButtons.Count; i++)
			{
				SkillButton skillButton = SkillButtons[i];
				SkillData.SkillDefinition item = Main.Instance.GameDB.GetItem<SkillData.SkillDefinition>(Main.Instance.GameDB.FindGameIdFromGuid(skillButton.SkillID));
				skillButton.SetupSkillButton(item);
				if (skillButton.available)
				{
					skillButton.BoxCollider.enabled = true;
					continue;
				}
				skillButton.BoxCollider.enabled = false;
				skillButton.SkillTextureUp = null;
				skillButton.SkillTextureDown = null;
				skillButton.SkillIcon.ClearTexture();
			}
		}
		else
		{
			for (int j = 0; j < SkillButtons.Count; j++)
			{
				SkillButton skillButton2 = SkillButtons[j];
				skillButton2.BoxCollider.enabled = false;
			}
		}
	}
}
