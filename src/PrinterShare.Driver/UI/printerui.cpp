#include <windows.h>

extern "C" BOOL APIENTRY DllMain(HMODULE module, DWORD reason, LPVOID reserved)
{
    UNREFERENCED_PARAMETER(module);
    UNREFERENCED_PARAMETER(reason);
    UNREFERENCED_PARAMETER(reserved);
    return TRUE;
}
