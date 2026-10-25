using Assets.Scripts.Engine.ExtensionMethods;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Network.Missions.Mission.Data;
using Database;
using UbiCore.Utils;
using UnityEngine;

public class MissionSpawnStaticObject : MissionSpawnEntity<MissionSpawnStaticObjectData>
{
	[StaticObjectTypeGuid]
	public int Type;

	private EntityData.IEntityDefinition m_entityDefinition;

	private GameObject m_geometryNode;

	private StaticObjectData.ObjectType m_objectType;

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		base.OnPostRead(_nodeData);
		base.transform.localScale = new Vector3(base.Data.Scale.x, base.Data.Scale.y, base.Data.Scale.z);
		if (base.Data.TypeGuid != null)
		{
			GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(base.Data.TypeGuid);
			m_objectType = new StaticObjectData.ObjectType(gameID);
			m_entityDefinition = UbiCore.Utils.Singleton<StaticObjectData>.Instance.GetDefinition(m_objectType);
			Type = m_objectType.GameID.Guid;
		}
	}

	protected override GameEntity CreateEntity()
	{
		StaticObject staticObject = Helpers.CreateEntity<StaticObject>(m_entityDefinition.EntityType);
		staticObject.Definition.EntityDefinition = m_entityDefinition;
		m_geometryNode = ItemHelper.CreateItemOrEffectFromDB(staticObject.StaticObjectDefinition.Definition.StaticObjectGameID);
		if (m_geometryNode == null)
		{
			return null;
		}
		m_geometryNode.ParentTo(this);
		return staticObject;
	}

	protected override void PostSetupEntity(GameEntity _entity)
	{
		base.PostSetupEntity(_entity);
		EntityWrapperComponent entityWrapperComponent = m_geometryNode.AddComponent<EntityWrapperComponent>();
		entityWrapperComponent.Entity = _entity;
		_entity.Highlight.SetMode(Highlight.EHighlightLayer.Base, Highlight.ESelectionMode.Off);
	}
}
