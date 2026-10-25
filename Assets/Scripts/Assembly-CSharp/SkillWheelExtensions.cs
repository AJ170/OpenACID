using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

public static class SkillWheelExtensions
{
	public static UISkillId GetSkillID(int _tierID, Assassin.EClassTypeId _AvatarType)
	{
		Assets.Scripts.UI.Utils.Extensions.SkillData[][] array = null;
		array = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetSkills(true);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i][0].AvatarType == _AvatarType && array[i][0].TierId == _tierID)
			{
				return array[i][0].SkillId;
			}
		}
		array = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetSkills(false);
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j][0].AvatarType == _AvatarType && array[j][0].TierId == _tierID)
			{
				return array[j][0].SkillId;
			}
		}
		return null;
	}

	public static void SetIcon(UITexture icon, int tierID, Assassin.EClassTypeId AvatarType)
	{
		UISkillId skillID = GetSkillID(tierID, AvatarType);
		Assets.Scripts.Mission.Definitions.SkillData.SkillDefinition item = Main.Instance.GameDB.GetItem<Assets.Scripts.Mission.Definitions.SkillData.SkillDefinition>(Main.Instance.GameDB.FindGameIdFromGuid(skillID));
		if (icon != null)
		{
			icon.mainTexture = item.IconUp.AccessObject<Texture2D>();
		}
	}

	public static void SetIcon(UITexture icon, int tierID, bool pressed, Assassin.EClassTypeId AvatarType)
	{
		UISkillId skillID = GetSkillID(tierID, AvatarType);
		Assets.Scripts.Mission.Definitions.SkillData.SkillDefinition item = Main.Instance.GameDB.GetItem<Assets.Scripts.Mission.Definitions.SkillData.SkillDefinition>(Main.Instance.GameDB.FindGameIdFromGuid(skillID));
		if (pressed && icon != null)
		{
			icon.mainTexture = item.IconDown.AccessObject<Texture2D>();
		}
	}

	public static int GetSkillLevel(int _tierID, Assassin.EClassTypeId _AvatarType)
	{
		if (_AvatarType != Assassin.EClassTypeId.None)
		{
			Assets.Scripts.UI.Utils.Extensions.SkillData[][] skills = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetSkills(true);
			for (int i = 0; i < skills.Length; i++)
			{
				if (skills[i][0].AvatarType == _AvatarType && skills[i][0].TierId == _tierID)
				{
					return skills[i][0].Level;
				}
			}
			skills = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetSkills(false);
			for (int j = 0; j < skills.Length; j++)
			{
				Debug.Log(_AvatarType);
				if (skills[j][0].AvatarType == _AvatarType && skills[j][0].TierId == _tierID)
				{
					return skills[j][0].Level;
				}
			}
		}
		return 0;
	}
}
