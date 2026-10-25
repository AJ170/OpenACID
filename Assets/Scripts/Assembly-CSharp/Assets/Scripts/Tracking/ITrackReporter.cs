using System;

namespace Assets.Scripts.Tracking
{
	public interface ITrackReporter
	{
		void Add(long _id, string _name, int _int);

		void Add(long _id, string _name, bool _bool);

		void Add(long _id, string _name, string _string);

		void Add(long _id, string _name, DateTime _time);

		void Add(long _id, string _name, TimeSpan _span);

		void Add(long _id, string _name, float _float);

		void Add(long _id, string _name, double _double);

		void Add(long _id, string _name, long _long);

		void Add(long _id, string _name, Enum _enum);

		TrackReport MakeReport();

		void Clear();
	}
}
