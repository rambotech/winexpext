using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace winexpext.lib.Helper
{
	public class Registry
	{
		private string _ExeFullPath = string.Empty;
		public Registry()
		{
			var o = new BOG.SwissArmyKnife.AssemblyVersion(BOG.SwissArmyKnife.AssemblyVersion.AssemblySource.Entry);
			_ExeFullPath = o.Filename.Replace(@"\", @"\\").Replace(".dll", ".exe", StringComparison.OrdinalIgnoreCase);
		}

		public void AddExplorerShortcuts()
		{
			var payload = @"Windows Registry Editor Version 5.00

[HKEY_CLASSES_ROOT\*\shell]
 
[HKEY_CLASSES_ROOT\*\shell\Timestamped Copy]

[HKEY_CLASSES_ROOT\*\shell\Timestamped Copy\command]
@=""{{APPEXE}} timestampCopy \""%1\"" %*""
""extension""=""""

[HKEY_CLASSES_ROOT\*\shell\Timestamped Rename]

[HKEY_CLASSES_ROOT\*\shell\Timestamped Rename\command]
@=""{{APPEXE}} timestampRename \""%1\"" %*""
""extension""=""""

[HKEY_CLASSES_ROOT\*\shell\Remove Copy Mark]

[HKEY_CLASSES_ROOT\*\shell\Remove Copy Mark\command]
@=""{{APPEXE}} removeCopyMark \""%1\"" %*""
""extension""=""""

[HKEY_CLASSES_ROOT\*\shell\Remove Rename Mark]

[HKEY_CLASSES_ROOT\*\shell\Remove Rename Mark\command\]
@=""{{APPEXE}} removeRenameMark \""%1\"" %*""
""extension""=""""

[HKEY_CLASSES_ROOT\*\shell\Remove Timestamp Mark]

[HKEY_CLASSES_ROOT\*\shell\Remove Timestamp Mark\command\]
@=""{{APPEXE}} removeTimestampedMark \""%1\"" %*""
""extension""=""""

[HKEY_CLASSES_ROOT\*\shell\Hash MD5]

[HKEY_CLASSES_ROOT\*\shell\Hash MD5\command]
@=""{{APPEXE}} hashMD5 \""%1\"" %*""
""extension""=""""

[HKEY_CLASSES_ROOT\*\shell\Hash SHA1\command]
@=""{{APPEXE}} hashSHA1 \""%1\"" %*""
""extension""=""""

[HKEY_CLASSES_ROOT\*\shell\Hash SHA256]

[HKEY_CLASSES_ROOT\*\shell\Hash SHA256\command]
@=""{{APPEXE}} exe hashSHA256 \""%1\"" %*""
""extension""=""""

[HKEY_CLASSES_ROOT\*\shell\Hash SHA384]

[HKEY_CLASSES_ROOT\*\shell\Hash SHA384\command]
@=""{{APPEXE}} hashSHA384 \""%1\"" %*""
""extension""=""""

[HKEY_CLASSES_ROOT\*\shell\Hash SHA512]

[HKEY_CLASSES_ROOT\*\shell\Hash SHA512\command]
@=""{{APPEXE}} hashSHA512 \""%1\"" %*""
""extension""=""""

".Replace(@"{{APPEXE}}", _ExeFullPath);
			ImportExplorerShortcutChangeToRegistry(payload, "add_winexpext.reg");
		}

		public void RemoveExplorerShortcuts()
		{
			var payload = @"Windows Registry Editor Version 5.00

[-HKEY_CLASSES_ROOT\*\shell\Timestamped Copy]

[-HKEY_CLASSES_ROOT\*\shell\Timestamped Rename]

[-HKEY_CLASSES_ROOT\*\shell\Remove Copy Mark]

[-HKEY_CLASSES_ROOT\*\shell\Remove Rename Mark]

[-HKEY_CLASSES_ROOT\*\shell\Remove Timestamp Mark]

[-HKEY_CLASSES_ROOT\*\shell\Hash MD5]

[-HKEY_CLASSES_ROOT\*\shell\Hash SHA1]

[-HKEY_CLASSES_ROOT\*\shell\Hash SHA256]

[-HKEY_CLASSES_ROOT\*\shell\Hash SHA384]

[-HKEY_CLASSES_ROOT\*\shell\Hash SHA512]

";
			ImportExplorerShortcutChangeToRegistry(payload, "remove_winexpext.reg");

		}
		public void ImportExplorerShortcutChangeToRegistry(string registryContent, string filename)
		{
			var outputFolder =
				(Environment.GetEnvironmentVariable("TEMP") ?? Environment.GetEnvironmentVariable("TMP"))
				+ @"\BitsOfGenius\winexpext";
			if (!Directory.Exists(outputFolder))
			{
				Directory.CreateDirectory(outputFolder);
			}
			var outputFile = Path.Combine(outputFolder, filename);
			File.WriteAllText(outputFile, registryContent, Encoding.ASCII);
			Process.Start("C:\\Windows\\explorer.exe", $"/n /s /root,\"{outputFile}\"");
			//Process.Start(outputFile);
		}
	}
}