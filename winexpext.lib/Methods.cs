using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace winexpext.lib
{
	public class Methods
	{
		public enum HashingMethod : int
		{
			NotSpecified,
			MD5,
			SHA1,
			SHA256,
			SHA384,
			SHA512
		}

		public enum FileAction : int
		{
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

		public static readonly FileAction[] FileActionTesting =
		[
			FileAction.timestampCopy,
			FileAction.timestampRename,
			FileAction.removeCopyMark,
			FileAction.removeRenameMark
		];

		public static readonly int NoDelay = -1;
		public static readonly int UserMustClose = 0;
		public static readonly bool LogToFile = true;
		public static readonly bool LogToConsole = false;

		public static string TimestampedSuffix(string fileName)
		{
			var datetimePortion = File.GetLastWriteTime(fileName).ToString("-yyyyMMdd-HHmmss");

			return Path.Combine(
				Path.GetDirectoryName(fileName) ?? string.Empty,
				Path.GetFileNameWithoutExtension(fileName) + datetimePortion + Path.GetExtension(fileName)
			);
		}

		public static string RemoveCopyMark(string fileName)
		{
			const string remove = " - Copy";

			var pathEndIndex = fileName.LastIndexOf(Path.DirectorySeparatorChar) + 1;
			var pathPortion = fileName.Substring(0, pathEndIndex);
			var filePortion = pathPortion.Length == fileName.Length ? string.Empty : fileName.Substring(pathEndIndex).Trim();

			var fileIndex = filePortion.LastIndexOf(remove);
			var fileFixed = pathPortion + (fileIndex < 0
				? filePortion
				: filePortion.Substring(0, fileIndex).Trim() + (filePortion + " ").Substring(fileIndex + remove.Length).Trim());

			while (fileFixed.Length > 0 && fileFixed.EndsWith("."))
			{
				fileFixed = fileFixed.Substring(0, fileFixed.Length - 1);
			}

			return fileFixed;
		}

		public static string RemoveRenameMark(string fileName)
		{
			var r = new Regex(@"\([\d]+\)", RegexOptions.None);
			var fileFixed = fileName;

			var matches = r.Matches(fileName);
			if (matches.Count > 0)
			{
				var remove = matches[0];
				var pathEndIndex = fileName.LastIndexOf(Path.DirectorySeparatorChar) + 1;
				var pathPortion = fileName.Substring(0, pathEndIndex);
				var filePortion = pathPortion.Length == fileName.Length ? string.Empty : fileName.Substring(pathEndIndex);

				var fileIndex = filePortion.LastIndexOf(remove.Value);
				fileFixed = pathPortion + (fileIndex < 0
					? filePortion
					: filePortion.Substring(0, fileIndex).Trim() + (filePortion + " ").Substring(fileIndex + remove.Length).Trim());
			}

			while (fileFixed.Length > 0 && fileFixed.EndsWith("."))
			{
				fileFixed = fileFixed.Substring(0, fileFixed.Length - 1);
			}

			return fileFixed;
		}

		public static string RemoveTimestampedMark(string fileName)
		{
			var r = new Regex(@"\-[\d]{8}\-[\d]{6}", RegexOptions.None);
			var fileFixed = fileName;

			var matches = r.Matches(fileName);
			if (matches.Count > 0)
			{
				var remove = matches[0];
				var pathEndIndex = fileName.LastIndexOf(Path.DirectorySeparatorChar) + 1;
				var pathPortion = fileName.Substring(0, pathEndIndex);
				var filePortion = pathPortion.Length == fileName.Length ? string.Empty : fileName.Substring(pathEndIndex);

				var fileIndex = filePortion.LastIndexOf(remove.Value);
				fileFixed = pathPortion + (fileIndex < 0
					? filePortion
					: filePortion.Substring(0, fileIndex).Trim() + (filePortion + " ").Substring(fileIndex + remove.Length).Trim());
			}

			while (fileFixed.Length > 0 && fileFixed.EndsWith("."))
			{
				fileFixed = fileFixed.Substring(0, fileFixed.Length - 1);
			}

			return fileFixed;
		}

		public static string CalculateHash(string filename, HashingMethod hashMethod)
		{
			using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan))
			{
				var hashed = hashMethod switch
				{
					HashingMethod.MD5 => MD5.Create().ComputeHash(stream),
					HashingMethod.SHA1 => SHA1.Create().ComputeHash(stream),
					HashingMethod.SHA256 => SHA256.Create().ComputeHash(stream),
					HashingMethod.SHA384 => SHA384.Create().ComputeHash(stream),
					HashingMethod.SHA512 => SHA512.Create().ComputeHash(stream),
					_ => throw new Exception($"Unrecognized hash method: {hashMethod}")
				};
				return BitConverter.ToString(hashed).ToLowerInvariant().Replace("-", string.Empty);
			}
		}
	}
}