using AcierProtocol;
using Assets.Scripts.UI.Types;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class AvatarExtensions
	{
		public static bool IsValidClassType(this AvatarClassDefinitions _avatarClassDefinitions, Assassin.EClassTypeId _type)
		{
			for (int i = 0; i < _avatarClassDefinitions.classes.Length; i++)
			{
				AvatarClassDefinitions.AvatarClass avatarClass = _avatarClassDefinitions.classes[i];
				if (avatarClass.classTypeId == _type)
				{
					return true;
				}
			}
			return false;
		}

		public static TrainingCost GetCost(this AvatarClassDefinitions.AvatarClass _def, UISkillId _id, int _rank, Assassin _assassin)
		{
			AvatarClassDefinitions.AvatarClass.AvatarTier.AvatarSkill skill = _def.GetSkill(_id);
			for (int i = 0; i < skill.trainingData.Length; i++)
			{
				AvatarClassDefinitions.AvatarClass.AvatarTier.AvatarSkill.TrainingData trainingData = skill.trainingData[i];
				if (trainingData.level == _rank)
				{
					TrainingCost trainingTimeSeconds = _assassin.GetTrainingTimeSeconds();
					trainingTimeSeconds.Resources = trainingData.costs.resources;
					return trainingTimeSeconds;
				}
			}
			return null;
		}

		public static AvatarClassDefinitions.AvatarClass.AvatarTier.AvatarSkill GetSkill(this AvatarClassDefinitions.AvatarClass _def, UISkillId _id)
		{
			for (int i = 0; i < _def.tiers.Length; i++)
			{
				if (_def.tiers[i].activeSkill.guid == _id)
				{
					return _def.tiers[i].activeSkill;
				}
				if (_def.tiers[i].passiveSkillOne.guid == _id)
				{
					return _def.tiers[i].passiveSkillOne;
				}
				if (_def.tiers[i].passiveSkillTwo.guid == _id)
				{
					return _def.tiers[i].passiveSkillTwo;
				}
			}
			return null;
		}

		public static AvatarClassDefinitions.AvatarClass.AvatarTier GetSkillTier(this AvatarClassDefinitions.AvatarClass _def, int _id)
		{
			for (int i = 0; i < _def.tiers.Length; i++)
			{
				if (_def.tiers[i].id == _id)
				{
					return _def.tiers[i];
				}
			}
			return new AvatarClassDefinitions.AvatarClass.AvatarTier();
		}

		public static AssassinSkillInTraining.SkillType GetSkillType(this AvatarClassDefinitions.AvatarClass _def, UISkillId _id)
		{
			for (int i = 0; i < _def.tiers.Length; i++)
			{
				if (_def.tiers[i].activeSkill.guid == _id)
				{
					return AssassinSkillInTraining.SkillType.Active;
				}
				if (_def.tiers[i].passiveSkillOne.guid == _id)
				{
					return AssassinSkillInTraining.SkillType.PassiveOne;
				}
				if (_def.tiers[i].passiveSkillTwo.guid == _id)
				{
					return AssassinSkillInTraining.SkillType.PassiveTwo;
				}
			}
			return AssassinSkillInTraining.SkillType.None_SkillType;
		}

		public static int GetTierId(this AvatarClassDefinitions.AvatarClass _def, UISkillId _id)
		{
			for (int i = 0; i < _def.tiers.Length; i++)
			{
				AvatarClassDefinitions.AvatarClass.AvatarTier avatarTier = _def.tiers[i];
				if (avatarTier.activeSkill.guid == _id || avatarTier.passiveSkillOne.guid == _id || avatarTier.passiveSkillTwo.guid == _id)
				{
					return avatarTier.id;
				}
			}
			return -1;
		}

		public static int GetTierIndex(this AvatarClassDefinitions.AvatarClass _def, int _tierId)
		{
			for (int i = 0; i < _def.tiers.Length; i++)
			{
				if (_def.tiers[i].id == _tierId)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
