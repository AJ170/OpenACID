using System.Collections.Generic;
using SharedBaseLib.Serialization;
using SharedBaseLib.System.Extensions;

public class FirstContact
{
	public class DataFile : DataSerializableBase
	{
		public string Filename { get; set; }

		public string Hash { get; set; }

		protected override void SerializeInternal(DataSerializerBase _s, object _customData = null)
		{
			Filename = _s.SerializeString("Name", Filename);
			Hash = _s.SerializeString("Hash", Hash);
		}
	}

	public List<DataFile> FileMap { get; set; }

	public FirstContact()
	{
		FileMap = new List<DataFile>();
	}

	public void Serialize(DataSerializerBase _s)
	{
		FileMap = _s.SerializeList("Filemap", FileMap);
	}

	public DataFile GetDataFile(string _filename)
	{
		return FileMap.First((DataFile _dataFile) => _dataFile.Filename == _filename);
	}
}
