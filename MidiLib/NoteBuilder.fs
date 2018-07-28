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
        }

        member this.MidiNumber = 
            this.MidiBase + this.Octave * 12


    //
    // NoteBuilder
    type NoteBuilder (notes:string) =
        let mutable _notes = notes
        let mutable _vel = 64
        let mutable _dur = 1
        let mutable _reps = 1
        let mutable _octave = 1
        let splits = notes.ToLower().Split(Constants.Separators)
        //let mutable _notes = 

        member this.Notes 
            with get() = _notes
            and set(value) = _notes <- value

        member this.DefaultVelocity 
            with get() = _vel 
            and set(value) = _vel <- value

        member this.DefaultDuration 
            with get() = _dur
            and set(value) = _dur <- value

        member this.DefaultOctave
            with get() = _octave
            and set(value) = _octave <- value

        member this.Seq = seq { 
            for i in 1.._reps do 
                yield! Array.map (fun s -> 
                    { 
                        Name = s 
                        Duration = _dur
                        MidiBase = match GetMidiNum (s) with
                                    | Some n -> n
                                    | None -> failwith "no note"
                        Octave = _octave
                        Velocity = _vel
                    }) splits }

        member this.Repeats 
            with get() = _reps
            and set(value) = _reps <- value



        //member this.Transpose (semis:int) =
        //    let a = Array.map (
        //        fun n -> 
        //            match n.Number with
        //            | Some(num) -> new Note("a")
        //            | None -> failwith "no note"
        //        ) _notes
        //    a


