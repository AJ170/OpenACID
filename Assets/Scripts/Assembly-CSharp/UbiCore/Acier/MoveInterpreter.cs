using System;
using System.Collections.Generic;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Motion;
using Assets.Scripts.Mission.Motion.Hotspots;
using Assets.Scripts.Mission.Selection;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using SharedBaseLib.System.Extensions;
using UbiCore.Debug;
using UbiCore.Messenger;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace UbiCore.Acier
{
	public class MoveInterpreter : IconInterpreter
	{
		public enum EInteractionType
		{
			None = 0,
			Move = 1,
			TappedMesh = 2,
			Arrow = 3,
			HotSpot = 4,
			InteractableEntity = 5
		}

		public static bool DisplayDebug = false;

		public static bool StickyInput = false;

		public static bool VariableMoveSpeed = false;

		public static float NoInputStopMovementDelay = 0.1f;

		public AnimationCurve SelectMappingCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		public static float SelectMinDistance = 1f;

		public static float SelectMaxDistance = 1.5f;

		public static bool AutoSelectTarget = true;

		public static float AutoSelectRadius = 5f;

		public static float AutoSelectMinScreenMargin = 0.125f;

		public static float AutoSelectReplaceDistanceThreshold = 5f;

		public static float InputCorrectionDamping = 0.25f;

		public static float InputCorrectionDragMultiplier = 0.8f;

		public static float InputCorrectionBase = 0.4f;

		public static float TapNavMeshLookupRadius = 1f;

		public static float TapMaxDistance = 60f;

		public static float TapMaxTargetOffset = 2f;

		public static float TapPathTooLongThreshold = 2f;

		public static float TapMaxWallHeight = 100f;

		public static float DragMoveDistance = 2.5f;

		public static AnimationCurve DragSpeedCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		public static float DragMaxPathStartAngle = 100f;

		public static float DragMaxEdgeDistance = 0.5f;

		public static float DragPathTooLongThreshold = 2f;

		public static float DragPathMaxSlideAngle = 45f;

		public static int RoofEdgeAngleSteps = 10;

		public static int RoofEdgeDistanceSteps = 10;

		public static bool RoofEdgePreview = false;

		public static bool RoofEdgePreviewAcrobatics = false;

		public static float MinInputStrength = 0.001f;

		public static float GamepadSelectionSensitivity = 10f;

		public static float GamepadSelectionMaxAngle = 80f;

		public static float GamepadSelectionPeriod = 0.35f;

		private Vector3 m_referenceInputDirection = Vector3.forward;

		private bool m_enabled;

		private float m_lastDragDistance;

		private Vector2 m_lastDragPosition;

		private bool m_hasInput;

		private bool m_hadInput;

		private Vector3 m_inputVector = Vector3.zero;

		private bool m_padHasInput;

		private Vector3 m_lastPadInput;

		private float m_stopMovingTime = float.MaxValue;

		private bool m_interruptApproach;

		private float m_padSelectCooldown;

		private BBSimpleList<SelectionCandidate> m_selectionCandidates;

		private Vector2 m_roofEdgeScreenPoint;

		private Vector3 m_roofEdgetarget;

		private bool m_hasRoofEdgetarget;

		private Vector3 m_lastReboundPosition;

		private bool m_playNextRebound;

		private readonly NavMeshPath m_path = new NavMeshPath();

		private RaycastHit m_raycastHit;

		private NavMeshHit m_navMeshHit;

		private BBList<GameObject> m_targetJumpSpots = new BBList<GameObject>();

		public bool HasInput
		{
			get
			{
				return m_hasInput;
			}
		}

		public override bool InputEnabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				m_enabled = value;
				AcierInputHandler.Instance.RoofEdgeDetection.Reset();
				AcierInputHandler.Instance.ParcourPrediction.Reset();
				m_hasInput = false;
				m_padHasInput = false;
				m_inputVector = Vector3.zero;
				if (value)
				{
					Vector3 forward = Camera.main.transform.parent.forward;
					forward.y = 0f;
					forward.Normalize();
					m_referenceInputDirection = forward;
					if (LogicMain.PlayerEntity.GetIsNavigating(true))
					{
						m_stopMovingTime = float.MaxValue;
					}
					else
					{
						m_stopMovingTime = Mathf.Min(m_stopMovingTime, LogicMain.CurrentTime + NoInputStopMovementDelay);
					}
				}
			}
		}

		public override void OnUpdate()
		{
			if (!base.IsReady || !InputEnabled || !PlayerHelper.IsMovementAllowed)
			{
				return;
			}
			base.OnUpdate();
			if (!CheckPlayerStatus())
			{
				return;
			}
			Profiling.Start("MoveInterpreter::Update()", float.MaxValue);
			Vector3 position = LogicMain.PlayerEntity.Position;
			if (!m_playNextRebound && (position - m_lastReboundPosition).LengthXZ() > 2f)
			{
				m_playNextRebound = true;
			}
			if (LogicMain.CurrentTime > m_stopMovingTime)
			{
				m_stopMovingTime = float.MaxValue;
				StopMoving();
			}
			if (UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection == null)
			{
				PlayerCharacter playerEntity = LogicMain.PlayerEntity;
				if (playerEntity.GetIsHidingInPatrol() || playerEntity.GetIsStalking())
				{
					AutoSelectEnemy();
				}
			}
			CheckPadSelection();
			UpdatePadInput();
			UpdateRoofEdgeMode();
			if (!m_hasRoofEdgetarget)
			{
				bool flag = CheckParcourInteraction();
				if (m_hasInput && !flag)
				{
					ApplyInput();
				}
			}
			if (!m_hasInput && m_hadInput)
			{
				m_stopMovingTime = Mathf.Min(m_stopMovingTime, LogicMain.CurrentTime + NoInputStopMovementDelay);
				m_hadInput = false;
			}
			m_hasInput = StickyInput;
			if (!m_hasInput)
			{
				m_inputVector = Vector3.zero;
			}
			Profiling.Stop();
		}

		private void AutoSelectEnemy()
		{
			Vector3 position = IconInterpreter.Entity.Position;
			Collider[] array = Physics.OverlapSphere(position, AutoSelectRadius, Layers.InteractorsLayerMask);
			if (array.Length <= 0)
			{
				return;
			}
			Array.Sort(array, (Collider o, Collider p) => (o.transform.position - position).sqrMagnitude.CompareTo((p.transform.position - position).sqrMagnitude));
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].tag == "Interactable")
				{
					EnemyCharacter enemyCharacter = array[i].gameObject.TryGetEntity() as EnemyCharacter;
					if (enemyCharacter != null && !enemyCharacter.GetIsDead())
					{
						UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection = enemyCharacter;
						break;
					}
				}
			}
		}

		private void CheckPadSelection()
		{
			m_padSelectCooldown -= LogicMain.FrameTime;
			if (m_padSelectCooldown > 0f)
			{
				return;
			}
			GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
			if (gamepad.Select.sqrMagnitude < 0.25f)
			{
				return;
			}
			Camera main = Camera.main;
			if (m_selectionCandidates == null)
			{
				m_selectionCandidates = new BBSimpleList<SelectionCandidate>();
			}
			else
			{
				m_selectionCandidates.Clear();
			}
			GetObjectsOnScreen(m_selectionCandidates);
			int num = -1;
			ISelectable currentSelection = UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection;
			Vector2 vector;
			if (currentSelection == null)
			{
				vector = new Vector2(main.pixelWidth, main.pixelHeight) * 0.5f;
			}
			else
			{
				num = m_selectionCandidates.FindIndex((SelectionCandidate _c) => _c.Entity == currentSelection);
				vector = m_selectionCandidates[num].ScreenPosition;
			}
			int num2 = -1;
			float num3 = float.MaxValue;
			for (int i = 0; i < m_selectionCandidates.Count; i++)
			{
				if (num == i)
				{
					continue;
				}
				Vector2 to = m_selectionCandidates[i].ScreenPosition - vector;
				float num4 = Vector2.Angle(gamepad.Select, to);
				if (!(num4 > GamepadSelectionMaxAngle))
				{
					float magnitude = to.magnitude;
					float num5 = num4 / GamepadSelectionMaxAngle + GamepadSelectionSensitivity * Mathf.Min(magnitude / (float)main.pixelWidth, 1f);
					if (num5 < num3)
					{
						num3 = num5;
						num2 = i;
					}
				}
			}
			if (num2 >= 0)
			{
				UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection = m_selectionCandidates[num2].Entity;
				m_padSelectCooldown = GamepadSelectionPeriod;
			}
		}

		private bool CheckPlayerStatus()
		{
			if (LogicMain.PlayerEntity.AssassinCharacterStateMachines.PlayerMain.ScheduledStateType == EStateType.CombatCharacterMain_Parcour)
			{
				return false;
			}
			return true;
		}

		private void UpdateRoofEdgeMode()
		{
			RoofEdgeDetection roofEdgeDetection = AcierInputHandler.Instance.RoofEdgeDetection;
			roofEdgeDetection.UpdateRoofEdgeDetection((!(m_inputVector.magnitude > MinInputStrength)) ? Vector3.zero : m_inputVector.normalized);
			if (roofEdgeDetection.AtRoofEdge)
			{
				CheckRoofEdgePadInput();
				if (RoofEdgePreview)
				{
					DisplayTargetJumpSpots();
				}
			}
			else
			{
				m_hasRoofEdgetarget = false;
				if (RoofEdgePreview)
				{
					HideTargetJumpSpots();
				}
			}
		}

		private bool CheckRoofEdgePadInput()
		{
			GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
			if (gamepad.FreeAimDown)
			{
				m_roofEdgeScreenPoint.x = (float)Screen.width * 0.5f;
				m_roofEdgeScreenPoint.y = (float)Screen.height * 0.5f;
			}
			bool freeAimHeld = gamepad.FreeAimHeld;
			if (freeAimHeld)
			{
				m_inputVector = Vector3.zero;
				m_hasInput = false;
				m_roofEdgeScreenPoint += gamepad.Direction * Time.deltaTime * AcierInputHandler.Instance.PixelScaleFactor;
				m_roofEdgeScreenPoint.x = Mathf.Clamp(m_roofEdgeScreenPoint.x, 0f, Screen.width);
				m_roofEdgeScreenPoint.y = Mathf.Clamp(m_roofEdgeScreenPoint.y, 0f, Screen.height);
				Ray ray = Camera.main.ScreenPointToRay(m_roofEdgeScreenPoint);
				m_hasRoofEdgetarget = false;
				if (RaycastForTargetTapPosition(m_roofEdgeScreenPoint, ref m_raycastHit))
				{
					m_roofEdgetarget = m_raycastHit.point;
					if (ParcourHelper.CanJumpToPoint(IconInterpreter.NavMeshAgent.transform.position, m_raycastHit.point))
					{
						m_hasRoofEdgetarget = true;
						MoveInterpreterDecals.Instance.NotifyGoodPosition(m_roofEdgetarget);
					}
					else
					{
						MoveInterpreterDecals.Instance.NotifyInvalidPosition(m_roofEdgetarget);
					}
				}
			}
			bool freeAimUp = gamepad.FreeAimUp;
			if (freeAimUp)
			{
				if (m_hasRoofEdgetarget && ParcourHelper.CanJumpToPoint(IconInterpreter.NavMeshAgent.transform.position, m_roofEdgetarget))
				{
					PlayerHelper.EnterParcour(new JumpAnywhereEvent(m_roofEdgetarget));
					MoveInterpreterDecals.Instance.NotifyGoodPosition(m_roofEdgetarget);
				}
				m_roofEdgeScreenPoint = Vector2.zero;
				m_roofEdgetarget = Vector3.zero;
				m_hasRoofEdgetarget = false;
			}
			ParcourPrediction parcourPrediction = AcierInputHandler.Instance.ParcourPrediction;
			if (m_hasRoofEdgetarget)
			{
				parcourPrediction.SetJump(m_roofEdgetarget);
			}
			return freeAimHeld || freeAimUp;
		}

		private void HideTargetJumpSpots()
		{
			for (int i = 0; i < m_targetJumpSpots.Count; i++)
			{
				if (m_targetJumpSpots[i].activeSelf)
				{
					m_targetJumpSpots[i].SetActive(false);
				}
			}
		}

		private void DisplayTargetJumpSpots()
		{
			Profiling.Start("Roof edge preview", float.MaxValue);
			RoofEdgeDetection roofEdgeDetection = AcierInputHandler.Instance.RoofEdgeDetection;
			Quaternion quaternion = Quaternion.LookRotation(-roofEdgeDetection.RoofEdgeNormal);
			Quaternion a = Quaternion.Euler(0f, 0f - RoofEdgeDetection.MaxAngle, 0f) * quaternion;
			Quaternion b = Quaternion.Euler(0f, RoofEdgeDetection.MaxAngle, 0f) * quaternion;
			NavMeshAgent navMeshAgent = IconInterpreter.NavMeshAgent;
			Vector3 position = navMeshAgent.transform.position;
			BBHashSet<Vector3> bBHashSet = new BBHashSet<Vector3>();
			Profiling.Start("List points", float.MaxValue);
			int roofEdgeAngleSteps = RoofEdgeAngleSteps;
			int roofEdgeDistanceSteps = RoofEdgeDistanceSteps;
			NavMeshHit hit;
			for (int i = 0; i < roofEdgeAngleSteps; i++)
			{
				float t = (float)i / (float)(roofEdgeAngleSteps - 1);
				Quaternion quaternion2 = Quaternion.Slerp(a, b, t);
				Vector3 vector = quaternion2 * Vector3.forward;
				for (int j = 0; j < roofEdgeDistanceSteps; j++)
				{
					float t2 = (float)j / (float)(roofEdgeDistanceSteps - 1);
					float num = Mathf.Lerp(0.5f, ParcourHelper.Acrobatics.JumpMaxLength, t2);
					Vector3 _targetPosition;
					if (RoofEdgePreviewAcrobatics)
					{
						eLinkType _linkType;
						OffMeshLinkEntity _link;
						Vector3 _targetDirection;
						GameObject _targetObject;
						if (ParcourHelper.CheckAcrobatics(navMeshAgent, position, vector, out _linkType, out _link, out _targetPosition, out _targetDirection, out _targetObject, num) != 0)
						{
							bBHashSet.Add(_targetPosition);
						}
						continue;
					}
					_targetPosition = position + vector * num;
					RaycastHit hitInfo;
					if (Physics.Raycast(_targetPosition, Vector3.down, out hitInfo, ParcourHelper.Acrobatics.ObstacleHeight + ParcourHelper.Acrobatics.JumpMaxFallHeightDamaged, Layers.StaticCollidersLayersMaskPreset) && NavMesh.SamplePosition(hitInfo.point, out hit, 0.2f, NavMeshLayers.WalkableLayerMaskPreset) && ParcourHelper.CanJumpToPoint(position, hit.position, false))
					{
						_targetPosition = hit.position;
						bBHashSet.Add(_targetPosition);
					}
				}
			}
			Profiling.Stop();
			Profiling.Start("Group points", float.MaxValue);
			BBList<BBHashSet<Vector3>> bBList = new BBList<BBHashSet<Vector3>>();
			while (bBHashSet.Count > 0)
			{
				Vector3 _targetPosition = bBHashSet.GetEnumerator().Current;
				bBHashSet.Remove(_targetPosition);
				bool flag = false;
				BBHashSet<Vector3> bBHashSet2 = null;
				BBList<BBHashSet<Vector3>> bBList2 = new BBList<BBHashSet<Vector3>>();
				foreach (BBHashSet<Vector3> item in bBList)
				{
					foreach (Vector3 item2 in item)
					{
						if (!NavigationHelper.SafeRaycast(_targetPosition, item2, out hit, NavMeshLayers.WalkableLayerMaskPreset) && (hit.position - item2).magnitude < 0.1f)
						{
							if (!flag)
							{
								item.Add(_targetPosition);
								bBHashSet2 = item;
								flag = true;
							}
							else
							{
								bBList2.Add(item);
							}
							break;
						}
					}
				}
				foreach (BBHashSet<Vector3> item3 in bBList2)
				{
					foreach (Vector3 item4 in item3)
					{
						bBHashSet2.Add(item4);
					}
					bBList.Remove(item3);
				}
				if (!flag)
				{
					BBHashSet<Vector3> bBHashSet3 = new BBHashSet<Vector3>();
					bBHashSet3.Add(_targetPosition);
					bBList.Add(bBHashSet3);
				}
			}
			Profiling.Stop();
			Profiling.Start("Find centers", float.MaxValue);
			BBList<Vector3> bBList3 = new BBList<Vector3>();
			foreach (BBHashSet<Vector3> item5 in bBList)
			{
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				float num2 = float.MaxValue;
				int num3 = 0;
				foreach (Vector3 item6 in item5)
				{
					zero2 += item6;
					num3++;
					float num4 = Vector3.Distance(item6, position) + Vector3.Angle(item6 - position, quaternion * Vector3.forward) / 360f * ParcourHelper.Acrobatics.JumpMaxLength / (float)roofEdgeDistanceSteps;
					if (num4 < num2)
					{
						num2 = num4;
						zero = item6;
					}
				}
				if (num3 > 0)
				{
					zero2 /= (float)num3;
				}
				foreach (Vector3 item7 in item5)
				{
					if (!((item7 - zero2).sqrMagnitude > 0.01f))
					{
					}
				}
				Vector3 vector2 = zero2;
				bool flag2 = false;
				float num5 = float.MaxValue;
				foreach (Vector3 item8 in item5)
				{
					float magnitude = (item8 - zero2).magnitude;
					if (magnitude < num5 && ParcourHelper.CanJumpToPoint(position, vector2))
					{
						num5 = magnitude;
						vector2 = item8;
						flag2 = true;
					}
				}
				if (flag2)
				{
					bBList3.Add(vector2);
				}
			}
			Profiling.Stop();
			Profiling.Start("Display targets", float.MaxValue);
			while (m_targetJumpSpots.Count < bBList.Count)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Environment/Indicators/MoveInterpreter/TargetIndicator/Prefabs/target_indicator", typeof(GameObject)));
				m_targetJumpSpots.Add(gameObject);
				gameObject.transform.parent = AcierInputHandler.Instance.transform;
				gameObject.name = "JumpSpot_Highlight";
			}
			for (int k = bBList.Count; k < m_targetJumpSpots.Count; k++)
			{
				if (m_targetJumpSpots[k].activeSelf)
				{
					m_targetJumpSpots[k].SetActive(false);
				}
			}
			for (int l = 0; l < bBList3.Count; l++)
			{
				GameObject gameObject2 = m_targetJumpSpots[l];
				if (!gameObject2.activeSelf)
				{
					gameObject2.SetActive(true);
				}
				gameObject2.transform.position = bBList3[l] + Vector3.up * 0.1f;
			}
			Profiling.Stop();
			Profiling.Stop();
		}

		private void UpdatePadInput()
		{
			GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
			Vector3 vector = new Vector3(gamepad.Direction.x, 0f, gamepad.Direction.y);
			m_interruptApproach = vector != m_lastPadInput;
			m_lastPadInput = vector;
			if (vector.magnitude > float.Epsilon)
			{
				m_padHasInput = true;
				Vector3 cameraForward = GetCameraForward();
				m_inputVector = Quaternion.LookRotation(cameraForward) * vector;
				m_hasInput = true;
				if (AutoSelectTarget)
				{
					AutoSelectClosest();
				}
			}
			else if (m_padHasInput)
			{
				m_padHasInput = false;
				m_hasInput = false;
			}
		}

		private void AutoSelectClosest()
		{
			SelectObject(GetClosestObject(true));
		}

		private void ApplyInput()
		{
			float speed = Mathf.Clamp01((!VariableMoveSpeed) ? 1f : DragSpeedCurve.Evaluate(m_inputVector.magnitude));
			if ((m_interruptApproach || !PlayerHelper.IsApproachingTarget) && CanAbortOtherInteractions(EInteractionType.Move))
			{
				AbortOtherInteractions(EInteractionType.Move);
				MoveInDragDirection(m_inputVector.normalized, speed, EDispatcherType.Drag);
			}
			m_interruptApproach = false;
			m_hadInput = true;
			m_stopMovingTime = float.MaxValue;
			MoveInterpreterDecals.Instance.Enabled = false;
		}

		private bool CheckParcourInteraction()
		{
			ParcourPrediction parcourPrediction = AcierInputHandler.Instance.ParcourPrediction;
			parcourPrediction.UpdateParcourPrediction((!(m_inputVector.magnitude > MinInputStrength)) ? Vector3.zero : m_inputVector.normalized, -1f);
			if (!parcourPrediction.HasInteraction || parcourPrediction.DistanceToStart > float.Epsilon)
			{
				return false;
			}
			bool flag = false;
			switch (parcourPrediction.Interaction)
			{
			case ParcourHelper.EEnvironmentInteraction.Hotspot:
				if (UbiCore.Utils.Singleton<HotspotService>.Instance.Interact(PlayerHelper.Entity, parcourPrediction.Object, IconInterpreter.NavMeshAgent))
				{
					MoveInterpreterDecals.Instance.Enabled = false;
					flag = true;
				}
				break;
			case ParcourHelper.EEnvironmentInteraction.CivilianCover:
				if (PlayerHelper.Entity.AssassinInteraction.EnterPatrol(parcourPrediction.Entity.Id, ((Patrol)parcourPrediction.Entity).PatrolDefinition.Definition.InteractionReason))
				{
					MoveInterpreterDecals.Instance.Enabled = false;
					flag = true;
				}
				break;
			case ParcourHelper.EEnvironmentInteraction.JumpDown:
				flag = PlayerHelper.EnterParcour(new JumpAnywhereEvent(parcourPrediction.EndPosition));
				break;
			case ParcourHelper.EEnvironmentInteraction.JumpForward:
				flag = PlayerHelper.EnterParcour(new JumpAnywhereEvent(parcourPrediction.EndPosition));
				break;
			case ParcourHelper.EEnvironmentInteraction.Link:
				flag = PlayerHelper.EnterParcour(new UseLinkEvent(parcourPrediction.Link));
				break;
			case ParcourHelper.EEnvironmentInteraction.JumpHorizontalToWall:
			{
				List<ClimbRouteEntry> list = new List<ClimbRouteEntry>();
				list.Add(new ClimbRouteEntry(ParcourHelper.JumpHorizontalToWallLastEdge, ParcourHelper.JumpHorizontalToWallLastEdge.ClosestPoint(parcourPrediction.EndPosition)));
				ClimbRoute route = new ClimbRoute(list);
				flag = PlayerHelper.EnterParcour(new JumpHorizontalToWallEvent(route, parcourPrediction.StartPosition, parcourPrediction.EndPosition));
				break;
			}
			case ParcourHelper.EEnvironmentInteraction.Rebound:
				if (m_playNextRebound)
				{
					flag = PlayerHelper.EnterParcour(new ClimbReboundEvent(parcourPrediction.EndPosition, -parcourPrediction.EndDirection));
					m_lastReboundPosition = parcourPrediction.StartPosition;
					m_playNextRebound = false;
				}
				break;
			}
			if (parcourPrediction.Interaction == ParcourHelper.EEnvironmentInteraction.CivilianCover || parcourPrediction.Interaction == ParcourHelper.EEnvironmentInteraction.Hotspot)
			{
				IconInterpreter.Entity.PlayerEventHandler.StopApproaching(false);
			}
			if (flag)
			{
				parcourPrediction.Reset();
			}
			return flag;
		}

		public override bool OnDoubleTap(TapEvent _event)
		{
			if (!PlayerHelper.IsMovementAllowed)
			{
				return false;
			}
			if (base.OnDoubleTap(_event))
			{
				return true;
			}
			GameObject touchedObject = GetTouchedObject(_event.Position);
			GameObjectEntity gameObjectEntity = ((!(touchedObject != null)) ? null : touchedObject.TryGetEntity());
			if (gameObjectEntity != null && PlayerHelper.AssassinInteraction.DoubleTapEntity(gameObjectEntity.Id))
			{
				AbortOtherInteractions(EInteractionType.InteractableEntity);
				return true;
			}
			return false;
		}

		public override bool OnSecondTap(TapEvent _event)
		{
			if (!PlayerHelper.IsMovementAllowed)
			{
				return false;
			}
			if (base.OnSecondTap(_event))
			{
				return true;
			}
			return OnTap(_event);
		}

		public override bool OnTap(TapEvent _event)
		{
			if (!PlayerHelper.IsMovementAllowed)
			{
				return false;
			}
			if (base.OnTap(_event))
			{
				m_stopMovingTime = float.MaxValue;
				return true;
			}
			GameObject touchedObject = GetTouchedObject(_event.Position);
			if (InteractWithObject(touchedObject))
			{
				m_hasInput = false;
				m_stopMovingTime = float.MaxValue;
				return true;
			}
			if (m_hasInput)
			{
				m_hasInput = false;
				IconInterpreter.NavMeshAgent.ResetPath();
				return true;
			}
			if (StickyInput && IconInterpreter.NavMeshAgent.hasPath)
			{
				IconInterpreter.NavMeshAgent.ResetPath();
				return true;
			}
			return MoveToTapPosition(_event.Position);
		}

		public override bool OnSelect(SelectEvent _event)
		{
			if (!PlayerHelper.IsMovementAllowed)
			{
				return false;
			}
			if (base.OnSelect(_event))
			{
				return true;
			}
			UpdateDragEventReferences(_event);
			if (_event.Phase == InputEvent.InputPhase.Ended)
			{
				m_hasInput = false;
				StopMoving();
				return true;
			}
			if (Vector3.Dot(m_referenceInputDirection, GetCameraForward()) < 0f)
			{
				return true;
			}
			NavMeshAgent navMeshAgent = IconInterpreter.NavMeshAgent;
			Vector3 position = navMeshAgent.transform.position;
			Ray ray = Camera.main.ScreenPointToRay(_event.Position);
			Vector3 vector = ray.origin + ray.direction * ((position.y - ray.origin.y) / (0f - Mathf.Abs(ray.direction.y)));
			Vector3 vector2 = vector - position;
			Vector3 normalized = vector2.normalized;
			float num = SelectMappingCurve.Evaluate(Mathf.Clamp01((vector2.magnitude - SelectMinDistance) / (SelectMaxDistance - SelectMinDistance)));
			m_hasInput = num > 0f;
			if (!m_hasInput)
			{
				return true;
			}
			m_inputVector = normalized * num;
			if (AutoSelectTarget)
			{
				SelectObject(GetTouchedObject(_event.Position, true));
			}
			return true;
		}

		public override bool OnDrag(DragEvent _event)
		{
			if (!PlayerHelper.IsMovementAllowed)
			{
				return false;
			}
			if (base.OnDrag(_event))
			{
				return true;
			}
			UpdateDragEventReferences(_event);
			if (_event.Phase == InputEvent.InputPhase.Ended)
			{
				m_hasInput = false;
				StopMoving();
				return true;
			}
			float num = _event.StickInput.magnitude * m_referenceInputDirection.magnitude;
			Quaternion quaternion = Quaternion.LookRotation(m_referenceInputDirection);
			Vector3 vector = new Vector3(_event.StickInput.x, 0f, _event.StickInput.y);
			Vector3 vector2 = quaternion * vector.normalized;
			if (DisplayDebug)
			{
			}
			if (DisplayDebug)
			{
			}
			m_hasInput = num > 0f;
			if (!m_hasInput)
			{
				return true;
			}
			m_inputVector = vector2 * num;
			if (AutoSelectTarget)
			{
				AutoSelectClosest();
			}
			return true;
		}

		private void UpdateDragEventReferences(DragEvent _event)
		{
			float pixelScaleFactor = AcierInputHandler.Instance.PixelScaleFactor;
			m_lastDragDistance = ((_event.Phase == InputEvent.InputPhase.Ended) ? 0f : ((_event.Position - _event.StartPosition).magnitude / pixelScaleFactor));
			m_interruptApproach = _event.Phase == InputEvent.InputPhase.Started || (_event.Position - m_lastDragPosition).magnitude / pixelScaleFactor > 0.05f;
			m_lastDragPosition = _event.Position;
			Vector3 cameraForward = GetCameraForward();
			if (_event.Phase == InputEvent.InputPhase.Started || _event.Phase == InputEvent.InputPhase.Ended || _event.StickInput.magnitude <= 0f)
			{
				m_referenceInputDirection = cameraForward;
				return;
			}
			float num = _event.Delta.magnitude / pixelScaleFactor * InputCorrectionDragMultiplier + InputCorrectionBase * UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime;
			m_referenceInputDirection = Vector3.Lerp(m_referenceInputDirection, cameraForward, num / (num + InputCorrectionDamping));
		}

		private Vector3 GetCameraForward()
		{
			Vector3 forward = Camera.main.transform.parent.forward;
			forward.y = 0f;
			forward.Normalize();
			return forward;
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
				m_stopMovingTime = float.MaxValue;
				return true;
			}
			return false;
		}

		private GameObject GetTouchedObject(Vector2 _position, bool _onlyCharacters = false)
		{
			if (!RaycastForInteractableObject(_position, ref m_raycastHit))
			{
				return null;
			}
			GameObject gameObject = m_raycastHit.collider.gameObject;
			Character character = gameObject.TryGetEntity() as Character;
			if ((_onlyCharacters && character != null && !character.GetIsDead()) || (!_onlyCharacters && (character == null || !character.GetIsDead())))
			{
				return gameObject;
			}
			return null;
		}

		private GameObject GetClosestObject(bool _onlyCharacters = false)
		{
			NavMeshAgent navMeshAgent = IconInterpreter.NavMeshAgent;
			if (navMeshAgent == null)
			{
				return null;
			}
			Vector3 position = navMeshAgent.transform.position;
			if (navMeshAgent.nextOffMeshLinkData.valid)
			{
				position = navMeshAgent.nextOffMeshLinkData.startPos;
			}
			else if (navMeshAgent.hasPath)
			{
				NavMeshPath path = navMeshAgent.path;
				Vector3[] corners = path.corners;
				if (corners.Length > 0)
				{
					position = corners[corners.Length - 1];
				}
			}
			position += Vector3.up * 1f;
			return GetClosestObject(position, _onlyCharacters);
		}

		private GameObject GetClosestObject(Vector3 _position, bool _onlyCharacters = false)
		{
			GameObject result = null;
			Collider[] array = Physics.OverlapSphere(_position, AutoSelectRadius, Layers.InteractorsLayerMask);
			if (array.Length > 0)
			{
				BBList<Collider> bBList = new BBList<Collider>(array);
				bBList.Sort((Collider o, Collider p) => (o.transform.position - _position).sqrMagnitude.CompareTo((p.transform.position - _position).sqrMagnitude));
				foreach (Collider item in bBList)
				{
					if (!(item.tag == "Interactable"))
					{
						continue;
					}
					Plane[] lastCameraFrustum = AcierCameraController.Instance.LastCameraFrustum;
					if (!GeometryUtility.TestPlanesAABB(lastCameraFrustum, item.bounds))
					{
						continue;
					}
					Vector3 center = item.bounds.center;
					Vector3 vector = Camera.main.transform.position - center;
					if (Physics.Raycast(center, vector.normalized, vector.magnitude, Layers.StaticCollidersLayersMaskPreset))
					{
						continue;
					}
					Vector3 vector2 = Camera.main.WorldToScreenPoint(center);
					float num = Mathf.Min(Screen.width, Screen.height);
					if (!(vector2.x / num < AutoSelectMinScreenMargin) && !(((float)Screen.width - vector2.x) / num < AutoSelectMinScreenMargin) && !(vector2.y / num < AutoSelectMinScreenMargin) && !(((float)Screen.height - vector2.y) / num < AutoSelectMinScreenMargin))
					{
						Character character = item.gameObject.TryGetEntity() as Character;
						if ((_onlyCharacters && character != null && !character.GetIsDead()) || (!_onlyCharacters && (character == null || !character.GetIsDead())))
						{
							result = item.gameObject;
							break;
						}
					}
				}
			}
			return result;
		}

		private bool InteractWithObject(GameObject _target)
		{
			if (_target == null)
			{
				return false;
			}
			TappedMesh component = _target.GetComponent<TappedMesh>();
			if (component != null && component.OnTap())
			{
				AbortOtherInteractions(EInteractionType.TappedMesh);
				return true;
			}
			if (UbiCore.Utils.Singleton<HotspotService>.Instance.Interact(PlayerHelper.Entity, _target, IconInterpreter.NavMeshAgent))
			{
				AbortOtherInteractions(EInteractionType.HotSpot);
				return true;
			}
			GameObjectEntity gameObjectEntity = _target.TryGetEntity();
			if (gameObjectEntity != null && PlayerHelper.AssassinInteraction.TapEntity(gameObjectEntity.Id))
			{
				AbortOtherInteractions(EInteractionType.InteractableEntity);
				return true;
			}
			return false;
		}

		private bool SelectObject(GameObject _target, bool _forceReplace = false)
		{
			if (_target == null)
			{
				return false;
			}
			GameObjectEntity gameObjectEntity = _target.TryGetEntity();
			if (gameObjectEntity != null && PlayerHelper.AssassinInteraction.SelectEntity(gameObjectEntity.Id, _forceReplace, AutoSelectReplaceDistanceThreshold, false))
			{
				return true;
			}
			return false;
		}

		private bool CanAbortOtherInteractions(EInteractionType _eventType)
		{
			bool result = true;
			if (_eventType != EInteractionType.InteractableEntity && _eventType != EInteractionType.HotSpot)
			{
				result = PlayerHelper.AssassinInteraction.CanStopApproaching;
			}
			return result;
		}

		private void AbortOtherInteractions(EInteractionType _eventType)
		{
			if (_eventType != EInteractionType.InteractableEntity && _eventType != EInteractionType.HotSpot)
			{
				if (_eventType != EInteractionType.Move)
				{
					UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection = null;
				}
				PlayerHelper.AssassinInteraction.StopApproaching(false);
			}
			if (_eventType != EInteractionType.Move)
			{
				MoveInterpreterDecals.Instance.Enabled = false;
			}
		}

		public void MoveInDragDirection(Vector3 _direction, float _speed, EDispatcherType _dispatcher)
		{
			Vector3 position = IconInterpreter.NavMeshAgent.transform.position;
			Vector3 targetPosition = position + _direction * DragMoveDistance;
			NavMeshPath path = m_path;
			bool bestDragPath = GetBestDragPath(position, targetPosition, path);
			if (DisplayDebug)
			{
			}
			UbiCore.Messenger.Messenger.Instance.Broadcast(new MoveToEvent((!bestDragPath) ? new NavMeshPath() : path, _direction, MoveToEvent.EInputType.GroundDrag, _speed, null, _dispatcher));
			if (!bestDragPath)
			{
				StopMoving();
			}
		}

		private bool GetBestDragPath(Vector3 _position, Vector3 _targetPosition, NavMeshPath _path)
		{
			if (!CorrectTargetDragPosition(ref _targetPosition))
			{
				return false;
			}
			Vector3 normalized = (_targetPosition - _position).normalized;
			Vector3 vector = normalized;
			vector.y = 0f;
			vector.Normalize();
			if (normalized.magnitude < float.Epsilon)
			{
				return false;
			}
			if (IsCrossingAnimusWalls(_position, normalized))
			{
				return false;
			}
			if (DisplayDebug)
			{
			}
			if (!NavigationHelper.SafeRaycast(_position, _targetPosition, out m_navMeshHit, NavMeshLayers.WalkableLayerMaskPreset))
			{
				return GetValidDragPath(_position, _targetPosition, _path);
			}
			Vector3 position = m_navMeshHit.position;
			Vector3 normal = m_navMeshHit.normal;
			if (DisplayDebug)
			{
			}
			ParcourPrediction parcourPrediction = AcierInputHandler.Instance.ParcourPrediction;
			parcourPrediction.UpdateParcourPrediction(m_inputVector.normalized, -1f);
			if ((Vector3.Distance(_position, position) > DragMaxEdgeDistance || parcourPrediction.HasInteraction) && GetValidDragPath(_position, position, _path))
			{
				return true;
			}
			if (!NavigationHelper.CascadingSamplePosition(_targetPosition, out m_navMeshHit, NavMeshLayers.WalkableLayerMaskPreset))
			{
				return false;
			}
			_targetPosition = m_navMeshHit.position;
			if (DisplayDebug)
			{
			}
			if (DisplayDebug)
			{
			}
			if (Vector3.Angle(_targetPosition - _position, normalized) > DragPathMaxSlideAngle)
			{
				return false;
			}
			return GetValidDragPath(_position, _targetPosition, _path);
		}

		private bool GetValidDragPath(Vector3 _position, Vector3 _targetPosition, NavMeshPath _path)
		{
			Vector3 vector = _targetPosition - _position;
			Vector3 normalized = vector.normalized;
			float magnitude = vector.magnitude;
			if (!NavMesh.CalculatePath(_position, _targetPosition, NavMeshLayers.WalkableLayerMaskPreset, _path))
			{
				return false;
			}
			if (_path.status == NavMeshPathStatus.PathInvalid || _path.status == NavMeshPathStatus.PathPartial)
			{
				return false;
			}
			if (_path.corners.Length >= 2 && Vector3.Angle(normalized, _path.corners[1] - _path.corners[0]) > DragMaxPathStartAngle)
			{
				return false;
			}
			if (_path.CalculateLength() > magnitude * DragPathTooLongThreshold)
			{
				return false;
			}
			return true;
		}

		private bool CorrectTargetDragPosition(ref Vector3 _targetPosition)
		{
			float num = 0.5f;
			if (Physics.Raycast(_targetPosition + Vector3.up * num, Vector3.down, out m_raycastHit, num * 2f))
			{
				_targetPosition = m_raycastHit.point;
			}
			return true;
		}

		private bool IsCrossingAnimusWalls(Vector3 _position, Vector3 _direction)
		{
			float backOffset = 1.5f;
			float upOffset = 0.5f;
			float num = 15f;
			RaycastHit _hit;
			if (ParcourHelper.CheckForAnimusWalls(_position, _direction, backOffset, upOffset, out _hit) || ParcourHelper.CheckForAnimusWalls(_position, Quaternion.Euler(0f, num, 0f) * _direction, backOffset, upOffset, out _hit) || ParcourHelper.CheckForAnimusWalls(_position, Quaternion.Euler(0f, 0f - num, 0f) * _direction, backOffset, upOffset, out _hit))
			{
				return true;
			}
			return false;
		}

		private bool MoveToTapPosition(Vector2 _screenPosition)
		{
			bool flag = false;
			if (CanAbortOtherInteractions(EInteractionType.Move))
			{
				flag = RaycastForTargetTapPosition(_screenPosition, ref m_raycastHit) && MoveToTargetTapPosition(m_raycastHit.point, m_raycastHit.normal, Vector3.zero, true);
				if (!flag)
				{
					MoveInterpreterDecals.Instance.NotifyInvalidPosition(m_raycastHit.point);
					StopMoving();
				}
				else
				{
					MoveInterpreterDecals.Instance.NotifyGoodPosition(m_raycastHit.point);
					m_stopMovingTime = float.MaxValue;
				}
				AbortOtherInteractions(EInteractionType.Move);
			}
			return flag;
		}

		public bool MoveToTargetTapPosition(Vector3 _targetPosition, Vector3 _targetNormal, Vector3 _facingDirection, bool _showDecals)
		{
			Vector3 position = IconInterpreter.NavMeshAgent.transform.position;
			bool flag = _facingDirection.magnitude > 0.5f;
			if (ParcourHelper.CanJumpToPoint(position, _targetPosition))
			{
				PlayerHelper.EnterParcour(new JumpAnywhereEvent(_targetPosition));
				return true;
			}
			bool _corrected = false;
			if (!CorrectTargetTapPosition(ref _targetPosition, ref _targetNormal, ref _corrected))
			{
				return false;
			}
			float magnitude = (_targetPosition - position).magnitude;
			if (magnitude >= TapMaxDistance)
			{
				return false;
			}
			if (!NavMesh.CalculatePath(position, _targetPosition, LogicConstants.AutoPassableNavMeshLayerMask, m_path) || m_path.status == NavMeshPathStatus.PathInvalid)
			{
				return false;
			}
			if (m_path.status == NavMeshPathStatus.PathPartial && (_targetPosition - m_path.corners[m_path.corners.Length - 1]).magnitude > TapMaxTargetOffset)
			{
				if (!NavigationHelper.SafeRaycast(position, _targetPosition, out m_navMeshHit, NavMeshLayers.WalkableLayerMaskPreset))
				{
					return false;
				}
				if (!NavMesh.CalculatePath(position, m_navMeshHit.position, NavMeshLayers.WalkableLayerMaskPreset, m_path) || m_path.status == NavMeshPathStatus.PathInvalid)
				{
					return false;
				}
			}
			if (_corrected && m_path.CalculateLength() > magnitude * TapPathTooLongThreshold)
			{
				return false;
			}
			if (m_path.corners.Length >= 2 && ParcourHelper.IsLeavingMissionArea(position, (m_path.corners[1] - m_path.corners[0]).normalized))
			{
				return false;
			}
			if (PlayerHelper.IsApproachingTarget)
			{
				PlayerHelper.Entity.StopApproaching(false);
			}
			if (!flag)
			{
				_facingDirection = ((m_path.corners.Length < 2) ? IconInterpreter.NavMeshAgent.transform.forward : (m_path.corners[m_path.corners.Length - 1] - m_path.corners[m_path.corners.Length - 2]).normalized);
			}
			UbiCore.Messenger.Messenger.Instance.Broadcast(new MoveToEvent(m_path, _facingDirection, MoveToEvent.EInputType.GroundTap, -1f, null, EDispatcherType.Tap, !_showDecals));
			MoveInterpreterDecals.Instance.Enabled = _showDecals;
			return true;
		}

		private bool CorrectTargetTapPosition(ref Vector3 _targetPosition, ref Vector3 _targetNormal, ref bool _corrected)
		{
			_corrected = false;
			if (NavMesh.SamplePosition(_targetPosition, out m_navMeshHit, TapNavMeshLookupRadius, NavMeshLayers.WalkableLayerMaskPreset))
			{
				_targetPosition = m_navMeshHit.position;
				return true;
			}
			_corrected = true;
			Vector3 vector = _targetPosition + 0.5f * _targetNormal + Vector3.up * TapMaxWallHeight;
			bool _hasBestPosition = false;
			Vector3 _bestPosition = vector;
			Vector3 position = IconInterpreter.NavMeshAgent.transform.position;
			float rayLength = vector.y - position.y - 1f;
			CorrectTargetTapPositionInternal(vector, rayLength, 0f, ref _bestPosition, ref _hasBestPosition);
			vector = _targetPosition - _targetNormal + Vector3.up * TapMaxWallHeight;
			rayLength = vector.y - position.y + 1f;
			CorrectTargetTapPositionInternal(vector, rayLength, 0.5f, ref _bestPosition, ref _hasBestPosition);
			if (!_hasBestPosition)
			{
				return false;
			}
			_targetPosition = _bestPosition;
			return true;
		}

		private void CorrectTargetTapPositionInternal(Vector3 _rayStart, float _rayLength, float _ratingOffset, ref Vector3 _bestPosition, ref bool _hasBestPosition)
		{
			RaycastHit[] array = Physics.RaycastAll(_rayStart, Vector3.down, _rayLength, Layers.StaticCollidersLayersMaskPreset);
			for (int i = 0; i < array.Length; i++)
			{
				RaycastHit raycastHit = array[i];
				if (!(raycastHit.point.y + _ratingOffset > _bestPosition.y) && NavMesh.SamplePosition(raycastHit.point, out m_navMeshHit, TapNavMeshLookupRadius, NavMeshLayers.WalkableLayerMaskPreset))
				{
					_bestPosition = m_navMeshHit.position;
					_hasBestPosition = true;
				}
			}
		}

		private void StopMoving()
		{
			PlayerHelper.StopMoving();
			MoveInterpreterDecals.Instance.Enabled = false;
		}

		private bool RaycastForTargetTapPosition(Vector2 _position, ref RaycastHit _hit)
		{
			Ray ray = Camera.main.ScreenPointToRay(_position);
			RaycastHit[] array = Physics.RaycastAll(ray, float.PositiveInfinity, Layers.StaticCollidersLayersMaskPreset);
			if (array != null && array.Length > 0)
			{
				BBSimpleList<RaycastHit> bBSimpleList = new BBSimpleList<RaycastHit>(array);
				bBSimpleList.Sort((RaycastHit o, RaycastHit p) => o.distance.CompareTo(p.distance));
				for (int i = 0; i < bBSimpleList.Count; i++)
				{
					RaycastHit raycastHit = bBSimpleList[i];
					if (!(raycastHit.collider.tag == "Interactable"))
					{
						_hit = raycastHit;
						return true;
					}
				}
			}
			return false;
		}
	}
}
