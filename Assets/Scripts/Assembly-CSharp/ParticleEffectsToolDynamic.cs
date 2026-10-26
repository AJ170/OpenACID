using Assets.Scripts.Audio;
using UbiCore.Utils;
using UnityEngine;

public class ParticleEffectsToolDynamic : MonoBehaviour
{
	public enum BuildOrder
	{
		TopLeftBottomRight = 0,
		TopRightBottomLeft = 1,
		BottomLeftTopRight = 2,
		BottomRightTopLeft = 3
	}

	private static float uiScale = 0.0013020834f;

	[SerializeField]
	private BuildOrder buildOrder;

	[SerializeField]
	private UIWidget uiWidget;

	[SerializeField]
	private UIPanel uiPanel;

	[SerializeField]
	private int m_renderSortOrder = 2;

	[SerializeField]
	private bool calculateDynamically;

	[SerializeField]
	private float targetParticleSize;

	[SerializeField]
	private ParticleSystem particleSys;

	[SerializeField]
	private float minLifeTime;

	[SerializeField]
	private bool isAlwaysOnTop;

	[SerializeField]
	private bool isTimeScaleIndependent;

	[SerializeField]
	private int OverflowTop;

	[SerializeField]
	private int OverflowLeft;

	[SerializeField]
	private int OverflowRight;

	[SerializeField]
	private bool m_suppressAudio;

	private bool waitForStartInit = true;

	private bool showEffectAutoReset = true;

	private float currentParticleSize;

	private float width;

	private float height;

	private int numParticlesX;

	private int numParticlesY;

	private int m_maxParticles;

	public bool IsPlaying
	{
		get
		{
			return particleSys != null && particleSys.isPlaying;
		}
	}

	public ParticleSystem ParticleSys
	{
		get
		{
			return particleSys;
		}
		set
		{
			if (particleSys == null)
			{
				particleSys = value;
			}
		}
	}

	public void SetWidget(UIWidget _widget, bool _showEffect = true)
	{
		uiWidget = _widget;
		showEffectAutoReset = _showEffect;
	}

	public void updateScale()
	{
		if (uiPanel != null)
		{
			uiPanel.Update();
			width = uiPanel.width;
			height = uiPanel.height;
		}
		if (uiWidget != null)
		{
			uiWidget.Update();
			width = uiWidget.width;
			height = uiWidget.height;
		}
		if (calculateDynamically)
		{
			currentParticleSize = targetParticleSize * uiScale;
			particleSys.startSize = currentParticleSize;
			numParticlesX = Mathf.CeilToInt(width * uiScale / currentParticleSize) + OverflowLeft + OverflowRight;
			numParticlesY = Mathf.CeilToInt(height * uiScale / currentParticleSize) + OverflowTop + OverflowRight;
		}
	}

	private void Start()
	{
		waitForStartInit = true;
	}

	private void Update()
	{
		if (particleSys == null)
		{
			return;
		}
		if (waitForStartInit)
		{
			particleSys.simulationSpace = ParticleSystemSimulationSpace.Local;
			if (isAlwaysOnTop)
			{
				particleSys.GetComponent<Renderer>().sortingLayerName = "Foreground";
				particleSys.GetComponent<Renderer>().sortingOrder = m_renderSortOrder;
			}
			waitForStartInit = false;
		}
		if (isTimeScaleIndependent)
		{
			particleSys.Simulate(UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime, true, false);
		}
	}

	public void Stop()
	{
		particleSys.Stop();
	}

	[ContextMenu("Create Particles")]
	public void createParticlesDynamically()
	{
		if (showEffectAutoReset)
		{
			updateScale();
			m_maxParticles = numParticlesX * numParticlesY;
			particleSys.maxParticles = m_maxParticles;
			particleSys.Emit(m_maxParticles);
			ParticleSystem.Particle[] array = new ParticleSystem.Particle[particleSys.maxParticles];
			particleSys.GetParticles(array);
			int num = 0;
			switch (buildOrder)
			{
			case BuildOrder.BottomLeftTopRight:
			{
				for (float num8 = 0f; num8 < (float)numParticlesY; num8 += 1f)
				{
					for (float num9 = 0f; num9 < (float)numParticlesX; num9 += 1f)
					{
						Vector3 position = new Vector3((float)numParticlesX * particleSys.startSize / 2f - num9 * particleSys.startSize - particleSys.startSize / 2f, (float)numParticlesY * particleSys.startSize / 2f - num8 * particleSys.startSize - particleSys.startSize / 2f, 0f);
						array[num].position = position;
						array[num].startLifetime = particleSys.startLifetime + particleSys.startLifetime / (float)numParticlesX / (float)numParticlesY * num9 + particleSys.startLifetime / (float)numParticlesX / (float)numParticlesY * num8;
						array[num].remainingLifetime = minLifeTime + particleSys.startLifetime - array[num].remainingLifetime / (float)numParticlesX / (float)numParticlesY * num9 - array[num].remainingLifetime / (float)numParticlesX / (float)numParticlesY * num8;
						num++;
					}
				}
				break;
			}
			case BuildOrder.BottomRightTopLeft:
			{
				for (float num4 = 0f; num4 < (float)numParticlesY; num4 += 1f)
				{
					for (float num5 = 0f; num5 < (float)numParticlesX; num5 += 1f)
					{
						Vector3 vector2 = new Vector3((float)numParticlesX * particleSys.startSize / 2f - ((float)numParticlesX - num5) * particleSys.startSize + particleSys.startSize, (float)numParticlesY * particleSys.startSize / 2f - num4 * particleSys.startSize - particleSys.startSize, 0f);
						array[num].position = vector2 - new Vector3(particleSys.startSize, 0f, 0f);
						array[num].startLifetime = particleSys.startLifetime + particleSys.startLifetime / (float)numParticlesX / (float)numParticlesY * num5 + particleSys.startLifetime / (float)numParticlesX / (float)numParticlesY * num4;
						array[num].remainingLifetime = minLifeTime + particleSys.startLifetime - array[num].remainingLifetime / (float)numParticlesX / (float)numParticlesY * num5 - array[num].remainingLifetime / (float)numParticlesX / (float)numParticlesY * num4;
						num++;
					}
				}
				break;
			}
			case BuildOrder.TopRightBottomLeft:
			{
				for (float num6 = 0f; num6 < (float)numParticlesY; num6 += 1f)
				{
					for (float num7 = 0f; num7 < (float)numParticlesX; num7 += 1f)
					{
						Vector3 vector3 = new Vector3((float)numParticlesX * particleSys.startSize / 2f - ((float)numParticlesX - num7) * particleSys.startSize + particleSys.startSize, (float)numParticlesY * particleSys.startSize / 2f - ((float)numParticlesY - num6) * particleSys.startSize + particleSys.startSize * 1.5f, 0f);
						array[num].position = vector3 - new Vector3(particleSys.startSize, particleSys.startSize, 0f);
						array[num].startLifetime = particleSys.startLifetime + particleSys.startLifetime / (float)numParticlesX / (float)numParticlesY * num7 + particleSys.startLifetime / (float)numParticlesX / (float)numParticlesY * num6;
						array[num].remainingLifetime = minLifeTime + particleSys.startLifetime - array[num].remainingLifetime / (float)numParticlesX / (float)numParticlesY * num7 - array[num].remainingLifetime / (float)numParticlesX / (float)numParticlesY * num6;
						num++;
					}
				}
				break;
			}
			case BuildOrder.TopLeftBottomRight:
			{
				for (float num2 = 0f; num2 < (float)numParticlesY; num2 += 1f)
				{
					for (float num3 = 0f; num3 < (float)numParticlesX; num3 += 1f)
					{
						Vector3 vector = new Vector3((float)numParticlesX * particleSys.startSize / 2f - ((float)numParticlesX - ((float)numParticlesX - num3)) * particleSys.startSize - particleSys.startSize / 2f, (float)numParticlesY * particleSys.startSize / 2f - ((float)numParticlesY - num2) * particleSys.startSize + particleSys.startSize * 1.5f, 0f);
						array[num].position = vector - new Vector3(0f, particleSys.startSize, 0f);
						array[num].startLifetime = particleSys.startLifetime + particleSys.startLifetime / (float)numParticlesX / (float)numParticlesY * num3 + particleSys.startLifetime / (float)numParticlesX / (float)numParticlesY * num2;
						array[num].remainingLifetime = minLifeTime + particleSys.startLifetime - array[num].remainingLifetime / (float)numParticlesX / (float)numParticlesY * num3 - array[num].remainingLifetime / (float)numParticlesX / (float)numParticlesY * num2;
						num++;
					}
				}
				break;
			}
			}
			particleSys.SetParticles(array, m_maxParticles);
			if (Main.IsInitialized && Main.Instance.GameDB != null && !m_suppressAudio)
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_flash);
			}
		}
		showEffectAutoReset = true;
	}
}
