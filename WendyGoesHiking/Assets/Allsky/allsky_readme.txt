WHAT'S NEW IN ALLSKY V4. 
 
 	- Sky but even more so.

 		AllSky now contains over 200 skies. 

 		Version4 - A whole new set of very high detail skies with a 16k equirect texture.
 		
 		Space - Some cool new space scenes.
 		
 		Cartoon - More simple gradients for use as a base if you want to add your own clouds.
 		 		
 		HDRIHaven - A special note : HDRI Haven provides Creative Commons Zero (CC0) HDRI photo-sourced equirectangular textures. I have taken those that showed full and interesting skies and extensively edited them (tonemapped, removed the horizon, graded) and including them in Unity usable formats with demo scenes.  Note they are not HDR in the AllSky versions. Those can be downloaded for free at https://hdrihaven.com, check them out if you want access to more HDR cubemaps. Support HDRIHaven's patreon if you want to see even more! https://www.patreon.com/hdrihaven/overview

 		Misc - A few new ones here and there and fixes to old skies.

 	- Equirectangular source textures. Previously all skies were provided in the 6-sided format. Now 99% of skies have their equirectangular equivalent in the folder too.  You can choose which you want to use. I have heard from some users who find the equirect version more suited to their projects. 
 	You may also find them easier to edit in an imagine editing application as they are a single texture. But be mindful of accidentally creating seams at the edges, and the distortion at the top and bottom of the texture.

WHAT'S NEW IN ALLSKY V3

	- Even More Skies -	20 extra skies added for a new total of 160!  

		Epic - A selection of absolutely vast and detailed sunsets, storms and cloud formations.

		Haze - The sun is out but the atmosphere is dense and foggy.

		Sunless & Moonless - Fulfilled frequent request for skies which have no distinct sun or moon position. These have no sun or moon glare or object on them and clouds which don't portray any distinct illumination direction. They are also pleasingly reminiscent of Sega arcade games - 'Blue Sky Gaming'.

		Space - Two new space skyboxes. These are fairly dark cloud nebulae and have no stars baked onto them so you can add non-texture resolution dependent stars via your own means (such as particles, shaders or object billboards).

	- Skies now import at full resolution.

WHAT'S NEW IN ALLSKY V2

	- New Skies -
	23 extra skies added for a new total of 140!  Fantasy skies at different times of day with very dramatic clouds and colours. Cartoon skies in distinctive painterly, watercolour, airbrushed or pixelated styles.  Space skyboxes with epic, colourful nebulae.  More overcast skies for moody scenes.

	- New Format -
	Skies re-exported in a new format. From equi-rectangular to 6 sided. This results in a small quality increase, and should make life easier for developers on certain platforms.

	- Lighting Examples -
	All 140 skies now have a low poly demo environment with example lighting and fog pass. Great if you want a reference for colour and luminance values.
	Please set your project to the deferred rendering path and linear lighting color space to view these as intended.






ALLSKY

	A full palette of 200+ skies for Unity! Provided as 6 sided cubemaps sized from x1024 to x2048 per-side along with an equirectangular cubemap texture ranging from 4k to 16k in size. Each has an example lighting setup scene!

	Various styles: Day, Night, Cartoon, Fantasy, Hazy, Epic, Space, Sunless and Moonless!

	For lighting artists, environment artists and indie developers looking for a wide suite of skies to light their environments.

	Lighting from day to night: Twilight, sunset, multiple times of day, multiple times of night, skyglow.

	Many weather and cloud types: Clear, overcast, summery, stormy, autumnal, hazy, epic, foggy, cumulus.

	Tested in Unity 5.6 - Unity 2017 LTS - Unity 2018.3.

TECHNICAL

	Texture format: Each sky is a 6 sided cubemap. Source PNG texture resolution per-side ranges from x1024 to x2048.  Equirectangular images vary in size up to 16k textures.  

	Skies are sorted by time of day or style in folders. 
	Each individual sky has a folder which contains the textures and a material with those textures assigned. 
	There is also a demo scene with example lighting and fog pass for reference.

	Each sky has its own 6 sided skybox material which you can set to your scene's current skybox. 
	Please consult the Unity documentation if you are unsure how to do this.
	http://docs.unity3d.com/Manual/HOWTO-UseSkybox.html

	There is also an equirectangular material. Some users report that this is preferable in their use-case or build platform.

	The materials are mostly set as /mobile/skyboxes shaders - which should be fastest - but you can change them to the other skybox shaders that ship with Unity and set the required textures. Some add tint, exposure and rotation controls.

	The import resolution and type of compression used on the sky textures is entirely up to you.  It should be set at a level which you feel utilises appropriate amounts of memory for your game amd platform, taking into account the amount of compression artifacts that you feel are acceptable.

DEMO SCENE

	Each sky folder also has a demo scene. This shows a simple low-poly environment to demonstrate lighting and fog settings for that sky.  

	It was lit in the Forward Lighting Rendering Path with Linear lighting Color Space. 
	For intended demo scene lighting values and fog to be visible you will need a project with those settings.
	(Under Edit->Project Settings->Player)
	If you have to change these settings it may be necessary to re-import the sky textures.

	The demo scene can benefit from increasing the Pixel light count in quality settings, and the Shadow Distance.

WHO

	This asset pack is by Richard Whitelock.
	A game developer, digital artist & photographer.
	15+ years in the games industry working in a variety of senior art roles on 20+ titles. 
	Particularly experienced in environment art, lighting & special FX.
	Currently working on various indie game & personal projects. 

	http://www.richardwhitelock.com

	http://www.twitter.com/rpgwhitelock/

