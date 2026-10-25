using SharedBaseLib.Core;
using UnityEngine;

public class HumanoidMeshCache
{
	private BBDictionary<string, Mesh> m_MeshDatabase;

	public HumanoidMeshCache()
	{
		m_MeshDatabase = new BBDictionary<string, Mesh>();
	}

	public void Add(string _guid, Mesh _mesh)
	{
		m_MeshDatabase.Add(_guid, _mesh);
	}

	public Mesh Find(string _guid)
	{
		Mesh value = null;
		if (m_MeshDatabase.TryGetValue(_guid, out value))
		{
			if (value == null)
			{
				m_MeshDatabase.Remove(_guid);
			}
			return value;
		}
		return null;
	}

	public void Clear()
	{
		foreach (Mesh value in m_MeshDatabase.Values)
		{
			Object.Destroy(value);
		}
		m_MeshDatabase.Clear();
	}
}
