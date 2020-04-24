using System;
using System.IO;

namespace winexpext   // Windows Explorer Extension
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1) ShowHelp("Usage");
                if (args[0] == "timestampRename")
                {
                    if (args.Length != 2) ShowHelp("timestampRename requires a filename");
                    if (! File.Exists(args[1])) ShowHelp("timestampRename requires an existing file");
                    TimestampedCopyOrRename(args[0], false);
                    System.Environment.Exit(1);
                }
                if (args[0] == "timestampCopy")
                {
                    if (args.Length != 2) ShowHelp("timestampCopy requires a filename");
                    if (! File.Exists(args[1])) ShowHelp("timestampCopy requires an existing file");
                    TimestampedCopyOrRename(args[0], true);
                    System.Environment.Exit(1);
                }
                ShowHelp($"Unknown function: {args[0]}");
                System.Environment.Exit(2);
            }
            catch (Exception err)
            {
                Console.WriteLine (err.Message);
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
            Console.WriteLine("timestampedCopy sourceFile");
            System.Environment.Exit(2);
        }

        static void TimestampedCopyOrRename(string filename, bool isCopy)
        {
            var datetimePortion = File.GetLastWriteTime(filename).ToString("-yyyyMMdd-HHnnss");
            var newFileName = Path.Combine(
                Path.GetDirectoryName(filename), 
                Path.GetFileNameWithoutExtension(filename),
                datetimePortion,
                Path.GetExtension(filename)
            );
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
