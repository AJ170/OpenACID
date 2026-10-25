using SharedBaseLib.System;

public static class LinkTypeExtension
{
	public static int ToNavMeshLayer(this eLinkType _linkType)
	{
		switch (_linkType)
		{
		case eLinkType.JumpHorizontal:
			return NavMeshLayers.JumpHorizontalLayer;
		case eLinkType.JumpDown:
			return NavMeshLayers.JumpDownLayer;
		case eLinkType.JumpHorizontalToWall:
			return NavMeshLayers.JumpHorizontalToWallLayer;
		case eLinkType.Climb:
			return NavMeshLayers.ClimbLayer;
		case eLinkType.Balance:
			return NavMeshLayers.BalanceLayer;
		case eLinkType.ClimbUpLadder:
			return NavMeshLayers.ClimbUpLadderLayer;
		case eLinkType.ClimbDownLadder:
			return NavMeshLayers.ClimbDownLadderLayer;
		case eLinkType.RunPassOver:
			return NavMeshLayers.RunPassOverLayer;
		case eLinkType.RunPassOverDown:
			return NavMeshLayers.RunPassOverDownLayer;
		case eLinkType.ParkourSequence:
			return NavMeshLayers.ParkourSequenceLayer;
		default:
			return NavMeshLayers.NotWalkableLayer;
		}
	}
}
