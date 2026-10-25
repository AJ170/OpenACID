using System.Xml;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Conditionals
{
	public class Conditional : BehaviorComponent, IHasDelegate
	{
		public delegate bool DelegateType();

		private int m_delegateEnumIndex;

		public string DelegateEnumEntry
		{
			get
			{
				return base.Tree.DelegateHolder.ConvertConditionalIndexToString(DelegateEnumIndex);
			}
			private set
			{
				DelegateEnumIndex = base.Tree.DelegateHolder.ConvertConditionalStringToIndex(value);
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
				string name = base.Tree.DelegateHolder.ConvertConditionalIndexToString(value);
				Delegate = base.Tree.DelegateHolder.GetConditionalDelegate(name);
			}
		}

		public DelegateType Delegate { get; private set; }

		public Conditional()
		{
			base.Name = "Conditional";
			base.BehaviorType = EBehaviorType.Conditional;
		}

		protected override void BehaveInternal(ITreeRuntimeData _runtimeData, IBehaviorRuntimeData _behaviorRuntimeData)
		{
			bool flag = Delegate();
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
		}
	}
}
