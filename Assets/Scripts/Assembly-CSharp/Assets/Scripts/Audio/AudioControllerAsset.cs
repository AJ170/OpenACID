using Database;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class AudioControllerAsset : ScriptableObject
	{
		[SerializeField]
		private string m_guidString;

		private GameID m_gameId = GameID.Invalid;

		public GameID GameId
		{
			get
			{
				if (!GameID.IsValid(m_gameId) && !string.IsNullOrEmpty(m_guidString))
				{
					GameDB safeGameDb = Main.SafeGameDb;
					m_gameId = safeGameDb.FindGameIdFromGuid(m_guidString);
					m_guidString = string.Empty;
				}
				return m_gameId;
			}
		}
	}
}
