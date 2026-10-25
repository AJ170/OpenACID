using System;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Selection;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public abstract class Character : GameEntity, IBuffsCarrier, ICharacterAppearanceCarrier, ICharacterStatsCarrier, IDefinitionCarrier, ICharacterDefinitionCarrier, IEffectsCarrier, ICharacterStateMachinesCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHeadIconCarrier, IHighlightCarrier, ISelectionIconCarrier, ISpacialEntityCarrier, IStateMachinesCarrier, IUnityComponentCacheCarrier, IWeaponHandlerCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity, ISelectable
	{
		public abstract CharacterDefinition CharacterDefinition { get; }

		public override EntityStats EntityStats
		{
			get
			{
				return CharacterStats;
			}
		}

		public abstract CharacterStats CharacterStats { get; }

		public HeadIcon HeadIcon { get; private set; }

		public SelectionIcon SelectionIcon { get; private set; }

		public Buffs Buffs { get; private set; }

		public Assets.Scripts.Mission.Logic.Components.StateMachines StateMachines { get; private set; }

		public WeaponHandlerCache WeaponHandlerCache { get; private set; }

		public Effects Effects { get; private set; }

		public CharacterStateMachines CharacterStateMachines
		{
			get
			{
				return (CharacterStateMachines)StateMachines;
			}
		}

		public CharacterAppearance CharacterAppearance { get; private set; }

		protected Character(Func<IEntity, Assets.Scripts.Mission.Logic.Components.StateMachines> _stateMachinesGenerator)
		{
			HeadIcon = AddComponent<HeadIcon>();
			SelectionIcon = AddComponent<SelectionIcon>();
			Buffs = AddComponent<Buffs>();
			StateMachines = AddComponent(_stateMachinesGenerator(this));
			WeaponHandlerCache = AddComponent<WeaponHandlerCache>();
			Effects = AddComponent<Effects>();
			CharacterAppearance = AddComponent<CharacterAppearance>();
		}
	}
}
