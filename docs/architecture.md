# PrinterShare Architecture

PrinterShare is split into host, client, driver, and installer layers.

## Host

The host runs on the machine with physical printers. It discovers Windows printers, controls which printers are shared, exposes a TLS 1.3 gRPC endpoint, advertises itself through mDNS, and queues incoming jobs before forwarding them to the Windows spooler.

## Client

The client discovers hosts, registers with certificate-based authentication, and installs one local Windows printer object per shared remote printer. Installation is delegated to `PrinterShare.DriverInstallerService` so driver registration can run with service privileges.

## Driver

The Windows printer integration is represented by a Unidrv package:

- `printerdriver.inf` registers the universal remote printer model.
- `printerdriver.gpd` defines page sizes, color, duplex, resolution, and paper source capabilities.
- `portmonitor.dll` owns `PSHARE:` virtual ports and captures spooler writes.
- `printerui.dll` is the preferences/status extension point.

The current native C++ files are buildable WDK stubs that define the integration boundaries. A production driver must be signed, cataloged, HLK-tested, and expanded to stream print data to the local gRPC bridge.

## Security

The host enforces client certificates at the Kestrel endpoint. Certificates and trust policy are intentionally deployment-specific and should be provisioned by the MSI or enterprise device management.
