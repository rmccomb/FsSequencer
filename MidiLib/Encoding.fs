module Encoding

    open System

    type ChannelEventType = 
        | NoteOff
        | NoteOn
        | PolyKeyPressure
        | ControlChange
        | ProgramChange
        | ChannelPressure
        | PitchBendChange
        | BadType

    type ChannelMessage =
        { Type: ChannelEventType;
          Channel: int
          Pitch: int
          Velocity: int
          Timestamp: UInt32; }    

    let GetType status =
        match status &&& 0xF0 with
        | 0x08 -> Ok NoteOff
        | 0x09 -> Ok NoteOn
        | 0x10 -> Ok PolyKeyPressure
        | 0x11 -> Ok ControlChange
        | 0x12 -> Ok ProgramChange
        | 0x13 -> Ok ChannelPressure
        | 0x14 -> Ok PitchBendChange
        | _ -> Error "not a channel message"

    let GetChannel status =
        status &&& 0x0F

    let EncodeNoteOn chan note vel =
        0x90 ||| chan ||| note <<< 0x08 ||| vel <<< 0x16

    let EncodeNoteOff chan note vel =
        0x80 ||| chan ||| note <<< 0x08 ||| vel <<< 0x16
        
    let DecodeMessage param1 param2 =
        match GetType param1 with
        | Ok t -> Some({Type = t;
                        Channel = param1 &&& 0x0F; 
                        Pitch = (param1 &&& 0xFF00) >>> 0x08;
                        Velocity = (param1 &&& 0xFF0000) >>> 0x10;
                        Timestamp = param2; })
        | Error e -> None


        
