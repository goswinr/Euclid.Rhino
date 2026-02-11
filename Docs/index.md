
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

## Documentation

Just open the Euclid namespace.<br>
`open Euclid`<br>
after referencing the package `Euclid.Rhino`.<br>
This will auto-open the module `AutoOpenRhinoIntegration`<br>
You will get access to all the extension methods for Rhino integration.

For the main functions look at [main.fs](https://github.com/goswinr/Euclid.Rhino/blob/main/main.fs) .<br>
Or go to [goswinr.github.io/Euclid.Rhino](https://goswinr.github.io/Euclid.Rhino/reference/euclid-autoopenrhinointegration.html) for the full documentation.

## Sample Code

### Type Conversions

```fsharp
open Euclid
open Rhino.Geometry

// Rhino → Euclid (via extension properties)
let rhinoPoint = Point3d(1.0, 2.0, 3.0)
let euclidPnt : Pnt = rhinoPoint.Pnt       // 3D point
let euclidPt  : Pt  = rhinoPoint.Pt        // 2D point (Z ignored)

let rhinoVec = Vector3d(1.0, 0.0, 0.0)
let euclidVec : Vec = rhinoVec.Vec         // 3D vector
let euclidVc  : Vc  = rhinoVec.Vc          // 2D vector (Z ignored)

// Euclid → Rhino (via .Rh* properties)
let pnt = Pnt(1.0, 2.0, 3.0)
let rhPoint : Point3d = pnt.RhPt

let vec = Vec(1.0, 0.0, 0.0)
let rhVec : Vector3d = vec.RhVec

let line = Line3D(Pnt.Origin, Pnt(10.0, 5.0, 0.0))
let rhLine : Line = line.RhLine
```

### Drawing in Rhino

```fsharp
open Euclid

// Draw on current layer
let pt = Pnt(5.0, 10.0, 0.0)
pt.Draw()

// Draw on a specific layer (auto-created if missing)
pt.Draw("MyLayer")

// Draw with layer and object name
pt.Draw("MyLayer", "object name xyz")

// Draw with labeled dot
pt.DrawDot("Start")

// Functional style for piping
let points = [Pnt(0,0,0); Pnt(1,0,0); Pnt(1,1,0)]
points |> List.iter Pnt.draw

// Draw lines and polylines
let ln = Line3D(Pnt.Origin, Pnt(10.0, 10.0, 0.0))
ln.Draw("layer name")

let poly = Polyline3D.create points
poly.Draw("layer name", "Object Name")
```

## Changelog
see [CHANGELOG.md](https://github.com/goswinr/Euclid.Rhino/blob/main/CHANGELOG.md)

## Build from source
Just run `dotnet build` in the root directory.


## Use of AI and LLMs
All core function are are written by hand to ensure performance and correctness.<br>
However, AI tools have been used for code review, typo and grammar checking in documentation<br>
and to generate not all but many of the tests.

## License
[MIT](https://github.com/goswinr/Euclid.Rhino/blob/main/LICENSE.md)



