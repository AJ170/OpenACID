using UbiCore.Messenger;
using UnityEngine;

namespace Assets.Scripts.Mission.UI
{
	public interface IUiSubSystem : IUISubSystemLayout
	{
		bool IsActive { get; }

		bool InitiallyVisible { get; }

		void UpdateFrame(float _deltaTime);

		bool Tap(TapEvent _event);

		void OnMissionLoad();

		void OnMissionLoaded();

		void OnMissionUnload();

		void OnMissionUnloaded();

		bool IsOnUi(Vector2 _mousePosition);

		void OnSceneLoaded();

		void OnSceneUnload();

		void OnCameraCreated();

		void OnSharedUILoaded();

		void OnMissionEnded();

		void OnMissionOutroStarted();

		void OnUIAlignmentChanged(bool _isRightHanded);

		void OnUIReadyToUse();

		void SetVisible(bool _value);
	}
}
