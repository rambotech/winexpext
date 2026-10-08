using System.Runtime.CompilerServices;
using NUnit.Framework;
using NUnit.Framework.Internal;
using winexpext;
using winexpext.lib;

namespace winexpext.unittest
{
	[TestFixture]
	public class Tests
	{
		private string UsePath = string.Empty;
		private Dictionary<string, string> _TestFiles = new Dictionary<string, string>();

		public Tests()
		{
		}

		[NUnit.Framework.OneTimeSetUp]
		public void OneTimeSetUp()
		{
			UsePath = Path.Combine(
				Path.GetTempPath(),
				string.Format("winexpext.unittest.{0:yyyyMMdd-HHmmss}", DateTime.Now)
			);
			Directory.CreateDirectory(UsePath);

			_TestFiles.Add("Test_removeCopyMark_1", Path.Combine(UsePath, "test-backup(1).tar - Copy.gz"));
			_TestFiles.Add("Test_removeCopyMark_2", Path.Combine(UsePath, "test-backup(1) - Copy.tar.gz"));
			_TestFiles.Add("Test_removeCopyMark_3", Path.Combine(UsePath, "test-backup(1) - Copy.tar.gz"));
			_TestFiles.Add("Test_removeCopyMark_4", Path.Combine(UsePath, "test-backup(1) - Copy."));
			_TestFiles.Add("Test_removeCopyMark_5", Path.Combine(UsePath, "test-backup(1) - Cop.tar.gz"));
			_TestFiles.Add("Test_removeRenameMark_1", Path.Combine(UsePath, "awsadministration_thedefinitiveguide_reupload (1).mobi"));
			_TestFiles.Add("Test_removeRenameMark_2", Path.Combine(UsePath, "awsadministration_thedefinitiveguide_reupload (1).mobi"));
			_TestFiles.Add("Test_removeRenameMark_3", Path.Combine(UsePath, "awsadministration_thedefinitiveguide_reupload (1).mobi"));
			_TestFiles.Add("Test_removeRenameMark_4", Path.Combine(UsePath, "awsadministration_thedefinitiveguide_reupload(1) .mobi"));
			_TestFiles.Add("Test_removeRenameMark_5", Path.Combine(UsePath, "awsadministration_thedefinitiveguide_reupload(1)"));
			_TestFiles.Add("Test_removeRenameMark_6", Path.Combine(UsePath, "awsadministration_thedefinitiveguide_reupload(1)."));
			_TestFiles.Add("Test_removeRenameMark_7", Path.Combine(UsePath, "awsadministration_thedefinitiveguide_reupload (1) .mobi"));
			_TestFiles.Add("Test_removeTimestampedMark_1", Path.Combine(UsePath, "awsadministration_thedefinitiveguide_reupload-20260526-091716.mobi"));
			_TestFiles.Add("Test_removeTimestampedMark_2", Path.Combine(UsePath, "awsadministration_thedefinitiveguide_reupload-20260526-091716.mobi"));
			_TestFiles.Add("Test_removeTimestampedMark_3", Path.Combine(UsePath, "awsadministration_thedefinitiveguide_reupload-20260526-091716"));
			_TestFiles.Add("Test_removeTimestampedMark_4", Path.Combine(UsePath, "awsadministration_thedefinitiveguide_reupload-20260526-091716."));

			foreach (var key in _TestFiles.Keys)
			{
				if (!File.Exists(_TestFiles[key])) File.Create(_TestFiles[key]);
			}
		}

		[Test]
		public void Test_removeCopyMark_1()
		{
			var result = Methods.RemoveCopyMark(_TestFiles["Test_removeCopyMark_1"]);
			var expected = $"{UsePath}\\test-backup(1).tar.gz";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveCopyMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}

		[Test]
		public void Test_removeCopyMark_2()
		{
			var result = Methods.RemoveCopyMark(_TestFiles["Test_removeCopyMark_2"]);
			var expected = $"{UsePath}\\test-backup(1).tar.gz";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveCopyMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}

		[Test]
		public void Test_removeCopyMark_3()
		{
			var result = Methods.RemoveCopyMark(_TestFiles["Test_removeCopyMark_3"]);
			var expected = $"{UsePath}\\test-backup(1).tar.gz";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveCopyMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}

		[Test]
		public void Test_removeCopyMark_4()
		{
			var result = Methods.RemoveCopyMark(_TestFiles["Test_removeCopyMark_4"]);
			var expected = $"{UsePath}\\test-backup(1)";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveCopyMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}

		[Test]
		public void Test_removeCopyMark_5()
		{
			var result = Methods.RemoveCopyMark(_TestFiles["Test_removeCopyMark_5"]);
			var expected = $"{UsePath}\\test-backup(1) - Cop.tar.gz";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveCopyMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}
		[Test]

		public void Test_removeRenameMark_1()
		{
			var result = Methods.RemoveRenameMark(_TestFiles["Test_removeRenameMark_1"]);
			var expected = $"{UsePath}\\awsadministration_thedefinitiveguide_reupload.mobi";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveRenameMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}

		[Test]
		public void Test_removeRenameMark_2()
		{
			var result = Methods.RemoveRenameMark(_TestFiles["Test_removeRenameMark_2"]);
			var expected = $"{UsePath}\\awsadministration_thedefinitiveguide_reupload.mobi";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveRenameMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}

		[Test]
		public void Test_removeRenameMark_3()
		{
			var result = Methods.RemoveRenameMark(_TestFiles["Test_removeRenameMark_3"]);
			var expected = $"{UsePath}\\awsadministration_thedefinitiveguide_reupload.mobi";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveRenameMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}

		[Test]
		public void Test_removeRenameMark_4()
		{
			var result = Methods.RemoveRenameMark(_TestFiles["Test_removeRenameMark_4"]);
			var expected = $"{UsePath}\\awsadministration_thedefinitiveguide_reupload.mobi";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveRenameMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}

		[Test]
		public void Test_removeRenameMark_5()
		{
			var result = Methods.RemoveRenameMark(_TestFiles["Test_removeRenameMark_5"]);
			var expected = $"{UsePath}\\awsadministration_thedefinitiveguide_reupload";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveRenameMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}

		[Test]
		public void Test_removeRenameMark_6()
		{
			var result = Methods.RemoveRenameMark(_TestFiles["Test_removeRenameMark_6"]);
			var expected = $"{UsePath}\\awsadministration_thedefinitiveguide_reupload";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveRenameMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}

		[Test]
		public void Test_removeRenameMark_7()
		{
			var result = Methods.RemoveRenameMark(_TestFiles["Test_removeRenameMark_7"]);
			var expected = $"{UsePath}\\awsadministration_thedefinitiveguide_reupload.mobi";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveCopyMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}

		[Test]
		public void Test_removeTimestampedMark_1()
		{
			var result = Methods.RemoveTimestampedMark(_TestFiles["Test_removeTimestampedMark_1"]);
			var expected = $"{UsePath}\\awsadministration_thedefinitiveguide_reupload.mobi";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveTimestampedMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}

		[Test]
		public void Test_removeTimestampedMark_2()
		{
			var result = Methods.RemoveTimestampedMark(_TestFiles["Test_removeTimestampedMark_2"]);
			var expected = $"{UsePath}\\awsadministration_thedefinitiveguide_reupload.mobi";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveTimestampedMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}

		[Test]
		public void Test_removeTimestampedMark_3()
		{
			var result = Methods.RemoveTimestampedMark(_TestFiles["Test_removeTimestampedMark_3"]);
			var expected = $"{UsePath}\\awsadministration_thedefinitiveguide_reupload";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveTimestampedMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}

		[Test]
		public void Test_removeTimestampedMark_4()
		{
			var result = Methods.RemoveTimestampedMark(_TestFiles["Test_removeTimestampedMark_4"]);
			var expected = $"{UsePath}\\awsadministration_thedefinitiveguide_reupload";
			Assert.That(string.Compare(expected, result, false) == 0, $"RemoveTimestampedMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
		}
	}
}
