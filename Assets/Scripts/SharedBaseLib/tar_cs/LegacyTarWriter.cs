using System;
using System.IO;
using System.Threading;

namespace tar_cs
{
	public class LegacyTarWriter : IDisposable
	{
		private readonly Stream outStream;

		protected byte[] buffer = new byte[1024];

		private bool isClosed;

		public bool ReadOnZero = true;

		protected virtual Stream OutStream
		{
			get
			{
				return outStream;
			}
		}

		public LegacyTarWriter(Stream writeStream)
		{
			outStream = writeStream;
		}

		public void Dispose()
		{
			Close();
		}

		public void WriteDirectoryEntry(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}
			string text = path;
			if (text[text.Length - 1] != '/')
			{
				path += "/";
			}
			DateTime lastModificationTime = ((!Directory.Exists(path)) ? DateTime.Now : Directory.GetLastWriteTime(path));
			WriteHeader(path, lastModificationTime, 0L, 101, 101, 777, EntryType.Directory);
		}

		public void WriteDirectory(string directory, bool doRecursive)
		{
			if (string.IsNullOrEmpty(directory))
			{
				throw new ArgumentNullException("directory");
			}
			WriteDirectoryEntry(directory);
			string[] files = Directory.GetFiles(directory);
			foreach (string fileName in files)
			{
				Write(fileName);
			}
			files = Directory.GetDirectories(directory);
			foreach (string text in files)
			{
				WriteDirectoryEntry(text);
				if (doRecursive)
				{
					WriteDirectory(text, true);
				}
			}
		}

		public void Write(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				throw new ArgumentNullException("fileName");
			}
			using (FileStream fileStream = File.OpenRead(fileName))
			{
				Write(fileStream, fileStream.Length, fileName, 61, 61, 511, File.GetLastWriteTime(fileStream.Name));
			}
		}

		public void Write(FileStream file)
		{
			string text = Path.GetFullPath(file.Name).Replace(Path.GetPathRoot(file.Name), string.Empty);
			text = text.Replace(Path.DirectorySeparatorChar, '/');
			Write(file, file.Length, text, 61, 61, 511, File.GetLastWriteTime(file.Name));
		}

		public void Write(Stream data, long dataSizeInBytes, string name)
		{
			Write(data, dataSizeInBytes, name, 61, 61, 511, DateTime.Now);
		}

		public virtual void Write(string name, long dataSizeInBytes, int userId, int groupId, int mode, DateTime lastModificationTime, WriteDataDelegate writeDelegate)
		{
			IArchiveDataWriter archiveDataWriter = new DataWriter(OutStream, dataSizeInBytes);
			WriteHeader(name, lastModificationTime, dataSizeInBytes, userId, groupId, mode, EntryType.File);
			while (archiveDataWriter.CanWrite)
			{
				writeDelegate(archiveDataWriter);
			}
			AlignTo512(dataSizeInBytes, false);
		}

		public virtual void Write(Stream data, long dataSizeInBytes, string name, int userId, int groupId, int mode, DateTime lastModificationTime)
		{
			if (isClosed)
			{
				throw new TarException("Can not write to the closed writer");
			}
			WriteHeader(name, lastModificationTime, dataSizeInBytes, userId, groupId, mode, EntryType.File);
			WriteContent(dataSizeInBytes, data);
			AlignTo512(dataSizeInBytes, false);
		}

		protected void WriteContent(long count, Stream data)
		{
			while (count > 0 && count > buffer.Length)
			{
				int num = data.Read(buffer, 0, buffer.Length);
				if (num < 0)
				{
					throw new IOException("LegacyTarWriter unable to read from provided stream");
				}
				if (num == 0)
				{
					if (!ReadOnZero)
					{
						break;
					}
					Thread.Sleep(100);
				}
				OutStream.Write(buffer, 0, num);
				count -= num;
			}
			if (count <= 0)
			{
				return;
			}
			int num2 = data.Read(buffer, 0, (int)count);
			if (num2 < 0)
			{
				throw new IOException("LegacyTarWriter unable to read from provided stream");
			}
			if (num2 == 0)
			{
				while (count > 0)
				{
					OutStream.WriteByte(0);
					count--;
				}
			}
			else
			{
				OutStream.Write(buffer, 0, num2);
			}
		}

		protected virtual void WriteHeader(string name, DateTime lastModificationTime, long count, int userId, int groupId, int mode, EntryType entryType)
		{
			TarHeader tarHeader = new TarHeader
			{
				FileName = name,
				LastModification = lastModificationTime,
				SizeInBytes = count,
				UserId = userId,
				GroupId = groupId,
				Mode = mode,
				EntryType = entryType
			};
			OutStream.Write(tarHeader.GetHeaderValue(), 0, tarHeader.HeaderSize);
		}

		public void AlignTo512(long size, bool acceptZero)
		{
			size %= 512;
			if (size != 0L || acceptZero)
			{
				while (size < 512)
				{
					OutStream.WriteByte(0);
					size++;
				}
			}
		}

		public virtual void Close()
		{
			if (!isClosed)
			{
				AlignTo512(0L, true);
				AlignTo512(0L, true);
				isClosed = true;
			}
		}
	}
}
