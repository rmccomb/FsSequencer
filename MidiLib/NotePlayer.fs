namespace MidiLib

    open Native
    open System

    type NotePlayer () =
        let mutable _tempo: Option<Tempo> = None
        let mutable _handle: Option<HMIDI_IO> = None
        member this.Tempo with set(value) = _tempo <- Some value
        member this.Device with set(value) = _handle <- value

        member this.Play (notes:seq<Note>) =
            for n in notes do
                match _handle with
                | Some h -> printf "sending to device"
                | None -> printfn "%s %A %f" n.Name n.MidiNumber n.Duration

            

