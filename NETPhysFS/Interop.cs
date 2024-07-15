using System;
using System.Runtime.InteropServices;

namespace MoonTools.NETPhysFS
{
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
  public delegate int InitDelegate();

  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
  public delegate void DeinitDelegate();

  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
  public delegate IntPtr MallocDelegate(ulong size);

  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
  public delegate IntPtr ReallocDelegate(IntPtr ptr, ulong size);

  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
  public delegate void FreeDelegate(IntPtr ptr);

  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
  public delegate void StringCallback(IntPtr data, string str);

  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
  public delegate void EnumFilesCallback(IntPtr data, string origdir, string fname);

  [StructLayout(LayoutKind.Sequential)]
  public struct ArchiveInfo
  {
    [MarshalAs(UnmanagedType.LPStr)]
    public string extension;

    [MarshalAs(UnmanagedType.LPStr)]
    public string description;

    [MarshalAs(UnmanagedType.LPStr)]
    public string author;

    [MarshalAs(UnmanagedType.LPStr)]
    public string url;
  }

  [StructLayout(LayoutKind.Sequential)]
  public struct Version
  {
    public byte major;
    public byte minor;
    public byte patch;
  }

  [StructLayout(LayoutKind.Sequential)]
  public struct Allocator
  {
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public InitDelegate Init;

    [MarshalAs(UnmanagedType.FunctionPtr)]
    public DeinitDelegate Deinit;

    [MarshalAs(UnmanagedType.FunctionPtr)]
    public MallocDelegate Malloc;

    [MarshalAs(UnmanagedType.FunctionPtr)]
    public ReallocDelegate Realloc;

    [MarshalAs(UnmanagedType.FunctionPtr)]
    public FreeDelegate Free;
  }

  internal static class Interop
  {
    private const string s_nativeLibName = "physfs";

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern void PHYSFS_getLinkedVersion(ref Version v);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_init(string argv0);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_deinit();

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_supportedArchiveTypes();

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern void PHYSFS_freeList(IntPtr h);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_getLastError();

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_getDirSeparator();

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern void PHYSFS_permitSymbolicLinks(int permit);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_getCdRomDirs();

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_getBaseDir();

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_getUserDir();

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_getPrefDir(string org, string app);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_getWriteDir();

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_setWriteDir(string s);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_unmount(string s);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_getSearchPath();

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_setSaneConfig(string s1, string s2, string s3, int i1, int i2);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_mkdir(string s);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_delete(string s);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_getRealDir(string s);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_enumerateFiles(string s);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_exists(string s);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_isDirectory(string s);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_isSymbolicLink(string s);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern long PHYSFS_getLastModTime(string s);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_openWrite(string s);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_openAppend(string s);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_openRead(string s);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_close(IntPtr ptr);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern long PHYSFS_read(IntPtr ptr1, IntPtr ptr2, uint i1, uint i2);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern long PHYSFS_write(IntPtr ptr1, IntPtr ptr2, uint i1, uint i2);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_eof(IntPtr ptr);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern long PHYSFS_tell(IntPtr ptr);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_seek(IntPtr ptr, ulong u);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern long PHYSFS_fileLength(IntPtr ptr);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_setBuffer(IntPtr ptr, ulong u);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_flush(IntPtr ptr);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_isInit();

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_symbolicLinksPermitted();

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_setAllocator(Allocator alloc);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int PHYSFS_mount(string s1, string s2, int i);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern IntPtr PHYSFS_getMountPoint(string s);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern void PHYSFS_getCdRomDirsCallback(StringCallback c, IntPtr p);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern void PHYSFS_getSearchPathCallback(StringCallback c, IntPtr p);

    [DllImport(s_nativeLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern void PHYSFS_enumerateFilesCallback(string s, EnumFilesCallback c, IntPtr p);
  }
}
