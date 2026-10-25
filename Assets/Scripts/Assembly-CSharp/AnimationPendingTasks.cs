using Assets.Scripts.Mission.Logic;
using SharedBaseLib.Core;

internal class AnimationPendingTasks
{
	public class PendingAnimation
	{
		public int Id { get; set; }

		public string Type { get; set; }

		public uint Owner { get; set; }

		public string OwnerType { get; set; }

		public bool Disposed { get; set; }

		public bool Exited { get; set; }
	}

	private static BBDictionary<int, PendingAnimation> s_pendingAnimations = new BBDictionary<int, PendingAnimation>(50);

	public static BBList<PendingAnimation> Pending
	{
		get
		{
			BBList<PendingAnimation> bBList = null;
			lock (s_pendingAnimations)
			{
				return new BBList<PendingAnimation>(s_pendingAnimations.Values);
			}
		}
	}

	public static void AddPending(AnimationTaskBase _animationTask)
	{
		PendingAnimation pendingAnimation = new PendingAnimation();
		pendingAnimation.Id = _animationTask.ID;
		pendingAnimation.Type = _animationTask.TaskType.ToString();
		pendingAnimation.Owner = _animationTask.Owner;
		if (pendingAnimation.Owner != (uint)EntityId.InvalidId)
		{
			pendingAnimation.OwnerType = _animationTask.Owner.Type.ToString();
		}
		lock (s_pendingAnimations)
		{
			s_pendingAnimations.Add(pendingAnimation.Id, pendingAnimation);
		}
	}

	public static void OnDispose(AnimationTaskBase _animationTask)
	{
		lock (s_pendingAnimations)
		{
			PendingAnimation value = null;
			if (s_pendingAnimations.TryGetValue(_animationTask.ID, out value))
			{
				value.Disposed = true;
			}
		}
	}

	public static void OnExit(AnimationTaskBase _animationTask)
	{
		lock (s_pendingAnimations)
		{
			PendingAnimation value = null;
			if (s_pendingAnimations.TryGetValue(_animationTask.ID, out value))
			{
				value.Exited = true;
			}
		}
	}

	public static void RemovePending(AnimationTaskBase _animationTask)
	{
		lock (s_pendingAnimations)
		{
			s_pendingAnimations.Remove(_animationTask.ID);
		}
	}
}
