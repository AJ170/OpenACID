using System.Collections.Generic;
using SharedBaseLib.Core;

namespace Assets.Scripts.Crowd
{
	public class CrowdStaticCache
	{
		public static BBSimpleList<double> FlowWeights;

		public static BBList<Crowd.CrowdStationData> Stations;

		public static BBList<Crowd.CrowdNodeData> NodeQueryList;

		public static HashSet<Crowd.CrowdNodeData> NodeQuerySet;
	}
}
