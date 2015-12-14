Unity Debug.Log Wrapper
========================

This is a plugin for the Unity3D game engine that allows you to wrap your Debug.Log calls and write them to a file.

Usage:
------
```csharp
using UnityDebugLogWrapper;

public void Start() 
{
	Log.Init("Game.log", "My Game Name Log"); //creates file Game.log in your game directory, initializes log with title "My Game Name Log"

	Log.Write("First line of log");

	var item = "format";

	Log.Write("Also supports string {0}", item);
}
```
This will create Debug.Log statements in your unity console and also produce a text file in a similar format with timestamps:

```
--------------------------------
------- My Game Name Log -------
--------------------------------
Current Time: Dec 13 2015 19:22:20

19:22:20 -- Initialize Log File
19:22:20 -- First line of log
19:22:21 -- Also supports string format
```

Installation
------------
Go to the Releases tab and download the packaged .dll file, drop it anywhere in the Assets folder of your project (personally I keep it in Assets/Plugins) and you should be able to reference it to your code.

Let me know if you've got any suggestions by raising an issue!
