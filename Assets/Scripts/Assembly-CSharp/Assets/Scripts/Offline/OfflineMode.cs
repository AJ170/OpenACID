using System;
using System.Collections.Generic;
using System.IO;
using AcierProtocol;
using UnityEngine;

namespace Assets.Scripts.Offline
{
	// Runs the game without Ubisoft's servers (shut down). What the servers sent at startup comes from a local copy
	// of the CDN data (first contact file, game database, bundle dictionaries, bundles, missions) and from the
	// defaults below (player profile, assassin class, tutorial mapping).
	//
	// What the client needs offline was worked out by the AC Identity Offline Revival Kit
	// (github.com/CROWNZENYTH-70/acid-identity-revival), which patches the Android assemblies; this is a source
	// reimplementation of the same changes.
	public static class OfflineMode
	{
		public static readonly bool Enabled = true;

		// The local CDN copy: FirstContact_Android.xml, AssetBundleDict_*.xml, GameDB_*.bin, AssetBundles/,
		// RuntimeAssets/, Missions/ (the layout of the CDN folder ACID_<version>/).
		public static string DataRoot
		{
			get
			{
				if (Application.platform == RuntimePlatform.PSP2)
				{
					return "ux0:data/ACID/AcierData";
				}
				// Editor / desktop: AcierData next to the project folder (the original LocalFileSystem location).
				return Path.GetFullPath(Path.Combine(Application.dataPath, "../../AcierData")).Replace('\\', '/');
			}
		}

		public static string DataUrl
		{
			get
			{
				return "file://" + DataRoot;
			}
		}

		// Mission to start in instead of the Animus hub: the first line of <DataRoot>/boot.txt, a path relative to
		// the data root such as "Missions/tutorial_0001.bin". No file (or an empty one): the hub.
		public static string BootMission
		{
			get
			{
				try
				{
					string path = Path.Combine(DataRoot, "boot.txt");
					if (File.Exists(path))
					{
						string[] lines = File.ReadAllLines(path);
						if (lines.Length > 0 && lines[0].Trim().Length > 0)
						{
							return lines[0].Trim();
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("OfflineMode: boot.txt unreadable: " + ex.Message);
				}
				return null;
			}
		}

		// Berserker body, weapon and head (live GameDB entries).
		private const string BerserkerHumanoid = "2183e8fb-3d3e-4f20-bd68-1ed2657dba9c";

		private const string BerserkerWeapon = "7d63cb2a-eecd-4e8c-bf8d-cdfe658d3faf";

		private const string BerserkerHead = "77d5f907-1c42-41bd-a94e-283d1a274fff";

		private const string AssassinId = "offline_ber01";

		// Given to the intro tutorial as its "finished" entitlement: nobody owns it, so the intro counts as not done.
		private const string IntroTutorialLock = "offline_tutorial_lock";

		// Enemy GUIDs used by the revival kit's mission files. They exist in the GameDB as other item types, so they
		// are mapped to the GUID hash of a live NpcData definition before the lookup.
		private static readonly Dictionary<string, int> s_guidAliases = new Dictionary<string, int>
		{
			{ "520620a5-024d-4633-aaf8-21a5f0db7903", -1861353487 }, // enemy_papal_medici
			{ "2d2ca21b-99e9-4a71-ad13-68dd5748574c", -1600799789 }, // enemy_guardcaptain_medici
			{ "d91147f6-5386-4ba7-b5b2-0334051fac5b", -1847913466 } // enemy_crossbowman_medici
		};

		public static bool TryGetGuidAlias(string _guid, out int _guidHash)
		{
			_guidHash = 0;
			return Enabled && _guid != null && s_guidAliases.TryGetValue(_guid, out _guidHash);
		}

		public static UserProfile BuildProfile()
		{
			Assassin assassin = new Assassin
			{
				id = AssassinId,
				classTypeId = Assassin.EClassTypeId.Berserker,
				extendedClassTypeId = Assassin.EClassTypeId.Berserker,
				level = 1,
				hitPoints = 90L,
				strength = 25L,
				precision = 25L,
				baseWeapon = 23L,
				armoreRating = 13L,
				incognito = 20L,
				humanoidGuid = BerserkerHumanoid,
				weaponGuid = BerserkerWeapon,
				headGuid = BerserkerHead,
				equippedElements = new EquippedElement[0],
				assassinName = new AssassinName
				{
					overrideName = "Assassin"
				}
			};
			return new UserProfile
			{
				id = "offline",
				userName = "Assassin",
				level = 1,
				lastUserAssassionId = AssassinId,
				Inventory = new Inventory
				{
					maxSize = 100,
					elements = new InventoryElement[0]
				},
				assassins = new Assassins
				{
					assassins = new Assassin[1] { assassin }
				},
				resources = new AcierProtocol.Resources
				{
					resources = new Resource[0]
				},
				counters = new Counters
				{
					counters = new Counter[0]
				},
				tutorial = new Tutorial
				{
					categories = new Tutorial.Category[0]
				},
				missionCollection = new MissionCollection
				{
					assassinMissions = new MissionCollection.AssassinMissions[0]
				},
				achievements = new AcierProtocol.Achievements
				{
					entries = new Achievement[0]
				},
				userSettings = new UserSettings
				{
					pushNotificationSettings = new PushNotificationSetting[0]
				}
			};
		}

		public static AvatarClassDefinitions BuildAvatarClasses()
		{
			return new AvatarClassDefinitions
			{
				classes = new AvatarClassDefinitions.AvatarClass[1]
				{
					new AvatarClassDefinitions.AvatarClass
					{
						classTypeId = Assassin.EClassTypeId.Berserker,
						humanoidGuid = BerserkerHumanoid,
						weaponGuid = BerserkerWeapon,
						heads = new AvatarClassDefinitions.AvatarClass.AvatarHeadSettings
						{
							headGuid = BerserkerHead
						},
						tiers = new AvatarClassDefinitions.AvatarClass.AvatarTier[0],
						hirelingAbilityGuids = new string[0]
					}
				}
			};
		}

		public static TutorialCategoryMapping BuildTutorialMapping()
		{
			Array categories = Enum.GetValues(typeof(ETutorialCategory));
			TutorialCategoryMapping.Entry[] entries = new TutorialCategoryMapping.Entry[categories.Length];
			for (int i = 0; i < categories.Length; i++)
			{
				ETutorialCategory category = (ETutorialCategory)categories.GetValue(i);
				entries[i] = new TutorialCategoryMapping.Entry
				{
					tutorialCategory = category,
					grantedEntitlements = (category != ETutorialCategory.TutCatIntroMission) ? new string[0] : new string[1] { IntroTutorialLock },
					reqEntitlements = new string[0]
				};
			}
			return new TutorialCategoryMapping
			{
				entries = entries
			};
		}
	}
}
