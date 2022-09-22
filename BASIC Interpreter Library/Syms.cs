using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BASIC_Interpreter_Library.Interpreter_symbol;
using static BASIC_Interpreter_Library.Constants;

namespace BASIC_Interpreter_Library {
    public class Syms {
        private List<Token> st;
        // глубина
        private int depth;
        public Syms() {
            depth = 0;
            st = new List<Token>();
            st.Add(new Token());
            st[0].Stt = TOK_EOT;
            st[0].Str_val = "$";
        }
        public int Size => st.Count - 1;
        // возвращает номер символа в таблице
        public long find(ref Token tok) {
            for (long i = 1; i < Size + 1; i++) {
                if (tok.Name == st[(int)i].Name) {
                    tok.Data_Type = st[(int)i].Data_Type;
                    tok.Bool_val = st[(int)i].Bool_val;
                    tok.Dbl_val = st[(int)i].Dbl_val;
                    tok.Int_val = st[(int)i].Int_val;
                    tok.Str_val = st[(int)i].Str_val;
                    return i;
                }
            }
            return ST_NOTFOUND;
        }
        // вставляет символ в таблицу
        public long insert(Token tok) {
            Token temp = new Token() {
                Stt = tok.Stt,
                Int_val = tok.Int_val,
                Dbl_val = tok.Dbl_val,
                Str_val = tok.Str_val,
                Name = tok.Name,
                Bool_val = tok.Bool_val,
                Data_Type = tok.Data_Type,
                Line_Number = tok.Line_Number
            };
            if (find(ref temp) == ST_NOTFOUND) {
                st.Add(temp);
                if (depth < Size)
                    depth = Size;
                return Size;
            } else {
                return ST_EXISTS;
            }
        }
        public Token this[int index] => st[index];
    }
}
