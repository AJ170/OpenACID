using Assets.Scripts.Engine.ExtensionMethods;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Database;
using UnityEngine;

internal static class MissionFactory
{
	public static string StaticObjectIdToGuid(int _hashID)
	{
		if (_hashID == GameID.Invalid.Guid)
		{
			return GameID.Invalid.GuidString;
		}
		GameID _result = GameID.Invalid;
		bool flag = Main.Instance.GameDB.FindGameIdFromGuidHash(_hashID, ref _result);
		return new StaticObjectData.ObjectType(_result).GameID.GuidString;
	}

	public static StaticObjectData.ObjectType StaticObjectFromGuid(string _typeGuid)
	{
		if (string.IsNullOrEmpty(_typeGuid))
		{
			return null;
		}
		GameID _gameId;
		if (!Main.Instance.GameDB.TryFindGameIdFromGuid(_typeGuid, out _gameId))
		{
			return null;
		}
		return new StaticObjectData.ObjectType(_gameId);
	}

	public static HighlightBehaviour InstantiateStaticObject(EntityData.IEntityDefinition _definition, Transform _targetTransform)
	{
		if (_definition == null)
		{
			return null;
		}
		StaticObject staticObject = Helpers.CreateEntity<StaticObject>(_definition.EntityType);
		staticObject.Definition.EntityDefinition = _definition;
		GameID staticObjectGameID = staticObject.StaticObjectDefinition.Definition.StaticObjectGameID;
		GameObject gameObject = ItemHelper.CreateItemOrEffectFromDB(staticObjectGameID);
		if (gameObject == null)
		{
			return null;
		}
		gameObject.ParentTo(_targetTransform);
		return gameObject.GetComponentInChildren<HighlightBehaviour>();
	}
}
