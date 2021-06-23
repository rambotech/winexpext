using System;
using System.IO;

namespace winexpext   // Windows Explorer Extension
{
    class Program
    {
        enum FileAction : int
        {
            timestampCopy,
            timestampRename
        }

        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2) ShowHelp("Usage");
                FileAction action;
                if (! Enum.TryParse<FileAction>(args[0], out action))
                {
                     ShowHelp($"Unrecognized command: {args[0]}");
                }
                Console.WriteLine ($"");
                var pauseForOperatorMessage = false;
                for (var index = 1; index < args.Length; index++)
                {
                    if (! File.Exists(args[index]))
                    {
                        Console.WriteLine($"file not found: {args[index]}");
                        pauseForOperatorMessage = true;
                        continue;
                    }
                    Console.Write($"    processing: {args[index]}: ");
                    try
                    {
                        switch (action)
                        {
                            case FileAction.timestampRename:
                                TimestampedCopyOrRename(args[index], false);
                                Console.WriteLine("OK");
                                break;
                            case FileAction.timestampCopy:
                                TimestampedCopyOrRename(args[index], true);
                                Console.WriteLine("OK");
                                break;
                            default:
                                Console.WriteLine($"No method for defined action \"{action}\"");
                                break;
                        }
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine($"Action failed.  \"{err.Message}\"");
                        pauseForOperatorMessage = true;
                    }
                }
                if (pauseForOperatorMessage)
                {
                    Console.WriteLine($"Completed, with one or more problems.. press any key to close.");
                    Console.ReadKey();
                }
                System.Environment.Exit(pauseForOperatorMessage ? 1 : 0);
            }
            catch (Exception err)
            {
                // Wait for user key press to clear any error
                Console.WriteLine (err.Message);
                Console.WriteLine ("Press any key to clear...");
                Console.ReadKey();
                System.Environment.Exit(3);
            }
        }

        static void ShowHelp(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine(message);
                Console.WriteLine();
            }
            Console.WriteLine("winexpext command [options]");
            Console.WriteLine();
            Console.WriteLine("winexpext timestampedCopy sourceFile [sourceFile2 [...]]");
            Console.WriteLine("winexpext timestampedRename sourceFile [sourceFile2 [...]]");
            System.Environment.Exit(2);
        }

        static void TimestampedCopyOrRename(string filename, bool isCopy)
        {
            var datetimePortion = File.GetLastWriteTime(filename).ToString("-yyyyMMdd-HHmmss");
            var newFileName = Path.Combine(
                Path.GetDirectoryName(filename), 
                Path.GetFileNameWithoutExtension(filename) + datetimePortion + Path.GetExtension(filename)
            );
            Console.WriteLine($"{filename} --> {newFileName}");
            if (isCopy)
            {
                File.Copy(filename, newFileName);
            }
            else
            {
                File.Move(filename, newFileName);
            }
        }
    }
}
