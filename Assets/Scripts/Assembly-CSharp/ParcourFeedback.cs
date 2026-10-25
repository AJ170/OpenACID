using Assets.Scripts.Mission.Motion;
using UnityEngine;

public class ParcourFeedback : MonoBehaviour
{
	public Transform StartTransform;

	public Transform TargetTransform;

	public float LineWidth = 0.5f;

	public int LineSegments = 32;

	public float LineDefaultHeight = 1f;

	public float LineEdgeHeightOffset = 1f;

	public AnimationCurve LineDefaultStraightCurve = new AnimationCurve();

	public Color FallDamageColor = Color.Lerp(Color.red, Color.white, 0.25f);

	public Color NoFallDamageColor = Color.white;

	public float LineStartHeight = 0.25f;

	public float LineStartOffset = -0.4f;

	public float LineStartDamping = 0.025f;

	public float LineEndHeight;

	public float LineEndOffset = 0.4f;

	public float LineEndDamping = 0.2f;

	public float ClimbLineTailApproachLength = 2f;

	public float ClimbLineTailFacingLength = 1f;

	public float ClimbLineBezierStrength = 1f;

	public float RunPassOverLineStartApproachLength = 0.7f;

	public float RunPassOverLineStartFacingLength = 0.3f;

	public float RunPassOverLineEndApproachLength = -0.4f;

	public float RunPassOverLineEndFacingLength;

	public float RunPassOverDownLineStartApproachLength = 0.7f;

	public float RunPassOverDownLineStartFacingLength = 0.3f;

	public float RunPassOverDownLineEndApproachLength = -0.4f;

	public float RunPassOverDownLineEndFacingLength;

	public float RunPassOverDownLineHeightOffsetMultiplier = 0.2f;

	public float RunPassOverDownLineEndFacingOffsetMultiplier = 0.2f;

	public float LineDisplayDistance = 10f;

	public float LineDisplayPow = 2f;

	public float LineFadeSpeed = 2f;

	public bool AlwaysAddLine;

	public AnimationCurve ClimbLinkHeightCurve;

	public Vector3 LineStart;

	public Vector3 LineEnd;

	private Renderer m_renderer;

	private MeshFilter m_meshFilter;

	private ParcourHelper.EEnvironmentInteraction m_lastInteraction;

	private eLinkType m_lastLinkType = eLinkType.Invalid;

	private Vector3[] m_vertices;

	private Vector2[] m_uv;

	private Vector2[] m_uv2;

	private Color32[] m_colors32;

	private Vector3[] m_normals;

	private Vector4[] m_tangents;

	private int[] m_indices;

	private int m_propTintColorId;

	private ParcourPrediction m_prediction;

	private void Start()
	{
		m_renderer = GetComponent<Renderer>();
		m_propTintColorId = Shader.PropertyToID("_TintColor");
		m_meshFilter = GetComponent<MeshFilter>();
		m_meshFilter.sharedMesh = Object.Instantiate(m_meshFilter.sharedMesh);
		m_meshFilter.sharedMesh.MarkDynamic();
		int num = (LineSegments + 1) * 2;
		m_vertices = new Vector3[num];
		m_uv = new Vector2[num];
		m_uv2 = new Vector2[num];
		m_colors32 = new Color32[num];
		m_normals = new Vector3[num];
		m_tangents = new Vector4[num];
		m_indices = new int[LineSegments * 2 * 3];
		for (int i = 0; i <= LineSegments; i++)
		{
			m_normals[i * 2] = Vector3.up;
			m_normals[i * 2 + 1] = Vector3.up;
			m_tangents[i * 2] = Vector3.right;
			m_tangents[i * 2 + 1] = Vector3.right;
			m_colors32[i * 2] = new Color32(1, 1, 1, 1);
			m_colors32[i * 2 + 1] = new Color32(1, 1, 1, 1);
		}
		BuildDefaultLine(Vector3.forward);
	}

	private void OnDestroy()
	{
		Object.Destroy(m_renderer.material);
	}

	public bool ShouldDisplayLine()
	{
		return m_prediction != null && (m_prediction.HasInteraction || AlwaysAddLine) && m_prediction.Interaction != ParcourHelper.EEnvironmentInteraction.Rebound && m_prediction.Interaction != 0 && m_prediction.Interaction != ParcourHelper.EEnvironmentInteraction.Hotspot;
	}

	private bool ShouldDisplayTargetDecal()
	{
		return m_prediction != null && (m_prediction.HasInteraction || AlwaysAddLine) && (m_prediction.Interaction == ParcourHelper.EEnvironmentInteraction.JumpForward || m_prediction.Interaction == ParcourHelper.EEnvironmentInteraction.JumpDown || m_prediction.Interaction == ParcourHelper.EEnvironmentInteraction.Link);
	}

	public void Update()
	{
		m_prediction = AcierInputHandler.Instance.ParcourPrediction;
		if (ShouldDisplayLine())
		{
			ShowLineUpdate();
			m_lastInteraction = m_prediction.Interaction;
			m_lastLinkType = m_prediction.LinkType;
		}
		else
		{
			HideLineUpdate();
		}
	}

	private void UpdateLineEnds()
	{
		float t = LogicMain.FrameTime / (LogicMain.FrameTime + LineStartDamping);
		float t2 = LogicMain.FrameTime / (LogicMain.FrameTime + LineEndDamping);
		if (m_lastInteraction != m_prediction.Interaction || m_lastInteraction == ParcourHelper.EEnvironmentInteraction.CivilianCover)
		{
			t = 1f;
			t2 = 1f;
		}
		Vector3 b = m_prediction.StartPosition - m_prediction.StartDirection * LineStartOffset + Vector3.up * LineStartHeight;
		Vector3 b2 = m_prediction.EndPosition - m_prediction.StartDirection * LineEndOffset + Vector3.up * LineEndHeight;
		LineStart = Vector3.Lerp(LineStart, b, t);
		LineEnd = Vector3.Lerp(LineEnd, b2, t2);
	}

	private void ShowLineUpdate()
	{
		UpdateLineEnds();
		base.transform.position = LineStart;
		Vector3 forward = Camera.main.transform.forward;
		forward.y = 0f;
		base.transform.rotation = Quaternion.LookRotation(forward);
		switch (m_prediction.Interaction)
		{
		case ParcourHelper.EEnvironmentInteraction.Link:
			switch (m_prediction.LinkType)
			{
			case eLinkType.Climb:
				PrepareClimbLine();
				break;
			case eLinkType.RunPassOverDown:
				PrepareRunPassOverDownLine();
				break;
			case eLinkType.RunPassOver:
				PrepareRunPassOverLine();
				break;
			case eLinkType.JumpDown:
				PrepareDefaultLine();
				break;
			default:
				PrepareDefaultLine();
				break;
			}
			break;
		case ParcourHelper.EEnvironmentInteraction.JumpForward:
		case ParcourHelper.EEnvironmentInteraction.JumpDown:
			PrepareDefaultLine();
			break;
		default:
			PrepareDefaultLine();
			break;
		}
		float num = m_prediction.StartPosition.y - m_prediction.EndPosition.y;
		Color color = ((!(num > ParcourHelper.Acrobatics.JumpMaxFallHeightNoDamage)) ? NoFallDamageColor : FallDamageColor);
		Color color2 = m_renderer.material.GetColor(m_propTintColorId);
		float num2 = 1f - Mathf.Pow(Mathf.Clamp01(m_prediction.DistanceToStart / LineDisplayDistance), LineDisplayPow);
		color.a = Mathf.Clamp01(Mathf.Min(color2.a + LogicMain.FrameTime * LineFadeSpeed, num2 * 0.8f));
		m_renderer.material.SetColor(m_propTintColorId, color);
		m_renderer.enabled = true;
		if (StartTransform != null)
		{
			StartTransform.gameObject.SetActive(true);
			StartTransform.position = LineStart;
		}
		if (TargetTransform != null)
		{
			TargetTransform.gameObject.SetActive(ShouldDisplayTargetDecal() && color.a > 0.5f);
			TargetTransform.position = LineEnd;
		}
	}

	private void PrepareDefaultLine()
	{
		BuildDefaultLine(Quaternion.Inverse(base.transform.rotation) * (LineEnd - LineStart));
	}

	private void PrepareRunPassOverLine()
	{
		BuildPassOverLine(base.transform.InverseTransformPoint(LineStart - (m_prediction.CurrentDirection * RunPassOverLineStartApproachLength + m_prediction.StartDirection * RunPassOverLineStartFacingLength)), base.transform.InverseTransformPoint(LineEnd + -(m_prediction.CurrentDirection * RunPassOverLineEndApproachLength + m_prediction.StartDirection * RunPassOverLineEndFacingLength)), base.transform.InverseTransformPoint(m_prediction.Link.ClimbRoute.ClimbRouteEntries[0].Edge.ClosestPoint(LineStart)));
	}

	private void PrepareRunPassOverDownLine()
	{
		Vector3 position = m_prediction.Link.ClimbRoute.ClimbRouteEntries[0].Edge.ClosestPoint(LineStart);
		float num = position.y - LineStart.y;
		BuildPassOverDownLine(base.transform.InverseTransformPoint(LineStart - (m_prediction.CurrentDirection * RunPassOverDownLineStartApproachLength + m_prediction.StartDirection * RunPassOverDownLineStartFacingLength)), base.transform.InverseTransformPoint(LineEnd + -(m_prediction.CurrentDirection * (RunPassOverDownLineEndApproachLength + num * RunPassOverDownLineEndFacingOffsetMultiplier) + m_prediction.StartDirection * RunPassOverDownLineEndFacingLength)), base.transform.InverseTransformPoint(position));
	}

	private void PrepareClimbLine()
	{
		BuildClimbLine(base.transform.InverseTransformPoint(LineStart - (m_prediction.CurrentDirection * ClimbLineTailApproachLength + m_prediction.StartDirection * ClimbLineTailFacingLength)), base.transform.InverseTransformPoint(LineStart), base.transform.InverseTransformPoint(LineEnd), base.transform.InverseTransformDirection(m_prediction.CurrentDirection), base.transform.InverseTransformDirection(m_prediction.StartDirection));
	}

	private void HideLineUpdate()
	{
		if (StartTransform != null)
		{
			StartTransform.gameObject.SetActive(false);
		}
		if (TargetTransform != null)
		{
			TargetTransform.gameObject.SetActive(false);
		}
		Color color = m_renderer.material.GetColor(m_propTintColorId);
		color.a = Mathf.Clamp01(color.a - LogicMain.FrameTime * LineFadeSpeed);
		m_renderer.material.SetColor(m_propTintColorId, color);
		m_renderer.enabled = color.a > 0.01f;
		m_lastInteraction = ParcourHelper.EEnvironmentInteraction.Invalid;
		m_lastLinkType = eLinkType.Invalid;
	}

	private void BuildDefaultLine(Vector3 offset)
	{
		float y = Mathf.Max(offset.y * 0.5f, 0f) + LineDefaultHeight;
		Vector3 p = new Vector3(0f, y, 0f);
		Vector3 p2 = new Vector3(offset.x, y, offset.z);
		for (int i = 0; i <= LineSegments; i++)
		{
			float num = (float)i / (float)LineSegments;
			Vector3 a = CubicBezier(Vector3.zero, p, p2, offset, num);
			Vector3 b = Vector3.Lerp(Vector3.zero, offset, num);
			a = Vector3.Lerp(a, b, LineDefaultStraightCurve.Evaluate(num));
			m_vertices[i * 2] = a + Vector3.left * LineWidth * 0.5f;
			m_vertices[i * 2 + 1] = a + Vector3.right * LineWidth * 0.5f;
		}
		FinalizeMeshData();
	}

	private void BuildPassOverDownLine(Vector3 start, Vector3 end, Vector3 edgePoint)
	{
		float num = edgePoint.y - end.y;
		float y = edgePoint.y + LineEdgeHeightOffset + num * RunPassOverDownLineHeightOffsetMultiplier;
		Vector3 p = new Vector3(start.x, y, start.z);
		Vector3 p2 = new Vector3(end.x, y, end.z);
		for (int i = 0; i <= LineSegments; i++)
		{
			float t = (float)i / (float)LineSegments;
			Vector3 vector = CubicBezier(start, p, p2, end, t);
			m_vertices[i * 2] = vector + Vector3.left * LineWidth * 0.5f;
			m_vertices[i * 2 + 1] = vector + Vector3.right * LineWidth * 0.5f;
		}
		FinalizeMeshData();
	}

	private void BuildPassOverLine(Vector3 start, Vector3 end, Vector3 edgePoint)
	{
		float y = edgePoint.y + LineEdgeHeightOffset;
		Vector3 p = new Vector3(start.x, y, start.z);
		Vector3 p2 = new Vector3(end.x, y, end.z);
		for (int i = 0; i <= LineSegments; i++)
		{
			float t = (float)i / (float)LineSegments;
			Vector3 vector = CubicBezier(start, p, p2, end, t);
			m_vertices[i * 2] = vector + Vector3.left * LineWidth * 0.5f;
			m_vertices[i * 2 + 1] = vector + Vector3.right * LineWidth * 0.5f;
		}
		FinalizeMeshData();
	}

	private void BuildClimbLine(Vector3 leadStart, Vector3 climbStart, Vector3 climbEnd, Vector3 leadDirection, Vector3 climbDirection)
	{
		for (int i = 0; i <= LineSegments; i++)
		{
			float num = (float)i / (float)LineSegments;
			Vector3 vector = CubicBezier(leadStart, leadStart + leadDirection * ClimbLineBezierStrength, climbStart, climbEnd, num);
			if (ClimbLinkHeightCurve != null)
			{
				vector.y = climbStart.y + LineStartHeight + ClimbLinkHeightCurve.Evaluate(num) * (climbEnd.y - climbStart.y);
			}
			m_vertices[i * 2] = vector + Vector3.left * LineWidth * 0.5f;
			m_vertices[i * 2 + 1] = vector + Vector3.right * LineWidth * 0.5f;
		}
		FinalizeMeshData();
	}

	private void FinalizeMeshData()
	{
		for (int i = 0; i <= LineSegments; i++)
		{
			float y = (float)i / (float)LineSegments;
			m_uv[i * 2].x = -0.5f;
			m_uv[i * 2].y = y;
			m_uv[i * 2 + 1].x = 0.5f;
			m_uv[i * 2 + 1].y = y;
			m_uv2[i * 2].x = -0.5f;
			m_uv2[i * 2].y = y;
			m_uv2[i * 2 + 1].x = 0.5f;
			m_uv2[i * 2 + 1].y = y;
		}
		for (int j = 0; j < LineSegments; j++)
		{
			m_indices[j * 6] = j * 2;
			m_indices[j * 6 + 1] = j * 2 + 2;
			m_indices[j * 6 + 2] = j * 2 + 1;
			m_indices[j * 6 + 3] = j * 2 + 1;
			m_indices[j * 6 + 4] = j * 2 + 2;
			m_indices[j * 6 + 5] = j * 2 + 3;
		}
		if (m_meshFilter != null)
		{
			SetMeshData();
		}
	}

	private void SetMeshData()
	{
		Mesh sharedMesh = m_meshFilter.sharedMesh;
		sharedMesh.Clear();
		sharedMesh.vertices = m_vertices;
		sharedMesh.uv = m_uv;
		sharedMesh.uv2 = m_uv2;
		sharedMesh.normals = m_normals;
		sharedMesh.tangents = m_tangents;
		sharedMesh.colors32 = m_colors32;
		sharedMesh.SetTriangles(m_indices, 0);
	}

	private static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		float num = 1f - t;
		return p0 * (num * num * num) + p1 * (3f * num * num * t) + p2 * (3f * num * t * t) + p3 * (t * t * t);
	}
}
