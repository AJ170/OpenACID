using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AcierNetwork.Communication;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.Network.Missions.Mission.Serialization;
using BB.Mobile.Net;
using BB.Mobile.Net.Http;
using Database;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UbiCore.Helpers;

public class MissionLoaderProvider : IProvider
{
	public class Input : Params
	{
		public string MissionUrl { get; set; }

		public MissionFile FullMissionProto { get; set; }

		public MissionGoals MissionGoals { get; set; }

		public bool IsEliteMission { get; set; }
	}

	public class ResultData : Model
	{
		public Assets.Scripts.Network.Missions.Mission.Serialization.SerializedNode MissionNode;

		public BBHashSet<AssetBundleInfo> RuntimeAssetBundles;

		public MissionRootData MissionRootData;

		public MissionGoals MissionGoals { get; set; }

		public bool IsEliteMission { get; set; }
	}

	private Query<ResultData> m_currentRequest;

	private readonly HttpClient mHttpClient = new HttpClient(null, null, false, 60000, 1);

	public bool PotentialConnectionIssue { get; private set; }

	private ResultData Data { get; set; }

	public override bool Accepts(Type _type)
	{
		return _type == typeof(ResultData);
	}

	public override bool BeginRead<T>(AcierNetwork.Communication.Request _request)
	{
		if (m_currentRequest != null || !Accepts(typeof(T)))
		{
			return false;
		}
		m_currentRequest = (Query<ResultData>)_request;
		return true;
	}

	public override void Initialize()
	{
	}

	protected override IEnumerator UpdateAsync()
	{
		Input input = (Input)m_currentRequest.Param;
		Data = new ResultData
		{
			RuntimeAssetBundles = new BBHashSet<AssetBundleInfo>()
		};
		HashSet<int> checkedGuids = new HashSet<int>();
		if (!string.IsNullOrEmpty(input.MissionUrl))
		{
			PotentialConnectionIssue = false;
			Uri val = new Uri(GameFolders.GetAcierDataURL(Main.Instance.BaseURL, input.MissionUrl));
			MemoryStream stream = new MemoryStream();
			IAsyncResult asyncResult = mHttpClient.BeginReadRequest(val, null, new StreamPayload(stream, false), null, this, null);
			yield return asyncResult;
			try
			{
				try
				{
					HttpResult httpResult = mHttpClient.EndRequest(asyncResult);
					if (!httpResult.Succeeded)
					{
						base.Result = new QueryResult(RequestResult.Status.Failed);
					}
					input.FullMissionProto = MissionSerializer.ReadBinaryStream<MissionFile>(stream);
				}
				catch (MobileNetException ex)
				{
					MobileNetException mobileNetException = ex;
					PotentialConnectionIssue = mobileNetException.PotentialConnectionIssue;
					throw;
				}
			}
			catch (Exception)
			{
				base.Result = new QueryResult(RequestResult.Status.Failed);
			}
			stream.Dispose();
		}
		if (input.FullMissionProto == null)
		{
			base.Result = new QueryResult(RequestResult.Status.Failed);
		}
		else
		{
			if (input.FullMissionProto != null)
			{
				Profiling.Start("MissionLoaderProvider::Parsing Proto", float.MaxValue);
				Data.MissionNode = MissionSerializer.Convert(input.FullMissionProto);
				Data.MissionGoals = input.MissionGoals;
				Data.IsEliteMission = input.IsEliteMission;
				Profiling.Stop();
			}
			Profiling.Start("MissionLoaderProvider::retrieving GUIDs", float.MaxValue);
			GameDB gameDB = Main.Instance.GameDB;
			List<Assets.Scripts.Network.Missions.Mission.Serialization.SerializedNodeComponent> cmps = Data.MissionNode.ComponentsRecursive();
			for (int i = 0; i < cmps.Count; i++)
			{
				Assets.Scripts.Network.Missions.Mission.Serialization.SerializedNodeComponent cmp = cmps[i];
				if (cmp.NodeData == null)
				{
					continue;
				}
				switch (cmp.NodeData.GetType().Name)
				{
				case "MissionRootData":
				{
					MissionRootData data = (MissionRootData)cmp.NodeData;
					Data.MissionRootData = data;
					if (!string.IsNullOrEmpty(data.PlayerAssassinGUID))
					{
						AddGuid(data.PlayerAssassinGUID, checkedGuids, gameDB);
					}
					if (!string.IsNullOrEmpty(data.PlayerHirelingGUID))
					{
						AddGuid(data.PlayerHirelingGUID, checkedGuids, gameDB);
					}
					break;
				}
				case "MissionNpcSpawnData":
				{
					string guidString = ((MissionNpcSpawnData)cmp.NodeData).TypeGuid;
					AddGuid(guidString, checkedGuids, gameDB);
					break;
				}
				case "ObjectiveSpawnData":
				{
					ObjectiveSpawnData objectiveSpawnData = (ObjectiveSpawnData)cmp.NodeData;
					for (int index = 0; index < objectiveSpawnData.SpawnFrames.Length; index++)
					{
						AddGuid(objectiveSpawnData.SpawnFrames[index].Guid, checkedGuids, gameDB);
					}
					break;
				}
				case "MissionSpawnObjectData":
				{
					string guidString2 = ((MissionSpawnObjectData)cmp.NodeData).TypeGuid;
					int hash = guidString2.GetHashCode();
					if (!checkedGuids.Contains(hash))
					{
						AddBundlesToResult(gameDB.FindGameIdFromGuid(guidString2));
						checkedGuids.Add(hash);
					}
					break;
				}
				}
			}
			Profiling.Stop();
			base.Result = new QueryResult(RequestResult.Status.Suceeded);
		}
		m_currentRequest.EndRead(base.Result, Data);
		base.Result = null;
		m_currentRequest = null;
		Data = null;
	}

	private void AddGuid(string guidString, HashSet<int> checkedGuids, GameDB gameDB)
	{
		int hashCode = guidString.GetHashCode();
		if (!checkedGuids.Contains(hashCode))
		{
			GameID gameID = gameDB.FindGameIdFromGuid(guidString);
			AddBundlesToResult(gameID);
			checkedGuids.Add(hashCode);
		}
	}

	private void AddBundlesToResult(GameID gameID)
	{
		DBEntry item = Main.Instance.GameDB.GetItem<DBEntry>(gameID);
		for (int i = 0; i < item.DepAssetBundlesInfos.Length; i++)
		{
			short id = item.DepAssetBundlesInfos[i];
			AssetBundleInfo assetBundleInfoByBundleId = Main.Instance.GameDB.GetAssetBundleInfoByBundleId(id);
			if (!Data.RuntimeAssetBundles.Contains(assetBundleInfoByBundleId))
			{
				Data.RuntimeAssetBundles.Add(assetBundleInfoByBundleId);
			}
		}
	}
}
