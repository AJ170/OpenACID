using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.SocialScreen
{
	public class FriendTab : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedLabel m_username;

		[SerializeField]
		private UIExtendedLabel m_itemPower;

		[SerializeField]
		private UIExtendedLabel m_rank;

		[SerializeField]
		private UIExtendedLabel m_positionLabel;

		[SerializeField]
		private UIHeroicPortraitTexture m_heroicPortrait;

		[SerializeField]
		private UISprite m_relation;

		[SerializeField]
		private TweenAlpha m_highlight;

		[SerializeField]
		private TweenScale m_scale;

		[SerializeField]
		private BBTweenAlpha m_tweenHeaderHighLight;

		[SerializeField]
		private BBTweenColor m_nameHighlightColor;

		[SerializeField]
		private BBTweenColor m_positionHighlightColor;

		public List<EventDelegate> onClick = new List<EventDelegate>();

		private bool m_selected;

		public static FriendTab Current { get; private set; }

		// Runtime data only. Explicitly non serialised: the editor otherwise walks the whole
		// protobuf profile graph (depth limit warning, recursive serialisation error).
		[NonSerialized]
		private FriendProfile m_friend;

		public FriendProfile Friend
		{
			get
			{
				return m_friend;
			}
			private set
			{
				m_friend = value;
			}
		}

		public int Index { get; private set; }

		public bool IsPlayer { get; private set; }

		public bool IsSelected
		{
			get
			{
				return m_selected;
			}
		}

		public FriendTab()
		{
			Index = -1;
		}

		private void Awake()
		{
		}

		private void OnClick()
		{
			Current = this;
			EventDelegate.Execute(onClick);
			Current = null;
		}

		public void UpdateData(FriendProfile _friend, int _index, bool _isPlayer)
		{
			UIDataMapper instance = UbiCore.Utils.Singleton<UIDataMapper>.Instance;
			Friend = _friend;
			Index = _index;
			IsPlayer = _isPlayer;
			long highestAssassinItemPower = FriendScreen.GetHighestAssassinItemPower(Friend);
			m_itemPower.text = LocalizationManager.Instance.GetInteger(highestAssassinItemPower);
			m_positionLabel.text = (Index + 1).ToString();
			string text = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.HeroicRankDescription[_friend.maxRankIdx].NameId);
			m_rank.text = ((!IsPlayer) ? text : LocalizationManager.Instance.Get(8433, new StringMapper { { "player_rank", text } }));
			FriendAssassin[] friendAssassin = Friend.friendAssassin;
			Array.Sort(friendAssassin, (FriendAssassin _assassin1, FriendAssassin _assassin2) => _assassin2.itemPower.CompareTo(_assassin1.itemPower));
			FriendAssassin friendAssassin2 = ((friendAssassin.Length <= 0) ? null : friendAssassin[0]);
			UIPortraitUtil.UpdateHeroicPortraitByFriendAssassin(friendAssassin2, Friend.maxRankIdx, m_heroicPortrait);
			m_username.text = Friend.userName;
			m_relation.gameObject.SetActive(!IsPlayer);
			m_relation.spriteName = instance.FriendRelationSprite[(int)Friend.friendType];
		}

		public void SetSelected(bool _selected)
		{
			if (m_selected != _selected)
			{
				m_selected = _selected;
				m_scale.from = ((!m_selected) ? new Vector3(1.1f, 1.1f, 1.1f) : Vector3.one);
				m_scale.to = (m_selected ? new Vector3(1.1f, 1.1f, 1.1f) : Vector3.one);
				m_scale.ResetToBeginning();
				m_scale.PlayForward();
				m_highlight.Play(m_selected);
				m_tweenHeaderHighLight.Play(m_selected);
				m_nameHighlightColor.Play(m_selected);
				if (Index != 0)
				{
					m_positionHighlightColor.Play(m_selected);
				}
			}
		}

		public void SetSelectedInstant(bool _selected)
		{
			m_selected = _selected;
			m_scale.from = ((!m_selected) ? new Vector3(1.1f, 1.1f, 1.1f) : Vector3.one);
			m_scale.to = (m_selected ? new Vector3(1.1f, 1.1f, 1.1f) : Vector3.one);
			m_scale.Sample(1f, false);
			float factor = ((!_selected) ? 0f : 1f);
			m_highlight.Sample(factor, false);
			m_tweenHeaderHighLight.Sample(factor, false);
			m_nameHighlightColor.Sample(factor, false);
			if (Index != 0)
			{
				m_positionHighlightColor.Sample(factor, false);
			}
		}
	}
}
