namespace Assets.Scripts.UI.Interfaces
{
	public interface IInitialized
	{
		void Initialize();

		void Clear();
	}
	public interface IInitialized<in T>
	{
		void Initialize(T _data);

		void Clear();
	}
	public interface IInitialized<in T1, in T2>
	{
		void Initialize(T1 _p1, T2 _p2);

		void Clear();
	}
}
