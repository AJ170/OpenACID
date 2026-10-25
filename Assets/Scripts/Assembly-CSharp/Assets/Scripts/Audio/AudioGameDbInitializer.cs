using System;
using System.Collections.Generic;
using Database;
using UbiCore.Utils;

namespace Assets.Scripts.Audio
{
	public class AudioGameDbInitializer
	{
		public GameDB GameDb { get; private set; }

		public GameDB.SubDB AudioControllerDb { get; private set; }

		public GameDB.SubDB MixGroupDb { get; private set; }

		public GameDB.SubDB MixStateDb { get; private set; }

		public AudioGameDbInitializer(GameDB _db)
		{
			GameDb = _db;
			AudioControllerDb = InitAudioControllers(GameDb);
			MixGroupDb = InitMixGroups(GameDb);
			MixStateDb = InitMixStates(GameDb, MixGroupDb);
		}

		private GameDB.SubDB InitAudioControllers(GameDB _db)
		{
			UbiCore.Utils.Singleton<AudioGameIds>.Initialize();
			GameDB.SubDB subDB = GameDb.GetSubDB(typeof(AudioControllerDef));
			foreach (KeyValuePair<GameID, DBEntry> datum in subDB.Data)
			{
				AudioControllerDef audioControllerDef = (AudioControllerDef)datum.Value;
				for (int i = 0; i < audioControllerDef.SoundThemes.Count; i++)
				{
					AudioControllerDef.SoundTheme soundTheme = audioControllerDef.SoundThemes[i];
					if (soundTheme != null)
					{
						for (int j = 0; j < soundTheme.SoundProperties.Count; j++)
						{
							AudioControllerDef.SoundProperty soundProperty = soundTheme.SoundProperties[j];
							soundProperty.GameID = ((string.IsNullOrEmpty(soundProperty.SoundGuid) || !(soundProperty.SoundGuid != Guid.Empty.ToString())) ? GameID.Invalid : _db.FindGameIdFromGuid(soundProperty.SoundGuid));
							soundProperty.SoundGuid = string.Empty;
						}
					}
				}
				audioControllerDef.MixingGroupGameId = (string.IsNullOrEmpty(audioControllerDef.MixingGroupGuidString) ? UbiCore.Utils.Singleton<MixGroupGameIds>.Instance.Default : _db.FindGameIdFromGuid(audioControllerDef.MixingGroupGuidString));
				if (!GameID.IsValid(audioControllerDef.MixingGroupGameId))
				{
					audioControllerDef.MixingGroupGameId = UbiCore.Utils.Singleton<MixGroupGameIds>.Instance.Default;
				}
				audioControllerDef.MixingGroupGuidString = string.Empty;
			}
			return subDB;
		}

		private GameDB.SubDB InitMixGroups(GameDB _db)
		{
			UbiCore.Utils.Singleton<MixGroupGameIds>.Initialize();
			GameDB.SubDB subDB = GameDb.GetSubDB(typeof(AudioMixingGroupDef));
			foreach (KeyValuePair<GameID, DBEntry> datum in subDB.Data)
			{
				AudioMixingGroupDef audioMixingGroupDef = (AudioMixingGroupDef)datum.Value;
				for (int num = audioMixingGroupDef.DuckingInfos.Count - 1; num >= 0; num--)
				{
					AudioMixingGroupDef.DuckingInfo duckingInfo = audioMixingGroupDef.DuckingInfos[num];
					duckingInfo.MixingGroup = ((string.IsNullOrEmpty(duckingInfo.MixingGroupGuidString) || !(duckingInfo.MixingGroupGuidString != Guid.Empty.ToString())) ? GameID.Invalid : _db.FindGameIdFromGuid(duckingInfo.MixingGroupGuidString));
					if (!GameID.IsValid(duckingInfo.MixingGroup))
					{
						audioMixingGroupDef.DuckingInfos.RemoveAt(num);
					}
					duckingInfo.MixingGroupGuidString = string.Empty;
				}
			}
			return subDB;
		}

		private GameDB.SubDB InitMixStates(GameDB _db, GameDB.SubDB _mixGroupDb)
		{
			UbiCore.Utils.Singleton<MixStateGameIds>.Initialize();
			GameDB.SubDB subDB = _db.GetSubDB(typeof(AudioMixStateDef));
			foreach (KeyValuePair<GameID, DBEntry> datum in subDB.Data)
			{
				AudioMixStateDef audioMixStateDef = (AudioMixStateDef)datum.Value;
				for (int num = audioMixStateDef.DuckingInfos.Count - 1; num >= 0; num--)
				{
					AudioMixingGroupDef.DuckingInfo duckingInfo = audioMixStateDef.DuckingInfos[num];
					duckingInfo.MixingGroup = ((string.IsNullOrEmpty(duckingInfo.MixingGroupGuidString) || !(duckingInfo.MixingGroupGuidString != Guid.Empty.ToString())) ? GameID.Invalid : _db.FindGameIdFromGuid(duckingInfo.MixingGroupGuidString));
					if (!GameID.IsValid(duckingInfo.MixingGroup))
					{
						audioMixStateDef.DuckingInfos.RemoveAt(num);
					}
					duckingInfo.MixingGroupGuidString = string.Empty;
				}
				if (!audioMixStateDef.UseDefault)
				{
					continue;
				}
				foreach (KeyValuePair<GameID, DBEntry> datum2 in _mixGroupDb.Data)
				{
					bool flag = true;
					for (int num2 = audioMixStateDef.DuckingInfos.Count - 1; num2 >= 0; num2--)
					{
						AudioMixingGroupDef.DuckingInfo duckingInfo2 = audioMixStateDef.DuckingInfos[num2];
						if (duckingInfo2.MixingGroup == datum2.Key)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						AudioMixingGroupDef.DuckingInfo duckingInfo3 = new AudioMixingGroupDef.DuckingInfo(audioMixStateDef.DefaultState);
						duckingInfo3.MixingGroup = datum2.Key;
						audioMixStateDef.DuckingInfos.Add(duckingInfo3);
					}
				}
			}
			return subDB;
		}
	}
}
