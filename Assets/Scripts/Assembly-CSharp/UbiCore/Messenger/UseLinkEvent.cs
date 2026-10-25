using System;
using Assets.Scripts.Mission.Logic.Entities;

namespace UbiCore.Messenger
{
	[Serializable]
	public class UseLinkEvent : ParcourEvent
	{
		public OffMeshLinkEntity Link { get; private set; }

		public UseLinkEvent(OffMeshLinkEntity _link)
		{
			base.Type = EParcourType.UseLink;
			Link = _link;
		}
	}
}
