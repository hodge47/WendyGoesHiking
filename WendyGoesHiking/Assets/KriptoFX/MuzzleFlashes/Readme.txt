My email is "kripto289@gmail.com"
You can contact me for any questions.

My English is not very good, and if there are any translation errors, you can let me know :)


Pack includes prefabs of impact effects (decals/particles/etc) + muzzle flash effects (fire/light, etc)

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Supported platforms:

	All platforms (PC/Consoles/VR/Mobiles)
	All effects tested on Oculus Rift CV1 with single/dual/instanced pass and works correcrly.
	Supported SRP rendering. LightWeight render pipeline (LWRP) and HighDefinition render pipeline (HDRP)

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Using on PC:
	If you want to use posteffect for PC like in the demo video:

	1) Download unity free posteffects 
	https://assetstore.unity.com/packages/essentials/post-processing-stack-83912
	2) Add "PostProcessingBehaviour.cs" on main Camera.
	3) Set the "PostEffects" profile. ("\Assets\KriptoFX\MuzzleFlashes\ImagePostEffects\PostEffectsProfile.asset")
	4) You should turn on "HDR" on main camera for correct posteffects. (bloom posteffect works correctly only with HDR)
	If you have forward rendering path (by default in Unity), you need disable antialiasing "edit->project settings->quality->antialiasing"
	or turn of "MSAA" on main camera, because HDR does not works with msaa. If you want to use HDR and MSAA then use "post effect profile -> antialiasing". 
	It's faster then default MSAA and have the same quality.

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Using on MOBILES:
	For correct work on mobiles in your project scene you need:

	1) Add script "FPS_MobileBloom.cs" on main camera and disable HDR on main camera. That all :)
	You need disable HDR on main camera for avoid rendering bug on unity 2018+ (maybe later it will be fixed by unity).

	This script will render scene to renderTexture with HDR format and use it for postprocessing. 
	It's faster then default any posteffects, because it's avoid "OnRenderImage" and overhead on cpu readback. 
	(a source https://forum.unity.com/threads/post-process-mobile-performance-alternatives-to-graphics-blit-onrenderimage.414399/#post-2759255)
	Also, I use RenderTextureFormat.RGB111110Float for mobile rendering and it have the same overhead like a default texture (RGBA32)


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Using effects:

Simple using (without characters/weapons):

	1) Place "\Assets\KriptoFX\MuzzleFlashes\Prefab\FireManager.prefab" to a gun muzzle position. 
	2) Add for each objects with collision (walls/floor etc) script "MaterialType.cs" and set type of material (for example, for brick wall set "brick")

