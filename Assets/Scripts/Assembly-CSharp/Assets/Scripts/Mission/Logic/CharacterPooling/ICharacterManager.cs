using UnityEngine;

namespace Assets.Scripts.Mission.Logic.CharacterPooling
{
	public interface ICharacterManager
	{
		GameObject CharacterPoolNode { get; }

		bool Update(float _dt);

		void OnMissionLoad();

		void OnMissionUnloaded();
	}
}
