using System;
using System.Collections.ObjectModel;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Selection;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Ingame;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using SharedBaseLib.System.Extensions;
using UbiCore.Messenger;
using UbiCore.Mission.Motion;
using UbiCore.Utils;
using UnityEngine;

namespace UbiCore.Acier
{
	public class IconInterpreter : AbstractInterpreter
	{
		protected class SelectionCandidate
		{
			public ISelectable Entity;

			public Vector2 ScreenPosition;
		}

		public bool IsReady
		{
			get
			{
				return NavMeshAgent != null;
			}
		}

		public static NavMeshAgent NavMeshAgent
		{
			get
			{
				return (LogicMain.PlayerEntity != null) ? LogicMain.PlayerEntity.UnityComponentCache.NavMeshAgent : null;
			}
		}

		public static PlayerCharacter Entity
		{
			get
			{
				return LogicMain.PlayerEntity;
			}
		}

		private static int raycastOrder(RaycastHit _left, RaycastHit _right)
		{
			return _left.distance.CompareTo(_right.distance);
		}

		public override IconMaterial GetDefaultActionIcon()
		{
			IconDisplay defaultIconDisplay = GetDefaultIconDisplay();
			if (defaultIconDisplay == null)
			{
				return IconMaterial.Invalid;
			}
			return defaultIconDisplay.GetIcon();
		}

		public override bool OnTriggerDefaultAction()
		{
			if (base.OnTriggerDefaultAction())
			{
				return true;
			}
			IconDisplay defaultIconDisplay = GetDefaultIconDisplay();
			if (defaultIconDisplay != null && defaultIconDisplay.OnTap(NavMeshAgent))
			{
				return true;
			}
			return false;
		}

		public override bool OnSecondTap(TapEvent _event)
		{
			return OnTap(_event);
		}

		public override bool OnTap(TapEvent _event)
		{
			return EvaluateIconTap(_event.ScreenPosition);
		}

		public override void OnUpdate()
		{
			MoveInterpreterDecals.Instance.Update();
		}

		private bool EvaluateIconTap(Vector2 _position)
		{
			bool flag = false;
			Camera uICamera = UbiCore.Utils.Singleton<UiMain>.Instance.UICamera;
			Ray ray = uICamera.ScreenPointToRay(_position);
			ray.origin -= ray.direction * 500f;
			RaycastHit[] array = Physics.RaycastAll(ray, float.PositiveInfinity, Layers.InteractableUiLayerMask.value);
			if (array != null && array.Length > 0)
			{
				Array.Sort(array, raycastOrder);
				for (int i = 0; i < array.Length; i++)
				{
					if (flag)
					{
						break;
					}
					if (array[i].collider.gameObject.layer == Layers.InteractableUiLayer)
					{
						IconDisplay component = array[i].collider.gameObject.GetComponent<IconDisplay>();
						if (component != null)
						{
							NavMeshAgent navMeshAgent = NavMeshAgent;
							flag = component.OnTap(navMeshAgent);
						}
					}
				}
				flag = true;
			}
			return flag;
		}

		private IconDisplay GetDefaultIconDisplay()
		{
			ReadOnlyCollection<IconDisplay> readOnlyInstances = IconDisplay.ReadOnlyInstances;
			float num = float.MaxValue;
			IconDisplay result = null;
			for (int i = 0; i < readOnlyInstances.Count; i++)
			{
				IconDisplay iconDisplay = readOnlyInstances[i];
				if (!IsIconInteractable(iconDisplay))
				{
					continue;
				}
				EntityIcon entityIcon = iconDisplay.IconAttachment as EntityIcon;
				if (entityIcon != null)
				{
					EntityId selectedEntityId = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.SelectedEntityId;
					if (entityIcon.EntityId.Equals(selectedEntityId))
					{
						return iconDisplay;
					}
				}
				float magnitude = (iconDisplay.transform.position - new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f)).magnitude;
				if (magnitude < num)
				{
					num = magnitude;
					result = iconDisplay;
				}
			}
			return result;
		}

		protected static bool IsIconInteractable(IconDisplay _icon, bool _checkVisible = true)
		{
			if (!_icon.gameObject.activeSelf)
			{
				return false;
			}
			SmartIcon smartIcon = _icon.IconAttachment as SmartIcon;
			if (smartIcon != null)
			{
				if (_checkVisible && !smartIcon.Visible)
				{
					return false;
				}
				if (smartIcon.IsHiddenOnBorders && smartIcon.IsOnBorder)
				{
					return false;
				}
				if (smartIcon.TopPriority < 4)
				{
					return false;
				}
			}
			if (!_icon.IconAttachment.IsInteractable || !_icon.IconAttachment.HasInteractAction)
			{
				return false;
			}
			Texture2D texture = UbiCore.Utils.Singleton<IconManager>.Instance.GetTexture(_icon.GetIcon());
			return texture != null;
		}

		protected void GetObjectsOnScreen(BBSimpleList<SelectionCandidate> _results)
		{
			bool flag = false;
			ISelectable selection = UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection;
			ReadOnlyCollection<IconDisplay> readOnlyInstances = IconDisplay.ReadOnlyInstances;
			for (int i = 0; i < readOnlyInstances.Count; i++)
			{
				IconDisplay iconDisplay = readOnlyInstances[i];
				if (!IsIconInteractable(iconDisplay, false))
				{
					continue;
				}
				EntityIcon entityIcon = iconDisplay.IconAttachment as EntityIcon;
				if (entityIcon == null)
				{
					continue;
				}
				ISelectable selectable = entityIcon.EntityId.TryGetEntity<ISelectable>();
				if (selectable != null)
				{
					if (selectable == selection)
					{
						flag = true;
					}
					_results.Add(new SelectionCandidate
					{
						Entity = selectable,
						ScreenPosition = iconDisplay.transform.position
					});
				}
			}
			Vector3 position = LogicMain.PlayerEntity.Position;
			Camera main = Camera.main;
			Vector3 position2 = main.transform.position;
			Collider[] array = Physics.OverlapSphere(position, 20f, Layers.InteractorsLayerMask);
			foreach (Collider collider in array)
			{
				if (collider.tag != "Interactable")
				{
					continue;
				}
				Bounds bounds = collider.bounds;
				Vector3 center = bounds.center;
				center = main.WorldToScreenPoint(center);
				if (center.x < 0f || center.x > (float)main.pixelWidth || center.y < 0f || center.y > (float)main.pixelHeight || center.z < main.nearClipPlane || center.z > main.farClipPlane)
				{
					continue;
				}
				Character entity = collider.gameObject.TryGetEntity() as Character;
				if (entity == null || !entity.IsSelectable() || !PlayerHelper.AssassinInteraction.CanInteractWithNpc(entity.Id) || _results.FindIndex((SelectionCandidate _c) => _c.Entity == entity) >= 0)
				{
					continue;
				}
				Vector3 vector = bounds.center + Vector3.up * bounds.size.y * 0.45f;
				Vector3 vector2 = position2 - vector;
				float magnitude = vector2.magnitude;
				RaycastHit hitInfo;
				if (!(magnitude < 0.001f) && !Physics.Raycast(vector, vector2 / magnitude, out hitInfo, magnitude, Layers.StaticCollidersLayersMaskPreset))
				{
					if (entity == selection)
					{
						flag = true;
					}
					_results.Add(new SelectionCandidate
					{
						Entity = entity,
						ScreenPosition = center
					});
				}
			}
			if (!flag && selection != null)
			{
				_results.Add(new SelectionCandidate
				{
					Entity = selection,
					ScreenPosition = main.WorldToScreenPoint(selection.Position)
				});
			}
		}
	}
}
