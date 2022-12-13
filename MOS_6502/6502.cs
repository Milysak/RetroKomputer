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

        // Opcode:
        /* jest to liczba - np. 0x60, będąca fragmentem rozkazu przekazywanego do wykonania do procesora, 
         * która informuje jaka operacja ma być wykonana. Każde polecenie asemblera jak add, sub, itd. posiada swój numer, 
         * na który jest zamieniane podczas kompilacji do kodu maszynowego.
         * 
         * Tablica instrukcji 6502 jest ułożona zgodnie ze wzorem a-b-c, gdzie 
         * a i b to liczby ósemkowe, po których następuje grupa dwóch cyfr binarnych c,
         * jak w wektorze bitowym "aaabbbcc".
         * np. z przedziałów: a = <0;7>, b = <0;7>, c = <0;2>, bierzemy: a = 6, b = 4, c = 0...
         * 3 pierwsze cyfry w wektorze to ósemkowe 'a' zapisane binarnie, czyli: 6 -> 110, wektor: 110bbbcc
         * następne 3 to 'b', 4 -> 100, wektor: 110100cc
         * i na końcu 'c', 0 -> 00, wektor: 11010000.
         * Po przekształceniu na szesnastkowy: 11010000 -> 0xD0 - na podstawie tego opcode możliwe jest
         * wywołanie: BNE rel - Branch on Result, relative, które znajduje się w tabeli pod 
         * podanymi wyżej wartościami a, b oraz c.
         */

        byte _opcode;
        // Ostatni opcode
        public byte Opcode { get { return _opcode; } }

        byte _data;
        // Pierwszy bajt po ostatnim opcode
        public byte OpcodeData { get { return _data; } }

        ushort _address;
        // Adres po ostatnim opcode
        public ushort OpcodeAddress { get { return _address; } }

        #region Rejestry
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

        #region Flagi Rejestru
        // Flagi:
        /* Rejestr procesora opisujący i kontrolujący jego stan. 
         * Zawartość tego rejestru może zależeć od ostatnio wykonanej operacji (zmiana pośrednia), 
         * bądź trybu pracy procesora, który można ustawiać (zmiana bezpośrednia).
         */
        bool _carry;    //0x1 - 0000 0001
        bool _zero;     //0x2 - 0000 0010
        bool _interrupt;//0x4 - 0000 0100
        bool _decimal;  //0x8 - 0000 1000
        //bool _break;  //0x10 - tylko na stosie
        //bool _unused; //0x20 - nie używany
        bool _overflow; //0x40 - 0100 0000
        bool _negative; //0x80 - 1000 0000

        // Carry flag - wskazuje, że wynik operacji zawiera się w większej niż dostępna liczbie bitów.
        public bool Carry { get { return _carry; } }

        // Zero flag - wskazuje, że wynikiem operacji arytmetycznej bądź logicznej było zero.
        public bool Zero { get { return _zero; } }

        // Interrupt disable flag - ustawienie lub wyczyszczenie flagi umożliwia włączenie bądź wyłączenie przerwań.
        public bool Interrupt { get { return _interrupt; } }

        // Decimal flag - po ustawieniu niektóre instrukcje działają w trybie dziesiętnym, a nie binarnym.
        public bool Decimal { get { return _decimal; } }

        // Overflow flag - wskazuje, czy wynik operacji powoduje przepełnienie reprezentacji słowa procesora; flaga podobna do flagi przeniesienia, jednak odnosi się do operacji ze znakiem.
        public bool Overflow { get { return _overflow; } }

        // Negative flag - wskazuje, że wynik operacji arytmetycznej był ujemny.
        public bool Negative { get { return _negative; } }
        #endregion

        byte _status // Status flag rejestru
        {
            get
            {
                return (byte)
                    ((_carry ? 0x1 : 0) |
                    (_zero ? 0x2 : 0) |
                    (_interrupt ? 0x4 : 0) |
                    (_decimal ? 0x8 : 0) |
                    0x10 |
                    0x20 |
                    (_overflow ? 0x40 : 0) |
                    (_negative ? 0x80 : 0));
                /* zwraca wektor odpowiadający rejestrowi:
                 * Jeśli jakaś flaga jest ustawiona to zwraca kod odpowiadający tej fladze, np. dla carry -> 0x1,
                 * wszystkie te flagi są logicznie sumowane i zwracane jako stan rejestru flag, czyli:
                 * jeśli mamy ustawione flagi: Negative, Zero i Carry, to dla tych flag zwróci: 0x1, 0x2 i 080 i sumuje,
                 * co najlepiej pokazać na zwracanych ośmiu bitach:
                 * 0x1 -> 0000 0001
                 * 0x2 -> 0000 0010
                 * 0x80 -> 1000 0000
                 * Po zsumowaniu: 1000 0011 - czyli pierwsza jedynka odpowiada za Negative, druga za Zero a trzecia za Carry.
                 */
            }
            set
            {
                _carry = (value & 0x1) != 0;
                _zero = (value & 0x2) != 0;
                _interrupt = (value & 0x4) != 0;
                _decimal = (value & 0x8) != 0;
                _overflow = (value & 0x40) != 0;
                _negative = (value & 0x80) != 0;
            }
        }
        public byte Status { get { return _status; } }

        bool _nmi; // Non-Maskable Interrupt
        bool _irq; // Interrupt Request
        bool _reset;

        bool _jam; // Prawda jeśli procesor jest jammed - zawieszony
        public bool Jam { get { return _jam; } }

        ulong _cycles; // Cycle
        public ulong Cycles { get { return _cycles; } }

        public _6502(RAM_64KB ram)
        {
            _ram = ram;
        }
    }
}
