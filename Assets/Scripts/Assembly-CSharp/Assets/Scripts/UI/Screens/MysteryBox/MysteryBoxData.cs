using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Screens.MysteryBox
{
	public class MysteryBoxData
	{
		private List<InventoryElement> m_rolledElements;

		private MysteryBoxChain m_chain;

		private string m_boxId;

		private bool m_isPreRolled;

		public SimpleDelegate OnGeneralUpdate;

		public SimpleDelegate OnReset;

		public SimpleDelegate OnClaimedChain;

		public SimpleDelegate OnLootedAgain;

		public List<InventoryElement> RolledElements
		{
			get
			{
				return m_rolledElements;
			}
			set
			{
				m_rolledElements = value;
			}
		}

		public int OverallChainLenght
		{
			get
			{
				MysteryBoxChain chain = ((Chain == null) ? Singleton<StateManager>.Instance.CurrentChain : Chain);
				return chain.Info().chainLength + 1;
			}
		}

		public Resource OverspendingCost
		{
			get
			{
				MysteryBoxChain chain = ((Chain == null) ? Singleton<StateManager>.Instance.CurrentChain : Chain);
				return chain.Info().overspendingCost;
			}
		}

		public bool IsPreRolled
		{
			get
			{
				return m_isPreRolled;
			}
		}

		public Quality.EQuality RewardQuality
		{
			get
			{
				MysteryBoxChain chain = ((Chain == null) ? Singleton<StateManager>.Instance.CurrentChain : Chain);
				return chain.Info().bonusQuality;
			}
		}

		public EMysteryBoxType BoxType
		{
			get
			{
				return (m_chain == null) ? Singleton<StateManager>.Instance.CurrentChain.mysteryBoxType : m_chain.mysteryBoxType;
			}
		}

		public Quality.EQuality BoxQuality
		{
			get
			{
				return (m_chain == null) ? Singleton<StateManager>.Instance.CurrentChain.quality : m_chain.quality;
			}
		}

		public string BoxId
		{
			get
			{
				return m_boxId;
			}
		}

		public MysteryBoxChain Chain
		{
			get
			{
				return m_chain;
			}
		}

		public int MysteryBoxesLeft
		{
			get
			{
				return Singleton<StateManager>.Instance.ProfileManager.Profile.Inventory.GetAmountOfBoxesByTypeAndQuality(BoxType, BoxQuality);
			}
		}

		public bool Overspending
		{
			get
			{
				return MysteryBoxesLeft < 1;
			}
		}

		public MysteryBoxData()
		{
			OnGeneralUpdate = new SimpleDelegate(this, new Type[1] { typeof(MysteryBoxData) });
			OnClaimedChain = new SimpleDelegate(this, new Type[1] { typeof(MysteryBoxData) });
			OnLootedAgain = new SimpleDelegate(this, new Type[1] { typeof(MysteryBoxData) });
			OnReset = new SimpleDelegate(this, new Type[1] { typeof(MysteryBoxData) });
			m_chain = Singleton<StateManager>.Instance.CurrentChain;
			m_isPreRolled = m_chain != null && m_chain.bonus.rolledLoot != null;
			UpdateRolledItemsList();
		}

		public void LootAgain()
		{
			if (BoxType == EMysteryBoxType.MysteryBoxRessourceType)
			{
				Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.ProfileSyncEnabled = false;
			}
			bool newChain = m_chain == null;
			string boxId = string.Empty;
			if (Overspending)
			{
				boxId = string.Empty;
			}
			else if (InventoryExtensions.GetItemAmountWithId(Singleton<StateManager>.Instance.CurrentChain.lastUsedInventoryElementId) > 0)
			{
				boxId = Singleton<StateManager>.Instance.CurrentChain.lastUsedInventoryElementId;
			}
			else
			{
				boxId = Singleton<StateManager>.Instance.ProfileManager.Profile.Inventory.GetAllMysteryBoxesOfTypeAndQuality(BoxType, BoxQuality)[0].id;
			}
			EMysteryBoxType type = Singleton<StateManager>.Instance.CurrentChain.mysteryBoxType;
			Quality.EQuality quality = Singleton<StateManager>.Instance.CurrentChain.quality;
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncRequest)
			{
				Singleton<StateManager>.Instance.ProfileManager.OpenMysteryBox(quality, type, Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinId, boxId, newChain, Overspending, _asyncRequest);
			}, null, delegate(IAsyncResult _result)
			{
				if (!_result.IsError())
				{
					if (newChain)
					{
						UbiCore.Utils.Singleton<LocalStorage>.Instance.MysteryBoxSlotCount = -1;
					}
					if (BoxType == EMysteryBoxType.MysteryBoxRessourceType && Overspending)
					{
						Resource overspendingCost = Singleton<StateManager>.Instance.TemplateManager.MysteryBoxMapping.GetEntryByQualityAndType(BoxQuality, BoxType).overspendingCost;
						Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.SubtractAmount(overspendingCost.type, overspendingCost.amount);
					}
					m_chain = ((AsyncDataResult<OpenMysteryBoxResponse>)_result).Result().mysteryBoxChain;
					Singleton<StateManager>.Instance.CurrentChain = m_chain;
					UpdateRolledItemsList();
					OnLootedAgain.Trigger(this);
				}
			});
		}

		public void ClaimChain()
		{
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncRequest)
			{
				Singleton<StateManager>.Instance.ProfileManager.ClaimMysteryBox(_asyncRequest);
			}, null, delegate(IAsyncResult _result)
			{
				if (!_result.IsError())
				{
					OnClaimedChain.Trigger(this);
				}
			});
		}

		public void UpdateData()
		{
			if (Chain == null)
			{
				return;
			}
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncRequest)
			{
				Singleton<StateManager>.Instance.ProfileManager.GetMysteryBoxInfo(_asyncRequest);
			}, null, delegate(IAsyncResult _result)
			{
				if (!_result.IsError())
				{
					m_chain = ((AsyncDataResult<GetMysteryBoxInfoResponse>)_result).Result().mysteryBoxChain;
					UpdateRolledItemsList();
					OnGeneralUpdate.Trigger(this);
				}
			});
		}

		public void ResetData()
		{
			m_chain = null;
			m_boxId = string.Empty;
			OnReset.Trigger(this);
			UbiCore.Utils.Singleton<LocalStorage>.Instance.MysteryBoxSlotCount = -1;
		}

		public bool CanReciveMysteryBoxRewards()
		{
			return Singleton<StateManager>.Instance.ProfileManager.Profile.CanClaimMysteryBoxRewards(Singleton<StateManager>.Instance.CurrentChain);
		}

		private void UpdateRolledItemsList()
		{
			if (m_rolledElements != null)
			{
				m_rolledElements.Clear();
			}
			m_rolledElements = new List<InventoryElement>();
			for (int i = 0; i < m_chain.slots.Length; i++)
			{
				m_rolledElements.Add(m_chain.slots[i].rolledLoot[0].element);
			}
		}
	}
}
