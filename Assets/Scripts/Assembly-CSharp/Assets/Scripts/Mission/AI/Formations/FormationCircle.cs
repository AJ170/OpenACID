using UnityEngine;

namespace Assets.Scripts.Mission.AI.Formations
{
	public class FormationCircle : AbstractFormation
	{
		private const float DeviationThreshold = 0.01f;

		private const float FactorHalfExtend = 0.5f;

		protected override void CalculateLocalPositions()
		{
			bool flag = base.MaxDeviation > 0.01f;
			float num = 360f / (float)MaxCount;
			float num2 = (0f - num) * (float)(MaxCount - 1) / 2f;
			float num3 = 1f;
			float num4 = 0f;
			int num5 = 0;
			for (int i = 0; i < MaxCount; i++)
			{
				Quaternion quaternion = Quaternion.AngleAxis(num2 + num3 * num4 * num, Vector3.up);
				float num6 = 0.5f * base.MaxExtend;
				m_positions[i] = quaternion * (Vector3.forward * num6);
				if (flag)
				{
					Vector2 vector = Random.insideUnitCircle * base.MaxDeviation;
					m_positions[i].x += vector.x;
					m_positions[i].z += vector.y;
				}
				if (num5 == 1)
				{
					num3 *= -1f;
					num5 = 0;
				}
				else
				{
					num4 += 1f;
					num5++;
				}
			}
		}
	}
}
