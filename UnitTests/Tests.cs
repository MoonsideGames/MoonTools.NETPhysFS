using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SharpPhysFS;

namespace UnitTests
{
  public class Tests
  {
    [Test]
    public void IsInit()
    {
      using var pfs = new PhysFS("");
      Assert.True(pfs.IsInit(), "PhysFS was not initialized");
    }

    [Theory]
    [TestCase(3, 0, 2)]
    public void VersionCheck(byte major, byte minor, byte patch)
    {
      using var pfs = new PhysFS("");
      new SharpPhysFS.Version() { major = major, minor = minor, patch = patch }.Should().BeEquivalentTo(pfs.GetLinkedVersion());
    }

    [Test]
    public void DirSeparator()
    {
      using var pfs = new PhysFS("");
      pfs.GetDirSeparator().Should().NotBeNullOrEmpty();
    }

    [Test]
    public void PermitSymbolicLinks()
    {
      using var pfs = new PhysFS("");
      pfs.SymbolicLinksPermitted().Should().BeFalse();
      pfs.PermitSymbolicLinks(true);
      pfs.SymbolicLinksPermitted().Should().BeTrue();
      pfs.PermitSymbolicLinks(false);
      pfs.SymbolicLinksPermitted().Should().BeFalse();
    }

    [Test]
    public void Mounting()
    {
      using var pfs = new PhysFS("");
      pfs.GetSearchPath().Should().BeEmpty();

      pfs.Mount("./", "/", false);

      pfs.GetSearchPath().Should().BeEquivalentTo(new string[] { "./" });
      pfs.GetMountPoint("./").Should().Be("/");
      pfs.IsDirectory("/").Should().BeTrue();

      pfs.Mount("../", "foo", true);
      pfs.GetSearchPath().Should().BeEquivalentTo(new string[] { "./", "../" });
      pfs.GetMountPoint("../").Should().Be("foo/");
      pfs.IsDirectory("/foo").Should().BeTrue();

      pfs.Mount("../../", "bar", false);
      pfs.GetSearchPath().Should().BeEquivalentTo(new string[] { "../../", "./", "../" });
      pfs.GetMountPoint("../../").Should().Be("bar/");
      pfs.IsDirectory("/bar").Should().BeTrue();

      pfs.UnMount("../");
      pfs.GetSearchPath().Should().BeEquivalentTo(new string[] { "../../", "./" });
    }

    [Test]
    public void FileEnumeration()
    {
      using var pfs = new PhysFS("");
      pfs.Mount("./", "/", false);

      var effectiveFiles = Directory.GetFiles("./").Select(Path.GetFileName).ToArray();
      Array.Sort(effectiveFiles);
      var enumeratedFiles = pfs.EnumerateFiles("/").ToArray();
      Array.Sort(enumeratedFiles);

      enumeratedFiles.Should().BeEquivalentTo(effectiveFiles);
    }

    [Test]
    public void DriveEnumeration()
    {
      using var pfs = new PhysFS("");
      var effectiveCdDrives = DriveInfo.GetDrives()
        .Where(x => x.DriveType == DriveType.CDRom)
        .Select(x => x.RootDirectory.FullName)
        .ToArray();

      var enumeratedCdDrives = pfs.GetCdRomDirs();

      Array.Sort(effectiveCdDrives);
      Array.Sort(enumeratedCdDrives);

      enumeratedCdDrives.Should().BeEquivalentTo(effectiveCdDrives);
    }

    [Test]
    public void UserDirectory()
    {
      using var pfs = new PhysFS("");
      var userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
      var pfsUserDirectory = pfs.GetUserDir();
      Path.GetPathRoot(pfsUserDirectory).Should().Be(Path.GetPathRoot(userDirectory));
    }

    [Test]
    public void DirectoryManipulation()
    {
      using var pfs = new PhysFS("");
      pfs.SetWriteDir("./");
      pfs.GetWriteDir().Should().Be("./");

      pfs.CreateDirectory("hello");
      Directory.Exists("./hello").Should().BeTrue();

      pfs.Delete("hello");
      Directory.Exists("./hello").Should().BeFalse();
    }

    [Test]
    public void FileManipulation()
    {
      using var pfs = new PhysFS("");
      pfs.SetWriteDir("./");
      pfs.Mount("./", "/", true);

      using (var sw = new StreamWriter(pfs.OpenWrite("foo")))
      {
        sw.Write("hello, world! èòàùã こんにちは世界 你好世界");
      }

      Assert.True(File.Exists("./foo"));

      var fileContent = File.ReadAllText("./foo");
      using (var sr = new StreamReader(pfs.OpenRead("foo")))
      {
        sr.ReadToEnd().Should().BeEquivalentTo(fileContent);
      }

      using (var sw = new StreamWriter(pfs.OpenAppend("foo")))
      {
        sw.Write("foo");
      }
      File.ReadAllText("./foo").Should().BeEquivalentTo(fileContent + "foo");

      pfs.Delete("foo");
      Assert.False(File.Exists("./foo"));
    }
  }
}
