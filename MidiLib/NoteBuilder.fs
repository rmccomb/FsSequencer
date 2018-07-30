namespace MidiLib

    open Encoding

    // Note record
    type Note =
        {
            Name: string
            Duration: double
            MidiBase: int
            Octave: int
            Velocity: int
            Index: int
            Group: int
        } 
        member this.MidiNumber = 
            this.MidiBase + this.Octave * 12


    // NoteBuilder
    type NoteBuilder (notes:string[]) =
        let mutable _vel = 64
        let mutable _duration = 1.0
        let mutable _reps = 1
        let mutable _octave = 1
        let mutable _notes = notes
        let mutable _shift = 0

        static let NotesToArray (nn:string) = 
            nn.ToLower().Split(Constants.Separators)

        new(notes:string) =
            NoteBuilder(NotesToArray(notes))

        member this.Notes with get() = _notes and set(value) = _notes <- value
        member this.NotesAsString = String.concat " " _notes
        member this.Velocity with get() = _vel and set(value) = _vel <- value
        member this.Duration with get() = _duration and set(value) = _duration <- value
        member this.Octave with get() = _octave and set(value) = _octave <- value
        member this.Shift with get() = _shift and set(value) = _shift <- value
        member this.Repeats with get() = _reps and set(value) = _reps <- value

        member this.Clone = this.MemberwiseClone() :?> NoteBuilder

        member this.Seq (?repeats, ?shift) = seq { 
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
                        Duration = _duration
                        MidiBase = match GetMidiNum (s) with
                                    | Some n -> n + sh
                                    | None -> failwith "no note"
                        Octave = _octave
                        Velocity = _vel
                        Index = i
                        Group = 0
                    }) _notes }

        member this.WithNotes (value:string) = 
            let nb = this.Clone
            nb.Notes <- NotesToArray value
            nb

        member this.WithVelocity value = 
            let nb = this.Clone
            nb.Velocity <- value
            nb

        member this.WithDuration value = 
            let nb = this.Clone
            nb.Duration <- value
            nb

        member this.WithOctave value = 
            let nb = this.Clone
            nb.Octave <- value
            nb

        member this.WithShift value = 
            let nb = this.Clone
            nb.Shift <- value
            nb

        member this.WithRepeats value = 
            let nb = this.Clone
            nb.Repeats <- value
            nb

        member this.Append (notes:string) =
            let nb = this.Clone
            nb.Notes <- Array.append this.Notes (NotesToArray notes)
            nb
        
