# Welcome to the P2P Launcher

[![BuyMeCoffee][buymecoffeebadge]][buymecoffee]

## What is the P2P Launcher?
P2PLauncher is a program designed to help connect multiple peers to a simulated LAN environment (virtual LAN) using FreeLAN - an open source, peer-to-peer VPN.
By sporting an easy-to-use interface to make the set-up less jarring, P2PLauncher is here to be your preferred method for LAN connectivity.

It is intended for use mainly by gamers, but can be used for other purposes. The program aims to allow gamers to play old games whose servers are no longer supported by their developers or have networking problems with their existing systems, but have the option for LAN connectivity (e.g Call of Duty, Battlefield, Quake, Splinter Cell, Ghost Recon, SWAT4, etc).

It is being developed by Noah “Ndo360” Cline as its project manager, and Kacper “K4CZP3R” as its lead programmer.

## Why use this when there are others?
Yeah, you’re right, there are other popular alternatives. To name a few off the top of our heads - Radmin VPN, ZeroTier, Softether, Hamachi, GameRanger. Previously we had Tunngle and Evolve as well. With all of those mentioned, why would you use P2PLauncher? Hell, FreeLAN doesn’t even have its own GUI.

**Well, here’s why:**
* P2PLauncher is not at risk of being shut down. All connections are direct peer-to-peer and entirely self-hosted by the users. Hamachi, Zerotier, Radmin and many more rely on their servers to keep running.
* While Hamachi and Radmin limit the number of players per personal session, P2PLauncher does not.
* The backbone of P2PLauncher, FreeLAN, has very minimal overhead, resulting in negligible ping from the program itself and lower overall ping.
* Works with potentially any game that supports LAN matchmaking, unlike GameRanger’s custom hooking/hole-punching which leads to its limited game list.
* Open source.
* Easy to use interface.

So, why don’t you give P2PLauncher a try?

## How to get started.
1. Download the latest [release](https://github.com/K4CZP3R/P2PLauncher/releases), and extract it to a location of your choosing. Run P2PLauncher.exe as administrator. Windows Defender might raise a warning of running a program from an unknown publisher. P2PLauncher is not harmful. Promise. You can study the code yourself. Click on More Info and then Run Anyway to launch the program.
2. Now at the main launcher window, you’ll be able to see 4 tabs that take the left side of the interface, and a region dubbed Information on the right side of the interface.

## Initial Configuration
This configuration is a one-time process. Subsequent uses of the program will not require interfering with the Information region.
* Click on FreeLAN Status’ configure button. The button Find FreeLAN will detect the presence of FreeLAN on your system. FreeLAN, by default, installs in [OS Drive]\Program Files\FreeLAN. If it finds it there, you’ll be informed that everything’s good to go, and may proceed to the **ADAPTERS** section.
* If you do not have FreeLAN installed, use the Download tab. Inside it is a button that will link you to the FreeLAN v2.2. The program will also tell you if you need the 64, or 32 bit version. Download and install the version the program tells you to your desired location, making sure all of the checkboxes are marked in the “Select Component” screen. 
* Whether or not you want FreeLAN in a start menu folder is up to you. Make sure all of the checkboxes in the “Select Additional Tasks” screen are marked and proceed to install.

## Finding FreeLAN
Once done, we’ll need to tell P2PLauncher where to locate FreeLAN. Had you installed in the default location, all you need to do is click the Find FreeLAN button once again. Had you not installed it in the default location, you’ll have to locate it manually through the “Locate it” tab.
* Either way, once FreeLAN has been located, you’ll be informed of it, and may close FreeLAN Status’ configuration window. One last time, ensure the status is “FreeLAN is located and valid!” in the main window, thus finishing this step.

## Adapters
If you played any games that required virtual LAN before, you might still have an adapter that belongs to one of the many alternatives mentioned previously. In order to make sure those adapters don’t interfere with P2PLauncher, we’ll mark them for the program to disable. Right by the “Adapters to disable” text you’ll find a Configure button that leads to a window listing every network adapter installed on your system. If you don’t have a VPN adapter on your system, you may skip this section.
* If you do, highlight the relevant adapter, then click on the “>” button, moving it to the “Disabled” list

## Services
Much like the previous section, click on the Configure button that’s next to the “Services to disable” text. Find the services whose adapters you put in the “Disabled” list in the last step, and do the same. For your convenience, there’s a search bar, as well as a checkbox that filters the common alternatives.

## Hosting
To host a personal session, navigate to - you guessed it - the Host tab.
Input the password that you’d like to use for your personal session, then click on Start. Simple!
In order for your friends to connect to you, you will need to hand out the public address shown in the Information region.
* Note: If no one can connect to you make sure to port forwarding 12000 UDP.

## Client
To connect to a personal session, navigate to the Client tab.
* Fill in the public IP address given to you by the host.
* Fill in a unique number, between 2 - 240, in the ID field (It is advised to ask the host what your ID is). __No one in your party can use the same ID.__
* Lastly, input the password given to you by the host.
Once done, click on Start and the connection will be made. Simple!

## Important Notes
If you’re interested in the logs or the nerd code executed by the command prompt, you can do so at the bottom right corner of the program.

Ensure the session is running by looking at “State”. It’s on the bottom left corner of the Information region. (You know it is working once you see a FreeLAN address appear.

If you can’t host a session or enter one as a client, ensure the TAP adapter is enabled in your Network & Sharing Center.

The “Public address” is your public IP, be careful not to freely hand it out to everyone!

As a client, ensure your connectivity to the host by opening command prompt (type in “cmd” in the start menu) and typing in “ping 9.0.0.1”. If you get a response, then everything checks out. While this may not work, as windows typically blocks ping requests, it's a common test that can be preformed. (Other than loading a game)

Should go without saying, opt out of the connection by clicking Stop or closing the program.

## Hubs
Hubs are community networks that allow clients to join and play with those also connected to the same host. A seperate guide will be made on creating and running your own HUB soon.

# Credits
> Project Lead: Noah C. "Ndo360" Cline
> Lead Programmer: K4CZP3R
> Guide Writen By: "Gossqe"

---
[buymecoffee]: https://www.buymeacoffee.com/k4czp3r
[buymecoffeebadge]: https://www.buymeacoffee.com/assets/img/custom_images/yellow_img.png

