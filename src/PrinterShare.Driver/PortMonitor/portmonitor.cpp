#include <windows.h>
#include <winsplp.h>

extern "C" BOOL WINAPI InitializePrintMonitor2(PMONITORINIT monitorInit, PHANDLE monitor)
{
    UNREFERENCED_PARAMETER(monitorInit);
    *monitor = reinterpret_cast<HANDLE>(0x50534852);
    return TRUE;
}

extern "C" BOOL WINAPI OpenPort(HANDLE monitor, LPWSTR portName, PHANDLE port)
{
    UNREFERENCED_PARAMETER(monitor);
    UNREFERENCED_PARAMETER(portName);
    *port = reinterpret_cast<HANDLE>(0x50534A42);
    return TRUE;
}

extern "C" BOOL WINAPI StartDocPort(HANDLE port, LPWSTR printerName, DWORD jobId, DWORD level, LPBYTE docInfo)
{
    UNREFERENCED_PARAMETER(port);
    UNREFERENCED_PARAMETER(printerName);
    UNREFERENCED_PARAMETER(jobId);
    UNREFERENCED_PARAMETER(level);
    UNREFERENCED_PARAMETER(docInfo);
    return TRUE;
}

extern "C" BOOL WINAPI WritePort(HANDLE port, LPBYTE buffer, DWORD bytesToWrite, LPDWORD bytesWritten)
{
    UNREFERENCED_PARAMETER(port);
    // Production implementation streams this buffer to the local gRPC client bridge.
    *bytesWritten = bytesToWrite;
    UNREFERENCED_PARAMETER(buffer);
    return TRUE;
}

extern "C" BOOL WINAPI EndDocPort(HANDLE port)
{
    UNREFERENCED_PARAMETER(port);
    return TRUE;
}

extern "C" BOOL WINAPI ClosePort(HANDLE port)
{
    UNREFERENCED_PARAMETER(port);
    return TRUE;
}
