using Assets.Scripts.Mission.HUD;
using UnityEngine;

namespace Assets.Scripts.Mission.UI
{
	public interface IUISubSystemLayout
	{
		Vector2 BorderOffset { get; set; }

		ScreenAnchor ScreenAnchor { get; set; }

		EUISubSystem SubSystem { get; }
	}
}
