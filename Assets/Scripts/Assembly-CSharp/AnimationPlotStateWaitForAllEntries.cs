using System.Collections.Generic;

public class AnimationPlotStateWaitForAllEntries : AnimationPlotStateBase
{
	private List<AnimationPlotEntryBase> m_entries;

	public AnimationPlotStateWaitForAllEntries(List<AnimationPlotEntryBase> _entries)
	{
		m_entries = _entries;
		for (int i = 0; i < m_entries.Count; i++)
		{
			m_entries[i].Owner = this;
		}
	}

	public override void OnEntryFinished(AnimationPlotEntryBase _entry)
	{
		if (!base.Finished && CheckIsFinished())
		{
			base.Finished = true;
			base.Owner.OnStateFinished(this);
		}
	}

	private bool CheckIsFinished()
	{
		bool result = true;
		for (int i = 0; i < m_entries.Count; i++)
		{
			if (!m_entries[i].Finished)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public override void OnStart()
	{
		base.OnStart();
		for (int i = 0; i < m_entries.Count; i++)
		{
			m_entries[i].OnStart();
		}
	}
}
