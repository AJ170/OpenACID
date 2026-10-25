using System;
using System.Collections.Generic;
using Assets.Scripts.Network.Error;
using MiniJSON;
using UnityEngine;

namespace Assets.Scripts.Network.Model
{
	public class NetworkMissionData : IJsonExternizable
	{
		public string MissionToken { get; set; }

		public string MissionGuid { get; set; }

		public string AvatarDefinition { get; set; }

		private void throwIOException(string message)
		{
			Debug.Log(message);
			throw new AcierErrorException(Assets.Scripts.Network.Error.Error.DEFAULT);
		}

		public void ReadExternal(string _data)
		{
			if (_data != null)
			{
				ReadExternal(Json.Deserialize(_data) as Dictionary<string, object>);
			}
		}

		public void ReadExternal(Dictionary<string, object> _data)
		{
			if (_data == null)
			{
				throwIOException("MissionToken: _data is null");
			}
			if (!_data.ContainsKey("version"))
			{
				throwIOException("MissionToken: version tag not found");
			}
			double num = double.Parse(_data["version"].ToString());
			if (num != 1.0)
			{
				throwIOException("MissionToken: version mismatch");
			}
			if (!_data.ContainsKey("docType"))
			{
				throwIOException("MissionToken: docType tag not found");
			}
			string text = _data["docType"] as string;
			if (!text.Equals("MissionToken"))
			{
				throwIOException("MissionToken: docType mismatch");
			}
			MissionToken = _data["mt"] as string;
			MissionGuid = _data["mid"] as string;
			AvatarDefinition = _data["ad"] as string;
		}

		public void WriteExternal(out string _data)
		{
			throw new NotImplementedException();
		}
	}
}
