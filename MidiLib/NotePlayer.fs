namespace MidiLib

    open Native
    open System

    type NotePlayer (t:Tempo, h:Option<HMIDI_IO>) =
        let mutable _tempo = t
        let mutable _handle: Option<HMIDI_IO> = h
        let mutable _channel: int = 0 // = MIDI CH 1

        let DoPlayNote (h: HMIDI_IO, n:Note) = 
            let msgOn = Encoding.EncodeNoteOn(_channel, n.MidiNumber, n.Velocity)
            let msgOff = Encoding.EncodeNoteOff(_channel, n.MidiNumber, n.Velocity)
            midiOutShortMsg(h, msgOn) |> ignore // printfn "%A"
            Threading.Thread.Sleep (_tempo.MS - 7) // NB Note end before official end
            midiOutShortMsg(h, msgOff) |> ignore // printfn "%A"
            ()
        
        let PlayNote n i =
            printfn "%d %s %A %f" i n.Name n.MidiNumber n.Duration
            match _handle with 
            | Some h -> DoPlayNote (h, n)
            | None -> ()
            ()

        member __.Device with set(value) = _handle <- value
        member __.Channel with get() = _channel and set(value) = _channel <- value

        member __.Play (notes:seq<Note>) =
            let mutable i = 0;
            for n in notes do
                match _handle with
                | Some _ -> PlayNote n i
                | None -> ()
                i <- i + 1



