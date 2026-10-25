using UnityEngine;

public class RenderingOutlines : MonoBehaviour
{
	private void CombineSkinnedMesh(SkinnedMeshRenderer[] smRenderers, Transform target)
	{
		SkinnedMeshRenderer skinnedMeshRenderer = target.gameObject.AddComponent<SkinnedMeshRenderer>();
		skinnedMeshRenderer.castShadows = false;
		skinnedMeshRenderer.receiveShadows = false;
		skinnedMeshRenderer.sharedMesh = smRenderers[0].sharedMesh;
		Material material = new Material(Shader.Find("Character/Outlines"));
		material.SetColor("_OutlineColor", Color.white);
		material.SetFloat("_Outline", 0.005f);
		skinnedMeshRenderer.sharedMaterial = material;
		skinnedMeshRenderer.bones = smRenderers[0].bones;
		skinnedMeshRenderer.localBounds = smRenderers[0].localBounds;
	}

	public void CreateOutlines(GameObject _go)
	{
		GameObject gameObject = new GameObject("Outline Renderer");
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.parent = _go.transform;
		SkinnedMeshRenderer[] componentsInChildren = _go.GetComponentsInChildren<SkinnedMeshRenderer>(false);
		CombineSkinnedMesh(componentsInChildren, gameObject.transform);
	}
}
