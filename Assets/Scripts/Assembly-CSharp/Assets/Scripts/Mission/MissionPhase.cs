using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using Mission;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UnityEngine;

namespace Assets.Scripts.Mission
{
	public class MissionPhase
	{
		public delegate void OnProgressHandler();

		private readonly BBList<IObjectiveBase> m_objectives = new BBList<IObjectiveBase>();

		private readonly BBList<IObjectiveBase> m_updateObjectives = new BBList<IObjectiveBase>();

		private readonly BBList<IObjectiveBase> m_waitingObjectives = new BBList<IObjectiveBase>();

		private BBList<IMissionBase> m_newEntries = new BBList<IMissionBase>();

		private int m_lastUpdateTurn = -1;

		public MissionRoot Root { get; private set; }

		public BBList<IMissionBase> MissionEntries { get; private set; }

		public BBList<IObjectiveBase> MissionObjectives
		{
			get
			{
				return m_objectives;
			}
		}

		public MissionPlayerSpawn MissionPlayerSpawn { get; private set; }

		public event OnProgressHandler OnProgress;

		public MissionPhase(MissionRoot _root)
		{
			Root = _root;
			MissionEntries = new BBList<IMissionBase>();
		}

		public void Destroy()
		{
			UnityEngine.Object.Destroy(Root);
			MissionObjectives.Clear();
			for (int i = 0; i < MissionEntries.Count; i++)
			{
				IMissionBase missionBase = MissionEntries[i];
				missionBase.OnDeactivate();
				UnityEngine.Object.Destroy((MissionMonoBehaviour)missionBase);
			}
			MissionEntries.Clear();
		}

		public EObjectiveResult IsFullFilled(bool _ignoreProgress)
		{
			IObjectiveBase objectiveBase = m_objectives.Find((IObjectiveBase obj) => obj.ObjectiveData.IsPrimaryObjective);
			if (objectiveBase == null)
			{
				if (m_objectives.Count == 0)
				{
					return EObjectiveResult.Fulfilled;
				}
				if (m_objectives.Count == 1)
				{
					return m_objectives[0].Result;
				}
				return EObjectiveResult.Failed;
			}
			ObjectiveBaseData objectiveData = objectiveBase.ObjectiveData;
			bool flag = true;
			for (int i = 0; i < m_objectives.Count; i++)
			{
				ObjectiveBaseData objectiveData2 = m_objectives[i].ObjectiveData;
				if (objectiveData2.IsOptionalObjective)
				{
					continue;
				}
				switch (m_objectives[i].Result)
				{
				case EObjectiveResult.Failed:
					if (_ignoreProgress)
					{
						return EObjectiveResult.Failed;
					}
					if (objectiveData.OnFailId != -2)
					{
						return EObjectiveResult.Undefined;
					}
					return EObjectiveResult.Failed;
				case EObjectiveResult.Undefined:
					flag = false;
					break;
				}
			}
			if (flag)
			{
				if (_ignoreProgress)
				{
					return EObjectiveResult.Fulfilled;
				}
				if (objectiveData.OnSuccessId != -2)
				{
					return EObjectiveResult.Undefined;
				}
				return EObjectiveResult.Fulfilled;
			}
			return EObjectiveResult.Undefined;
		}

		public void Revive()
		{
			for (int i = 0; i < m_objectives.Count; i++)
			{
				m_objectives[i].PrepareRevive();
			}
		}

		public void RemoveMissionEntries(IList<IMissionBase> _entries)
		{
			for (int i = 0; i < _entries.Count; i++)
			{
				RemoveMissionEntry(_entries[i]);
			}
		}

		public void RemoveMissionEntry(IMissionBase _entry)
		{
			bool flag = MissionEntries.Remove(_entry);
			IObjectiveBase objectiveBase = _entry as IObjectiveBase;
			if (objectiveBase != null)
			{
				MissionObjectives.Remove(objectiveBase);
			}
			_entry.OnDeactivate();
		}

		public IEnumerator Load(Func<float, bool> _onProgressFunc)
		{
			Profiling.Start("MissionPhase::Load", 0f);
			bool playerLoaded = false;
			Stopwatch stopWatch = new Stopwatch();
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			MissionMonoBehaviour[] elements = Root.GetComponentsInChildren<MissionMonoBehaviour>();
			BBList<long> activeObjectives = new BBList<long>();
			bool reloop;
			do
			{
				reloop = false;
				foreach (MissionMonoBehaviour missionElem in elements)
				{
					if (MissionEntries.Contains(missionElem))
					{
						continue;
					}
					MissionBaseData data = missionElem.MissionData;
					if (data.ActivateAtId == -2 || activeObjectives.Contains(data.ActivateAtId))
					{
						MissionEntries.Add(missionElem);
						IObjectiveBase obj = missionElem as IObjectiveBase;
						if (obj != null && (data.ActivateAtId == -2 || activeObjectives.Contains(data.ActivateAtId)))
						{
							MissionObjectives.Add(obj);
							activeObjectives.Add(data.ID);
							reloop = true;
						}
					}
				}
			}
			while (reloop);
			float total = MissionEntries.Count;
			float prog = 0f;
			for (int j = 0; (float)j < total; j++)
			{
				Profiling.Start("MissionPhase Call On Load", float.MaxValue);
				IMissionBase entry = MissionEntries[j];
				entry.OnPreActivate();
				Profiling.Stop();
				bool isPlayer = entry is MissionPlayerSpawn;
				if (isPlayer)
				{
					MissionPlayerSpawn = (MissionPlayerSpawn)entry;
				}
				playerLoaded = playerLoaded || isPlayer;
				prog += 1f;
				if (stopWatch.ElapsedMilliseconds > 200)
				{
					_onProgressFunc(prog / total * 0.5f);
					yield return wait;
					stopWatch.Reset();
					stopWatch.Start();
				}
			}
			for (int i = 0; (float)i < total; i++)
			{
				Profiling.Start("MissionPhase Call On Loaded", float.MaxValue);
				IMissionBase entry2 = MissionEntries[i];
				entry2.OnActivate();
				Profiling.Stop();
				if (stopWatch.ElapsedMilliseconds > 200)
				{
					_onProgressFunc(prog / total * 0.5f + 0.5f);
					yield return wait;
					stopWatch.Reset();
					stopWatch.Start();
				}
			}
			Profiling.StopAndLogBreach();
		}

		public void AddMissionEntries(IList<IMissionBase> _entries)
		{
			MissionEntries.AddRange(_entries);
			for (int i = 0; i < _entries.Count; i++)
			{
				_entries[i].OnPostRead(_entries[i].MissionData);
			}
			for (int j = 0; j < _entries.Count; j++)
			{
				_entries[j].OnReadCompleted();
			}
			for (int k = 0; k < _entries.Count; k++)
			{
				_entries[k].OnPreActivate();
			}
			for (int l = 0; l < _entries.Count; l++)
			{
				_entries[l].OnActivate();
			}
		}

		public void Update()
		{
			m_updateObjectives.AddRange(MissionObjectives);
			int count = m_updateObjectives.Count;
			bool flag = m_lastUpdateTurn != LogicMain.CurrentTurn;
			for (int i = 0; i < count; i++)
			{
				IObjectiveBase objectiveBase = m_updateObjectives[i];
				objectiveBase.UpdateFrame();
				if (flag)
				{
					objectiveBase.UpdateTurn();
				}
			}
			if (flag)
			{
				m_lastUpdateTurn = LogicMain.CurrentTurn;
			}
			m_updateObjectives.Clear();
		}

		public bool NeedsTutorialSystem()
		{
			bool flag = false;
			for (int i = 0; i < m_objectives.Count; i++)
			{
				if (flag)
				{
					break;
				}
				flag = m_objectives[i] is ObjectiveUi;
			}
			return flag;
		}

		public void OnWidgetPress(string _path)
		{
			for (int i = 0; i < m_objectives.Count; i++)
			{
				ObjectiveUi objectiveUi = m_objectives[i] as ObjectiveUi;
				if ((bool)objectiveUi)
				{
					objectiveUi.OnWidgetPress(_path);
				}
			}
		}

		public IEnumerator Progress(IObjectiveBase _fullfilledObjective)
		{
			Stopwatch stopWatch = new Stopwatch();
			long targetId = ((_fullfilledObjective.Result != EObjectiveResult.Failed) ? _fullfilledObjective.ObjectiveData.OnSuccessId : _fullfilledObjective.ObjectiveData.OnFailId);
			if (_fullfilledObjective is ObjectiveStayAlive)
			{
				yield break;
			}
			if (m_waitingObjectives.Remove(_fullfilledObjective))
			{
			}
			if (!_fullfilledObjective.ObjectiveData.IsOptionalObjective && IsFullFilled(true) == EObjectiveResult.Undefined)
			{
				m_waitingObjectives.Add(_fullfilledObjective);
				yield break;
			}
			IObjectiveBase[] objectives = m_waitingObjectives.ToArray();
			m_waitingObjectives.Clear();
			int k;
			for (k = 0; k < objectives.Length; k++)
			{
				IEnumerator ite = Progress(objectives[k]);
				while (ite.MoveNext())
				{
					yield return 0;
				}
			}
			MissionEntries.Remove(_fullfilledObjective);
			MissionObjectives.Remove(_fullfilledObjective);
			_fullfilledObjective.OnDeactivate();
			int c2 = MissionEntries.Count;
			k = 0;
			while (k < c2)
			{
				IMissionBase entry = MissionEntries[k];
				long removeId = entry.MissionData.RemoveAtId;
				if (removeId != -2 && removeId == targetId)
				{
					entry.OnDeactivate();
					MissionEntries.RemoveAt(k);
					IObjectiveBase obj = entry as IObjectiveBase;
					if (obj != null && MissionObjectives.Contains(obj))
					{
						MissionObjectives.Remove(obj);
					}
					c2--;
				}
				else
				{
					k++;
				}
				if (stopWatch.ElapsedMilliseconds > 20)
				{
					yield return new WaitForEndOfFrame();
					stopWatch.Reset();
					stopWatch.Start();
				}
			}
			MissionMonoBehaviour[] elements = Root.GetComponentsInChildren<MissionMonoBehaviour>();
			c2 = elements.Length;
			BBList<long> activateObjectives = new BBList<long>();
			bool reloop;
			do
			{
				reloop = false;
				for (k = 0; k < c2; k++)
				{
					MissionMonoBehaviour missionElem = elements[k];
					if (MissionEntries.Contains(missionElem) || m_newEntries.Contains(missionElem))
					{
						continue;
					}
					MissionBaseData data = missionElem.MissionData;
					if ((data.ActivateAtId != -2 && data.ActivateAtId == targetId) || data.ID == targetId || activateObjectives.Contains(data.ActivateAtId))
					{
						m_newEntries.Add(missionElem);
						MissionEntries.Add(missionElem);
						IObjectiveBase obj2 = missionElem as IObjectiveBase;
						if (obj2 != null)
						{
							MissionObjectives.Add(obj2);
							activateObjectives.Add(data.ID);
							reloop = true;
						}
						missionElem.OnPreActivate();
					}
					if (stopWatch.ElapsedMilliseconds > 20)
					{
						yield return new WaitForEndOfFrame();
						stopWatch.Reset();
						stopWatch.Start();
					}
				}
			}
			while (reloop);
		}

		public void FinalizeProgress()
		{
			for (int i = 0; i < m_newEntries.Count; i++)
			{
				IMissionBase missionBase = m_newEntries[i];
				missionBase.OnActivate();
			}
			m_newEntries.ClearIfNotEmpty();
			PlayerHelper.UpdateVision();
			if (this.OnProgress != null)
			{
				this.OnProgress();
			}
		}

		public void Deactivate()
		{
			int num = 0;
			while (num < MissionObjectives.Count)
			{
				IObjectiveBase objectiveBase = MissionObjectives[num];
				if (objectiveBase.IsActive)
				{
					objectiveBase.OnDeactivate();
					MissionObjectives.RemoveAt(num);
					MissionEntries.Remove(objectiveBase);
				}
				else
				{
					num++;
				}
			}
		}
	}
}
