using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS_6502
{
    internal class _6502
    {
        readonly RAM_64KB _ram;
        public RAM_64KB RAM { get { return _ram; } }

        byte _opcode;
        // Last opcode
        public byte Opcode { get { return _opcode; } }
        byte _data;
        // First byte after last opcode
        public byte OpcodeData { get { return _data; } }
        ushort _address;
        // Combined address value after last opcode
        public ushort OpcodeAddress { get { return _address; } }

        #region Rejestry
        // Registers
        byte _a;
        // Accumulator
        public byte A { get { return _a; } }
        
        byte _x;
        // Index register X
        public byte X { get { return _x; } }
        
        byte _y;
        // Index register Y
        public byte Y { get { return _y; } }
        
        byte _sp;
        // Stack Pointer
        public byte SP { get { return _sp; } }
        
        ushort _pc;
        // Program Counter
        public ushort PC { get { return _pc; } }
        #endregion

        #region Flagi
        // Flags
        bool _carry;    //0x1
        bool _zero;     //0x2
        bool _interrupt;//0x4
        bool _decimal;  //0x8
        //bool _break;  //0x10 only exists on stack
        //bool _unused; //0x20
        bool _overflow; //0x40
        bool _negative; //0x80

        // Carry flag
        public bool Carry { get { return _carry; } }

        // Zero flag
        public bool Zero { get { return _zero; } }

        // Interrupt disable flag
        public bool Interrupt { get { return _interrupt; } }

        // Decimal flag
        public bool Decimal { get { return _decimal; } }

        // Overflow flag
        public bool Overflow { get { return _overflow; } }

        // Sign flag
        public bool Negative { get { return _negative; } }
        #endregion

        byte _status // Status register
        {
            get
            {
                return (byte)
                    ((_carry ? 0x1 : 0) |
                    (_zero ? 0x2 : 0) |
                    (_interrupt ? 0x4 : 0) |
                    (_decimal ? 0x8 : 0) |
                    0x10 | //(_break ? 0x10 : 0) |
                    0x20 |
                    (_overflow ? 0x40 : 0) |
                    (_negative ? 0x80 : 0));
            }
            set
            {
                _carry = (value & 0x1) != 0;
                _zero = (value & 0x2) != 0;
                _interrupt = (value & 0x4) != 0;
                _decimal = (value & 0x8) != 0;
                //_break = (value & 0x10) != 0;
                _overflow = (value & 0x40) != 0;
                _negative = (value & 0x80) != 0;
            }
        }
        public byte Status { get { return _status; } }

        bool _nmi;
        bool _irq;
        bool _reset;

        bool _jam; // Returns true if the cpu is jammed
        public bool Jam { get { return _jam; } }

        ulong _cycles; // Cycle counter. Returns the amount of cycles since the last reset.
        public ulong Cycles { get { return _cycles; } }

        public _6502(RAM_64KB ram)
        {
            _ram = ram;
        }
    }
}
