using System.Collections;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.TutorialMissionPopup
{
	public class TutorialMissionPopUpContent : PopUpContentView<TutorialMissionPopUpContent.TutorialMissionPopUpData>
	{
		public class TutorialMissionPopUpData : PopUpContentData
		{
			public string MissionName;

			public string Description;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.TutorialMission;
				}
			}
		}

		[SerializeField]
		private UITexture m_TutorialContextImage;

		[SerializeField]
		private UILabel m_TutorialDescriptionText;

		[SerializeField]
		private UIAlignmentBox m_vBox;

		private TutorialMissionPopUpData m_data;

		private void Awake()
		{
		}

		protected override void Render(TutorialMissionPopUpData _data)
		{
			m_data = _data;
			m_TutorialDescriptionText.text = m_data.Description;
			StartCoroutine(UpdateTexture());
		}

		private IEnumerator UpdateTexture()
		{
			m_TutorialContextImage.mainTexture = null;
			yield return null;
			string imagePath = string.Empty;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.TutorialMissionDescription.TryGetValue(m_data.MissionName, out imagePath) && !string.IsNullOrEmpty(imagePath))
			{
				Texture2D textureAtlas = (Texture2D)Resources.Load(imagePath, typeof(Texture2D));
				m_TutorialContextImage.mainTexture = textureAtlas;
			}
			EventDelegate.Add(m_vBox.OnReposition, OnAlignmentUpdate, true);
			m_vBox.RepositionNow = true;
		}

		public override void UpdateLayout()
		{
			if (m_TutorialContextImage.mainTexture != null)
			{
				EventDelegate.Add(m_vBox.OnReposition, OnAlignmentUpdate, true);
				m_vBox.RepositionNow = true;
			}
		}

		public void OnAlignmentUpdate()
		{
			EventDelegate.Execute(onLayoutFinished);
		}
	}
}
