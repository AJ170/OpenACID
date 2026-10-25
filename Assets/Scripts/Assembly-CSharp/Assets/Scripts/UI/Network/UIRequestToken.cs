namespace Assets.Scripts.UI.Network
{
	public sealed class UIRequestToken
	{
		public bool Abandoned { get; set; }

		public override string ToString()
		{
			return string.Format("UIRequestToken(Abandoned={0})", Abandoned);
		}
	}
}
