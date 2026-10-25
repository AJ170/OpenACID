using System.IO;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class DirectoryExtensions
	{
		public static void Clean(this DirectoryInfo _dir, bool _keepFolders)
		{
			FileInfo[] files = _dir.GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				fileInfo.Delete();
			}
			if (!_keepFolders)
			{
				DirectoryInfo[] directories = _dir.GetDirectories();
				foreach (DirectoryInfo directoryInfo in directories)
				{
					directoryInfo.Delete(true);
				}
			}
		}

		public static void Clean(string _path, bool _keepFolders)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(_path);
			if (directoryInfo.Exists)
			{
				directoryInfo.Clean(_keepFolders);
			}
		}
	}
}
