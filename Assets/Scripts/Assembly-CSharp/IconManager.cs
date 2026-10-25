using System;
using System.Collections.Generic;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

public class IconManager : UbiCore.Utils.Singleton<IconManager>
{
	private BBDictionary<int, Material> m_materials = new BBDictionary<int, Material>();

	private BBDictionary<int, Texture2D> m_textures = new BBDictionary<int, Texture2D>();

	public IconManager()
	{
		m_materials.Clear();
		m_materials.Add(0, null);
		UnityEngine.Object[] array = Resources.LoadAll("UI/Icons/Materials", typeof(Material));
		UnityEngine.Object[] array2 = array;
		foreach (UnityEngine.Object @object in array2)
		{
			if (@object.name.StartsWith("Icon"))
			{
				string value = @object.name.Substring(4);
				IconMaterial key = (IconMaterial)(int)Enum.Parse(typeof(IconMaterial), value);
				m_materials.Add((int)key, (Material)@object);
			}
		}
		m_textures.Clear();
		UnityEngine.Object[] array3 = Resources.LoadAll("UI/Icons/Textures/Action/", typeof(Texture2D));
		UnityEngine.Object[] array4 = array3;
		foreach (UnityEngine.Object object2 in array4)
		{
			if (object2.name.StartsWith("Icon"))
			{
				string value2 = object2.name.Substring(4);
				IconMaterial key2 = (IconMaterial)(int)Enum.Parse(typeof(IconMaterial), value2);
				m_textures.Add((int)key2, (Texture2D)object2);
			}
		}
	}

	public Material GetMaterial(IconMaterial _name)
	{
		Material value;
		if (m_materials.TryGetValue((int)_name, out value))
		{
			return value;
		}
		return null;
	}

	public Texture2D GetTexture(IconMaterial _name)
	{
		Texture2D value;
		if (m_textures.TryGetValue((int)_name, out value))
		{
			return value;
		}
		return null;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		foreach (KeyValuePair<int, Material> material in m_materials)
		{
			if (material.Value != null)
			{
				Resources.UnloadAsset(material.Value);
			}
		}
		m_materials.Clear();
		m_materials = null;
		foreach (KeyValuePair<int, Texture2D> texture in m_textures)
		{
			if (texture.Value != null)
			{
				Resources.UnloadAsset(texture.Value);
			}
		}
		m_textures.Clear();
		m_textures = null;
	}
}
