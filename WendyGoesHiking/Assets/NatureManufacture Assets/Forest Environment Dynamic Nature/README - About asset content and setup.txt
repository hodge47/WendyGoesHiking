To run HD or LW RP versions please visit HD and LW RP Support Packs folder and check readme files.

This pack is 100% scanned forest environment. 
Please use trees with numbers 6,7,8  for forest as they were optimised for forest environment, they have low polygons with detailed barks etc. Their leaves pass alot light into ground so forest will look properly.

We convert HD RP wind into lower engine versions so you could use it at lw, standard and hd rp aswell.

After you import this pack please:
- import unity post processing 2 if you want to use unity post process. It's in unity package manager. Window -> Package Manager -> Post Processing
- change quality level to ultra
- change shadow distance to higher in quality settings at least like 120 or 150
- change color space at player settings to linear as we setup pack to linear rendering
- change render type at graphics settings to deferred. We use many reflection probes at scene and at forward they could be heavy. OFc you can switch them off and use forward rendering aswell.
- if fps is avarage you could reduce LOD BIAS to 1.5 or 1 in most cases it will help alot for lower end devices.

We put our RAM system trial - few particles and NON tesseled water shaders. If you would like to create new rivers and lakes and roads via our splines and systems you need RAM 2019 in your project.
It's available here: https://assetstore.unity.com/packages/tools/terrain/r-a-m-2019-river-auto-material-2019-145937?aid=1011l7vcu
After you import it you could change river and swamp materials into tesseled version and they will look much much better if you want to use tesselation ofc. 
This system will be able to carve, paint, simulate lakes and rivers.

We setup LOD pretty short at this models as forest is very dense area  and we want to keep very high fps.
