using System.Xml;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Actions
{
	public class BehaviorActionParameterized : AbstractBehaviorAction, IHasDelegate, IHasDelegateParameterized
	{
		public delegate BehaviorReturnCode DelegateType(IParameterContainer _container, BehaviorReturnCode _lastReturnCode);

		public delegate void CancelDelegateType(IParameterContainer _container);

		public delegate void AttachDelegateType(IParameterContainer _container);

		private int m_delegateEnumIndex;

		public IParameterContainer ParameterContainer { get; set; }

		public DelegateType Delegate { get; private set; }

		public CancelDelegateType CancelDelegate { get; protected set; }

		public AttachDelegateType AttachDelegate { get; protected set; }

		public override string DelegateEnumEntry
		{
			get
			{
				return base.Tree.DelegateHolder.ConvertActionParameterizedIndexToString(DelegateEnumIndex);
			}
			protected set
			{
				DelegateEnumIndex = base.Tree.DelegateHolder.ConvertActionParameterizedStringToIndex(value);
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
				string name = base.Tree.DelegateHolder.ConvertActionParameterizedIndexToString(value);
				Delegate = base.Tree.DelegateHolder.GetActionParameterizedDelegate(name);
				CancelDelegate = base.Tree.DelegateHolder.GetActionParameterizedCancelDelegate(name);
				AttachDelegate = base.Tree.DelegateHolder.GetActionParameterizedAttachDelegate(name);
			}
		}

		public BehaviorActionParameterized()
		{
			base.Name = "BehaviorActionParameterized";
			base.BehaviorType = EBehaviorType.BehaviorActionParameterized;
		}

		protected override void BehaveInternal(ITreeRuntimeData _runtimeData, IBehaviorRuntimeData _behaviorRuntimeData)
		{
			_behaviorRuntimeData.ReturnCode = Delegate(ParameterContainer, _behaviorRuntimeData.ReturnCode);
		}

		public override void Cancel(IBehaviorRuntimeData _runtimeData)
		{
			base.Cancel(_runtimeData);
			if (CancelDelegate != null)
			{
				CancelDelegate(ParameterContainer);
			}
		}

		public override void Attach()
		{
			if (AttachDelegate != null)
			{
				AttachDelegate(ParameterContainer);
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
			ParameterContainer = base.Tree.DelegateHolder.CreateActionParameterContainer(DelegateEnumIndex);
			using (XmlReader xmlReader = _reader.ReadSubtree())
			{
				ParameterContainer.Read(xmlReader);
				xmlReader.Close();
			}
		}
	}
}
