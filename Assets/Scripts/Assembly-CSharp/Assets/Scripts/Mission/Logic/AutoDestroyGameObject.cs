using Assets.Scripts.Mission.Utilities.AutoUpdating;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic
{
	public class AutoDestroyGameObject : AutoUpdateableBase
	{
		public GameObject GameObject { get; set; }

		public int TurnDuration { get; set; }

		public int StartTurn { get; private set; }

		public override void Start()
		{
			base.Start();
			StartTurn = LogicMain.CurrentTurn;
		}

		protected override bool UpdateFrameInternal(float _gameTimeDelta)
		{
			if (StartTurn + TurnDuration <= LogicMain.CurrentTurn)
			{
				if (GameObject != null)
				{
					Object.Destroy(GameObject);
				}
				GameObject = null;
				return true;
			}
			return false;
		}
	}
}
