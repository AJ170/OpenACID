using SharedBaseLib.System;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	public static class LayerPresets
	{
		public static readonly LayerMask VisibilityCheckLayersMaskPreset = Layers.StaticCollidersLayersMaskPreset;

		public static void Init()
		{
		}
	}
}
