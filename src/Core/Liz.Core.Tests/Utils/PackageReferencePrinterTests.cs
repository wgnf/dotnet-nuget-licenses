﻿using ArrangeContext.Moq;
using Liz.Core.License.Contracts.Models;
using Liz.Core.Logging.Contracts;
using Liz.Core.PackageReferences.Contracts.Models;
using Liz.Core.Settings;
using Liz.Core.Utils;
using Moq;
using Xunit;

namespace Liz.Core.Tests.Utils;

public class PackageReferencePrinterTests
{
    [Fact]
    public void PrintPackageReferences_Fails_On_Invalid_Parameters()
    {
        var settings = Mock.Of<ExtractLicensesSettingsBase>();
        settings.SuppressPrintDetails = false;
        
        var context = ArrangeContext<PackageReferencePrinter>.Create();
        context.Use(settings);
        
        var sut = context.Build();

        Assert.Throws<ArgumentNullException>(() => sut.PrintPackageReferences(null!));
    }

    [Fact]
    public void PrintPackageReferences_Does_Nothing_When_Disabled()
    {
        var settings = Mock.Of<ExtractLicensesSettingsBase>();
        settings.SuppressPrintDetails = true;
        
        var context = ArrangeContext<PackageReferencePrinter>.Create();
        context.Use(settings);
        
        var sut = context.Build();

        var packageReference = new PackageReference("SomePackage", "net6.0", "1.2.5")
        {
            LicenseInformation = new LicenseInformation
            {
                Text = "Something something", 
                Type = "MIT", 
                Url = "https://example.org/"
            }
        };

        sut.PrintPackageReferences(new[] { packageReference });

        context
            .For<ILogger>()
            .Verify(logger => 
                logger.Log(It.IsAny<LogLevel>(), It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
    }
    
    [Fact]
    public void PrintPackageReferences_Does_Nothing_When_No_Package_References()
    {
        var settings = Mock.Of<ExtractLicensesSettingsBase>();
        settings.SuppressPrintDetails = false;
        
        var context = ArrangeContext<PackageReferencePrinter>.Create();
        context.Use(settings);
        
        var sut = context.Build();
        
        sut.PrintPackageReferences(Enumerable.Empty<PackageReference>());

        context
            .For<ILogger>()
            .Verify(logger => 
                logger.Log(It.IsAny<LogLevel>(), It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
    }

    [Fact]
    public void PrintPackageReferences()
    {
        var settings = Mock.Of<ExtractLicensesSettingsBase>();
        settings.SuppressPrintDetails = false;
        
        var context = ArrangeContext<PackageReferencePrinter>.Create();
        context.Use(settings);
        
        var sut = context.Build();
        
        var packageReference = new PackageReference("SomePackage", "net6.0", "1.2.5")
        {
            LicenseInformation = new LicenseInformation
            {
                Text = "Something something", 
                Type = "MIT", 
                Url = "https://example.org/"
            }
        };

        sut.PrintPackageReferences(new[] { packageReference });

        context
            .For<ILogger>()
            .Verify(logger =>
                    logger.Log(
                        It.Is<LogLevel>(level => level == LogLevel.Information),
                        It.Is<string>(message =>
                            message.Contains(packageReference.Name) && 
                            message.Contains(packageReference.Version) &&
                            message.Contains(packageReference.LicenseInformation.Url) &&
                            message.Contains(packageReference.LicenseInformation.Type)),
                        It.Is<Exception>(exception => exception == null)),
                Times.Once);
    }
    
    [Fact]
    public void PrintPackageReferencesIssues_Fails_On_Invalid_Parameters()
    {
        var settings = Mock.Of<ExtractLicensesSettingsBase>();
        settings.SuppressPrintIssues = false;
        
        var context = ArrangeContext<PackageReferencePrinter>.Create();
        context.Use(settings);
        
        var sut = context.Build();

        Assert.Throws<ArgumentNullException>(() => sut.PrintPackageReferencesIssues(null!));
    }

    [Fact]
    public void PrintPackageReferencesIssues_Does_Nothing_When_Disabled()
    {
        var settings = Mock.Of<ExtractLicensesSettingsBase>();
        settings.SuppressPrintIssues = true;
        
        var context = ArrangeContext<PackageReferencePrinter>.Create();
        context.Use(settings);
        
        var sut = context.Build();

        var packageReference = new PackageReference("SomePackage", "net6.0", "1.2.5")
        {
            LicenseInformation = new LicenseInformation
            {
                Text = "Something something", 
                Type = "MIT", 
                Url = "https://example.org/"
            }
        };

        sut.PrintPackageReferencesIssues(new[] { packageReference });

        context
            .For<ILogger>()
            .Verify(logger => 
                logger.Log(It.IsAny<LogLevel>(), It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
    }
    
    [Fact]
    public void PrintPackageReferencesIssues_Does_Nothing_When_No_Package_References()
    {
        var settings = Mock.Of<ExtractLicensesSettingsBase>();
        settings.SuppressPrintIssues = false;
        
        var context = ArrangeContext<PackageReferencePrinter>.Create();
        context.Use(settings);
        
        var sut = context.Build();
        
        sut.PrintPackageReferencesIssues(Enumerable.Empty<PackageReference>());

        context
            .For<ILogger>()
            .Verify(logger => 
                logger.Log(It.IsAny<LogLevel>(), It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
    }
    
    [Fact]
    public void PrintPackageReferencesIssues_Does_Nothing_When_No_Issues()
    {
        var settings = Mock.Of<ExtractLicensesSettingsBase>();
        settings.SuppressPrintIssues = false;
        
        var context = ArrangeContext<PackageReferencePrinter>.Create();
        context.Use(settings);
        
        var sut = context.Build();

        var packageReference = new PackageReference("SomePackage", "net6.0", "1.2.5")
        {
            LicenseInformation = new LicenseInformation
            {
                Text = "Something something", 
                Type = "MIT", 
                Url = "https://example.org/"
            }
        };

        sut.PrintPackageReferencesIssues(new[] { packageReference });

        context
            .For<ILogger>()
            .Verify(logger => 
                logger.Log(It.IsAny<LogLevel>(), It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
    }

    [Fact]
    public void PrintPackageReferencesIssues()
    {
        var settings = Mock.Of<ExtractLicensesSettingsBase>();
        settings.SuppressPrintIssues = false;
        
        var context = ArrangeContext<PackageReferencePrinter>.Create();
        context.Use(settings);
        
        var sut = context.Build();

        var packageReference = new PackageReference("SomePackage", "net6.0", "1.2.5");

        sut.PrintPackageReferencesIssues(new[] { packageReference });

        context
            .For<ILogger>()
            .Verify(logger =>
                    logger.Log(
                        It.Is<LogLevel>(level => level == LogLevel.Warning),
                        It.Is<string>(message =>
                            message.Contains(packageReference.Name) && 
                            message.Contains(packageReference.Version) &&
                            message.Contains("URL") &&
                            message.Contains("Type") &&
                            message.Contains("Text")),
                        It.Is<Exception>(exception => exception == null)),
                Times.Once);
    }
}
