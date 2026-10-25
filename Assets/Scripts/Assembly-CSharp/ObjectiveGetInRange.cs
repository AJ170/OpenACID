using System;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;
using UnityEngine;

public abstract class ObjectiveGetInRange<TData> : ObjectiveTarget<TData> where TData : ObjectiveGetInRangeData, new()
{
	private const float c_descriptionUpdateDelta = 0.5f;

	private float m_curDistance = -0.5f;

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.InRange;
		}
	}

	protected override EObjectiveResult InitialResult
	{
		get
		{
			return EObjectiveResult.Undefined;
		}
	}

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return false;
		}
	}

	protected abstract Vector3 TargetPosition { get; }

	protected abstract float TargetRange { get; }

	public override void FrameUpdateImpl()
	{
		UpdateTargetPositions();
		float _distance;
		if (!TryComputeMinimalDistance(out _distance) || Math.Abs(_distance - m_curDistance) < 0.5f)
		{
			return;
		}
		m_curDistance = _distance;
		UpdateDescription();
		bool flag = m_curDistance <= TargetRange;
		bool flag2 = !base.Data.Inverse;
		if (flag2 && flag)
		{
			base.VisualTrackerState = EProgressState.Fulfilled;
			base.VisualSummaryState = true;
			base.Result = EObjectiveResult.Fulfilled;
			if (!base.Data.ResetIfInvalidated)
			{
				Complete();
			}
		}
		else if (!flag2 && flag)
		{
			base.VisualTrackerState = EProgressState.Failed;
			base.VisualSummaryState = false;
			base.Result = EObjectiveResult.Failed;
			if (!base.Data.ResetIfInvalidated)
			{
				Complete();
			}
		}
		else if (base.Data.ResetIfInvalidated)
		{
			base.VisualTrackerState = InitialVisualTrackerState;
			base.VisualSummaryState = InitialVisualSummaryState;
			base.Result = InitialResult;
		}
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Add("current_distance", m_curDistance);
		stringMapper.Add("target_distance", TargetRange);
		stringMapper.Add("remaining_distance", Math.Max(0f, m_curDistance - TargetRange));
		stringMapper.Add("Current", m_curDistance);
		stringMapper.Add("Threshold", TargetRange);
		return stringMapper;
	}

	private bool TryComputeMinimalDistance(out float _distance)
	{
		if (LogicMain.Player == null)
		{
			_distance = 0f;
			return false;
		}
		if (Targets == null || Targets.Count == 0)
		{
			_distance = (LogicMain.Player.transform.position - TargetPosition).magnitude;
		}
		else
		{
			_distance = float.MaxValue;
			for (int i = 0; i < Targets.Count; i++)
			{
				if (Targets[i] != null)
				{
					GameEntity gameEntity = Targets[i].GameEntity;
					if (gameEntity != null && gameEntity.IsValid)
					{
						float magnitude = (gameEntity.Position - TargetPosition).magnitude;
						_distance = Math.Min(magnitude, _distance);
					}
				}
			}
		}
		return true;
	}

	private void UpdateTargetPositions()
	{
		TargetPositions.ClearIfNotEmpty();
		if (ObjectiveData.MapIcon != 0)
		{
			TargetPositions.Add(TargetPosition);
		}
	}

	public override string ToString()
	{
		return "Range: " + TargetRange;
	}
}
