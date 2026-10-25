namespace Assets.Scripts.Network.Missions.Math
{
	public interface IPoint
	{
		int Length { get; }

		float this[int _i] { get; set; }
	}
}
