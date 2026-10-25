using UbiCore.Messenger;

namespace UbiCore.Common
{
	public interface IInterpreter
	{
		bool InputEnabled { get; set; }

		float InputPriority { get; set; }

		bool OnTouch(TouchEvent _event);

		bool OnTap(TapEvent _event);

		bool OnDrag(DragEvent _event);

		bool OnSelect(SelectEvent _event);

		IconMaterial GetDefaultActionIcon();

		bool OnTriggerDefaultAction();

		bool OnPrepareDefaultAction();

		bool OnCancelDefaultAction();

		bool OnSecondTouch(TouchEvent _event);

		bool OnSecondTap(TapEvent _event);

		bool OnSecondDrag(DragEvent _event);

		bool OnSecondSelect(SelectEvent _event);

		bool OnSwipe(SwipeEvent _event);

		bool OnTwist(TwistEvent _event);

		bool OnPinch(PinchEvent _event);

		bool OnDoubleTap(TapEvent _event);

		void OnUpdate();
	}
}
