using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Actions;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Conditionals;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components
{
	public interface IDelegateHolder
	{
		BehaviorAction.DelegateType GetActionDelegate(string _name);

		BehaviorAction.CancelDelegateType GetActionCancelDelegate(string _name);

		BehaviorAction.AttachDelegateType GetActionAttachDelegate(string _name);

		BehaviorActionParameterized.DelegateType GetActionParameterizedDelegate(string _name);

		BehaviorActionParameterized.CancelDelegateType GetActionParameterizedCancelDelegate(string _name);

		BehaviorActionParameterized.AttachDelegateType GetActionParameterizedAttachDelegate(string _name);

		Conditional.DelegateType GetConditionalDelegate(string _name);

		ConditionalParameterized.DelegateType GetConditionalParameterizedDelegate(string _name);

		IParameterContainer CreateActionParameterContainer(int _index);

		IParameterContainer CreateConditionalParameterContainer(int _index);

		IParameterContainer CreateParameterContainer(string _name);

		string ConvertActionIndexToString(int _index);

		string ConvertActionParameterizedIndexToString(int _index);

		string ConvertConditionalIndexToString(int _index);

		string ConvertConditionalParameterizedIndexToString(int _index);

		int ConvertActionStringToIndex(string _name);

		int ConvertActionParameterizedStringToIndex(string _name);

		int ConvertConditionalStringToIndex(string _name);

		int ConvertConditionalParameterizedStringToIndex(string _name);

		string[] GetParameterTypeNames();

		string[] GetDelegateNames(EBehaviorType _behaviorType);

		string[] GetParameterNames(EBehaviorType _behaviorType);

		BehaviorAction.AttachDelegateType GetDefaultAttachDelegate();
	}
}
