﻿module Devices

    open Native
    open System

    let Eventide = "Eventide"
    let PC3K = "Kurzweil PC3K"

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

    let OpenOutputDevice (devices: (uint32 * string) list, name:string) :HMIDI_IO =
        let deviceId, _ = List.find (fun (_,n) -> n = name) devices
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