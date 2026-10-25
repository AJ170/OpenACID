using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame
{
	public interface ISpriteControlBehaviour
	{
		float Aspect { get; }

		List<Collider> Collider { get; }

		Vector2 MeshCenterOffset { get; }

		Vector2 MeshSize { get; }

		float ScreenWidth { get; set; }

		bool IsVisible { get; set; }

		bool IsRendered { get; set; }

		bool IsEnabled { get; set; }

		void Construct();

		bool IsOnUi(Vector2 _position);

		void Dispose();
	}
}
