using System.Collections.Generic;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Screens.CodexScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;

public class CodexManager
{
	public bool NewCodexEntryUnlocked;

	private List<CodexDefinition> m_entitlementEntries = new List<CodexDefinition>();

	public void Initialize()
	{
		List<KeyValuePair<GameID, DBEntry>> items = Main.Instance.GameDB.GetItems<CodexDefinition>();
		for (int i = 0; i < items.Count; i++)
		{
			CodexDefinition codexDefinition = (CodexDefinition)items[i].Value;
			if (codexDefinition.ReqEntitlementID != GameID.Invalid)
			{
				m_entitlementEntries.Add(codexDefinition);
			}
		}
		Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate += new SimpleDelegate.CallbackFunction(CheckCodexEntitlements);
	}

	public CodexDefinition GetCodexDefinitionByEntitlementGuid(string _entitlementGuid)
	{
		GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(_entitlementGuid);
		return GetCodexDefinitionByEntitlementGuid(gameID.Guid);
	}

	public CodexDefinition GetCodexDefinitionByEntitlementGuid(int _entitlementGuidHash)
	{
		for (int i = 0; i < m_entitlementEntries.Count; i++)
		{
			CodexDefinition codexDefinition = m_entitlementEntries[i];
			if (codexDefinition.ReqEntitlementID.Guid == _entitlementGuidHash)
			{
				return codexDefinition;
			}
		}
		return null;
	}

	public void CheckCodexEntitlements(object _obj)
	{
		EntitlementManager entitlementManager = Singleton<StateManager>.Instance.ProfileManager.EntitlementManager;
		for (int i = 0; i < m_entitlementEntries.Count; i++)
		{
			CodexDefinition codexDefinition = m_entitlementEntries[i];
			if (entitlementManager.HasEntitlement(codexDefinition.ReqEntitlementID.Guid) && !UbiCore.Utils.Singleton<LocalStorage>.Instance.CodexEntryIsUnlocked(codexDefinition.GameID))
			{
				UbiCore.Utils.Singleton<LocalStorage>.Instance.CodexUnlockEntry(codexDefinition.GameID);
				CodexData.NewCodexEntryUnlocked = true;
			}
		}
	}
}
