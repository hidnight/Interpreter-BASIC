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
            st.Add(new Token());
            st[0].Stt = TOK_EOT;
            st[0].str_val = "$";
        }
        public int Size => st.Count - 1;
        // возвращает номер символа в таблице
        public int find(Token tok) {
            for (int i = 1; i < Size + 1; i++) {
                if (tok.str_val == st[i].str_val) {
                    return i;
                }
            }
            return ST_NOTFOUND;
        }
        // вставляет символ в таблицу
        public int insert(Token tok) {
            if (find(tok) == ST_NOTFOUND) {
                st.Add(tok);
                if (depth < Size + 1)
                    depth = Size + 1;
                return Size + 1;
            } else {
                return ST_EXISTS;
            }
        }
        public Token this[int index] => st[index];
    }
}
