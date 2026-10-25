using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.UI.Types;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
	public UITexture SkillIcon;

	public UISprite HighLight;

	public UISprite Background;

	public UISprite BackgroundGlow;

	public bool locked;

	public bool available;

	public BoxCollider BoxCollider;

	public SkillWheelEntry WheelEntry;

	public UISkillId SkillID;

	private bool m_isSelected;

	public bool IsSelected
	{
		get
		{
			return m_isSelected;
		}
		set
		{
			m_isSelected = value;
			SkillIcon.mainTexture = ((!m_isSelected) ? SkillTextureUp : SkillTextureDown);
			SkillIcon.mainTexture = SkillTextureUp;
		}
	}

	public SkillData.ISkillDefinition SkillDescription { get; set; }

	public Texture2D SkillTextureUp { get; set; }

	public Texture2D SkillTextureDown { get; set; }

	public void SetupSkillButton(SkillData.ISkillDefinition _skillDescription)
	{
		MonoBehaviour.print(available);
		if (available)
		{
			SkillDescription = _skillDescription;
			SkillTextureUp = _skillDescription.IconUp.AccessObject<Texture2D>();
			SkillTextureDown = _skillDescription.IconDown.AccessObject<Texture2D>();
			IsSelected = false;
		}
	}

	public void SwapSkills(SkillButton _button)
	{
		UISkillId skillID = SkillID;
		SkillWheelEntry wheelEntry = WheelEntry;
		SkillDescription = _button.SkillDescription;
		SkillTextureUp = _button.SkillTextureUp;
		SkillTextureDown = _button.SkillTextureDown;
		IsSelected = IsSelected;
		WheelEntry.tierId = _button.WheelEntry.tierId;
		SkillID = _button.SkillID;
		_button.SkillID = skillID;
		_button.WheelEntry = wheelEntry;
	}

	private void Start()
	{
		Transform transform = base.transform;
		if (transform.localEulerAngles.z != 0f)
		{
			transform.localEulerAngles = -transform.parent.localEulerAngles;
		}
	}

	private void OnDestroy()
	{
		SkillTextureUp = null;
		SkillTextureDown = null;
	}
}
