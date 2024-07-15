using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace winexpext   // Windows Explorer Extension
{
    class Program
    {
        enum FileAction : int
        {
            timestampCopy,
            timestampRename,
            hashMD5,
            hashSHA1,
            hashSHA256,
            hashSHA384,
            hashSHA512
        }

        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2) ShowHelp("Usage");
                FileAction action;
                if (!Enum.TryParse<FileAction>(args[0], out action))
                {
                    ShowHelp($"Unrecognized command: {args[0]}");
                }
                Console.WriteLine($"");
                var pauseForOperatorMessage = false;
                for (var index = 1; index < args.Length; index++)
                {
                    if (!File.Exists(args[index]))
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
                            case FileAction.hashMD5:
                                CalculateMD5(args[index]);
                                Console.WriteLine("OK");
                                break;
                            case FileAction.hashSHA1:
                                CalculateSHA1(args[index]);
                                Console.WriteLine("OK");
                                break;
                            case FileAction.hashSHA256:
                                CalculateSHA256(args[index]);
                                Console.WriteLine("OK");
                                break;
                            case FileAction.hashSHA384:
                                CalculateSHA384(args[index]);
                                Console.WriteLine("OK");
                                break;
                            case FileAction.hashSHA512:
                                CalculateSHA512(args[index]);
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
                Console.WriteLine(err.Message);
                Console.WriteLine("Press any key to clear...");
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

        static void CalculateMD5(string filename)
        {
            var resultFile = Path.Combine(
                Path.GetTempPath(),
                filename + ".md5.txt");
            using (var sw = new StreamWriter(resultFile, false))
            {
                sw.WriteLine($"MD5 for {filename}...");
                sw.WriteLine();
                using (var stream = new FileStream(
                    filename,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    4096,
                    FileOptions.SequentialScan))
                {
                    sw.WriteLine(BitConverter
                        .ToString(MD5.Create().ComputeHash(stream))
                        .ToLowerInvariant().Replace("-", string.Empty));
                }
                sw.WriteLine();
                var s = DateTime.Now.ToString("f");
                sw.WriteLine($"Generated On: {s}...");
            }
            var p = new ProcessStartInfo
            {
                ErrorDialog = true,
                FileName = resultFile,
                UseShellExecute = true
            };
            Process.Start(p);
        }

        static void CalculateSHA1(string filename)
        {
            var resultFile = Path.Combine(
                Path.GetTempPath(),
                filename + ".sha1.txt");
            using (var sw = new StreamWriter(resultFile, false))
            {
                sw.WriteLine($"SHA1 for {filename}...");
                sw.WriteLine();
                using (var stream = new FileStream(
                    filename,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    4096,
                    FileOptions.SequentialScan))
                {
                    sw.WriteLine(BitConverter
                        .ToString(SHA1.Create().ComputeHash(stream))
                        .ToLowerInvariant().Replace("-", string.Empty));
                }
                sw.WriteLine();
                var s = DateTime.Now.ToString("f");
                sw.WriteLine($"Generated On: {s}...");
            }
            var p = new ProcessStartInfo
            {
                ErrorDialog = true,
                FileName = resultFile,
                UseShellExecute = true
            };
            Process.Start(p);
        }

        static void CalculateSHA256(string filename)
        {
            var resultFile = Path.Combine(
                Path.GetTempPath(),
                filename + ".sha256.txt");
            using (var sw = new StreamWriter(resultFile, false))
            {
                sw.WriteLine($"SHA256 for {filename}...");
                sw.WriteLine();
                using (var stream = new FileStream(
                    filename,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    4096,
                    FileOptions.SequentialScan))
                {
                    sw.WriteLine(BitConverter
                        .ToString(SHA256.Create().ComputeHash(stream))
                        .ToLowerInvariant().Replace("-", string.Empty));
                }
                sw.WriteLine();
                var s = DateTime.Now.ToString("f");
                sw.WriteLine($"Generated On: {s}...");
            }
            var p = new ProcessStartInfo
            {
                ErrorDialog = true,
                FileName = resultFile,
                UseShellExecute = true
            };
            Process.Start(p);
        }

        static void CalculateSHA384(string filename)
        {
            var resultFile = Path.Combine(
                Path.GetTempPath(),
                filename + ".sha384.txt");
            using (var sw = new StreamWriter(resultFile, false))
            {
                sw.WriteLine($"SHA384 for {filename}...");
                sw.WriteLine();
                using (var stream = new FileStream(
                    filename,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    4096,
                    FileOptions.SequentialScan))
                {
                    sw.WriteLine(BitConverter
                        .ToString(SHA384.Create().ComputeHash(stream))
                        .ToLowerInvariant().Replace("-", string.Empty));
                }
                sw.WriteLine();
                var s = DateTime.Now.ToString("f");
                sw.WriteLine($"Generated On: {s}...");
            }
            var p = new ProcessStartInfo
            {
                ErrorDialog = true,
                FileName = resultFile,
                UseShellExecute = true
            };
            Process.Start(p);
        }

        static void CalculateSHA512(string filename)
        {
            var resultFile = Path.Combine(
                Path.GetTempPath(),
                filename + ".sha512.txt");
            using (var sw = new StreamWriter(resultFile, false))
            {
                sw.WriteLine($"SHA512 for {filename}...");
                sw.WriteLine();
                using (var stream = new FileStream(
                    filename,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    4096,
                    FileOptions.SequentialScan))
                {
                    sw.WriteLine(BitConverter
                        .ToString(SHA512.Create().ComputeHash(stream))
                        .ToLowerInvariant().Replace("-", string.Empty));
                }
                sw.WriteLine();
                var s = DateTime.Now.ToString("f");
                sw.WriteLine($"Generated On: {s}...");
            }
            var p = new ProcessStartInfo
            {
                ErrorDialog = true,
                FileName = resultFile,
                UseShellExecute = true
            };
            Process.Start(p);
        }
    }
}