using AcierProtocol;
using Assets.Scripts.Mission.Definitions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class NpcDefinition : CharacterDefinition
	{
		public new NpcData.INpcDefinition Definition
		{
			get
			{
				return (NpcData.INpcDefinition)base.Definition;
			}
			set
			{
				base.Definition = value;
			}
		}

		public NpcDefinition(IEntity _entity)
			: base(_entity)
		{
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			base.Attach(_wrapperComponent);
			AnimationController component = GameEntity.GameObject.GetComponent<AnimationController>();
			component.OverrideSpeedFactor(EMovementSpeed.Run, Definition.RunSpeedOverwrite);
		}
	}
}
