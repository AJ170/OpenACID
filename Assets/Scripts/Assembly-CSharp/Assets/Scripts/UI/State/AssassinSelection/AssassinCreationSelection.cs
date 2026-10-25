using AcierProtocol;

namespace Assets.Scripts.UI.State.AssassinSelection
{
	public class AssassinCreationSelection
	{
		private bool m_validType;

		private Assassin.EClassTypeId m_avatarType;

		public Assassin.EClassTypeId AvatarType
		{
			get
			{
				return m_avatarType;
			}
			set
			{
				m_avatarType = value;
				m_validType = true;
			}
		}

		public bool Valid
		{
			get
			{
				return m_validType;
			}
		}

		public void Clear()
		{
			m_validType = false;
		}
	}
}
