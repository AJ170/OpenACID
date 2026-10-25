using System.Xml;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Composites;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components
{
	public abstract class BehaviorComponent
	{
		public string Name { get; set; }

		public AbstractComposite Parent { get; set; }

		public BehaviorTree Tree { get; set; }

		public EBehaviorType BehaviorType { get; protected set; }

		public virtual BBList<BehaviorComponent> Behaviors
		{
			get
			{
				return null;
			}
		}

		protected BehaviorComponent()
		{
			Name = "BehaviorComponent";
			BehaviorType = EBehaviorType.Invalid;
		}

		public IBehaviorRuntimeData Behave(ITreeRuntimeData _runtimeData)
		{
			IBehaviorRuntimeData behaviorRuntimeData = _runtimeData.RequestBehaviorRuntimeData(this);
			Tree.CurrentTicked.Add(this);
			BehaveInternal(_runtimeData, behaviorRuntimeData);
			return behaviorRuntimeData;
		}

		protected abstract void BehaveInternal(ITreeRuntimeData _runtimeData, IBehaviorRuntimeData _behaviorRuntimeData);

		public void MoveBehavior(bool _up)
		{
			Parent.MoveBehavior(this, _up);
		}

		public bool RemoveFromHierachy()
		{
			return Parent.RemoveBehavior(this);
		}

		public override string ToString()
		{
			return string.Format("{0}:{1}", GetType().Name, Name);
		}

		public virtual void Read(XmlReader _reader)
		{
			Name = _reader.GetAttribute("Name");
		}
	}
}
