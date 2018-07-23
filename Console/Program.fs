(*
{ macro-stuff }

a = [g c d d# f]
b = [g c d d# f g c d d#]
d = [100 80 75 60 60]

// play(note-list, repeats, dynamics-list)
play a 14 

*)
open Native
open System
open System.Runtime.InteropServices

    [<EntryPoint>]
    let main argv = 

        let nDevices = midiInGetNumDevs()
    
        let mutable deviceId = 0u;
        while deviceId < nDevices do
            let mutable caps = MIDIINCAPS()
            let pDevId = UIntPtr(deviceId)
            let ppDevId = pDevId.ToPointer()
            let sz = uint32(sizeof<MIDIINCAPS>)
            let result = midiInGetDevCaps(pDevId, &caps, sz)
            deviceId <- deviceId + 1u
            printfn "%A %A" result caps.szPname

        0 // return an integer exit code
