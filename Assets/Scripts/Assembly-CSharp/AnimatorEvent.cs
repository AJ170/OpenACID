using UnityEngine;

public class AnimatorEvent
{
	public Animator Source { get; private set; }

	public int StateNameHash { get; private set; }

	public AnimatorEventType Type { get; private set; }

	public int Layer { get; private set; }

	public int BlendID { get; private set; }

	public Object Payload { get; private set; }

	public AnimatorEvent(Animator _source, int _stateNameHash, AnimatorEventType _type, int _blendID, int _layer, Object _payload)
	{
		Source = _source;
		StateNameHash = _stateNameHash;
		Type = _type;
		BlendID = _blendID;
		Layer = _layer;
		Payload = _payload;
	}
}
