using System;
using Assets.Scripts.UI.MVC.Controllers.Misc;

namespace Assets.Scripts.UI.MVC.Models.Events
{
	public class UIEventBaseModel : UIModelBase
	{
		public virtual int CurrentValue
		{
			get
			{
				return 0;
			}
		}

		public virtual int MaxValue
		{
			get
			{
				return 0;
			}
		}

		public virtual bool IsClaimed
		{
			get
			{
				return false;
			}
		}

		public virtual bool IsClaimable
		{
			get
			{
				return false;
			}
		}

		public virtual bool CanClaimItem
		{
			get
			{
				return false;
			}
		}

		public virtual string HeaderText
		{
			get
			{
				return string.Empty;
			}
		}

		public virtual string HeaderDescription
		{
			get
			{
				return string.Empty;
			}
		}

		public virtual bool IsLocked
		{
			get
			{
				return false;
			}
		}

		public virtual int RewardItemCount()
		{
			return 1;
		}

		public virtual void InitializeRewardItem(int _rewardIndex, UIRewardItemController _items)
		{
		}

		public virtual bool ShouldPlayClaimAnimationForReward(int _itemIndex)
		{
			return RewardItemCount() > _itemIndex;
		}

		public virtual void UpdateData()
		{
		}

		public virtual void OnClaimItem(Action<bool, bool, bool> _onItemClaimed)
		{
		}
	}
}
