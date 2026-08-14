using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using winexpext.lib;
using winexpext.lib.Helper;
using static winexpext.lib.Methods;

namespace winexpext   // Windows Explorer Extension
{
	class Program
	{
		enum TimestampingAction : int
		{
			Copy,
			Rename
		}

		static readonly int NoDelay = -1;
		static readonly int UserMustClose = 0;
		static readonly bool LogToFile = true;
		static readonly bool LogToConsole = false;

		static string Filename = string.Empty;

		static void Main(string[] args)
		{
			try
			{
				if (args.Length == 0)
				{
					ShowHelp("No argument(s)");
					Console.WriteLine("Press ENTER to close...");
					Console.ReadLine();
					System.Environment.Exit(2);
				}
				if (args.Length == 1)
				{
					if (string.Compare(args[0], "--add") == 0)
					{
						new Registry().AddExplorerShortcuts();
						Console.WriteLine("Adding Explorer shortcuts...");
						Console.WriteLine("Press ENTER to close...");
						Console.ReadLine();
						System.Environment.Exit(0);
					}
					if (string.Compare(args[0], "--del") == 0)
					{
						new Registry().RemoveExplorerShortcuts();
						Console.WriteLine("Removing Explorer shortcuts...");
						Console.WriteLine("Press ENTER to close...");
						Console.ReadLine();
						System.Environment.Exit(0);
					}
				}

				Methods.FileAction action;
				if (!Enum.TryParse<Methods.FileAction>(args[0], out action))
				{
					ShowHelp($"Unrecognized command: {args[0]}");
					Console.WriteLine("Press ENTER to close...");
					Console.ReadLine();
					System.Environment.Exit(2);
				}
				for (int i = 0; i < args.Length; i++)
				{
					Filename = args[i];
					if (!File.Exists(Filename))
					{
						Console.WriteLine($"-- File not found: {Filename}");
						continue;
					}
					Console.Write($"Processing: {args[1]}: ");
					switch (action)
					{
						case Methods.FileAction.timestampRename:
							TimestampedCopyOrRename(TimestampingAction.Rename);
							Console.WriteLine("OK");
							break;
						case Methods.FileAction.timestampCopy:
							TimestampedCopyOrRename(TimestampingAction.Copy);
							Console.WriteLine("OK");
							break;
						case Methods.FileAction.removeCopyMark:
							RemoveCopyMark();
							Console.WriteLine("OK");
							break;
						case Methods.FileAction.removeRenameMark:
							RemoveRenameMark();
							Console.WriteLine("OK");
							break;
						case Methods.FileAction.removeTimestampedMark:
							RemoveTimestampedMark();
							Console.WriteLine("OK");
							break;
						case Methods.FileAction.hashMD5:
							CalculateHash(HashingMethod.MD5);
							Console.WriteLine("OK");
							break;
						case Methods.FileAction.hashSHA1:
							CalculateHash(HashingMethod.SHA1);
							Console.WriteLine("OK");
							break;
						case Methods.FileAction.hashSHA256:
							CalculateHash(HashingMethod.SHA256);
							Console.WriteLine("OK");
							break;
						case Methods.FileAction.hashSHA384:
							CalculateHash(HashingMethod.SHA384);
							Console.WriteLine("OK");
							break;
						case Methods.FileAction.hashSHA512:
							CalculateHash(HashingMethod.SHA512);
							Console.WriteLine("OK");
							break;
						default:
							throw new Exception($"No method for defined action \"{action}\"");
					}
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
			Console.WriteLine("winexpext removeCopyMarks sourceFile");
			Console.WriteLine("winexpext removeRenameMarks sourceFile");
			Console.WriteLine("winexpext removeTimestamp sourceFile");
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

		/// <summary>
		/// Removes " - Copy." ... appendage on the root file name, which does not cause a naming conflict.
		/// These appendages are added by Windows when copying a file in the same directory, and are not added when renaming a file 
		/// to an existing name. So this method is for removing copy appendages, but not rename appendages.
		/// E.g. "filename - Copy.txt" --> "filename.txt" if "file.txt" does not exist, but does nothing if "file.txt" still exists.
		/// </summary>
		/// <param name="filename"></param>
		static void RemoveCopyMark()
		{
			var newFilename = Methods.RemoveCopyMark(Filename);
			// accommodates a file with no extension after its name.
			if (File.Exists(newFilename))
			{
				throw new ArgumentException($"Cannot remove copy mark because the original file still exists: {newFilename}");
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
			var newFilename = Methods.RemoveRenameMark(Filename);
			// accommodates a file with no extension after its name.
			if (File.Exists(newFilename))
			{
				throw new ArgumentException($"Cannot remove rename mark because the original file still exists: {newFilename}");
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
				var message = $"Error removing rename mark:\r\nFrom: .. \"{Filename}\"\r\nTo: .... \"{newFilename}\"";
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
		static void RemoveTimestampedMark()
		{
			var newFilename = Methods.RemoveTimestampedMark(Filename);
			// accommodates a file with no extension after its name.
			if (File.Exists(newFilename))
			{
				throw new ArgumentException($"Cannot remove timestamp because the original file still exists: {newFilename}");
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
				var message = $"Error removing timestamp :\r\nFrom: .. \"{Filename}\"\r\nTo: .... \"{newFilename}\"";
				throw new Exception(message, err);
			}
		}

		static void TimestampedCopyOrRename(TimestampingAction action)
		{
			var newFilename = Methods.TimestampedSuffix(Filename);
			// accommodates a file with no extension after its name.
			if (File.Exists(newFilename))
			{
				throw new ArgumentException($"Cannot remove timestamp because the original file still exists: {newFilename}");
			}

			try
			{
				if (action == TimestampingAction.Copy)
				{
					File.Copy(
						Filename,
						newFilename);
				}
				else
				{
					File.Move(
						Filename,
						newFilename);
				}
			}
			catch (Exception err)
			{
				var message = $"Failed to {action} file:\r\nFrom: .. \"{Filename}\"\r\nTo: .... \"{newFilename}\"";
				throw new Exception(message, err);
			}
		}

		static void CalculateHash(Methods.HashingMethod hashMethod)
		{
			var resultFile = Path.Combine(
				Path.GetTempPath(),
				Filename + $".{hashMethod}.txt");
			using (var sw = new StreamWriter(resultFile, false))
			{
				sw.WriteLine($"{hashMethod} for {Filename}...");
				sw.WriteLine();
				sw.WriteLine(Methods.CalculateHash(Filename, hashMethod));
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