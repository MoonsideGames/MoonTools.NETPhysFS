using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpPhysFS;

namespace Test
{
  internal class Program
  {
    private static PhysFS physFS;

    private static void PrintSupportedArchives()
    {
      Console.Write("Supported archive types: ");
      bool any = false;
      foreach (var archive in physFS.SupportedArchiveTypes())
      {
        any = true;
        Console.WriteLine("\n - {0}: {1}", archive.extension, archive.description);
        Console.WriteLine("   Written by {0}", archive.author);
        Console.Write("   {0}", archive.url);
      }
      if (!any)
      {
        Console.WriteLine("NONE.");
      }
      else
      {
        Console.WriteLine();
      }
    }

    private static IEnumerable<string> ParseInput(string input)
    {
      var sb = new StringBuilder();
      bool openString = false;
      foreach (var c in input)
      {
        if (char.IsWhiteSpace(c))
        {
          if (!openString)
          {
            if (sb.ToString() != "")
            {
              yield return sb.ToString();
            }
            sb.Clear();
            continue;
          }
          else
          {
            sb.Append(c);
          }
        }

        if (c == '"')
        {
          if (sb.ToString() != "")
          {
            yield return sb.ToString();
          }

          sb.Clear();

          openString = !openString;
        }
        else
        {
          sb.Append(c);
        }
      }

      if (sb.ToString() != "")
      {
        yield return sb.ToString();
      }
    }

    private static Dictionary<string, Func<string[], bool>> commands = new Dictionary<string, Func<string[], bool>>();

    #region Commands
    private static bool Help(string[] args)
    {
      Console.WriteLine("Commands:");
      foreach (var kvp in commands)
      {
        Console.WriteLine(" - {0}", kvp.Key);
      }
      return true;
    }

    private static bool Mount(string[] args)
    {
      if (args.Length < 3)
      {
        Console.WriteLine("Usage: mount <archive> <mntpoint> <append>");
        return false;
      }
      if (!bool.TryParse(args[2], out bool append))
      {
        Console.WriteLine("append can only be true or false");
      }

      physFS.Mount(args[0], args[1], append);
      return true;
    }

    private static bool Enumerate(string[] args)
    {
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: enumerate/ls <dir>");
        return false;
      }

      foreach (var f in physFS.EnumerateFiles(args[0]))
      {
        Console.WriteLine(" - {0}", f);
      }
      return true;
    }

    private static bool GetLastError(string[] args)
    {
      Console.WriteLine(physFS.GetLastError());
      return true;
    }

    private static bool GetDirSeparator(string[] args)
    {
      Console.WriteLine(physFS.GetDirSeparator());
      return true;
    }

    private static bool GetCdRomDirectories(string[] args)
    {
      foreach(var d in physFS.GetCdRomDirs())
      {
        Console.WriteLine(" - {0}", d);
      }
      return true;
    }

    private static bool GetSearchPath(string[] args)
    {
      foreach (var d in physFS.GetSearchPath())
      {
        Console.WriteLine(" - {0}", d);
      }
      return true;
    }

    private static bool GetBaseDirectory(string[] args)
    {
      Console.WriteLine(physFS.GetBaseDir());
      return true;
    }

    private static bool GetUserDirectory(string[] args)
    {
      Console.WriteLine(physFS.GetUserDir());
      return true;
    }

    private static bool GetWriteDirectory(string[] args)
    {
      Console.WriteLine(physFS.GetWriteDir());
      return true;
    }

    private static bool SetWriteDirectory(string[] args)
    {
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: setwritedir <dir>");
        return false;
      }
      physFS.SetWriteDir(args[0]);
      return true;
    }

    private static bool PermitSymlinks(string[] args)
    {
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: permitsymlinks <true/false>");
        return false;
      }
      if (!bool.TryParse(args[0], out bool permit))
      {
        Console.WriteLine("Usage: permitsymlinks <true/false>");
      }
      physFS.PermitSymbolicLinks(permit);
      return true;
    }

    private static bool SetSaneConfig(string[] args)
    {
      if(args.Length < 5)
      {
        Console.WriteLine("Usage: setsaneconfig <org> <appName> <arcExt> <includeCdRoms> <archivesFirst>");
        return false;
      }
      if (bool.TryParse(args[3], out bool includeCdRoms) && bool.TryParse(args[4], out bool archivesFirst))
      {
        physFS.SetSaneConfig(args[0], args[1], args[2], includeCdRoms, archivesFirst);
      }
      else
      {
        Console.WriteLine("Usage: setsaneconfig <org> <appName> <arcExt> <includeCdRoms> <archivesFirst>");
      }
      return true;
    }

    private static bool MkDir(string[] args)
    {
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: mkdir <dir>");
        return false;
      }
      physFS.CreateDirectory(args[0]);
      return true;
    }

    private static bool Delete(string[] args)
    {
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: delete <dir>");
        return false;
      }
      physFS.Delete(args[0]);
      return true;
    }

    private static bool GetRealDir(string[] args)
    {
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: getrealdir <dir>");
        return false;
      }
      Console.WriteLine(physFS.GetRealDir(args[0]));
      return true;
    }

    private static bool Exists(string[] args)
    {
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: exists <file>");
        return false;
      }
      Console.WriteLine(physFS.Exists(args[0]));
      return true;
    }

    private static bool IsDir(string[] args)
    {
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: isdir <path>");
        return false;
      }
      Console.WriteLine(physFS.IsDirectory(args[0]));
      return true;
    }

    private static bool IsSymlink(string[] args)
    {
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: issymlink <path>");
        return false;
      }
      Console.WriteLine(physFS.IsSymbolicLink(args[0]));
      return true;
    }

    private static bool Cat(string[] args)
    {
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: cat <file>");
        return false;
      }

      using (var stream = physFS.OpenRead(args[0]))
      using (var reader = new System.IO.StreamReader(stream))
      {
        Console.WriteLine(reader.ReadToEnd());
      }
      return true;
    }

    private static bool FileLength(string[] args)
    {
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: filelength <file>");
        return false;
      }
      using (var stream = physFS.OpenRead(args[0]))
      {
        Console.WriteLine(stream.Length);
      }
      return true;
    }

    private static bool GetMountPoint(string[] args)
    {
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: getmountpoint <file>");
        return false;
      }
      Console.WriteLine(physFS.GetMountPoint(args[0]));
      return true;
    }

    #endregion

    private static void Main(string[] args)
    {
      try
      {
        physFS = new PhysFS("");
      }
      catch (PhysFSLibNotFound)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine("ERROR: PhysFS could not be loaded. Are you sure it is installed or a suitable module is in your working directory?");
        return;
      }

      var version = physFS.GetLinkedVersion();

      Console.WriteLine("SharpPhysFS Test console");
      Console.WriteLine("Loaded PhysFS version: {0}.{1}.{2}", version.major, version.minor, version.patch);
      PrintSupportedArchives();

      Console.WriteLine("Type 'help' for a list of commands");

      commands.Add("help", Help);
      commands.Add("mount", Mount);
      commands.Add("enumerate", Enumerate);
      commands.Add("ls", Enumerate);
      commands.Add("getdirsep", GetDirSeparator);
      commands.Add("getcdromdirs", GetCdRomDirectories);
      commands.Add("getsearchpath", GetSearchPath);
      commands.Add("getbasedir", GetBaseDirectory);
      commands.Add("getuserdir", GetUserDirectory);
      commands.Add("getwritedir", GetWriteDirectory);
      commands.Add("setwritedir", SetWriteDirectory);
      commands.Add("permitsymlinks", PermitSymlinks);
      commands.Add("setsaneconfig", SetSaneConfig);
      commands.Add("mkdir", MkDir);
      commands.Add("delete", Delete);
      commands.Add("getrealdir", GetRealDir);
      commands.Add("exists", Exists);
      commands.Add("isdir", IsDir);
      commands.Add("issymlink", IsSymlink);
      commands.Add("cat", Cat);
      commands.Add("filelength", FileLength);
      commands.Add("getmountpoint", GetMountPoint);

      while (true)
      {
        Console.Write("> ");
        var input = Console.ReadLine();
        var split = ParseInput(input);
        if (split.Count() == 0)
        {
          continue;
        }

        if (split.First() == "quit")
        {
          break;
        }
        else
        {
          if (commands.TryGetValue(split.First(), out Func<string[], bool> cmd))
          {
            try
            {
              if (cmd(split.Skip(1).ToArray()))
              {
                Console.WriteLine("Done.");
              }
            }
            catch (PhysFS.PhysFSException e)
            {
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Error.WriteLine("ERROR: {0}", e.Message);
              Console.ForegroundColor = ConsoleColor.Gray;
            }
          }
          else
          {
            Console.Error.WriteLine("Invalid command");
          }
        }
      }

      physFS.Dispose();
    }
  }
}
