using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Notifications
{
	public abstract class UINewsEvaluatorBase : INewsEvaluator
	{
		public delegate void HandleValueChange(bool _value);

		private const string c_defaultIconName = "aUI_icon_new";

		private bool m_value;

		public abstract ENewsType Type { get; }

		public virtual string DisplayIcon
		{
			get
			{
				return "aUI_icon_new";
			}
		}

		public bool Value
		{
			get
			{
				return m_value;
			}
			protected set
			{
				if (m_value != value && IsNewsTypeUnlocked())
				{
					m_value = value;
					if (this.OnValueChanged != null)
					{
						this.OnValueChanged(m_value);
					}
				}
			}
		}

		public event HandleValueChange OnValueChanged;

		public virtual void Init()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate.Add(OnProfileUpdated);
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.OnCategoryFinished += OnTutorialCategoryFinished;
		}

		public virtual void Destroy()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate.Remove(OnProfileUpdated);
			if (UbiCore.Utils.Singleton<UiMain>.IsAvailable && UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager != null)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.OnCategoryFinished -= OnTutorialCategoryFinished;
			}
		}

		public abstract void Update();

		protected virtual void OnProfileUpdated(params object[] _params)
		{
			Update();
		}

		private void OnTutorialCategoryFinished(ETutorialCategory _obj)
		{
			Update();
		}

		protected bool IsNewsTypeUnlocked()
		{
			switch (Type)
			{
			case ENewsType.Inventory:
				return UIButtonHeaderUtil.IsButtonHeaderTypeUnlocked(UIButtonHeader.EHeaderType.Assassin);
			case ENewsType.SkillPointsCurrent:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.SkillTreeEntitlementGUID);
			case ENewsType.Mail:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.SocialCategoryTabMailGUID);
			case ENewsType.Friends:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.SocialCategoryTabFriendsGUID);
			case ENewsType.Achievements:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.SocialCategoryTabAchievementsGUID);
			case ENewsType.Forge:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.FusionEntitlementGUID);
			default:
				return true;
			}
		}
	}
}
