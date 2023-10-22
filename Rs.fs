namespace Euclid

// all modules copied over and adapted from from https://github.com/goswinr/Rhino.Scripting


open System
open Rhino
open Rhino.Geometry

/// OptionalAttribute for member parameters
type internal OPT = Runtime.InteropServices.OptionalAttribute

/// DefaultParameterValueAttribute for member parameters
type internal DEF =  Runtime.InteropServices.DefaultParameterValueAttribute

type internal Rs private () =
    
    static member setLayer (layer:string) (objectId:Guid) : unit = 
        let obj = State.Doc.Objects.FindId(objectId) 
        if isNull obj then failwithf "Euclid.Rhino.Rs.setLayer: %O not found for layer '%s'"  objectId layer
        let layerIndex = UtilLayer.getOrCreateLayer(layer,UtilLayer.randomLayerColor, UtilLayer.ByParent, UtilLayer.ByParent, true).Index
            
        obj.Attributes.LayerIndex <- layerIndex
        if not <| obj.CommitChanges() then failwithf "Euclid.Rhino.Rs.setLayer: Setting it failed for layer '%s' on: %O " layer objectId
        State.Doc.Views.Redraw() 
        
    static member setLayerAndName (layer:string) (name:string) (objectId:Guid) : unit = 
        let obj = State.Doc.Objects.FindId(objectId) 
        if isNull obj then failwithf "Euclid.Rhino.Rs.setLayerAndName: %O not found for layer '%s'"  objectId layer
        let layerIndex = UtilLayer.getOrCreateLayer(layer,UtilLayer.randomLayerColor, UtilLayer.ByParent, UtilLayer.ByParent, true).Index
            
        obj.Attributes.LayerIndex <- layerIndex
        if UtilLayer.isAcceptableStringId(name,true) then 
            obj.Attributes.Name <- name
        else
            failwithf "Euclid.Rhino.Rs.setLayerAndName: Setting it failed for name '%s' on layer '%s' : %O " name layer objectId
        if not <| obj.CommitChanges() then failwithf "Euclid.Rhino.Rs.setLayerAndName: Setting it failed for layer '%s' on: %O " layer objectId
        State.Doc.Views.Redraw() 

    static member setName  (name:string) (objectId:Guid) : unit = 
        let obj = State.Doc.Objects.FindId(objectId) 
        if isNull obj then failwithf "Euclid.Rhino.Rs.setName: %O not found for name '%s'"  objectId name          
        
        if UtilLayer.isAcceptableStringId(name,true) then 
            obj.Attributes.Name <- name
        else
            failwithf "Euclid.Rhino.Rs.setName: Setting it failed for name '%s' on  : %O " name objectId
        if not <| obj.CommitChanges() then failwithf "Euclid.Rhino.Rs.setName: Setting it failed for name '%s' on: %O " name objectId
        State.Doc.Views.Redraw() 

    static member AddPoint(x,y,z) : Guid = 
        let g = State.Doc.Objects.AddPoint(x,y,z)
        if g = Guid.Empty then failwithf "Euclid.Rhino.Rs.AddPoint failed on x:%g, y:%g, z:%g" x y z
        State.Doc.Views.Redraw()
        g

    static member AddTextDot(t,x,y,z) : Guid = 
        let g = State.Doc.Objects.AddTextDot(t, Point3d(x,y,z))
        if g = Guid.Empty then failwithf "Euclid.Rhino.Rs.AddTextDot failed on x:%g, y:%g, z:%g" x y z
        State.Doc.Views.Redraw()
        g
    
    static member AddTextDot(t,p:Point3d) : Guid = 
        let g = State.Doc.Objects.AddTextDot(t, p)
        if g = Guid.Empty then failwithf "Euclid.Rhino.Rs.AddTextDot failed on point :%O" p
        State.Doc.Views.Redraw()
        g

    ///<summary>Adds a line Curve to the current model.</summary>
    ///<param name="start">(Point3d) Startpoint of the line</param>
    ///<param name="ende">(Point3d) Endpoint of the line</param>
    ///<returns>(Guid) objectId of the new Curve object.</returns>
    static member AddLine(start:Point3d, ende:Point3d) : Guid = 
        let  rc = State.Doc.Objects.AddLine(start, ende)
        if rc = Guid.Empty then failwithf "Euclid.Rhino.Rs.AddLine: Unable to add line to document. start:%O ende:%O" start ende
        State.Doc.Views.Redraw()
        rc

    ///<summary>Adds a line Curve to the current model.</summary>
    ///<param name="startX">(float) Startpoint of the line: X position</param>
    ///<param name="startY">(float) Startpoint of the line: Y position</param>
    ///<param name="startZ">(float) Startpoint of the line: Z position</param>
    ///<param name="endX">(float) Endpoint of the line: X position</param>
    ///<param name="endY">(float) Endpoint of the line: Y position</param>
    ///<param name="endZ">(float) Endpoint of the line:Z position</param>    
    ///<returns>(Guid) objectId of the new Curve object.</returns>
    static member AddLine(startX,startY,startZ,endX,endY,endZ:float) : Guid = 
        let start = Point3d(startX,startY,startZ)
        let ende = Point3d(endX,endY,endZ)
        let  rc = State.Doc.Objects.AddLine(start, ende)
        if rc = Guid.Empty then failwithf "Euclid.Rhino.Rs.AddLine: Unable to add line to document. startX:%g ,startY:%g ,startZ:%g and endX:%g ,endY:%g ,endZ:%g" startX startY startZ endX endY endZ
        State.Doc.Views.Redraw()
        rc
    
    ///<summary>Adds a 2D Line Curve to the current model at z level 0.0</summary>
    ///<param name="startX">(float) Startpoint of the line: X position</param>
    ///<param name="startY">(float) Startpoint of the line: Y position</param>
    ///<param name="endX">(float) Endpoint of the line: X position</param>
    ///<param name="endY">(float) Endpoint of the line: Y position</param>
    ///<returns>(Guid) objectId of the new Curve object.</returns>
    static member AddLine2D(startX,startY,endX,endY:float) : Guid = 
        let start = Point3d(startX,startY,0.0)
        let ende = Point3d(endX,endY,0.0)
        let  rc = State.Doc.Objects.AddLine(start, ende)
        if rc = Guid.Empty then failwithf "Euclid.Rhino.Rs.AddLine2D: Unable to add line to document. startX:%g ,startY:%g  and  endX:%g ,endY:%g," startX startY  endX endY 
        State.Doc.Views.Redraw()
        rc
    
    ///<summary>Enables or disables a Curve object's annotation arrows.</summary>
    ///<param name="curveId">(Guid) Identifier of a Curve</param>
    ///<param name="arrowStyle">(int) The style of annotation arrow to be displayed.
    ///    0 = no arrows
    ///    1 = display arrow at start of Curve
    ///    2 = display arrow at end of Curve
    ///    3 = display arrow at both start and end of Curve</param>
    ///<returns>(unit) void, nothing.</returns>
    static member CurveArrows(curveId:Guid, arrowStyle:int) : unit = //SET
        let rhobj = State.Doc.Objects.FindId(curveId) 
        if isNull rhobj then failwithf "Euclid.Rhino.Rs.CurveArrows: %O not found " curveId 
        let attr = rhobj.Attributes
        //let rc = attr.ObjectDecoration
        if arrowStyle >= 0 && arrowStyle <= 3 then
            if arrowStyle = 0 then
                attr.ObjectDecoration <- DocObjects.ObjectDecoration.None
            elif arrowStyle = 1 then
                attr.ObjectDecoration <- DocObjects.ObjectDecoration.StartArrowhead
            elif arrowStyle = 2 then
                attr.ObjectDecoration <- DocObjects.ObjectDecoration.EndArrowhead
            elif arrowStyle = 3 then
                attr.ObjectDecoration <- DocObjects.ObjectDecoration.BothArrowhead
            if not <| State.Doc.Objects.ModifyAttributes(curveId, attr, quiet=true) then
                failwithf "Euclid.Rhino.Rs.CurveArrows ModifyAttributes failed on style %d on %A" arrowStyle curveId
            State.Doc.Views.Redraw()
        else
            failwithf "Euclid.Rhino.Rs.CurveArrows style %d is invalid" arrowStyle

    /// Draws a line with a Curve Arrows from World Origin.
    static member DrawVector( vector:Vector3d) : Guid  = 
        let l = Rs.AddLine(Point3d.Origin, Point3d.Origin + vector )
        Rs.CurveArrows(l, 2)
        l
    
    /// Draws a line with a Curve Arrows from a given point.
    static member DrawVector( vector:Vector3d, fromPoint:Point3d) : Guid  = 
        let l = Rs.AddLine(fromPoint, fromPoint + vector )
        Rs.CurveArrows(l, 2)            
        l
    
    ///<summary>Draws the axes of a Plane and adds TextDots to label them.</summary>
    ///<param name="pl">(Plane)</param>
    ///<param name="axLength">(float) Optional, Default Value: <c>1.0</c>, the size of the drawn lines</param>
    ///<param name="suffixInDot">(string) Optional, Default Value: no suffix, text to add to x textdot label do of x axis. And y and z too.</param>
    ///<param name="layer">(string) Optional, Default Value: the current layer, String for layer to draw plane on. The Layer will be created if it does not exist.</param>
    ///<returns>List of Guids of added Objects</returns>
    static member DrawPlane(    pl:Plane,
                                [<OPT;DEF(1.0)>]axLength:float,
                                [<OPT;DEF("")>]suffixInDot:string,
                                [<OPT;DEF("")>]layer:string ) : ResizeArray<Guid>  = 
        let a=Rs.AddLine(pl.Origin, pl.Origin + pl.XAxis*axLength)
        let b=Rs.AddLine(pl.Origin, pl.Origin + pl.YAxis*axLength)
        let c=Rs.AddLine(pl.Origin, pl.Origin + pl.ZAxis*axLength*0.5)
        let e=Rs.AddTextDot("x"+suffixInDot, pl.Origin + pl.XAxis*axLength)
        let f=Rs.AddTextDot("y"+suffixInDot, pl.Origin + pl.YAxis*axLength)
        let g=Rs.AddTextDot("z"+suffixInDot, pl.Origin + pl.ZAxis*axLength*0.5)
        let es = ResizeArray [ a;b;c;e;f;g ]
        if layer <>"" then 
            for e in es do 
                Rs.setLayer layer e

        let index = State.Doc.Groups.Add()        
        if not <| State.Doc.Groups.AddToGroup(index, es) then failwithf "Euclid.Rhino.DrawPlane failed on %A" pl        
        es


    ///<summary>Adds a Polyline Curve.</summary>
    ///<param name="points">(Point3d seq) List of 3D points. The list must contain at least two points. If the
    ///    list contains less than four points, then the first point and
    ///    last point must be different</param>
    ///<returns>(Guid) objectId of the new Curve object.</returns>
    static member AddPolyline(points:Point3d seq) : Guid = 
        let pl = Polyline(points)        
        let rc = State.Doc.Objects.AddPolyline(pl)
        if rc = Guid.Empty then
            let ps = points |> Seq.map string |> String.concat "\r\n   "
            failwithf "Euclid.Rhino.AddPolyline: Unable to add polyline to document form %d points:\r\n '%s'" (Seq.length points) ps
        State.Doc.Views.Redraw()
        rc