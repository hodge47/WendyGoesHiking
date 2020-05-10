About scene:
We set all plants and grass as tree objects as  unity HD RP doesn't support terrain grass systems. 

No grass system affect the performance. That's why number of saved by baching is so huge but...performance at scene should be good anyway. About 60 FPS+
We will change this as soon unity will support terrain grass or something that we could use to build proper scene. (It should be very soon)

BEFORE YOU START:
- you need Unity  2019.3 
- you need HD SRP pipline 7.18 if you use higher etc custom shaders could not work but seams they should. 
Thats why we provide 7.2 version which seams to work with much higher versions aswell. 
For all higher RP versions please use 7.2 HD RP support pack.

Be patient this tech is so fluid... we coudn't fallow ever beta version

Step 1 - Setup Shadows and other render setups. Find File "HDRenderPipelineAsset" 
    - Change shadow atlas width and height to 2048 or 4096, Rather this first one.
	- !!!! IMPORTANT !!!! Find Material section at "HDRenderPipelineAsset" and drag and drop our SSS settings diffusion profiles for foliagec and water into Diffusion profile list:
		  NM_SSSSettings_Skin_Foliage
		  NM_SSSSettings_Skin_NM Foliage
		  NM_SSSSettings_Skin_NM Foliage Trees
		  NM_SSSSettings_Water_Forest
	Without this foliage, water materials will not become affected by scattering and they will look wrong.
	"Open HDRenderPiplineAsset" and:
	- Optionaly turn on "high quality" resolution at volumetrics (a bit expensive but I didn't notice big drop so..) 
	- Check if you got Deferred only in Lit Shader mode.
	- Check if contact shadows are turned on
	- LOD Bias to = 1 or 1.5

Step 2 Go to project settings and quality and set:
	- Set VSync to don't sync

Step 3 Find "HD RP Forest Demo Scene" and open it.

Step 4 Find "Post Process Volume" at scene hierarchy and turn "on" depth of field - make note that in windows 7 it could make sky black - seams unity engine bug 


Step 5 - chose way of movment. Movie track or free movment.
	Chose camera and turn on or off "playable directior" and "animation" or leave free camera movment turned on.

Step 6 - HIT PLAY!:)

About scene construction:
		- There is post process profile: Post Process Volume. Manage post process by scene post process object.
		- There is Sky and Fog Volume object, It's are important like hell because basicly it's core of rendering and light managment.
		- There are Density Volume objects which manage volumetric fog density in forest
		- Prefab wind manage wind speed and direction at the scene

Play with it give us feedback and lern about hd srp power.

