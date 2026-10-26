# AC Identity → Unity 2017.4.4 / PS Vita

Source: the legacy AssetRipper export of the Android build (Unity 5.2.3p1).
The untouched export is the first git commit; everything after it is the port.

## Building

Unity 2017.4.4f1 with the PS Vita (PSP2) module and the PSP2 SDK.

- Editor: menu **AC Identity → Build PS Vita** (output in `Build/PSVita`).
- Batch:
  ```
  "C:\Program Files\Unity\2017\Editor\Unity.exe" -batchmode -quit -projectPath <this folder> -buildTarget PSP2 -executeMethod ACIDVitaBuild.BuildBatch -logFile Logs/build.log
  ```
  Add `-acidDevelopment` for a development build.

Player settings are applied from code in `Assets/Editor/ACIDVitaBuild.cs` (IL2CPP, title
`ACID00001`, +109 MB memory expansion, power mode C, gamma colour space).

The build output is the Unity PSP2 layout (`PSVita.self`, `Media/`, `sce_sys/`,
`sce_module/`); package it into a VPK the same way as your other Unity Vita projects.
`Assets/Editor/ACIDDiagnostics.cs` has batch helpers that open every build scene or load
every asset one by one, to attribute load-time errors.

## Shaders

The rip only contained Unity 5.2 *compiled* GLES shaders (with junk bytes in front of every
file), which neither compile on 2017.4 nor run on the Vita GPU. All 103 shaders were rebuilt
as Cg source:

- `_RipReference/OriginalShaders/` – the originals (junk bytes stripped), kept as reference.
- `Tools/ShaderPort/specs/` – hand-ported Cg bodies, one `//@@PASS` per programmable pass.
- `Tools/ShaderPort/port_shaders.py` – rebuilds `Assets/**/*.shader` from the original
  ShaderLab shell (properties, tags, render states, fallback) plus the spec bodies. The
  `.meta` files are untouched, so every material keeps its shader reference.
- `Assets/Shader/Include/ACID*.cginc` – shared code (characters, environment, NGUI, UI,
  effects, blur, fog/SH/fade helpers).

Edit a spec or include, then run `python -I Tools/ShaderPort/port_shaders.py --check`.

Every formula was taken from the original GLSL. Deliberate changes, all for the Vita:

- `_LQ` character shaders and non-bumped environment shaders evaluate lighting per vertex
  (SH ambient, N·L, rim). The normal is per vertex there anyway; the result is the same for
  smooth lighting and the pixel shader drops to a texture fetch plus a few MADs.
- Keyword variants that produced identical pixels are not compiled (environment
  `FX_HIGHLIGHT`; shadow variants of shaders that ignored shadows).
- The obsolete `SHADOWCOLLECTOR` passes are dropped; shadow casters use Unity 2017 macros.
- Clip-space depth tricks (outlines, collider debug) go through `ACID_ClipZToGL` so they
  behave the same on reversed-Z platforms.
- The ripped built-in `Standard` and `BlitScaleCopy` shaders were removed; the two
  materials that used `Standard` now point at Unity's built-in one.

Normal maps are read with `UnpackNormal`; import them as *Normal map* textures.

## Scripts

- The original assembly split is restored with `.asmdef` files (`AcierProtocol`,
  `AcierProtocolSerializer`, `AcierNetwork`, `Mobile.Net`, `SharedBaseLib`,
  `SharedEnvironmentLib`); `protobuf-net` is the original DLL.
- Unity 5.5+ API moves (`UnityEngine.AI`, `UnityEngine.Profiling`,
  `AssetBundle.LoadFromFile`, removed `RuntimePlatform` members kept by value).
- Decompiler fixes: interface forwarding stubs (`Tools/Decompile/remove_interface_stubs.py`),
  `TypeModel` overrides, a lambda name clash, NGUI `ByteReader`, and the `Skills` string
  switch (the lost case label was recovered from the original assembly's string heap).
- `link.xml` keeps IL2CPP from stripping the game assemblies.
- Unity 5.4+ forbids engine calls while objects are being constructed/deserialised, which
  5.2 tolerated:
  - 13 `NavMeshPath` fields (combat, AI, movement) are now created on first use; before
    this `CombatController` failed to construct at all.
  - `CameraStateDead` no longer `new`s a MonoBehaviour (same fallback behaviour).
  - `CameraStateRoofEdge`'s constructor uses a reference check, not a native one.
  - `FriendTab.Friend` is explicitly non-serialised.
  - `UIExtendedLabel` skips localisation in edit mode (only matters for the editor/build).

## PS Vita behaviour changes

| Change | Where |
|---|---|
| Real multi-touch input (was mouse simulation) | `AcierInputHandler`, `UICamera` |
| System IME for text fields | `UIInput`, `TouchKeyboardCompat` |
| 220 DPI fallback → phone UI layout | `UIUtil`, `NGUIMath` |
| `PSVita` device profile: low bundles, native 960×544 (no half-res upscaler) | `QualitySettingsHelper`, `Startup` |
| `PSVita` quality level: vsync 1, no realtime shadows, no aniso | `ProjectSettings/QualitySettings.asset` |
| Missions at 60 fps (phones used 25) | `SceneLoader`, `Rendering` |
| SGX bloom blur path | `FxBasicBloom` |
| No low-end-device popup (light UI atlases still used) | `Main` |
| iOS Game Center backend and the logo movie compiled out | `ClientAchievementIOS`, `Main` |

## Offline mode (no servers)

`Assets/Scripts/Assembly-CSharp/Assets/Scripts/Offline/OfflineMode.cs`. What the client needs
without Ubisoft's servers was worked out by the AC Identity Offline Revival Kit
(github.com/CROWNZENYTH-70/acid-identity-revival, IL patches for the Android build); this is a
source reimplementation of it:

- Startup reads the CDN files from a local copy, `OfflineMode.DataRoot`
  (PS Vita `ux0:data/ACID/AcierData`, editor `../AcierData` next to the project): the layout of
  the CDN folder (`FirstContact_Android.xml`, `AssetBundleDict_*.xml`, `GameDB_*.bin`,
  `AssetBundles/`, `RuntimeAssets/`, `Missions/`).
- A local player profile (one Berserker), assassin class definitions and tutorial mapping
  replace the server statics; profile requests without a session are dropped
  (`ProfileManager.GenericDataStoreCall`).
- `<DataRoot>/boot.txt` (first line, e.g. `Missions/tutorial_0001.bin`) starts in that mission
  instead of the Animus hub. The kit's mission files (`mission/` in its repo) work with it: its
  three enemy GUIDs are aliased in `OfflineMode.TryGetGuidAlias`, and offline spawns snap to the
  navmesh.
- Bug fixes the kit found: lone NPCs without a formation, crowd bodies missing from the
  database, parcour cover on NPCs without a patrol, missions without objectives.

## Downloaded content (asset bundles)

Everything in the game except the menus came from 96 CDN bundles (17 levels, 79 content
bundles), Unity 5.2 Android ETC2. Pipeline:

1. `cache files/cache_named` / `cache_283_named`: the phone caches with the bundles under their
   CDN names (`catalogue.bin` maps them). `cache_283_named` is the 2.8.2 cache with the High
   quality bundles of the 2.7.0 cache swapped in, plus the 2.8.3 Mono DLLs in `Managed/`.
2. `Tools/BundleImport/bundle_index.py`: every bundle's explicit asset names (what the game
   loads), objects and dependencies (needs UnityPy) -> `cache_283_named/bundle_index.json`
   (42 MB, kept with the caches).
3. `Tools/BundleRip`: one AssetRipper export per bundle (a merged rip cannot tell same-path
   assets of different bundles apart). Built against the installed ULegacy AssetRipper DLLs.
4. `Tools/BundleImport/import_bundles.py`: copies each export to
   `Assets/BundleAssets/<bundle>/` (GUIDs kept), points shader references at the project's or
   Unity's built-in shader of the same name, and writes `_import/bundles.json` (per bundle: scene
   or not, every explicit name and its project path). `_import/shaders.json` lists the shaders
   the project does not have yet; their disassembly is in `Tools/ShaderPort/bundle_originals/`.

`Assets/BundleAssets/` is not in git (rerun step 4 to regenerate); `_import/` is.

In the editor the imported bundles are used directly (`Offline/LocalBundles.cs`, installed at
startup): `AssetBundleManager` treats a bundle listed in `_import/bundles.tsv` as local (no
download, no bundle file) and loads its assets with the AssetDatabase; level scenes load by path.
Bundle names are matched without platform and quality (`EnvAnimus_AnimusGlobe_Env`), so the
game's AndroidGeneric / Low requests find the imported ETC2 High assets. Offline, the up-front
download of every bundle is skipped.

Unattended Play-mode test (normal editor, not batch mode):
```
Unity.exe -projectPath <this folder> -executeMethod ACIDPlayTest.Begin -acidPlayTarget AnimusGlobe -logFile Logs/playtest_editor.log
```
writes `Logs/playtest.txt` (scenes loaded, every warning/error) and `Logs/playtest.png`.
## PS Vita content

The bundles are rebuilt for PSP2 from the imported assets and read straight from the data folder
(no download, no cache):

1. Import (`Tools/BundleImport/import_bundles.py`) also makes the textures Vita-ready
   (`textures.py`): the rip forces the Android ETC2 format on every texture (set to automatic, so
   PSP2 picks its own compression), and raw ETC textures (level lightmaps) are decoded to PNG under
   the same GUID.
2. **AC Identity → Build PS Vita Bundles** (`Assets/Editor/ACIDBundleBuild.cs`, batch:
   `-buildTarget PSP2 -executeMethod ACIDBundleBuild.BuildBatch`): one LZ4 bundle per imported
   bundle, named by `LocalBundles.BundleKey`, level scenes in their own bundles, plus `bundles.tsv`
   and the offline data from `../AcierData`, into `Build/PSVitaData/ACID/AcierData`.
3. **Build PS Vita** for the player, then a VPK: `eboot.bin` (= `PSVita.self`), `sce_sys/`,
   `sce_module/`, `Media/` from `Build/PSVita`.
4. On the Vita: install the VPK, copy `Build/PSVitaData/ACID` to `ux0:data/ACID`.

At runtime `LocalBundles.FileSource` opens `ux0:data/ACID/AcierData/Bundles/<key>` with
`AssetBundle.LoadFromFile` (kept open; LZ4 keeps only headers in memory) and loads assets by their
project path; scenes load by name after their bundle is open.

## Not done yet

- **Bundle shaders.** `Animus/PrimitiveFloatingFloor` and `Character/BfSkin` (Caribbean) are not
  ported yet; `Tools/BundleImport/shader_remap.py` points the materials at them once they are.
- **Physical controls.** Touch works; mapping the Vita buttons/sticks needs an InControl
  device profile.
- **Memory/performance on hardware.** The `~iPad` UI atlases are 2048² RGBA32. Measure
  on a Vita before compressing or splitting them (NGUI sprite rects are in pixels, so the
  textures cannot simply be imported smaller).
