using AcierProtocol;
using Assets.Scripts.Network.Missions.Mission.Data;
using Database;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;

public class MissionRoot : MissionBase<MissionRootData>
{
	public override void OnPreActivate()
	{
		base.OnPreActivate();
		if (base.Data.ObjectiveType == EObjectiveType.Tutorial_EObjectiveType)
		{
			Singleton<EffectManager>.Instance.PrepareEffects(Main.Instance.GameDB, new GameID[1] { UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_floatingFloorEffect });
			Singleton<EffectManager>.Instance.setupEffectPackages();
		}
	}

	public override void OnActivate()
	{
		base.OnActivate();
		base.gameObject.AddComponentIfNotExist<MissionSectors>();
		base.gameObject.AddComponentIfNotExist<MissionHotspots>();
		base.Data.MapIcon = EMapIconType.Invalid_EMapIconType;
		if (base.Data.ObjectiveType == EObjectiveType.Tutorial_EObjectiveType)
		{
			LogicMain.PlayerEntity.Effects.OnEffectOn("Animus_FloatingFloor");
		}
	}

	public override void OnDeactivate()
	{
		if (base.Data.ObjectiveType == EObjectiveType.Tutorial_EObjectiveType)
		{
			LogicMain.PlayerEntity.Effects.OnEffectOff("Animus_FloatingFloor");
		}
		base.OnDeactivate();
	}
}
