using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ChooseSecondClass;
using Assets.Scripts.UI.Screens.Popup.ErrorPopup;
using Assets.Scripts.UI.Screens.Popup.SkillPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using BB.Mobile.Net;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class SkillTreeDetails : UIScreenChild
	{
		public delegate void TrainOkInternal(SkillTreeData _data);

		public delegate void InstantTrainOkInternal(SkillTreeData _data);

		public delegate void TrainAssassin(string _assassinId, AssassinSkillInTraining.SkillClass _skillTierClass, int _tierId, AssassinSkillInTraining.SkillType _tierSkillType, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		[SerializeField]
		private SkillPointsHandler m_pointsHandler;

		[SerializeField]
		private TrainButtonHandler m_trainButtonHandler;

		[SerializeField]
		private TabHandler m_tabHandler;

		[SerializeField]
		private Panelhandler m_panelHandler;

		[SerializeField]
		private SkillTreeHandler m_treeHandler;

		[SerializeField]
		private SecondClassHandler m_classHandler;

		[SerializeField]
		private Color m_MaxLevelColor;

		[SerializeField]
		private Color m_NextLevelColor;

		[SerializeField]
		private Color m_ValueColor;

		[SerializeField]
		private UIExtendedButton m_buyButton;

		private SkillTreeData m_data;

		public List<EventDelegate> onChooseClass = new List<EventDelegate>();

		private TrainingCost m_trainCostOverride;

		private TrainOkInternal m_trainOkInternalOverride;

		private InstantTrainOkInternal m_instantTrainOkInternalOverride;

		private UIPopupResourcesContent.ResourceConfiguration m_instantTrainResourceConfigOverride;

		private bool m_activateScreenOnUpdate;

		private static bool IsSkillPointsTutorialActive
		{
			get
			{
				return UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatMs25SkillPoints);
			}
		}

		public TrainingCost TrainCostOverride
		{
			get
			{
				return (!IsSkillPointsTutorialActive) ? null : m_trainCostOverride;
			}
			set
			{
				m_trainCostOverride = value;
			}
		}

		public TrainOkInternal TrainOkInternalOverride
		{
			get
			{
				return (!IsSkillPointsTutorialActive) ? null : m_trainOkInternalOverride;
			}
			set
			{
				m_trainOkInternalOverride = value;
			}
		}

		public InstantTrainOkInternal InstantTrainOkInternalOverride
		{
			get
			{
				return (!IsSkillPointsTutorialActive) ? null : m_instantTrainOkInternalOverride;
			}
			set
			{
				m_instantTrainOkInternalOverride = value;
			}
		}

		public UIPopupResourcesContent.ResourceConfiguration InstantTrainResourceConfigOverride
		{
			get
			{
				return (!IsSkillPointsTutorialActive) ? null : m_instantTrainResourceConfigOverride;
			}
			set
			{
				m_instantTrainResourceConfigOverride = value;
			}
		}

		public bool ShowCloseButtonForTrainAndInstantPopup
		{
			get
			{
				return !IsSkillPointsTutorialActive;
			}
		}

		public bool UseProfileFacadeMock
		{
			get
			{
				return m_data.UseProfileFacadeMock;
			}
			set
			{
				m_data.UseProfileFacadeMock = value;
			}
		}

		public void ForceUpdateData()
		{
			m_data.Update();
		}

		public static string GetSkillTrackName(string _assassinId)
		{
			return string.Format("LastSkillTrain{0}", _assassinId);
		}

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeActivate);
			m_treeHandler.InitScreen();
		}

		protected override void OnScreenDestroy()
		{
			if (m_screen != null)
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
				EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeActivate);
			}
		}

		private void OnScreenActivate()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinDataChanged.Add(OnAssassinDataChanged);
			m_data = new SkillTreeData
			{
				NextLevelColor = m_NextLevelColor,
				MaxLevelColor = m_MaxLevelColor,
				ValueColor = m_ValueColor
			};
			m_activateScreenOnUpdate = true;
			ReloadScreen();
			EnableListeners();
		}

		private void OnScreenDeActivate()
		{
			StopAllCoroutines();
			DisableListeners();
			m_data.Clear();
			m_data = null;
			m_activateScreenOnUpdate = false;
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinDataChanged.Remove(OnAssassinDataChanged);
			PurgeScreen();
		}

		private void Update()
		{
			if (m_activateScreenOnUpdate && m_data != null)
			{
				m_activateScreenOnUpdate = false;
				ActivateScreen(m_data);
			}
		}

		private void OnResetPoints()
		{
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(1780),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						ResetPoints();
					},
					SoundIntent = EButtonIntent.Accept
				}
			};
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(2341);
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Buttons = buttons;
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = LocalizationManager.Instance.Get(2328)
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		public void OnTrain()
		{
			Assets.Scripts.UI.Utils.Extensions.SkillData selectedSkillData = m_data.SelectedSkillData;
			Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition definition = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetDefinition(m_data.SelectedSkill, (selectedSkillData.Level <= 0) ? 1 : selectedSkillData.Level);
			TrainingCost cost = TrainCostOverride ?? m_data.GetCost(selectedSkillData.SkillId, selectedSkillData.Level + 1, m_data.Assassin);
			ShowTrainPopup(definition, selectedSkillData, cost);
		}

		private void ShowTrainPopup(Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition _skillDef, Assets.Scripts.UI.Utils.Extensions.SkillData _skillData, TrainingCost _cost)
		{
			StringMapper stringMapper = new StringMapper();
			stringMapper.Add("skill_name", LocalizationManager.Instance.Get(_skillDef.NameLocaKey.Key));
			stringMapper.Add("rank", _skillData.Level);
			stringMapper.Add("next_rank", _skillData.Level + 1);
			StringMapper stringMapper2 = stringMapper;
			int num = 2343;
			if (!_skillData.Active)
			{
				AvatarClassDefinitions.AvatarClass.AvatarTier tierById = m_data.GetTierById(_skillData.TierId);
				Assets.Scripts.UI.Utils.Extensions.SkillData skill = m_data.GetSkill(tierById.passiveSkillOne.guid);
				Assets.Scripts.UI.Utils.Extensions.SkillData skill2 = m_data.GetSkill(tierById.passiveSkillTwo.guid);
				if (skill.Level == 0 && skill2.Level == 0)
				{
					Assets.Scripts.UI.Utils.Extensions.SkillData skillData = ((!(skill.SkillId == _skillData.SkillId)) ? skill : skill2);
					Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition definition = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetDefinition(skillData.SkillId, 1);
					stringMapper2["lock_name"] = LocalizationManager.Instance.Get(definition.NameLocaKey.Key);
					num = 4666;
				}
			}
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			Resource[] resources = _cost.Resources;
			Array.Sort(resources, UIUtil.SortResourcesByType);
			List<UIPopupResourcesContent.ResourceConfiguration> list = new List<UIPopupResourcesContent.ResourceConfiguration>(resources.Length);
			PopUpButtonConfiguration[] array = new PopUpButtonConfiguration[resources.Length];
			for (int i = 0; i < resources.Length; i++)
			{
				UIPopupResourcesContent.ResourceConfiguration resConfig = new UIPopupResourcesContent.ResourceConfiguration
				{
					HasEnoughResources = profile.HasEnoughResources(resources[i]),
					Resource = resources[i]
				};
				PopUpButtonConfiguration popUpButtonConfiguration = UIBuyHelper.CreateResourceCostButton(resConfig, 1780, delegate
				{
					UIBuyHelper.DetermineAllowPurches(resConfig.Resource, Train, null);
				});
				popUpButtonConfiguration.SoundIntent = EButtonIntent.Buy;
				array[i] = popUpButtonConfiguration;
				list.Add(resConfig);
			}
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.ShowCloseButton = ShowCloseButtonForTrainAndInstantPopup;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(3597);
			popUpConfiguration.Buttons = array;
			popUpConfiguration.Data = new SkillPopupContent.SkillPopupData
			{
				Description = LocalizationManager.Instance.Get(num, stringMapper2),
				ResourcesCost = list,
				ResourceSymbolAdditive = true,
				SkillData = new Assets.Scripts.UI.Utils.Extensions.SkillData[1] { _skillData }
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private void OnSelectSecondClass()
		{
			bool flag = m_data.SecondaryClass != Assassin.EClassTypeId.None;
			m_data.IsSecondClassPreviewShown = !flag;
			m_classHandler.Visible = !flag;
			m_treeHandler.Visible = flag;
			m_pointsHandler.UpdateHandler();
		}

		private void OnSecondClassSelected()
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			SecondSkillTreeCostMapping.SecondSkillTreeCostEntry secondClassUnlockCost = Singleton<StateManager>.Instance.ProfileManager.Profile.GetSecondClassUnlockCost();
			List<PopUpButtonConfiguration> list = new List<PopUpButtonConfiguration>();
			if (secondClassUnlockCost == null || m_data.PreviewClass == Assassin.EClassTypeId.None)
			{
				return;
			}
			Resource[] resources = secondClassUnlockCost.costOptions.resources;
			Array.Sort(resources, UIUtil.SortResourcesByType);
			List<UIPopupResourcesContent.ResourceConfiguration> list2 = new List<UIPopupResourcesContent.ResourceConfiguration>(resources.Length);
			ClassDescription classDescription = UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions[(int)m_data.PreviewClass];
			UIPopup.EContentAlignTarget aligntarget = UIPopup.EContentAlignTarget.CloseButtonOverlay;
			for (int i = 0; i < resources.Length; i++)
			{
				bool hasEnoughResources = profile.HasEnoughResources(resources[i]);
				UIPopupResourcesContent.ResourceConfiguration resConfig = new UIPopupResourcesContent.ResourceConfiguration
				{
					HasEnoughResources = hasEnoughResources,
					Resource = resources[i]
				};
				PopUpButtonConfiguration popUpButtonConfiguration = UIBuyHelper.CreateResourceCostButton(resConfig, 679, delegate
				{
					UIBuyHelper.DetermineAllowPurches(resConfig.Resource, delegate
					{
						SetSecondClassAfterConfirmation(resConfig.Resource.type);
					}, null);
				});
				popUpButtonConfiguration.SoundIntent = EButtonIntent.Buy;
				list.Add(popUpButtonConfiguration);
				list2.Add(resConfig);
			}
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel);
			};
			popUpConfiguration.Aligntarget = aligntarget;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(7925, new StringMapper { 
			{
				"class_name",
				LocalizationManager.Instance.Get(classDescription.NameId)
			} });
			popUpConfiguration.Buttons = list.ToArray();
			popUpConfiguration.Data = new ChooseSecondClassContent.ChooseSecondClassData
			{
				Description = LocalizationManager.Instance.Get(4529),
				ClassDescription = classDescription,
				ResourcesCost = list2
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private void SetSecondClassAfterConfirmation(EResourceType _resourceType)
		{
			AsyncCallback callback = delegate
			{
				m_data.IsSecondClassPreviewShown = false;
				m_classHandler.Visible = false;
				EventDelegate.Execute(onChooseClass);
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncCallback)
			{
				Singleton<StateManager>.Instance.ProfileManager.SetExtendedClass(m_data.Assassin.id, m_data.PreviewClass, _resourceType, _asyncCallback);
			}, null, callback);
		}

		private void OnAssassinDataChanged(params object[] _params)
		{
			ReloadScreen();
		}

		private void OnApplicationPause(bool _pauseStatus)
		{
			if (!_pauseStatus)
			{
				ReloadScreen();
			}
		}

		private void OnClassSelectionChange()
		{
			m_classHandler.Visible = false;
			m_treeHandler.Visible = true;
			m_data.IsSecondClassPreviewShown = false;
			m_data.SelectedSkill = null;
		}

		private void OnUnlockTier(int _tierIndex)
		{
			string assassin = m_data.Assassin.id;
			AvatarClassDefinitions.AvatarClass.AvatarTier tier = m_data.GetAvatarTierByIndex(_tierIndex);
			AssassinSkillInTraining.SkillClass classType = ((!m_data.PrimarySelected) ? AssassinSkillInTraining.SkillClass.ExtendedClass : AssassinSkillInTraining.SkillClass.BaseClass);
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			Resource[] resources = tier.tierPayUnlockCosts.resources;
			Array.Sort(resources, UIUtil.SortResourcesByType);
			PopUpButtonConfiguration[] array = new PopUpButtonConfiguration[resources.Length];
			List<UIPopupResourcesContent.ResourceConfiguration> list = new List<UIPopupResourcesContent.ResourceConfiguration>(resources.Length);
			for (int i = 0; i < resources.Length; i++)
			{
				UIPopupResourcesContent.ResourceConfiguration resConfig = new UIPopupResourcesContent.ResourceConfiguration
				{
					HasEnoughResources = profile.HasEnoughResources(resources[i]),
					Resource = resources[i]
				};
				PopUpButtonConfiguration popUpButtonConfiguration = UIBuyHelper.CreateResourceCostButton(resConfig, 1780, delegate
				{
					UIBuyHelper.DetermineAllowPurches(resConfig.Resource, delegate
					{
						UnlockTierAsync(assassin, tier.id, classType);
					}, null);
				});
				popUpButtonConfiguration.SoundIntent = EButtonIntent.Buy;
				array[i] = popUpButtonConfiguration;
				list.Add(resConfig);
			}
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(3506);
			popUpConfiguration.Data = new SkillPopupContent.SkillPopupData
			{
				Description = LocalizationManager.Instance.Get(3507),
				ResourcesCost = list,
				SkillData = m_data.GetSkills(m_data.PrimarySelected, _tierIndex),
				ResourceSymbolAdditive = false
			};
			popUpConfiguration.Buttons = array;
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private void OnUnlockSkill()
		{
			Assets.Scripts.UI.Utils.Extensions.SkillData selectedSkillData = m_data.SelectedSkillData;
			Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition definition = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetDefinition(m_data.SelectedSkill, (selectedSkillData.Level <= 0) ? 1 : selectedSkillData.Level);
			AvatarClassDefinitions.AvatarClass.AvatarTier tierById = m_data.GetTierById(selectedSkillData.TierId);
			string assassin = m_data.Assassin.id;
			int tierId = selectedSkillData.TierId;
			AssassinSkillInTraining.SkillClass classType = m_data.Assassin.GetSkillClass(selectedSkillData.SkillId);
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			Resource[] resources = tierById.secondPassivePayUnlockCosts.resources;
			Array.Sort(resources, UIUtil.SortResourcesByType);
			PopUpButtonConfiguration[] array = new PopUpButtonConfiguration[resources.Length];
			List<UIPopupResourcesContent.ResourceConfiguration> list = new List<UIPopupResourcesContent.ResourceConfiguration>(resources.Length);
			for (int i = 0; i < resources.Length; i++)
			{
				UIPopupResourcesContent.ResourceConfiguration resConfig = new UIPopupResourcesContent.ResourceConfiguration
				{
					HasEnoughResources = profile.HasEnoughResources(resources[i]),
					Resource = resources[i]
				};
				PopUpButtonConfiguration popUpButtonConfiguration = UIBuyHelper.CreateResourceCostButton(resConfig, 1780, delegate
				{
					UIBuyHelper.DetermineAllowPurches(resConfig.Resource, delegate
					{
						UnlockSkillAsync(assassin, tierId, classType);
					}, null);
				});
				popUpButtonConfiguration.SoundIntent = EButtonIntent.Buy;
				array[i] = popUpButtonConfiguration;
				list.Add(resConfig);
			}
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(definition.NameLocaKey.Key);
			popUpConfiguration.Buttons = array;
			popUpConfiguration.Data = new SkillPopupContent.SkillPopupData
			{
				Description = LocalizationManager.Instance.Get(2118),
				ResourcesCost = list,
				SkillData = new Assets.Scripts.UI.Utils.Extensions.SkillData[1] { selectedSkillData },
				ResourceSymbolAdditive = true
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private void EnableListeners()
		{
			EventDelegate.Add(m_pointsHandler.onResetPoints, OnResetPoints);
			EventDelegate.Add(m_trainButtonHandler.onTrain, OnTrain);
			EventDelegate.Add(m_tabHandler.onSelectSecondClass, OnSelectSecondClass);
			EventDelegate.Add(m_buyButton.onClick, OnSecondClassSelected);
			EventDelegate.Add(m_data.onClassSelectionChanged, OnClassSelectionChange);
			EventDelegate.Add(m_trainButtonHandler.onUnlockSkill, OnUnlockSkill);
			m_treeHandler.OnUnlockHandler = OnUnlockTier;
		}

		private void DisableListeners()
		{
			EventDelegate.Remove(m_pointsHandler.onResetPoints, OnResetPoints);
			EventDelegate.Remove(m_trainButtonHandler.onTrain, OnTrain);
			EventDelegate.Remove(m_tabHandler.onSelectSecondClass, OnSelectSecondClass);
			EventDelegate.Remove(m_buyButton.onClick, OnSecondClassSelected);
			EventDelegate.Remove(m_data.onClassSelectionChanged, OnClassSelectionChange);
			EventDelegate.Remove(m_trainButtonHandler.onUnlockSkill, OnUnlockSkill);
			m_treeHandler.OnUnlockHandler = null;
		}

		private void ReloadScreen()
		{
			m_data.Update();
		}

		private void ActivateScreen(SkillTreeData _data)
		{
			m_tabHandler.Init(_data, this);
			m_pointsHandler.Init(_data);
			m_trainButtonHandler.Init(_data);
			m_panelHandler.Init(_data, m_tabHandler);
			m_treeHandler.Init(_data);
			m_classHandler.Init(_data);
		}

		private void PurgeScreen()
		{
			m_pointsHandler.Clear();
			m_trainButtonHandler.Clear();
			m_tabHandler.Clear();
			m_panelHandler.Clear();
			m_treeHandler.Clear();
			m_classHandler.Clear();
		}

		private void Train()
		{
			if (TrainOkInternalOverride != null)
			{
				TrainOkInternalOverride(m_data);
			}
			else
			{
				TrainSelectedSkill(Singleton<StateManager>.Instance.ProfileManager.TrainAssassin, OnTrainStarted);
			}
		}

		public void TrainAssassinDuringTutorial(AsyncCallback _onTrainFinished)
		{
			TrainSelectedSkill(Singleton<StateManager>.Instance.ProfileManager.TrainAssassinDuringTutorial, _onTrainFinished);
		}

		private void TrainSelectedSkill(TrainAssassin _trainFuntion, AsyncCallback _onTrain)
		{
			string assassinId = m_data.Assassin.id;
			UISkillId selectedSkill = m_data.SelectedSkill;
			AssassinSkillInTraining.SkillClass skillClass = m_data.Assassin.GetSkillClass(selectedSkill);
			int tierid = m_data.Assassin.GetTierId(selectedSkill);
			AssassinSkillInTraining.SkillType skillType = m_data.Assassin.GetSkillType(selectedSkill);
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _result)
			{
				_trainFuntion(assassinId, skillClass, tierid, skillType, _result);
			}, null, _onTrain);
		}

		private void OnTrainStarted(IAsyncResult _ar)
		{
			if (!_ar.IsError())
			{
				m_data.SelectedSkillTrained();
			}
		}

		private void ResetPoints()
		{
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.ResetSkillTree(Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinId, (!m_tabHandler.PrimaryTabIsActive) ? AssassinSkillInTraining.SkillClass.ExtendedClass : AssassinSkillInTraining.SkillClass.BaseClass, _asyncResult);
			}, null, OnResetSkillPointsCallback);
		}

		private void UnlockTierAsync(UIAssasinId _assassin, int _tierId, AssassinSkillInTraining.SkillClass _class)
		{
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.UnlockTier(_assassin, _tierId, _class, _asyncResult);
			}, null, OnUnlockTierCallback);
		}

		private void UnlockSkillAsync(UIAssasinId _assassin, int _tierId, AssassinSkillInTraining.SkillClass _class)
		{
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.UnlockPassiveSkill(_assassin, _tierId, _class, _asyncResult);
			}, null, OUnlockPassiveSkillCallback);
		}

		private void OUnlockPassiveSkillCallback(IAsyncResult _ar)
		{
			AsyncDataResult<UnlockPassiveSkillResponse> asyncDataResult = (AsyncDataResult<UnlockPassiveSkillResponse>)_ar;
			if (asyncDataResult.Exception == null)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4571));
			}
		}

		private void OnUnlockTierCallback(IAsyncResult _ar)
		{
			AsyncDataResult<UnlockTierResponse> asyncDataResult = (AsyncDataResult<UnlockTierResponse>)_ar;
			if (asyncDataResult.Exception == null)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4572));
			}
		}

		private void OnResetSkillPointsCallback(IAsyncResult _ar)
		{
			AsyncDataResult<ResetSkillTreeResponse> asyncDataResult = (AsyncDataResult<ResetSkillTreeResponse>)_ar;
			if (asyncDataResult.Exception == null)
			{
				m_treeHandler.Init(m_data);
				m_pointsHandler.Init(m_data);
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4574));
			}
		}
	}
}
