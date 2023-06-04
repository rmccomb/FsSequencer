using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCs
{
    class Program
    {
        static void Main( )
        {
            var chan = 1;
            var pitch = 64;
            var velocity = 127;
            var msg = EncodeNoteOn(chan, pitch, velocity);
            Debug.WriteLine(string.Format("{0:x}", msg));

            DecodeNoteOn(msg, out int chan2, out int pitch2, out int velocity2);
            Debug.Assert(chan == chan2);
            Debug.Assert(pitch == pitch2);
            Debug.Assert(velocity == velocity2);
        }

        public static void DecodeNoteOn(uint dwParam1, 
            out int channel, out int pitch, out int velocity)
        {
            channel = (int)dwParam1 & 0x0f;
            pitch = ((int)dwParam1 & 0xff00) >> 8;
            velocity = ((int)dwParam1 & 0xff0000) >> 16;
        }

        public static uint EncodeNoteOn(int channel, int pitch, int velocity)
        {
            return (uint)(0x90 | channel | (pitch << 8) | (velocity << 16));
        }
    }
}
