using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
	public StalkingZoneComponent StalkingZoneComponent { get; set; }

	private void OnTriggerEnter(Collider _collider)
	{
		EntityWrapperComponent component = _collider.gameObject.GetComponent<EntityWrapperComponent>();
		if (component != null && component.Entity != null)
		{
			StalkingZoneComponent.OnCollisionEnter(component.Entity.Id);
		}
	}

	private void OnTriggerExit(Collider _collider)
	{
		EntityWrapperComponent component = _collider.gameObject.GetComponent<EntityWrapperComponent>();
		if (component != null && component.Entity != null)
		{
			StalkingZoneComponent.OnCollisionExit(component.Entity.Id);
		}
	}
}
