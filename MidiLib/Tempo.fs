namespace MidiLib

    type Tempo (bpm:double) =
        let mutable _bpm = 120.0

        member this.BPM with get() = _bpm and set(value) = _bpm <- value

