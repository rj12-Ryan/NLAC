using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static NLAC.LUT;

namespace NLAC
{
    //Data structure that stores information on LUTS (constructed with current info on key/mode/etc & contains static information on LUT notes)
    public class LUT
    {
        public LUT(LUT.Keys key, LUT.Modes mode)
        {
            _currentKey = key;
            KeyName = key.ToString();
            _currentMode = mode;
        }

        public string KeyName;

        public int Size()
        {
            switch (_currentKey)
            {
                case Keys.A: return _A.Count;
                case Keys.B: return _B.Count;
                case Keys.C: return _C.Count;
                case Keys.D: return _D.Count;
                case Keys.E: return _E.Count;
                case Keys.F: return _F.Count;
                case Keys.Chromatic: return _Chromatic.Count;
                default: throw new Exception("Invalid LUT Set");
            }
        }
        public List<String> GetNotes()
        {
            switch (_currentKey)
            {
                case Keys.A: return _A; 
                case Keys.B: return _B; 
                case Keys.C: return _C; 
                case Keys.D: return _D; 
                case Keys.E: return _E; 
                case Keys.F: return _F; 
                case Keys.Chromatic: return _Chromatic;
                default: throw new Exception("Invalid LUT Set");
            }
        }

        LUT.Keys _currentKey;
        public enum Keys
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            Chromatic
        }

        private Modes _currentMode;
        public enum Modes
        {
            Major,
            Minor,
            Chromatic
        }

        private List<string> _A = new List<string> { "A,,,", "B,,,", "C,,", "D,,", "E,,", "F,,", "G,,", "A,,", "B,,", "C,", "D,", "E,", "F,", "G,", "A,", "B,", "C", "D", "E", "F", "G", "A", "B", "c", "d", "e", "f", "g", "a", "b", "c'", "d'", "e'", "f'", "g'", "a'", "b'", "c''", "d''", "e''", "f''", "g''", "a''", "b''", "c'''", "d'''", "e'''", "f'''", "g'''", "a'''", "b'''", "c''''", "d''''", "e''''", "f''''" };
        private List<string> _B = new List<string> { "B,,,", "C,,", "D,,", "E,,", "F,,", "G,,", "A,,", "B,,", "C,", "D,", "E,", "F,", "G,", "A,", "B,", "C", "D", "E", "F", "G", "A", "B", "c", "d", "e", "f", "g", "a", "b", "c'", "d'", "e'", "f'", "g'", "a'", "b'", "c''", "d''", "e''", "f''", "g''", "a''", "b''", "c'''", "d'''", "e'''", "f'''", "g'''", "a'''", "b'''", "c''''", "d''''", "e''''", "f''''", "g''''" };
        private List<string> _C = new List<string> { "C,,,", "D,,,", "E,,,", "F,,,", "G,,,", "A,,,", "B,,,", "C,,", "D,,", "E,,", "F,,", "G,,", "A,,", "B,,", "C,", "D,", "E,", "F,", "G,", "A,", "B,", "C", "D", "E", "F", "G", "A", "B", "c", "d", "e", "f", "g", "a", "b", "c'", "d'", "e'", "f'", "g'", "a'", "b'", "c''", "d''", "e''", "f''", "g''", "a''", "b''", "c'''", "d'''", "e'''", "f'''", "g'''", "a'''" };
        private List<string> _D = new List<string> { "D,,,", "E,,,", "F,,,", "G,,,", "A,,,", "B,,,", "C,,", "D,,", "E,,", "F,,", "G,,", "A,,", "B,,", "C,", "D,", "E,", "F,", "G,", "A,", "B,", "C", "D", "E", "F", "G", "A", "B", "c", "d", "e", "f", "g", "a", "b", "c'", "d'", "e'", "f'", "g'", "a'", "b'", "c''", "d''", "e''", "f''", "g''", "a''", "b''", "c'''", "d'''", "e'''", "f'''", "g'''", "a'''", "b'''" };
        private List<string> _E = new List<string> { "E,,,", "F,,,", "G,,,", "A,,,", "B,,,", "C,,", "D,,", "E,,", "F,,", "G,,", "A,,", "B,,", "C,", "D,", "E,", "F,", "G,", "A,", "B,", "C", "D", "E", "F", "G", "A", "B", "c", "d", "e", "f", "g", "a", "b", "c'", "d'", "e'", "f'", "g'", "a'", "b'", "c''", "d''", "e''", "f''", "g''", "a''", "b''", "c'''", "d'''", "e'''", "f'''", "g'''", "a'''", "b'''", "c''''" };
        private List<string> _F = new List<string> { "F,,,", "G,,,", "A,,,", "B,,,", "C,,", "D,,", "E,,", "F,,", "G,,", "A,,", "B,,", "C,", "D,", "E,", "F,", "G,", "A,", "B,", "C", "D", "E", "F", "G", "A", "B", "c", "d", "e", "f", "g", "a", "b", "c'", "d'", "e'", "f'", "g'", "a'", "b'", "c''", "d''", "e''", "f''", "g''", "a''", "b''", "c'''", "d'''", "e'''", "f'''", "g'''", "a'''", "b'''", "c''''", "d''''" };
        private List<string> _G = new List<string> { "G,,,", "A,,,", "B,,,", "C,,", "D,,", "E,,", "F,,", "G,,", "A,,", "B,,", "C,", "D,", "E,", "F,", "G,", "A,", "B,", "C", "D", "E", "F", "G", "A", "B", "c", "d", "e", "f", "g", "a", "b", "c'", "d'", "e'", "f'", "g'", "a'", "b'", "c''", "d''", "e''", "f''", "g''", "a''", "b''", "c'''", "d'''", "e'''", "f'''", "g'''", "a'''", "b'''", "c''''", "d''''", "e''''" };
        private List<string> _Chromatic = new List<string> { "C,,,", "^C,,,", "D,,,", "^D,,,", "E,,,", "F,,,", "^F,,,", "G,,,", "^G,,,", "A,,,", "^A,,,", "B,,,", "C,,", "^C,,", "D,,", "^D,,", "E,,", "F,,", "^F,,", "G,,", "^G,,", "A,,", "^A,,", "B,,", "C,,", "^C,,", "D,,", "^D,,", "E,,", "F,,", "^F,,", "G,,", "^G,,", "A,,", "^A,,", "B,,", "C,", "^C,", "D,", "^D,", "E,", "F,", "^F,", "G,", "^G,", "A,", "^A,", "B,", "C", "^C", "D", "^D", "E", "F", "^F", "G", "^G", "A", "^A", "B", "c", "^c", "d", "^d", "e", "f", "^f", "g", "^g", "a", "^a", "b", "c'", "^c'", "d'", "^d'", "e'", "f'", "^f'", "g'", "^g'", "a'", "^a'", "b'", "c''", "^c''", "d''", "^d''", "e''", "f''", "^f''", "g''", "^g''", "a''", "^a''", "b''", "c'''", "^c'''", "d'''", "^d'''", "e'''", "f'''", "^f'''", "g'''", "^g'''", "a'''", "^a'''" };
    }
}
