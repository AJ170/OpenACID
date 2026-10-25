using Database;

namespace Assets.Scripts.Mission.Definitions
{
	public interface IDataType
	{
		GameID GameID { get; }

		bool IsValid { get; }
	}
}
