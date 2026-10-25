using Assets.Scripts.Mission.Logic.Components;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic
{
	public interface IPositionEntity : IDefinitionCarrier, IEntityStatsCarrier, IEntity, IPositionProvider
	{
		new Vector3 Position { get; set; }

		new Vector3 Scale { get; set; }

		new Quaternion Rotation { get; set; }
	}
}
