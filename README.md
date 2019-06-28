# SMB-Share-Enumerator
C# Console application for enumerating and copying files to and from a SMB share in restricted environments where the use of NET USE, Explorer and Powerwhell are prevented.

The application will run in two modes CLI or Console App.

If running as an application a terminal window will open and prompt you for the path of the network share, usernam and password. eg: 

```
=============================================
             Drive Mapper Tool
Ralph Vickery
June 2019 - v0.1
=============================================

Share to connect to eg: \\192.168.1.1\test
\\192.168.1.1\test
Username eg: Domain\User
admin
Account Password
MySecurePassword
```

If the connection to the share was successfull then the application then will provide you with 4 options.

```
Attempting to connect to the share
\\192.168.20.20\Movies
Share connected

=============================================

 1 - Recursively list all files on share to file
 2 - Recursively list all a particular folder to file
 3 - Copy a file to or from the share
 4 - Exit - USE THIS DO NOT JUST CLOSE
Choose Option:
```
Option 1: will recursively list all files on the share with the full path and store them in a text file called All_Files.txt in your current working diretory.

Option 2: allows you to specify a particular folder on the share "Documents", you do not need to encapsulate the file path.

Option 3: allows you to copy a file to or from the share. Ensure that your use the full file path for it to be successful.
  eg: C:\tmp\copyFrom.exe \\192.168.1.1\test\My Documents\copyTo.exe

Option 4: IMPORTANT USE THIS TO EXIT DO NOT CLOSE THE APPLICATION OTHER WISE THE CONNECTION WILL DO PROPERLY CLOSE.


It is possible to run the application in a restricted terminal using command line arguements though functionality is somewhat restricted.

To recursively list files on a share:
`SMB-Share-Enumerator.exe \\192.168.1.1\Share Username Password`
If you just need to enumerate a particular folder then add it to the share path.

To copy a file add two more arguements at the end for the file information (ensure you use a full path).
`SMB-Share-Enumerator.exe \\192.168.1.1\Share Username Password FileToCopy NewFileLocation`

The portable executable available above aswell as the source code if you need to edit it.

Enjoy!
