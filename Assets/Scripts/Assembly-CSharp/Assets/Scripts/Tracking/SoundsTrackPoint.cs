using AcierProtocol;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Tracking
{
	public class SoundsTrackPoint : TimedTrackPoint
	{
		public bool IsGlobalSoundOn { get; private set; }

		public bool IsUISoundOn { get; private set; }

		public bool IsMusicOn { get; private set; }

		public bool IsThemeOn { get; private set; }

		public SoundsTrackPoint()
		{
		}

		public SoundsTrackPoint(bool _isGlobalSoundOn, bool _isUISoundOn, bool _isMusicOn, bool _isThemeOn)
		{
			IsGlobalSoundOn = _isGlobalSoundOn;
			IsUISoundOn = _isUISoundOn;
			IsMusicOn = _isMusicOn;
			IsThemeOn = _isThemeOn;
		}

		public override void Serialize(DataSerializerBase _ser)
		{
			base.Serialize(_ser);
			IsGlobalSoundOn = _ser.SerializeBool("IsGlobalSoundOn", IsGlobalSoundOn);
			IsUISoundOn = _ser.SerializeBool("IsUISoundOn", IsUISoundOn);
			IsMusicOn = _ser.SerializeBool("IsMusicOn", IsMusicOn);
			IsThemeOn = _ser.SerializeBool("IsThemeOn", IsThemeOn);
		}

		public override void Report(ITrackReporter _reporter, long _id)
		{
			base.Report(_reporter, _id);
			_reporter.Add(_id, "type", ETrackingEventType.Sounds);
			_reporter.Add(_id, "is_global_sound_on", IsGlobalSoundOn);
			_reporter.Add(_id, "is_animus_ui_sound_on", IsUISoundOn);
			_reporter.Add(_id, "is_music_on", IsMusicOn);
			_reporter.Add(_id, "is_main_theme_on", IsThemeOn);
		}
	}
}
