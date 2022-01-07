### Introduction
Monitors the shutdown process of the Server. If the shutdown takes too long to complete it will hard-kill it and restart the server after that. 

This allows to get past the restart hangs that appear every now and then especially on huge servers. 

### Disclaimer
This plugin only really works on Dedicated machines where Torches restart Command also works.  Managed hosting systems where you only manage the server via web panel and ftp will have issues you cannot avoid. 

Most commonly the server being started, but not being shown in your admin panel. So restarting it again wont work due to the port being blocked. 

### Configuration
We now have a UI in torch. If you want to use the XML Version see below:

There is only one setting to change which is the time to wait before the force restart will happen.

The default config is 120 Seconds which can be set up in the RestartWatchdog.cfg in your Instances folder

Edit: &lt;DelayInSeconds&gt;120&lt;/DelayInSeconds&gt; to whatever seconds you need. 

### Github
[https://github.com/LordTylus/SE-Torch-Plugin-ALE-RestartWatchdog](https://github.com/LordTylus/SE-Torch-Plugin-ALE-RestartWatchdog)