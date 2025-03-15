
![Logo](https://raw.githubusercontent.com/goswinr/Euclid.Rhino/main/Docs/img/logo128.png)

# Euclid.Rhino


[![Euclid.Rhino on nuget.org](https://img.shields.io/nuget/v/Euclid.Rhino)](https://www.nuget.org/packages/Euclid.Rhino/)
[![Build Status](https://github.com/goswinr/Euclid.Rhino/actions/workflows/build.yml/badge.svg)](https://github.com/goswinr/Euclid.Rhino/actions/workflows/build.yml)
[![Docs Build Status](https://github.com/goswinr/Euclid.Rhino/actions/workflows/docs.yml/badge.svg)](https://github.com/goswinr/Euclid.Rhino/actions/workflows/docs.yml)
[![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2Fgoswinr%2FEuclid&count_bg=%2379C83D&title_bg=%23555555&icon=github.svg&icon_color=%23E7E7E7&title=hits&edge_flat=false)](https://hits.seeyoufarm.com)
[![license](https://img.shields.io/github/license/goswinr/Euclid.Rhino)](LICENSE.md)
![code size](https://img.shields.io/github/languages/code-size/goswinr/Euclid.Rhino.svg)


This package is for using the [Euclid](https://github.com/goswinr/Euclid) geometry library in [Rhino3D](https://www.rhino3d.com/).

It provides extension members for drawing Euclid geometry in Rhino.

And for converting Rhino geometry to Euclid geometry. And vice versa.

The version number of `Euclid.Rhino` is aligned with the referenced version of `Euclid`.

### Documentation

Just open the Euclid namespace.
`open Euclid`
This will auto-open the module  `AutoOpenRhinoIntegration`
You will get access to all the extension methods for Rhino integration.

Call `setupEuclidDebugFunctions()` once to replace the default debug drawing functions with Rhino specific ones.
The library Euclid has no reference to Rhino.
However it has these mutable functions to display debug information in case of errors.
By default these functions do nothing.
After calling `setupEuclidDebugFunctions()` they get replaced with implementations that use Rhino for drawing.


For now see [full API documentation on fuget.org](https://www.fuget.org/packages/Euclid.Rhino)
or look at [main.fs](https://github.com/goswinr/Euclid.Rhino/blob/main/main.fs) for the source code.

## Full API Documentation

[goswinr.github.io/Euclid.Rhino](https://goswinr.github.io/Euclid.Rhino)

## Changelog
see [CHANGELOG.md](https://github.com/goswinr/Euclid.Rhino/blob/main/CHANGELOG.md)

## Build from source
Just run `dotnet build` in the root directory.


## License
[MIT](https://github.com/goswinr/Euclid.Rhino/blob/main/LICENSE.md)



