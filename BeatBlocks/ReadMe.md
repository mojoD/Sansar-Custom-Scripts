# Beat Blocks Code
C# Code for Project Sansar

This repository has my code for the Beat Blocks Experience in Sansar.  It is an Electronic Dance Music Creation Experience using the following code:

**BPMBlock** - Code inside of the Beats Per Minute Blocks on the Specials Rack in the Sample Warehouse.  This Script sets the Tempo and starts a "click track" that the rest of the tracks in the Raver Script syncs to.

**BeatBlock2** - Code inside of all the Sample Blocks on all the Sample Racks in the Sample Warehouse.  This passes the sample and timing information of the sample to the Raver Program and controls the movement of the Beat Block from the floor of the Bin, to the Display over the Bin and eventually to the reshelving of the block in the Sample Warehouse.

**BeatBlockConfig** - This code is the second script of every Beat Block containing samples.  This has the data associated with the Beat Block including the Sample to Play and the length in beats of the sample.  It passes the data to the beatblocks2 script in the Beat Block via Reflective.  There are two scripts to separate the code from the data associated with a Beat Block.

**BeatBlockRaverV0** - This is the main code that creates the music it reads data from Beat Blocks dropped into the Beat Bins and then assigns them to Tracks.  The Tracks are played together and synched to the "Click Track" that is associated with the BPM Block.  This is a very long program and handles all the music part of the Beat Block Experience.

**OnOffSign** - simple script that displays the On and Off Signs after the Avatar starts the game by stepping on the Start Beat Blocks button.

**StopBlock** - Code inside the Stop Block that passes stop information for either a given track/loop or the entire song if the Stop Block is placed in the Tempo Bin.  This Script just passes the Stop message to the BeatBlockRaver script that actually shuts down the playing of the samples.

**VolumeBlock** - Code inside the Volume Block that passes volume control information for a given track or for the entire song if the Volume Block is placed in the Tempo Bin.  This Script just passes the volume chnage informationn to the BeatBlockRaver script that actually changes the volume of the samples.


