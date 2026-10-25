using System.Collections.Generic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Selection;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class OffMeshLinkEntity : Entity, IDefinitionCarrier, IEntityStatsCarrier, IEntity, IPositionProvider, IPositionEntity, ISelectable
	{
		public InteractionLocators.Locator StartLocator { get; private set; }

		public InteractionLocators.Locator EndLocator { get; private set; }

		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.OffMeshLinkEntity;
			}
		}

		public override Definition Definition
		{
			get
			{
				return null;
			}
		}

		public override EntityStats EntityStats
		{
			get
			{
				return null;
			}
		}

		public OffMeshLink OffMeshLink { get; private set; }

		public Vector3 StartPosition { get; private set; }

		public Vector3 EndPosition { get; private set; }

		public List<EdgeObject> EdgeList { get; private set; }

		public List<ParkourStep> StepList { get; private set; }

		public eLinkType LinkType { get; private set; }

		public ClimbRoute ClimbRoute { get; private set; }

		public Vector3 Position
		{
			get
			{
				return OffMeshLink.transform.position;
			}
			set
			{
				OffMeshLink.transform.position = value;
			}
		}

		public Vector3 Scale
		{
			get
			{
				return OffMeshLink.transform.localScale;
			}
			set
			{
				OffMeshLink.transform.localScale = value;
			}
		}

		public Quaternion Rotation
		{
			get
			{
				return (!(OffMeshLink == null)) ? OffMeshLink.transform.rotation : Quaternion.LookRotation((EndPosition - StartPosition).Mask(new Vector3(1f, 0f, 1f)));
			}
			set
			{
				OffMeshLink.transform.rotation = value;
			}
		}

		public OffMeshLinkEntity(OffMeshLink _offMeshLink)
		{
			OffMeshLink = _offMeshLink;
			if (_offMeshLink != null)
			{
				StartPosition = _offMeshLink.startTransform.position;
				EndPosition = _offMeshLink.endTransform.position;
				StartLocator = new InteractionLocators.Locator(OffMeshLink.startTransform.gameObject, null, null);
				EndLocator = new InteractionLocators.Locator(OffMeshLink.endTransform.gameObject, null, null);
				OffMeshLinkMeta component = _offMeshLink.GetComponent<OffMeshLinkMeta>();
				EdgeList = component.EdgeObjectList;
				LinkType = component.Type;
				StepList = component.ParkourStepList;
				ClimbRoute = component.ClimbRoute;
			}
		}

		public OffMeshLinkEntity(ClimbRoute _route, Vector3 _startPosition, Vector3 _endPosition)
		{
			StartPosition = _startPosition;
			EndPosition = _endPosition;
			StartLocator = null;
			EndLocator = null;
			EdgeList = null;
			LinkType = eLinkType.Climb;
			StepList = null;
			ClimbRoute = _route;
		}
	}
}
