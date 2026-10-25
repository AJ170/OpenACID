namespace Assets.Scripts.Engine
{
	public delegate bool Predicate<T1, T2>(T1 obj1, T2 _obj2);
	public delegate bool Predicate<T1, T2, T3>(T1 obj1, T2 _obj2, T3 _obj3);
}
