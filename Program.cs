using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using static System.Formats.Asn1.AsnWriter;

namespace winexpext   // Windows Explorer Extension
{
	class Program
	{
		enum FileAction : int
		{
			timestampCopy,
			timestampRename,
			dropAppendages,
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
				if (args.Length != 2) ShowHelp("Usage");
				FileAction action;
				if (!Enum.TryParse<FileAction>(args[0], out action))
				{
					ShowHelp($"Unrecognized command: {args[0]}");
				}
				if (!File.Exists(args[1]))
				{
					throw new Exception($"File not found: {args[1]}");
				}
				Console.Write($"    processing: {args[1]}: ");
				switch (action)
				{
					case FileAction.timestampRename:
						TimestampedCopyOrRename(args[1], false);
						Console.WriteLine("OK");
						break;
					case FileAction.timestampCopy:
						TimestampedCopyOrRename(args[1], true);
						Console.WriteLine("OK");
						break;
					case FileAction.dropAppendages:
						RemoveCopyAppendages(args[1]);
						Console.WriteLine("OK");
						break;
					case FileAction.hashMD5:
						CalculateMD5(args[1]);
						Console.WriteLine("OK");
						break;
					case FileAction.hashSHA1:
						CalculateSHA1(args[1]);
						Console.WriteLine("OK");
						break;
					case FileAction.hashSHA256:
						CalculateSHA256(args[1]);
						Console.WriteLine("OK");
						break;
					case FileAction.hashSHA384:
						CalculateSHA384(args[1]);
						Console.WriteLine("OK");
						break;
					case FileAction.hashSHA512:
						CalculateSHA512(args[1]);
						Console.WriteLine("OK");
						break;
					default:
						throw new Exception($"No method for defined action \"{action}\"");
				}
				System.Environment.Exit(0);
			}
			catch (Exception err)
			{
				TimedErrorMessage($"{err.Message}", "(Main)", args[1]);
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
			Console.WriteLine("winexpext command [options]");
			Console.WriteLine();
			Console.WriteLine("winexpext timestampedCopy sourceFile [sourceFile2 [...]]");
			Console.WriteLine("winexpext timestampedRename sourceFile [sourceFile2 [...]]");
			Console.WriteLine("winexpext dropAppendages sourceFile");
			Console.WriteLine("winexpext hashMD5 sourceFile");
			Console.WriteLine("winexpext hashSHA1 sourceFile");
			Console.WriteLine("winexpext hashSHA256 sourceFile");
			Console.WriteLine("winexpext hashSHA384 sourceFile");
			Console.WriteLine("winexpext hashSHA512 sourceFile");
			System.Environment.Exit(2);
		}

		static void TimestampedCopyOrRename(string filename, bool isCopy)
		{
			try
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
			catch (Exception err)
			{
				TimedErrorMessage($"Failed to {(isCopy ? "copy" : "rename")} file: {err.Message}", "TimestampedCopyOrRename", filename);
			}
		}

		/// <summary>
		/// Removes "(#) ... appendage on the root file name, which does not cause a naming conflict.
		/// E.g. "file(2).txt" --> "file.txt" if "file.txt" does not exist, but does nothing if "file.txt" still exists.
		/// </summary>
		/// <param name="filename"></param>
		static void RemoveCopyAppendages(string filename)
		{
			if (!File.Exists(filename))
			{
				TimedErrorMessage($"File not found", "RemoveCopyAppendages", filename);
				return;
			}

			var regex = new Regex(@"^(.+)(\s?\([\d]+\))");

			var origfileName = Path.GetFileName(filename);

			var match = regex.Match(origfileName);
			if (!match.Success)
			{
				TimedErrorMessage("Nothing to do: filename has no copy appendage", "RemoveCopyAppendages", filename);
				return;
			}
			var fixedRootName = regex.Replace(origfileName, "$1");
			var fixedFilename = Path.Combine(Path.GetDirectoryName(filename), fixedRootName);
			if (File.Exists(fixedFilename))
			{
				TimedErrorMessage($"Cannot remove copy appendage because the original file still exists", "RemoveCopyAppendages", filename);
				return;
			}
			File.Move(filename, fixedFilename);
		}

		static void CalculateMD5(string filename)
		{
			try
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
			catch (Exception err)
			{
				TimedErrorMessage($"Failed to calculate MD5 hash: {err.Message}", "CalculateMD5", filename);
			}
		}

		static void CalculateSHA1(string filename)
		{
			try
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
			catch (Exception err)
			{
				TimedErrorMessage($"Failed to calculate SHA1 hash: {err.Message}", "CalculateSHA1", filename);
			}
		}

		static void CalculateSHA256(string filename)
		{
			try
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
			catch (Exception err)
			{
				TimedErrorMessage($"Failed to calculate SHA256 hash: {err.Message}", "CalculateSHA256", filename);
			}
		}

		static void CalculateSHA384(string filename)
		{
			try
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
			catch (Exception err)
			{
				TimedErrorMessage($"Failed to calculate SHA384 hash: {err.Message}", "CalculateSHA384", filename);
			}
		}

		static void CalculateSHA512(string filename)
		{
			try
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
			catch (Exception err)
			{
				TimedErrorMessage($"Failed to calculate SHA512 hash: {err.Message}", "CalculateSHA512", filename);
			}
		}

		static void TimedErrorMessage(string message, string functionName, string filename)
		{
			TimedErrorMessage(message, functionName, filename, 20.0d);
		}

		static void TimedErrorMessage(string message, string functionName, string filename, double seconds)
		{
			Console.WriteLine(message);
			Console.Write("Press any key to clear...");
			var timeoutSeconds = seconds < 2.0d ? 5.0d : seconds;
			var deadlineTime = DateTime.Now.AddSeconds(timeoutSeconds);
			while (DateTime.Now <= deadlineTime)
			{
				if (DateTime.Now > deadlineTime)
				{
					break;
				}
				if (Console.KeyAvailable)
				{
					Console.ReadKey(true);
					break;
				}
				System.Threading.Thread.Sleep(100);
			}
			var errorFile = Path.Combine(Path.GetTempPath(), $"ERROR_winexpext_{DateTime.Now:yyyy-MM-dd}.txt");
			using (var sw = new StreamWriter(errorFile, true))
			{
				sw.WriteLine(new string('-', 50));
				sw.WriteLine($"Generated On: ... {DateTime.Now:f}");
				sw.WriteLine($"Function:     ... {functionName}");
				sw.WriteLine($"File name:    ... {filename}");
				sw.WriteLine();
				sw.WriteLine(message);
				sw.WriteLine();
			}
		}
	}
}