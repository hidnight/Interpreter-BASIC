using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BASIC_Interpreter_Library.Constants;
using static BASIC_Interpreter_Library.Interpreter_symbol;
using static BASIC_Interpreter_Library.Data_type;

namespace BASIC_Interpreter_Library {
    public class Token {
        // тип токена
        private Interpreter_symbol stt;
        // целочисленное значение
        private int int_val;
        // вещественное значение
        private double dbl_val;
        // тип данных
        Data_type data_type;
        // строковое значение
        public string str_val;
        public Token() {
            reset();
        }
        public void reset() {
            stt = TOK_EOT;
            int_val = 0;
            dbl_val = 0;
            data_type = STDT_NULL;
            str_val = "";
        }
        // добавляет символ
        public bool Append(char c) {
            int len = str_val.Length;
            if (len <= MAX_QUOTE) {
                str_val.Append(c);
                return true;
            }
            return false;
        }
        public Interpreter_symbol Stt {
            get {
                return stt;
            }
            set {
                stt = value;
            }
        }
        public int Int_val {
            get {
                return int_val;
            }
            set {
                int_val = value;
            }
        }
        public double Dbl_val {
            get {
                return dbl_val;
            }
            set {
                dbl_val = value;
            }
        }
    }
}
