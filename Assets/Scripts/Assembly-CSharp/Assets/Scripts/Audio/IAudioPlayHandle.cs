using System.Collections;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public interface IAudioPlayHandle
	{
		bool IsPlaying { get; }

		float PlaybackTime { get; }

		float PlaybackLength { get; }

		bool ShouldPlayOnLoadReady { get; set; }

		bool UnloadOnPlayFailed { get; set; }

		float Volume { get; set; }

		IVolumeFader VolumeFader { get; }

		AudioController AudioController { get; }

		ILoadStrategyEvent LoadStrategyEvent { get; }

		SimpleDelegate OnFinishedEvent { get; set; }

		SimpleDelegate OnScheduledPlay { get; set; }

		IEnumerator FadeOutHandler { get; set; }

		PlayResult Play(bool _loop = false, AudioSource _audioSource = null, float _delay = 0f);

		void Stop();

		bool DetachChannelFromParent();
	}
}
