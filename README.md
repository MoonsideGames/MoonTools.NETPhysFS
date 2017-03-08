# SharpPhysFS
## PhysicsFS wrapper for .NET

This library is a wrapper around the [PhysFS library](https://icculus.org/physfs/) designed
to work with .NET languages. As such, it employs standard .NET behaviors such as *Exceptions*
and *IEnumerable*s to represent native objects. It also provides access to the underlying low-level
methods and a *Stream* subclass for easy use of the APIs.

The documentation for the methods is copied from the original doxygen and only slightly adapted.

## Platform support

The library is designed to work regardless of the underlying OS, so it should run on Windows, Linux
and OSX equally well. I haven't tested the OSX port though, and I only superficially tried it on Linux.

If anyone feels so inclined, he/she could contribute by testing it and reporting the results. This would
be greatly appreciated.

## Installation

You can use this library by compiling it as described in the [wiki](https://github.com/frabert/SharpPhysFS/wiki)
or by adding it as a reference using NuGet:

    Install-Package SharpPhysFS

## Support on Beerpay
Hey dude! Help me out for a couple of :beers:!

[![Beerpay](https://beerpay.io/frabert/SharpPhysFS/badge.svg?style=beer-square)](https://beerpay.io/frabert/SharpPhysFS)  [![Beerpay](https://beerpay.io/frabert/SharpPhysFS/make-wish.svg?style=flat-square)](https://beerpay.io/frabert/SharpPhysFS?focus=wish)