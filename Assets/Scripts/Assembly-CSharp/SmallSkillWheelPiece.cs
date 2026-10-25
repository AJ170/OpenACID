using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.UI.Types;
using UnityEngine;

public class SmallSkillWheelPiece : MonoBehaviour
{
	[SerializeField]
	private UITexture skillIcon;

	public SkillWheelEntry m_entry;

	public UISkillId m_skillID;

	public void SetupSkillButton(SkillData.ISkillDefinition _skillDescription)
	{
		if (_skillDescription == null)
		{
			skillIcon.ClearTexture();
		}
		else
		{
			skillIcon.mainTexture = _skillDescription.IconUp.AccessObject<Texture2D>();
		}
	}
}
