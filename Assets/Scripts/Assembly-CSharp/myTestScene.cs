using UnityEngine;

public class myTestScene : MonoBehaviour
{
	public Transform Thing;

	public RuntimeAnimatorController Controller;

	public Avatar TheAvatar;

	public int CountX;

	public int County;

	private void Start()
	{
		RuntimeAnimatorController controller = Controller;
		for (int i = 0; i < County; i++)
		{
			for (int j = 0; j < CountX; j++)
			{
				Transform transform = Object.Instantiate(Thing, new Vector3(j * 2, 0f, i * 2), Quaternion.identity) as Transform;
				transform.parent = base.transform;
				GameObject gameObject = transform.gameObject;
				Animator animator = gameObject.GetComponent<Animator>();
				if (animator == null)
				{
					animator = gameObject.AddComponent<Animator>();
				}
				if (TheAvatar != null)
				{
					animator.avatar = TheAvatar;
				}
				animator.runtimeAnimatorController = controller;
				animator.applyRootMotion = false;
			}
		}
	}

	private void Update()
	{
	}
}
