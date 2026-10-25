using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Screens.ModuleScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class AssassinExtensions
	{
		public static Assassin GetAssassinById(this Assassins _assassins, string _assassinId)
		{
			for (int i = 0; i < _assassins.assassins.Length; i++)
			{
				Assassin assassin = _assassins.assassins[i];
				if (assassin.id == _assassinId)
				{
					return assassin;
				}
			}
			return null;
		}

		public static string GetAssassinName(this Assassin _assassin)
		{
			return ComputeAssassinName(_assassin.assassinName);
		}

		public static string GetHirelingName(this HirelingAssassin _hirelingAssassin)
		{
			return ComputeAssassinName(_hirelingAssassin.assassinName);
		}

		public static string ComputeAssassinName(AssassinName _assassinName)
		{
			if (string.IsNullOrEmpty(_assassinName.overrideName))
			{
				string text = LocalizationManager.Instance.Get(_assassinName.firstNameLocaId) + " " + LocalizationManager.Instance.Get(_assassinName.lastNameLocaId);
				if (string.IsNullOrEmpty(text))
				{
					text = "<Animus Error>";
				}
				return text;
			}
			return _assassinName.overrideName;
		}

		public static string GetAssassinClassDisplayNames(this Assassin _assassin)
		{
			return GetAssassinClassDisplayNames(_assassin.classTypeId, _assassin.extendedClassTypeId);
		}

		public static string GetAssassinClassDisplayNames(Assassin.EClassTypeId _primaryClassType, Assassin.EClassTypeId _secondaryClassType)
		{
			Dictionary<int, ClassDescription> assassinClassDescriptions = UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions;
			string text = LocalizationManager.Instance.Get(assassinClassDescriptions[(int)_primaryClassType].NameId);
			if (_secondaryClassType != Assassin.EClassTypeId.None)
			{
				string value = LocalizationManager.Instance.Get(assassinClassDescriptions[(int)_secondaryClassType].NameId);
				return LocalizationManager.Instance.Get(2116, new StringMapper
				{
					{ "Primary_Class_Name", text },
					{ "Secondary_Class_Name", value }
				});
			}
			return text;
		}

		public static AssassinSummary GetSummary(this Assassin _assassin)
		{
			AssassinSummary assassinSummary = new AssassinSummary();
			UbiCore.Utils.Singleton<AvatarData>.Instance.SetPlayer(_assassin);
			AvatarData.IAvatarDefinition playerDefinition = UbiCore.Utils.Singleton<AvatarData>.Instance.GetPlayerDefinition();
			if (playerDefinition != null)
			{
				assassinSummary.Power = _assassin.TotalItemPower();
				bool _isBuffed;
				assassinSummary.Armour = AssassinStatsHelper.GetStat(playerDefinition, EAssassinStat.ArmorRating, false, out _isBuffed);
				assassinSummary.Damage = AssassinStatsHelper.GetStat(playerDefinition, EAssassinStat.Damage, false, out _isBuffed);
				assassinSummary.Health = AssassinStatsHelper.GetStat(playerDefinition, EAssassinStat.Health, false, out _isBuffed);
			}
			return assassinSummary;
		}

		public static int GetSkillpointForLevel(this LevelSkillPointMapping _mapping, int _level)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < _mapping.skillPointsPerLevel.Length; i++)
			{
				int level = _mapping.skillPointsPerLevel[i].level;
				if (level == _level)
				{
					num = _mapping.skillPointsPerLevel[i].skillPoint;
				}
				else if (level == _level - 1)
				{
					num2 = _mapping.skillPointsPerLevel[i].skillPoint;
				}
			}
			return num - num2;
		}

		public static UIAssasinId GetFirstHireling(this Assassin _assassin)
		{
			if (_assassin != null && _assassin.hirelingAssassins != null && _assassin.hirelingAssassins.hirelingAssassins != null && _assassin.hirelingAssassins.hirelingAssassins.Length > 0)
			{
				return _assassin.hirelingAssassins.hirelingAssassins[0].hirelingId;
			}
			return UIAssasinId.Invalid;
		}

		public static AssassinSkills GetSkillTier(this Assassin _assassin, int _id, bool _primaryClass)
		{
			AssassinSkillTiers assassinSkillTiers = ((!_primaryClass) ? _assassin.extendedClassskillTiers : _assassin.baseClassskillTiers);
			for (int i = 0; i < assassinSkillTiers.skillTiers.Length; i++)
			{
				if (assassinSkillTiers.skillTiers[i].tier == _id)
				{
					return assassinSkillTiers.skillTiers[i].skills;
				}
			}
			return null;
		}

		public static int GetTierId(this Assassin _assassin, UISkillId _id)
		{
			int[] array = new int[2]
			{
				(int)_assassin.classTypeId,
				(int)_assassin.extendedClassTypeId
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != 1)
				{
					AvatarClassDefinitions.AvatarClass definitionByType = Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition.GetDefinitionByType((Assassin.EClassTypeId)array[i]);
					int tierId = definitionByType.GetTierId(_id);
					if (tierId != -1)
					{
						return tierId;
					}
				}
			}
			return -1;
		}

		public static bool IsMaxLevel(this Assassin _assassin)
		{
			XpPerLevelTable xpPerLevelTable = Singleton<StateManager>.Instance.TemplateManager.XpPerLevelTable;
			return xpPerLevelTable.IsMaxLevel(_assassin.level);
		}

		public static int TotalItemPower(EquippedElement[] _equippedElements)
		{
			int num = 0;
			for (int i = 0; i < _equippedElements.Length; i++)
			{
				InventoryElement element = _equippedElements[i].element;
				if (element.IsItem())
				{
					num += element.itemData.CalculateItemPower();
				}
			}
			return num;
		}

		public static int TotalItemPower(this Assassin _assassin)
		{
			return TotalItemPower(_assassin.equippedElements);
		}

		public static float GetXPFactor(this Assassin _assassin)
		{
			if (!_assassin.IsMaxLevel())
			{
				long currentXPRelativeToCurrentLevel = _assassin.GetCurrentXPRelativeToCurrentLevel();
				return Mathf.Clamp01((float)currentXPRelativeToCurrentLevel / (float)_assassin.GetCurrentLevelMaxXP());
			}
			return 1f;
		}

		public static long GetCurrentXPRelativeToCurrentLevel(this Assassin _assassin)
		{
			XpPerLevelTable xpPerLevelTable = Singleton<StateManager>.Instance.TemplateManager.XpPerLevelTable;
			XpPerLevelTable.Entry xpLevelEntryFromXp = xpPerLevelTable.GetXpLevelEntryFromXp(_assassin.xp);
			return _assassin.IsMaxLevel() ? _assassin.GetCurrentLevelMaxXP() : (_assassin.xp - xpLevelEntryFromXp.xp);
		}

		public static long GetCurrentLevelMaxXP(this Assassin _assassin)
		{
			XpPerLevelTable xpPerLevelTable = Singleton<StateManager>.Instance.TemplateManager.XpPerLevelTable;
			return xpPerLevelTable.GetMaxXpToNextLevel(_assassin.level);
		}

		public static AssassinSkillInTraining.SkillType GetSkillType(this Assassin _assassin, UISkillId _id)
		{
			int[] array = new int[2]
			{
				(int)_assassin.classTypeId,
				(int)_assassin.extendedClassTypeId
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != 1)
				{
					AvatarClassDefinitions.AvatarClass definitionByType = Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition.GetDefinitionByType((Assassin.EClassTypeId)array[i]);
					AssassinSkillInTraining.SkillType skillType = definitionByType.GetSkillType(_id);
					if (skillType != AssassinSkillInTraining.SkillType.None_SkillType)
					{
						return skillType;
					}
				}
			}
			return AssassinSkillInTraining.SkillType.None_SkillType;
		}

		public static int SkillLevel(this Assassin _assassin, UISkillId _id)
		{
			int[] array = new int[2]
			{
				(int)_assassin.classTypeId,
				(int)_assassin.extendedClassTypeId
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != 1)
				{
					AvatarClassDefinitions.AvatarClass definitionByType = Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition.GetDefinitionByType((Assassin.EClassTypeId)array[i]);
					int tierId = definitionByType.GetTierId(_id);
					AssassinSkillInTraining.SkillType skillType = definitionByType.GetSkillType(_id);
					AssassinSkills skillTier = _assassin.GetSkillTier(tierId, array[i] == (int)_assassin.classTypeId);
					switch (skillType)
					{
					case AssassinSkillInTraining.SkillType.Active:
						return skillTier.activeSkill.level;
					case AssassinSkillInTraining.SkillType.PassiveOne:
						return skillTier.passiveOneSkill.level;
					case AssassinSkillInTraining.SkillType.PassiveTwo:
						return skillTier.passiveTwoSkill.level;
					}
				}
			}
			return -1;
		}

		public static AssassinSkillTier_ GetTier(this Assassin _assassin, int _id, AssassinSkillInTraining.SkillClass skillClass)
		{
			AssassinSkillTier_[] array = new AssassinSkillTier_[0];
			switch (skillClass)
			{
			case AssassinSkillInTraining.SkillClass.BaseClass:
				array = _assassin.baseClassskillTiers.skillTiers;
				break;
			case AssassinSkillInTraining.SkillClass.ExtendedClass:
				array = _assassin.extendedClassskillTiers.skillTiers;
				break;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].tier == _id)
				{
					return array[i];
				}
			}
			return null;
		}

		public static AssassinSkillTier_ GetTier(this Assassin _assassin, bool _primary, int _index)
		{
			AssassinSkillTier_[] array = ((!_primary) ? _assassin.extendedClassskillTiers.skillTiers : _assassin.baseClassskillTiers.skillTiers);
			return array[_index];
		}

		public static SkillData[][][] GetSkills(this Assassin _assassin)
		{
			return new SkillData[2][][]
			{
				_assassin.GetSkills(true),
				_assassin.GetSkills(false)
			};
		}

		public static SkillData[][] GetSkills(this Assassin _assassin, bool _baseClass)
		{
			AssassinSkillTier_[] array = ((!_baseClass) ? _assassin.extendedClassskillTiers.skillTiers : _assassin.baseClassskillTiers.skillTiers);
			Assassin.EClassTypeId eClassTypeId = ((!_baseClass) ? _assassin.extendedClassTypeId : _assassin.classTypeId);
			if (eClassTypeId == Assassin.EClassTypeId.None)
			{
				return new SkillData[array.Length][];
			}
			AvatarClassDefinitions.AvatarClass definitionByType = Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition.GetDefinitionByType(eClassTypeId);
			SkillData[][] array2 = new SkillData[array.Length][];
			for (int i = 0; i < array.Length; i++)
			{
				AssassinSkillTier_ assassinSkillTier_ = array[i];
				array2[i] = new SkillData[3];
				array2[i][0] = new SkillData
				{
					AvatarType = eClassTypeId,
					Level = assassinSkillTier_.skills.activeSkill.level,
					SkillType = AssassinSkillInTraining.SkillType.Active,
					Locked = !assassinSkillTier_.skills.activeSkill.unlocked,
					SkillId = definitionByType.GetSkillTier(assassinSkillTier_.tier).activeSkill.guid,
					TierId = assassinSkillTier_.tier
				};
				array2[i][1] = new SkillData
				{
					AvatarType = eClassTypeId,
					Level = assassinSkillTier_.skills.passiveOneSkill.level,
					SkillType = AssassinSkillInTraining.SkillType.PassiveOne,
					Locked = !assassinSkillTier_.skills.passiveOneSkill.unlocked,
					SkillId = definitionByType.GetSkillTier(assassinSkillTier_.tier).passiveSkillOne.guid,
					TierId = assassinSkillTier_.tier
				};
				array2[i][2] = new SkillData
				{
					AvatarType = eClassTypeId,
					Level = assassinSkillTier_.skills.passiveTwoSkill.level,
					SkillType = AssassinSkillInTraining.SkillType.PassiveTwo,
					Locked = !assassinSkillTier_.skills.passiveTwoSkill.unlocked,
					SkillId = definitionByType.GetSkillTier(assassinSkillTier_.tier).passiveSkillTwo.guid,
					TierId = assassinSkillTier_.tier
				};
			}
			return array2;
		}

		public static AssassinSkillInTraining.SkillClass GetSkillClass(this Assassin _assassin, UISkillId _id)
		{
			SkillData[][][] skills = _assassin.GetSkills();
			for (int i = 0; i < skills.Length; i++)
			{
				for (int j = 0; j < skills[i].Length; j++)
				{
					for (int k = 0; k < skills[i][j].Length; k++)
					{
						if (skills[i][j][k].SkillId == _id)
						{
							return (i != 0) ? AssassinSkillInTraining.SkillClass.ExtendedClass : AssassinSkillInTraining.SkillClass.BaseClass;
						}
					}
				}
			}
			return AssassinSkillInTraining.SkillClass.None_SkillClass;
		}

		public static TrainingCost GetTrainingTimeSeconds(this Assassin _assassin)
		{
			TrainingCost _trainingCost = new TrainingCost();
			long seconds = -1L;
			int num = _assassin.PointSpent();
			for (int i = 0; i < Singleton<StateManager>.Instance.TemplateManager.SkillTrainingTimeMapping.entries.Length; i++)
			{
				SkillTrainingTimeMapping.Entry entry = Singleton<StateManager>.Instance.TemplateManager.SkillTrainingTimeMapping.entries[i];
				if (num == entry.spentSkillPoints)
				{
					seconds = entry.trainingTimeSeconds;
					break;
				}
			}
			_trainingCost.Seconds = seconds;
			ApplyTraniningTimeModifier(_assassin, ref _trainingCost);
			return _trainingCost;
		}

		private static double CalcModRemainingTime(long _origRemainingTime, float _trainingFactor, long _moduleRemainingTime, bool _missionBased)
		{
			double num = Math.Round((double)_origRemainingTime / (double)_trainingFactor);
			if (_missionBased)
			{
				return num;
			}
			double val = Math.Round((float)_origRemainingTime - (float)_moduleRemainingTime * _trainingFactor + (float)_moduleRemainingTime);
			return Math.Max(val, num);
		}

		private static void ApplyTraniningTimeModifier(Assassin _assassin, ref TrainingCost _trainingCost)
		{
			InventoryElement elementForSlot = _assassin.GetElementForSlot(EInventorySlot.SlotModule);
			if (elementForSlot == null)
			{
				return;
			}
			ModuleData moduleData = elementForSlot.moduleData;
			bool flag = moduleData.durationType == EDurationType.MissionCount;
			long num = ((!flag) ? (moduleData.ComputeRemainingMilliSeconds() / 1000) : long.MaxValue);
			long seconds = _trainingCost.Seconds;
			Modification[] modifications = moduleData.modifications;
			foreach (Modification modification in modifications)
			{
				if (modification.moduleModifier == EModuleModifier.SkillTrainingTimeFactor)
				{
					float trainingFactor = (float)modification.effectValue / 100f + 1f;
					double num2 = CalcModRemainingTime(seconds, trainingFactor, num, flag);
					_trainingCost.ModuleFactor = modification.effectValue;
					_trainingCost.SpeedUpPart = 1f;
					_trainingCost.ModuleDuration = num;
					_trainingCost.NormalDuration = seconds;
					_trainingCost.Seconds = (long)num2;
					break;
				}
			}
		}

		public static int PointSpent(this Assassin _assassin)
		{
			return _assassin.PointSpent(false) + _assassin.PointSpent(true);
		}

		public static int PointSpent(this Assassin _assassin, bool _baseClass)
		{
			AssassinSkillTiers assassinSkillTiers = ((!_baseClass) ? _assassin.extendedClassskillTiers : _assassin.baseClassskillTiers);
			int num = 0;
			for (int i = 0; i < assassinSkillTiers.skillTiers.Length; i++)
			{
				num += assassinSkillTiers.skillTiers[i].skills.activeSkill.level + assassinSkillTiers.skillTiers[i].skills.passiveOneSkill.level + assassinSkillTiers.skillTiers[i].skills.passiveTwoSkill.level;
			}
			return num;
		}

		public static GameID GetAssassinHeadAccessoryGameID(this Assassin _assassin)
		{
			GameID result = GameID.Invalid;
			EquippedElement equippedElement = _assassin.equippedElements.Find((EquippedElement _item) => _item.slot == EInventorySlot.SlotInsignia);
			if (equippedElement != null)
			{
				UIGUID uIGUID = new UIGUID(equippedElement.element.guid);
				if (uIGUID.Valid)
				{
					GameDB gameDB = Main.Instance.GameDB;
					result = gameDB.FindGameIdFromGuid(uIGUID);
				}
			}
			return result;
		}

		public static bool HasAssassinHeadAccessoryEquiped(this Assassin _assassin)
		{
			InventoryElement elementForCategory = _assassin.GetElementForCategory(Item.EItemCategory.Insignia);
			if (elementForCategory != null)
			{
				return elementForCategory.IsHeadAccessoryItem();
			}
			return false;
		}

		public static InventoryElement GetElementForCategory(this Assassin _assassin, Item.EItemCategory _category)
		{
			for (int i = 0; i < _assassin.equippedElements.Length; i++)
			{
				if (_assassin.equippedElements[i].element.category == EInventoryElementCategory.InventoryItem && _assassin.equippedElements[i].element.itemData.category == _category)
				{
					return _assassin.equippedElements[i].element;
				}
			}
			return null;
		}

		public static InventoryElement GetElementForSlot(this Assassin _assassin, EInventorySlot _slot)
		{
			InventoryElement inventoryElement = null;
			for (int i = 0; i < _assassin.equippedElements.Length; i++)
			{
				if (_assassin.equippedElements[i].slot == _slot)
				{
					inventoryElement = _assassin.equippedElements[i].element;
				}
			}
			if (_slot == EInventorySlot.SlotModule && inventoryElement != null)
			{
				return (!inventoryElement.moduleData.Expired()) ? inventoryElement : null;
			}
			return inventoryElement;
		}

		public static InventoryElement GetEquipedEquivalent(this Assassin _assassin, InventoryElement _element)
		{
			for (int i = 0; i < _assassin.equippedElements.Length; i++)
			{
				if (_assassin.equippedElements[i].element.Equivalent(_element))
				{
					return _assassin.equippedElements[i].element;
				}
			}
			return null;
		}

		public static HumanoidDef FinalizeAppearance(this Assassin _assassin, CharacterAppearance _appearance)
		{
			GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(_assassin.humanoidGuid);
			HumanoidDef humanoidDef = Main.SafeGameDb.GetItem<HumanoidDef>(gameID);
			bool flag = false;
			for (int i = 0; i < _assassin.equippedElements.Length; i++)
			{
				EquippedElement equippedElement = _assassin.equippedElements[i];
				GameID _humanoidId;
				GameID _colorDefId;
				AvatarData.TryGetOutfitModification(equippedElement.element, out _humanoidId, out _colorDefId);
				HumanoidDef item = Main.SafeGameDb.GetItem<HumanoidDef>(_humanoidId);
				if (item != null)
				{
					humanoidDef = item;
				}
				ColorDef item2 = Main.SafeGameDb.GetItem<ColorDef>(_colorDefId);
				if (item2 != null)
				{
					_appearance.Color0 = item2.Color0;
					_appearance.Color1 = item2.Color1;
					flag = true;
				}
			}
			if (!flag)
			{
				bool flag2 = false;
				if (!string.IsNullOrEmpty(_assassin.colorSetting))
				{
					GameID gameID2 = Main.SafeGameDb.FindGameIdFromGuid(_assassin.colorSetting);
					if (gameID2 != GameID.Invalid)
					{
						ColorDef item3 = Main.SafeGameDb.GetItem<ColorDef>(gameID2);
						if (item3 != null)
						{
							_appearance.Color0 = item3.Color0;
							_appearance.Color1 = item3.Color1;
							flag2 = true;
						}
					}
				}
				if (!flag2)
				{
					ColorTableDef item4 = Main.SafeGameDb.GetItem<ColorTableDef>(humanoidDef.ColorTable);
					ColorDef item5 = Main.SafeGameDb.GetItem<ColorDef>(item4.Colors[0]);
					_appearance.Color0 = item5.Color0;
					_appearance.Color1 = item5.Color1;
				}
			}
			return humanoidDef;
		}

		public static void GetAssasssinPortraitData(this Assassin _assassin, out GameID _headId, out GameID _humanoidId, out GameID _colorId, out GameID _itemId, out bool _transparentAssassin)
		{
			_transparentAssassin = false;
			if (_assassin == null)
			{
				_assassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
				_transparentAssassin = true;
			}
			_headId = GameID.Invalid;
			_humanoidId = GameID.Invalid;
			_colorId = GameID.Invalid;
			_itemId = GameID.Invalid;
			GameDB gameDB = Main.Instance.GameDB;
			UIGUID uIGUID = new UIGUID(_assassin.headGuid);
			if (!uIGUID.Valid)
			{
				return;
			}
			GameID gameID = gameDB.FindGameIdFromGuid(uIGUID);
			GameID _humanoidId2 = GameID.Invalid;
			GameID _colorDefId = GameID.Invalid;
			EquippedElement equippedElement = _assassin.equippedElements.Find((EquippedElement _item) => _item.slot == EInventorySlot.SlotOutfit);
			if (equippedElement != null)
			{
				AvatarData.TryGetOutfitModification(equippedElement.element, out _humanoidId2, out _colorDefId);
			}
			if (equippedElement == null || !GameID.IsValid(_humanoidId2) || !GameID.IsValid(_colorDefId))
			{
				UIGUID uIGUID2 = new UIGUID(_assassin.humanoidGuid);
				UIGUID uIGUID3 = new UIGUID(_assassin.colorSetting);
				if (!uIGUID.Valid || !uIGUID2.Valid || !uIGUID3.Valid)
				{
					return;
				}
				_humanoidId2 = gameDB.FindGameIdFromGuid(uIGUID2);
				_colorDefId = gameDB.FindGameIdFromGuid(uIGUID3);
			}
			EquippedElement equippedElement2 = _assassin.equippedElements.Find((EquippedElement _item) => _item.slot == EInventorySlot.SlotInsignia);
			if (equippedElement2 != null)
			{
				UIGUID uIGUID4 = new UIGUID(equippedElement2.element.guid);
				if (uIGUID4.Valid)
				{
					_itemId = gameDB.FindGameIdFromGuid(uIGUID4);
				}
			}
			_headId = gameID;
			_humanoidId = _humanoidId2;
			_colorId = _colorDefId;
		}
	}
}
