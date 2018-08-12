module Console = 

    open MidiLib
    open Native
    open System
    open Devices

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
        
        let g = new NoteBuilder "g c"
        let d = new NoteBuilder "d e- f"
        //let a = new NoteBuilder "a"
        g.Octave <- 3
        g.Denominator <- 8
        let a = g.Append "d e- f" // 5 note unit
        //let b = 
        let c = (a.Append "g c d e-").Append "g c d"
        let e = a.WithNotes "d e- f"
        //printfn "%s" c.Notes
        
        let notes = seq { 
            // 1
            yield! (g.Append (d.Notes)).Seq (34)
            yield! (a.Append "g c d e-").Seq (18)
            yield! c.Seq (14)
            // 4
            yield! (c.Append "g c").Seq (15)
            yield! c.Seq (17)
            // 6
            yield! (a.Append "g c d e-").Seq (22)
            yield! a.Seq (26)
            yield! (a.Append "d").Seq (14)
            yield! (a.Append "d e-").Seq (18)
            // 10
            yield! (a.Append "d e- f").Seq (11)
            yield! (a.Append "d e- f d").Seq (16)
            yield! (a.Append "d e- f d e-").Seq (14)
            // 13
            yield! (a.Append "d e- f d e- f").Seq (11)
            yield! (a.Append "d e- f d e- f d").Seq (7)
            // 15 
            yield! ((g.Append (e.Notes, 3)).Append "d e-").Seq (11)
            yield! ((g.Append (e.Notes, 4)).Append "d e-").Seq (6)
            // 17
            yield! ((g.Append (e.Notes, 5)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 6)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 7)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 8)).Append "d e-").Seq ()
            // 21
            yield! ((g.Append (e.Notes, 9)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 10)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 12)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 14)).Append "d e-").Seq ()
            // 25
            yield! ((g.Append (e.Notes, 16)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 18)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 16)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 14)).Append "d e-").Seq ()
            // 29
            yield! ((g.Append (e.Notes, 12)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 10)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 9)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 8)).Append "d e-").Seq ()
            // 33
            yield! ((g.Append (e.Notes, 7)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 6)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 5)).Append "d e-").Seq ()
            yield! ((g.Append (e.Notes, 4)).Append "d e-").Seq (7)
            // 37
            yield! ((g.Append (e.Notes, 3)).Append "d e-").Seq (9)
            yield! ((g.Append (e.Notes, 2)).Append "d e-").Seq (11)
            yield! ((g.Append (e.Notes, 1)).Append "d e-").Seq (18)
            yield! ((g.Append "d e- g c d e- f d e-").Seq (3)
            // 41

        }
        
        // Create a player
        let temp = new Tempo 128.0
        let devices = GetOutputDevices()
        let device = OpenOutputDevice(devices, Devices.PC3K)
        let np = new NotePlayer (temp, device)
        np.Play notes
            
        //let task = 
        //    [|
        //        for i in 0..
        //    |]
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


