﻿using ArrangeContext.Moq;
using DotnetNugetLicenses.Core.Projects;
using FluentAssertions;
using Moq;
using SlnParser.Contracts;
using System;
using System.IO;
using System.IO.Abstractions;
using Xunit;

namespace DotnetNugetLicenses.Core.Tests.Projects;

public class DefaultGetProjectsTests
{
    [Fact]
    public void Should_Check_If_Target_File_Is_Null()
    {
        var context = new ArrangeContext<GetProjectsViaSlnParser>();
        var sut = context.Build();

        Assert.Throws<ArgumentException>(() => sut.GetFromFile(null!));
    }

    [Fact]
    public void Should_Check_Provided_Target_File_If_Exists_And_Throw_When_Not()
    {
        var context = new ArrangeContext<GetProjectsViaSlnParser>();
        var sut = context.Build();
        
        var targetFileMock = new Mock<IFileInfo>();
        targetFileMock
            .Setup(fileInfo => fileInfo.Exists)
            .Returns(false);

        context
            .For<IFileSystem>()
            .Setup(fileSystem => fileSystem.FileInfo.FromFileName(It.IsAny<string>()))
            .Returns(targetFileMock.Object);

        Assert.Throws<FileNotFoundException>(() => sut.GetFromFile("something"));
    }

    [Theory]
    [InlineData(".txt")]
    [InlineData(".exe")]
    [InlineData(".something super complicated")]
    public void Should_Check_Provided_Target_File_If_Correct_Extension_And_Throw_When_Not(string extension)
    {
        var context = new ArrangeContext<GetProjectsViaSlnParser>();
        var sut = context.Build();

        var targetFileMock = new Mock<IFileInfo>();
        targetFileMock
            .Setup(fileInfo => fileInfo.Exists)
            .Returns(true);
        targetFileMock
            .Setup(fileInfo => fileInfo.Extension)
            .Returns(extension);

        context
            .For<IFileSystem>()
            .Setup(fileSystem => fileSystem.FileInfo.FromFileName(It.IsAny<string>()))
            .Returns(targetFileMock.Object);

        Assert.Throws<ArgumentException>(() => sut.GetFromFile("something"));
    }

    [Fact]
    public void Should_Only_Provide_Project_File_When_File_Is_Csproj()
    {
        var context = new ArrangeContext<GetProjectsViaSlnParser>();
        var sut = context.Build();

        var targetFileMock = new Mock<IFileInfo>();
        targetFileMock
            .Setup(file => file.Exists)
            .Returns(true);
        targetFileMock
            .Setup(file => file.Extension)
            .Returns(".csproj");

        const string expectedName = "some project";

        context
            .For<IFileSystem>()
            .Setup(fileSystem => fileSystem.Path.GetFileNameWithoutExtension(It.IsAny<string>()))
            .Returns(expectedName);
        
        context
            .For<IFileSystem>()
            .Setup(fileSystem => fileSystem.FileInfo.FromFileName(It.IsAny<string>()))
            .Returns(targetFileMock.Object);

        var projects = sut.GetFromFile("something");

        projects
            .Should()
            .ContainSingle(project =>
                project.Name == expectedName &&
                project.File == targetFileMock.Object);
    }

    [Fact]
    public void Should_Only_Provide_Project_File_When_File_Is_Fsproj()
    {
        var context = new ArrangeContext<GetProjectsViaSlnParser>();
        var sut = context.Build();

        var targetFileMock = new Mock<IFileInfo>();
        targetFileMock
            .Setup(file => file.Exists)
            .Returns(true);
        targetFileMock
            .Setup(file => file.Extension)
            .Returns(".fsproj");

        const string expectedName = "some project";

        context
            .For<IFileSystem>()
            .Setup(fileSystem => fileSystem.Path.GetFileNameWithoutExtension(It.IsAny<string>()))
            .Returns(expectedName);
        
        context
            .For<IFileSystem>()
            .Setup(fileSystem => fileSystem.FileInfo.FromFileName(It.IsAny<string>()))
            .Returns(targetFileMock.Object);

        var projects = sut.GetFromFile("something");

        projects
            .Should()
            .ContainSingle(project =>
                project.Name == expectedName &&
                project.File == targetFileMock.Object);
    }

    [Fact]
    public void Should_Provide_Correct_Projects_When_File_Is_Solution()
    {
        var context = new ArrangeContext<GetProjectsViaSlnParser>();
        var sut = context.Build();

        const string targetFile = "something";
        
        var targetFileMock = new Mock<IFileInfo>();
        targetFileMock
            .Setup(file => file.Exists)
            .Returns(true);
        targetFileMock
            .Setup(file => file.Extension)
            .Returns(".sln");
        
        context
            .For<IFileSystem>()
            .Setup(fileSystem => fileSystem.FileInfo.FromFileName(targetFile))
            .Returns(targetFileMock.Object);

        var solutionFolder = new SolutionFolder(Guid.NewGuid(), "Solution Folder", Guid.NewGuid(),
            ProjectType.SolutionFolder);
        var randomProject = new SolutionProject(Guid.NewGuid(), "SomeProject", Guid.NewGuid(), ProjectType.Test,
            new FileInfo("some/file.txt"));
        var notExistingCsproj = new SolutionProject(Guid.NewGuid(), "SomeCsproj", Guid.NewGuid(), ProjectType.Test,
            new FileInfo("some/file.csproj"));

        var existingFile = new FileInfo("some/file.fsproj");

        var existingFsproj = new SolutionProject(Guid.NewGuid(), "SomeFsproj", Guid.NewGuid(), ProjectType.Test,
            existingFile);

        var solution = new Mock<ISolution>();
        solution
            .Setup(sln => sln.AllProjects)
            .Returns(new IProject[] { solutionFolder, randomProject, notExistingCsproj, existingFsproj });

        context
            .For<IFileSystem>()
            .Setup(fileSystem => fileSystem.File.Exists(It.Is<string>(s => s == existingFile.FullName)))
            .Returns(true);

        context
            .For<ISolutionParser>()
            .Setup(slnParser => slnParser.Parse(It.IsAny<string>()))
            .Returns(solution.Object);

        context
            .For<IFileSystem>()
            .Setup(fileSystem => fileSystem.FileInfo.FromFileName(existingFile.FullName))
            .Returns(Mock.Of<IFileInfo>());

        var projects = sut.GetFromFile(targetFile);
        projects
            .Should()
            .ContainSingle(project =>
                project.Name == existingFsproj.Name &&
                project.File != null);
    }
}
