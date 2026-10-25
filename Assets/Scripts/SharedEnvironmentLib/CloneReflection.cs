using UnityEngine;

public class CloneReflection : PipelineBehaviour
{
	private void Start()
	{
		Clone();
	}

	public void Clone()
	{
		GameObject obj = Object.Instantiate(base.gameObject);
		obj.transform.localPosition = base.transform.position;
		obj.transform.localRotation = base.transform.rotation;
		obj.transform.localScale = base.transform.lossyScale;
		obj.layer = LayerMask.NameToLayer("Reflection");
		GameObject gameObject = GameObject.Find("EnvScenario/Reflection");
		if (gameObject == null)
		{
			gameObject = new GameObject("Reflection");
			GameObject gameObject2 = GameObject.Find("EnvScenario");
			if (gameObject2 == null)
			{
				gameObject2 = new GameObject("EnvScenario");
			}
			gameObject.transform.parent = gameObject2.transform;
		}
		obj.transform.parent = gameObject.transform;
		Object.Destroy(this);
	}
}
