using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Database
{
	public class CTADefinitionUnspentSkillPoints : CTADefinitionBase
	{
		private UIAssasinId m_switchToAssassinId;

		public int UsedSPThreshold { get; protected set; }

		public int UnusedSPNeverUsed { get; protected set; }

		public int UnusedSPNormal { get; protected set; }

		public int UnusedSPRandomAssassin { get; protected set; }

		public CTADefinitionUnspentSkillPoints(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			UsedSPThreshold = _s.SerializeInt("UsedSPThreshold", UsedSPThreshold);
			UnusedSPNeverUsed = _s.SerializeInt("UnusedSPNeverUsed", UnusedSPNeverUsed);
			UnusedSPNormal = _s.SerializeInt("UnusedSPNormal", UnusedSPNormal);
			UnusedSPRandomAssassin = _s.SerializeInt("UnusedSPRandomAssassin", UnusedSPRandomAssassin);
		}

		protected override EResult CheckTrigger()
		{
			m_switchToAssassinId = null;
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			Assassin[] assassins = profileManager.Profile.assassins.assassins;
			Assassin assassin = null;
			Assassin assassin2 = null;
			foreach (Assassin assassin3 in assassins)
			{
				if (assassin2 == null || assassin3.skillPoints > assassin2.skillPoints)
				{
					assassin2 = assassin3;
				}
				if (assassin3.id == profileManager.CurrentAssassinId)
				{
					assassin = assassin3;
				}
			}
			if (assassin != null)
			{
				int num = ((assassin.PointSpent() <= UsedSPThreshold) ? UnusedSPNeverUsed : UnusedSPNormal);
				if (assassin.skillPoints >= num)
				{
					m_switchToAssassinId = assassin.id;
					return EResult.Fullfilled;
				}
			}
			if (assassin2 != null && assassin2.skillPoints >= UnusedSPRandomAssassin)
			{
				m_switchToAssassinId = assassin2.id;
				return EResult.Fullfilled;
			}
			return EResult.Proceed;
		}

		public override void RedirectToScreen()
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			if (m_switchToAssassinId != null && profileManager.SelectedAssassin != m_switchToAssassinId)
			{
				profileManager.SelectedAssassin = m_switchToAssassinId;
			}
			base.RedirectToScreen();
		}
	}
}
