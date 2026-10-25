using System;
using System.Collections;
using UnityEngine;

public class retinaProUtil : MonoBehaviour
{
	public static retinaProUtil sharedInstance;

	private void Awake()
	{
		sharedInstance = this;
	}

	public void refreshAllWidgetsPixelPerfect(GameObject go)
	{
		if (!retinaProAtlasController.sharedInstance.isRuntimeDeviceLoaded())
		{
			Debug.LogWarning("retinaProAtlasController did not load properly.");
			return;
		}
		retinaProAtlasController.sharedInstance.fixRoot();
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		if (componentsInChildren != null)
		{
			UIWidget[] array = componentsInChildren;
			foreach (UIWidget uIWidget in array)
			{
				GameObject gameObject = null;
				Type type = uIWidget.GetType();
				if (type == typeof(UISprite))
				{
					gameObject = ((UISprite)uIWidget).atlas.gameObject;
				}
				else if (type == typeof(UILabel))
				{
					gameObject = ((UILabel)uIWidget).bitmapFont.gameObject;
				}
				if (gameObject != null)
				{
					retinaProAtlasController.sharedInstance.fixAtlasOrFont(gameObject);
				}
			}
		}
		refreshVisible(go);
		StartCoroutine(refreshColliders(go));
	}

	private IEnumerator refreshColliders(GameObject go)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		fixColliders(go);
		yield return null;
	}

	public void refreshVisible(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		UIPanel component = go.GetComponent<UIPanel>();
		if (component != null)
		{
			NGUITools.MarkParentAsChanged(go);
			go.BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
		}
		UIWidget component2 = go.GetComponent<UIWidget>();
		if (component2 != null)
		{
			component2.MakePixelPerfect();
			component2.MarkAsChanged();
		}
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		if (componentsInChildren != null)
		{
			UIWidget[] array = componentsInChildren;
			foreach (UIWidget uIWidget in array)
			{
				uIWidget.MakePixelPerfect();
				uIWidget.MarkAsChanged();
			}
		}
		if (component != null)
		{
			component.Refresh();
			go.BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
			NGUITools.MarkParentAsChanged(go);
		}
	}

	private void fixColliders(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		UIPanel component = go.GetComponent<UIPanel>();
		BoxCollider component2 = go.GetComponent<BoxCollider>();
		if (component2 != null)
		{
			NGUITools.AddWidgetCollider(component2.gameObject);
		}
		BoxCollider[] componentsInChildren = go.GetComponentsInChildren<BoxCollider>();
		if (componentsInChildren != null)
		{
			BoxCollider[] array = componentsInChildren;
			foreach (BoxCollider boxCollider in array)
			{
				NGUITools.AddWidgetCollider(boxCollider.gameObject);
			}
		}
		if (component != null)
		{
			go.BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
			component.Refresh();
		}
	}

	[Obsolete("Use retinaProUtil.sharedInstance.refreshAllWidgetsPixelPerfect(myGameObject) instead")]
	public static void refreshWidgetPixelPerfect(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		if (componentsInChildren != null)
		{
			UIWidget[] array = componentsInChildren;
			foreach (UIWidget uIWidget in array)
			{
				uIWidget.MakePixelPerfect();
			}
		}
		BoxCollider[] componentsInChildren2 = go.GetComponentsInChildren<BoxCollider>();
		if (componentsInChildren2 != null)
		{
			BoxCollider[] array2 = componentsInChildren2;
			foreach (BoxCollider boxCollider in array2)
			{
				NGUITools.AddWidgetCollider(boxCollider.gameObject);
			}
		}
	}
}
