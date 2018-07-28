module Notes

    open Encoding

    // Note record
    type Note =
        {
            Name: string
            Duration: int
            MidiBase: int
            Octave: int
            Velocity: int
            Index: int
        } 

        member this.MidiNumber = 
            this.MidiBase + this.Octave * 12


    //
    // NoteBuilder
    type NoteBuilder (notes:string) =
        let mutable _vel = 64
        let mutable _dur = 1
        let mutable _reps = 1
        let mutable _octave = 1
        let splits = notes.ToLower().Split(Constants.Separators)

        member this.DefaultVelocity 
            with get() = _vel 
            and set(value) = _vel <- value

        member this.DefaultDuration 
            with get() = _dur
            and set(value) = _dur <- value

        member this.DefaultOctave
            with get() = _octave
            and set(value) = _octave <- value

        member this.Seq (?repeats, ?shift) = seq { 
            let rep = match repeats with
                        | Some n -> n
                        | None -> 1
            let sh = match shift with
                        | Some s -> s
                        | None -> 0
            for i in 1..rep do 
                yield! Array.map (fun s -> 
                    { 
                        Name = s 
                        Duration = _dur
                        MidiBase = match GetMidiNum (s) with
                                    | Some n -> n + sh
                                    | None -> failwith "no note"
                        Octave = _octave
                        Velocity = _vel
                        Index = i
                    }) splits }




        //member this.Transpose (semis:int) =
        //    let a = Array.map (
        //        fun n -> 
        //            match n.Number with
        //            | Some(num) -> new Note("a")
        //            | None -> failwith "no note"
        //        ) _notes
        //    a


