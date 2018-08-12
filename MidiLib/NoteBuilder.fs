namespace MidiLib

    open Encoding

    // Note record
    type Note =
      { Name: string
        Duration: double
        MidiBase: int
        Octave: int
        Velocity: int
        Index: int
        Group: int } 
        member __.MidiNumber = 
            __.MidiBase + __.Octave * 12


    // NoteBuilder
    type NoteBuilder (notes:string[]) =
        let mutable _vel = 64
        let mutable _duration = 1.0
        let mutable _denominator = 4
        let mutable _reps = 1
        let mutable _octave = 1
        let mutable _notes = notes
        let mutable _shift = 0

        static let NotesToArray (nn:string) = 
            nn.Trim().ToLower().Split(Constants.Separators)

        new(notes:string) =
            NoteBuilder(NotesToArray(notes))

        member __.NotesArray with get() = _notes and set(value) = _notes <- value
        member __.Notes = String.concat " " _notes
        member __.Velocity with get() = _vel and set(value) = _vel <- value
        member __.Denominator with get() = _denominator and set(value) = _denominator <- value
        member __.Duration with get() = _duration and set(value) = _duration <- value
        member __.Octave with get() = _octave and set(value) = _octave <- value
        member __.Shift with get() = _shift and set(value) = _shift <- value
        member __.Repeats with get() = _reps and set(value) = _reps <- value

        member __.Clone = __.MemberwiseClone() :?> NoteBuilder

        member __.Seq (?repeats, ?shift) = seq { 
            let rep = match repeats with
                        | Some n -> n
                        | None -> _reps
            let sh = match shift with
                        | Some s -> s + _shift
                        | None -> _shift
            for i in 1..rep do 
                yield! Array.map (fun s -> 
                    { 
                        Name = s 
                        Duration = _duration / float(_denominator)
                        MidiBase = match GetMidiNum (s) with
                                    | Some n -> n + sh
                                    | None -> failwith "no note"
                        Octave = _octave
                        Velocity = _vel
                        Index = i
                        Group = 0
                    }) _notes }

        member __.Append (notes:string, ?repeats:int) =
            let tnotes = match repeats with
                            | Some n -> String.replicate n (notes + " ")
                            | None -> notes

            let notesArr = NotesToArray tnotes            
            let nb = __.Clone
            nb.NotesArray <- Array.append __.NotesArray notesArr
            nb

        member __.WithNotes (value:string) = 
            let nb = __.Clone
            nb.NotesArray <- NotesToArray value
            nb

        member __.WithVelocity value = 
            let nb = __.Clone
            nb.Velocity <- value
            nb

        member __.WithDuration value = 
            let nb = __.Clone
            nb.Duration <- value
            nb

        member __.WithOctave value = 
            let nb = __.Clone
            nb.Octave <- value
            nb

        member __.WithShift value = 
            let nb = __.Clone
            nb.Shift <- value
            nb

        member __.WithRepeats value = 
            let nb = __.Clone
            nb.Repeats <- value
            nb

        
