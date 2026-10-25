using UbiCore.Messenger;

namespace Assets.Scripts.Mission.UI
{
	public class SkillEvent : UbiCore.Messenger.SkillEvent, ILogicUiEvent, IMessage
	{
		public SkillEvent(UbiCore.Messenger.SkillEvent _event)
			: base(_event.EntityId, _event.SubType, _event.SkillType)
		{
		}
	}
}
