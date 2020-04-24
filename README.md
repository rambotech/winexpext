# winexpext
A collection of Microsoft Windows Explorer file and folder context-menu extensions.

- Requires .NET Core 3.0 installed to build the binaries.
- Designed using Visual Studio Code.

To install the extensions in Windows Explorer:

### Build the binaries needed

- Download the code, one of the two following ways.

    - Clone this repo to a local folder: ```git clone https://github.com/rambotech/winexpext.git```
    - Download the code as a zip from here, and extract it to a local folder: ```https://github.com/rambotech/winexpext/archive/master.zip```

- Change the current directory to the folder with the code

- Execute: ```dotnet publish -c release -o c:\apps\winextexp -r win10-x64```

### Add the extensions to Windows Explorer

- Open Windows Explorer and navigate the folder with the code
- Double-click the file add_winexpext.reg, and import it into the Windows Registry.

### To use it

- Open windows explorer and select a file (not a folder), and right click on a file.  There will be new items in the context menu for the following:

    - **Timestamped Copy**: create a copy of a file, and suffixes the root name with a formated timestamp.  This is useful for a snapshot in time of the file's contents, also known as generational clones.
    - **Timestamped Rename**: same as *Timestamped Copy* above, but does a rename instead.





