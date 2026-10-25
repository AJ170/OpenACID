using AcierProtocol;

public interface IMonitoredHidingBehaviour
{
	bool PreventFailingOnIncrease { get; }

	bool MonitorsLimitType(ELimitType _limitType);

	bool WillFailOnIncrease(int _increase);
}
