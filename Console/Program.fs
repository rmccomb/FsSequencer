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

    let PlaySequence (deviceName) =

        //
        // Two Pages by Philip Glass 1969
        //

        let temp = new Tempo 115.0
        
        let g = new NoteBuilder "g c"
        let d = new NoteBuilder "d e- f"
        g.Octave <- 4
        g.Denominator <- 8
        let a = g.Append "d e- f" // 5 note unit
        let c = (a.Append "g c d e-").Append "g c d"
        let e = a.WithNotes "d e- f"
        let g7 = g.WithNotes "g c d e- g c d" // 7 note unit for bar 41
        let d5 = a.WithNotes "d e- f d e-" // 5 note unit for bar 41, 57
        let c4 = a.WithNotes "c d e- f" // 4 note unit for 57 -

        
        let notes = seq { 
            // 1
            yield! (g.Append (d.Notes)).Seq 34
            yield! (a.Append "g c d e-").Seq 18
            yield! c.Seq 14
            // 4
            yield! (c.Append "g c").Seq 15
            yield! c.Seq 17
            // 6
            yield! (a.Append "g c d e-").Seq 22
            yield! a.Seq 26
            yield! (a.Append "d").Seq 14
            yield! (a.Append "d e-").Seq 18
            // 10
            yield! (a.Append "d e- f").Seq 11
            yield! (a.Append "d e- f d").Seq 16
            yield! (a.Append "d e- f d e-").Seq 14
            // 13
            yield! (a.Append "d e- f d e- f").Seq 11
            yield! (a.Append "d e- f d e- f d").Seq 7
            // 15 
            yield! ((g.Append (e.Notes, 3)).Append "d e-").Seq 11
            yield! ((g.Append (e.Notes, 4)).Append "d e-").Seq 6
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
            yield! ((g.Append (e.Notes, 4)).Append "d e-").Seq 7
            // 37
            yield! ((g.Append (e.Notes, 3)).Append "d e-").Seq 9
            yield! ((g.Append (e.Notes, 2)).Append "d e-").Seq 11
            yield! ((g.Append (e.Notes, 1)).Append "d e-").Seq 18
            yield! (g.Append "d e- g c d e- f d e-").Seq 3
            // 41
            //yield! ((g7.Append "g c").Append d5.Notes).Seq (7)
            //yield! (((g7.WithRepeats 2).Append g.Notes).Append (d5.Notes, 2)).Seq (6)
            // 41 to 55
            yield! seq {
                for i in [1;2;3;5;6;7;8;9;10;11;13;15;17;19;21] do
                    yield! g7.Seq i
                    yield! g.Seq ()
                    yield! d5.Seq i
            }
            // 56
            yield! d5.Seq 19

            // 57
            yield! seq {
                for i in [1;2;3;4;5;6;7;8;9;10;12;14;16;18;20] do
                    yield! c4.Seq i
                    yield! d5.Seq ()
            }
            // 72
            yield! c4.Seq ()
            // 73
            yield! seq {
                for _ in 1..14 do
                    yield! g.Seq()
                    yield! (g.WithNotes "d e- f c d e- f").Seq ()
            }
            // 74
            yield! seq {
                for _ in 1..7 do
                    yield! g.Seq()
                    yield! (g.WithNotes "d e- f c d e- f d e- f").Seq ()
            }
            // 75 
            yield! seq {
                for _ in 1..10 do
                    yield! g.Seq()
                    yield! (g.WithNotes "d e- f c d e- f d e- f e- f").Seq ()
            }
            // END
        }
        
        // Create a player
        let devices = GetOutputDevices()
        let device = OpenOutputDevice(devices, deviceName)
        let np = new NotePlayer (temp, device)
        np.Play notes
            
        //let task = 
        //    [|
        //        for i in 0..
        //    |]
        ()


    [<EntryPoint>]
    let main argv = 

        // Test play note
        // Create a player
    (*    let devices = GetOutputDevices()
        let device = OpenOutputDevice(devices, Devices.LoopMidiPort)
        match device with 
            | Some h -> PlayNote(h)
            | None -> ()
            *)
        
        //PlaySequence(Devices.LoopMidiPort)
        PlaySequence(Devices.KurzweilPC3K)



        0 // exit code


