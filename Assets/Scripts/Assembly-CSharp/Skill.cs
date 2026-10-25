using UnityEngine;

public class Skill
{
	public string m_name;

	public int m_translationIdName;

	public int m_translationIdDesc;

	public Texture2D SkillIcon;

	public Skill(string _name, int _translationIdName, int _translationIdDesc, Texture2D _skillIcon)
	{
		m_name = _name;
		m_translationIdName = _translationIdName;
		m_translationIdDesc = _translationIdDesc;
		SkillIcon = _skillIcon;
	}
}
