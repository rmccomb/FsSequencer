namespace MidiLib

    type Tempo (bpm:double) =
        
        let mutable _bpm = bpm
        let endAdjust = 7 // end note early to avoid clicks, compensate here

        member __.BPM with get() = _bpm and set(value) = _bpm <- value
        
        member __.MS with get() = int32(60.0 / 4.0 / _bpm * 1000.0) + endAdjust
