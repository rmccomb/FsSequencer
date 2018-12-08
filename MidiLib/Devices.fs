module Devices

    open Native
    open System
    open System.Collections.Generic

    let Eventide = "Eventide Dice"
    let KurzweilPC3K = "Kurzweil PC3K"

    let GetInputDevices () =
        let nDevices = midiInGetNumDevs()
        printfn "%d input devices found" nDevices
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
        printfn "%d output devices found" nDevices
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

    let OpenOutputDevice (devices: (uint32 * string) list, name:string) :Option<HMIDI_IO> =
        try
            let deviceId, _ = List.find (fun (_,n) -> n = name) devices
            let mutable handle = HMIDI_IO()
            let uDeviceId = UIntPtr(deviceId)
            let callback = null
            let callbackInt = UIntPtr(0u)
            let flags = uint32(0)
            let result = midiOutOpen(&handle, uDeviceId, callback, callbackInt, flags)
            printfn "%A" result
            Some handle
        with 
            | :? KeyNotFoundException -> None

    let CloseOutputDevice (handle: HMIDI_IO) = 
        let result = midiOutClose(handle)
        printfn "%A" result
        ()