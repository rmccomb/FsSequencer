module Native 

    open System
    open System.Runtime.InteropServices

    // MIDI in messages
    type MidiInMessage = 
        | MIM_OPEN = 0x3C1
        | MIM_CLOSE = 0x3C2
        | MIM_DATA = 0x3C3
        | MIM_LONGDATA = 0x3C4
        | MIM_ERROR = 0x3C5
        | MIM_LONGERROR = 0x3C6
        | MIM_MOREDATA = 0x3CC

    // MIDI-specific return codes
    type MMRESULT =
        | MIDIERR_BASE = 64
        | MIDIERR_UNPREPARED = 64
        | MIDIERR_STILLPLAYING = 65
        | MIDIERR_NOMAP = 66
        | MIDIERR_NOTREADY = 67
        | MIDIERR_NODEVICE = 68
        | MIDIERR_INVALIDSETUP = 69
        | MIDIERR_BADOPENMODE = 70
        | MIDIERR_DONT_CONTINUE = 71
        | MIDIERR_LASTERROR = 72

    // Win32 handle for a MIDI output device
    [<StructLayout(LayoutKind.Sequential)>]
    type HMIDI_IO = 
        struct
            val handle: IntPtr
        end

    // MIDI output device capabilities structure
    [<StructLayout(LayoutKind.Sequential)>]
    type MIDIOUTCAPS = 
        struct
            val handle: UInt16
            val wPid: UInt16
            val vDriverVersion: UInt32
            [< MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32) >] 
            val szPname: String
            val wTechnology: UInt16
            val wVoices: UInt16
            val wNotes: UInt16
            val dwSupport: UInt32
        end

    // MIDI input device capabilities structure
    [<StructLayout(LayoutKind.Sequential)>]
    type MIDIINCAPS =
        struct
            val wMid: UInt16
            val wPid: UInt16
            val vDriverVersion: UInt32
            [< MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32) >]
            val szPname: String
            val dwSupport: UInt32
        end

    // Callback invoked when a MIDI event is received or device opened or closed
    // http://msdn.microsoft.com/en-us/library/ms711612(VS.85).aspx
    //delegate void MidiInProc(HMIDIIN hMidiIn, MidiInMessage wMsg, UIntPtr dwInstance, UIntPtr dwParam1, UIntPtr dwParam2);
    type MidiCallbackProc = delegate of HMIDI_IO * MidiInMessage * UIntPtr * UIntPtr * UIntPtr -> unit


    // Returns the number of MIDI input devices available
    // http://msdn.microsoft.com/en-us/library/ms711608(VS.85).aspx
    [< DllImport("winmm.dll", SetLastError = true) >]
    extern UInt32 midiInGetNumDevs()

    // Return the number of MIDI output devices available
    // https://msdn.microsoft.com/en-us/library/windows/desktop/dd798472(v=vs.85).aspx
    [< DllImport("winmm.dll", SetLastError = true) >]
    extern UInt32 midiOutGetNumDevs()

    [< DllImport("winmm.dll", SetLastError = true) >]
    // https://msdn.microsoft.com/en-us/library/windows/desktop/dd798453(v=vs.85).aspx
    extern MMRESULT midiInGetDevCaps(UIntPtr uDeviceId, [<Out>] MIDIINCAPS& caps, UInt32 cbMidiInCaps)

    // https://msdn.microsoft.com/en-us/library/windows/desktop/dd798469(v=vs.85).aspx
    [< DllImport("winmm.dll", SetLastError = true) >]
    extern MMRESULT midiOutGetDevCaps(UIntPtr uDeviceId, [<Out>] MIDIOUTCAPS& caps, UInt32 cbMidiOutCaps)

    // Opens a MIDI input device
    // NOTE: This is adapted from the original Win32 function in order to make it typesafe.
    //public static MMRESULT midiInOpen(out HMIDIIN lphMidiIn, UIntPtr uDeviceId,
    //    MidiInProc dwCallback, UIntPtr dwCallbackInstance)
    //{
    //    return midiInOpen(out lphMidiIn, uDeviceId, dwCallback, dwCallbackInstance,
    //        dwCallback == null ? MidiOpenFlags.CALLBACK_NULL : MidiOpenFlags.CALLBACK_FUNCTION);
    //}

    // Opens a MIDI output device for playback
    // http://msdn.microsoft.com/en-us/library/ms711610(VS.85).aspx
    [< DllImport("winmm.dll", SetLastError = true) >]
    extern MMRESULT midiInOpen([<Out>] HMIDI_IO& lphMidiIn, UIntPtr uDeviceId,
        MidiCallbackProc dwCallback, UIntPtr dwCallbackInstance, UInt64 dwFlags);

    // Starts input on a MIDI input device
    // http://msdn.microsoft.com/en-us/library/ms711614(VS.85).aspx
    [< DllImport("winmm.dll", SetLastError = true) >]
    extern MMRESULT midiInStart(HMIDI_IO hMidiIn)

    // Stops input on a MIDI input device
    // http://msdn.microsoft.com/en-us/library/ms711615(VS.85).aspx
    [< DllImport("winmm.dll", SetLastError = true) >]
    extern MMRESULT midiInStop(HMIDI_IO hMidiIn)


    // Opens a MIDI output device for playback
    // https://msdn.microsoft.com/en-us/library/windows/desktop/dd798476(v=vs.85).aspx
    [< DllImport("winmm.dll", SetLastError = true) >]
    extern MMRESULT midiOutOpen(
        [<Out>] HMIDI_IO& lphmo, 
        UIntPtr uDeviceId, 
        MidiCallbackProc dwCallback, 
        UIntPtr dwCallbackInstance, 
        UInt32 dwFlags)

    // Sends a short MIDI message (not sysex or stream)
    // http://msdn.microsoft.com/en-us/library/ms711640(VS.85).aspx
    [< DllImport("winmm.dll", SetLastError = true) >]
    extern MMRESULT midiOutShortMsg(HMIDI_IO hmo, UInt64 dwMsg)

    // Sends a long MIDI message (sysex)
    // http://msdn.microsoft.com/en-us/library/ms711640(VS.85).aspx
    [< DllImport("winmm.dll", SetLastError = true) >]
    extern MMRESULT midiOutLongMsg(HMIDI_IO hmo, IntPtr lpMidiOutHdr, UInt32 cbMidiOutHdr)


    // Turn off all notes
    // http://msdn.microsoft.com/en-us/library/dd798479(VS.85).aspx
    [< DllImport("winmm.dll", SetLastError = true) >]
    extern MMRESULT midiOutReset(HMIDI_IO hmo)

    // Closes a MIDI input device
    // Win32 docs: http://msdn.microsoft.com/en-us/library/ms711602(VS.85).aspx
    [< DllImport("winmm.dll", SetLastError = true) >]
    extern MMRESULT midiInClose(HMIDI_IO hMidiIn)

    // Closes a MIDI output device.
    // http://msdn.microsoft.com/en-us/library/ms711620(VS.85).aspx
    [< DllImport("winmm.dll", SetLastError = true) >]
    extern MMRESULT midiOutClose(HMIDI_IO hmo)

