module Notes

open System

    //
    // Note 
    type Note(name:string, duration:int, octave:int) =
        let _name = name
        let _duration = duration
        let _octave = octave
        let _midiNum = 
            match name with
            | "c" -> Some(12)
            | "c#" -> Some(13)
            | "d" -> Some(14)
            | "d#" -> Some(15)
            | "e" -> Some(16)
            | "f" -> Some(17)
            | "f#" -> Some(18)
            | "g" -> Some(19)
            | "g#" -> Some(20)
            | "a" -> Some(21)
            | "a#" -> Some(22)
            | "b" -> Some(23)
            | _ -> None
            
        new (name:string, duration:int) = Note(name, duration, octave=0)
        new (name:string) = Note(name, duration=1)
        new (number:int, duration, octave) =
            let name = 
                match number with
                | n when n % 12 = 0 -> Some "c"
                | n when n % 13 = 0 -> Some "c#"
                | n when n % 14 = 0 -> Some "d"
                | n when n % 15 = 0 -> Some "d#"
                | n when n % 16 = 0 -> Some "e"
                | n when n % 17 = 0 -> Some "f"
                | n when n % 18 = 0 -> Some "f#"
                | n when n % 19 = 0 -> Some "g"
                | n when n % 20 = 0 -> Some "g#"
                | n when n % 21 = 0 -> Some "a"
                | n when n % 22 = 0 -> Some "a#"
                | n when n % 23 = 0 -> Some "b"
                | _ -> None

            match name with
            | Some note -> Note(note, duration, octave)
            | None -> Note("")

        member this.Name = _name
        member this.Number = _midiNum
        member this.Duration = _duration
        member this.Octave = _octave

    //
    // NoteBuilder
    type NoteBuilder (notes:string) =
        let splits = notes.ToLower().Split(Constants.Separators)
        let mutable _notes = Array.map (fun s -> new Note(s, 1)) splits
        let mutable _vel = 64
        let mutable _dur = 1
        let mutable _reps = 1

        new () = NoteBuilder("")

        member this.Notes 
            with get() = _notes
            and set(value) = _notes <- value

        member this.DefaultVelocity 
            with get() = _vel 
            and set(value) = _vel <- value

        member this.DefaultDuration 
            with get() = _dur
            and set(value) = _dur <- value

        member this.Seq = seq { for i in 1.._reps do yield! _notes }

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


