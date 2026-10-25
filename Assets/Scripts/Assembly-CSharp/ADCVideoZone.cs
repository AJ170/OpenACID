public class ADCVideoZone
{
	public string zoneId = string.Empty;

	public ADCVideoZoneType zoneType;

	public ADCVideoZone(string newZoneId, ADCVideoZoneType newVideoZoneType)
	{
		zoneId = newZoneId;
		zoneType = newVideoZoneType;
	}
}
