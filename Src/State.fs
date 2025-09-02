namespace EuclidRhino

// all modules copied over and adapted from from https://github.com/goswinr/Rhino.Scripting

open System
open Rhino
open Rhino.Runtime


type internal Delegate = delegate of unit -> unit

/// An internal static class to hold current state like active Rhino document.
[<AbstractClass; Sealed>] //static class, use these attributes to match C# static class and make in visible in C# // https://stackoverflow.com/questions/13101995/defining-static-classes-in-f
type internal State private () =


    /// The current active Rhino document (= the file currently open)
    static let mutable doc : RhinoDoc = null


    //----------------------------------------------------------------
    //-----------------Update state:----------------------------------
    //----------------------------------------------------------------


    /// keep the reference to the active Document (3d file ) updated.
    static let updateDoc (document:RhinoDoc) = doc <- document //Rhino.RhinoDoc.ActiveDoc


    // -------Events: --------------------

    static let setupEventsInSync() =
        let delg = new Delegate( fun () -> RhinoDoc.ActiveDocumentChanged.Add (fun args -> updateDoc args.Document) )
        try
            RhinoApp.InvokeOnUiThread (delg)
        with e ->
            let err = sprintf "%A" e
            eprintfn "%s" err
            RhinoApp.WriteLine err


    static let initState()=
        if not HostUtils.RunningInRhino then
            failwithf "Euclid.Rhino.State.initState Failed to find the active Rhino document, is this dll running hosted inside the Rhino process? "
        else
            //RhinoSync.Initialize()
            updateDoc(RhinoDoc.ActiveDoc)  // do first
            setupEventsInSync()            // do after Doc is set up


    //----------------------------------------------------------------
    //-----------------internally public members------------------------
    //----------------------------------------------------------------

    /// The current active Rhino document (= the file currently open)
    static member Doc
        with get()=
            if isNull doc then initState()
            doc

    static member DoSync (func:unit->'T) : 'T =
        if RhinoApp.InvokeRequired then
            if isNull Threading.SynchronizationContext.Current then
                Eto.Forms.Application.Instance.Invoke func
            else
                async{
                    do! Async.SwitchToContext Threading.SynchronizationContext.Current
                    return func()
                    } |> Async.RunSynchronously
        else
            func()