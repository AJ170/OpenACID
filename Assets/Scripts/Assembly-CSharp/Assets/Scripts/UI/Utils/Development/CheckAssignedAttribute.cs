using System;
using System.Diagnostics;
using System.Reflection;
using AcierProtocol;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.UI.Utils.Development
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class CheckAssignedAttribute : Attribute
	{
		public bool NullAllowed { get; set; }

		public CheckAssignedAttribute()
		{
			NullAllowed = false;
		}

		[Conditional("UNITY_DEBUG")]
		[Conditional("UNITY_RELEASE")]
		public static void CheckAssignedMembers(object _type)
		{
			FieldInfo[] array = from _p in _type.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where _p.GetCustomAttributes(typeof(CheckAssignedAttribute), true).Any((object _a) => !((CheckAssignedAttribute)_a).NullAllowed)
				select _p;
			foreach (FieldInfo fieldInfo in array)
			{
			}
		}
	}
}
