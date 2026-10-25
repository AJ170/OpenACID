using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.UI;
using UbiCore.Messenger;
using UbiCore.Utils;

namespace UbiCore.Acier
{
	public class NpcInteractionInterpreter : IconInterpreter
	{
		public override bool OnTap(TapEvent _event)
		{
			bool flag = base.OnTap(_event);
			if (!flag)
			{
				flag = EvaluateGetOutOfNPCInteractionTap();
			}
			return flag;
		}

		public override IconMaterial GetDefaultActionIcon()
		{
			IconMaterial defaultActionIcon = base.GetDefaultActionIcon();
			if (defaultActionIcon != 0)
			{
				return defaultActionIcon;
			}
			return IconMaterial.Invalid;
		}

		public override bool OnTriggerDefaultAction()
		{
			if (base.OnTriggerDefaultAction())
			{
				return true;
			}
			return false;
		}

		public override bool OnDrag(DragEvent _event)
		{
			if (base.OnDrag(_event))
			{
				return true;
			}
			return EvaluateGetOutOfNPCInteractionTap();
		}

		private bool EvaluateGetOutOfNPCInteractionTap()
		{
			EntityId entityId = PlayerHelper.PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_NpcInteraction).EntityId;
			UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.CharacterInteraction_RejectOffer(entityId);
			return true;
		}
	}
}
