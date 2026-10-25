using AcierProtocol;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.ScreenManager.SceenPlaceholder;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MysteryBox
{
	public class UIMysteryBoxBehaviour : MonoBehaviour
	{
		private GameObject m_mysteryBoxItemPrefab;

		private GameObject m_mysteryBoxRessourcePrefab;

		private GameObject m_mysteryBoxMiscPrefab;

		[SerializeField]
		private BBTweenAlpha m_refTween;

		private GameObject m_itemBox;

		private GameObject m_currencyBox;

		private GameObject m_bootyBox;

		private Animator m_currentBox;

		private static string closing = "closing";

		private static string opening = "opening";

		private static string appearing = "appear";

		private static string closed = "closed";

		private static string open = "open";

		private Quality.EQuality m_quality;

		public bool IsOpening
		{
			get
			{
				return m_currentBox.GetCurrentAnimatorStateInfo(0).IsName(opening);
			}
		}

		public bool IsOpen
		{
			get
			{
				return m_currentBox.GetCurrentAnimatorStateInfo(0).IsName(open);
			}
		}

		public bool IsClosing
		{
			get
			{
				return m_currentBox.GetCurrentAnimatorStateInfo(0).IsName(closing);
			}
		}

		public bool IsClosed
		{
			get
			{
				return m_currentBox.GetCurrentAnimatorStateInfo(0).IsName(closed);
			}
		}

		public bool IsAppearing
		{
			get
			{
				return m_currentBox.GetCurrentAnimatorStateInfo(0).IsName(appearing);
			}
		}

		private void Awake()
		{
			m_mysteryBoxItemPrefab = Main.Instance.GameDB.GetItem<ItemDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_mysteryBoxItemPrefab).ItemPrefab.AccessObject<GameObject>();
			m_mysteryBoxMiscPrefab = Main.Instance.GameDB.GetItem<ItemDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_mysteryBoxMiscPrefab).ItemPrefab.AccessObject<GameObject>();
			m_mysteryBoxRessourcePrefab = Main.Instance.GameDB.GetItem<ItemDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_mysteryBoxRessourcePrefab).ItemPrefab.AccessObject<GameObject>();
		}

		private Animator GetCurrentBoxByType(EMysteryBoxType _type)
		{
			Animator result = null;
			switch (_type)
			{
			case EMysteryBoxType.MysteryBoxItemType:
			case EMysteryBoxType.MysteryBoxItemType_Tutorial:
				if (m_itemBox == null)
				{
					m_itemBox = CreateBox(m_mysteryBoxItemPrefab);
				}
				result = m_itemBox.GetComponent<Animator>();
				break;
			case EMysteryBoxType.MysteryBoxBootyChestType:
				if (m_bootyBox == null)
				{
					m_bootyBox = CreateBox(m_mysteryBoxMiscPrefab);
				}
				result = m_bootyBox.GetComponent<Animator>();
				break;
			case EMysteryBoxType.MysteryBoxMovieType:
				if (m_currencyBox == null)
				{
					m_currencyBox = CreateBox(m_mysteryBoxRessourcePrefab);
				}
				result = m_currencyBox.GetComponent<Animator>();
				break;
			}
			return result;
		}

		private GameObject CreateBox(GameObject _prefab)
		{
			UIPlaceholder placeholder = Singleton<UIScreenManager>.Instance.SceneData.Placeholders.GetPlaceholder(UIPlaceholders.EPlaceholderType.MysteryBox);
			GameObject gameObject = NGUITools.AddChild(placeholder.gameObject, _prefab);
			gameObject.SetActive(false);
			return gameObject;
		}

		public void Activate(EMysteryBoxType _type, Quality.EQuality _quality)
		{
			if (m_currentBox != null)
			{
				m_currentBox.gameObject.SetActive(false);
			}
			m_quality = _quality;
			m_currentBox = GetCurrentBoxByType(_type);
			if (m_currentBox != null)
			{
				m_currentBox.gameObject.SetActive(true);
				m_currentBox.Play(appearing);
			}
		}

		public void OpenBox(bool _playCoins, Quality.EQuality _quality)
		{
			m_quality = _quality;
			if (!IsOpening && !IsOpen)
			{
				m_currentBox.Play(opening);
				m_currentBox.gameObject.GetComponent<ChestLight>().SetEffectColor(m_quality);
				if (_playCoins)
				{
					m_currentBox.gameObject.GetComponent<ChestLight>().PlayCoins();
				}
			}
			else
			{
				m_currentBox.Play(open);
			}
		}

		public void CloseBox()
		{
			if (!IsClosing && !IsClosed)
			{
				m_currentBox.Play(closing);
			}
		}

		public void Deactivate()
		{
			m_currentBox.gameObject.SetActive(false);
		}

		private void OnDestroy()
		{
			if (m_bootyBox != null)
			{
				Object.Destroy(m_bootyBox.gameObject);
			}
			if (m_itemBox != null)
			{
				Object.Destroy(m_itemBox.gameObject);
			}
			if (m_currencyBox != null)
			{
				Object.Destroy(m_currencyBox.gameObject);
			}
		}
	}
}
