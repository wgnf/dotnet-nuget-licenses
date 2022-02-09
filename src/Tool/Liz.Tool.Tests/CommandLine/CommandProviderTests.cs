﻿using ArrangeContext.Moq;
using FluentAssertions;
using Liz.Core.Logging.Contracts;
using Liz.Tool.CommandLine;
using System.IO;
using System.Linq;
using Xunit;

namespace Liz.Tool.Tests.CommandLine;

public sealed class CommandProviderTests
{
    [Fact]
    public void Should_Provide_Root_Command()
    {
        var sut = new ArrangeContext<CommandProvider>().Build();

        var rootCommand = sut.Get();
        rootCommand
            .Should()
            .NotBeNull();
        rootCommand
            .Description
            .Should()
            .NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Provided_Root_Command_Should_Have_File_Input_Argument()
    {
        var sut = new ArrangeContext<CommandProvider>().Build();

        var rootCommand = sut.Get();

        var targetFileArgument = rootCommand.Arguments.FirstOrDefault(opt => opt.Name == "targetFile");
        Assert.NotNull(targetFileArgument);

        targetFileArgument
            .Description
            .Should()
            .NotBeNullOrWhiteSpace();

        targetFileArgument
            .ValueType
            .Should()
            .Be<FileInfo>();
    }

    [Fact]
    public void Provided_Root_Command_Should_Have_Log_Level_Option()
    {
        var sut = new ArrangeContext<CommandProvider>().Build();

        var rootCommand = sut.Get();

        var logLevelOption = rootCommand.Options.FirstOrDefault(opt => opt.Name == "log-level");
        Assert.NotNull(logLevelOption);

        logLevelOption
            .Description
            .Should()
            .NotBeNullOrWhiteSpace();

        logLevelOption
            .ValueType
            .Should()
            .Be<LogLevel>();

        logLevelOption
            .Aliases
            .Should()
            .Contain(alias =>
                alias == "--log-level" ||
                alias == "-l");
    }
    
    [Fact]
    public void Provided_Root_Command_Should_Have_Include_Transitive_Option()
    {
        var sut = new ArrangeContext<CommandProvider>().Build();

        var rootCommand = sut.Get();

        var includeTransitiveOption = rootCommand.Options.FirstOrDefault(opt => opt.Name == "include-transitive");
        Assert.NotNull(includeTransitiveOption);

        includeTransitiveOption
            .Description
            .Should()
            .NotBeNullOrWhiteSpace();

        includeTransitiveOption
            .ValueType
            .Should()
            .Be<bool>();

        includeTransitiveOption
            .Aliases
            .Should()
            .Contain(alias =>
                alias == "--include-transitive" ||
                alias == "-i");
    }
    
    [Fact]
    public void Provided_Root_Command_Should_Have_Suppress_Print_Details_Option()
    {
        var sut = new ArrangeContext<CommandProvider>().Build();

        var rootCommand = sut.Get();

        var suppressPrintDetailsOption = rootCommand.Options.FirstOrDefault(opt => opt.Name == "suppress-print-details");
        Assert.NotNull(suppressPrintDetailsOption);

        suppressPrintDetailsOption
            .Description
            .Should()
            .NotBeNullOrWhiteSpace();

        suppressPrintDetailsOption
            .ValueType
            .Should()
            .Be<bool>();

        suppressPrintDetailsOption
            .Aliases
            .Should()
            .Contain(alias =>
                alias == "--suppress-print-details" ||
                alias == "-sp");
    }
    
    [Fact]
    public void Provided_Root_Command_Should_Have_Suppress_Print_Issues_Option()
    {
        var sut = new ArrangeContext<CommandProvider>().Build();

        var rootCommand = sut.Get();

        var suppressPrintIssuesOption = rootCommand.Options.FirstOrDefault(opt => opt.Name == "suppress-print-issues");
        Assert.NotNull(suppressPrintIssuesOption);

        suppressPrintIssuesOption
            .Description
            .Should()
            .NotBeNullOrWhiteSpace();

        suppressPrintIssuesOption
            .ValueType
            .Should()
            .Be<bool>();

        suppressPrintIssuesOption
            .Aliases
            .Should()
            .Contain(alias =>
                alias == "--suppress-print-issues" ||
                alias == "-si");
    }
}
