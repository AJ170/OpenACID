using System.Xml;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Composites
{
	public abstract class AbstractComposite : BehaviorComponent
	{
		protected BBList<BehaviorComponent> m_behaviors;

		public int MaxChildCount { get; protected set; }

		public bool Full
		{
			get
			{
				return m_behaviors.Count >= MaxChildCount;
			}
		}

		public override BBList<BehaviorComponent> Behaviors
		{
			get
			{
				return m_behaviors;
			}
		}

		protected AbstractComposite()
		{
			MaxChildCount = int.MaxValue;
			m_behaviors = new BBList<BehaviorComponent>();
		}

		public void Cancel(IBehaviorRuntimeData _runtimeData)
		{
			_runtimeData.Reset();
		}

		public void AddBehavior(BehaviorComponent _behavior, bool _inFront)
		{
			if (_inFront)
			{
				m_behaviors.Insert(0, _behavior);
				_behavior.Parent = this;
			}
			else
			{
				m_behaviors.Add(_behavior);
				_behavior.Parent = this;
			}
		}

		public void AddBehavior(BehaviorComponent _behavior)
		{
			AddBehavior(_behavior, false);
		}

		public bool InsertBehavior(BehaviorComponent _behaviorInList, BehaviorComponent _newBehavior, bool _before)
		{
			return InsertBehaviorIgnoreFull(_behaviorInList, _newBehavior, _before);
		}

		public bool Substitute(BehaviorComponent _behaviorInList, BehaviorComponent _newBehavior)
		{
			if (!InsertBehaviorIgnoreFull(_behaviorInList, _newBehavior, true))
			{
				return false;
			}
			return RemoveBehavior(_behaviorInList);
		}

		public bool RemoveBehavior(BehaviorComponent _behavior)
		{
			if (m_behaviors.Remove(_behavior))
			{
				_behavior.Parent = null;
				return true;
			}
			return false;
		}

		public void MoveBehavior(BehaviorComponent _behavior, bool _up)
		{
			int num = m_behaviors.IndexOf(_behavior);
			if (num < 0)
			{
				return;
			}
			int index = num;
			num = ((!_up) ? (num + 1) : (num - 1));
			if (num < 0 || num >= m_behaviors.Count)
			{
				if (base.Parent != null)
				{
					_behavior.Parent = null;
					if (base.Parent.InsertBehavior(this, _behavior, _up))
					{
						m_behaviors.RemoveAt(index);
					}
					else
					{
						_behavior.Parent = this;
					}
				}
				return;
			}
			AbstractComposite abstractComposite = m_behaviors[num] as AbstractComposite;
			if (abstractComposite != null && !abstractComposite.Full)
			{
				bool inFront = !_up;
				_behavior.Parent = null;
				abstractComposite.AddBehavior(_behavior, inFront);
				m_behaviors.RemoveAt(index);
			}
			else
			{
				m_behaviors.RemoveAt(index);
				m_behaviors.Insert(num, _behavior);
			}
		}

		public override void Read(XmlReader _reader)
		{
			base.Read(_reader);
			while (_reader.Read())
			{
				if (_reader.NodeType != XmlNodeType.Element || !(_reader.Name == "Children"))
				{
					continue;
				}
				int depth = _reader.Depth;
				while (_reader.Read() && _reader.Depth >= depth)
				{
					if (_reader.NodeType == XmlNodeType.Element && _reader.Name == "Behavior")
					{
						BehaviorComponent behavior = base.Tree.ReadBehavior(_reader);
						AddBehavior(behavior);
					}
				}
				break;
			}
		}

		private bool InsertBehaviorIgnoreFull(BehaviorComponent _behaviorInList, BehaviorComponent _newBehavior, bool _before)
		{
			int num = m_behaviors.IndexOf(_behaviorInList);
			if (!_before)
			{
				num++;
			}
			m_behaviors.Insert(num, _newBehavior);
			_newBehavior.Parent = this;
			return true;
		}
	}
}
