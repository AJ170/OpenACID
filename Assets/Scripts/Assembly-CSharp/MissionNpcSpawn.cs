using Assets.Scripts.Database;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.AI.System;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Network.Missions.Mission.Data;
using Database;
using Humanoids;
using UbiCore.Utils;
using UnityEngine;

public class MissionNpcSpawn : MissionSpawnEntity<MissionNpcSpawnData>
{
	[NpcTypeGuid]
	public int Type;

	private NpcData.INpcDefinition m_npcDefinition;

	public NpcData.NpcType NpcType { get; private set; }

	public int RolledNameLocaId { get; set; }

	public int RolledPortraitNameLocaId { get; set; }

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		base.OnPostRead(_nodeData);
		GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(base.Data.TypeGuid);
		NpcType = new NpcData.NpcType(gameID);
		if (NpcType.IsValid)
		{
			Type = NpcType.GameID.Guid;
		}
	}

	public override void OnActivate()
	{
		GameEntity gameEntity = EntityID.GameEntity;
		if (gameEntity != null)
		{
			Assets.Scripts.Mission.Logic.Components.CharacterAppearance characterAppearance = gameEntity.Cast<Character>().CharacterAppearance;
			HeadDef headDefinition = characterAppearance.HeadDefinition;
			if (headDefinition != null)
			{
				HeadDef.HeadMaterials headMaterials = characterAppearance.HeadMaterials;
				if (headMaterials != null && headMaterials.Portrait.WrappedObjectIsNull())
				{
					Main.Instance.StartCoroutine(headMaterials.Portrait.PrepareAsync());
				}
			}
		}
		base.OnActivate();
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
	}

	protected override GameEntity CreateEntity()
	{
		m_npcDefinition = NpcType.Definition;
		return Assets.Scripts.Mission.Logic.Helpers.CreateEntity<NpcCharacter>(m_npcDefinition);
	}

	protected override void PostSetupEntity(GameEntity _entity)
	{
		base.PostSetupEntity(_entity);
		NpcCharacter npcCharacter = _entity as NpcCharacter;
		if (npcCharacter != null)
		{
			SpawnHelper.PostSetupNpc(npcCharacter, !base.Data.IsMimic, MissionData.MapIcon, RolledNameLocaId, RolledPortraitNameLocaId);
			EnemyCharacter enemyCharacter = npcCharacter as EnemyCharacter;
			if (enemyCharacter != null)
			{
				enemyCharacter.EnemyNpcStats.SetIncrementFactor((float)(UbiCore.Utils.Singleton<MissionManager>.Instance.MissionTargetLevel - 1) * base.Data.ScalingMultiplier * UbiCore.Utils.Singleton<MissionManager>.Instance.MissionTargetRankMultiplier);
			}
		}
	}

	public override bool OnDeserialized()
	{
		if (!base.OnDeserialized())
		{
			return false;
		}
		if (!NpcType.IsValid)
		{
			return false;
		}
		NpcData.INpcDefinition definition = NpcType.Definition;
		GameID overridePrefab = null;
		if (GameID.IsValid(definition.HumanoidPrefabOverride))
		{
			overridePrefab = definition.HumanoidPrefabOverride;
			AnimatorDef item = Main.SafeGameDb.GetItem<AnimatorDef>(definition.HumanoidPrefabOverride);
			GameObject _go = null;
			if (!HumanoidFactoryHelpers.TryGetCachedGo(item.GameID, out _go))
			{
				_go = item.AnimatorPrefab.GetObject<GameObject>();
				HumanoidFactoryHelpers.AddGoToCache(item.GameID, _go);
				UbiCore.Utils.Singleton<AnimationMain>.Instance.GetAnimatorStateMachineStatic(_go.GetComponent<Animator>());
			}
		}
		PrepareDBObject(definition.WeaponGameID);
		PrepareDBObject(definition.RangedWeaponGameID);
		HumanoidDef item2 = Main.Instance.GameDB.GetItem<HumanoidDef>(definition.HumanoidGameID);
		if (item2.BodyPortrait.WrappedObjectIsNull())
		{
			Main.Instance.StartCoroutine(item2.BodyPortrait.PrepareAsync());
		}
		if (item2.BodyPortraitMask.WrappedObjectIsNull())
		{
			Main.Instance.StartCoroutine(item2.BodyPortraitMask.PrepareAsync());
		}
		if (!Singleton<HumanoidFactory>.Instance.PreCacheHumanoidFromType(NpcType, overridePrefab))
		{
			return false;
		}
		string behaviorTreePath = NpcType.Definition.BehaviorTreePath;
		NpcTreeHolder npcTreeHolder = UbiCore.Utils.Singleton<BehaviorTreeService>.Instance.RequestBehaviorTreeHolder(behaviorTreePath, BehaviorTreeCategory.Npc) as NpcTreeHolder;
		return true;
	}

	private void PrepareDBObject(GameID _id)
	{
		if (GameID.IsValid(_id))
		{
			ItemDef item = Main.Instance.GameDB.GetItem<ItemDef>(_id);
			if (item.ItemPrefab.WrappedObjectIsNull())
			{
				Main.Instance.StartCoroutine(item.ItemPrefab.PrepareAsync());
			}
		}
	}
}
