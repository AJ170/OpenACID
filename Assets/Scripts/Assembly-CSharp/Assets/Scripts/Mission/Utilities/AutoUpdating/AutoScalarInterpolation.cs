using UnityEngine;

namespace Assets.Scripts.Mission.Utilities.AutoUpdating
{
	public class AutoScalarInterpolation : AutoInterpolation
	{
		private float m_Scalar;

		public float From { get; set; }

		public float To { get; set; }

		public float Scalar
		{
			get
			{
				return m_Scalar;
			}
		}

		protected override bool UpdateFrameInternal(float _gameTimeDelta)
		{
			bool result = base.UpdateFrameInternal(_gameTimeDelta);
			m_Scalar = Mathf.Lerp(From, To, base.Value);
			return result;
		}
	}
}
