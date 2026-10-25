using System;
using AcierProtocol;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class AvatarClassDefinitionsExtensions
	{
		public static AvatarClassDefinitions.AvatarClass GetDefinitionByType(this AvatarClassDefinitions _def, Assassin.EClassTypeId _type)
		{
			return Array.Find(_def.classes, (AvatarClassDefinitions.AvatarClass x) => x.classTypeId == _type);
		}
	}
}
