
![Logo](https://raw.githubusercontent.com/goswinr/Euclid.Rhino/main/Docs/img/logo128.png)

# Euclid.Rhino


[![Euclid.Rhino on nuget.org](https://img.shields.io/nuget/v/Euclid.Rhino)](https://www.nuget.org/packages/Euclid.Rhino/)
[![Build Status](https://github.com/goswinr/Euclid.Rhino/actions/workflows/build.yml/badge.svg)](https://github.com/goswinr/Euclid.Rhino/actions/workflows/build.yml)
[![Docs Build Status](https://github.com/goswinr/Euclid.Rhino/actions/workflows/docs.yml/badge.svg)](https://github.com/goswinr/Euclid.Rhino/actions/workflows/docs.yml)

[![license](https://img.shields.io/github/license/goswinr/Euclid.Rhino)](LICENSE.md)
![code size](https://img.shields.io/github/languages/code-size/goswinr/Euclid.Rhino.svg)


This package is for using the [Euclid](https://github.com/goswinr/Euclid) geometry library in [Rhino3D](https://www.rhino3d.com/).<br>
It provides extension members for drawing Euclid geometry in Rhino.<br>
And for converting Rhino geometry to Euclid geometry. And vice versa.<br>
The version number of `Euclid.Rhino` is aligned with the referenced version of `Euclid`.

### Documentation

Just open the Euclid namespace.<br>
`open Euclid`<br>
This will auto-open the module `AutoOpenRhinoIntegration`<br>
You will get access to all the extension methods for Rhino integration.

Call `setupEuclidDebugFunctions()` once to replace the default debug drawing functions with Rhino-specific ones.<br>
The library Euclid has no reference to Rhino.<br>
However, it has these mutable functions to display debug information in case of errors.<br>
By default these functions do nothing.<br>
After calling `setupEuclidDebugFunctions()` they get replaced with implementations that use Rhino for drawing.


For the main functions look at [main.fs](https://github.com/goswinr/Euclid.Rhino/blob/main/main.fs) .<br>
Or go to [goswinr.github.io/Euclid.Rhino](https://goswinr.github.io/Euclid.Rhino/reference/euclid-autoopenrhinointegration.html) for the full documentation.

## Changelog
see [CHANGELOG.md](https://github.com/goswinr/Euclid.Rhino/blob/main/CHANGELOG.md)

## Build from source
Just run `dotnet build` in the root directory.


## License
[MIT](https://github.com/goswinr/Euclid.Rhino/blob/main/LICENSE.md)



