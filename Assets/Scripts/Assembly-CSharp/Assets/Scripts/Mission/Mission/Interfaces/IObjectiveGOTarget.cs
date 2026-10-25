using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mission.Mission.Interfaces
{
	public interface IObjectiveGOTarget
	{
		List<GameObject> TargetGameObjects { get; }

		Type TargetComponentType { get; }
	}
}
