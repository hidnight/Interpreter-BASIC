using static BASIC_Interpreter_Library.Constants;
using static BASIC_Interpreter_Library.Interpreter_symbol;
using static BASIC_Interpreter_Library.Data_type;

namespace BASIC_Interpreter_Library {
    public class Token {
        public Token() {
            Reset();
        }
        public void Reset() {
            Stt = TOK_EOT;
            Int_val = 0;
            Dbl_val = 0;
            Data_Type = STDT_NULL;
            Line_Number = 0;
            Str_val = "";
        }

        // добавляет символ
        public bool Append(char c) {
            int len = Str_val.Length;
            if (len <= MAX_QUOTE) {
                Str_val += c;
                return true;
            }
            return false;
        }
        public Interpreter_symbol Stt {
            get;
            set;
        }
        public long Int_val {
            get;
            set;
        }
        public double Dbl_val {
            get;
            set;
        }
        public bool Bool_val {
            get;
            set;
        }
        public string Str_val {
            get;
            set;
        }
        public Data_type Data_Type {
            get;
            set;
        }
        public ulong Line_Number {
            get;
            set;
        }
    }
}
