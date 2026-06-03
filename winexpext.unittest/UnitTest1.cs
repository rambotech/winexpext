using NUnit.Framework.Internal;
using winexpext;

namespace winexpext.unittest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_removeCopyMark_1()
        {
            const string testFileName = @"c:\_1\test-backup(1).tar - Copy.gz";
            var result = lib.Helpers.Methods.RemoveCopyMark(testFileName);
            var expected = @"c:\_1\test-backup(1).tar.gz";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveCopyMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }

        [Test]
        public void Test_removeCopyMark_2()
        {
            const string testFileName = @"c:\_1\test-backup(1) - Copy.tar.gz";
            var result = lib.Helpers.Methods.RemoveCopyMark(testFileName);
            var expected = @"c:\_1\test-backup(1).tar.gz";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveCopyMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }

        [Test]
        public void Test_removeCopyMark_3()
        {
            const string testFileName = @"c:\_1\test-backup(1) - Copy";
            var result = lib.Helpers.Methods.RemoveCopyMark(testFileName);
            var expected = @"c:\_1\test-backup(1)";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveCopyMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }


        [Test]
        public void Test_removeCopyMark_4()
        {
            const string testFileName = @"c:\_1\test-backup(1) - Cop.tar.gz";
            var result = lib.Helpers.Methods.RemoveCopyMark(testFileName);
            var expected = @"c:\_1\test-backup(1) - Cop.tar.gz";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveCopyMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }
        [Test]
        public void Test_removeRenameMark_1()
        {
            const string testFileName = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload (1).mobi";
            var result = lib.Helpers.Methods.RemoveRenameMark(testFileName);
            var expected = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload.mobi";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveRenameMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }

        [Test]
        public void Test_removeRenameMark_2()
        {
            const string testFileName = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload (1).mobi";
            var result = lib.Helpers.Methods.RemoveRenameMark(testFileName);
            var expected = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload.mobi";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveRenameMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }

        [Test]
        public void Test_removeRenameMark_3()
        {
            const string testFileName = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload(1).mobi";
            var result = lib.Helpers.Methods.RemoveRenameMark(testFileName);
            var expected = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload.mobi";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveRenameMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }

        [Test]
        public void Test_removeRenameMark_4()
        {
            const string testFileName = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload(1) .mobi";
            var result = lib.Helpers.Methods.RemoveRenameMark(testFileName);
            var expected = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload.mobi";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveRenameMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }

        [Test]
        public void Test_removeRenameMark_5()
        {
            const string testFileName = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload(1)";
            var result = lib.Helpers.Methods.RemoveRenameMark(testFileName);
            var expected = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveRenameMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }

        [Test]
        public void Test_removeRenameMark_6()
        {
            const string testFileName = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload(1).";
            var result = lib.Helpers.Methods.RemoveRenameMark(testFileName);
            var expected = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload.";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveRenameMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }

        [Test]
        public void Test_removeRenameMark_7()
        {
            const string testFileName = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload (1) .mobi";
            var result = lib.Helpers.Methods.RemoveRenameMark(testFileName);
            var expected = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload.mobi";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveCopyMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }

        [Test]
        public void Test_removeTimestampedMark_1()
        {
            const string testFileName = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload-20260526-091716.mobi";
            var result = lib.Helpers.Methods.RemoveTimestampedMark(testFileName);
            var expected = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload.mobi";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveTimestampedMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }

        [Test]
        public void Test_removeTimestampedMark_2()
        {
            const string testFileName = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload-20260526-091716.mobi";
            var result = lib.Helpers.Methods.RemoveTimestampedMark(testFileName);
            var expected = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload.mobi";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveTimestampedMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }

        [Test]
        public void Test_removeTimestampedMark_3()
        {
            const string testFileName = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload-20260526-091716";
            var result = lib.Helpers.Methods.RemoveTimestampedMark(testFileName);
            var expected = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveTimestampedMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }

        [Test]
        public void Test_removeTimestampedMark_4()
        {
            const string testFileName = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload-20260526-091716.";
            var result = lib.Helpers.Methods.RemoveTimestampedMark(testFileName);
            var expected = @"C:\Users\johnm\Documents\Books\CloudComputing\awsadministration_thedefinitiveguide_reupload.";
            Assert.That(string.Compare(expected, result, false) == 0, $"RemoveTimestampedMark()\r\n:.. expected {expected}\r\n:.. actual: {result}");
        }
    }
}

/*
test-backup(1).tar - Copy.gz

test-backup(1).tar.gz

test-backup(2).tar - Copy.gz

test-backup(2).tar.gz

test-backup.tar.gz

test-copy.txt

test-rename.txt

test-rename-20260526-091716.txt


*/