(*
{ macro-stuff }

a = [g c d d# f]
b = [g c d d# f g c d d#]
d = [100 80 75 60 60]

// play(note-list, repeats, dynamics-list)
play a 14 

*)
module Console = 

    open Devices
    open Notes
    open Native
    open System

    let PlayNote (handle: HMIDI_IO) = 
        let chan = 0 // zero-based here (=1)
        let pitch = 68
        let velocity = 99
        let msg = Encoding.EncodeNoteOn(chan, pitch, velocity)

        // Check encoding
        match Encoding.DecodeMessage (msg, 0u) with
        | Some m -> printfn "%A" m
        | None -> ()
        
        // Send note-on and wait 1 sec
        let result = midiOutShortMsg(handle, msg)
        printfn "%A" result
        Threading.Thread.Sleep 1000

        // Send note-off
        let msg = Encoding.EncodeNoteOff(chan, pitch, velocity)
        let result = midiOutShortMsg(handle, msg)
        printfn "%A" result
        ()

    let PlaySequence ((*handle: HMIDI_IO*)) =
        
        let a = new NoteBuilder "g c d e- f"
        //let a = new NoteBuilder "a"
        a.Octave <- 2
        let b = a.Append "g c d e-"
        let c = b.Append "g c d"

        printfn "%s" c.NotesAsString
        
        //let nn = seq { yield! a.Notes }
        //for n in nn do
        //    printfn "%s %A" n.Name n.Number

        //let nn = seq { for i in 1..5 do yield! a.Notes }
        //for n in nn do
        //    printfn "%s %A" n.Name n.Number

        let nn = seq { 
            yield! a.Seq ()
            yield! b.Seq ()
        }

        for n in nn do
            printfn "%s %A %d" n.Name n.MidiNumber n.Duration
            


        ()






    [<EntryPoint>]
    let main argv = 
        
        // midi output
        //let devices = GetOutputDevices()
        //let handle = OpenOutputDevice(devices, Devices.PC3K)
        //PlayNote(handle)
        //CloseOutputDevice(handle)
        // end midi output

        PlaySequence()

        0 // exit code


