using System;
using SharedBaseLib.System;
using UbiCore.Common;
using UbiCore.Messenger;
using UnityEngine;

namespace UbiCore.Mission.Motion
{
	public abstract class AbstractInterpreter : IInterpreter
	{
		public virtual bool InputEnabled { get; set; }

		public virtual float InputPriority { get; set; }

		protected AbstractInterpreter()
		{
			InputPriority = 1f;
		}

		public virtual IconMaterial GetDefaultActionIcon()
		{
			return IconMaterial.Invalid;
		}

		public virtual bool OnTriggerDefaultAction()
		{
			return false;
		}

		public virtual bool OnPrepareDefaultAction()
		{
			return false;
		}

		public virtual bool OnCancelDefaultAction()
		{
			return false;
		}

		public virtual bool OnTouch(TouchEvent _event)
		{
			return false;
		}

		public virtual bool OnDoubleTap(TapEvent _event)
		{
			return false;
		}

		public virtual bool OnTap(TapEvent _event)
		{
			return false;
		}

		public virtual bool OnDrag(DragEvent _event)
		{
			return false;
		}

		public virtual bool OnSelect(SelectEvent _event)
		{
			return false;
		}

		public virtual bool OnSecondTouch(TouchEvent _event)
		{
			return false;
		}

		public virtual bool OnSecondTap(TapEvent _event)
		{
			return false;
		}

		public virtual bool OnSecondDrag(DragEvent _event)
		{
			return false;
		}

		public virtual bool OnSecondSelect(SelectEvent _event)
		{
			return false;
		}

		public virtual bool OnSwipe(SwipeEvent _event)
		{
			return false;
		}

		public virtual bool OnTwist(TwistEvent _event)
		{
			return false;
		}

		public virtual bool OnPinch(PinchEvent _event)
		{
			return false;
		}

		public virtual void OnUpdate()
		{
		}

		protected bool RaycastForInteractableObject(Vector2 _position, ref RaycastHit _hit, Collider[] _excludeFromResult)
		{
			RaycastHit[] array = RaycastForInteractableObjectGetAll(_position);
			int num = 0;
			while (num < array.Length)
			{
				if (_excludeFromResult != null)
				{
					bool flag = false;
					for (int i = 0; i < _excludeFromResult.Length; i++)
					{
						if (_excludeFromResult[i] == array[num].collider)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						num++;
						continue;
					}
				}
				if (array[num].collider.tag != "Interactable")
				{
					break;
				}
				_hit = array[num];
				return true;
			}
			return false;
		}

		protected bool RaycastForInteractableObject(Vector2 _position, ref RaycastHit _hit)
		{
			return RaycastForInteractableObject(_position, ref _hit, null);
		}

		protected RaycastHit[] RaycastForInteractableObjectGetAll(Vector2 _position)
		{
			Ray ray = Camera.main.ScreenPointToRay(_position);
			RaycastHit[] array = Physics.RaycastAll(ray, float.PositiveInfinity, (int)Layers.InteractorsLayerMask | (int)Layers.StaticCollidersLayersMaskPreset);
			if (array != null && array.Length > 1)
			{
				Array.Sort(array, SortByDistanceAsc);
			}
			return array;
		}

		private static int SortByDistanceAsc(RaycastHit _left, RaycastHit _right)
		{
			return _left.distance.CompareTo(_right.distance);
		}
	}
}
