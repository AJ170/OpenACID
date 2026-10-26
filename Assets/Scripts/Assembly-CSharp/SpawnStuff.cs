using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnStuff : MonoBehaviour
{
	public GameObject Prefab;

	public GameObject Body;

	public GameObject Head;

	public GameObject Accesoir;

	private GameObject Cached;

	private void Start()
	{
		SetupCached();
		GameObject gameObject = Object.Instantiate(Cached);
		GameObject gameObject2 = GameObject.Find("CharacterNode");
		Transform parent = gameObject2.transform;
		Transform transform = gameObject.transform;
		transform.parent = parent;
		if (Accesoir != null)
		{
			GameObject gameObject3 = Object.Instantiate(Accesoir);
			gameObject3.transform.parent = gameObject.transform.Find("Head");
			gameObject3.transform.localPosition = Vector4.zero;
			gameObject3.transform.localRotation = Quaternion.identity;
		}
		gameObject.SetActive(true);
	}

	private void SetupCached()
	{
		GameObject gameObject = GameObject.Find("Cache");
		Transform parent = gameObject.transform;
		GameObject gameObject2 = Object.Instantiate(Prefab);
		GameObject gameObject3 = Object.Instantiate(Body);
		Transform transform = gameObject3.transform;
		transform.parent = parent;
		CopyPrefabToTarget(gameObject2, gameObject3);
		GameObject gameObject4 = Object.Instantiate(Head);
		Renderer[] componentsInChildren = gameObject3.GetComponentsInChildren<Renderer>();
		Renderer[] componentsInChildren2 = gameObject4.GetComponentsInChildren<Renderer>();
		bool flag = gameObject3.GetComponent<LODGroup>() != null;
		int num = componentsInChildren.Length;
		for (int i = 0; num > i; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren[i] as SkinnedMeshRenderer;
			SkinnedMeshRenderer skinnedMeshRenderer2 = componentsInChildren2[i] as SkinnedMeshRenderer;
			List<Transform> list = new List<Transform>();
			list.AddRange(skinnedMeshRenderer.bones);
			list.AddRange(skinnedMeshRenderer2.bones);
			Mesh mesh = new Mesh();
			CombineInstance[] array = new CombineInstance[2];
			array[0].mesh = skinnedMeshRenderer.sharedMesh;
			array[1].mesh = skinnedMeshRenderer2.sharedMesh;
			array[0].subMeshIndex = 0;
			array[1].subMeshIndex = 0;
			array[0].transform = Matrix4x4.identity;
			array[1].transform = Matrix4x4.identity;
			mesh.CombineMeshes(array);
			skinnedMeshRenderer.sharedMesh = mesh;
			Transform[] array2 = new Transform[list.Count];
			for (int j = 0; list.Count > j; j++)
			{
				array2[j] = FindNodeRecursive(transform, list[j].name);
			}
			skinnedMeshRenderer.bones = array2;
		}
		if (flag)
		{
			LODGroup component = gameObject3.GetComponent<LODGroup>();
			component.RecalculateBounds();
		}
		Object.Destroy(gameObject4);
		Object.Destroy(gameObject2);
		gameObject3.SetActive(false);
		Cached = gameObject3;
	}

	private void Update()
	{
	}

	public static Transform FindNodeRecursive(Transform _base, string _name)
	{
		if (_base.name == _name)
		{
			return _base;
		}
		int childCount = _base.childCount;
		Transform transform = null;
		for (int i = 0; childCount > i; i++)
		{
			transform = FindNodeRecursive(_base.GetChild(i), _name);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	private void SanitizeGameObject(GameObject pre_fab)
	{
		Transform transform = pre_fab.transform;
		int childCount = transform.childCount;
		Transform[] array = new Transform[childCount];
		for (int i = 0; childCount > i; i++)
		{
			array[i] = transform.GetChild(i);
		}
		Transform[] array2 = array;
		foreach (Transform transform2 in array2)
		{
			Object.Destroy(transform2.gameObject);
		}
		LODGroup component = pre_fab.GetComponent<LODGroup>();
		if (component != null)
		{
			Object.DestroyImmediate(component);
		}
	}

	private void CopyCapsuleCollider(CapsuleCollider _source, GameObject _dest)
	{
		CapsuleCollider capsuleCollider = _dest.AddComponent<CapsuleCollider>();
		capsuleCollider.center = _source.center;
		capsuleCollider.radius = _source.radius;
		capsuleCollider.height = _source.height;
		capsuleCollider.direction = _source.direction;
	}

	private void CopyRigidbody(Rigidbody _source, GameObject _dest)
	{
		Rigidbody rigidbody = _dest.AddComponent<Rigidbody>();
		rigidbody.mass = _source.mass;
		rigidbody.drag = _source.drag;
		rigidbody.angularDrag = _source.angularDrag;
		rigidbody.useGravity = _source.useGravity;
		rigidbody.isKinematic = _source.isKinematic;
		rigidbody.interpolation = _source.interpolation;
		rigidbody.collisionDetectionMode = _source.collisionDetectionMode;
		rigidbody.constraints = _source.constraints;
	}

	private void CopyNavMeshAgent(NavMeshAgent _source, GameObject _dest)
	{
		NavMeshAgent navMeshAgent = _dest.AddComponent<NavMeshAgent>();
		navMeshAgent.radius = _source.radius;
		navMeshAgent.speed = _source.speed;
		navMeshAgent.acceleration = _source.acceleration;
		navMeshAgent.angularSpeed = _source.angularSpeed;
		navMeshAgent.stoppingDistance = _source.stoppingDistance;
		navMeshAgent.autoTraverseOffMeshLink = _source.autoTraverseOffMeshLink;
		navMeshAgent.autoBraking = _source.autoBraking;
		navMeshAgent.autoRepath = _source.autoRepath;
		navMeshAgent.height = _source.height;
		navMeshAgent.baseOffset = _source.baseOffset;
		navMeshAgent.obstacleAvoidanceType = _source.obstacleAvoidanceType;
		navMeshAgent.avoidancePriority = _source.avoidancePriority;
		navMeshAgent.areaMask = _source.areaMask;
	}

	private void CopyAnimator(Animator _source, Animator _dest)
	{
		_dest.runtimeAnimatorController = _source.runtimeAnimatorController;
		_dest.avatar = _source.avatar;
		_dest.applyRootMotion = _source.applyRootMotion;
		_dest.updateMode = _source.updateMode;
		_dest.cullingMode = _source.cullingMode;
	}

	private void CopyPrefabToTarget(GameObject _source, GameObject _dest)
	{
		CapsuleCollider component = _source.GetComponent<CapsuleCollider>();
		CopyCapsuleCollider(component, _dest);
		Rigidbody component2 = _source.GetComponent<Rigidbody>();
		CopyRigidbody(component2, _dest);
		NavMeshAgent component3 = _source.GetComponent<NavMeshAgent>();
		CopyNavMeshAgent(component3, _dest);
		AnimatorDataController component4 = _source.GetComponent<AnimatorDataController>();
		AnimatorDataController.Copy(component4, _dest);
		AnimationController component5 = _source.GetComponent<AnimationController>();
		AnimationController.Copy(component5, _dest);
		Animator component6 = _source.GetComponent<Animator>();
		Animator component7 = _dest.GetComponent<Animator>();
		if (component6 != null && component7 != null)
		{
			CopyAnimator(component6, component7);
		}
	}
}
