namespace MidiLib

    open Native
    open System

    type NotePlayer (t:Tempo, h:Option<HMIDI_IO>) =
        let mutable _tempo = t
        let mutable _handle: Option<HMIDI_IO> = h
        let mutable _channel: int = 1

        let DoPlayNote (h: HMIDI_IO, n:Note) = 
            let msgOn = Encoding.EncodeNoteOn(_channel, n.MidiNumber, n.Velocity)
            let msgOff = Encoding.EncodeNoteOff(_channel, n.MidiNumber, n.Velocity)
            midiOutShortMsg(h, msgOn) |> printfn "%A"
            Threading.Thread.Sleep t.MS
            midiOutShortMsg(h, msgOff) |> printfn "%A"
            ()
        
        let PlayNote n =
            printfn "%s %A %f" n.Name n.MidiNumber n.Duration
            match _handle with 
            | Some h -> DoPlayNote (h, n)
            | None -> ()
            ()

        member this.Device with set(value) = _handle <- value
        member this.Channel with get() = _channel and set(value) = _channel <- value

        member this.Play (notes:seq<Note>) =
            for n in notes do
                match _handle with
                | Some h -> PlayNote n
                | None -> ()


