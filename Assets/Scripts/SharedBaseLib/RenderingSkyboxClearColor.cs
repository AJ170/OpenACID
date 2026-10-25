using UnityEngine;

public class RenderingSkyboxClearColor : RenderingSky
{
	[SerializeField]
	private Color m_color;

	public override void Initialize()
	{
		Camera main = Camera.main;
		main.backgroundColor = m_color;
		main.clearFlags = CameraClearFlags.Color;
	}
}
