# MoonTools.NETPhysFS

## PhysFS wrapper for .NET Standard

This library is a .NET Standard wrapper around the cross-platform IO libary [PhysFS](https://icculus.org/physfs/).
It provides *IEnumerable* iterators to avoid creating garbage, and a *Stream* subclass for easy usage.

## Installation

You can use this library by adding it as a submodule and then referencing it in your .csproj file.

```sh
  git submodule add
```

You must include a compiled binary of PhysFS for your platform for this to work properly.

## Example

```csharp
using var pfs = new PhysFS(""); // automatic dispose pattern
using (var reader = new StreamReader(pfs.OpenRead("/helloworld.txt")))
{
  var contents = reader.ReadToEnd();
}
```
