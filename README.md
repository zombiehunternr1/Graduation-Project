# AR Escape Room
Graduation project for Fontys InnovationLab Eindhoven

<h1>Helpful documentation</h1>

To get the image detection working with AR I used Unity's AR tracked image manager:
- https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.0/manual/tracked-image-manager.html

<h3>Important insight!</h3>
To get a better understanding how Unity's AR basics work, I would advise you to follow this tutorial:

- https://learn.unity.com/course/create-with-ar-markers-and-planes?uv=2021.3

For making communication working over the network I've made use of Mirror:
- https://mirror-networking.gitbook.io/docs/

<h1>Project information</h1>

Within the <b>Assets folder</b> I have created the following folder structure for my project.

<h2>Project folder structure</h2>

The project has been categorized in the following folders:
- <b>_Project:</b> In this folder I keep all my different prototypes I have made during my graduation semester
- <b>Editor:</b> In this folder I keep my custom ID generator script that only works within the Unity Editor for the Wack-A-Mole prototype
- <b>Plugins:</b> In this folder I keep all the plugins I have been using during my graduation semester. For example, FMOD, Mirror, TMPro, etc.
- <b>ScriptTemplates:</b> Mirror's template scripts to manage network components
- <b>StreamingAssets:</b> FMOD automatically generated folder to store the master bank file for all the music and SFX
<h2>Project category structure</h2>

The project folder has been categorized in the following folders:
- <b>Art:</b> In this folder I store everything that is visuals related, for example, animations, materials, models, etc.
- <b>Code:</b> In this folder I store everything coding related, these are the different types of scripts and scriptable object assets
- <b>Level:</b> In this folder I store all my prefabs and scenes I have used in this project
<h2>Project prototypes structure</h2>

Inside each of the <b>category folders</b> I have made a subfolder that represents their respective reference. These are the following references:
- <b>General:</b> Scripts/Scriptable objects that I'll be using throughout all my prototypes
- <b>Buttons:</b> The first prototype to get AR interaction working with basic Mirror communication across the network
- <b>FirstARPuzzle:</b> Experiment to try and grab a puzzle piece and move it along with your phone's camera position
- <b>FirstPrototype:</b> Wack-A-Mole prototype that has working AR interaction implemented and synchronization over the network using Mirror
- <b>FindTheMatch:</b> The first mini-game that is being used for the AR escape room game
<h2>Project scripts/Scriptable Objects structure</h2>

In order to keep my scripts easy to find, I have made sub-folders for them that represents their functionality.
I'll be using the <b>Find The Match</b> prototype as an example:
- <b>General:</b> The script that handles when the key animation display should be played
- <b>AR:</b> In the <b>Scripts</b> folder I keep the script that handles the detecting and tracking of the image references. In the <b>Scriptable Objects</b> folder under <b>Games</b> I keep the image reference library asset that is being used to keep track of which images are being used as trackables
- <b>EventSystem:</b> In the <b>Scripts</b> folder I keep all the scripts that handle the communication that should be handled only on the server and over the network. Inside the <b>networking</b> folder I have separated the <b>Commands</b> that goes from the client to the server, and the <b>ClientRpc</b> that sends a task from the server to all currently connected clients.
- <b>Network</b>: In this folder I keep all the scripts that are being used network objects, these will exist as long as the session is active.
