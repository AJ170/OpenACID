using System;
using System.Collections.Generic;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.MVC.Models.Misc;
using Assets.Scripts.UI.MVC.UIView.Misc;
using Assets.Scripts.UI.Utils.Development;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Misc
{
	public class UIProgressBarController : UIControllerBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIProgressBarViewPresenter m_viewPresenter;

		private IAudioHandle m_progressTickAudio;

		public UIProgressBarModel Model { get; set; }

		public List<EventDelegate> OnTweenFinished
		{
			get
			{
				return m_viewPresenter.ProgressBarTween.onFinished;
			}
		}

		public bool IsMaxProgress
		{
			get
			{
				return Math.Abs(m_viewPresenter.ProgressBar.value - 1f) < 0.0001f;
			}
		}

		public void Initialize(UIProgressBarModel _model, bool _instant)
		{
			Show();
			base.IsInitialized = true;
			UpdateController(_model, _instant);
		}

		public void UpdateController(UIProgressBarModel _model, bool _instant)
		{
			Model = _model;
			m_viewPresenter.ShowIcons = _model.ShowIcons;
			m_viewPresenter.SetupTweener(_model.CurrentValue, _model.AnimationEndValue, _model.StartValue, _model.MaxValue);
		}

		public void PlayProgressSound()
		{
			if (m_progressTickAudio != null)
			{
				m_progressTickAudio.Stop();
			}
		}

		public void StopProgressSound()
		{
			if (m_progressTickAudio != null)
			{
				m_progressTickAudio.Stop();
			}
		}

		public override void Show()
		{
			base.Show();
			m_viewPresenter.gameObject.SetActive(true);
		}

		public override void Hide()
		{
			base.Hide();
			m_viewPresenter.gameObject.SetActive(false);
		}

		protected override void OnAwakeUnity()
		{
			base.OnAwakeUnity();
			m_progressTickAudio = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_missionreward_score_loop, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
			m_progressTickAudio.Load();
		}

		protected override void OnDestroyUnity()
		{
			base.OnDestroyUnity();
			if (m_progressTickAudio != null)
			{
				m_progressTickAudio.Stop();
				m_progressTickAudio.Unload();
				m_progressTickAudio = null;
			}
		}
	}
}
