using UnityEngine;

public class ParticleScaler : MonoBehaviour
{
	private void Start()
	{
		float x = base.transform.parent.localScale.x;
		GetComponent<ParticleSystem>().startSpeed *= x;
		GetComponent<ParticleSystem>().startSize *= x;
	}
}
