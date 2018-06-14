# Sansar
C# Code for Project Sansar

This repository has my code I have been doing inside of the Linden Labs Project Sansar Virtual World Software.

BPMBlock - Code inside of the Beats Per Minute Blocks on the Specials Rack in the Sample Warehouse.  This Script sets the Tempo and starts a "click track" that the rest of the tracks in the Raver Script syncs to.

BeatBlock2 - Code inside of all the Sample Blocks on all the Sample Racks in the Sample Warehouse.  This passes the sample and timing information of the sample to the Raver Program and controls the movement of the Beat Block from the floor of the Bin, to the Display over the Bin and eventually to the reshelving of the block in the Sample Warehouse.

BeatBlockConfig - This code is the second script of every Beat Block containing samples.  This has the data associated with the Beat Block including the Sample to Play and the length in beats of the sample.  It passes the data to the beatblocks2 script in the Beat Block via Reflective.  There are two scripts to separate the code from the data associated with a Beat Block.

BeatBlockRaverV0 - This is the main code that creates the music it reads data from Beat Blocks dropped into the Beat Bins and then assigns them to Tracks.  The Tracks are played together and synched to the "Click Track" that is associated with the BPM Block.  This is a very long program and handles all the music part of the Beat Block Experience.

OnOffSign - simple script that displays the On and Off Signs after the Avatar starts the game by stepping on the Start Beat Blocks button.

StopBlock - Code inside the Stop Block that passes stop information for either a given track/loop or the entire song if the Stop Block is placed in the Tempo Bin.  This Script just passes the Stop message to the BeatBlockRaver script that actually shuts down the playing of the samples.

VolumeBlock - Code inside the Volume Block that passes volume control information for a given track or for the entire song if the Volume Block is placed in the Tempo Bin.  This Script just passes the volume chnage informationn to the BeatBlockRaver script that actually changes the volume of the samples.

StandAloneInstrument - Code Associated with the Synthesizers in the EXperience.  This code allows you to play along with the song by pressing the PC keyboard keys 1 through 0 on the top of the keyboard and 0 through 9 on the key pad.  You can also play an aditional 20 notes by using the shift key along with the number keys to get access to playing up to 40 different notes.  The StandAlone Instrument script currently is set to play a minor pentatonic scale in the correct key to match the key of the song being played in the BeatBlockRaver script.  A minor pentatonic scale means 5 notes per octave that are all matched to the key so you can never play a wrong note.  Whatever key you hit will match the music playing.  Also, this script listens for chat commands to chante the voice or instrument playing through the StandAlone Instrument.  

SawWaveInstrumentPack9 - is an example of a script that passes the actual samples to the StandAlone Instruments.  An instrument pack represents a voice or instrument that can be called into the StandAlone Instrument.  

gotMojoAlgoRaverV0- Live Music Coding in Sansar.  This program uses the Chat Window as a UI to be able to control loops that play samples, instruments and other special comands.  The musician can type in commands that create a set of beats or full songs.  The musician can have live performances in Sansar using this program to create EDM Music.


