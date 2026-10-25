using System.Xml;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Actions
{
	public class BehaviorAction : AbstractBehaviorAction
	{
		public delegate BehaviorReturnCode DelegateType(BehaviorReturnCode _lastReturnCode);

		public delegate void CancelDelegateType();

		public delegate void AttachDelegateType();

		private int m_delegateEnumIndex;

		public override string DelegateEnumEntry
		{
			get
			{
				return base.Tree.DelegateHolder.ConvertActionIndexToString(DelegateEnumIndex);
			}
			protected set
			{
				DelegateEnumIndex = base.Tree.DelegateHolder.ConvertActionStringToIndex(value);
			}
		}

		public override int DelegateEnumIndex
		{
			get
			{
				return m_delegateEnumIndex;
			}
			set
			{
				m_delegateEnumIndex = value;
				string name = base.Tree.DelegateHolder.ConvertActionIndexToString(value);
				Delegate = base.Tree.DelegateHolder.GetActionDelegate(name);
				CancelDelegate = base.Tree.DelegateHolder.GetActionCancelDelegate(name);
				AttachDelegate = base.Tree.DelegateHolder.GetActionAttachDelegate(name);
			}
		}

		public DelegateType Delegate { get; private set; }

		public CancelDelegateType CancelDelegate { get; protected set; }

		public AttachDelegateType AttachDelegate { get; protected set; }

		public BehaviorAction()
		{
			base.Name = "BehaviorAction";
			base.BehaviorType = EBehaviorType.BehaviorAction;
		}

		protected override void BehaveInternal(ITreeRuntimeData _runtimeData, IBehaviorRuntimeData _behaviorRuntimeData)
		{
			_behaviorRuntimeData.ReturnCode = Delegate(_behaviorRuntimeData.ReturnCode);
		}

		public override void Cancel(IBehaviorRuntimeData _runtimeData)
		{
			base.Cancel(_runtimeData);
			if (CancelDelegate != null)
			{
				CancelDelegate();
			}
		}

		public override void Attach()
		{
			if (AttachDelegate != null)
			{
				AttachDelegate();
			}
		}

		public override string ToString()
		{
			return string.Format("{0}:{1}:{2}", GetType().Name, base.Name, DelegateEnumEntry);
		}

		public override void Read(XmlReader _reader)
		{
			base.Read(_reader);
			string attribute = _reader.GetAttribute("Delegate");
			DelegateEnumEntry = attribute;
			if (base.Tree.PreStartAction == null)
			{
				int preStartActionIndex = base.Tree.PreStartActionIndex;
				if (preStartActionIndex > 0 && preStartActionIndex == DelegateEnumIndex)
				{
					base.Tree.PreStartAction = this;
				}
			}
		}
	}
}
