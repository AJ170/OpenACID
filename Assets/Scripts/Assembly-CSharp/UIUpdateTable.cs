using UnityEngine;

public class UIUpdateTable : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		GetComponent<UITable>().Reposition();
	}
}
