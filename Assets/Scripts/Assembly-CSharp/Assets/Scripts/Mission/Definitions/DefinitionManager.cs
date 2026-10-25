using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Definitions
{
	public class DefinitionManager : UbiCore.Utils.Singleton<DefinitionManager>
	{
		public enum EStatus
		{
			Fresh = 0,
			Closed = 1
		}

		private readonly BBList<Data.Definition> m_Definitions = new BBList<Data.Definition>();

		public EStatus Status { get; private set; }

		public int DefinitionCount
		{
			get
			{
				return m_Definitions.Count;
			}
		}

		public DefinitionManager()
		{
			Status = EStatus.Fresh;
		}

		public new static void Initialize()
		{
			UbiCore.Utils.Singleton<DefinitionManager>.Shutdown();
			UbiCore.Utils.Singleton<DefinitionManager>.Initialize();
			UbiCore.Utils.Singleton<GameData>.Initialize();
			UbiCore.Utils.Singleton<RuleData>.Initialize();
			UbiCore.Utils.Singleton<SkillData>.Initialize();
			UbiCore.Utils.Singleton<AvatarData>.Initialize();
			UbiCore.Utils.Singleton<NpcData>.Initialize();
			UbiCore.Utils.Singleton<InteractableObjectData>.Initialize();
			UbiCore.Utils.Singleton<StaticObjectData>.Initialize();
			UbiCore.Utils.Singleton<PatrolData>.Initialize();
			UbiCore.Utils.Singleton<ProjectileData>.Initialize();
			UbiCore.Utils.Singleton<BuffData>.Initialize();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			UbiCore.Utils.Singleton<GameData>.Shutdown();
			UbiCore.Utils.Singleton<RuleData>.Shutdown();
			UbiCore.Utils.Singleton<SkillData>.Shutdown();
			UbiCore.Utils.Singleton<AvatarData>.Shutdown();
			UbiCore.Utils.Singleton<NpcData>.Shutdown();
			UbiCore.Utils.Singleton<InteractableObjectData>.Shutdown();
			UbiCore.Utils.Singleton<StaticObjectData>.Shutdown();
			UbiCore.Utils.Singleton<PatrolData>.Shutdown();
			UbiCore.Utils.Singleton<ProjectileData>.Shutdown();
			UbiCore.Utils.Singleton<BuffData>.Shutdown();
		}

		public T GetDefinition<T>(int _id) where T : Data.IDefinition
		{
			return (T)(object)m_Definitions[_id];
		}

		public int RegisterDefinition(Data.IDefinition _definition)
		{
			if (Status == EStatus.Fresh)
			{
				m_Definitions.Add((Data.Definition)_definition);
				return m_Definitions.Count - 1;
			}
			return -1;
		}

		public int RegisterOrReplaceDefinition(Data.Definition _definition)
		{
			int i;
			for (i = 0; i < m_Definitions.Count; i++)
			{
				if (m_Definitions[i].GameID == _definition.GameID)
				{
					m_Definitions[i] = _definition;
					break;
				}
			}
			if (i == m_Definitions.Count)
			{
				m_Definitions.Add(_definition);
			}
			return i;
		}

		public void InitializeDefinitions()
		{
			for (int i = 0; i < m_Definitions.Count; i++)
			{
				m_Definitions[i].InitializePass1();
			}
			for (int j = 0; j < m_Definitions.Count; j++)
			{
				m_Definitions[j].InitializePass2();
			}
			CloseDefinitions();
		}

		public void CloseDefinitions()
		{
			Status = EStatus.Closed;
		}
	}
}
