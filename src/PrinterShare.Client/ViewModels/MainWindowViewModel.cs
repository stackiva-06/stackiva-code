using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PrinterShare.Client.ViewModels;

public sealed partial class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<RemotePrinterViewModel> Printers { get; } =
    [
        new("Accounting Laser", "HOST-PC", "HP LaserJet", "Discovered"),
        new("Warehouse Labels", "HOST-PC", "Zebra ZD", "Discovered")
    ];

    [RelayCommand]
    private void Discover()
    {
        // mDNS discovery and authenticated gRPC enumeration are implemented by PrinterDiscoveryClient.
    }

    [RelayCommand]
    private void InstallSelected()
    {
        // Elevates through PrinterShare.DriverInstallerService to add the Unidrv driver and PSHARE: port.
    }
}

public sealed record RemotePrinterViewModel(string DisplayName, string HostName, string Model, string Status);
