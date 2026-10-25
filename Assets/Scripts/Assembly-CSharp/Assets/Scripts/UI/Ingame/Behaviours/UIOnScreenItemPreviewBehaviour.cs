using System;
using System.Collections;
using AcierProtocol;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIOnScreenItemPreviewBehaviour : UIBaseBehaviour
	{
		[SerializeField]
		private UnityEngine.Animation m_animation;

		[SerializeField]
		private UIExtendedLabel m_countLabel;

		[SerializeField]
		private UIExtendedLabel m_nameLabel;

		[SerializeField]
		private UIAlignmentBox m_labelAlignmentBox;

		[SerializeField]
		private UIRewardItemController m_rewardController;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.OnScreenPreview;
			}
		}

		public event Action Hidden;

		[ContextMenu("Show Debug preview")]
		public void ShowMessageDbug()
		{
			UIOnScreenItemPreview subSystemByType = UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenItemPreview>();
			BBList<IMissionBase> missionEntries = UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.MissionEntries;
			for (int i = 0; i < missionEntries.Count; i++)
			{
				ObjectiveLoot objectiveLoot = missionEntries[i] as ObjectiveLoot;
				if (!(objectiveLoot == null))
				{
					MissionGoalLoot _goalLoot = null;
					InteractableObject challengeLootChest = objectiveLoot.GetChallengeLootChest(ref _goalLoot);
					if (challengeLootChest != null)
					{
						subSystemByType.CreateMessagesDebug(_goalLoot);
						return;
					}
				}
			}
			MissionGoalLoot missionGoalLoot = new MissionGoalLoot();
			missionGoalLoot.elements = new InventoryElements();
			missionGoalLoot.resources = new ResourceFromTemplate[1]
			{
				new ResourceFromTemplate
				{
					quality = Quality.EQuality.Perfect,
					resource = new Resource
					{
						type = EResourceType.SoftCurrency,
						amount = 568L
					}
				}
			};
			MissionGoalLoot goalLoot = missionGoalLoot;
			subSystemByType.CreateMessagesDebug(goalLoot);
		}

		public void ShowMessage(UIOnScreenItemPreview.QueueItem _queueItem)
		{
			if (_queueItem != null)
			{
				UpdateRewardNameAndController(_queueItem);
				SetVisibility(true);
			}
		}

		private void UpdateRewardNameAndController(UIOnScreenItemPreview.QueueItem _queueItem)
		{
			if (_queueItem.Item != null)
			{
				Quality.EQuality itemDisplayQuality = _queueItem.Item.GetItemDisplayQuality();
				ColorDef colorDef = UIItemUtil.GetColorDefForQuality(itemDisplayQuality);
				if (_queueItem.Item.IsBlueprint())
				{
					colorDef = UIItemUtil.GetColorDefForColorGuid(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.BlueprintGlowColor);
				}
				else if (_queueItem.Item.IsCustomLoot())
				{
					colorDef = UIItemUtil.GetColorDefForColorGuid(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.LockpicksGlowColor);
				}
				bool flag = _queueItem.Item.stackSize > 1;
				m_countLabel.gameObject.SetActive(flag);
				if (flag)
				{
					m_countLabel.SetNumber(_queueItem.Item.stackSize);
				}
				m_nameLabel.text = _queueItem.Item.GetTitle();
				m_nameLabel.color = colorDef.Color0;
				m_rewardController.Initialize(_queueItem.Item);
			}
			else
			{
				m_countLabel.gameObject.SetActive(true);
				m_countLabel.color = _queueItem.LootQualityColor;
				m_countLabel.SetNumber(_queueItem.LootCount);
				m_nameLabel.color = _queueItem.LootQualityColor;
				m_nameLabel.text = _queueItem.LootName;
				m_rewardController.Initialize(_queueItem.GameID, _queueItem.LootQualityColor);
			}
			m_labelAlignmentBox.RepositionNow = true;
		}

		public override void SetVisibility(bool _visible)
		{
			base.SetVisibility(_visible);
			if (_visible)
			{
				base.gameObject.SetActive(true);
				StopAllCoroutines();
				StartCoroutine(OnShowPreview(m_animation));
			}
		}

		private IEnumerator OnShowPreview(UnityEngine.Animation _animation)
		{
			_animation.gameObject.SetActive(true);
			_animation.SampleAnimation(0f, string.Empty);
			UIWidget widget = _animation.gameObject.GetComponent<UIWidget>();
			if (widget != null)
			{
				widget.alpha = 0f;
			}
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			yield return wait;
			if (widget != null)
			{
				widget.alpha = 1f;
			}
			_animation.PlayFirstClip();
			while (_animation.isPlaying)
			{
				yield return wait;
			}
			OnHidden();
			base.gameObject.SetActive(false);
		}

		protected virtual void OnHidden()
		{
			if (this.Hidden != null)
			{
				this.Hidden();
			}
		}

		private void Update()
		{
			if (m_animation.isPlaying)
			{
				m_animation.UpdateAnimationSpeed();
			}
		}
	}
}
