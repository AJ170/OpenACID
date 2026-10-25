using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Selection
{
	public static class SelectableExtensions
	{
		public static void SetSelected(this ISelectable _selectable, bool _selected)
		{
			IGameEntity gameEntity = _selectable as IGameEntity;
			if (gameEntity != null)
			{
				gameEntity.CanBePooled = !_selected;
			}
			ISelectionIconCarrier selectionIconCarrier = _selectable as ISelectionIconCarrier;
			if (selectionIconCarrier != null)
			{
				selectionIconCarrier.SelectionIcon.Selected = _selected;
			}
			IHighlightCarrier highlightCarrier = _selectable as IHighlightCarrier;
			if (highlightCarrier != null && highlightCarrier.HighlightedSelection)
			{
				if (_selected)
				{
					highlightCarrier.Highlight.SetMode(Highlight.EHighlightLayer.Selection, Highlight.ESelectionMode.On);
				}
				else
				{
					highlightCarrier.Highlight.RemoveMode(Highlight.EHighlightLayer.Selection);
				}
			}
		}

		public static bool IsBeingApproached(this ISelectable _selectable)
		{
			return LogicMain.PlayerEntity.GetIsApproachingTarget() && LogicMain.PlayerEntity.PlayerCharacterStateMachines.AssassinApproachingTarget.TargetId.GameEntity == _selectable;
		}

		public static bool IsSelectable(this ISelectable _selectable)
		{
			if (CombatController.IsActive)
			{
				return false;
			}
			if (!_selectable.IsValid)
			{
				return false;
			}
			if (_selectable.Position.DistanceXZ(LogicMain.PlayerEntity.Position) > 20f && !_selectable.IsBeingApproached())
			{
				return false;
			}
			Haystack haystack = _selectable as Haystack;
			Vector3 _edgePosition;
			if (LogicMain.PlayerEntity.Position.y - _selectable.Position.y > ParcourHelper.Acrobatics.JumpMaxFallHeightNoDamage && (haystack == null || !ParcourHelper.CanLeapOfFaith(LogicMain.PlayerEntity, haystack, out _edgePosition)) && !_selectable.IsBeingApproached())
			{
				return false;
			}
			Character character = _selectable as Character;
			if (character != null && character.GetIsDead())
			{
				return false;
			}
			EnemyCharacter enemyCharacter = _selectable as EnemyCharacter;
			if (enemyCharacter != null && enemyCharacter.EntityStats.Get_bool(StatTypes.EStatType.Enemy_IsDisguised))
			{
				return false;
			}
			return true;
		}
	}
}
