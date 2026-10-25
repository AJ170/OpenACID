using UnityEngine;

public class bloodDecalsHandler : MonoBehaviour
{
	public Transform[] decalTransforms;

	public int amountOfBloodDecals;

	private Transform[] decals;

	private int index;

	private ParticleCollisionEvent[] collisionEvents = new ParticleCollisionEvent[3];

	private void Awake()
	{
		decals = new Transform[amountOfBloodDecals];
		int num = 0;
		for (int i = 0; i < decals.Length; i++)
		{
			decals[i] = Object.Instantiate(decalTransforms[num], Vector3.zero, Quaternion.identity) as Transform;
			num++;
			if (num >= decalTransforms.Length)
			{
				num = 0;
			}
		}
	}

	private void OnParticleCollision(GameObject other)
	{
		int safeCollisionEventSize = GetComponent<ParticleSystem>().GetSafeCollisionEventSize();
		if (collisionEvents.Length < safeCollisionEventSize)
		{
			collisionEvents = new ParticleCollisionEvent[safeCollisionEventSize];
		}
		int num = GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);
		for (int i = 0; i < num; i++)
		{
			if (decals[index].gameObject.activeSelf)
			{
				break;
			}
			Vector3 position = collisionEvents[i].intersection + collisionEvents[i].normal * 0.1f;
			decals[index].position = position;
			decals[index].rotation = Quaternion.LookRotation(-collisionEvents[i].normal);
			decals[index].Rotate(collisionEvents[i].normal, Random.Range(0, 360), Space.World);
			decals[index].GetComponent<Renderer>().material.color = Color.white;
			decals[index].gameObject.SetActive(true);
			index++;
			if (index >= decals.Length)
			{
				index = 0;
			}
		}
	}
}
