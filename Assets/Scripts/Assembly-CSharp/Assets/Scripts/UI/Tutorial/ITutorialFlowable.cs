using AcierProtocol;

namespace Assets.Scripts.UI.Tutorial
{
	public interface ITutorialFlowable
	{
		ETutorialCategory Category { get; }

		STutorialFlowId Index { get; }

		string Name { get; }

		bool IsActive { get; }

		void Activate();

		void Deactivate();

		bool ConditionAllowShow();
	}
}
