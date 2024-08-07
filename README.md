# winexpext
A collection of Microsoft Windows Explorer file and folder context-menu extensions.

- Requires .NET 5 installed to build the binaries.
- Designed using Visual Studio Code.

| *branch* | Currently |
| :--- | :---: |
| master | [![Build status](https://github.com/rambotech/winexpext/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/rambotech/UpwinexpextdateDDNS) |
| release | [![Build status](https://github.com/rambotech/winexpext/actions/workflows/dotnet.yml/badge.svg?branch=develop)](https://github.com/rambotech/winexpext/tree/release) |
| develop | [![Build status](https://github.com/rambotech/winexpext/actions/workflows/dotnet.yml/badge.svg?branch=develop)](https://github.com/rambotech/winexpext/tree/develop) |

To install the extensions in Windows Explorer:

### Build the binaries needed

- Download the code, one of the two following ways.

    - Clone this repo to a local folder: ```git clone https://github.com/rambotech/winexpext.git```
    - Download the code as a zip from here, and extract it to a local folder: ```https://github.com/rambotech/winexpext/archive/master.zip```

- Change the current directory to the local folder used above.
- Execute this command line to put the binaries into a new folder.
    - ```dotnet publish -c release -o c:\apps\winexpext -r win10-x64```

### Add the extensions to Windows Explorer

- Open Windows Explorer and navigate to the local folder.
- Double-click the file add_winexpext.reg, and import it into the Windows Registry.

### Optional cleanup

If you don't plan to add any new functions or tweak the code for your needs:
- Remove the folder from the zip file or the ``` git clone ``` action,

### To use it

- Open windows explorer and select a file (not a folder), and right click on a file.  There will be new items in the context menu for the following:

    - **Timestamped Copy**: create a copy of a file, and suffixes the root name with a formated timestamp of the file's last modification timestamp.  This is useful for a snapshot in time of the file's content, also known as generational clones.  Multiple files can be selected.
    - **Timestamped Rename**: same as *Timestamped Copy* above, but does a rename instead.  Multiple files can also be selected.

### Revision History

2024-07-15 -- v1.2.0
- Add hashing for MD5, SHA1, SHA256, SHA384 and SHA512

2021-06-23 -- v1.1.0
- Better exception handling and expandability.
- Added Travis CI build stuff.

2021-10-14 -- v 1.0.0
- Original version

