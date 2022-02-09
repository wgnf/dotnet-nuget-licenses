﻿using Liz.Core.Logging.Contracts;
using System.IO;
using System.Threading.Tasks;

namespace Liz.Tool.Contracts.CommandLine;

public interface ICommandRunner
{
    Task RunAsync(
        FileInfo targetFile, 
        LogLevel logLevel, 
        bool includeTransitive, 
        bool suppressPrintDetails,
        bool suppressPrintIssues);
}
