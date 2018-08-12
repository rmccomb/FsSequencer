namespace MidiLib

    type Tempo (bpm:double) =
        
        let mutable _bpm = bpm

        member this.BPM with get() = _bpm and set(value) = _bpm <- value
        
        member this.MS with get() = int32(60.0 / 4.0 / _bpm * 1000.0)
