using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS_6502
{
    internal class RAM_64KB
    {
        readonly byte[] _ram;

        public RAM_64KB()
        {
            _ram = new byte[0x10000]; // 65536 B = 64 KB
            for (int i = 0; i < _ram.Length; i++)
                _ram[i] = 0xFF; // na każdy bajt przypada 8 bitów - 0xFF to 1111 1111
        }

        public byte Read(ushort address)
        {
            return _ram[address];
        }

        public void Write(ushort address, byte value)
        {
            _ram[address] = value;
        }
    }
}
