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
        // булево значение
        private bool bool_val;
        // тип данных
        Data_type data_type_;
        // строковое значение
        public string str_val;
        public Token() {
            reset();
        }
        public void reset() {
            Stt = TOK_EOT;
            int_val = 0;
            dbl_val = 0;
            data_type_ = STDT_NULL;
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
        public bool Bool_val {
            get {
                return bool_val;
            }
            set {
                bool_val = value;
            }
        }
        public Data_type Data_Type {
            get {
                return data_type_;
            }
            set {
                data_type_ = value;
            }
        }
    }
}
