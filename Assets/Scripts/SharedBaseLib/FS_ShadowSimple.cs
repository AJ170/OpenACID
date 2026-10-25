using UnityEngine;

[AddComponentMenu("Fast Shadows/Simple Shadow")]
public class FS_ShadowSimple : MonoBehaviour
{
	[HideInInspector]
	public float maxProjectionDistance = 100f;

	[HideInInspector]
	public float girth = 1f;

	[HideInInspector]
	public float shadowHoverHeight = 0.2f;

	public LayerMask layerMask = -1;

	[HideInInspector]
	public Material shadowMaterial;

	[HideInInspector]
	public bool isStatic;

	[HideInInspector]
	public bool useLightSource;

	[HideInInspector]
	public GameObject lightSource;

	[HideInInspector]
	public Vector3 lightDirection = new Vector3(0f, -1f, 0f);

	[HideInInspector]
	public bool doVisibilityCulling;

	[HideInInspector]
	public Rect uvs = new Rect(0f, 0f, 1f, 1f);

	[HideInInspector]
	public float opacity = 0.5f;

	[HideInInspector]
	public float raycastHitOpacityAdjustmentSpeed = 3f;

	[HideInInspector]
	public GameObject inheritFromTransform;

	[HideInInspector]
	public Vector3 inheritWorldOffset = Vector3.zero;

	[HideInInspector]
	public Vector3 localScale = Vector3.one;

	[HideInInspector]
	public bool useRaycastNormal = true;

	private float _girth;

	private bool _isStatic;

	private Vector3 _lightDirection = Vector3.zero;

	private bool isGoodPlaneIntersect;

	private Color gizmoColor = Color.white;

	private float currentOpacity;

	private Vector3[] _corners = new Vector3[4];

	private Ray r;

	private RaycastHit rh;

	private Bounds bounds;

	private Color _color = new Color(1f, 1f, 1f, 0f);

	private Vector3 _normal;

	private GameObject[] cornerGOs = new GameObject[4];

	private GameObject shadowCaster;

	private Plane shadowPlane;

	private FS_MeshKey meshKey;

	public Vector3[] corners
	{
		get
		{
			return _corners;
		}
	}

	public Color color
	{
		get
		{
			return _color;
		}
	}

	public Vector3 normal
	{
		get
		{
			return _normal;
		}
	}

	private void Awake()
	{
		_isStatic = isStatic;
		if (shadowMaterial == null)
		{
			shadowMaterial = (Material)Resources.Load("FS_ShadowMaterial", typeof(Material));
			bool flag = shadowMaterial == null;
		}
		if (isStatic)
		{
			CalculateShadowGeometry();
		}
	}

	public void CalculateShadowGeometry()
	{
		if (shadowMaterial == null)
		{
			return;
		}
		if (inheritFromTransform != null)
		{
			base.transform.position = inheritFromTransform.transform.position + inheritWorldOffset;
			Vector3 vector = ((!useLightSource) ? inheritFromTransform.transform.forward : lightSource.transform.forward);
			vector.y = 0f;
			base.transform.rotation = Quaternion.LookRotation(vector.normalized);
		}
		if (useLightSource && lightSource == null)
		{
			useLightSource = false;
		}
		if (useLightSource)
		{
			lightDirection = lightSource.transform.forward;
		}
		else if (lightDirection != _lightDirection || lightDirection == Vector3.zero)
		{
			if (lightDirection == Vector3.zero)
			{
				lightDirection = -Vector3.up;
			}
			lightDirection.Normalize();
			_lightDirection = lightDirection;
		}
		if (shadowCaster == null || girth != _girth)
		{
			if (shadowCaster == null)
			{
				shadowCaster = new GameObject("shadowSimple");
				cornerGOs = new GameObject[4];
				for (int i = 0; i < 4; i++)
				{
					(cornerGOs[i] = new GameObject("c" + i)).transform.parent = shadowCaster.transform;
				}
				shadowCaster.transform.parent = base.transform;
				shadowCaster.transform.localPosition = Vector3.zero;
				shadowCaster.transform.localRotation = Quaternion.identity;
				shadowCaster.transform.localScale = Vector3.one;
			}
			Vector3 forward = ((!(Mathf.Abs(Vector3.Dot(base.transform.forward, lightDirection)) < 0.9f)) ? (base.transform.up - Vector3.Dot(base.transform.up, lightDirection) * lightDirection) : (base.transform.forward - Vector3.Dot(base.transform.forward, lightDirection) * lightDirection));
			shadowCaster.transform.rotation = Quaternion.LookRotation(forward, -lightDirection);
			if (useLightSource)
			{
				cornerGOs[0].transform.position = shadowCaster.transform.position + girth * (lightSource.transform.up * localScale.y - lightSource.transform.right * localScale.x);
				cornerGOs[1].transform.position = shadowCaster.transform.position + girth * (lightSource.transform.up * localScale.y + lightSource.transform.right * localScale.x);
				cornerGOs[2].transform.position = shadowCaster.transform.position + girth * (-lightSource.transform.up * localScale.y + lightSource.transform.right * localScale.x);
				cornerGOs[3].transform.position = shadowCaster.transform.position + girth * (-lightSource.transform.up * localScale.y - lightSource.transform.right * localScale.x);
			}
			else
			{
				cornerGOs[0].transform.position = shadowCaster.transform.position + girth * (shadowCaster.transform.forward * localScale.y - shadowCaster.transform.right * localScale.x);
				cornerGOs[1].transform.position = shadowCaster.transform.position + girth * (shadowCaster.transform.forward * localScale.y + shadowCaster.transform.right * localScale.x);
				cornerGOs[2].transform.position = shadowCaster.transform.position + girth * (-shadowCaster.transform.forward * localScale.y + shadowCaster.transform.right * localScale.x);
				cornerGOs[3].transform.position = shadowCaster.transform.position + girth * (-shadowCaster.transform.forward * localScale.y - shadowCaster.transform.right * localScale.x);
			}
			_girth = girth;
		}
		Transform transform = shadowCaster.transform;
		r.origin = transform.position;
		r.direction = lightDirection;
		if (maxProjectionDistance > 0f && Physics.Raycast(r, out rh, maxProjectionDistance, layerMask))
		{
			if (doVisibilityCulling)
			{
				Plane[] cameraFustrumPlanes = FS_ShadowManager.Manager().getCameraFustrumPlanes();
				bounds.center = rh.point;
				bounds.size = new Vector3(2f * girth, 2f * girth, 2f * girth);
				if (!GeometryUtility.TestPlanesAABB(cameraFustrumPlanes, bounds))
				{
					return;
				}
			}
			Vector3 forward = ((!(Mathf.Abs(Vector3.Dot(base.transform.forward, lightDirection)) < 0.9f)) ? (base.transform.up - Vector3.Dot(base.transform.up, lightDirection) * lightDirection) : (base.transform.forward - Vector3.Dot(base.transform.forward, lightDirection) * lightDirection));
			shadowCaster.transform.rotation = Quaternion.Lerp(shadowCaster.transform.rotation, Quaternion.LookRotation(forward, -lightDirection), 0.01f);
			float num = rh.distance - shadowHoverHeight;
			float num2 = 1f - num / maxProjectionDistance;
			if (num2 < 0f)
			{
				return;
			}
			num2 = Mathf.Clamp01(num2);
			currentOpacity = Mathf.Min(currentOpacity + raycastHitOpacityAdjustmentSpeed * Time.deltaTime, 1f);
			if (useRaycastNormal && rh.normal.y > 0.3f)
			{
				_normal = rh.normal;
			}
			else
			{
				_normal = Vector3.up;
			}
			Vector3 vector2 = rh.point - shadowHoverHeight * lightDirection;
			shadowPlane.SetNormalAndPosition(_normal, vector2);
			Camera main = Camera.main;
			if (main != null && main.gameObject != null && main.gameObject.transform != null)
			{
				Vector3 position = main.gameObject.transform.position;
				float num3 = Vector3.Distance(vector2, position);
				currentOpacity = Mathf.SmoothStep(0f, currentOpacity, num3 - QualitySettings.shadowDistance + 2f);
			}
			_color.a = Mathf.Lerp(0f, currentOpacity * opacity, num2);
			isGoodPlaneIntersect = true;
			float enter = 0f;
			r.origin = cornerGOs[0].transform.position;
			isGoodPlaneIntersect = shadowPlane.Raycast(r, out enter);
			if (!isGoodPlaneIntersect && enter == 0f)
			{
				return;
			}
			isGoodPlaneIntersect = true;
			_corners[0] = r.origin + r.direction * enter;
			r.origin = cornerGOs[1].transform.position;
			isGoodPlaneIntersect = shadowPlane.Raycast(r, out enter);
			if (!isGoodPlaneIntersect && enter == 0f)
			{
				return;
			}
			isGoodPlaneIntersect = true;
			_corners[1] = r.origin + r.direction * enter;
			r.origin = cornerGOs[2].transform.position;
			isGoodPlaneIntersect = shadowPlane.Raycast(r, out enter);
			if (!isGoodPlaneIntersect && enter == 0f)
			{
				return;
			}
			isGoodPlaneIntersect = true;
			_corners[2] = r.origin + r.direction * enter;
			r.origin = cornerGOs[3].transform.position;
			isGoodPlaneIntersect = shadowPlane.Raycast(r, out enter);
			if (!isGoodPlaneIntersect && enter == 0f)
			{
				return;
			}
			isGoodPlaneIntersect = true;
			_corners[3] = r.origin + r.direction * enter;
			if (isGoodPlaneIntersect)
			{
				if (meshKey == null || meshKey.mat != shadowMaterial || meshKey.isStatic != isStatic)
				{
					meshKey = new FS_MeshKey(shadowMaterial, isStatic);
				}
				FS_ShadowManager.Manager().registerGeometry(this, meshKey);
				gizmoColor = Color.white;
			}
			else
			{
				gizmoColor = Color.magenta;
			}
		}
		else
		{
			isGoodPlaneIntersect = false;
			gizmoColor = Color.red;
			currentOpacity = 0f;
		}
	}

	private void LateUpdate()
	{
		if (_isStatic != isStatic)
		{
			if (isStatic)
			{
				meshKey = new FS_MeshKey(shadowMaterial, isStatic);
				CalculateShadowGeometry();
				FS_ShadowManager.Manager().RecalculateStaticGeometry(null, meshKey);
			}
			else
			{
				FS_MeshKey fS_MeshKey = meshKey;
				meshKey = new FS_MeshKey(shadowMaterial, isStatic);
				FS_ShadowManager.Manager().RecalculateStaticGeometry(this, fS_MeshKey);
			}
			_isStatic = isStatic;
		}
		if (!isStatic)
		{
			CalculateShadowGeometry();
		}
	}

	private void OnDrawGizmos()
	{
		if (shadowCaster != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawRay(shadowCaster.transform.position, shadowCaster.transform.up);
			Gizmos.DrawRay(shadowCaster.transform.position, shadowCaster.transform.forward);
			Gizmos.DrawRay(shadowCaster.transform.position, shadowCaster.transform.right);
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(shadowCaster.transform.position, base.transform.forward);
			Gizmos.color = gizmoColor;
			if (isGoodPlaneIntersect)
			{
				Gizmos.DrawLine(cornerGOs[0].transform.position, corners[0]);
				Gizmos.DrawLine(cornerGOs[1].transform.position, corners[1]);
				Gizmos.DrawLine(cornerGOs[2].transform.position, corners[2]);
				Gizmos.DrawLine(cornerGOs[3].transform.position, corners[3]);
				Gizmos.DrawLine(cornerGOs[0].transform.position, cornerGOs[1].transform.position);
				Gizmos.DrawLine(cornerGOs[1].transform.position, cornerGOs[2].transform.position);
				Gizmos.DrawLine(cornerGOs[2].transform.position, cornerGOs[3].transform.position);
				Gizmos.DrawLine(cornerGOs[3].transform.position, cornerGOs[0].transform.position);
				Gizmos.DrawLine(corners[0], corners[1]);
				Gizmos.DrawLine(corners[1], corners[2]);
				Gizmos.DrawLine(corners[2], corners[3]);
				Gizmos.DrawLine(corners[3], corners[0]);
			}
		}
	}
}
