using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

public class SmallSkillWheelDistributor : MonoBehaviour
{
	[SerializeField]
	private List<SmallSkillWheelPiece> m_pieces;

	[SerializeField]
	private SmallSkillWheelPiece m_eagleVision;

	[SerializeField]
	private SmallSkillWheelPiece m_callHireling;

	[SerializeField]
	private UIButton prevButton;

	[SerializeField]
	private UIButton nextButton;

	private SkillWheel m_currentSkillwheel;

	private Assassin m_currentAssassin;

	private void Start()
	{
		EventDelegate.Add(prevButton.onClick, OnAssassinChanged);
		EventDelegate.Add(nextButton.onClick, OnAssassinChanged);
		SelfUpdate();
	}

	private void OnDestroy()
	{
		EventDelegate.Remove(prevButton.onClick, OnAssassinChanged);
		EventDelegate.Remove(nextButton.onClick, OnAssassinChanged);
	}

	private void OnAssassinChanged()
	{
		MonoBehaviour.print(Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.classTypeId);
		SelfUpdate();
	}

	public void SelfUpdate()
	{
		m_currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
		m_currentSkillwheel = m_currentAssassin.skillWheel;
		Assets.Scripts.UI.Utils.Extensions.SkillData[][] skills = m_currentAssassin.GetSkills(true);
		if (m_currentSkillwheel != null)
		{
			for (int i = 0; i < m_pieces.Count; i++)
			{
				SmallSkillWheelPiece smallSkillWheelPiece = m_pieces[i];
				smallSkillWheelPiece.m_skillID = null;
				if (i < m_currentSkillwheel.entries.Length)
				{
					smallSkillWheelPiece.m_entry.classType = m_currentSkillwheel.entries[i].classType;
					smallSkillWheelPiece.m_entry.tierId = m_currentSkillwheel.entries[i].tierId;
					smallSkillWheelPiece.m_entry.position = m_currentSkillwheel.entries[i].position;
					for (int j = 0; j < skills.Length; j++)
					{
						if (skills[j][0].TierId == smallSkillWheelPiece.m_entry.tierId && skills[j][0].Level > 0)
						{
							smallSkillWheelPiece.m_skillID = skills[j][0].SkillId;
						}
					}
				}
				MonoBehaviour.print(smallSkillWheelPiece.m_skillID);
			}
			for (int k = 0; k < m_pieces.Count; k++)
			{
				SmallSkillWheelPiece smallSkillWheelPiece2 = m_pieces[k];
				Assets.Scripts.Mission.Definitions.SkillData.SkillDefinition item = Main.Instance.GameDB.GetItem<Assets.Scripts.Mission.Definitions.SkillData.SkillDefinition>(Main.Instance.GameDB.FindGameIdFromGuid(smallSkillWheelPiece2.m_skillID));
				smallSkillWheelPiece2.SetupSkillButton(item);
			}
		}
		m_callHireling.SetupSkillButton("Call_Hireling".ToEnum<SkillTypes>().GetDefinition());
		m_eagleVision.SetupSkillButton("Eagle_Vision".ToEnum<SkillTypes>().GetDefinition());
	}
}
