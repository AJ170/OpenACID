using UnityEngine;

namespace Assets.Scripts.Mission.Utilities.AutoUpdating
{
	public class AutoRotationInterpolation : AutoInterpolation
	{
		private Quaternion m_Rotation;

		public Quaternion From { get; set; }

		public Quaternion To { get; set; }

		public Quaternion Rotation
		{
			get
			{
				return m_Rotation;
			}
		}

		protected override bool UpdateFrameInternal(float _gameTimeDelta)
		{
			bool result = base.UpdateFrameInternal(_gameTimeDelta);
			m_Rotation = Quaternion.Slerp(From, To, base.Value);
			return result;
		}
	}
}
