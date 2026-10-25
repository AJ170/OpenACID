namespace Assets.Scripts.LicenseCheck
{
	public class LicenseCheckBase
	{
		public enum ELicenseCheckResponseCode
		{
			INVALID = 0,
			ERROR_INVALID_PACKAGE_NAME = 1,
			ERROR_NOT_MATCHING_UID = 2,
			ERROR_NOT_MARKET_MANAGED = 3,
			NOT_LICENSED = 4,
			LICENSE_CHECK_FAILED = 5,
			LICENSED = 6
		}

		public delegate void LicenseCheckCallback(string _signedData, string _signature, ErrorState _errorState);

		protected LicenseCheckCallback Callback;

		public virtual void StartLicenseCheck(LicenseCheckCallback _callback = null)
		{
			Callback = _callback;
			CompleteLicenseCheck(ELicenseCheckResponseCode.LICENSED, string.Empty, string.Empty);
		}

		public virtual void ProcessResponse(string _jsonResponse)
		{
		}

		protected void CompleteLicenseCheck(ELicenseCheckResponseCode _code, string _signedData, string _signature, ErrorState _errorState = null)
		{
			if (Callback != null)
			{
				Callback(_signedData, _signature, _errorState);
			}
		}
	}
}
