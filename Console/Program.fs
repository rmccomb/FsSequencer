(*
{ macro-stuff }

a = [g c d d# f]
b = [g c d d# f g c d d#]
d = [100 80 75 60 60]

// play(note-list, repeats, dynamics-list)
play a 14 

*)
module Console = 

    open Native
    open System
    //open System.Runtime.InteropServices

    let DisplayDevices() =
        let nDevices = midiInGetNumDevs()
        let mutable deviceId = 0u;
        while deviceId < nDevices do
            let mutable caps = MIDIINCAPS()
            let pDevId = UIntPtr(deviceId)
            let sz = uint32(sizeof<MIDIINCAPS>)
            let result = midiInGetDevCaps(pDevId, &caps, sz)
            printfn "%A %A" result caps.szPname
            deviceId <- deviceId + 1u

    [<EntryPoint>]
    let main argv = 

        DisplayDevices()

        0 // exit code


