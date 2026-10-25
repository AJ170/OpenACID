using System;
using Assets.Scripts.Mission.Logic.Entities;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic
{
	public static class SkillAreaHelper
	{
		public interface IAreaInstance
		{
			BBList<AffectedNpc> AffectedCharacters { get; set; }

			BBList<Zone> Zones { get; set; }

			bool[,] ZoneConnections { get; set; }

			Vector3 AreaCenter { get; set; }
		}

		public class AffectedNpc : IComparable<AffectedNpc>
		{
			public NpcCharacter Npc { get; private set; }

			public float Distance { get; private set; }

			public AffectedNpc(NpcCharacter _npc, float _distance)
			{
				Npc = _npc;
				Distance = _distance;
			}

			public int CompareTo(AffectedNpc _other)
			{
				return Distance.CompareTo(_other.Distance);
			}
		}

		public class Zone
		{
			public Vector3 Position { get; set; }

			public float Radius { get; set; }

			public bool IsFree { get; set; }

			public bool IsBlocked { get; set; }

			public Zone(Vector3 _position, float _radius)
			{
				Vector3 origin = _position;
				origin.y += 1.5f;
				DummyRay.origin = origin;
				DummyRay.direction = Vector3.down;
				RaycastHit[] array = Physics.RaycastAll(DummyRay, 10f, Layers.StaticCollidersLayersMaskPreset);
				float num = float.MaxValue;
				for (int i = 0; i < array.Length; i++)
				{
					float num2 = origin.y - array[i].point.y;
					if (num2 < num)
					{
						num = num2;
					}
				}
				Position = _position;
				Radius = _radius;
				IsFree = true;
				IsBlocked = false;
			}
		}

		public const float ExtraWidth = 0.3f;

		public static readonly int WalkableMask = NavMeshLayers.WalkableLayerMaskPreset;

		public static readonly NavMeshPath DummyPath = new NavMeshPath();

		public static NavMeshHit DummyHit;

		public static RaycastHit DummyRayHit;

		private static Ray DummyRay = default(Ray);

		public static void FindZones(this IAreaInstance _instance, int _zoneCount, float _zoneRadius)
		{
			float num = _zoneRadius + 0.3f;
			float num2 = (float)Math.PI / (float)(_zoneCount - 1);
			float num3 = num2 * 2f;
			float num4 = num / Mathf.Tan(num2);
			float radius = num4 - num - 0.3f;
			_instance.Zones = new BBList<Zone>();
			Zone zone = new Zone(_instance.AreaCenter, radius);
			DropZoneToGround(zone);
			SetZoneBlocked(zone);
			_instance.Zones.Add(zone);
			_instance.AreaCenter = zone.Position;
			float num5 = 0f;
			for (int i = 0; i < _zoneCount - 1; i++)
			{
				Vector3 position = _instance.AreaCenter + new Vector3(num4 * Mathf.Sin(num5), 0f, num4 * Mathf.Cos(num5));
				zone = new Zone(position, _zoneRadius);
				DropZoneToGround(zone);
				SetZoneBlocked(zone);
				_instance.Zones.Add(zone);
				num5 += num3;
			}
			_instance.ZoneConnections = new bool[_zoneCount, _zoneCount];
			for (int j = 0; j < _zoneCount; j++)
			{
				for (int k = 0; k < _zoneCount; k++)
				{
					_instance.ZoneConnections[j, k] = false;
				}
			}
			for (int l = 1; l < _zoneCount - 1; l++)
			{
				_instance.SetZonesConnected(0, l);
				_instance.SetZonesConnected(l, l + 1);
			}
			_instance.SetZonesConnected(0, _zoneCount - 1);
			_instance.SetZonesConnected(1, _zoneCount - 1);
		}

		public static int FindClosestZoneIdx(this IAreaInstance _instance, Vector3 _characterPosition)
		{
			int num = -1;
			float num2 = float.MaxValue;
			for (int i = 0; i < _instance.Zones.Count; i++)
			{
				if (_instance.CanReachZone(i, -1))
				{
					Zone zone = _instance.Zones[i];
					if (!(zone.Position.DistanceSquared(_characterPosition) >= num2) && NavMesh.CalculatePath(_characterPosition, zone.Position, WalkableMask, DummyPath))
					{
						num2 = DummyPath.CalculateLength();
						num2 *= num2;
						num = i;
					}
				}
			}
			if (num >= 0)
			{
				_instance.Zones[num].IsFree = false;
			}
			return num;
		}

		public static int FindRandomZoneIdx(this IAreaInstance _instance)
		{
			return _instance.FindRandomZoneIdx(0, _instance.Zones.Count - 1, -1);
		}

		public static int FindRandomZoneIdx(this IAreaInstance _instance, int _min, int _max, int _curIdx)
		{
			int num = 0;
			for (int i = _min; i <= _max; i++)
			{
				if (_instance.CanReachZone(i, _curIdx))
				{
					num++;
				}
			}
			if (num == 0)
			{
				return -1;
			}
			int num2 = LogicMain.Random.Rand(0, num);
			num = 0;
			for (int j = _min; j <= _max; j++)
			{
				if (_instance.CanReachZone(j, _curIdx))
				{
					if (num == num2)
					{
						_instance.Zones[j].IsFree = false;
						return j;
					}
					num++;
				}
			}
			return -1;
		}

		public static bool CouldPotentiallyReachArea(this IAreaInstance _instance, NpcCharacter _char)
		{
			int autoPassableNavMeshLayerMask = _char.GetAutoPassableNavMeshLayerMask();
			if (!NavMesh.SamplePosition(_char.Position, out DummyHit, 0.5f, autoPassableNavMeshLayerMask))
			{
				return false;
			}
			Vector3 areaCenter = _instance.AreaCenter;
			if (!NavMesh.SamplePosition(areaCenter, out DummyHit, 10f, DummyHit.mask))
			{
				return false;
			}
			areaCenter = DummyHit.position;
			NavMesh.CalculatePath(_char.Position, areaCenter, autoPassableNavMeshLayerMask, DummyPath);
			if (DummyPath.status == NavMeshPathStatus.PathInvalid)
			{
				return false;
			}
			if (DummyPath.status == NavMeshPathStatus.PathComplete)
			{
				return true;
			}
			if (DummyPath.corners.Length == 0)
			{
				return false;
			}
			Vector3 rhs = DummyPath.corners[DummyPath.corners.Length - 1];
			for (int i = 0; i < _instance.Zones.Count; i++)
			{
				Zone zone = _instance.Zones[i];
				if (!zone.IsBlocked)
				{
					float num = zone.Radius + 0.3f;
					num *= num;
					if (zone.Position.DistanceSquared(rhs) <= num)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool CanReachZone(this IAreaInstance _instance, int _testIdx, int _curIdx)
		{
			if (!_instance.Zones[_testIdx].IsFree)
			{
				return false;
			}
			if (_curIdx < 0)
			{
				return !_instance.Zones[_testIdx].IsBlocked;
			}
			return _instance.ZoneConnections[_curIdx, _testIdx];
		}

		private static void DropZoneToGround(Zone _zone)
		{
			Vector3 position = _zone.Position;
			position.y += 3f;
			DummyRay.origin = position;
			DummyRay.direction = Vector3.down;
			RaycastHit[] array = Physics.RaycastAll(DummyRay, 18f, Layers.StaticCollidersLayersMaskPreset);
			float num = float.MaxValue;
			Vector3 position2 = _zone.Position;
			for (int i = 0; i < array.Length; i++)
			{
				float num2 = Mathf.Abs(_zone.Position.y - array[i].point.y);
				if (num2 < num)
				{
					num = num2;
					position2 = array[i].point;
				}
			}
			_zone.Position = position2;
		}

		private static void SetZoneBlocked(Zone _zone)
		{
			_zone.IsBlocked = true;
			if (NavMesh.SamplePosition(_zone.Position, out DummyHit, 2f, WalkableMask))
			{
				_zone.Position = new Vector3(_zone.Position.x, DummyHit.position.y, _zone.Position.z);
				if (!(DummyHit.position.DistanceXZSquared(_zone.Position) > 0.0001f) && NavMesh.FindClosestEdge(_zone.Position, out DummyHit, WalkableMask))
				{
					_zone.IsBlocked = DummyHit.position.DistanceXZSquared(_zone.Position) < _zone.Radius * _zone.Radius;
				}
			}
		}

		private static void SetZonesConnected(this IAreaInstance _instance, int _z1, int _z2)
		{
			if (!_instance.Zones[_z1].IsBlocked && !_instance.Zones[_z2].IsBlocked && !NavigationHelper.SafeRaycast(_instance.Zones[_z1].Position, _instance.Zones[_z2].Position, out DummyHit, WalkableMask))
			{
				_instance.ZoneConnections[_z1, _z2] = true;
				_instance.ZoneConnections[_z2, _z1] = true;
			}
		}
	}
}
