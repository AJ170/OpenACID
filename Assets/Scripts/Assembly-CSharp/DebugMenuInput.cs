using System.Text;
using Assets.Scripts.Mission.Motion;
using Assets.Scripts.Mission.UI;
using InControl;
using UbiCore.Acier;
using UbiCore.Debug.UI;
using UnityEngine;

public class DebugMenuInput : DebugMenuItem
{
	private bool m_showGamepadInput;

	public override string GetName()
	{
		return "Input";
	}

	private void OnGUI()
	{
		GUI.depth = -1;
		OnRenderShowGamepadInputStatus();
	}

	public override void OnMenu()
	{
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		if (GUILayout.Button("Snap to NavMesh"))
		{
			NavMeshAgent navMeshAgent = AcierCameraController.NavMeshAgent;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(navMeshAgent.transform.position, out hit, 10f, navMeshAgent.areaMask))
			{
				navMeshAgent.transform.position = hit.position;
			}
			else
			{
				Debug.LogError("Could not snap to NavMesh: Not found");
			}
		}
		GUILayout.Label(string.Format("Active input interpreter: {0}", AcierInputHandler.Instance.ActiveInterpreter));
		if (GUILayout.Button(string.Format("Holding tap: {0}", (!AcierInputHandler.Instance.EnableStick) ? "Point to ground" : "Virtual stick")))
		{
			AcierInputHandler.Instance.EnableStick = !AcierInputHandler.Instance.EnableStick;
			AcierInputHandler.Instance.EnableSelect = !AcierInputHandler.Instance.EnableSelect;
		}
		if (GUILayout.Button(string.Format("Camera control: {0}", (!AcierInputHandler.Instance.EnableSecondTouchArea) ? "Two fingers" : "Screen area")))
		{
			AcierInputHandler.Instance.EnableSecondTouchArea = !AcierInputHandler.Instance.EnableSecondTouchArea;
			CameraEvents.StopMovementWhileDragging = !AcierInputHandler.Instance.EnableSecondTouchArea;
		}
		if (AcierInputHandler.Instance.EnableSecondTouchArea)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("x");
			float.TryParse(GUILayout.TextField(AcierInputHandler.Instance.SecondTouchMinX.ToString("F2")), out AcierInputHandler.Instance.SecondTouchMinX);
			float.TryParse(GUILayout.TextField(AcierInputHandler.Instance.SecondTouchMaxX.ToString("F2")), out AcierInputHandler.Instance.SecondTouchMaxX);
			if (GUILayout.Button("Mirror"))
			{
				AcierInputHandler.Instance.SecondTouchMinX *= -1f;
				AcierInputHandler.Instance.SecondTouchMaxX *= -1f;
			}
			GUILayout.Label("y");
			float.TryParse(GUILayout.TextField(AcierInputHandler.Instance.SecondTouchMinY.ToString("F2")), out AcierInputHandler.Instance.SecondTouchMinY);
			float.TryParse(GUILayout.TextField(AcierInputHandler.Instance.SecondTouchMaxY.ToString("F2")), out AcierInputHandler.Instance.SecondTouchMaxY);
			GUILayout.EndHorizontal();
		}
		Toggle(ref AcierInputHandler.Instance.DisableSecondTouchAreaInCombat, "Disable camera area while in combat");
		Toggle(ref AcierInputHandler.Instance.EnableGamepadInAnimus, "Enable gamepad in Animus");
		Toggle(ref m_showGamepadInput, "Show GamePad input Status");
		Toggle(ref AcierInputHandler.Instance.EnableTouchFeedback, "Display touch feedback");
		Toggle(ref AcierInputHandler.Instance.ShowParcourFeedback, "Display parcour helper");
		Toggle(ref AcierInputHandler.Instance.ShowPlayerSpeed, "Display player speed");
		Toggle(ref ParcourHelper.Debugging, "Display parcour debug");
		MoveInterpreter.DisplayDebug = ParcourHelper.Debugging;
		AnimationController.Debugging = ParcourHelper.Debugging;
		Toggle(ref AnimationController.WalkOnCollider, "Place agent on collider (instead of NavMesh)");
		Toggle(ref AnimatorIK.PlaceOnCollider, "Place feet on collider (instead of NavMesh)");
		GUILayout.Label(string.Format("Height offset: {0}m", AnimationController.GroundHeightOffset.ToString("F2")));
		AnimationController.GroundHeightOffset = GUILayout.HorizontalSlider(AnimationController.GroundHeightOffset, -0.5f, 0.5f);
		GUILayout.Space(10f);
		OnMenuVirtualStick();
		GUILayout.Space(10f);
		OnMenuTapAndHold();
		GUILayout.Space(10f);
		OnMenuDoubleTap();
		GUILayout.EndVertical();
		GUILayout.BeginVertical();
		OnMenuRoaming();
		GUILayout.Space(10f);
		OnMenuCombat();
		GUILayout.Space(10f);
		OnMenuAcrobatics();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	private void OnMenuRoaming()
	{
		GUILayout.BeginVertical("box");
		GUILayout.Label("Roaming");
		Toggle(ref MoveInterpreter.StickyInput, "Sticky input");
		GUILayout.Label(string.Format("Move distance: {0}m", MoveInterpreter.DragMoveDistance.ToString("F2")));
		MoveInterpreter.DragMoveDistance = GUILayout.HorizontalSlider(MoveInterpreter.DragMoveDistance, 0.1f, 2f);
		Toggle(ref MoveInterpreter.AutoSelectTarget, "Auto select targets");
		if (MoveInterpreter.AutoSelectTarget)
		{
			GUILayout.Label(string.Format("Radius: {0}m", MoveInterpreter.AutoSelectRadius.ToString("F2")));
			MoveInterpreter.AutoSelectRadius = GUILayout.HorizontalSlider(MoveInterpreter.AutoSelectRadius, 0f, 25f);
			GUILayout.Label(string.Format("Screen margin: {0}% of screen shortest side", MoveInterpreter.AutoSelectMinScreenMargin.ToString("F2")));
			MoveInterpreter.AutoSelectMinScreenMargin = GUILayout.HorizontalSlider(MoveInterpreter.AutoSelectMinScreenMargin, 0f, 1f);
		}
		Toggle(ref MoveInterpreter.VariableMoveSpeed, "Variable movement speed");
		if (AcierInputHandler.Instance.EnableSelect)
		{
			GUILayout.Label(string.Format("Select min distance: {0}m", MoveInterpreter.SelectMinDistance.ToString("F2")));
			MoveInterpreter.SelectMinDistance = GUILayout.HorizontalSlider(MoveInterpreter.SelectMinDistance, 0f, 5f);
			GUILayout.Label(string.Format("Select max distance: {0}m", MoveInterpreter.SelectMaxDistance.ToString("F2")));
			MoveInterpreter.SelectMaxDistance = GUILayout.HorizontalSlider(MoveInterpreter.SelectMaxDistance, 0f, 5f);
		}
		GUILayout.Label("Input direction alignment");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Always"))
		{
			MoveInterpreter.InputCorrectionBase = 2f;
			MoveInterpreter.InputCorrectionDragMultiplier = 0f;
			MoveInterpreter.InputCorrectionDamping = 0.25f;
		}
		if (GUILayout.Button("When moving stick"))
		{
			MoveInterpreter.InputCorrectionBase = 0f;
			MoveInterpreter.InputCorrectionDragMultiplier = 80f;
			MoveInterpreter.InputCorrectionDamping = 0.25f;
		}
		if (GUILayout.Button("Mix"))
		{
			MoveInterpreter.InputCorrectionBase = 0.2f;
			MoveInterpreter.InputCorrectionDragMultiplier = 80f;
			MoveInterpreter.InputCorrectionDamping = 0.5f;
		}
		GUILayout.EndHorizontal();
		GUILayout.Label(string.Format("Input realign base: {0}", MoveInterpreter.InputCorrectionBase.ToString("F2")));
		MoveInterpreter.InputCorrectionBase = GUILayout.HorizontalSlider(MoveInterpreter.InputCorrectionBase, 0f, 1f);
		GUILayout.Label(string.Format("Input realign motion sensitivity: {0}", (MoveInterpreter.InputCorrectionDragMultiplier * 0.01f).ToString("F2")));
		MoveInterpreter.InputCorrectionDragMultiplier = GUILayout.HorizontalSlider(MoveInterpreter.InputCorrectionDragMultiplier, 0f, 100f);
		GUILayout.Label(string.Format("Input realign damping: {0}", MoveInterpreter.InputCorrectionDamping.ToString("F2")));
		MoveInterpreter.InputCorrectionDamping = GUILayout.HorizontalSlider(MoveInterpreter.InputCorrectionDamping, 0f, 1f);
		GUILayout.Label(string.Format("Rotation damping (all units): {0}", AnimationStateFollowPath.FaceDirectionDamping.ToString("F2")));
		AnimationStateFollowPath.FaceDirectionDamping = GUILayout.HorizontalSlider(AnimationStateFollowPath.FaceDirectionDamping, 0f, 1f);
		GUILayout.EndVertical();
	}

	private void OnMenuCombat()
	{
		GUILayout.BeginVertical("box");
		GUILayout.Label("Combat");
		GUILayout.Label(string.Format("Move distance: {0}m", CombatInterpreter.MoveDistance.ToString("F2")));
		CombatInterpreter.MoveDistance = GUILayout.HorizontalSlider(CombatInterpreter.MoveDistance, 0.1f, 2f);
		if (AcierInputHandler.Instance.EnableSelect)
		{
			GUILayout.Label(string.Format("Select min distance: {0}m", CombatInterpreter.SelectMinDistance.ToString("F2")));
			CombatInterpreter.SelectMinDistance = GUILayout.HorizontalSlider(CombatInterpreter.SelectMinDistance, 0f, 5f);
			GUILayout.Label(string.Format("Select max distance: {0}m", CombatInterpreter.SelectMaxDistance.ToString("F2")));
			CombatInterpreter.SelectMaxDistance = GUILayout.HorizontalSlider(CombatInterpreter.SelectMaxDistance, 0f, 5f);
		}
		GUILayout.EndVertical();
	}

	private void OnMenuVirtualStick()
	{
		GUILayout.BeginVertical("box");
		AcierInputHandler instance = AcierInputHandler.Instance;
		Toggle(ref instance.EnableStick, "Enable virtual stick");
		if (instance.EnableStick)
		{
			GUILayout.Label(string.Format("Deadzone: {0}% of screen shortest side", (instance.StickMinDistance * 100f).ToString("F2")));
			instance.StickMinDistance = GUILayout.HorizontalSlider(instance.StickMinDistance, 0.01f, 0.25f);
			GUILayout.Label(string.Format("Trigger after: {0}s", instance.TapMaxDuration.ToString("F2")));
			instance.TapMaxDuration = GUILayout.HorizontalSlider(instance.TapMaxDuration, 0f, 1f);
			GUILayout.Label(string.Format("Size: {0}% of screen shortest side", (instance.StickMaxDistance * 100f).ToString("F2")));
			instance.StickMaxDistance = GUILayout.HorizontalSlider(instance.StickMaxDistance, 0.01f, 0.25f);
			GUILayout.Label(string.Format("Damping: {0} (lower = more responsive)", instance.StickDamping.ToString("F3")));
			instance.StickDamping = GUILayout.HorizontalSlider(instance.StickDamping, 0f, 0.25f);
			Toggle(ref instance.DragStick, "Drag stick around");
			GUILayout.Label(string.Format("Scale visual sprite: {0}x", TouchFeedbackBehavior.GlobalScaleFactor.ToString("F2")));
			TouchFeedbackBehavior.GlobalScaleFactor = GUILayout.HorizontalSlider(TouchFeedbackBehavior.GlobalScaleFactor, 0.5f, 5f);
		}
		GUILayout.EndVertical();
	}

	private void OnMenuTapAndHold()
	{
		GUILayout.BeginVertical("box");
		AcierInputHandler instance = AcierInputHandler.Instance;
		Toggle(ref instance.EnableSelect, "Enable point to ground (Select)");
		if (instance.EnableSelect)
		{
			GUILayout.Label(string.Format("Deadzone {0}% of screen shortest side", (instance.StickMinDistance * 100f).ToString("F2")));
			instance.StickMinDistance = GUILayout.HorizontalSlider(instance.StickMinDistance, 0.01f, 0.25f);
			GUILayout.Label(string.Format("Trigger after: {0}s", instance.TapMaxDuration.ToString("F2")));
			instance.TapMaxDuration = GUILayout.HorizontalSlider(instance.TapMaxDuration, 0f, 1f);
		}
		GUILayout.EndVertical();
	}

	private void OnMenuDoubleTap()
	{
		GUILayout.BeginVertical("box");
		AcierInputHandler instance = AcierInputHandler.Instance;
		Toggle(ref instance.EnableDoubleTap, "Enable double tap");
		if (instance.EnableDoubleTap)
		{
			GUILayout.Label(string.Format("Max distance: {0}% of screen shortest side", (instance.DoubleTapMaxDistance * 100f).ToString("F2")));
			instance.DoubleTapMaxDistance = GUILayout.HorizontalSlider(instance.DoubleTapMaxDistance, 0.01f, 0.25f);
			GUILayout.Label(string.Format("Max delay: {0}s", instance.DoubleTapMaxDelay.ToString("F2")));
			instance.DoubleTapMaxDelay = GUILayout.HorizontalSlider(instance.DoubleTapMaxDelay, 0f, 1f);
		}
		GUILayout.EndVertical();
	}

	private void OnMenuAcrobatics()
	{
		GUILayout.BeginVertical("box");
		GUILayout.Label("JumpHorizontalToWall");
		Toggle(ref ParcourHelper.Acrobatics.EnableJumpHorizontalToWall, "Enabled");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Hand offset", GUILayout.Width(220f));
		float.TryParse(GUILayout.TextField(AnimationTaskClimb.HorizontalJumpToWallHandOffset.x.ToString("F2"), 10, GUILayout.Width(45f)), out AnimationTaskClimb.HorizontalJumpToWallHandOffset.x);
		float.TryParse(GUILayout.TextField(AnimationTaskClimb.HorizontalJumpToWallHandOffset.y.ToString("F2"), 10, GUILayout.Width(45f)), out AnimationTaskClimb.HorizontalJumpToWallHandOffset.y);
		float.TryParse(GUILayout.TextField(AnimationTaskClimb.HorizontalJumpToWallHandOffset.z.ToString("F2"), 10, GUILayout.Width(45f)), out AnimationTaskClimb.HorizontalJumpToWallHandOffset.z);
		GUILayout.EndHorizontal();
		Slider("MinStraightLength", ref ParcourHelper.Acrobatics.JumpHorizontalToWallMinStraightLength, 0f, 20f);
		Slider("StraightLength", ref ParcourHelper.Acrobatics.JumpHorizontalToWallStraightLength, 0f, 20f);
		Slider("MaxHeight", ref ParcourHelper.Acrobatics.JumpHorizontalToWallMaxHeight, 0f, 25f);
		Slider("MaxLength", ref ParcourHelper.Acrobatics.JumpHorizontalToWallMaxLength, 0f, 30f);
		Slider("MaxAngle", ref ParcourHelper.Acrobatics.JumpHorizontalToWallMaxAngle, 0f, 180f);
		Slider("EdgeLookupRadius", ref ParcourHelper.Acrobatics.JumpHorizontalToWallEdgeLookupRadius, 0f, 5f);
		Slider("MaxHeightDifference", ref ParcourHelper.Acrobatics.JumpHorizontalToWallMaxHeightDifference, 0f, 5f);
		Slider("ObstacleHeight", ref ParcourHelper.Acrobatics.JumpHorizontalToWallObstacleHeight, 0f, 5f);
		Slider("RaycastOffsetZ", ref ParcourHelper.Acrobatics.JumpHorizontalToWallRaycastOffsetZ, 0f, 5f);
		Slider("RaycastOffsetY", ref ParcourHelper.Acrobatics.JumpHorizontalToWallRaycastOffsetY, 0f, 5f);
		Slider("RaycastOffsetX", ref ParcourHelper.Acrobatics.JumpHorizontalToWallRaycastOffsetX, 0f, 5f);
		Slider("MinGapDepth", ref ParcourHelper.Acrobatics.JumpHorizontalToWallMinGapDepth, 0f, 25f);
		GUILayout.EndVertical();
		GUILayout.BeginVertical("box");
		GUILayout.Label("Acrobatics");
		Slider("HideoutDirectTriggerRadius", ref ParcourHelper.Acrobatics.HideoutDirectTriggerDistanceOutside, -0.01f, 10f);
		Slider("HideoutExtraTriggerOffset", ref ParcourHelper.Acrobatics.HideoutDirectTriggerExtraOffset, 0f, 1f);
		Slider("Edge distance", ref ParcourHelper.Acrobatics.EdgeDistance, 0f, 5f);
		Slider("Edge angle", ref ParcourHelper.Acrobatics.EdgeAngle, 0f, 90f);
		Slider("Link search radius", ref ParcourHelper.Acrobatics.LinkSearchRadius, 0f, 5f);
		Slider("Link search height", ref ParcourHelper.Acrobatics.MaxLinkHeightDifference, 0f, 5f);
		Slider("Link offset from edge", ref ParcourHelper.Acrobatics.LinkOffsetFromEdge, 0f, 5f);
		Slider("NavMesh lookup radius", ref ParcourHelper.Acrobatics.JumpNavMeshLookUpRadius, 0f, 5f);
		Slider("Obstacle height", ref ParcourHelper.Acrobatics.ObstacleHeight, 0f, 5f);
		Slider("Wall height", ref ParcourHelper.Acrobatics.WallHeight, 0f, 5f);
		Slider("Min clear distance", ref ParcourHelper.Acrobatics.MinClearDistance, 0f, 5f);
		Slider("Max wall distance", ref ParcourHelper.Acrobatics.MaxWallDistance, 0f, 5f);
		ParcourHelper.Acrobatics.IgnoreJumpDownLinks = GUILayout.Toggle(ParcourHelper.Acrobatics.IgnoreJumpDownLinks, "Ignore jump down links");
		GUILayout.BeginVertical("box");
		GUILayout.Label("Balance");
		Slider("ReferencePointOffsetOnRope", ref AnimationTaskBalance.ReferencePointOffsetOnRope, 0f, 5f);
		Slider("MaxDistanceToStart", ref AnimationTaskBalance.MaxDistanceToStart, 0f, 5f);
		Slider("MaxAngleToAgentForward", ref AnimationTaskBalance.MaxAngleToAgentForward, 0f, 180f);
		Slider("MaxAngleToLinkDirection", ref AnimationTaskBalance.MaxAngleToLinkDirection, 0f, 180f);
		GUILayout.EndVertical();
		GUILayout.BeginVertical("box");
		GUILayout.Label("Climb");
		Slider("Speed", ref AnimationTaskClimb.ClimbingSpeed, 0.2f, 5f);
		Slider("Max pin duration", ref AnimationTaskClimb.MaxPinDuration, 0f, 10f);
		Toggle(ref AnimationTaskClimb.AutoClimb, "Climb automatically");
		Slider("Rebound distance to wall", ref AnimationTaskClimbRebound.TargetDistanceToWall, -2f, 2f);
		Slider("Climb start distance to wall", ref AnimationTaskClimb.TargetDistanceToWall, -2f, 2f);
		Slider("Min overhang", ref AnimationTaskClimb.MinOverhang, -1f, 2f);
		Slider("Pitch correction multiplier", ref AnimationTaskClimb.PitchMultiplier, 0f, 5f);
		Slider("Pitch correction damping", ref AnimationTaskClimb.PitchDamping, 0f, 2f);
		Slider("Max pitch correction", ref AnimationTaskClimb.MaxPitch, 0f, 90f);
		Slider("Pin IK anticipate time", ref AnimatorIK.PinIKAnticipateTime, 0f, 2f);
		Slider("Pin IK anticipate damping", ref AnimatorIK.PinIKAnticipateDamping, 0f, 2f);
		Slider("Pin IK release damping", ref AnimatorIK.PinIKReleaseDamping, 0f, 2f);
		Slider("EdgeDistanceX damping", ref AnimationTaskClimb.EdgeDistanceXDamping, 0f, 2f);
		GUILayout.EndVertical();
		GUILayout.BeginVertical("box");
		GUILayout.Label("Jump");
		Slider("Max length", ref ParcourHelper.Acrobatics.JumpMaxLength, 0f, 20f);
		Slider("Max fall height (damaged)", ref ParcourHelper.Acrobatics.JumpMaxFallHeightDamaged, 0f, 40f);
		Slider("Sample offset", ref ParcourHelper.Acrobatics.JumpSampleOffset, 0f, 2f);
		Slider("Down fall angle", ref ParcourHelper.Acrobatics.JumpDownFallAngle, 0f, 90f);
		Slider("Down min extra walking distance", ref ParcourHelper.Acrobatics.JumpDownMinExtraWalkingDistance, 0f, 20f);
		Slider("Down min extra walking multiplier", ref ParcourHelper.Acrobatics.JumpDownMinExtraWalkingMultiplier, 0f, 2f);
		Slider("Forward min extra walking distance", ref ParcourHelper.Acrobatics.JumpForwardMinExtraWalkingDistance, 0f, 20f);
		Slider("Forward min extra walking multiplier", ref ParcourHelper.Acrobatics.JumpForwardMinExtraWalkingMultiplier, 0f, 2f);
		Slider("Forward fall angle", ref ParcourHelper.Acrobatics.JumpForwardFallAngle, 0f, 90f);
		Slider("Forward straight length", ref ParcourHelper.Acrobatics.JumpForwardStraightLength, 0f, 20f);
		Slider("Edge mode min height", ref RoofEdgeDetection.MinHeight, 0f, 20f);
		Slider("Edge mode max trigger distance", ref RoofEdgeDetection.MaxEdgeDistance, 0f, 20f);
		Slider("Edge mode max aim angle", ref RoofEdgeDetection.MaxAngle, 0f, 180f);
		Toggle(ref MoveInterpreter.RoofEdgePreview, "Roof edge preview");
		Toggle(ref MoveInterpreter.RoofEdgePreviewAcrobatics, "Complex acrobatics");
		Slider("Roof edge angle steps", ref MoveInterpreter.RoofEdgeAngleSteps, 1, 100);
		Slider("Roof edge distance steps", ref MoveInterpreter.RoofEdgeDistanceSteps, 1, 100);
		Slider("GamepadSelectionSensitivity", ref MoveInterpreter.GamepadSelectionSensitivity, 1f, 50f);
		Slider("GamepadSelectionMaxAngle", ref MoveInterpreter.GamepadSelectionMaxAngle, 1f, 180f);
		Slider("GamepadSelectionPeriod", ref UIGamepadHandler.SelectionPeriod, 0.01f, 2f);
		Slider("GamepadSelectionFastPeriod", ref UIGamepadHandler.SelectionFastPeriod, 0.01f, 1f);
		GUILayout.EndVertical();
		GUILayout.EndVertical();
	}

	private void Slider(string _title, ref float _value, float _min, float _max)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(_title, GUILayout.Width(220f));
		float.TryParse(GUILayout.TextField(_value.ToString("F2"), 10, GUILayout.Width(45f)), out _value);
		_value = GUILayout.HorizontalSlider(_value, _min, _max);
		GUILayout.EndHorizontal();
	}

	private void Slider(string _title, ref int _value, int _min, int _max)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(_title, GUILayout.Width(220f));
		int.TryParse(GUILayout.TextField(_value.ToString("D"), 10, GUILayout.Width(45f)), out _value);
		_value = Mathf.RoundToInt(GUILayout.HorizontalSlider(_value, _min, _max));
		GUILayout.EndHorizontal();
	}

	public override void OnOpen()
	{
	}

	public override void OnApply()
	{
	}

	private void OnRenderShowGamepadInputStatus()
	{
		if (!m_showGamepadInput)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder("GamePad Settings:\n");
		UIGamepadHandler uIGamepadHandler = AcierInputHandler.Instance.UIGamepadHandler;
		GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
		if (gamepad == null)
		{
			stringBuilder.Append("No GamePad found\n\n");
		}
		else
		{
			if (uIGamepadHandler != null)
			{
				stringBuilder.AppendFormat("Animus Pad Status:\n\n");
				stringBuilder.AppendFormat("{0,-30} {1,5}\n", "PadActive:", uIGamepadHandler.PadActive);
				stringBuilder.AppendFormat("{0,-30} {1,5}\n", "PadDetected: ", uIGamepadHandler.PadDetected);
			}
			stringBuilder.AppendFormat("Name\n\n");
			stringBuilder.AppendFormat("{0,-30} {1,5}\n", "Direction:", gamepad.Direction);
			stringBuilder.AppendFormat("{0,-30} {1,5}\n", "Camera: ", gamepad.Camera);
			stringBuilder.AppendFormat("{0,-30} {1,5}\n", "AcceptHeld: ", gamepad.AcceptHeld);
			stringBuilder.AppendFormat("{0,-30} {1,5}\n", "CloseDown: ", gamepad.CloseDown);
			UnityInputDevice unityInputDeviceInstance = UnityInputDevice.UnityInputDeviceInstance;
			if (unityInputDeviceInstance != null)
			{
				int analogCount = unityInputDeviceInstance.Profile.AnalogCount;
				for (int i = 0; i < analogCount; i++)
				{
					InputControlMapping inputControlMapping = unityInputDeviceInstance.Profile.AnalogMappings[i];
					UnityAnalogSource unityAnalogSource = inputControlMapping.Source as UnityAnalogSource;
					if (unityAnalogSource != null)
					{
						float value = unityAnalogSource.GetValue(unityInputDeviceInstance);
						float otherValue = unityAnalogSource.GetOtherValue(unityInputDeviceInstance);
						stringBuilder.AppendFormat("{0,-30} {1,5:N2} {1,5:N2}\n", inputControlMapping.Handle, otherValue, value);
					}
				}
			}
		}
		if ((bool)uIGamepadHandler)
		{
			stringBuilder.AppendFormat("{0,-30} {1,5}\n", "isVis:", UIDownloadScreen.IsLoadedAndFullyVisible);
			stringBuilder.AppendFormat("{0,-30} {1,5}\n", "CurrentSelection:", (!(uIGamepadHandler.CurrentSelection != null)) ? "None" : uIGamepadHandler.CurrentSelection.name);
			stringBuilder.AppendFormat("{0,-30} {1,5}\n", "Selc Count :", uIGamepadHandler.CenterSelectables.Count);
			stringBuilder.AppendFormat("{0,-30} {1,5}\n", "CurrentScreen:", (!(uIGamepadHandler.CurrentScreen != null)) ? string.Empty : uIGamepadHandler.CurrentScreen.name);
		}
		float x = (float)Screen.width / 2f;
		DebugHelper.DrawOutlineLabel(new Rect(x, 0f, 100f, Screen.height), stringBuilder.ToString());
	}
}
