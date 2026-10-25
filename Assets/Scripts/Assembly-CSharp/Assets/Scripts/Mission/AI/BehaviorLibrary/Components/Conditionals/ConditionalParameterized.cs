using System.Xml;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Conditionals
{
	public class ConditionalParameterized : BehaviorComponent, IHasDelegate, IHasDelegateParameterized
	{
		public delegate bool DelegateType(IParameterContainer _container);

		private int m_delegateEnumIndex;

		public string DelegateEnumEntry
		{
			get
			{
				return base.Tree.DelegateHolder.ConvertConditionalParameterizedIndexToString(DelegateEnumIndex);
			}
			private set
			{
				DelegateEnumIndex = base.Tree.DelegateHolder.ConvertConditionalParameterizedStringToIndex(value);
			}
		}

		public int DelegateEnumIndex
		{
			get
			{
				return m_delegateEnumIndex;
			}
			set
			{
				m_delegateEnumIndex = value;
				string name = base.Tree.DelegateHolder.ConvertConditionalParameterizedIndexToString(value);
				Delegate = base.Tree.DelegateHolder.GetConditionalParameterizedDelegate(name);
			}
		}

		public IParameterContainer ParameterContainer { get; set; }

		public DelegateType Delegate { get; private set; }

		public ConditionalParameterized()
		{
			base.Name = "ConditionalParameterized";
			base.BehaviorType = EBehaviorType.ConditionalParameterized;
		}

		protected override void BehaveInternal(ITreeRuntimeData _runtimeData, IBehaviorRuntimeData _behaviorRuntimeData)
		{
			bool flag = Delegate(ParameterContainer);
			_behaviorRuntimeData.ReturnCode = ((!flag) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Success);
		}

		public override string ToString()
		{
			if (Delegate != null)
			{
				return string.Format("{0}:{1}:{2}", GetType().Name, base.Name, DelegateEnumEntry);
			}
			return string.Format("{0}:{1}:Delegate Invalid", GetType().Name, base.Name);
		}

		public override void Read(XmlReader _reader)
		{
			base.Read(_reader);
			string attribute = _reader.GetAttribute("Delegate");
			DelegateEnumEntry = attribute;
			ParameterContainer = base.Tree.DelegateHolder.CreateConditionalParameterContainer(DelegateEnumIndex);
			ParameterContainer.Read(_reader);
		}
	}
}
