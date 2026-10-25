using System;
using System.Xml;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Composites;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Decorators
{
	public class Enforcer : AbstractComposite
	{
		public BehaviorReturnCode ForcedReturnCode { get; set; }

		public Enforcer()
		{
			base.Name = "Enforcer";
			base.BehaviorType = EBehaviorType.Enforcer;
		}

		protected override void BehaveInternal(ITreeRuntimeData _runtimeData, IBehaviorRuntimeData _behaviorRuntimeData)
		{
			foreach (BehaviorComponent behavior in m_behaviors)
			{
				behavior.Behave(_runtimeData);
			}
			_behaviorRuntimeData.ReturnCode = ForcedReturnCode;
		}

		public override void Read(XmlReader _reader)
		{
			ForcedReturnCode = (BehaviorReturnCode)(int)Enum.Parse(typeof(BehaviorReturnCode), _reader.GetAttribute("ForcedReturnCode"));
			base.Read(_reader);
		}
	}
}
