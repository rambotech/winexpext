using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace winexpext   // Windows Explorer Extension
{
    class Program
    {
        enum FileAction : int
        {
            demonstrate,
            timestampCopy,
            timestampRename,
            removeCopyMark,
            removeRenameMark,
            removeTimestampedMark,
            hashMD5,
            hashSHA1,
            hashSHA256,
            hashSHA384,
            hashSHA512
        }

        enum TimestampingAction : int
        {
            Copy,
            Rename
        }

        static readonly FileAction[] FileActionTesting =
        [
            FileAction.timestampCopy,
            FileAction.timestampRename,
            FileAction.removeCopyMark,
            FileAction.removeRenameMark
        ];

        static readonly int NoDelay = -1;
        static readonly int UserMustClose = 0;
        static readonly bool LogToFile = true;
        static readonly bool LogToConsole = false;

        static bool TestingOnly = false;
        static string Filename = string.Empty;

        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 1)
                {
                    ShowHelp("No argument(s)");
                    Console.WriteLine("Press ENTER to close...");
                    Console.ReadLine();
                    System.Environment.Exit(2);
                }
                FileAction action;
                if (!Enum.TryParse<FileAction>(args[0], out action))
                {
                    ShowHelp($"Unrecognized command: {args[0]}");
                    Console.WriteLine("Press ENTER to close...");
                    Console.ReadLine();
                    System.Environment.Exit(2);
                }
                if (File.Exists(args[1]))
                {
                    Filename = args[1];
                }
                else if (args[1].Equals("/test", StringComparison.OrdinalIgnoreCase))
                {
                    TestingOnly = true;
                    Filename = args[2];
                    Console.WriteLine("... Testing mode ...");
                }
                if (!File.Exists(Filename))
                {
                    throw new Exception($"File not found: {Filename}");
                }
                Console.Write($"    processing: {args[1]}: ");
                switch (action)
                {
                    case FileAction.demonstrate:
                        DemostrateArgHandling(args);
                        Console.WriteLine("OK");
                        break;
                    case FileAction.timestampRename:
                        TimestampedCopyOrRename(TimestampingAction.Rename);
                        Console.WriteLine("OK");
                        break;
                    case FileAction.timestampCopy:
                        TimestampedCopyOrRename(TimestampingAction.Copy);
                        Console.WriteLine("OK");
                        break;
                    case FileAction.removeCopyMark:
                        RemoveCopyMark();
                        Console.WriteLine("OK");
                        break;
                    case FileAction.removeRenameMark:
                        RemoveRenameMark();
                        Console.WriteLine("OK");
                        break;
                    case FileAction.removeTimestampedMark:
                        RemoveTimestampedMark();
                        Console.WriteLine("OK");
                        break;
                    case FileAction.hashMD5:
                        CalculateHash("MD5");
                        Console.WriteLine("OK");
                        break;
                    case FileAction.hashSHA1:
                        CalculateHash("SHA1");
                        Console.WriteLine("OK");
                        break;
                    case FileAction.hashSHA256:
                        CalculateHash("SHA256");
                        Console.WriteLine("OK");
                        break;
                    case FileAction.hashSHA384:
                        CalculateHash("SHA384");
                        Console.WriteLine("OK");
                        break;
                    case FileAction.hashSHA512:
                        CalculateHash("SHA512");
                        Console.WriteLine("OK");
                        break;
                    default:
                        throw new Exception($"No method for defined action \"{action}\"");
                }
                System.Environment.Exit(0);
            }
            catch (ArgumentException err)
            {
                ConsoleAndLogged($"{err.Message}", "(Main)");
                System.Environment.Exit(2);
            }
            catch (Exception err)
            {
                ConsoleAndLogged($"{err.Message}", "(Main)", 0, true);
                System.Environment.Exit(3);
            }
        }

        static void ShowHelp(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine(message);
            }

            Console.WriteLine();
            Console.WriteLine("winexpext command filename");
            Console.WriteLine();
            Console.WriteLine("winexpext demonstrate sourceFile");
            Console.WriteLine("winexpext removeCopyMarks sourceFile");
            Console.WriteLine("winexpext removeRenameMarks sourceFile");
            Console.WriteLine("winexpext timestampedCopy sourceFile");
            Console.WriteLine("winexpext timestampedRename sourceFile");
            Console.WriteLine("winexpext hashMD5 sourceFile");
            Console.WriteLine("winexpext hashSHA1 sourceFile");
            Console.WriteLine("winexpext hashSHA256 sourceFile");
            Console.WriteLine("winexpext hashSHA384 sourceFile");
            Console.WriteLine("winexpext hashSHA512 sourceFile");
            Console.WriteLine();
            Console.WriteLine("Use:");
            Console.WriteLine("winexpext command /test filename");
            Console.WriteLine("To only display the results, with no change");
            Console.WriteLine();
        }

        static void DemostrateArgHandling(string[] args)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Demonstrating argument handling:");
            sb.AppendLine();

            for (int i = 0; i < args.Length; i++)
            {
                sb.AppendLine($"%{i} = {args[i]}");
            }
            sb.AppendLine();
            ConsoleAndLogged(sb.ToString(), "DemostrateArgHandling", 10, false);
        }

        /// <summary>
        /// Removes " - Copy." ... appendage on the root file name, which does not cause a naming conflict.
        /// These appendages are added by Windows when copying a file in the same directory, and are not added when renaming a file 
        /// to an existing name. So this method is for removing copy appendages, but not rename appendages.
        /// E.g. "filename - Copy.txt" --> "filename.txt" if "file.txt" does not exist, but does nothing if "file.txt" still exists.
        /// </summary>
        /// <param name="filename"></param>
        static void RemoveCopyMark()
        {
            var newFilename = lib.Helpers.Methods.RemoveCopyMark(Filename);   
            // accommodates a file with no extension after its name.
            if (File.Exists(newFilename))
            {
                throw new ArgumentException($"Cannot remove copy mark because the original file still exists: {newFilename}");
            }

            if (TestingOnly)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Original: {Filename}");
                sb.AppendLine($"New:      {newFilename}");
                ConsoleAndLogged(sb.ToString(), "RemoveCopyMark");
                return;
            }
            try
            {
                File.Move(
                    Filename,
                    newFilename
                );
            }
            catch (Exception err)
            {
                var message = $"Error removing copy mark:\r\nFrom: .. \"{Filename}\"\r\nTo: .... \"{newFilename}\"";
                throw new Exception(message, err);
            }
        }

        /// <summary>
        /// Removes "(#) ... appendage on the root file name, which does not cause a naming conflict.
        /// E.g. "file(2).txt" --> "file.txt" if "file.txt" does not exist, but does nothing if "file.txt" still exists.
        /// This appendage is added by Windows when copying or renaming a file in the same directory to an 
        /// existing name, and is not added when copying a file.
        /// </summary>
        /// <param name="filename"></param>
        static void RemoveRenameMark()
        {
            var r = new Regex(@"\s\([\d]+\)\.", RegexOptions.None);

            var origRootName = Path.GetFileNameWithoutExtension(Filename);
            var testRootName = origRootName + ".";
            if (!r.IsMatch(testRootName))
            {
                ConsoleAndLogged("Nothing to do: the file's root name does not end with \" (#..#)\"", "RemoveRenameMark", 10, true);
                return;
            }
            // accommodates a file with no extension after its name.
            var newFilename = Path.Combine(
                    Path.GetDirectoryName(Filename),
                    testRootName.Substring(0, r.Match(testRootName).Length) + Path.GetExtension(Filename)
            );

            if (File.Exists(newFilename))
            {
                ConsoleAndLogged($"Cannot remove reanme mark because the original file still exists", "RemoveRenameMark", 10, true);
                return;
            }

            if (TestingOnly)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Original: {Filename}");
                sb.AppendLine($"New:      {newFilename}");
                ConsoleAndLogged(sb.ToString(), "RemoveRanameMark");
            }
            else
            {
                try
                {
                    File.Move(
                        Filename,
                        newFilename
                    );
                }
                catch (Exception err)
                {
                    var message = $"Error removing rename mark:\r\nFrom: .. \"{Filename}\"\r\nTo: .... \"{newFilename}\"";
                    throw new Exception(message, err);
                }
            }
        }

        /// <summary>
        /// Removes "(#) ... appendage on the root file name, which does not cause a naming conflict.
        /// E.g. "file(2).txt" --> "file.txt" if "file.txt" does not exist, but does nothing if "file.txt" still exists.
        /// This appendage is added by Windows when copying or renaming a file in the same directory to an 
        /// existing name, and is not added when copying a file.
        /// </summary>
        /// <param name="filename"></param>
        static void RemoveTimestampedMark()
        {
            var r = new Regex(@"\-[\d]{8}\-[\d]{6}\.", RegexOptions.None);

            var origRootName = Path.GetFileNameWithoutExtension(Filename);
            var testRootName = origRootName + ".";
            if (!r.IsMatch(testRootName))
            {
                ConsoleAndLogged("Nothing to do: the file's root name does not end with \"--yyyyMMdd--HHmmss\"", "RemoveTimestampMark", 10, true);
                return;
            }
            // accommodates a file with no extension after its name.
            var newFilename = Path.Combine(
                    Path.GetDirectoryName(Filename),
                    testRootName.Substring(0, r.Match(testRootName).Length) + Path.GetExtension(Filename)
            );

            if (File.Exists(newFilename))
            {
                ConsoleAndLogged($"Cannot remove timestamp mark because the original file still exists", "RemoveTimestampedMark", 10, true);
                return;
            }

            if (!TestingOnly)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Original: {Filename}");
                sb.AppendLine($"New:      {newFilename}");
                ConsoleAndLogged(sb.ToString(), "RemoveTimestampedMark");
            }
            else
            {
                try
                {
                    File.Move(
                        Filename,
                        newFilename
                    );
                }
                catch (Exception err)
                {
                    var message = $"Error removing timestamp mark:\r\nFrom: .. \"{Filename}\"\r\nTo: .... \"{newFilename}\"";
                    throw new Exception(message, err);
                }
            }
        }

        static void TimestampedCopyOrRename(TimestampingAction action)
        {
            var datetimePortion = File.GetLastWriteTime(Filename).ToString("-yyyyMMdd-HHmmss");
            var newFileName = Path.Combine(
                Path.GetDirectoryName(Filename),
                Path.GetFileNameWithoutExtension(Filename) + datetimePortion + Path.GetExtension(Filename)
            );
            Console.WriteLine($"{Filename} --> {newFileName}");
            if (TestingOnly)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Original: {Filename}");
                sb.AppendLine($"New:      {newFileName}");
                ConsoleAndLogged(sb.ToString(), "RemoveTimestampedMark");
                return;
            }
            try
            {
                if (action == TimestampingAction.Copy)
                {
                    File.Copy(Filename, newFileName);
                }
                else
                {
                    File.Move(Filename, newFileName);
                }
            }
            catch (Exception err)
            {
                ConsoleAndLogged($"Failed to {action} file: {err.Message}", "TimestampedCopyOrRename", true);
            }
        }

        static void CalculateHash(string hashMethod)
        {
            var resultFile = Path.Combine(
                Path.GetTempPath(),
                Filename + $".{hashMethod.ToLower()}.txt");
            using (var sw = new StreamWriter(resultFile, false))
            {
                sw.WriteLine($"{hashMethod.ToUpper()} for {Filename}...");
                sw.WriteLine();
                using (var stream = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan))
                {
                    var hashed = hashMethod.ToUpperInvariant() switch
                    {
                        "MD5" => MD5.Create().ComputeHash(stream),
                        "SHA1" => SHA1.Create().ComputeHash(stream),
                        "SHA256" => SHA256.Create().ComputeHash(stream),
                        "SHA384" => SHA384.Create().ComputeHash(stream),
                        "SHA512" => SHA512.Create().ComputeHash(stream),
                        _ => throw new Exception($"Unrecognized hash method: {hashMethod}")
                    };
                    var value = BitConverter.ToString(hashed).ToLowerInvariant().Replace("-", string.Empty);

                    sw.WriteLine($"{hashMethod.ToUpper()} for {Filename}...");
                    sw.WriteLine();
                    sw.WriteLine(value);
                    sw.WriteLine();
                    var s = DateTime.Now.ToString("f");
                    sw.WriteLine($"Generated On: {s}...");
                }
            }
            var p = new ProcessStartInfo
            {
                ErrorDialog = true,
                FileName = resultFile,
                UseShellExecute = true
            };
            Process.Start(p);
        }
        static void ConsoleAndLogged(string message, string functionName)
        {
            ConsoleAndLogged(message, functionName, UserMustClose, LogToConsole);
        }

        static void ConsoleAndLogged(string message, string functionName, int secondsToDisplay)
        {
            ConsoleAndLogged(message, functionName, secondsToDisplay, false);
        }

        static void ConsoleAndLogged(string message, string functionName, bool logMessage)
        {
            ConsoleAndLogged(message, functionName, UserMustClose, logMessage);
        }

        static void ConsoleAndLogged(string message, string functionName, int secondsToDisplay, bool logMessage)
        {
            if (logMessage)
            {
                var errorFile = Path.Combine(Path.GetTempPath(), $"winexpext_{DateTime.Now:yyyy-MM-dd}_log.txt");
                using (var sw = new StreamWriter(errorFile, true))
                {
                    sw.WriteLine(new string('-', 50));
                    sw.WriteLine($"Generated On:     ... {DateTime.Now:f}");
                    sw.WriteLine($"Function:         ... {functionName}");
                    sw.WriteLine($"File name:        ... {Filename}");
                    if (TestingOnly)
                    {
                        sw.WriteLine($"TESTING ONLY:     ... {TestingOnly}");
                    }
                    sw.WriteLine();
                    sw.WriteLine(message);
                    sw.WriteLine();
                }
            }

            if (secondsToDisplay < 0)
            {
                return;
            }

            Console.WriteLine(message);

            var userMustClose = secondsToDisplay <= 0.0d;
            if (!userMustClose)  // timesout and return after the expired wait time.
            {
                var timeoutSeconds = (int)(secondsToDisplay < 2.0d ? 5.0d : secondsToDisplay);
                var secondsElapsed = -1;
                var sw = new Stopwatch();
                sw.Start();
                while (!userMustClose && sw.Elapsed.Seconds < timeoutSeconds)
                {
                    if (secondsElapsed != (int)sw.Elapsed.TotalSeconds)
                    {
                        secondsElapsed = (int)sw.Elapsed.TotalSeconds;
                        var Elapsed = $"  {timeoutSeconds - secondsElapsed}";
                        Console.WriteLine($"Press ENTER to close or X to keep open ... {Elapsed.Substring(Elapsed.Length - 3, 3)}");
                        Console.CursorTop--;
                    }
                    if (Console.KeyAvailable)
                    {
                        switch (Console.ReadKey(true).KeyChar)
                        {
                            case 'x':
                            case 'X':
                                userMustClose = true;
                                break;
                            case '\r':
                                timeoutSeconds = 0;
                                break;
                            default:
                                break;
                        }
                    }
                }
                System.Threading.Thread.Sleep(100);
            }
            if (userMustClose)
            {
                Console.WriteLine($"Press ENTER to close...                                 ");
                Console.ReadLine();
            }
        }
    }
}