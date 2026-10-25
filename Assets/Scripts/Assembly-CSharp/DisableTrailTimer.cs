using UnityEngine;

public class DisableTrailTimer : MonoBehaviour
{
	public float lifeTime;

	public EffectWeaponTrail effectWeaponTrail;

	private float startTime;

	private void Start()
	{
		startTime = Time.time;
	}

	private void LateUpdate()
	{
		if (Time.time > startTime + lifeTime && effectWeaponTrail != null)
		{
			effectWeaponTrail.Off();
		}
	}
}
