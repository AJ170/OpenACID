using AcierProtocol;
using Assets.Scripts.UI.Localization;

namespace Assets.Scripts.UI.Utils
{
	public class ClassDescription
	{
		public class HirelingDescription
		{
			public StringId ActiveNameId;

			public StringId PassiveNameId;

			public StringId ActiveDescriptionId;

			public StringId PassiveDescriptionId;
		}

		public HirelingDescription HirelingDescriptionData;

		public Assassin.EClassTypeId Type;

		public StringId NameId;

		public int[] DescriptionId;

		public string AvatarCreation;

		public string TrainingTab;
	}
}
