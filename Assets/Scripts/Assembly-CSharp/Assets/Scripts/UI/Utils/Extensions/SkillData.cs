using AcierProtocol;
using Assets.Scripts.UI.Types;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public class SkillData
	{
		public Assassin.EClassTypeId AvatarType;

		public int Level;

		public AssassinSkillInTraining.SkillType SkillType;

		public bool Locked;

		public UISkillId SkillId;

		public int TierId;

		public bool Active
		{
			get
			{
				return SkillType == AssassinSkillInTraining.SkillType.Active;
			}
		}
	}
}
