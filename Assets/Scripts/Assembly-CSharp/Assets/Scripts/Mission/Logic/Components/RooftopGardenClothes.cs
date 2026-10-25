using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using SharedBaseLib.Core;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class RooftopGardenClothes : Component
	{
		public enum ESide
		{
			Front = 0,
			Back = 1,
			Left = 2,
			Right = 3,
			Count = 4,
			Unknown = 5
		}

		private class Cloth
		{
			public Animator Animator { get; set; }

			public Cloth(Animator _animator)
			{
				Animator = _animator;
			}
		}

		private Cloth[] m_clothes = new Cloth[4];

		private BBDictionary<InteractionLocators.Locator, int> m_locatorToSide = new BBDictionary<InteractionLocators.Locator, int>(4);

		public RooftopGarden Owner
		{
			get
			{
				return Entity.Cast<RooftopGarden>();
			}
		}

		public override int Priority
		{
			get
			{
				return 14;
			}
		}

		public RooftopGardenClothes(IEntity _entity)
			: base(_entity)
		{
		}

		public override void OnActivate()
		{
			base.OnActivate();
			InitializeLocators();
		}

		public void JumpIn(InteractionLocators.Locator _locator)
		{
			m_clothes[GetSideByLocator(_locator)].Animator.SetRooftopGardenClothJumpIn();
		}

		public void JumpOut(InteractionLocators.Locator _locator)
		{
			m_clothes[GetSideByLocator(_locator)].Animator.SetRooftopGardenClothJumpOut();
		}

		public void Assassinate(InteractionLocators.Locator _locator)
		{
			m_clothes[GetSideByLocator(_locator)].Animator.SetRooftopGardenClothAssassinate();
		}

		public int GetSideByLocator(InteractionLocators.Locator _locator)
		{
			int value;
			if (!m_locatorToSide.TryGetValue(_locator, out value))
			{
				return 5;
			}
			return value;
		}

		public void Initialize(GameObject _goClothRoot)
		{
			m_clothes[3] = new Cloth(_goClothRoot.FindChildByName("RoofGarden_Cloth_01").GetComponent<Animator>());
			m_clothes[1] = new Cloth(_goClothRoot.FindChildByName("RoofGarden_Cloth_02").GetComponent<Animator>());
			m_clothes[2] = new Cloth(_goClothRoot.FindChildByName("RoofGarden_Cloth_03").GetComponent<Animator>());
			m_clothes[0] = new Cloth(_goClothRoot.FindChildByName("RoofGarden_Cloth_04").GetComponent<Animator>());
		}

		private void InitializeLocators()
		{
			InteractionLocators interactionLocators = Owner.InteractionLocators;
			for (int i = 0; i < interactionLocators.Locators.Count; i++)
			{
				InteractionLocators.Locator locator = interactionLocators.Locators[i];
				string name = locator.Position.name;
				ESide eSide = ESide.Unknown;
				if (string.Compare(name, "Left", true) == 0)
				{
					eSide = ESide.Left;
				}
				else if (string.Compare(name, "Right", true) == 0)
				{
					eSide = ESide.Right;
				}
				else if (string.Compare(name, "Front", true) == 0)
				{
					eSide = ESide.Front;
				}
				else if (string.Compare(name, "Back", true) == 0)
				{
					eSide = ESide.Back;
				}
				if (eSide != ESide.Unknown)
				{
					m_locatorToSide.Add(locator, (int)eSide);
				}
			}
		}
	}
}
