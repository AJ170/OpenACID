using System;
using AcierProtocol;

namespace Mission
{
	public static class EObjectiveResultUtil
	{
		public static EObjectiveResult ToResult(this AcierProtocol.EObjectiveResult _initialResult)
		{
			switch (_initialResult)
			{
			case AcierProtocol.EObjectiveResult.Undefined_EObjectiveResult:
				return EObjectiveResult.Undefined;
			case AcierProtocol.EObjectiveResult.Succeeded_EObjectiveResult:
				return EObjectiveResult.Fulfilled;
			case AcierProtocol.EObjectiveResult.Failed_EObjectiveResult:
				return EObjectiveResult.Failed;
			default:
				throw new ArgumentOutOfRangeException("_initialResult");
			}
		}
	}
}
