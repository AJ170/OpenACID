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

## Not done yet

- **Server bypass.** Startup downloads the first-contact file, game database and
  localisation from Ubisoft's servers, and missions/characters/levels come from CDN asset
  bundles. None of that content is in this rip (only the APK's menus and UI are).
- **Mission content.** The downloaded bundles were cached on the phone under the game's
  `persistentDataPath`. They are Android bundles and have to be ripped and rebuilt for PSP2;
  `GameFolders.GetCurrentAssetbundlePlatform` also needs a PSP2 case.
- **Physical controls.** Touch works; mapping the Vita buttons/sticks needs an InControl
  device profile.
- **Memory/performance on hardware.** The `~iPad` UI atlases are 2048² RGBA32. Measure
  on a Vita before compressing or splitting them (NGUI sprite rects are in pixels, so the
  textures cannot simply be imported smaller).
