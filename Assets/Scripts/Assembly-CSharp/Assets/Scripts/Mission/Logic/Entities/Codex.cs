using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class Codex : InteractableObject, ICodexEntryCarrier, IDefinitionCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity
	{
		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.Codex;
			}
		}

		public CodexEntry CodexEntry { get; private set; }

		public Codex()
		{
			CodexEntry = AddComponent<CodexEntry>();
		}
	}
}
