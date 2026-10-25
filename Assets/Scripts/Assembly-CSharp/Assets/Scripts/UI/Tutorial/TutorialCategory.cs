using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Tutorial.Flow;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.Tutorial
{
	public class TutorialCategory
	{
		public class LessComparer : IComparer<ITutorialFlowable>
		{
			public int Compare(ITutorialFlowable _a, ITutorialFlowable _b)
			{
				return (_a.Index < _b.Index) ? (-1) : ((_a.Index > _b.Index) ? 1 : 0);
			}
		}

		private readonly BBList<ITutorialFlowable> m_tutorials = new BBList<ITutorialFlowable>();

		private readonly LessComparer m_comparer = new LessComparer();

		private readonly ETutorialCategory m_category;

		public ETutorialCategory Category
		{
			get
			{
				return m_category;
			}
		}

		public STutorialFlowId CurrentFlowId { get; private set; }

		public bool Finished { get; private set; }

		public int Index { get; set; }

		public bool AnyFlowWasActive { get; private set; }

		public ReadOnlyCollection<ITutorialFlowable> Tutorials
		{
			get
			{
				return m_tutorials.AsReadOnly();
			}
		}

		public TutorialCategory(ETutorialCategory _category)
		{
			m_category = _category;
			Index = -1;
		}

		public void Init(AcierProtocol.Tutorial.Category _category = null)
		{
			Finished = TutorialHelper.OwningFinishedEntitlements(Category);
			CurrentFlowId = STutorialFlowId.Invalid;
		}

		public void Add(ITutorialFlowable _tutorial)
		{
			m_tutorials.Add(_tutorial);
			m_tutorials.Sort(m_comparer);
		}

		public void Remove(ITutorialFlowable _tutorial)
		{
			_tutorial.Deactivate();
			m_tutorials.Remove(_tutorial);
			m_tutorials.Sort(m_comparer);
		}

		public void Purge()
		{
			for (int i = 0; i < m_tutorials.Count; i++)
			{
				if (m_tutorials[i] != null)
				{
					m_tutorials[i].Deactivate();
				}
			}
			m_tutorials.Clear();
		}

		public bool SetFirstAvailableStep()
		{
			return NextStep(STutorialFlowId.Invalid);
		}

		public bool SetCurrentStep(STutorialFlowId _index)
		{
			STutorialFlowId currentFlowId = CurrentFlowId;
			BBList<ITutorialFlowable> bBList = FindAll(currentFlowId);
			BBList<ITutorialFlowable> bBList2 = FindAll(_index);
			if (bBList2.Count > 0)
			{
				CurrentFlowId = _index;
				if (bBList.Count > 0)
				{
					Deactivate(bBList);
				}
				if (Activate(bBList2))
				{
					int num = CurrentFlowId;
					ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
					AcierProtocol.Tutorial.Category profileTutorialCategory = TutorialHelper.GetProfileTutorialCategory(profileManager.Profile, Category);
					if (profileTutorialCategory == null || profileTutorialCategory.counter != num)
					{
						profileManager.SetTutorialStep(Category, num);
					}
					return true;
				}
			}
			CurrentFlowId = currentFlowId;
			return false;
		}

		public void SetFinished(AsyncCallback _callback)
		{
			BBList<ITutorialFlowable> bBList = FindAll(CurrentFlowId);
			if (bBList.Count > 0)
			{
				Deactivate(bBList);
			}
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			if (!TutorialHelper.OwningFinishedEntitlements(Category))
			{
				UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncCallback)
				{
					profileManager.SetTutorialFinished(Category, _asyncCallback);
				}, null, delegate(IAsyncResult _ar)
				{
					OnSetFinished(_ar, _callback);
				});
			}
			else
			{
				OnSetFinished(null, _callback);
			}
		}

		private void OnSetFinished(IAsyncResult _ar, AsyncCallback _callback)
		{
			if (_ar == null || !_ar.IsError())
			{
				CurrentFlowId = STutorialFlowId.Invalid;
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				_callback(_ar);
			}
		}

		public bool NextStep(STutorialFlowId _current)
		{
			bool flag = false;
			if (m_tutorials.Count > 0)
			{
				STutorialFlowId sTutorialFlowId = ((!(_current == STutorialFlowId.Invalid)) ? FindNext(_current) : new STutorialFlowId(0));
				if (sTutorialFlowId != STutorialFlowId.Invalid)
				{
					flag = SetCurrentStep(sTutorialFlowId);
					if (!flag)
					{
						flag = NextStep(sTutorialFlowId);
					}
				}
			}
			else
			{
				CurrentFlowId = STutorialFlowId.Invalid;
			}
			return flag;
		}

		private STutorialFlowId GetMinimumStep()
		{
			return (!(CurrentFlowId < 0L)) ? CurrentFlowId : new STutorialFlowId(0);
		}

		private STutorialFlowId GetMaximumStep()
		{
			return FlowIdHelper.GetMaxOfCategory(Category).ToTutorialFlowId();
		}

		private bool Activate(BBList<ITutorialFlowable> _tutorials)
		{
			bool flag = true;
			for (int i = 0; i < _tutorials.Count; i++)
			{
				ITutorialFlowable tutorialFlowable = _tutorials[i];
				if (!tutorialFlowable.ConditionAllowShow())
				{
					flag = false;
				}
			}
			if (flag)
			{
				for (int j = 0; j < _tutorials.Count; j++)
				{
					_tutorials[j].Activate();
					AnyFlowWasActive = true;
				}
			}
			return flag;
		}

		private void Deactivate(BBList<ITutorialFlowable> _tutorials)
		{
			for (int i = 0; i < _tutorials.Count; i++)
			{
				_tutorials[i].Deactivate();
			}
		}

		private BBList<ITutorialFlowable> FindAll(STutorialFlowId _id)
		{
			BBList<ITutorialFlowable> bBList = new BBList<ITutorialFlowable>();
			for (int i = 0; i < m_tutorials.Count; i++)
			{
				if (m_tutorials[i].Index == _id)
				{
					bBList.Add(m_tutorials[i]);
				}
			}
			return bBList;
		}

		private STutorialFlowId FindNext(STutorialFlowId _old)
		{
			for (int i = 0; i < m_tutorials.Count; i++)
			{
				if (_old < m_tutorials[i].Index)
				{
					return m_tutorials[i].Index;
				}
			}
			return STutorialFlowId.Invalid;
		}

		public bool GetIsStepActive()
		{
			bool flag = false;
			for (int i = 0; i < m_tutorials.Count; i++)
			{
				if (flag)
				{
					break;
				}
				flag = m_tutorials[i].IsActive;
			}
			return flag;
		}

		public bool IsAtFlowId(int _flowId)
		{
			return !Finished && CurrentFlowId == _flowId;
		}

		public static bool operator ==(TutorialCategory _a, TutorialCategory _b)
		{
			if (object.ReferenceEquals(_a, _b))
			{
				return true;
			}
			if (object.ReferenceEquals(_a, null) || object.ReferenceEquals(_b, null))
			{
				return false;
			}
			return false;
		}

		public static bool operator !=(TutorialCategory _a, TutorialCategory _b)
		{
			return !(_a == _b);
		}

		public static bool operator <(TutorialCategory _a, TutorialCategory _b)
		{
			if (_a == _b)
			{
				return false;
			}
			if (_a == null)
			{
				return true;
			}
			if (_b == null)
			{
				return false;
			}
			return _a.Index < _b.Index;
		}

		public static bool operator >(TutorialCategory _a, TutorialCategory _b)
		{
			if (_a == _b)
			{
				return false;
			}
			if (_b == null)
			{
				return true;
			}
			if (_a == null)
			{
				return false;
			}
			return _a.Index > _b.Index;
		}

		public static bool operator <=(TutorialCategory _a, TutorialCategory _b)
		{
			return _a < _b || _a == _b;
		}

		public static bool operator >=(TutorialCategory _a, TutorialCategory _b)
		{
			return _a > _b || _a == _b;
		}
	}
}
