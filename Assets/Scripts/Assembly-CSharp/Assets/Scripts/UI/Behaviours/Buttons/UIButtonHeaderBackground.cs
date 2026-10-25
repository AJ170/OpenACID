using Assets.Scripts.Audio;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.Buttons
{
	public class UIButtonHeaderBackground : MonoBehaviour
	{
		public void OnClick()
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_deny);
		}
	}
}
