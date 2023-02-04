namespace Euclid

// all modules copied over and adapted from from https://github.com/goswinr/Rhino.Scripting


open System
open System.Globalization // for UnicodeCategory
open Rhino
open Rhino.DocObjects

module internal ColorUtil = 
    /// A Red-Green-Blue Color made up of 3 bytes.
    /// ( NOT using System.Drawing.Color internally.)
    [<Struct;NoComparison>]
    type Color =
        /// Gets the Red part of this Red-Green-Blue Color 
        val Red : byte
    
        /// Gets the Green part of this Red-Green-Blue Color 
        val Green : byte

        /// Gets the Blue part of this Red-Green-Blue Color 
        val Blue : byte

        /// Create a new Red-Green-Blue Color 
        new (r,g,b) = {Red=r; Green=g; Blue=b}  

    /// Point must be at middle of expression: like this: min <=. x .<= max
    let inline (<=.) left middle = (left <= middle, middle)

    /// Point must be at middle of expression: like this: min <. x .< max
    let inline (<.) left middle = (left < middle, middle)

    /// Point must be at middle of expression: like this: min <. x .< max
    let inline (.<) (leftResult, middle) right = leftResult && (middle < right)

    /// To make sure a value is between 0.0 and 1.0 range
    let inline clamp01 (value:'T) :'T = 
        // if isNan value then ArgumentException.RaiseBase "FsEx.UtilMath.clamp01: given input is NaN."
        if   value > LanguagePrimitives.GenericOne< ^T>  then LanguagePrimitives.GenericOne< ^T>
        elif value < LanguagePrimitives.GenericZero< ^T> then LanguagePrimitives.GenericZero< ^T>
        else value

    let Rand = 
        let now = DateTime.Now
        let seed = now.TimeOfDay
        new Random(int <| seed.TotalMilliseconds * 0.001295473) // random seed different than other modules
    
    let mutable lastHue = 0.0
    
    let mutable lumUp = false
    
    /// Generates a Random color with high saturation probability, excluding yellow colors
    /// These are ideal for layer color in Rhino3d CAD app
    /// Using golden-ratio-loop subsequent colors will have very distinct hues
    let rec randomForRhinoRec () =  
        lastHue <- lastHue + 0.6180334 // golden angle conjugate
        lastHue <- lastHue % 1.0 // loop it between 0.0 and 1.0    
        let mutable s = Rand.NextDouble() 
        s <- s*s*s*s  //  0.0 - 1.0 increases the probability that the number is small( )
        s <- s*0.7    //  0.0 - 0.7 make sure it is less than 0.6
        s <- 1.1 - s  //  1.1 - 0.6  
        s <- clamp01 s //  1.0 - 0.6 
        let mutable l = Rand.NextDouble() 
        l <- l * l     //  0.0 - 1.0 increases the probability that the number is small( )
        l <- l * 0.35 * s   //  0.0 - 0.25 , and scale down with saturation too
        l <- if lumUp then lumUp<-false;  0.5+l*1.1 else lumUp<-true ;  0.5-l //alternate luminance up or down,  mor on the bright side
        if l > 0.3 && lastHue > 0.10 && lastHue < 0.22 then  // exclude yellow unless dark
            randomForRhinoRec() 
        else    
            lastHue, s, l
    
    /// Given Hue, Saturation, Luminance in range of 0.0 to 1.0, returns a FsEx.Color
    /// Will fail with ArgumentOutOfRangeException on too small or too big values,
    /// but up to a tolerance of 0.001 values will be clamped to 0 or 1.
    let fromHSL (hue:float, saturation:float, luminance:float) = 
        // from http://stackoverflow.com/questions/2942/hsl-in-net
        // or http://bobpowell.net/RGBHSB.aspx
        // allow some numerical error:
        if not (-0.001 <. hue        .< 1.001) then failwithf "Eclid.Rhino.ColorUtil.fromHSL: H is bigger than 1.0 or smaller than 0.0: %f" hue
        if not (-0.001 <. saturation .< 1.001) then failwithf "Eclid.Rhino.ColorUtil.fromHSL: S is bigger than 1.0 or smaller than 0.0: %f" saturation
        if not (-0.001 <. luminance  .< 1.001) then failwithf "Eclid.Rhino.ColorUtil.fromHSL: L is bigger than 1.0 or smaller than 0.0: %f" luminance
        let H = clamp01 hue
        let S = clamp01 saturation
        let L = clamp01 luminance
        let v = if L <= 0.5 then L * (1.0 + S) else L + S - L * S
        let r,g,b = 
            if v > 0.001 then
                let m = L + L - v
                let sv = (v - m) / v
                let h = H * 5.999999 // so sextant never actually becomes 6
                let sextant = int h
                let fract = h - float sextant
                let vsf = v * sv * fract
                let mid1 = m + vsf
                let mid2 = v - vsf
                match sextant with
                    | 0 -> v,   mid1,    m
                    | 1 -> mid2,   v,    m
                    | 2 -> m,      v, mid1
                    | 3 -> m,   mid2,    v
                    | 4 -> mid1,   m,    v
                    | 5 -> v,      m, mid2
                    | x -> failwithf "Eclid.Rhino.ColorUtil.fromHSL: Error in internal HLS Transform, sextant is %d at Hue=%g, Saturation=%g, Luminance=%g" x H S L
            else
                L,L,L // default to gray value
        Color (byte(round(255.* r)) ,  byte(round(255.* g)) , byte(round(255.* b)) )

    /// Generates a Random color with high saturation probability, excluding yellow colors
    /// These are ideal for layer color in Rhino3d CAD app.
    /// The colors returned by subsequent calls to this functions will have very distinct hues.
    /// This is achieved by using a golden-ratio-loop and an internal cache of the last generated color.
    let randomForRhino () =  
        let h, s, l = randomForRhinoRec()
        fromHSL (h,s,l)

module internal UtilLayer = 
    
    let randomLayerColor()=
        let c = ColorUtil.randomForRhino()
        Drawing.Color.FromArgb(int c.Red, int c.Green, int c.Blue)
    
    
    ///<summary>Checks if a string is a acceptable string for use in Rhino Object Names or User Dictionary keys and values.
    /// A acceptable string may not include line returns, tabs, and leading or trailing whitespace.
    /// Confusing or ambiguous Unicode characters that look like ASCII are allowed. </summary>
    ///<param name="name">(string) The string to check.</param>
    ///<param name="allowEmpty">(bool) set to true to make empty strings pass. </param>    
    ///<returns>(bool) true if the string is a valid name.</returns>
    let isAcceptableStringId(name:string, allowEmpty:bool ) : bool = 
        if isNull name then false
        elif allowEmpty && name = "" then true
        else
            let tr = name.Trim()
            if tr.Length <> name.Length then false
            else
                let rec loop i = 
                    if i = name.Length then true
                    else
                        let c = name.[i]
                        if   c < ' ' then false // is a control character
                        elif c <= '~' then // always OK , Unicode points 32 till 126 ( regular letters numbers and symbols)
                            loop(i+1)
                        else                            
                            let cat = Char.GetUnicodeCategory(c)
                            match cat with
                            // always OK :
                            | UnicodeCategory.UppercaseLetter 
                            | UnicodeCategory.LowercaseLetter
                            | UnicodeCategory.CurrencySymbol 
                            | UnicodeCategory.OtherSymbol       
                            | UnicodeCategory.MathSymbol        
                            | UnicodeCategory.OtherNumber    
                            | UnicodeCategory.SpaceSeparator         
                            | UnicodeCategory.ConnectorPunctuation   
                            | UnicodeCategory.DashPunctuation        
                            | UnicodeCategory.TitlecaseLetter
                            | UnicodeCategory.ModifierLetter
                            | UnicodeCategory.NonSpacingMark
                            | UnicodeCategory.SpacingCombiningMark
                            | UnicodeCategory.EnclosingMark
                            | UnicodeCategory.LetterNumber
                            | UnicodeCategory.LineSeparator
                            | UnicodeCategory.Format
                            | UnicodeCategory.OtherNotAssigned
                            | UnicodeCategory.ModifierSymbol
                            | UnicodeCategory.OtherPunctuation 
                            | UnicodeCategory.DecimalDigitNumber 
                            | UnicodeCategory.InitialQuotePunctuation
                            | UnicodeCategory.FinalQuotePunctuation
                            | UnicodeCategory.OtherLetter    
                            | UnicodeCategory.OpenPunctuation  
                            | UnicodeCategory.ClosePunctuation 
                                -> loop(i+1)                                

                            // NOT OK :
                            | UnicodeCategory.ParagraphSeparator
                            | UnicodeCategory.Control                            
                            | UnicodeCategory.Surrogate                          
                            | UnicodeCategory.PrivateUse                          
                            | _ -> false
                loop 0




    let eVSLN = "Rhino.Scripting.UtilLayer.failOnBadShortLayerName"

    /// Raise exceptions if short layer-name is not valid
    /// it may not contain :: or control characters
    let internal failOnBadShortLayerName(name:string, fullPath:string) : unit= 
        if isNull name then failwithf "Eclid.Rhino.UtilLayer : %s: null string as layer name in '%s'" eVSLN fullPath

        if String.IsNullOrWhiteSpace name then // to cover for StringSplitOptions.None
            failwithf "Eclid.Rhino.UtilLayer : %s Empty or just whitespace string as layer name in '%s'" eVSLN fullPath
        
        if name.Contains "::" then
            failwithf "Eclid.Rhino.UtilLayer : %s: Short layer name '%s' shall not contains two colons (::).  in '%s'" eVSLN name fullPath
        
         
        if not<| isAcceptableStringId(name, false) then 
                failwithf "Eclid.Rhino.UtilLayer : %s: Short layer name '%s' is invalid. It may not include line returns, tabs, and leading or trailing whitespace. in '%s'" eVSLN  name fullPath
       
        match Char.GetUnicodeCategory(name.[0]) with
        | UnicodeCategory.OpenPunctuation 
        | UnicodeCategory.ClosePunctuation ->  // { [ ( } ] ) don't work at start of layer name
            failwithf "Eclid.Rhino.UtilLayer : %s: Short layer name '%s' may not start with a '%c' in '%s'" eVSLN name name.[0] fullPath
        | _ -> ()

    let internal getParents(lay:Layer) = 
        let rec find (l:Layer) ps = 
            if l.ParentLayerId = Guid.Empty then ps
            else
            let pl = State.Doc.Layers.FindId(l.ParentLayerId)
            if isNull pl then failwithf "Eclid.Rhino.UtilLayer.getParents : ParentLayerId not found in layers"
            find pl (pl::ps)
        find lay []

    let internal visibleSetTrue(lay:Layer, forceVisible:bool) : unit = 
        if not lay.IsVisible then
            if forceVisible then
                if not (State.Doc.Layers.ForceLayerVisible(lay.Id)) then failwithf "Eclid.Rhino.UtilLayer.visibleSetTrue Failed to turn on sub-layers of layer  %s"  lay.FullPath
            else
                lay.SetPersistentVisibility(true)

    let internal visibleSetFalse(lay:Layer, persist:bool) : unit = 
        if lay.IsVisible then
            lay.IsVisible <- false
        if persist then
            if lay.ParentLayerId <> Guid.Empty then
                lay.SetPersistentVisibility(false)

    let internal lockedSetTrue(lay:Layer,forceLocked:bool) : unit = 
        if not lay.IsLocked then
            lay.IsLocked <- true
        if forceLocked then
            lay.SetPersistentLocking(true)


    let internal lockedSetFalse(lay:Layer,  parentsToo:bool) : unit = 
        if lay.IsLocked then
            //lay.IsLocked <- false // fails with msg box if parents are locked
            if parentsToo then
                for l in getParents(lay) do
                    l.SetPersistentLocking(false)
                lay.SetPersistentLocking(false) // does not unlock parents

    type internal LayerState = Off | On | ByParent

    [<Struct>]
    type internal FoundOrCreatedIndex = 
        |LayerCreated of createdIdx :int
        |LayerFound   of foundIdx   :int

        member this.Index = 
            match this with 
            |LayerCreated i -> i
            |LayerFound   i -> i
      

    /// Creates all parent layers too if they are missing, uses same locked state and colors for all new layers.
    /// The collapseParents parameter only has an effect if layer is created, not if it exists already  
    let internal getOrCreateLayer(name:string, colorForNewLayers, visible:LayerState, locked:LayerState, collapseParents:bool) : FoundOrCreatedIndex = 
        // TODO trim off leading and trailing '::' from name string to be more tolerant ?
        match State.Doc.Layers.FindByFullPath(name, RhinoMath.UnsetIntIndex) with
        | RhinoMath.UnsetIntIndex ->
            match name with
            | null -> failwithf "Eclid.Rhino.UtilLayer.getOrCreateLayer: Cannot get or create layer from null string"
            | ""   -> failwithf "Eclid.Rhino.UtilLayer.getOrCreateLayer: Cannot get or create layer from empty string"
            | _ ->
                match name.Split( [|"::"|], StringSplitOptions.None) with // TODO or use StringSplitOptions.RemoveEmptyEntries ??
                | [| |] -> failwithf "Eclid.Rhino.UtilLayer.getOrCreateLayer: Cannot get or create layer for name: '%s'" name
                | ns ->
                    let rec createLayer(nameList, prevId, prevIdx, root) : int = 
                        match nameList with
                        | [] -> prevIdx // exit recursion
                        | branch :: rest ->
                            if String.IsNullOrWhiteSpace branch then // to cover for StringSplitOptions.None
                                failwithf "Eclid.Rhino.UtilLayer.getOrCreateLayer: A segment falls into String.IsNullOrWhiteSpace. Cannot get or create layer for name: '%s'" name
                            let fullPath = if root="" then branch else root + "::" + branch
                            match State.Doc.Layers.FindByFullPath(fullPath, RhinoMath.UnsetIntIndex) with
                            | RhinoMath.UnsetIntIndex -> // actually create layer:
                                failOnBadShortLayerName (branch, name)   // only check non existing sub layer names
                                let layer = DocObjects.Layer.GetDefaultLayerProperties()
                                if prevId <> Guid.Empty then 
                                    layer.ParentLayerId <- prevId
                                    if collapseParents then 
                                        State.Doc.Layers.[prevIdx].IsExpanded <- false

                                match visible with
                                |ByParent -> ()
                                |On  -> visibleSetTrue(layer, true)
                                |Off -> visibleSetFalse(layer, true)

                                match locked with
                                |ByParent -> ()
                                |On  -> lockedSetTrue(layer, true)
                                |Off -> lockedSetFalse(layer, true)

                                layer.Name <- branch
                                layer.Color <- colorForNewLayers() // delay creation of (random) color till actually needed ( so random colors are not created, in most cases layer exists)
                                let i = State.Doc.Layers.Add(layer)
                                let id = State.Doc.Layers.[i].Id // just using layer.Id would be empty guid
                                createLayer(rest , id , i,  fullPath)

                            | i ->
                                createLayer(rest , State.Doc.Layers.[i].Id , i ,fullPath)

                    LayerCreated (createLayer( ns |> List.ofArray, Guid.Empty, 0, ""))
        | i -> LayerFound i

