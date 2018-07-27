module Encoding

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
          Timestamp: uint32; }    

    let GetChannel status =
        status &&& 0x0F

    let EncodeNoteOn (chan:int, note:int, vel:int) :uint32 =
        uint32((0x90 ||| chan) ||| (note <<< 08) ||| (vel <<< 16))

    let EncodeNoteOff (chan:int, note:int, vel:int) :uint32 =
        uint32((0x80 ||| chan) ||| (note <<< 08) ||| (vel <<< 16))
     
    let GetType (status) =
        match (int)status &&& 0xF0 with
        | 0x80 -> Ok NoteOff
        | 0x90 -> Ok NoteOn
        | 0xA0 -> Ok PolyKeyPressure
        | 0xB0 -> Ok ControlChange
        | 0xC0 -> Ok ProgramChange
        | 0xD0 -> Ok ChannelPressure
        | 0xE0 -> Ok PitchBendChange
        | _ -> Error BadType
   
    let DecodeMessage (param1:uint32, param2 :uint32) =
        match GetType param1 with
        | Ok t -> Some({ Type = t;
                         Channel = (int)(param1 &&& 0x0Fu); 
                         Pitch = (int)(param1 &&& 0xFF00u) >>> 0x08;
                         Velocity = (int)(param1 &&& 0xFF0000u) >>> 0x10;
                         Timestamp = param2; })
        | Error e -> None


        
