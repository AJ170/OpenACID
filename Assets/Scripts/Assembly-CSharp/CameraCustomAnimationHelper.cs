using SharedBaseLib.Core;
using UnityEngine;

public class CameraCustomAnimationHelper : MonoBehaviour
{
	public static bool ShouldActivateCamera;

	private BBList<AnimationClip> m_cameraAnimations = new BBList<AnimationClip>();

	private readonly string[] m_keyMapping = new string[17]
	{
		"y", "x", "c", "v", "b", "n", "m", "e", "z", "u",
		"i", "o", "p", "h", "j", "k", "l"
	};

	public string[] KeyMapping
	{
		get
		{
			return m_keyMapping;
		}
	}

	public Animation Animation { get; private set; }

	public AnimationClip CurrentClip { get; private set; }

	public void Update()
	{
		for (int i = 0; i < m_keyMapping.Length; i++)
		{
			string text = m_keyMapping[i];
			if (Input.GetKeyUp(text))
			{
				TriggerDebugCameraByKey(i);
			}
		}
	}

	public AnimationClip GetAnimationByIndex(int _index)
	{
		if (m_cameraAnimations.Count > _index)
		{
			return m_cameraAnimations[_index];
		}
		return null;
	}

	private void Awake()
	{
		m_cameraAnimations.Clear();
		Object[] array = Resources.LoadAll("Camera/Custom", typeof(AnimationClip));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			m_cameraAnimations.Add((AnimationClip)@object);
		}
		GameObject gameObject = new GameObject();
		Transform transform = gameObject.transform;
		transform.parent = base.gameObject.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		gameObject.layer = base.gameObject.layer;
		gameObject.name = "CameraStateCustomAnimation";
		Animation = gameObject.AddComponent<Animation>();
	}

	private void OnDestroy()
	{
		for (int i = 0; i < m_cameraAnimations.Count; i++)
		{
			AnimationClip animationClip = m_cameraAnimations[i];
			if (animationClip != null)
			{
				Resources.UnloadAsset(animationClip);
			}
		}
		m_cameraAnimations.Clear();
		m_cameraAnimations = null;
		Animation = null;
	}

	private void TriggerDebugCameraByKey(int _index)
	{
		CurrentClip = GetAnimationByIndex(_index);
		ShouldActivateCamera = CurrentClip != null;
	}
}
