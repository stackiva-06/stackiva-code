namespace PrinterShare.Contracts.Models;

public sealed record SharedPrinter(
    string PrinterId,
    string DisplayName,
    string HostName,
    string Model,
    bool IsDefault,
    bool IsShared,
    PrinterCapabilitySet Capabilities);

public sealed record PrinterCapabilitySet(
    IReadOnlyList<string> PageSizes,
    IReadOnlyList<int> ResolutionsDpi,
    bool SupportsColor,
    bool SupportsDuplex,
    IReadOnlyList<string> PaperSources,
    bool SupportsStapling,
    bool SupportsHolePunch)
{
    public static PrinterCapabilitySet Default => new(
        ["A4", "Letter", "Legal", "Executive"],
        [300, 600, 1200],
        SupportsColor: true,
        SupportsDuplex: true,
        ["Auto", "Tray 1", "Manual Feed"],
        SupportsStapling: false,
        SupportsHolePunch: false);
}
