
BEFORE YOU START:
- you need Unity 2019.3+
- you need URP SRP pipline 7.18 if you use higher please import 7.2 support pack.
- wind setup is in wind prefab at each scene
Be patient URP RP tech is still fluid and fresh...

Step 1 - Setup Shadows and other render setups. Find File "UniversalRP-HighQuality" 
    - Change shadow distance to 150 or 
	- Turn on "SRP Bacther" it will improve fps by 200% without it you should turn off reflection probes.(Seams instancing stop workin with probes and without srp batcher)
	- Turn on "Opaque Texture" this will fix water translucency and distortion
	- Turn on "Depth Texture" this will fix water visibility at playmode
	- Optionaly use 1k or 2k shadow resolution. We used 2k.
	- Turn on HDR if its turned off

Step 2 Go to project settings: 
    - Player and set:  Color Space to Linear
    - Quality settings: Go to quality settings and: 
	     * use ultra level 
	     * turn turn off vsync
		 * lod bias should be around 1.5-2 and 1 for low end devices.
                        

Step 3 Find "Forest Demo Scene" and open it.

Step 4 - chose way of movment. Movie track or free movment.
	Chose camera and turn on or off "playable directior" and "animation" or leave free camera movment turned on.

Step 5 - HIT PLAY!:)

About scene construction:
		- There is post process profile: Manage post process by scene post process object.
		- Prefab wind manage wind speed and direction at the scene

