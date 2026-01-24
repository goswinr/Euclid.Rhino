# Euclid.Rhino - AI Coding Instructions

## Project Overview

**Euclid.Rhino** is an F# library that bridges the [Euclid](https://github.com/goswinr/Euclid) geometry library with [Rhino3D](https://www.rhino3d.com/). It provides:
- Bidirectional type conversions between Euclid and RhinoCommon geometry types
- Extension members for drawing Euclid geometry in Rhino
- Debug visualization modules (`Debug2D`, `Debug3D`)

## Build & Targets

```bash
dotnet build  # Multi-targets: net48 (Rhino 7) and net8.0 (Rhino 8+)
```

Versioning is automated via `Ionide.KeepAChangelog.Tasks` from [CHANGELOG.md](CHANGELOG.md). The version aligns with the referenced `Euclid` package.

## Architecture

### File Organization (compilation order matters in F#)
1. [Src/State.fs](Src/State.fs) - Internal singleton managing active `RhinoDoc`, UI thread sync
2. [Src/Util.fs](Src/Util.fs) - Internal helpers: `ColorUtil` (HSL colors for layers), `UtilLayer` (layer management)
3. [Src/Rs.fs](Src/Rs.fs) - Internal static class wrapping RhinoCommon object creation (`AddPoint`, `AddLine`, etc.)
4. [Src/main.fs](Src/main.fs) - **Public API**: `AutoOpenRhinoIntegration` module with type extensions

### Namespace Convention
- **Public types**: `namespace Euclid` with `[<AutoOpen>]` on `AutoOpenRhinoIntegration`
- **Internal helpers**: `namespace EuclidRhino` (not exposed to consumers)

## Code Patterns

### Type Extensions Pattern
All conversions use F# type extensions with consistent naming:
```fsharp
// Rhino → Euclid (on Rhino types)
type Point3d with
    member v.Pnt = Pnt(v.X, v.Y, v.Z)           // Instance property
    static member toPnt(p:Point3d) = ...        // Static function

// Euclid → Rhino (on Euclid types)  
type Pnt with
    member p.RhPt = Point3d(p.X, p.Y, p.Z)      // Instance: .RhPt, .RhVec, .RhLine, .RhPlane
    static member toRhPt(p:Pnt) = ...           // Static: toRhPt, ofRhPt
```

### Draw Methods Pattern
Every drawable type has multiple `Draw` overloads:
```fsharp
static member draw(x:Type)                    // Curried for piping: |> Type.draw
member x.Draw()                               // No layer (current layer)
member x.Draw(layer)                          // Specific layer (auto-created)
member x.Draw(layer, name)                    // Layer + object name
```

### Layer Handling
Layers are created automatically if missing via `UtilLayer.getOrCreateLayer`. Layer colors use a golden-ratio HSL algorithm for visual distinction (excludes yellow).

### UI Thread Safety
All Rhino document operations go through `State.DoSync()` which handles `RhinoApp.InvokeRequired` and Eto synchronization.

## Naming Conventions

| Euclid Type | Rhino Equivalent | Conversion Property |
|-------------|------------------|---------------------|
| `Pt`, `Pnt` | `Point3d` | `.RhPt` |
| `Vc`, `Vec` | `Vector3d` | `.RhVec` |
| `Line2D`, `Line3D` | `Line` | `.RhLine` |
| `PPlane` | `Plane` | `.RhPlane` |
| `Polyline2D/3D` | `Polyline` | `.RhPolyline` |

## Testing & Validation

- This library runs **inside Rhino's process** - manual testing in Rhino/Grasshopper required
- All core functions are hand-written for performance; AI-generated tests exist for some functionality
- XML documentation is generated (`GenerateDocumentationFile=true`)
