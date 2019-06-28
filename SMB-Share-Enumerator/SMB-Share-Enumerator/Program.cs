using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SMB_Share_Enumerator
{
    class Program
    {
        static IEnumerable<string> GetFiles(string path)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        yield return files[i];
                    }
                }
            }
        }

        static string ChooseCommand()
        {
            Console.WriteLine();
            Console.WriteLine("=============================================");
            Console.WriteLine();

            Console.WriteLine(" 1 - Recursively list all files on share to file");
            Console.WriteLine(" 2 - Recursively list all a particular folder to file");
            Console.WriteLine(" 3 - Copy a file to or from the share");
            Console.WriteLine(" 4 - Exit - USE THIS DO NOT JUST CLOSE");
            Console.Write("Choose Option: ");
            string option = Console.ReadLine();


            Console.WriteLine();
            Console.WriteLine("=============================================");
            Console.WriteLine();

            return option;

        }

        static void WriteFiles(string filePath, string fileName)
        {
            fileName = string.Concat(fileName, ".txt");

            //foreach filename enumerated write to the given filename.
            using (System.IO.StreamWriter file =
    new System.IO.StreamWriter(fileName))
            {
                foreach (string line in GetFiles(filePath))
                {
                    file.WriteLine(line);
                }

                //confirm succefull file write
                Console.WriteLine("Output saved to " + fileName + ".txt");
            }
        }

        static void copyFile(string copyFrom, string copyTo)
        {
            //try and copy from one location to another. This is a blind copy, need to add permission checks first.
            try
            {
                File.Copy(copyFrom, copyTo);
                Console.WriteLine("File successfully copied");
            }
            catch
            {
                Console.WriteLine("Unable to copy file, check you have the required privilges and the file exists");
            }
        }


        //---------------------------------------------------------------------


        static void Main(string[] args)
        {
            // setup variables
            string share;
            string user_name;
            string password;
            string folderPath;
            string folder;
            string copyTo;
            string copyFrom;
            string option = "0";

            //header
            Console.WriteLine();
            Console.WriteLine("=============================================");
            Console.WriteLine("             Drive Mapper Tool");
            Console.WriteLine("Ralph Vickery");
            Console.WriteLine("June 2019 - v0.1");
            Console.WriteLine("=============================================");
            Console.WriteLine();

            if (args.Length == 0)
            {
                //get connection info
                Console.WriteLine("Share to connect to eg: \\\\192.168.1.1\\test");
                share = @Console.ReadLine();
                Console.WriteLine("Username eg: Domain\\User");
                user_name = @Console.ReadLine();
                Console.WriteLine("Account Password");
                password = Console.ReadLine();
            }
            else
            {
                share = @args[0];
                user_name = @args[1];
                password = args[2];
            }
            //create a network driveShare object
            NetworkShareAccesser driveShare;

            try
            {
                // try and connect, if not tool will error and exit.
                Console.WriteLine("Attempting to connect to the share");
                Console.WriteLine(share);
                driveShare = NetworkShareAccesser.Access(share, user_name, password);
                Console.WriteLine("Share connected");

                if (args.Length == 0)
                {
                    //Load the options menu and run the appropriate option.
                    while (option != "quit")
                    {

                        option = ChooseCommand();
                        switch (option)
                        {
                            case "1":
                                Console.WriteLine("List all files to localhost");
                                WriteFiles(share, "All_Files");
                                break;
                            case "2":
                                Console.WriteLine("List folder contents to localhost");
                                Console.WriteLine("Folder to list eg: Secure Documents");
                                folder = @Console.ReadLine();
                                folderPath = string.Concat(share, "\\", folder);
                                WriteFiles(folderPath, folder);
                                break;
                            case "3":
                                Console.WriteLine("Copy file to or from the share.");
                                Console.WriteLine("Full path of file to copy eg: \\\\192.168.1.1\\Test\\My Documents\\secure.txt");
                                copyFrom = @Console.ReadLine();
                                Console.WriteLine("Full path of new file: C:\\tmp\\secure.txt");
                                copyTo = @Console.ReadLine();
                                copyFile(copyFrom, copyTo);
                                break;
                            case "4":
                                driveShare.Dispose();
                                Console.WriteLine("Share Cleaned Up");
                                Console.WriteLine("Exiting DriveMapper");
                                option = "quit";
                                Environment.Exit(0);
                                break;
                            default:
                                Console.WriteLine("Option not recognised");
                                option = "0";
                                break;
                        }
                    }
                }
                else
                {
                    if (args.Length == 5)
                    {
                        copyFrom = @args[3];
                        copyTo = @args[4];
                        copyFile(copyFrom, copyTo);

                    }
                    else
                    {
                        Console.WriteLine("List all files to localhost");
                        WriteFiles(share, "All_Files");
                    }

                    driveShare.Dispose();
                    Console.WriteLine("Share Cleaned Up");
                    Console.WriteLine("Exiting DriveMapper");
                    option = "quit";

                }

            }
            catch
            {
                //catch error and wait for input.
                Console.WriteLine("Unable to connect to the network driveShare, clearing up.");
                Console.ReadLine();
            }
        }
    }
}
