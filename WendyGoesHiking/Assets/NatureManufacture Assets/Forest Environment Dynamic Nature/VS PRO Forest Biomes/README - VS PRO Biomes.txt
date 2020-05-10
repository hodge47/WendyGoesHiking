To build this scene we used:
• Forest Environment - Dynamic Nature (all 3d assets)
https://assetstore.unity.com/packages/3d/environments/fantasy/fantasy-forest-environment-34245?aid=1011lGkb
• Vegetation Studio Pro (foliage spawn and render system)
https://assetstore.unity.com/packages/tools/terrain/vegetation-studio-pro-131835?aid=1011lGkb
• River Auto Material 2019 - river, roads, lakes building
https://assetstore.unity.com/packages/tools/terrain/r-a-m-2019-river-auto-material-2019-145937?aid=1011lGkb
• CTS 2019 (terrain shading) 
https://assetstore.unity.com/packages/tools/terrain/cts-2019-complete-terrain-shader-140806?aid=1011lGkb

How to run demo from video:
• You need Unity 2019.2f3 or higher. (NON SRP) 
• Please do all step 1 by 1. It will save your time and confusion.

SETUP MANUAL:
1. Import Vegetation Studio PRO (Latest)
https://assetstore.unity.com/packages/tools/terrain/vegetation-studio-pro-131835?aid=1011lGkb
2. Setup project with VS project like it's mentioned here (Setting up unity part only): https://www.awesometech.no/index.php/setup-guide/
3. Setup project to linear color space in player settings
4. Setup project deferred rendering in Graphics Settings (not necessary but it improve speed ALOT) 
5. Import Post Process Stack 2 (if you don't have it) into your project from Window -Package Manager - Post Processing (click update or install)
6. Import R.A.M 2019! into your project (water shading, road and river splines, ground textures)
https://assetstore.unity.com/packages/tools/terrain/r-a-m-2019-river-auto-material-2019-145937?aid=1011lGkb
7. Import CTS 2019 – Complete Terrain Shader 2019 to your project, you dont have to import texture library. (terrain shader - not necessary but setup was made with CTS). 
https://assetstore.unity.com/packages/tools/terrain/cts-2019-complete-terrain-shader-140806?aid=1011lGkb
If you don't use CTS after scene import change material at terrain object back to standard shader at demo scene.
8. Import Forest Environment VS PRO files to your project into your project.
https://www.dropbox.com/s/ztyi68bt385497v/Forest%20VS%20Biomes.unitypackage?dl=0
9. Open scene called "Forest Environment VS PRO Demo"
10. Click Play
11. Low FPS -  for low end gpu turn off screen space reflections from post processing object (it's expensive) or play with vs settings.
12. Optional: Bake ambient light in window-rendering - lighting settings 
(It will break reflections a bit probably but nothing special)

More useful options about visual effects and optimisation: 
1. You could extend scene by drag and drop adding forest. Simply in biome profiles you will find Beech Forest Biome.
Drag and drop them and modify shape to get new forest objects. At the end you have to bake texture splatmap. 
2. You could expand foliage rendering distance in VegetationSystemPro objects in Vegetation Settings. (we use 400 for grass and additional 300 for trees at video)
If foliage distance will be high you probably should bake vegetation to spawn it smoothly. 
3. Reflections - make note that when you re-bake reflection probe you have to see area around probe because vs pro instance foliage only for camera.
We bake all probes with high view for foliage.
4. You could adjust shadow distance to 1350 like we did and improve shadow resolution to Very High Resolution.
5. What else? ENJOY and play with it, build a game or nice video! 
All best from NatureManufacture team and thanks for supporting us!