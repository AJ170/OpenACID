using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EReportReason")]
	public enum EReportReason
	{
		[ProtoEnum(Name = "ReportReasonSpam", Value = 0)]
		ReportReasonSpam = 0,
		[ProtoEnum(Name = "ReportReasonOffensive", Value = 1)]
		ReportReasonOffensive = 1,
		[ProtoEnum(Name = "ReportReasonOther", Value = 2)]
		ReportReasonOther = 2
	}
}
