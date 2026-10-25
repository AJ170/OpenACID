using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.HirelingScreen
{
	public class HirelingTab : MonoBehaviour
	{
		public UIExtendedLabel ItemPOW;

		public UIExtendedLabel AssassinsName;

		public UIExtendedLabel Classname;

		public TweenAlpha Highlight;

		public TweenScale Scale;

		public UISprite RelationSymbol;

		[SerializeField]
		private UIHeroicPortraitTexture m_heroicPortrait;

		private UIButton m_uiEventTrigger;

		public int HirelingIndex { get; set; }

		public UIButton UIEventTrigger
		{
			get
			{
				return m_uiEventTrigger;
			}
		}

		public string HirelingID
		{
			get
			{
				return HirelingData.HirelingID;
			}
		}

		public HirelingAssassinsData HirelingData { get; private set; }

		public void StartSelections(bool _instance = false)
		{
			Scale.from = Vector3.one;
			Scale.to = new Vector3(1.1f, 1.1f, 1.1f);
			if (_instance)
			{
				Scale.ResetToBeginning();
				Scale.PlayForward();
				Highlight.PlayForward();
				return;
			}
			Scale.enabled = false;
			Scale.tweenFactor = 1f;
			Scale.Sample(1f, false);
			Highlight.enabled = false;
			Highlight.tweenFactor = 1f;
			Highlight.Sample(1f, false);
		}

		public void StartDeselects(bool _instance = false)
		{
			Scale.to = Vector3.one;
			Scale.from = new Vector3(1.1f, 1.1f, 1.1f);
			if (_instance)
			{
				Scale.ResetToBeginning();
				Scale.PlayForward();
				Highlight.PlayReverse();
				return;
			}
			Scale.enabled = false;
			Scale.tweenFactor = 1f;
			Scale.Sample(1f, false);
			Highlight.enabled = false;
			Highlight.tweenFactor = 0f;
			Highlight.Sample(0f, false);
		}

		public void UpdateData(HirelingAssassin _hirelingAssassinData)
		{
			HirelingData = new HirelingAssassinsData(_hirelingAssassinData);
			m_uiEventTrigger = base.gameObject.AddComponentIfNotExist<UIButton>();
			ItemPOW.text = LocalizationManager.Instance.GetInteger(HirelingData.ItemPow);
			AssassinsName.text = HirelingData.AssassinsName;
			Classname.text = HirelingData.ClassName;
			RelationSymbol.enabled = HirelingData.ShowRelationSymbol;
			UIPortraitUtil.UpdateHeroicPortraitByHirelingAssassin(_hirelingAssassinData, _hirelingAssassinData.rankIdx, m_heroicPortrait);
		}
	}
}
