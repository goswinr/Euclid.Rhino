namespace Euclid

open System
open EuclidRhino
open Rhino
open Rhino.Geometry

/// If this module is opened then you can
/// convert Rhino points and vectors and Euclid points and vectors to each other
/// via new static and instance members on these types.
/// Call setupEuclidDebugFunctions() to set the functions on the modules Euclid.Debug3D and Euclid.Debug2D to draw in Rhino.
[<AutoOpen>]
module AutoOpenRhinoIntegration =


    // -----------------------------------------------------------------------
    // --------------------Extensions on Rhino types- ------------------------
    // -----------------------------------------------------------------------

    type Point3d with

        /// Convert a Rhino.Geometry.Point3d to a Euclid 2D point, ignoring the Z component.
        member inline v.Pt  = Pt(v.X, v.Y)

        /// Convert a Rhino.Geometry.Point3d to a Euclid 3D point.
        member inline v.Pnt = Pnt(v.X, v.Y, v.Z)

        /// Convert a Rhino.Geometry.Point3d to a Euclid 2D point, ignoring the Z component.
        static member inline toPt(p:Point3d) = Pt(p.X, p.Y)

        /// Convert a Rhino.Geometry.Point3d to a Euclid 3D point.
        static member inline toPnt(p:Point3d) = Pnt(p.X, p.Y, p.Z)


    type Point3f with

        /// Convert a Rhino.Geometry.Point3f to a Euclid 2D point, ignoring the Z component.
        member inline v.Pt  = Pt(float v.X, float v.Y)

        /// Convert a Rhino.Geometry.Point3f to a Euclid 3D point.
        member inline v.Pnt = Pnt(float v.X, float v.Y, float v.Z)

        /// Convert a Rhino.Geometry.Point3f to a Euclid 2D point, ignoring the Z component.
        static member inline toPt(p:Point3f) = Pt(float p.X, float p.Y)

        /// Convert a Rhino.Geometry.Point3f to a Euclid 3D point.
        static member inline toPnt(p:Point3f) = Pnt(float p.X, float p.Y, float p.Z)

    type Vector3d with

        /// Convert a Rhino.Geometry.Vector3d to a Euclid 2D vector, ignoring the Z component.
        member inline v.Vc = Vc( v.X,  v.Y)

        /// Convert a Rhino.Geometry.Vector3d to a Euclid 3D vector.
        member inline v.Vec = Vec( v.X,  v.Y,  v.Z)

        /// Convert a Rhino.Geometry.Vector3d to a Euclid 2D unitized vector, ignoring the Z component.
        member inline v.UnitVc = UnitVc.create( v.X, v.Y)

        /// Convert a Rhino.Geometry.Vector3d to a Euclid 3D unitized vector.
        member inline v.UnitVec = UnitVec.create( v.X,  v.Y, v.Z)

        /// Convert a Rhino.Geometry.Vector3d to a Euclid 2D vector, ignoring the Z component.
        static member inline toVc (v:Vector3d) = Vc( v.X,  v.Y)

        /// Convert a Rhino.Geometry.Vector3d to a Euclid 3D vector.
        static member inline toVec (v:Vector3d) = Vec( v.X,  v.Y,  v.Z)

    type Vector3f with
        /// Convert a Rhino.Geometry.Vector3f to a Euclid 2D vector, ignoring the Z component.
        member inline v.Vc = Vc(float v.X, float v.Y)

        /// Convert a Rhino.Geometry.Vector3f to a Euclid 3D vector.
        member inline v.Vec = Vec(float v.X, float v.Y, float v.Z)

        /// Convert a Rhino.Geometry.Vector3f to a Euclid 2D unitized vector, ignoring the Z component.
        member inline v.UnitVc = UnitVc.create(float v.X, float v.Y)

        /// Convert a Rhino.Geometry.Vector3f to a Euclid 3D unitized vector.
        member inline v.UnitVec = UnitVec.create(float v.X, float v.Y, float v.Z)

        /// Convert a Rhino.Geometry.Vector3f to a Euclid 2D vector, ignoring the Z component.
        static member inline toVc (v:Vector3f) = Vc(float v.X, float v.Y)

        /// Convert a Rhino.Geometry.Vector3f to a Euclid 3D vector.
        static member inline toVec (v:Vector3f) = Vec(float v.X, float v.Y, float v.Z)

    type Geometry.Line with

        /// Convert a Rhino.Geometry.Line to a Euclid 3D Line.
        member inline l.Line3D = Line3D(l.FromX,l.FromY,l.FromZ, l.ToX,l.ToY,l.ToZ)

        /// Convert a Rhino.Geometry.Line to a Euclid 3D line.
        static member inline toLine3D(l:Geometry.Line) = Line3D(l.FromX,l.FromY,l.FromZ, l.ToX,l.ToY,l.ToZ)

        /// Convert a Rhino.Geometry.Line to a Euclid 2D line. Ignoring the Z component.
        member inline l.Line2D = Line2D(l.FromX,l.FromY, l.ToX,l.ToY)

        /// Convert a Rhino.Geometry.Line to a Euclid 2D line. Ignoring the Z component.
        static member inline toLine2D(l:Geometry.Line) = Line2D(l.FromX,l.FromY, l.ToX,l.ToY)

    type Geometry.Plane with
        /// Convert a Rhino.Geometry.Plane to a Euclid parametrized plane.
        member p.PPlane = PPlane.createOriginXaxisYaxis (p.Origin.Pnt, p.XAxis.Vec, p.YAxis.Vec)

        /// Convert a Rhino.Geometry.Plane to a Euclid parametrized plane.
        static member toPPlane(p:Geometry.Plane) = PPlane.createOriginXaxisYaxis (p.Origin.Pnt, p.XAxis.Vec, p.YAxis.Vec)

    type Geometry.BoundingBox with

        /// Convert a Rhino.Geometry.BoundingBox to a Euclid bounding box.
        /// Reordering the bounding box minX, minY, minZ, maxX, maxY and maxZ if they are not in the increasing from min to max values.
        /// Rhino.Geometry.BoundingBoxes can be inverted. Euclid BBoxes not.
        member b.BBox = BBox.create(b.Min.Pnt, b.Max.Pnt)

        /// Convert a Rhino.Geometry.BoundingBox to a Euclid bounding box.
        /// Reordering the bounding box minX, minY, minZ, maxX, maxY and maxZ if they are not in the increasing from min to max values.
        /// Rhino.Geometry.BoundingBoxes can be inverted. Euclid BBoxes not.
        static member toBBox(b:Geometry.BoundingBox) = BBox.create(b.Min.Pnt, b.Max.Pnt)

    type Geometry.Box with

        /// Convert a Rhino.Geometry.Box to a Euclid box.
        member b.BoxEuclid = Euclid.Box.createFromPlane b.X.Length b.Y.Length b.Z.Length b.Plane.PPlane

        /// Convert a Rhino.Geometry.Box to a Euclid box.
        static member toBoxEuclid(b:Geometry.Box) = Euclid.Box.createFromPlane b.X.Length b.Y.Length b.Z.Length b.Plane.PPlane


    type Geometry.Polyline with

        /// Convert a Rhino.Geometry.Polyline to a Euclid 3D polyline.
        member p.Polyline3D = Polyline3D.create(Seq.map Point3d.toPnt p)

        /// Convert a Rhino.Geometry.Polyline to a Euclid 3D polyline.
        static member toPolyline3D(p:Geometry.Polyline) = Polyline3D.create(Seq.map Point3d.toPnt p)

        /// Convert a Rhino.Geometry.Polyline to a Euclid 2D polyline. Ignoring the Z component.
        member p.Polyline2D = Polyline2D.create(Seq.map Point3d.toPt p)

        /// Convert a Rhino.Geometry.Polyline to a Euclid 2D polyline. Ignoring the Z component.
        static member toPolyline2D(p:Geometry.Polyline) = Polyline2D.create(Seq.map Point3d.toPt p)


    // -----------------------------------------------------------------------
    // --------------------Extensions on Euclid types- ------------------------
    // -----------------------------------------------------------------------


    type Pt with

        /// Convert Euclid 2D point to Rhino Point3d with Z value 0.0.
        member inline p.RhPt  = Point3d(p.X, p.Y, 0.0)

        /// Convert Euclid 2D point to Rhino Point3d with given Z value.
        member inline p.RhPtZ(z)  = Point3d(p.X, p.Y, z)

        /// Convert Euclid 2D point to Rhino Point3d with Z value 0.0.
        static member inline toRhPt(p:Pt) = Point3d(p.X, p.Y, 0.0)

        /// Convert Euclid 2D point to Rhino Point3d with given Z value.
        static member inline toRhPtZ z (p:Pt) = Point3d(p.X, p.Y, z)

        /// Convert Rhino Point3d to Euclid 2D point, ignoring Z value.
        static member inline ofRhPt(p:Point3d) = Pt(p.X, p.Y)

        /// Draw the Euclid 2D point in Rhino on current layer.
        static member  draw (p:Pt) = Rs.AddPoint(p.X, p.Y, 0.0)

         /// Draw the Euclid 2D point as RhinoTextDot with given message.
        static member  drawDot msg (p:Pt)  = Rs.AddTextDot(msg, p.X, p.Y, 0.0)

        /// Draw the Euclid 2D point in Rhino on current layer.
        member p.Draw() = Rs.AddPoint(p.X, p.Y, 0.0) |> ignore

        /// Draw the Euclid 2D point in Rhino on given layer.
        /// The Layer will be created if it does not exist.
        member p.Draw(layer) = Rs.AddPoint(p.X, p.Y, 0.0) |> Rs.setLayer layer

        /// Draw the Euclid 2D point in Rhino on given layer with given name.
        /// The Layer will be created if it does not exist.
        member p.Draw(layer, name) = Rs.AddPoint(p.X, p.Y, 0.0) |> Rs.setLayerAndName layer name

        /// Draw the Euclid 2D point as RhinoTextDot with given message.
        member p.DrawDot(msg) = Rs.AddTextDot(msg, p.X, p.Y, 0.0) |> ignore

        /// Draw the Euclid 2D point as RhinoTextDot with given message and given layer.
        /// The Layer will be created if it does not exist.
        member p.DrawDot(msg, layer) = Rs.AddTextDot(msg, p.X, p.Y, 0.0) |> Rs.setLayer layer

    type Pnt with

        /// Convert Euclid 3D point to Rhino Point3d.
        member inline p.RhPt  = Point3d(p.X, p.Y, p.Z)

        /// Convert Euclid 3D point to Rhino Point3d.
        static member inline toRhPt(p:Pnt) = Point3d(p.X, p.Y, p.Z)

        /// Convert Rhino Point3d to Euclid 3D point.
        static member inline ofRhPt(p:Point3d) = Pnt(p.X, p.Y, p.Z)

        /// Draw the Euclid 3D point in Rhino on current layer.
        static member  draw (p:Pnt) = Rs.AddPoint(p.X, p.Y, p.Z)

         /// Draw the Euclid 3D point as RhinoTextDot with given message.
        static member  drawDot msg (p:Pnt)  = Rs.AddTextDot(msg, p.X, p.Y, p.Z)

        /// Draw the Euclid 3D point in Rhino on current layer.
        /// The Layer will be created if it does not exist.
        member p.Draw() = Rs.AddPoint(p.X, p.Y, p.Z) |> ignore

        /// Draw the Euclid 3D point in Rhino on given layer.
        /// The Layer will be created if it does not exist.
        member p.Draw(layer) = Rs.AddPoint(p.X, p.Y, p.Z) |> Rs.setLayer layer

        /// Draw the Euclid 3D point in Rhino on given layer with given name.
        /// The Layer will be created if it does not exist.
        member p.Draw(layer, name) = Rs.AddPoint(p.X, p.Y, p.Z) |> Rs.setLayerAndName layer name

        /// Draw the Euclid 3D point as RhinoTextDot with given message.
        member p.DrawDot(msg) = Rs.AddTextDot(msg, p.X, p.Y, p.Z) |> ignore

        /// Draw the Euclid 3D point as RhinoTextDot with given message and given layer.
        /// The Layer will be created if it does not exist.
        member p.DrawDot(msg, layer) = Rs.AddTextDot(msg, p.X, p.Y, p.Z) |> Rs.setLayer layer


    type Vc with

        /// Convert Euclid 2D vector to Rhino Vector3d with Z value 0.0.
        member inline v.RhVec = Vector3d(v.X, v.Y, 0.0)

        /// Convert Euclid 2D vector to Rhino Vector3d with Z value 0.0.
        static member inline toRhVec(v:Vc) = Vector3d(v.X, v.Y, 0.0)

        /// Convert Rhino Vector3d to Euclid 2D vector, ignoring Z value.
        static member inline ofRhVec(v:Vector3d) = Vc(v.X, v.Y)

        /// Draw the Euclid 2D vector in Rhino as line with Curve Arrow.
        /// Using given start point and scale.
        static member  draw (scale:float) (fromPt:Pt) (v:Vc) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt)

        /// Draw the Euclid 2D vector in Rhino as line with Curve Arrow.
        /// Using given start point, scale, layer and name.
        /// The Layer will be created if it does not exist.
        member v.Draw(fromPt:Pt, scale:float, layer,  name) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt) |> Rs.setLayerAndName layer name

        /// Draw the Euclid 2D vector in Rhino  as line with Curve Arrow.
        /// Using given start point, scale and layer.
        /// The Layer will be created if it does not exist.
        member v.Draw(fromPt:Pt, scale:float, layer) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt) |> Rs.setLayer layer

        /// Draw the Euclid 2D vector in Rhino as line with Curve Arrow.
        /// Using given start point and layer.
        /// The Layer will be created if it does not exist.
        member v.Draw(fromPt:Pt, layer) = Rs.DrawVector(v.RhVec , fromPt.RhPt) |> Rs.setLayer layer

        /// Draw the Euclid 2D vector in Rhino as line with Curve Arrow.
        /// Using  given start point and scale.
        member v.Draw(fromPt:Pt, scale:float) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt) |> ignore

    type UnitVc with

        /// Convert Euclid 2D unit vector to Rhino Vector3d with Z value 0.0.
        member inline v.RhVec = Vector3d(v.X, v.Y, 0.0)

        /// Convert Euclid 2D unit vector to Rhino Vector3d with Z value 0.0.
        static member inline toRhVec(v:UnitVc) = Vector3d(v.X, v.Y, 0.0)

        /// Unitize a Rhino Vector3d to Euclid 2D unit vector, ignoring Z value.
        static member inline ofRhVec(v:Vector3d) = UnitVc.create(v.X, v.Y)

        /// Draw the Euclid 2D unit vector in Rhino as line with Curve Arrow.
        /// Using given start point and scale.
        static member  draw (scale:float) (fromPt:Pt) (v:UnitVc) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt)

        /// Draw the Euclid 2D unit vector in Rhino as line with Curve Arrow.
        /// Using given start point, scale, layer and name.
        /// The Layer will be created if it does not exist.
        member v.Draw(fromPt:Pt, scale:float, layer,  name) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt) |> Rs.setLayerAndName layer name

        /// Draw the Euclid 2D unit vector in Rhino as line with Curve Arrow.
        /// Using given start point, scale and layer.
        /// The Layer will be created if it does not exist.
        member v.Draw(fromPt:Pt, scale:float, layer) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt) |> Rs.setLayer layer

        /// Draw the Euclid 2D unit vector in Rhino as line with Curve Arrow.
        /// Using given start point and layer.
        /// The Layer will be created if it does not exist.
        member v.Draw(fromPt:Pt, layer) = Rs.DrawVector(v.RhVec , fromPt.RhPt) |> Rs.setLayer layer

        /// Draw the Euclid 2D unit vector in Rhino as line with Curve Arrow.
        /// Using given start point and scale.
        member v.Draw(fromPt:Pt, scale:float) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt) |> ignore


    type Vec with

        /// Convert Euclid 3D vector to Rhino Vector3d.
        member inline v.RhVec = Vector3d(v.X, v.Y, v.Z)

        /// Convert Euclid 3D vector to Rhino Vector3d.
        static member inline toRhVec(v:Vec) = Vector3d(v.X, v.Y, v.Z)

        /// Convert Rhino Vector3d to Euclid 3D vector.
        static member inline ofRhVec(v:Vector3d) = Vec(v.X, v.Y, v.Z)

        /// Draw the Euclid 3D vector in Rhino as line with Curve Arrow.
        /// Using given start point and scale.
        static member draw (scale:float) (fromPt:Pt) (v:Vec) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt)

        /// Draw the Euclid 3D vector in Rhino with given start point, scale, layer and name.
        /// The Layer will be created if it does not exist.
        member v.Draw(fromPt:Pnt, scale:float, layer,  name) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt) |> Rs.setLayerAndName layer name

        /// Draw the Euclid 3D vector in Rhino as line with Curve Arrow.
        /// Using given start point, scale and layer.
        /// The Layer will be created if it does not exist.
        member v.Draw(fromPt:Pnt, scale:float, layer) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt) |> Rs.setLayer layer

        /// Draw the Euclid 3D vector in Rhino as line with Curve Arrow.
        /// Using given start point and layer.
        /// The Layer will be created if it does not exist.
        member v.Draw(fromPt:Pnt, layer) = Rs.DrawVector(v.RhVec , fromPt.RhPt) |> Rs.setLayer layer

        /// Draw the Euclid 3D vector in Rhino as line with Curve Arrow.
        /// Using given start point and scale.
        member v.Draw(fromPt:Pnt, scale:float) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt) |> ignore

    type UnitVec with

        /// Convert Euclid 3D unit vector to Rhino Vector3d.
        member inline v.RhVec = Vector3d(v.X, v.Y, v.Z)

        /// Convert Euclid 3D unit vector to Rhino Vector3d.
        static member inline toRhVec(v:UnitVec) = Vector3d(v.X, v.Y, v.Z)

        /// Unitize a Rhino Vector3d to Euclid 3D unit vector.
        static member inline ofRhVec(v:Vector3d) = UnitVec.create(v.X, v.Y, v.Z)

        /// Draw the Euclid 3D unit vector in Rhino as line with Curve Arrow.
        /// Using given start point and scale.
        static member draw (scale:float) (fromPt:Pt) (v:UnitVec) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt)

        /// Draw the Euclid 3D unit vector in Rhino as line with Curve Arrow.
        /// Using given start point, scale, layer and name.
        /// The Layer will be created if it does not exist.
        member v.Draw(fromPt:Pt, scale:float, layer,  name) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt) |> Rs.setLayerAndName layer name

        /// Draw the Euclid 3D unit vector in Rhino as line with Curve Arrow.
        /// Using given start point, scale and layer.
        /// The Layer will be created if it does not exist.
        member v.Draw(fromPt:Pt, scale:float, layer) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt) |> Rs.setLayer layer

        /// Draw the Euclid 3D unit vector in Rhino as line with Curve Arrow.
        /// Using given start point and layer.
        /// The Layer will be created if it does not exist.
        member v.Draw(fromPt:Pt, layer) = Rs.DrawVector(v.RhVec , fromPt.RhPt) |> Rs.setLayer layer

        /// Draw the Euclid 3D unit vector in Rhino as line with Curve Arrow.
        /// Using given start point and scale.
        member v.Draw(fromPt:Pt, scale:float) = Rs.DrawVector(v.RhVec * scale, fromPt.RhPt) |> ignore

    type Euclid.Line3D with

        /// Convert Euclid 3D Line to Rhino Line.
        member inline l.RhLine  = Geometry.Line(l.FromX,l.FromY,l.FromZ, l.ToX,l.ToY,l.ToZ)

        /// Convert Euclid 3D Line to Rhino Line.
        static member inline toRhLine(l:Line3D) = Geometry.Line(l.FromX,l.FromY,l.FromZ, l.ToX,l.ToY,l.ToZ)

        /// Convert Rhino Line to Euclid 3D Line.
        static member inline ofRhLine(l:Geometry.Line) = Line3D(l.FromX,l.FromY,l.FromZ, l.ToX,l.ToY,l.ToZ)

        /// Draw the Euclid 3D Line in Rhino on current layer.
        static member draw(l:Line3D) = Rs.AddLine(l.FromX,l.FromY,l.FromZ, l.ToX,l.ToY,l.ToZ)

        /// Draw the Euclid 3D Line in Rhino with given layer and name.
        /// The Layer will be created if it does not exist.
        member l.Draw(layer, name) = Rs.AddLine(l.FromX,l.FromY,l.FromZ, l.ToX,l.ToY,l.ToZ) |> Rs.setLayerAndName layer name

        /// Draw the Euclid 3D Line in Rhino on given layer.
        /// The Layer will be created if it does not exist.
        member l.Draw(layer) = Rs.AddLine(l.FromX,l.FromY,l.FromZ, l.ToX,l.ToY,l.ToZ) |> Rs.setLayer layer

        /// Draw the Euclid 3D Line in Rhino on current layer.
        member l.Draw() = Rs.AddLine(l.FromX,l.FromY,l.FromZ, l.ToX,l.ToY,l.ToZ) |> ignore

    type Euclid.Line2D with

        /// Convert Euclid 2D Line to Rhino Line. Using 0.0 as Z value.
        member inline l.RhLine  = Line(l.FromX,l.FromY,0,l.ToX,l.ToY,0)

        /// Convert Euclid 2D Line to Rhino Line. Using 0.0 as Z value.
        static member inline toRhLine(l:Line2D) = Line(l.FromX,l.FromY,0, l.ToX,l.ToY,0)

        /// Convert Rhino Line to Euclid 2D Line. Ignoring Z value.
        static member inline ofRhLine(l:Line) = Line2D(l.FromX,l.FromY, l.ToX,l.ToY)

        /// Draw the Euclid 2D Line in Rhino on current layer.
        static member draw(l:Line2D) = Rs.AddLine(l.FromX, l.FromY, 0, l.ToX, l.ToY, 0)

        /// Draw the Euclid 2D Line at given Z level in Rhino on current layer.
        static member drawWithZ z (l:Line2D) = Rs.AddLine(l.FromX, l.FromY, z, l.ToX, l.ToY, z)

        /// Draw the Euclid 2D Line in Rhino with given layer and name.
        /// The Layer will be created if it does not exist.
        member l.Draw(layer, name) = Rs.AddLine(l.From.RhPt, l.To.RhPt) |> Rs.setLayerAndName layer name

        /// Draw the Euclid 2D Line in Rhino on given layer.
        /// The Layer will be created if it does not exist.
        member l.Draw(layer) = Rs.AddLine(l.From.RhPt, l.To.RhPt) |> Rs.setLayer layer

        /// Draw the Euclid 2D Line in Rhino on current layer.
        member l.Draw() = Rs.AddLine(l.From.RhPt, l.To.RhPt) |> ignore



    type PPlane with

        /// Convert Euclid PPlane to Rhino Plane.
        member p.RhPlane = Geometry.Plane(p.Origin.RhPt, p.Xaxis.RhVec, p.Yaxis.RhVec)

        /// Convert Euclid PPlane to Rhino Plane.
        static member inline toRhPlane(p:PPlane) = Geometry.Plane(p.Origin.RhPt, p.Xaxis.RhVec, p.Yaxis.RhVec)

        /// Convert Rhino Plane to Euclid PPlane.
        static member inline ofRhPlane(p:Geometry.Plane) = PPlane.createOriginXaxisYaxis (p.Origin.Pnt,  p.XAxis.Vec, p.YAxis.Vec)

        ///<summary>Draws the axes of the PPlane and adds TextDots to label them.</summary>
        ///<param name="axLength">The length  of the drawn lines</param>
        ///<param name="plane">The plane to draw.</param>
        ///<returns>A Rarr of 6 Guids ( 3 Lines , 3 TextDots) added to Rhino</returns>
        static member draw (axLength) (plane:PPlane)= Rs.DrawPlane(plane.RhPlane, axLength)

        ///<summary>Draws the axes of the PPlane and adds TextDots to label them.</summary>
        ///<param name="axLength">The length  of the drawn lines</param>
        ///<param name="suffixInDot">Text to add to x TextDot label do of x axis. And y and z too.</param>
        ///<param name="layer">Layer to draw plane on. The Layer will be created if it does not exist.</param>
        ///<returns>unit</returns>
        member p.Draw(axLength, suffixInDot, layer ) = Rs.DrawPlane(p.RhPlane, axLength, suffixInDot, layer) |> ignore

        ///<summary>Draws the axes of the PPlane and adds TextDots to label them.</summary>
        ///<param name="axLength">The length  of the drawn lines</param>
        ///<param name="suffixInDot">Text to add to x TextDot label do of x axis. And y and z too.</param>
        ///<returns>unit</returns>
        member p.Draw(axLength, suffixInDot ) = Rs.DrawPlane(p.RhPlane, axLength, suffixInDot) |> ignore

        ///<summary>Draws the axes of the PPlane and adds TextDots to label them.</summary>
        ///<param name="axLength">The length  of the drawn lines</param>
        ///<returns>unit</returns>
        member p.Draw(axLength) = Rs.DrawPlane(p.RhPlane, axLength) |> ignore

        ///Draws the axes of the PPlane and adds TextDots to label them.
        member p.Draw() = Rs.DrawPlane(p.RhPlane) |> ignore

    type BRect with

        /// Draw the Euclid 2D Bounding Rectangle in Rhino as Polyline on current layer.
        static member drawPolyLine (rect:BRect) = Rs.AddPolyline(rect.RhPolyline)

        /// Convert Euclid 2D Bounding Rectangle to a closed Rhino Polyline.
        member r.RhPolyline =  new Polyline(r.PointsLooped |> Seq.map ( fun p -> p.RhPt) )


    type BBox with

        /// Draw the Euclid 3D Bounding Box in Rhino as Polyline with 10 vertices.
        /// Going from Point 0 to 1, 2, 3, again 0, 4, 5, 6, 7 and again 4.
        ///
        ///   Z-Axis       Y-Axis
        ///   ^           /
        ///   |   7      /        6 MaxPt
        ///   |   +---------------+
        ///   |  /|    /         /|
        ///   | / |   /         / |
        /// 4 |/  |  /       5 /  |
        ///   +---------------+   |
        ///   |   |/          |   |
        ///   |   +-----------|---+
        ///   |  / 3          |  / 2
        ///   | /             | /
        ///   |/              |/
        ///   +---------------+----> X-Axis
        ///   0 MinPt         1
        static member drawPolyLine (bbox:BBox) =
            let pts = seq{ bbox.Pt0; bbox.Pt1; bbox.Pt2; bbox.Pt3; bbox.Pt0; bbox.Pt4; bbox.Pt5; bbox.Pt6; bbox.Pt7; bbox.Pt4 }
            Rs.AddPolyline(bbox.Points |> Seq.map ( fun p -> p.RhPt))

        /// Draw the Euclid 3D Bounding Box  in Rhino as Mesh on current layer.
        static member drawMesh (bbox:BBox) =
            let b: Geometry.BoundingBox = bbox.RhBBox
            let m = Mesh.CreateFromBox(b, 1, 1, 1)
            let g = State.Doc.Objects.AddMesh(m)
            if g = Guid.Empty then failwithf "Euclid.Rhino.BBox.drawMesh failed for %O" bbox
            g

        /// Convert Euclid the 3D Bounding Box to a Rhino bounding box .
        member b.RhBBox = Geometry.BoundingBox(b.MinPnt.RhPt, b.MaxPnt.RhPt)

    type Euclid.Box with

        /// Convert Euclid 3D Box to a Rhino box .
        member b.RhBox =
            let pl = b.Plane.RhPlane
            let x = Geometry.Interval(0, b.SizeX)
            let y = Geometry.Interval(0, b.SizeY)
            let z = Geometry.Interval(0, b.SizeZ)
            Geometry.Box(pl,x,y,z)

        /// Draw the Euclid 3D  Box in Rhino as Polyline with 10 vertices.
        /// Going from Point 0 to 1, 2, 3, again 0, 4, 5, 6, 7 and again 4.
        ///
        ///   local        local
        ///   Z-Axis       Y-Axis
        ///   ^           /
        ///   |   7      /        6
        ///   |   +---------------+
        ///   |  /|    /         /|
        ///   | / |   /         / |
        /// 4 |/  |  /       5 /  |
        ///   +---------------+   |
        ///   |   |/          |   |
        ///   |   +-----------|---+
        ///   |  / 3          |  / 2
        ///   | /             | /
        ///   |/              |/     local
        ///   +---------------+----> X-Axis
        ///   0               1
        static member drawPolyLine (box:Euclid.Box) =
            let pts = seq{ box.Pt0; box.Pt1; box.Pt2; box.Pt3; box.Pt0; box.Pt4; box.Pt5; box.Pt6; box.Pt7; box.Pt4 }
            Rs.AddPolyline(box.Points |> Seq.map ( fun p -> p.RhPt))

        /// Draw the Euclid 3D Box in Rhino as Mesh on current layer.
        static member drawMesh (box:Euclid.Box) =
            let b: Geometry.Box = box.RhBox
            let m = Mesh.CreateFromBox(b, 1, 1, 1)
            let g = State.Doc.Objects.AddMesh(m)
            if g = Guid.Empty then failwithf "Euclid.Rhino.Box.drawMesh failed for %O" box
            g


    type Polyline3D with

        /// Convert Euclid 3D Polyline to a Rhino Polyline .
        member p.RhPolyline =
            let pts = p.Points |> Seq.map Pnt.toRhPt
            new Geometry.Polyline(pts)

        /// Convert Euclid 3D Polyline to a Rhino Polyline .
        member p.RhPolylineCurve =
            let pts = p.Points |> Seq.map Pnt.toRhPt
            new Geometry.PolylineCurve(pts)


        /// Draw Euclid 3D Polyline in  Rhino.
        static member draw (p:Polyline3D) = Rs.AddPolyline p.RhPolyline

        /// Convert Euclid 3D Polyline to a Rhino Polyline.
        static member inline toRhPolyline (p:Polyline3D) = p.RhPolyline

        /// Convert Euclid 3D Polyline to a Rhino PolylineCurve.
        static member inline toRhPolylineCurve (p:Polyline3D) = p.RhPolylineCurve

        /// Convert Rhino Polyline to Euclid 3D Polyline.
        static member inline ofRhPolyline (p:Geometry.Polyline) = p|> Seq.map Pnt.ofRhPt |> Polyline3D.create


    type Polyline2D with

        /// Convert Euclid 2D Polyline to a Rhino Polyline in World XY Plane.
        member pl.RhPolyline =
            let pts = pl.Points |> Seq.map Pt.toRhPt
            new Geometry.Polyline(pts)

        /// Convert Euclid 2D Polyline to a Rhino PolylineCurve in World XY Plane.
        member pl.RhPolylineCurve =
            let pts = pl.Points |> Seq.map Pt.toRhPt
            new Geometry.PolylineCurve(pts)

        /// Convert Euclid 2D Polyline to a Rhino Polyline at Given Z level.
        member pl.RhPolylineZ(z) =
            let pts = pl.Points |> Seq.map (Pt.toRhPtZ z)
            new Geometry.Polyline(pts)

        /// Draw Euclid 2D Polyline in Rhino in World XY Plane.
        static member draw (p:Polyline2D) = Rs.AddPolyline p.RhPolyline

        /// Draw Euclid 2D Polyline in Rhino at Given Z level.
        static member drawZ  z (p:Polyline2D) = Rs.AddPolyline (p.RhPolylineZ z)

        /// Convert Euclid 2D Polyline to a Rhino Polyline in World XY Plane.
        static member inline toRhPolyline (p:Polyline2D) = p.RhPolyline

        /// Convert Euclid 2D Polyline to a Rhino PolylineCurve in World XY Plane.
        static member inline toRhPolylineCurve (p:Polyline2D) = p.RhPolylineCurve

        /// Convert Euclid 2D Polyline to a Rhino Polyline at Given Z level.
        static member inline toRhPolylineZ z (p:Polyline2D) = p.RhPolylineZ z

        /// Convert Rhino Polyline to Euclid 2D Polyline  ignoring Z values.
        static member inline ofRhPolyline (p:Geometry.Polyline) = p|> Seq.map Pt.ofRhPt |> Polyline2D.create

    type Rect3D with

        /// Convert Euclid 3D Rectangle to a closed Rhino Polyline.
        member r.RhPolyline =
            let pts = r.PointsLooped |> Seq.map Pnt.toRhPt
            new Geometry.Polyline(pts)

        /// Convert Euclid 2D Rectangle to a closed Rhino PolylineCurve.
        member r.RhPolylineCurve =
            let pts = r.PointsLooped |> Seq.map Pnt.toRhPt
            new Geometry.PolylineCurve(pts)

        /// Convert Euclid 2D Rectangle to a Rhino Surface.
        member r.RhSurface =
            Geometry.NurbsSurface.CreateFromCorners(r.Pt0.RhPt, r.Pt1.RhPt, r.Pt2.RhPt, r.Pt3.RhPt)

    type Rect2D with

        /// Convert Euclid 2D Rectangle to a closed Rhino Polyline in World XY Plane.
        member r.RhPolyline =
            let pts = r.PointsLooped |> Seq.map Pt.toRhPt
            new Geometry.Polyline(pts)

        /// Convert Euclid 2D Rectangle to a closed Rhino PolylineCurve in World XY Plane.
        member r.RhPolylineCurve =
            let pts = r.PointsLooped |> Seq.map Pt.toRhPt
            new Geometry.PolylineCurve(pts)

        /// Convert Euclid 2D Rectangle to a Rhino Surface in World XY Plane.
        member r.RhSurface =
            Geometry.NurbsSurface.CreateFromCorners(r.Pt0.RhPt, r.Pt1.RhPt, r.Pt2.RhPt, r.Pt3.RhPt)


/// A module for drawing Euclid 2D geometry in Rhino
module Debug2D =

    let drawDot        (msg:string, pt:Pt)  = Rs.AddTextDot(msg, pt.X, pt.Y, 0.0)                 |> Rs.setLayer "Euclid.Debug2D::drawDot"
    let drawPt         (pt:Pt)              = Rs.AddPoint(pt.X, pt.Y, 0.0)                        |> Rs.setLayer "Euclid.Debug2D::drawPt"
    let drawLine       (ln:Line2D)          = Rs.AddLine2D(ln.FromX, ln.FromY, ln.ToX, ln.ToY)    |> Rs.setLayer "Euclid.Debug2D::drawLine"
    let drawLineFromTo (a:Pt, b:Pt)         = Rs.AddLine2D(a.X, a.Y, b.X, b.Y )                   |> Rs.setLayer "Euclid.Debug2D::drawLine"
    let drawPolyLine   (ps:seq<Pt>)         = Rs.AddPolyline(ps |> Seq.map Pt.toRhPt)             |> Rs.setLayer "Euclid.Debug2D::drawPolyLine"


    let drawDotLayer         (pt:Pt, msg:string, layer:string) = Rs.AddTextDot(msg, pt.X, pt.Y, 0.0)                |> Rs.setLayer layer
    let drawPtLayer          (pt:Pt, layer:string)             = Rs.AddPoint(pt.X, pt.Y, 0.0)                       |> Rs.setLayer layer
    let drawLineLayer        (ln:Line2D, layer:string)         = Rs.AddLine2D(ln.FromX, ln.FromY, ln.ToX, ln.ToY)   |> Rs.setLayer layer
    let drawPolyLineLayer    (ps:seq<Pt>, layer:string)        = Rs.AddPolyline(ps |> Seq.map Pt.toRhPt )           |> Rs.setLayer layer
    let drawLineFromToLayer  (a:Pt, b:Pt, layer:string)        = Rs.AddLine2D(a.X, a.Y, b.X, b.Y )                  |> Rs.setLayer layer

/// A module for drawing Euclid 3D geometry in Rhino
module Debug3D =

    let drawDot          (msg:string, pt:Pnt) = Rs.AddTextDot(msg, pt.X, pt.Y, pt.Z)                               |> Rs.setLayer "Euclid.Debug3D::drawDot"
    let drawPt           (pt:Pnt)             = Rs.AddPoint(pt.X, pt.Y, pt.Z)                                      |> Rs.setLayer "Euclid.Debug3D::drawPt"
    let drawLine         (ln:Line3D)          = Rs.AddLine(ln.FromX, ln.FromY, ln.FromZ, ln.ToX, ln.ToY, ln.ToZ)   |> Rs.setLayer "Euclid.Debug3D::drawLine"
    let drawLineFromTo   (a:Pnt, b:Pnt)       = Rs.AddLine(a.X, a.Y, a.Z, b.X, b.Y, b.Z )                          |> Rs.setLayer "Euclid.Debug3D::drawLine"
    let drawPolyLine     (ps:seq<Pnt>)        = Rs.AddPolyline(ps |> Seq.map Pnt.toRhPt)                           |> Rs.setLayer "Euclid.Debug3D::drawPolyLine"

    let drawDotLayer        (pt:Pnt, msg:string, layer:string)  = Rs.AddTextDot(msg, pt.X, pt.Y, pt.Z)                              |> Rs.setLayer layer
    let drawPtLayer         (pt:Pnt, layer:string)              = Rs.AddPoint(pt.X, pt.Y, pt.Z)                                     |> Rs.setLayer layer
    let drawLineLayer       (ln:Line3D, layer:string)           = Rs.AddLine(ln.FromX, ln.FromY, ln.FromZ, ln.ToX, ln.ToY, ln.ToZ)  |> Rs.setLayer layer
    let drawLineFromToLayer (a:Pnt, b:Pnt, layer:string)        = Rs.AddLine(a.X, a.Y, a.Z, b.X, b.Y, b.Z )                         |> Rs.setLayer layer
    let drawPolyLineLayer   (ps:seq<Pnt>, layer:string)         = Rs.AddPolyline(ps |> Seq.map Pnt.toRhPt)                          |> Rs.setLayer layer

