using BOG.SwissArmyKnife;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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

		static string Filename = string.Empty;
		static int fileArgIndex = 0;

		static void Main(string[] args)
		{
			var NothingDone = true;
			try
			{
				var a = new BOG.SwissArmyKnife.AssemblyVersion(BOG.SwissArmyKnife.AssemblyVersion.AssemblySource.Entry);
				ConsoleAndLoggedSimple($"{a.ToString()}", "*main", true);
				if (args.Length == 0)
				{
					throw new ArgumentNullException("No argument(s)");
				}
				if (args.Length == 1)
				{
					var addCmds = new string[] { "--addExplorerExtensions", "-A" };
					if (addCmds.Contains(args[0]))
					{
						ConsoleAndLoggedSimple("Adding Explorer shortcuts...", "--add", true);
						Console.WriteLine();
						new Registry().AddExplorerShortcuts();
						Console.WriteLine("Press ENTER to close...");
						Console.ReadLine();
						System.Environment.Exit(0);
					}
						var delCmds = new string[] { "--deleteExplorerExtensions", "-D" };
					if (delCmds.Contains(args[0]))
					{
						ConsoleAndLoggedSimple("Removing Explorer shortcuts...", "--del", true);
						new Registry().RemoveExplorerShortcuts();
						Console.WriteLine("Press ENTER to close...");
						Console.ReadLine();
						System.Environment.Exit(0);
					}
					throw new ArgumentNullException($"Missing one or more argument(s) for command \"{args[0]}\".");
				}

				Methods.FileAction action;
				if (!Enum.TryParse<Methods.FileAction>(args[0], out action))
				{
					throw new ArgumentException($"Unrecognized command: {args[0]}");
				}
				for (fileArgIndex = 1; fileArgIndex < args.Length; fileArgIndex++)
				{
					Filename = args[fileArgIndex];
					if (!File.Exists(Filename))
					{
						ConsoleAndLoggedSimple($"Skipping #{fileArgIndex} of {args.Length}: file not found: {Filename}", $"Filename #{fileArgIndex}", true);
						continue;
					}
					ConsoleAndLoggedFull($"Processing #{fileArgIndex} of {args.Length}: ", $"Filename #{fileArgIndex}", true);
					switch (action)
					{
						case Methods.FileAction.timestampRename:
							NothingDone = false;
							TimestampedCopyOrRename(TimestampingAction.Rename);
							break;
						case Methods.FileAction.timestampCopy:
							NothingDone = false;
							TimestampedCopyOrRename(TimestampingAction.Copy);
							break;
						case Methods.FileAction.removeCopyMark:
							NothingDone = false;
							RemoveCopyMark();
							break;
						case Methods.FileAction.removeRenameMark:
							NothingDone = false;
							RemoveRenameMark();
							break;
						case Methods.FileAction.removeTimestampedMark:
							NothingDone = false;
							RemoveTimestampedMark();
							break;
						case Methods.FileAction.hashMD5:
							NothingDone = false;
							CalculateHash(HashingMethod.MD5);
							break;
						case Methods.FileAction.hashSHA1:
							NothingDone = false;
							CalculateHash(HashingMethod.SHA1);
							break;
						case Methods.FileAction.hashSHA256:
							NothingDone = false;
							CalculateHash(HashingMethod.SHA256);
							break;
						case Methods.FileAction.hashSHA384:
							NothingDone = false;
							CalculateHash(HashingMethod.SHA384);
							break;
						case Methods.FileAction.hashSHA512:
							NothingDone = false;
							CalculateHash(HashingMethod.SHA512);
							break;
						default:
							throw new Exception($"No known method for the command \"{action}\"");
					}
				}
				if (NothingDone)
				{
					ShowHelp($"No changes were performed when executed {args[0]}");
					System.Environment.ExitCode = 2;
				}
			}
			catch (ArgumentNullException err)
			{
				var ex = (Exception)err;
				ConsoleAndLoggedSimple(DetailedException.WithMachineContent(ref ex), "(ArgumentNullException)", true);
				System.Environment.ExitCode = 1;
			}
			catch (ArgumentException err)
			{
				var ex = (Exception)err;
				ConsoleAndLoggedSimple(DetailedException.WithMachineContent(ref ex), "(ArgumentException)", true);
				System.Environment.ExitCode = 2;
			}
			catch (Exception err)
			{
				var ex = (Exception)err;
				ConsoleAndLoggedSimple(DetailedException.WithMachineContent(ref ex), $"{err.GetType()}", true);
				System.Environment.ExitCode = 3;
			}
			if (System.Environment.ExitCode != 0)
			{
				Console.ReadLine();
				System.Environment.Exit(2);
			}
		}

		static void ShowHelp(string message)
		{
			if (!string.IsNullOrWhiteSpace(message))
			{
				ConsoleAndLoggedSimple($"{message}", string.Empty, true);
				Console.WriteLine(message);
			}

			Console.WriteLine();
			Console.WriteLine("winexpext command filename");
			Console.WriteLine();
			Console.WriteLine("winexpext removeCopyMarks sourceFile [sourceFile [...]]");
			Console.WriteLine("winexpext removeRenameMarks sourceFile [sourceFile [...]]");
			Console.WriteLine("winexpext removeTimestamp sourceFile [sourceFile [...]]");
			Console.WriteLine("winexpext timestampedCopy sourceFile [sourceFile [...]]");
			Console.WriteLine("winexpext timestampedRename sourceFile [sourceFile [...]]");
			Console.WriteLine("winexpext hashMD5 sourceFile [sourceFile [...]]");
			Console.WriteLine("winexpext hashSHA1 sourceFile [sourceFile [...]]");
			Console.WriteLine("winexpext hashSHA256 sourceFile [sourceFile [...]]");
			Console.WriteLine("winexpext hashSHA384 sourceFile [sourceFile [...]]");
			Console.WriteLine("winexpext hashSHA512 sourceFile [sourceFile [...]]");
			Console.WriteLine();
			Console.WriteLine("Use:");
			Console.WriteLine("winexpext command filename [filename [...]]");
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

		static void ConsoleAndLoggedSimple(string message, string functionName, bool logMessage)
		{
			ConsoleAndLogged(message, functionName, logMessage, false);
		}

		static void ConsoleAndLoggedFull(string message, string functionName, bool logMessage)
		{
			ConsoleAndLogged(message, functionName, logMessage, true);
		}

		static void ConsoleAndLogged(string message, string functionName, bool logMessage, bool useDetailsInLog)
		{
			if (logMessage)
			{
				var errorFile = Path.Combine(Path.GetTempPath(), $"winexpext_{DateTime.Now:yyyy-MM-dd}_log.txt");
				using (var sw = new StreamWriter(errorFile, true))
				{
					if (useDetailsInLog)
					{
						sw.WriteLine(new string('-', 50));
						sw.WriteLine($"Generated On:     ... {DateTime.Now:f}");
						sw.WriteLine($"Function:         ... {functionName}");
						sw.WriteLine($"File name:        ... {Filename}");
						sw.WriteLine();
						sw.WriteLine(message);
						sw.WriteLine();
					}
					else
					{
						sw.WriteLine(message);
					}
				}
			}
			Console.WriteLine(message);

		}
	}
}