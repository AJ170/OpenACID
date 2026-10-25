using UnityEngine;

namespace SharedBaseLib.System
{
	public static class NavMeshLayers
	{
		public const string NotWalkableLayerName = "Not Walkable";

		public const string GroundSurfaceLayerName = "GroundSurface";

		public const string RoofSurfaceLayerName = "RoofSurface";

		public const string JumpHorizontalLayerName = "JumpHorizontal";

		public const string JumpDownLayerName = "JumpDown";

		public const string ClimbLayerName = "Climb";

		public const string ClimbUpLadderLayerName = "ClimbUpLadder";

		public const string ClimbDownLadderLayerName = "ClimbDownLadder";

		public const string RunPassOverLayerName = "RunPassOver";

		public const string RunPassOverDownLayerName = "RunPassOverDown";

		public const string BalanceLayerName = "Balance";

		public const string PropSurfaceLayerName = "PropSurface";

		public const string ParkourSequenceLayerName = "ParkourSequence";

		public const string JumpHorizontalToWallLayerName = "JumpHorizontalToWall";

		public static readonly int NotWalkableLayer = NavMesh.GetAreaFromName("Not Walkable");

		public static readonly int GroundSurfaceLayer = NavMesh.GetAreaFromName("GroundSurface");

		public static readonly int RoofSurfaceLayer = NavMesh.GetAreaFromName("RoofSurface");

		public static readonly int JumpHorizontalLayer = NavMesh.GetAreaFromName("JumpHorizontal");

		public static readonly int JumpDownLayer = NavMesh.GetAreaFromName("JumpDown");

		public static readonly int ClimbLayer = NavMesh.GetAreaFromName("Climb");

		public static readonly int ClimbUpLadderLayer = NavMesh.GetAreaFromName("ClimbUpLadder");

		public static readonly int ClimbDownLadderLayer = NavMesh.GetAreaFromName("ClimbDownLadder");

		public static readonly int RunPassOverLayer = NavMesh.GetAreaFromName("RunPassOver");

		public static readonly int RunPassOverDownLayer = NavMesh.GetAreaFromName("RunPassOverDown");

		public static readonly int BalanceLayer = NavMesh.GetAreaFromName("Balance");

		public static readonly int PropSurfaceLayer = NavMesh.GetAreaFromName("PropSurface");

		public static readonly int ParkourSequenceLayer = NavMesh.GetAreaFromName("ParkourSequence");

		public static readonly int JumpHorizontalToWallLayer = NavMesh.GetAreaFromName("JumpHorizontalToWall");

		public static readonly int NotWalkableLayerMask = 1 << NotWalkableLayer;

		public static readonly int GroundSurfaceLayerMask = 1 << GroundSurfaceLayer;

		public static readonly int RoofSurfaceLayerMask = 1 << RoofSurfaceLayer;

		public static readonly int JumpHorizontalLayerMask = 1 << JumpHorizontalLayer;

		public static readonly int JumpDownLayerMask = 1 << JumpDownLayer;

		public static readonly int ClimbLayerMask = 1 << ClimbLayer;

		public static readonly int ClimbUpLadderLayerMask = 1 << ClimbUpLadderLayer;

		public static readonly int ClimbDownLadderLayerMask = 1 << ClimbDownLadderLayer;

		public static readonly int RunPassOverLayerMask = 1 << RunPassOverLayer;

		public static readonly int RunPassOverDownLayerMask = 1 << RunPassOverDownLayer;

		public static readonly int BalanceLayerMask = 1 << BalanceLayer;

		public static readonly int PropSurfaceLayerMask = 1 << PropSurfaceLayer;

		public static readonly int ParkourSequenceLayerMask = 1 << ParkourSequenceLayer;

		public static readonly int JumpHorizontalToWallLayerMask = 1 << JumpHorizontalToWallLayer;

		public static readonly int WalkableLayerMaskPreset = GroundSurfaceLayerMask | RoofSurfaceLayerMask | PropSurfaceLayerMask;

		public static readonly int FullParcourMaskPreset = GroundSurfaceLayerMask | RoofSurfaceLayerMask | PropSurfaceLayerMask | JumpHorizontalLayerMask | JumpDownLayerMask | ClimbLayerMask | ClimbUpLadderLayerMask | ClimbDownLadderLayerMask | RunPassOverLayerMask | RunPassOverDownLayerMask | BalanceLayerMask | ParkourSequenceLayerMask;

		public static readonly int NoParcourMaskPreset = GroundSurfaceLayerMask | RoofSurfaceLayerMask | PropSurfaceLayerMask | ClimbUpLadderLayerMask | ClimbDownLadderLayerMask;

		public static void Init()
		{
		}
	}
}
