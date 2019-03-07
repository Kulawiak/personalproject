# Peronal Project
This repository contains the source code for the server side of my personal project, which sends array packets to the controller, or client side of my project. The program captures the audio before it reaches the output device and process it to display a pattern on the strip.

## Prerequisites
- If you plan to run this program from the provided build you will only need to make sure that the host device has: 
- Some type of internet connectivity 
- Is on the same network as the controller. (To set up the controller, take a look at “Controller Setup,” which can be found below.) 
Optionally, if you’d like to be able to access the control panel from any other device beside the host itself, make sure that the HTTP  port 80 is open and enabled in your firewall settings. 

Otherwise, to build from source, you also need Visual Studio, along with .NET Core installed with the IDE.

### Additional Notes
- Administrator privilege is required on the host machine.
- This program is designed and tested to work only on Windows. Despite using NET Core, which implements cross platform capability, the audio capturing library, CSCore, only supports the Windows environment.
- The build already contains the embedded and standalone .NET Core runtime, so installation is no necessary.

## Prerequisites
Running the Program
In the builds folder, you should be able to locate the zip file containing the program. Extract it and open the folder.
For your convince, a simple LFX.bat file has been added before the main directory, which simply launches LFX.exe in the library folder.
From here on, most of the setup is done by the program itself, and there are only three possible times when you’ll need to provide some type of input or perform an action:
1. At the initial start, if you have more than one active network adapters, the console will list all the IP addresses of the connections, prompting you to pick which interface to connect to.
2. An admin privilege message. You shouldn’t be seeing this error as the executable file is set to ask for admin level execution, but in the rare case you do, go into the lib folder and find the .exe file. Right click it, select properties, and make sure the option “run as Administrator” is checked. You can than proceed to run the program again.
3. A “port 80 is in use” error. If you get this error, make sure there aren’t any other instances of the program running. If this does not fix the problem, you’ll have to make sure that no other process, perhaps another server, is running on your machine, then try again.

### Additional Notes
- If incorrect input is provided when choosing from the list of two or more network adapters, the console will display an error, but still proceed, choosing the first possible default adapter.

Dominik Kulawiak | Personal Project
