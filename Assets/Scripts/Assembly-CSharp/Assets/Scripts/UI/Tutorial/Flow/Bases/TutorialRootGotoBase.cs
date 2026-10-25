namespace Assets.Scripts.UI.Tutorial.Flow.Bases
{
	public abstract class TutorialRootGotoBase : TutorialGotoBase
	{
		protected void Start()
		{
			OnScreenInit();
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			OnScreenDestroy();
		}
	}
}
