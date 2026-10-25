using System;
using System.Collections.Generic;
using Assets.Scripts.Network.Error;
using UnityEngine;

namespace Assets.Scripts.Network.Model
{
	public class LangKey : IJsonExternizable
	{
		public int Neutral { get; set; }

		public int Male { get; set; }

		public int Female { get; set; }

		private void throwIOException(string message)
		{
			Debug.Log(message);
			throw new AcierErrorException(Assets.Scripts.Network.Error.Error.DEFAULT);
		}

		public void ReadExternal(Dictionary<string, object> _data)
		{
			if (_data == null)
			{
				throwIOException("LangKey: _data is null");
			}
			if (!_data.ContainsKey("version"))
			{
				throwIOException("LangKey: version tag not found");
			}
			double num = double.Parse(_data["version"].ToString());
			if (num != 1.0)
			{
				throwIOException("LangKey: version mismatch");
			}
			if (!_data.ContainsKey("docType"))
			{
				throwIOException("LangKey: docType tag not found");
			}
			string text = _data["docType"] as string;
			if (!text.Equals("LangKey"))
			{
				throwIOException("LangKey: docType mismatch");
			}
			if (_data.ContainsKey("neutral"))
			{
				Neutral = int.Parse(_data["neutral"].ToString());
			}
			if (_data.ContainsKey("male"))
			{
				Male = int.Parse(_data["male"].ToString());
			}
			if (_data.ContainsKey("female"))
			{
				Female = int.Parse(_data["female"].ToString());
			}
		}

		public void ReadExternal(string _data)
		{
			throw new NotImplementedException();
		}

		public void WriteExternal(out string _data)
		{
			throw new NotImplementedException();
		}
	}
}
