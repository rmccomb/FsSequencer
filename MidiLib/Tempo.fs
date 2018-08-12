namespace MidiLib

    type Tempo (bpm:double) =
        
        let mutable _bpm = bpm

        member __.BPM with get() = _bpm and set(value) = _bpm <- value
        
        member __.MS with get() = int32(60.0 / 4.0 / _bpm * 1000.0)
