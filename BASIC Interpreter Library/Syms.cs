using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BASIC_Interpreter_Library.InterpreterSymbol;
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
            st[0].StrVal = "$";
        }
        public int Size => st.Count - 1;
        // возвращает номер символа в таблице
        public long Find(ref Token tok) {
            for (long i = 1; i < Size + 1; i++) {
                if (tok.Name == st[(int)i].Name) {
                    tok = (Token)st[(int)i].Clone();
                    return i;
                }
            }
            return ST_NOTFOUND;
        }
        // вставляет символ в таблицу
        public long Insert(Token tok) {
            Token temp = (Token)tok.Clone();
            if (Find(ref temp) == ST_NOTFOUND) {
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
