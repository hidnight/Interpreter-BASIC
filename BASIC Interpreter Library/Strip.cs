using System.Collections.Generic;
using System.Text;
using static BASIC_Interpreter_Library.Constants;
using static BASIC_Interpreter_Library.Interpreter_symbol;

namespace BASIC_Interpreter_Library {
    public class Strip {
        private List<Token> st;
        // счетчик меток
        private int label;
        // глубина
        private int depth;
        public int Size => st.Count - 1;
        public Strip() {
            st = new List<Token> {
                new Token()
            };
            st[0].Stt = OUT_START;
        }
        // очищает объект
        public void Reset() {
            depth = label = 0;
            st.Clear();
        }
        // возвращает новый идентификатор метки
        public int New_label() {
            return ++label;
        }
        public Token this[int index] => st[index];
        public void Add(Token e) {
            st.Add((Token)e.Clone());
            if (depth < Size + 1) {
                depth = Size + 1;
            }
        }
        // ищет на ленте определение метки с идентификатором id
        public int Find_DEF(long id) {
            for (int i = 1; i < Size + 1; i++) {
                if (st[i].Stt == OUT_END)
                    return -1; // конец ленты
                if (st[i].Stt == OUT_LABEL && st[i].Int_val == id) {
                    if (st[i + 1].Stt == OUT_DEFL) {
                        return (i + 2);
                    }
                }
            }
            return -1;
        }
        // вывод в текстовый файл
        override public string ToString() {
            StringBuilder builder = new StringBuilder();
            for (int i = 1; i < Size + 1; i++) {
                //fprintf(f, "%03d ", i);
                builder.Append(i.ToString("D4"));
                builder.Append(" " + st[i].Stt.ToString());
                switch (st[i].Stt) {
                case OUT_QUOTE:
                    builder.Append(" " + st[i].Str_val);
                    break;
                case OUT_ID:
                    builder.Append(" " + st[i].Name);
                    break;
                case OUT_I4:
                case OUT_LABEL:
                    builder.Append(" " + st[i].Int_val);
                    break;
                case OUT_R8:
                    builder.Append(" " + st[i].Dbl_val);
                    break;
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}
