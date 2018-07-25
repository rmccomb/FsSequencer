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

    let GetInputDevices () =
        let nDevices = midiInGetNumDevs()
        let mutable deviceId = 0u;
        let mutable devices = List.Empty
        while deviceId < nDevices do
            let mutable caps = MIDIINCAPS()
            let pDevId = UIntPtr(deviceId)
            let sz = uint32(sizeof<MIDIINCAPS>)
            let result = midiInGetDevCaps(pDevId, &caps, sz)
            printfn "%A %A" result caps.szPname
            devices <- (deviceId, caps.szPname) :: devices
            deviceId <- deviceId + 1u
        devices

    let GetOutputDevices () =
        let nDevices = midiOutGetNumDevs()
        let mutable deviceId = 0u;
        let mutable devices = List.Empty
        while deviceId < nDevices do
            let mutable caps = MIDIOUTCAPS()
            let pDevId = UIntPtr(deviceId)
            let sz = uint32(sizeof<MIDIOUTCAPS>)
            let result = midiOutGetDevCaps(pDevId, &caps, sz)
            printfn "%A %A" result caps.szPname
            devices <- (deviceId, caps.szPname) :: devices
            deviceId <- deviceId + 1u
        devices

    let OpenOutputDevice (deviceId:uint32, name:string) =
        //let deviceId = 0u
        let mutable handle = HMIDI_IO()
        let uDeviceId = UIntPtr(deviceId)
        let callback = null
        let callbackInt = UIntPtr(0u)
        let flags = uint32(0)
        let result = midiOutOpen(&handle, uDeviceId, callback, callbackInt, flags)
        printfn "%A" result
        handle

    let CloseOutputDevice (handle: HMIDI_IO) = 
        let result = midiOutClose(handle)
        printfn "%A" result
        ()

    let PlayNote (handle: HMIDI_IO) = 
        let chan = 1
        let pitch = 80
        let velocity = 99
        let msg = Encoding.EncodeNoteOn(chan, pitch, velocity)
        match Encoding.DecodeMessage (msg, 0u) with
        | Some m -> printfn "%A" m
        | None -> ()
        
        let result = midiOutShortMsg(handle, msg)
        printfn "%A" result

        let msg = Encoding.EncodeNoteOff(chan, pitch, velocity)
        let result = midiOutShortMsg(handle, msg)
        printfn "%A" result
        ()

    [<EntryPoint>]
    let main argv = 
        let devices = GetOutputDevices()
        let device = devices.Head
        let handle = OpenOutputDevice(device)
        PlayNote(handle)
        CloseOutputDevice(handle)
        0 // exit code


