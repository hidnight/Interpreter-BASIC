using static BASIC_Interpreter_Library.Constants;
using static BASIC_Interpreter_Library.InterpreterSymbol;
using static BASIC_Interpreter_Library.DataType;
using System;

namespace BASIC_Interpreter_Library {
    public class Token : ICloneable {
        public Token() {
            Reset();
        }
        public void Reset() {
            Stt = TOK_EOT;
            IntVal = 0;
            DblVal = 0;
            BoolVal = false;
            DataType = STDT_NULL;
            LineNumber = 0;
            StrVal = "";
            Name = "";
        }

        // добавляет символ
        public bool Append(char c) {
            int len = StrVal.Length;
            if (len <= MAX_QUOTE) {
                StrVal += c;
                return true;
            }
            return false;
        }

        public object Clone() {
            return MemberwiseClone();
        }

        public InterpreterSymbol Stt {
            get;
            set;
        }
        public long IntVal {
            get;
            set;
        }
        public double DblVal {
            get;
            set;
        }
        public bool BoolVal {
            get;
            set;
        }
        public string StrVal {
            get;
            set;
        }
        public DataType DataType {
            get;
            set;
        }
        public ulong LineNumber {
            get;
            set;
        }
        public string Name {
            get;
            set;
        }
    }
}
