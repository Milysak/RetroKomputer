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

        public byte this[ushort address]
        {
            get { return Read(address); }
            set { Write(address, value); }
        }

        public virtual byte Read(ushort address)
        {
            return _ram[address];
        }

        public ushort Read16(ushort address)
        {
            byte a = Read(address);
            byte b = Read(++address);
            return (ushort)((b << 8) | a);
        }
        // "<<" i ">>" to przesunięcia bitowe
        // np. 1001 0100 1100 0110 << 8
        // to będzie: 1100 0110 0000 0000 
        // czyli 8 od lewej znika a od prawej popisuje się 8 zer

        public virtual void Write(ushort address, byte value)
        {
            _ram[address] = value;
        }

        public void Write16(ushort address, ushort value)
        {
            Write(address, (byte)(value & 0xFF));
            Write(++address, (byte)(value >> 8));
        }
    }
}
