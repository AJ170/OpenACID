namespace SharedBaseLib.Storage
{
	public interface IAndroidExternalStorage
	{
		string SecondaryExternalStorage { get; }

		bool IsDirectoryWritable(string _dir);
	}
}
