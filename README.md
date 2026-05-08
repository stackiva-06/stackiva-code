# PrinterShare Desktop Application

PrinterShare is a Windows-only .NET 8 desktop application scaffold for sharing printers over LAN or remote networks. It is organized around a host application, a client application, a privileged driver installer service, a Windows Unidrv/port-monitor driver package, and an MSI installer.

> **Driver status:** this repository includes the required driver package shape and WDK C++ integration stubs. Before production use, the native driver must be completed, signed, cataloged, and validated through Microsoft's printer driver certification flow.

## Solution layout

| Path | Purpose |
| --- | --- |
| `src/PrinterShare.Contracts` | Shared gRPC protobuf service and printer capability models. |
| `src/PrinterShare.Host` | Host/server app that discovers local printers, shares selected printers, exposes gRPC, queues jobs, and advertises service discovery. |
| `src/PrinterShare.Client` | WPF client app with Material Design resources for discovering shared printers and requesting local virtual-printer installation. |
| `src/PrinterShare.DriverInstallerService` | Windows service/CLI boundary that performs elevated driver, port, and printer installation commands. |
| `src/PrinterShare.Driver` | WDK-oriented Unidrv INF/GPD plus port monitor and UI native module stubs. |
| `src/PrinterShare.Installer` | WiX package skeleton for MSI delivery. |
| `tests/PrinterShare.Tests` | Unit tests for shared capability defaults. |
| `docs/architecture.md` | Architecture notes and production driver caveats. |

## Core capabilities represented

- Host-side local printer discovery using the Windows printer settings API.
- Per-printer sharing and access-control manager boundaries.
- gRPC service definition for discovery, status streaming, client registration, heartbeats, capability lookup, job submission, job status, and cancellation.
- TLS 1.3/client-certificate configuration entry points on the host.
- WPF/Material Design client shell for remote printer management.
- Privileged driver installer CLI that calls `pnputil`, `prnport.vbs`, and `PrintUIEntry` to register the driver, create a `PSHARE:` port, and add a Windows printer instance.
- Unidrv INF/GPD files describing a real Windows printer model with page size, duplex, color, and DPI options.
- Custom port monitor native stub with spooler-facing entry points for `OpenPort`, `StartDocPort`, `WritePort`, and `EndDocPort`.
- WiX installer skeleton and Windows CI workflow.

## Prerequisites

- Windows 11 or Windows Server with administrator rights.
- .NET SDK 8.0.
- Visual Studio 2022 with WPF/.NET desktop workload.
- WiX Toolset 5.x.
- Windows Driver Kit for building and signing the native driver pieces.

## Build

```powershell
dotnet restore PrinterShare.sln
dotnet build PrinterShare.sln -c Release
dotnet test tests/PrinterShare.Tests/PrinterShare.Tests.csproj -c Release
```

Native driver binaries require a WDK project/build pipeline and are intentionally kept separate from the managed solution until signing and packaging policy are finalized.

## Next implementation milestones

1. Replace the host print queue placeholder with real `System.Printing` spooler submission and job state synchronization.
2. Expand the port monitor to stream captured spooler bytes to a local client bridge over named pipes or loopback gRPC.
3. Add certificate enrollment, pinning, and revocation management to the MSI and management UI.
4. Implement mDNS advertisement/discovery with TXT records for host identity and TLS certificate thumbprint.
5. Add production WDK project files, catalog generation, driver signing, and HLK validation automation.
