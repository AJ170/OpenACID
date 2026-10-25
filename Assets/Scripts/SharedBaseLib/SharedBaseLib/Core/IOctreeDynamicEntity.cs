using UnityEngine;

namespace SharedBaseLib.Core
{
	public interface IOctreeDynamicEntity
	{
		Vector3 Center { get; }

		float Radius { get; }

		OctreeDynamic CurrentCell { get; set; }
	}
}
