# Running Cyberbox on windows:

To run the convertor on windows, extract the .7z file to a directory, open said directory, double click the exe. It dont get any simpler than that.

# FAQ's

Q: "I'm on linux. Will there be a native linux port?"

A: Yesn't

Long Answer: I do plan to make a native linux version, but it'll depend on there being,

A: A free open source alternative to visual studio

B: How linux can handle some of the asssembly headers that are typically found in C# scripts

C: The type of compiler that I'll need if I have to rewrite everything so I can pack it either into a .AppImage file or a .sh file

D: Figure out how much more powerful the linux command line really is (If you can compile an entire program on the linux cli just by doing makepkg -si then I should be set)

E: Find out if I'll need FFMPEG, and if so learn how to use FFMPEG

So for now You'll have to use wine, specifically lutris for a graphical front-end to make setup easier which I will go over in the next section

# Setting up Cyberbox with lutris:

So you decided you don't/no longer wanna use windows. Congratulations on breaking free from the malware riddled shackles of microsoft and moving to the penguin valhalla.

To setup Cyberbox on linux with lutris first you will need to install wine and lutris, If you already have them installed you can move to the next section.

To install wine, open a terminal with either ctrl+alt+T, and type,

Arch/Arch Based:

`sudo pacman -S wine winetricks lutris`

Debian/Debian Based:

`sudo apt install wine winetrick lutris`

Fedore/Fedora Based:

`sudo DNF install wine winetricks lutris`

Note: If when trying to run these commands and you get and error saying
`sudo: dnf: command not found` then your distro is not based on any of these and you'll have to find out what your distros package manager is and accomodate the command accordingly or you used the wrong package manager command accidentally

Once installed go ahead and open lutris by typing lutris in the termianl or opening the start menu (even though in linux it has a different name but I resorted to calling it that as I don't remember what the actual name is) and going to Games > Lutris

Once lutris is opened, click the plus in the top left of the window
Click add locally installed game.

In the name field, type "Cyberbox"

In the runner dropdown select wine

Now click the game options tab next to that
and next to the executable field click the button with the three dots, and that will open a window where you can select the exe

If this is your first time using wine and lutris, you will not have any wine prefixes by default

So in the wine prefix field, you will want to point it to a directory that you would want the prefix to be

So for this example the prefix will be

`/home/$USER/Documents/Cyberbox`

and for the prefix architecture dropwdown, select 64-bit

Now in the runner options tab, you will see a dropdown for the wine executable

In the dropdown you should see an option called System (9.11) or whatever the version number would be for you.
If you have steam installed, then you will have other options like proton-xxx, or proton-ge-xxx.

If you got to this tab and by default it's set for proton-ge-xxx,
then you shouldnt need to do anything else

If you have an nvidia gpu you can leave everything default.
If you have an amd or intel gpu, you should be fine leaving everything default, but if you do experience any issues trying to get cyberbox working
this tab (in my experience) will be where you'd wanna play around with it until Cyberbox starts working

Now in the System options tab, again you should be fine leaving everything default, but if you do experience issues then this will be the second tab to play around with
until Cyberbox starts working

With that being said and done, you should now have a game item in lutris labled "Cyberbox."
All you need to do from here on is double click the game item, or click play at the bottom, and Cyberbox should open.

If not, then right click, and click Configure, and run through the Runner and System options tabs over again until Cyberbox starts working
