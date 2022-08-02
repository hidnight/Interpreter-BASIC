using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BASIC_Interpreter_Library.Constants;
using static BASIC_Interpreter_Library.Interpreter_symbol;

namespace BASIC_Interpreter_Library {
    /* LL */
    public class Syntaxan {
        // входной поток
        private FileStream parse_stream;
        // поток ошибок
        private FileStream error_stream;
        // лексический анализатор
        private Lexan lex;
        // текущий токен
        Token tok;
        // стек меток
        private Stack<int> stl = new Stack<int>(MAX_LABEL);
        // стек МП-автомата
        private Stack<short> sta = new Stack<short>(MAX_STACK);
        // таблица символов
        Syms syms;
        // исполняющий стек
        ExStack exe;
        // лента ПОЛИЗ
        Strip strip;
        // конструктор
        public Syntaxan(ref Lexan lex, FileStream parse_stream, FileStream error_stream) {
            this.lex = lex;
            this.parse_stream = parse_stream;
            this.error_stream = error_stream;
        }
        // LL(1) трансляция в ПОЛИЗ
        public int parse() {
            if (next_token() == 0)
                return 0;
            // вспомогательные переменные
            // i - итератор
            // n - длина правила
            int i, n;
            Interpreter_symbol at = TOK_EOT, bt = TOK_EOT;
            // символ на стеке
            Interpreter_symbol s = TOK_EOT;
            // значение из управляющей таблицы
            Interpreter_symbol t = TOK_EOT;
            // предварительные действия
            // уточнение длины правил
            get_rule_len();
            // проталкивание дна стека
            sta.push((byte)TOK_EOT);
            // проталкивание аксиомы
            sta.push((byte)START);
            // работа LL автомата
            while (true) {
                // состояние на стеке
                s = (Interpreter_symbol)clear_stack();
                // получение управляющего значения
                t = (Interpreter_symbol)SYNTA[(int)s][(int)tok.Stt];
                // анализ управляющего значения
                if (t <= 0) {
                    // ошибка
                    error_stream.Write(Encoding.ASCII.GetBytes("\nsyntaxan: failure synta =" + t + ".\n\n"), 0, 255);
                    return 0;
                } else {
                    if ((int)t == ACC) {
                        // допуск
                        break;
                    } else {
                        if ((int)t == POP) {
                            // выброс
                            at = (Interpreter_symbol)sta.pop();
                            // выталкиваем операционные символы
                            clear_stack();
                            // следующий токен
                            if (next_token() == 0)
                                return 0;
                            return 0;
                        } else {
                            if ((int)t <= MAX_RULE) {
                                // правило
                                at = (Interpreter_symbol)sta.pop();
                                n = RLEN[(int)t];
                                for (i = n - 1; i >= 0; i--) {
                                    bt = RULE[(int)t][i];
                                    sta.push((short)bt);
                                }
                                parse_stream.Write(Encoding.ASCII.GetBytes("\nRule " + t), 0, 255);
                                /*for (int i = 0; i < n; i++) {
                                    fprintf(parse_stream, "%d ", bt = RULE[t][i]);
                                }*/
                                parse_stream.Write(Encoding.ASCII.GetBytes("\n"), 0, 1);
                            } else {
                                // ошибка управляющей таблицы
                                throw new Exception("syntaxan: invalid SYNTA table");
                            }
                        }
                    }
                }
            }
            // записываем конец ленты ПОЛИЗ
            out_(OUT_END);
            // выводим содержимое ленты ПОЛИЗ
            parse_stream.Write(Encoding.ASCII.GetBytes(strip.ToString()), 0, strip.ToString().Length);
            execute();
            parse_stream.Write(Encoding.ASCII.GetBytes("\nsyntaxan: success.\n\n"), 0, 255);
            return 1;
        }
        // запись символа на ленту ПОЛИЗ
        void out_(Interpreter_symbol tt) {
            int i = 0, j = 0;
            Token t = tok;
            t.Stt = tt;
            switch (tt) {
            case OUT_PUSH: {
                j = strip.new_label();
                stl.push(j);
                t.Int_val = j;
                t.Stt = OUT_LABEL;
                break;
            }
            case OUT_POPL: {
                j = stl.pop();
                t.Int_val = j;
                t.Stt = OUT_LABEL;
                break;
            }
            case OUT_SWAP: {
                i = stl.pop();
                j = stl.pop();
                stl.push(i);
                t.Int_val = j;
                t.Stt = OUT_LABEL;
                break;
            }
            }
            strip.Add(t);
        }
        // выводит операционные символы со стека
        short clear_stack() {
            short s;
            while ((s = sta.Top) > (short)SYM_LAST) {
                s = sta.pop();
                out_((Interpreter_symbol)s);
            }
            return s;
        }
        // исполняющая машина ПОЛИЗ
        void execute() {
            // счетчик операций ПОЛИЗ
            int it_counter = 1;
            // указатель ленты ПОЛИЗ
            int strip_pointer = 1;
            // текущий элемент ПОЛИЗ
            Interpreter_symbol stt;
            // переменные для вычислений
            Token X = new Token(), Y = new Token();
            // вспомогательные
            int j = 0;
            // рабочий цикл выполнения ленты ПОЛИЗ
            while (true) {
                // очистка переменных
                X.reset();
                Y.reset();
                // тип элемента ленты
                stt = strip[strip_pointer].Stt;
                switch (stt) {
                case OUT_END: {
                    error_stream.Write(Encoding.ASCII.GetBytes("EXE DONE.\n\n"), 0, 255);
                    return;
                }
                case OUT_ID:
                case OUT_I4:
                case OUT_LONG: {
                    // проталкивание в стек
                    Token e = strip[strip_pointer];
                    exe.push(ref e);
                    // следующий элемент ленты
                    strip_pointer++;
                    break;
                }
                case OUT_DIM: {
                    exe.pop(ref Y);
                    exe.pop(ref X);
                    j = syms.insert(X);
                    if (j == ST_EXISTS) {
                        throw new Exception("exe duplicate declaration");
                    }
                    syms[j].Data_Type = Y.Data_Type;
                    strip_pointer++;
                    break;
                }
                case OUT_ASS: {
                    exe.pop(ref Y);
                    exe.pop(ref X);
                    j = syms.find(X);
                    if (j == ST_NOTFOUND) {
                        throw new Exception("exe_pop identifier not found");
                    }
                    syms[j].Int_val = Y.Int_val;
                    strip_pointer++;
                    break;
                }
                case OUT_ADD:
                case OUT_SUB:
                case OUT_MUL:
                case OUT_DIV:
                case OUT_EQ:
                case OUT_NE:
                case OUT_LT:
                case OUT_GT: {
                    // правый операнд
                    exe.pop(ref Y);
                    // левый операнд
                    exe.pop(ref X);
                    switch (stt) {
                    case OUT_ADD: {
                        X.Int_val += Y.Int_val;
                        break;
                    }
                    case OUT_SUB: {
                        X.Int_val -= Y.Int_val;
                        break;
                    }
                    case OUT_MUL: {
                        X.Int_val *= Y.Int_val;
                        break;
                    }
                    case OUT_DIV: {
                        if (Y.Int_val == 0) {
                            throw new Exception("exe division by zero");
                        }
                        break;
                    }
                    case OUT_EQ: {
                        X.Bool_val = X.Int_val == Y.Int_val;
                        break;
                    }
                    case OUT_NE: {
                        X.Bool_val = X.Int_val != Y.Int_val;
                        break;
                    }
                    case OUT_LT: {
                        X.Bool_val = X.Int_val < Y.Int_val;
                        break;
                    }
                    case OUT_GT: {
                        X.Bool_val = X.Int_val > Y.Int_val;
                        break;
                    }
                    }
                    exe.push(ref X);
                    strip_pointer++;
                    break;
                }
                case OUT_LABEL: {
                    // на стек помещаем значение метки
                    X.Int_val = strip[strip_pointer].Int_val;
                    // тип значения - константа
                    X.Stt = OUT_I4;
                    exe.push(ref X);
                    strip_pointer++;
                    break;
                }
                case OUT_DEFL: {
                    exe.pop(ref X);
                    strip_pointer++;
                    break;
                }
                case OUT_BZ: {
                    exe.pop(ref Y);
                    exe.pop(ref X);
                    if (X.Int_val == 0) {
                        j = strip.find_DEF(Y.Int_val);
                        if (j == -1) {
                            throw new Exception("exe label not found");
                        }
                        strip_pointer = j;
                    } else {
                        Y.Int_val = 0;
                    }
                    strip_pointer++;
                    break;
                }
                case OUT_BR: {
                    exe.pop(ref Y);
                    j = strip.find_DEF(Y.Int_val);
                    if (j == -1) {
                        throw new Exception("exe label not found");
                    }
                    strip_pointer = j;
                    break;
                }
                case OUT_PRINT: {
                    exe.pop(ref Y);
                    //fprintf(parse_stream, "exe print %d", Y.Int_val);
                    strip_pointer++;
                    break;
                }
                }
                if (++it_counter > MAX_IT) {
                    throw new Exception("exe deadlock");
                }
            }
        }
        // извлекает из стека значение
        void exe_pop(ref Token e) {
            exe.pop(ref e);
            if (e.Stt == OUT_I4) {

            } else if (e.Stt == OUT_ID) {
                int j = syms.find(e);
                if (j == ST_NOTFOUND) {
                    // символ не найден
                    throw new Exception("exe_pop identifier not found");
                }
                // извлекаем значение из таблицы символов
                e.Int_val = syms[j].Int_val;
                // меняем тип элемента на константный
                e.Stt = OUT_I4;
            } else {
                throw new Exception( "exe_pop internal error");
            }
        }
        // возвращает очередной токен
        int next_token() {
            return lex.Next_token(ref tok);
        }
    }
}
