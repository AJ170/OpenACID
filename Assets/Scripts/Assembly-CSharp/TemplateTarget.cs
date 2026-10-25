using AcierProtocol;
using Assets.Scripts.Network.Missions.Mission.Data;

public class TemplateTarget : MissionBase<TemplateTargetData>
{
	[EnumFlags]
	public ETemplateTargetKind TargetKinds = ETemplateTargetKind.Escape_ETemplateTargetKind;

	[EnumFlags]
	public EObjectSubCategory SubCategories = (EObjectSubCategory)3;

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		base.OnPostRead(_nodeData);
		TargetKinds = (ETemplateTargetKind)base.Data.TargetKind;
		SubCategories = (EObjectSubCategory)base.Data.SubCategories;
	}
}
