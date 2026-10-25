using System;
using System.IO;
using System.Xml;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Actions;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Composites;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Conditionals;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Decorators;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UnityEngine;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components
{
	public class BehaviorTree
	{
		private bool m_inititializedHandled;

		public AbstractComposite TreeRoot { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public IDelegateHolder DelegateHolder { get; private set; }

		public BBList<BehaviorComponent> CurrentTicked { get; set; }

		internal int PreStartActionIndex { get; private set; }

		internal BehaviorAction PreStartAction { get; set; }

		public void Init(IDelegateHolder _delegateHolder, TextAsset _xmlFile = null, int _preStartActionIndex = -1)
		{
			DelegateHolder = _delegateHolder;
			PreStartActionIndex = _preStartActionIndex;
			if (_xmlFile != null)
			{
				Read(_xmlFile);
			}
			else
			{
				TreeRoot = CreateBehavior(EBehaviorType.Selector) as AbstractComposite;
				TreeRoot.Name = "Root";
			}
			CurrentTicked = new BBList<BehaviorComponent>();
		}

		public virtual void OnAttachedAndInitialized(ITreeRuntimeData _runtimeData)
		{
			if (!m_inititializedHandled)
			{
				Behave(_runtimeData);
				m_inititializedHandled = true;
			}
			CallAttachOnRunningActions(_runtimeData);
		}

		public void Behave(ITreeRuntimeData _runtimeData, BehaviorComponent _behaviorComponent = null)
		{
			_behaviorComponent = _behaviorComponent ?? TreeRoot;
			Profiling.Start("BehaviorTree::Behave()", float.MaxValue);
			Profiling.Start("TreeRoot::Behave()", float.MaxValue);
			_behaviorComponent.Behave(_runtimeData);
			Profiling.Stop();
			Profiling.Start("Cancel behaviors", float.MaxValue);
			_runtimeData.LastTickedSet.Clear();
			int count = CurrentTicked.Count;
			for (int i = 0; i < count; i++)
			{
				_runtimeData.LastTickedSet.Add(CurrentTicked[i]);
			}
			CancelNoLongerRunning(_runtimeData);
			BBList<BehaviorComponent> lastTicked = _runtimeData.LastTicked;
			_runtimeData.LastTicked = CurrentTicked;
			CurrentTicked = lastTicked;
			CurrentTicked.Clear();
			Profiling.Stop();
			Profiling.Stop();
		}

		public void CancelRunning(ITreeRuntimeData _runtimeData)
		{
			_runtimeData.LastTickedSet.Clear();
			CancelNoLongerRunning(_runtimeData);
			_runtimeData.LastTicked.Clear();
		}

		public static Type GetDelegateReturnTypeFromBehavior(Type _behaviorType)
		{
			if (typeof(BehaviorAction).IsAssignableFrom(_behaviorType))
			{
				return typeof(BehaviorReturnCode);
			}
			if (typeof(Conditional).IsAssignableFrom(_behaviorType))
			{
				return typeof(bool);
			}
			if (typeof(ConditionalParameterized).IsAssignableFrom(_behaviorType))
			{
				return typeof(bool);
			}
			if (typeof(BehaviorActionParameterized).IsAssignableFrom(_behaviorType))
			{
				return typeof(BehaviorReturnCode);
			}
			return null;
		}

		public BehaviorComponent CreateBehavior(EBehaviorType _behaviorType)
		{
			BehaviorComponent behaviorComponent = null;
			switch (_behaviorType)
			{
			case EBehaviorType.BehaviorAction:
				behaviorComponent = new BehaviorAction();
				break;
			case EBehaviorType.BehaviorActionParameterized:
				behaviorComponent = new BehaviorActionParameterized();
				break;
			case EBehaviorType.ParallelSelector:
				behaviorComponent = new ParallelSelector();
				break;
			case EBehaviorType.ParallelSequence:
				behaviorComponent = new ParallelSequence();
				break;
			case EBehaviorType.RandomSelector:
				behaviorComponent = new RandomSelector();
				break;
			case EBehaviorType.Selector:
				behaviorComponent = new Selector();
				break;
			case EBehaviorType.Sequence:
				behaviorComponent = new Sequence();
				break;
			case EBehaviorType.Conditional:
				behaviorComponent = new Conditional();
				break;
			case EBehaviorType.ConditionalParameterized:
				behaviorComponent = new ConditionalParameterized();
				break;
			case EBehaviorType.Counter:
				behaviorComponent = new Counter();
				break;
			case EBehaviorType.Inverter:
				behaviorComponent = new Inverter();
				break;
			case EBehaviorType.Timer:
				behaviorComponent = new Timer();
				break;
			case EBehaviorType.Enforcer:
				behaviorComponent = new Enforcer();
				break;
			case EBehaviorType.Branch:
				behaviorComponent = new Branch();
				break;
			case EBehaviorType.Wait:
				behaviorComponent = new Wait();
				break;
			case EBehaviorType.Prolongator:
				behaviorComponent = new Prolongator();
				break;
			}
			behaviorComponent.Tree = this;
			return behaviorComponent;
		}

		private void Read(TextAsset _xmlFile)
		{
			Profiling.Start("BehaviorTree::Read()", float.MaxValue);
			using (StringReader reader = new StringReader(_xmlFile.text))
			{
				using (XmlReader xmlReader = XmlReader.Create(reader))
				{
					while (xmlReader.Read())
					{
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Behaviors")
						{
							Title = xmlReader.GetAttribute("Title");
							if (Title == string.Empty)
							{
								Title = _xmlFile.name;
							}
							Description = xmlReader.GetAttribute("Description");
							if (Description == string.Empty)
							{
								Description = "Enter description...";
							}
							while (xmlReader.Read() && (xmlReader.NodeType != XmlNodeType.Element || !(xmlReader.Name == "Behavior")))
							{
							}
							if (Assets.Scripts.Engine.Helpers.Verify(!xmlReader.EOF, "Invalid Behaviourtree {0} - no Behavior node found!", _xmlFile.name))
							{
								TreeRoot = ReadBehavior(xmlReader) as AbstractComposite;
							}
							break;
						}
					}
					xmlReader.Close();
				}
			}
			Profiling.Stop();
		}

		public BehaviorComponent ReadBehavior(XmlReader _reader)
		{
			EBehaviorType behaviorType = (EBehaviorType)(int)Enum.Parse(typeof(EBehaviorType), _reader.GetAttribute("BehaviorType"));
			BehaviorComponent behaviorComponent = CreateBehavior(behaviorType);
			behaviorComponent.Read(_reader);
			return behaviorComponent;
		}

		private void CallAttachOnRunningActions(ITreeRuntimeData _runtimeData)
		{
			int count = _runtimeData.LastTicked.Count;
			if (count == 0)
			{
				Behave(_runtimeData);
				count = _runtimeData.LastTicked.Count;
			}
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < count; i++)
			{
				BehaviorComponent behaviorComponent = _runtimeData.LastTicked[i];
				IHasAttachDelegate hasAttachDelegate = behaviorComponent as IHasAttachDelegate;
				if (hasAttachDelegate != null)
				{
					flag = true;
					if (_runtimeData.RequestBehaviorRuntimeData(behaviorComponent).ReturnCode == BehaviorReturnCode.Running)
					{
						hasAttachDelegate.Attach();
						flag2 = true;
					}
				}
			}
			if (flag && !flag2)
			{
				DelegateHolder.GetDefaultAttachDelegate()();
			}
		}

		private void CancelNoLongerRunning(ITreeRuntimeData _runtimeData)
		{
			int count = _runtimeData.LastTicked.Count;
			for (int i = 0; i < count; i++)
			{
				BehaviorComponent behaviorComponent = _runtimeData.LastTicked[i];
				if (!_runtimeData.LastTickedSet.Contains(behaviorComponent))
				{
					IBehaviorRuntimeData behaviorRuntimeData = _runtimeData.RequestBehaviorRuntimeData(behaviorComponent);
					IHasCancelDelegate hasCancelDelegate = behaviorComponent as IHasCancelDelegate;
					if (hasCancelDelegate != null && behaviorRuntimeData.ReturnCode == BehaviorReturnCode.Running)
					{
						hasCancelDelegate.Cancel(behaviorRuntimeData);
					}
					AbstractComposite abstractComposite = behaviorComponent as AbstractComposite;
					if (abstractComposite != null)
					{
						abstractComposite.Cancel(behaviorRuntimeData);
					}
				}
			}
		}
	}
}
