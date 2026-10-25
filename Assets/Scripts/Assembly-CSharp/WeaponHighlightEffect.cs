using System.Collections;
using UnityEngine;

public class WeaponHighlightEffect : EffectBase
{
	private bool IsSetUp;

	public Color EffectColor;

	public float FadeSpeed = 5f;

	private Color StartColor;

	private float Visibility;

	private bool FadingIn;

	private bool IsActive;

	private WeaponHandler weaponHandler;

	public ParticleSystem AppearEffect;

	private void Start()
	{
		weaponHandler = LogicMain.Player.GetComponent<WeaponHandler>();
	}

	private void FindMesh()
	{
		bool flag = false;
		foreach (Transform item in base.transform.parent)
		{
			if (item.tag == "Weapon")
			{
				base.transform.GetComponent<MeshFilter>().mesh = item.GetComponent<MeshFilter>().mesh;
				StartColor = EffectColor;
				StartColor.a = Visibility;
				base.transform.parent = item;
				flag = true;
			}
		}
		if (!flag)
		{
			Debug.LogWarning("No Weapon mesh found! " + base.transform.name);
		}
		else
		{
			IsSetUp = true;
		}
	}

	private void Update()
	{
		if (!IsSetUp)
		{
			FindMesh();
		}
		if (IsActive)
		{
			if (weaponHandler.WeaponDrawn)
			{
				FadingIn = true;
				StartCoroutine(FadeIn());
			}
			else
			{
				FadingIn = false;
				StartCoroutine(FadeOut());
			}
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			On();
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			Off();
		}
	}

	public override void On()
	{
		Debug.Log("On");
		AppearEffect.Play();
		IsActive = true;
	}

	public override void Off()
	{
		Debug.Log("Off");
		FadingIn = false;
		IsActive = false;
		StartCoroutine(FadeOut());
	}

	public override void Trigger()
	{
		Debug.Log("Trigger");
		AppearEffect.Play();
		IsActive = true;
	}

	private IEnumerator FadeIn()
	{
		while (true)
		{
			if (!IsSetUp)
			{
				yield return 0;
			}
			if (!FadingIn)
			{
				break;
			}
			StartColor.a = Mathf.Lerp(StartColor.a, 1f, FadeSpeed * Time.deltaTime);
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", StartColor);
			if (StartColor.a >= 0.99f)
			{
				break;
			}
			yield return 0;
		}
	}

	private IEnumerator FadeOut()
	{
		while (true)
		{
			if (!IsSetUp)
			{
				yield return 0;
			}
			if (FadingIn)
			{
				break;
			}
			StartColor.a = Mathf.Lerp(StartColor.a, 0f, FadeSpeed * Time.deltaTime);
			base.transform.GetComponent<Renderer>().material.SetColor("_Color", StartColor);
			if (StartColor.a <= 0.01f)
			{
				break;
			}
			yield return 0;
		}
	}
}
