using System;
using System.IO;
using System.Linq;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace IntegrationTests
{
    [TestClass]
    public class FileSystemVisitorTest
    {
        private FileSystemVisitor _fileSystemVisitor;
        private const string NewFolder1 = "NewFolder1";
        private const string NewFolder2 = "NewFolder2";
        private const string NewFolder3 = "NewFolder3";
        private const string File1 = "HTMLPage1.html";
        private const string File2 = "HTMLPage2.html";
        private const string File3 = "HTMLPage3.html";
        private int _counter;

        [TestInitialize]
        public void Setup()
        {
            _fileSystemVisitor = new FileSystemVisitor(
                new DirectoryInfo($".\\{NewFolder1}"));
        }

        [TestCleanup]
        public void CleanUp()
        {
            _counter = 0;
        }

        [TestMethod]
        public void GenerateDirectoryTree_WorksAsExpected_ReturnCorrectDirectoryTree()
        {
            // Arrange
            // Act
            var tree = _fileSystemVisitor.GenerateDirectoryTree();

            // Assert
            tree.ShouldNotBeNull();
            tree.Count().ShouldBe(5);
            tree.ShouldContain(t => t.Name.Equals(NewFolder2));
            tree.ShouldContain(t => t.Name.Equals(NewFolder3));
            tree.ShouldContain(t => t.Name.Equals(File1));
            tree.ShouldContain(t => t.Name.Equals(File2));
            tree.ShouldContain(t => t.Name.Equals(File3));
        }

        [TestMethod]
        public void GenerateDirectoryTree_FileFIndedWorkAsExpected_CorrectNumberOfCalls()
        {
            // Arrange
            _fileSystemVisitor.FileFinded += FakeHandler;
            
            // Act
            var tree = _fileSystemVisitor.GenerateDirectoryTree();

            // Assert
            tree.ShouldNotBeNull();
            _counter.ShouldBe(3);
        }

        [TestMethod]
        public void GenerateDirectoryTree_DirectoryFindedWorkAsExpected_CorrectNumberOfCalls()
        {
            // Arrange
            _fileSystemVisitor.DirectoryFinded += FakeHandler;

            // Act
            var tree = _fileSystemVisitor.GenerateDirectoryTree();

            // Assert
            tree.ShouldNotBeNull();
            _counter.ShouldBe(2);
        }

        [TestMethod]
        public void GenerateDirectoryTree_FilteredFileFindedWorkAsExpectedWithoutFilter_CorrectNumberOfCalls()
        {
            // Arrange
            _fileSystemVisitor.FilteredFileFinded += FakeHandler;

            // Act
            var tree = _fileSystemVisitor.GenerateDirectoryTree();

            // Assert
            tree.ShouldNotBeNull();
            _counter.ShouldBe(0);
        }

        [TestMethod]
        public void GenerateDirectoryTree_FilteredDirectoryFindedWorkAsExpectedWithoutFilter_CorrectNumberOfCalls()
        {
            // Arrange
            _fileSystemVisitor.FilteredDirectoryFinded += FakeHandler;

            // Act
            var tree = _fileSystemVisitor.GenerateDirectoryTree();

            // Assert
            tree.ShouldNotBeNull();
            _counter.ShouldBe(0);
        }

        [TestMethod]
        public void GenerateDirectoryTree_FilteredFileFindedWorkAsExpected_CorrectNumberOfCalls()
        {
            // Arrange
            _fileSystemVisitor = new FileSystemVisitor(
                new DirectoryInfo($".\\{NewFolder1}"), fileFilter: info => 
                    info.Name.Contains("HTML"));
            _fileSystemVisitor.FilteredFileFinded += FakeHandler;

            // Act
            var tree = _fileSystemVisitor.GenerateDirectoryTree();

            // Assert
            tree.ShouldNotBeNull();
            _counter.ShouldBe(3);
        }

        [TestMethod]
        public void GenerateDirectoryTree_FilteredDirectoryFindedWorkAsExpected_CorrectNumberOfCalls()
        {
            // Arrange
            _fileSystemVisitor = new FileSystemVisitor(
                new DirectoryInfo($".\\{NewFolder1}"), info =>
                    info.Name.Contains("Folder"));
            _fileSystemVisitor.FilteredDirectoryFinded += FakeHandler;

            // Act
            var tree = _fileSystemVisitor.GenerateDirectoryTree();

            // Assert
            tree.ShouldNotBeNull();
            _counter.ShouldBe(2);
        }

        [TestMethod]
        public void GenerateDirectoryTree_FileExclusionWorks_CorrectResultSet()
        {
            // Arrange
            _fileSystemVisitor = new FileSystemVisitor(
                new DirectoryInfo($".\\{NewFolder1}"));
            _fileSystemVisitor.FileFinded += (sender, arg) =>
            {
                if (arg.FileSystemInfo.Name.Contains("2"))
                    arg.IsExcluded = true;
            };

            _fileSystemVisitor.FileFinded += (sender, arg) =>
            {
                if (arg.FileSystemInfo.Name.Contains("2"))
                    arg.IsExcluded = true;
            };

            // Act
            var tree = _fileSystemVisitor.GenerateDirectoryTree();

            // Assert
            tree.ShouldNotBeNull();
            tree.Count().ShouldBe(4);
            tree.ShouldContain(t => t.Name.Equals(NewFolder3));
            tree.ShouldContain(t => t.Name.Equals(NewFolder2));
            tree.ShouldContain(t => t.Name.Equals(File1));
            tree.ShouldContain(t => t.Name.Equals(File3));
        }

        [TestMethod]
        public void GenerateDirectoryTree_DirectoryExclusionWorks_CorrectResultSet()
        {
            // Arrange
            _fileSystemVisitor = new FileSystemVisitor(
                new DirectoryInfo($".\\{NewFolder1}"));
            _fileSystemVisitor.DirectoryFinded += (sender, arg) =>
            {
                if (arg.FileSystemInfo.Name.Contains("2"))
                    arg.IsExcluded = true;
            };

            // Act
            var tree = _fileSystemVisitor.GenerateDirectoryTree();

            // Assert
            tree.ShouldNotBeNull();
            tree.Count().ShouldBe(4);
            tree.ShouldContain(t => t.Name.Equals(NewFolder3));
            tree.ShouldContain(t => t.Name.Equals(File2));
            tree.ShouldContain(t => t.Name.Equals(File1));
            tree.ShouldContain(t => t.Name.Equals(File3));
        }

        [TestMethod]
        public void GenerateDirectoryTree_FileFilterWorks_CorrectResultSet()
        {
            // Arrange
            const string ending = "2.html";
            _fileSystemVisitor = new FileSystemVisitor(
                new DirectoryInfo($".\\{NewFolder1}"), fileFilter: 
                info => info.Name.EndsWith(ending));

            // Act
            var tree = _fileSystemVisitor.GenerateDirectoryTree();

            // Assert
            tree.ShouldNotBeNull();
            tree.Count().ShouldBe(3);
            tree.ShouldContain(t => t.Name.EndsWith(ending));
        }

        [TestMethod]
        public void GenerateDirectoryTree_DirectoryFilterWorks_CorrectResultSet()
        {
            // Arrange
            const string ending = "2";
            _fileSystemVisitor = new FileSystemVisitor(
                new DirectoryInfo($".\\{NewFolder1}"), directoryFilter:
                info => info.Name.EndsWith(ending));

            // Act
            var tree = _fileSystemVisitor.GenerateDirectoryTree();

            // Assert
            tree.ShouldNotBeNull();
            tree.Count().ShouldBe(4);
            tree.ShouldContain(t => t.Name.EndsWith(ending));
        }

        private void FakeHandler(object obj, EventArgs args)
        {
            _counter++;
        }
    }
}
