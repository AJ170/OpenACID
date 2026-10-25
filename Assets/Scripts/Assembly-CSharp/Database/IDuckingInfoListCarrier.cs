using System.Collections.Generic;

namespace Database
{
	public interface IDuckingInfoListCarrier
	{
		List<AudioMixingGroupDef.DuckingInfo> DuckingInfos { get; }
	}
}
