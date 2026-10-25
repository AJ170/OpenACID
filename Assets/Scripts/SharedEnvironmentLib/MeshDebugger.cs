using UnityEngine;

internal class MeshDebugger : MonoBehaviour
{
	public int m_triangleIndex = -1;

	private void OnDrawGizmosSelected()
	{
		if (m_triangleIndex >= 0)
		{
			MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
			if (!(component == null) && !(component.sharedMesh == null) && m_triangleIndex < component.sharedMesh.triangles.Length / 3)
			{
				Vector3 position = component.mesh.vertices[component.sharedMesh.triangles[m_triangleIndex * 3 + 0]];
				Vector3 position2 = component.mesh.vertices[component.sharedMesh.triangles[m_triangleIndex * 3 + 1]];
				Vector3 position3 = component.mesh.vertices[component.sharedMesh.triangles[m_triangleIndex * 3 + 2]];
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(base.transform.TransformPoint(position), base.transform.TransformPoint(position2));
				Gizmos.DrawLine(base.transform.TransformPoint(position2), base.transform.TransformPoint(position3));
				Gizmos.DrawLine(base.transform.TransformPoint(position3), base.transform.TransformPoint(position));
			}
		}
	}
}
