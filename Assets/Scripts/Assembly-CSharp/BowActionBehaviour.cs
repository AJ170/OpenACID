using UnityEngine;

public class BowActionBehaviour : MonoBehaviour
{
	public Animator TheCharacter;

	public Animator TheBow;

	public bool DrawBow;

	public bool Aim;

	public bool Fire;

	private void SetInteger(string _name, int _value)
	{
		if (TheCharacter != null)
		{
			TheCharacter.SetInteger(_name, _value);
		}
		if (TheBow != null)
		{
			TheBow.SetInteger(_name, _value);
		}
	}

	private void SetBool(string _name, bool _value)
	{
		if (TheCharacter != null)
		{
			TheCharacter.SetBool(_name, _value);
		}
		if (TheBow != null)
		{
			TheBow.SetBool(_name, _value);
		}
	}

	private void SetGeneralState(int _state)
	{
		SetInteger("GeneralState", _state);
	}

	private void SetAttackRangeType(int _state)
	{
		SetInteger("AttackRangeType", _state);
	}

	private void SetStopAction(bool _value)
	{
		SetBool("StopAction", _value);
	}

	private void Update()
	{
		if (DrawBow)
		{
			SetGeneralState(7);
			SetStopAction(!Aim);
			if (Aim)
			{
				SetAttackRangeType(1);
				if (Fire)
				{
					SetAttackRangeType(2);
				}
			}
			else
			{
				SetAttackRangeType(0);
			}
		}
		else
		{
			SetAttackRangeType(0);
			SetGeneralState(0);
			SetStopAction(true);
		}
	}
}
