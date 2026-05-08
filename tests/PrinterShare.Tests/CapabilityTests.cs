using Xunit;
using PrinterShare.Contracts.Models;

namespace PrinterShare.Tests;

public sealed class CapabilityTests
{
    [Fact]
    public void DefaultCapabilitiesExposeRequiredPrintOptions()
    {
        var capabilities = PrinterCapabilitySet.Default;

        Assert.Contains("A4", capabilities.PageSizes);
        Assert.Contains("Letter", capabilities.PageSizes);
        Assert.Contains(600, capabilities.ResolutionsDpi);
        Assert.True(capabilities.SupportsColor);
        Assert.True(capabilities.SupportsDuplex);
    }
}
