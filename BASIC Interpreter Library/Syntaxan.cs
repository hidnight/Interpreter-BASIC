using System;
using System.Collections.Generic;
using System.Globalization;
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
        private StreamWriter parse_stream;
        // поток ошибок
        private StreamWriter error_stream;
        // лексический анализатор
        private Lexan lex;
        // текущий токен
        private Token tok;
        // стек меток
        private Stack<int> stl = new Stack<int>();
        // стек МП-автомата
        private Stack<Interpreter_symbol> sta = new Stack<Interpreter_symbol>();
        // таблица символов
        private Syms syms;
        // исполняющий стек
        private ExStack exe;
        // лента ПОЛИЗ
        private Strip strip;
        // конструктор
        public Syntaxan(ref Lexan lex, ref StreamWriter parse_stream, ref StreamWriter error_stream) {
            this.lex = lex;
            this.parse_stream = parse_stream;
            this.error_stream = error_stream;
            syms = new Syms();
            exe = new ExStack();
            strip = new Strip();
            tok = new Token();
        }
        /*public void FlushStreams() {
            parse_stream.Flush();
            error_stream.Flush();
        }*/
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
            Interpreter_symbol s = 0;
            // значение из управляющей таблицы
            int t = 0;
            // предварительные действия
            // уточнение длины правил
            get_rule_len();
            // проталкивание дна стека
            sta.push(TOK_EOT);
            // проталкивание аксиомы
            sta.push(START);
            // работа LL автомата
            while (true) {
                // состояние на стеке
                s = clear_stack();
                // получение управляющего значения
                try {
                    if (s >= TOK_EOT && s <= TOK_LAST &&
                        tok.Stt >= TOK_EOT && tok.Stt <= TOK_LAST) {
                        if (s == tok.Stt) {
                            t = POP;
                            if (s == TOK_EOT) {
                                t = ACC;
                            }
                        } else {
                            t = 0;
                        }
                    } else {
                        t = SYNTA[s - TOK_LAST - 1][(int)tok.Stt];
                    }
                    error_stream.Write("\n Ожидалось = " + s.ToString() + "\n");
                    error_stream.Write("\n Получено = " + tok.Stt.ToString() + "\n");
                    error_stream.Write("\n t = " + t.ToString() + "\n");
                } catch (Exception) {
                    throw;
                }
                // анализ управляющего значения
                if (t <= 0) {
                    // ошибка
                    error_stream.Write("\nОшибка синтаксического анализа. =" + t + ".\n\n");
                    parse_stream.Write("Ошибка синтаксического анализа.");
                    //FlushStreams();
                    return 0;
                } else {
                    if (t == ACC) {
                        // допуск
                        break;
                    } else {
                        if (t == POP) {
                            // выброс
                            at = sta.pop();
                            // выталкиваем операционные символы
                            clear_stack();
                            // следующий токен
                            if (next_token() == 0) {
                                return 0;
                            }
                        } else {
                            if (t <= MAX_RULE) {
                                // правило
                                at = sta.pop();
                                n = RLEN[(int)t];
                                for (i = n - 1; i >= 0; i--) {
                                    bt = RULE[(int)t][i];
                                    sta.push(bt);
                                }
                                error_stream.Write("Правило " + t + "->");
                                for (i = 0; i < n; i++) {
                                    error_stream.Write(" " + RULE[t][i].ToString() + " ");
                                }
                                if (n == 0) {
                                    error_stream.Write("λ");
                                }
                                error_stream.WriteLine();
                            } else {
                                // ошибка управляющей таблицы
                                error_stream.Write("syntaxan: Неверная синтаксическая таблица");
                                //FlushStreams();
                                return 0;
                            }
                        }
                    }
                }
            }
            
            // записываем конец ленты ПОЛИЗ
            out_(OUT_END);
            // выводим содержимое ленты ПОЛИЗ
            error_stream.Write("\nЛента ПОЛИЗ\n" + strip.ToString() + "\n");
            execute();
            error_stream.Write("\nsyntaxan: успешно.\n\n");
            //FlushStreams();
            return 1;
        }
        // запись символа на ленту ПОЛИЗ
        void out_(Interpreter_symbol tt) {
            int i = 0, j = 0;
            Token t = (Token)tok.Clone();
            t.Stt = tt;
            switch (tt) {
            case OUT_PUSH: {
                j = strip.New_label();
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
        Interpreter_symbol clear_stack() {
            Interpreter_symbol s;
            while ((s = sta.Top) > SYM_LAST) {
                s = sta.pop();
                out_(s);
            }
            return s;
        }
        // исполняющая машина ПОЛИЗ
        void execute() {
            // счетчик операций ПОЛИЗ
            long it_counter = 1;
            // указатель ленты ПОЛИЗ
            long strip_pointer = 1;
            // текущий элемент ПОЛИЗ
            Interpreter_symbol stt;
            // переменные для вычислений
            Token X = new Token(), Y = new Token();
            // вспомогательные
            long j = 0;
            // рабочий цикл выполнения ленты ПОЛИЗ
            while (true) {
                // очистка переменных
                X.Reset();
                Y.Reset();
                // тип элемента ленты
                stt = strip[(int)strip_pointer].Stt;
                switch (stt) {
                case OUT_END: {
                    error_stream.Write("\nВыполнение завершено.\n\n");
                    return;
                }
                case OUT_ID:
                case OUT_I4:
                case OUT_LONG:
                case OUT_R8:
                case OUT_REAL:
                case OUT_QUOTE:
                case OUT_STRING: {
                    // проталкивание в стек
                    Token e = strip[(int)strip_pointer];
                    switch (stt) {
                    case OUT_R8:
                    case OUT_REAL: {
                        e.Data_Type = Data_type.STDT_R8;
                        break;
                    }
                    case OUT_I4:
                    case OUT_LONG: {
                        e.Data_Type = Data_type.STDT_I4;
                        break;
                    }
                    case OUT_QUOTE:
                    case OUT_STRING: {
                        e.Data_Type = Data_type.STDT_QUOTE;
                        break;
                    }
                    case OUT_ID: {
                        break;
                    }
                    default: {
                        e.Data_Type = Data_type.STDT_NULL;
                        break;
                    }
                    }
                    exe.push(e);
                    // следующий элемент ленты
                    strip_pointer++;
                    break;
                }
                case OUT_DIM: {
                    // тип
                    exe.pop(ref Y);
                    // ID
                    exe.pop(ref X);
                    j = syms.insert(X);
                    if (j == ST_EXISTS) {
                        error_stream.Write("\nПовторное объявление переменной" +
                            ". Строка " + tok.Line_Number + "\n");
                        return;
                    }
                    syms[(int)j].Data_Type = Y.Data_Type;
                    strip_pointer++;
                    break;
                }
                case OUT_ASS: {
                    // значение
                    exe_pop(ref Y);
                    // ID
                    exe.pop(ref X);
                    j = syms.find(ref X);
                    if (j == ST_NOTFOUND) {
                        error_stream.Write("exe_pop Переменная не найдена" + ". Строка " + tok.Line_Number + "\n");
                        return;
                    }
                    if (Y.Data_Type == Data_type.STDT_BOOL) {
                        error_stream.Write("Невозможно присвоить булево значение" + ". Строка " + tok.Line_Number + "\n");
                        return;
                    }
                    switch (X.Data_Type) {
                    case Data_type.STDT_I4: {
                        switch (Y.Data_Type) {
                        case Data_type.STDT_I4: {
                            syms[(int)j].Int_val = Y.Int_val;
                            break;
                        }
                        case Data_type.STDT_R8: {
                            syms[(int)j].Int_val = (int)Y.Dbl_val;
                            break;
                        }
                        case Data_type.STDT_QUOTE: {
                            error_stream.Write("Переменной типа LONG нельзя присвоить значение типа QUOTE. Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }
                        break;
                    }
                    case Data_type.STDT_R8: {
                        switch (Y.Data_Type) {
                        case Data_type.STDT_I4: {
                            syms[(int)j].Dbl_val = Y.Int_val;
                            break;
                        }
                        case Data_type.STDT_R8: {
                            syms[(int)j].Dbl_val = Y.Dbl_val;
                            break;
                        }
                        case Data_type.STDT_QUOTE: {
                            error_stream.Write("Переменной типа REAL нельзя присвоить значение типа QUOTE." + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }
                        break;
                    }
                    case Data_type.STDT_QUOTE: {
                        switch (Y.Data_Type) {
                        case Data_type.STDT_I4: {
                            syms[(int)j].Str_val = Y.Int_val.ToString();
                            break;
                        }
                        case Data_type.STDT_R8: {
                            syms[(int)j].Str_val = Y.Dbl_val.ToString("R", CultureInfo.InvariantCulture);
                            break;
                        }
                        case Data_type.STDT_QUOTE: {
                            syms[(int)j].Str_val = Y.Str_val;
                            break;
                        }
                        }
                        break;
                    }
                    }
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
                case OUT_GT:
                case OUT_LE:
                case OUT_GE:
                case OUT_AND:
                case OUT_OR: {
                    // правый операнд
                    exe_pop(ref Y);
                    // левый операнд
                    exe_pop(ref X);
                    switch (stt) {
                    case OUT_ADD: {
                        switch (X.Data_Type) {
                        case Data_type.STDT_I4: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Int_val += Y.Int_val;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Data_Type = Data_type.STDT_R8;
                                X.Dbl_val = X.Int_val + Y.Dbl_val;
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сложить число и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сложить число и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_R8: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Dbl_val += Y.Int_val;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Dbl_val += Y.Dbl_val;
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сложить число и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сложить число и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_QUOTE: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Str_val += Y.Int_val.ToString();
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Str_val += Y.Dbl_val.ToString("R", CultureInfo.InvariantCulture);
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                X.Str_val += Y.Str_val;
                                break;
                            }
                            case Data_type.STDT_BOOL: {
                                X.Str_val += Y.Bool_val.ToString();
                                break;
                            }
                            default: {
                                error_stream.Write("Неверныйй тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_BOOL: {
                            error_stream.Write("Невозможно сложить с булевым значением" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        default: {
                            error_stream.Write("Неверныйй тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_SUB: {

                        switch (Y.Data_Type) {
                        case Data_type.STDT_QUOTE: {
                            error_stream.Write("Невозможно вычесть число и строку" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        case Data_type.STDT_BOOL: {
                            error_stream.Write("Невозможно вычесть из булева значения" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }

                        switch (X.Data_Type) {
                        case Data_type.STDT_I4: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Int_val -= Y.Int_val;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Data_Type = Data_type.STDT_R8;
                                X.Dbl_val = X.Int_val - Y.Dbl_val;
                                break;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_R8: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Dbl_val -= Y.Int_val;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Dbl_val -= Y.Dbl_val;
                                break;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_QUOTE: {
                            error_stream.Write("Невозможно вычесть из строки" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        case Data_type.STDT_BOOL: {
                            error_stream.Write("Невозможно вычесть из булева значения" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        default: {
                            error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_MUL: {

                        switch (Y.Data_Type) {
                        case Data_type.STDT_QUOTE: {
                            error_stream.Write("Невозможно умножить на строку" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        case Data_type.STDT_BOOL: {
                            error_stream.Write("Невозможно умножить на булево значение" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }

                        switch (X.Data_Type) {
                        case Data_type.STDT_I4: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Int_val *= Y.Int_val;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Data_Type = Data_type.STDT_R8;
                                X.Dbl_val = X.Int_val * Y.Dbl_val;
                                break;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_R8: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Dbl_val *= Y.Int_val;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Dbl_val *= Y.Dbl_val;
                                break;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_QUOTE: {
                            error_stream.Write("Невозможно умножить строку" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        case Data_type.STDT_BOOL: {
                            error_stream.Write("Невозможно умножить булево значение" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        default: {
                            error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_DIV: {
                        if (Y.Data_Type == Data_type.STDT_I4 && Y.Int_val == 0 ||
                            Y.Data_Type == Data_type.STDT_R8 && Y.Dbl_val == 0.0) {
                            error_stream.Write("Деление на нуль" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        switch (Y.Data_Type) {
                        case Data_type.STDT_QUOTE: {
                            error_stream.Write("Невозможно поделить на строку" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        case Data_type.STDT_BOOL: {
                            error_stream.Write("Невозможно поделить на булево значение" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }
                        switch (X.Data_Type) {
                        case Data_type.STDT_I4: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Int_val /= Y.Int_val;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Data_Type = Data_type.STDT_R8;
                                X.Dbl_val = X.Int_val / Y.Dbl_val;
                                break;
                            }
                            default: {
                                error_stream.Write("Неверныйй тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_R8: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Dbl_val /= Y.Int_val;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Dbl_val /= Y.Dbl_val;
                                break;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_QUOTE: {
                            error_stream.Write("Невозможно поделить строку" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        case Data_type.STDT_BOOL: {
                            error_stream.Write("Невозможно поделить булево значение" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        default: {
                            error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_EQ: {
                        switch (X.Data_Type) {
                        case Data_type.STDT_I4: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Bool_val = X.Int_val == Y.Int_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Bool_val = X.Int_val == Y.Dbl_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_R8: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Bool_val = X.Dbl_val == Y.Int_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Bool_val = X.Dbl_val == Y.Dbl_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_QUOTE: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4:
                            case Data_type.STDT_R8: {
                                error_stream.Write("Невозможно сравнить строку и число" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_QUOTE: {
                                X.Bool_val = X.Str_val == Y.Str_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сравнить строку и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_BOOL: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4:
                            case Data_type.STDT_R8: {
                                error_stream.Write("Невозможно сравнить булево значение и число" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сравнить булево значение и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                X.Bool_val = X.Bool_val == Y.Bool_val;
                                break;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        default: {
                            error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_NE: {
                        switch (X.Data_Type) {
                        case Data_type.STDT_I4: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Bool_val = X.Int_val != Y.Int_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Bool_val = X.Int_val != Y.Dbl_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_R8: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Bool_val = X.Dbl_val != Y.Int_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Bool_val = X.Dbl_val != Y.Dbl_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_QUOTE: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4:
                            case Data_type.STDT_R8: {
                                error_stream.Write("Невозможно сравнить строку и число" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_QUOTE: {
                                X.Bool_val = X.Str_val != Y.Str_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сравнить строку и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_BOOL: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4:
                            case Data_type.STDT_R8: {
                                error_stream.Write("Невозможно сравнить булево значение и число" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сравнить булево значение и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                X.Bool_val = X.Bool_val != Y.Bool_val;
                                break;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        default: {
                            error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_LT: {
                        switch (X.Data_Type) {
                        case Data_type.STDT_I4: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Bool_val = X.Int_val < Y.Int_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Bool_val = X.Int_val < Y.Dbl_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_R8: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Bool_val = X.Dbl_val < Y.Int_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Bool_val = X.Dbl_val < Y.Dbl_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_QUOTE: {
                            error_stream.Write("Невозможно применить < к строке" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        case Data_type.STDT_BOOL: {
                            error_stream.Write("Невозможно применить < к булевому значению" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        default: {
                            error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_GT: {
                        switch (X.Data_Type) {
                        case Data_type.STDT_I4: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Bool_val = X.Int_val > Y.Int_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Bool_val = X.Int_val > Y.Dbl_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_R8: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Bool_val = X.Dbl_val > Y.Int_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Bool_val = X.Dbl_val > Y.Dbl_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_QUOTE: {
                            error_stream.Write("Невозможно применить > к строке" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        case Data_type.STDT_BOOL: {
                            error_stream.Write("Невозможно применить > к булевому значению" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        default: {
                            error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_LE: {
                        switch (X.Data_Type) {
                        case Data_type.STDT_I4: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Bool_val = X.Int_val <= Y.Int_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Bool_val = X.Int_val <= Y.Dbl_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_R8: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Bool_val = X.Dbl_val <= Y.Int_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Bool_val = X.Dbl_val <= Y.Dbl_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_QUOTE: {
                            error_stream.Write("Невозможно применить <= к строке" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        case Data_type.STDT_BOOL: {
                            error_stream.Write("Невозможно применить <= к булевому значению" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        default: {
                            error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_GE: {
                        switch (X.Data_Type) {
                        case Data_type.STDT_I4: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Bool_val = X.Int_val >= Y.Int_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Bool_val = X.Int_val >= Y.Dbl_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_R8: {
                            switch (Y.Data_Type) {
                            case Data_type.STDT_I4: {
                                X.Bool_val = X.Dbl_val >= Y.Int_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_R8: {
                                X.Bool_val = X.Dbl_val >= Y.Dbl_val;
                                X.Data_Type = Data_type.STDT_BOOL;
                                break;
                            }
                            case Data_type.STDT_QUOTE: {
                                error_stream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            case Data_type.STDT_BOOL: {
                                error_stream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            default: {
                                error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case Data_type.STDT_QUOTE: {
                            error_stream.Write("Невозможно применить >= к строке" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        case Data_type.STDT_BOOL: {
                            error_stream.Write("Невозможно применить >= к булевому значению" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        default: {
                            error_stream.Write("Неверный тип данных операнда" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }
                        break;
                    }
                    }
                    exe.push(X);
                    strip_pointer++;
                    break;
                }

                case OUT_NOT:
                case OUT_U_ADD:
                case OUT_U_SUB: {
                    exe_pop(ref X);
                    switch (stt) {
                    case OUT_NOT: {
                        switch (X.Data_Type) {
                        case Data_type.STDT_I4:
                        case Data_type.STDT_R8:
                        case Data_type.STDT_QUOTE: {
                            error_stream.Write("Невозможно применить НЕ к небулевым значениям" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        case Data_type.STDT_BOOL: {
                            X.Bool_val = !X.Bool_val;
                            break;
                        }
                        }
                        break;
                    }
                    case OUT_U_SUB: {
                        switch (X.Data_Type) {
                        case Data_type.STDT_I4: {
                            X.Int_val = -X.Int_val;
                            break;
                        }
                        case Data_type.STDT_R8: {
                            X.Dbl_val = -X.Dbl_val;
                            break;
                        }
                        case Data_type.STDT_BOOL:
                        case Data_type.STDT_QUOTE: {
                            error_stream.Write("Невозможно применить унарным минус к нечисловым значениям" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        }
                        break;
                    }
                    }
                    exe.push(X);
                    strip_pointer++;
                    break;
                }

                case OUT_LABEL: {
                    // на стек помещаем значение метки
                    X.Int_val = strip[(int)strip_pointer].Int_val;
                    // тип значения - константа
                    X.Stt = OUT_I4;
                    exe.push(X);
                    strip_pointer++;
                    break;
                }

                case OUT_DEFL: {
                    exe.pop(ref X);
                    strip_pointer++;
                    break;
                }

                case OUT_BZ: {
                    exe_pop(ref Y);
                    exe_pop(ref X);
                    if (X.Bool_val == false) {
                        j = strip.Find_DEF(Y.Int_val);
                        if (j == -1) {
                            error_stream.Write("Метка не найдена" + ". Строка " + tok.Line_Number + "\n");
                            return;
                        }
                        strip_pointer = j;
                    } else {
                        strip_pointer++;
                    }
                    break;
                }

                case OUT_BR: {
                    exe_pop(ref Y);
                    j = strip.Find_DEF(Y.Int_val);
                    if (j == -1) {
                        error_stream.Write("Метка не найдена" + ". Строка " + tok.Line_Number + "\n");
                        return;
                    }
                    strip_pointer = j;
                    break;
                }

                case OUT_PRINT: {
                    exe_pop(ref Y);
                    switch (Y.Data_Type) {
                    case Data_type.STDT_I4: {
                        parse_stream.WriteLine(Y.Int_val.ToString());
                        break;
                    }
                    case Data_type.STDT_R8: {
                        parse_stream.WriteLine(Y.Dbl_val.ToString("R", CultureInfo.InvariantCulture));
                        break;
                    }
                    case Data_type.STDT_QUOTE: {
                        parse_stream.WriteLine(Y.Str_val);
                        break;
                    }
                    case Data_type.STDT_BOOL: {
                        parse_stream.WriteLine(Y.Bool_val);
                        break;
                    }
                    default: {
                        error_stream.Write("Неверный операнд" + ". Строка " + tok.Line_Number + "\n");
                        return;
                    }
                    }
                    strip_pointer++;
                    break;
                }
                }

                if (++it_counter > MAX_IT) {
                    error_stream.Write("Возможный бесконечный цикл. Строка " + tok.Line_Number + "\n");
                    //FlushStreams();
                    return;
                }
            }
        }
        // извлекает из стека значение
        void exe_pop(ref Token e) {
            exe.pop(ref e);
            if (e.Stt == OUT_I4 || e.Stt == OUT_R8 || e.Stt == OUT_QUOTE) {
            } else {
                if (e.Stt == OUT_ID) {
                    long j = syms.find(ref e);
                    if (j == ST_NOTFOUND) {
                        error_stream.Write("Идентификатор \"" + e.Name + "\" не найден" + ". Строка " + tok.Line_Number + "\n");
                        //FlushStreams();
                        return;
                    }
                    e.Data_Type = syms[(int)j].Data_Type;
                    switch (e.Data_Type) {
                    case Data_type.STDT_I4: {
                        e.Int_val = syms[(int)j].Int_val;
                        e.Stt = OUT_I4;
                        break;
                    }
                    case Data_type.STDT_R8: {
                        e.Dbl_val = syms[(int)j].Dbl_val;
                        e.Stt = OUT_R8;
                        break;
                    }
                    case Data_type.STDT_QUOTE: {
                        e.Str_val = syms[(int)j].Str_val;
                        e.Stt = OUT_QUOTE;
                        break;
                    }
                    }
                } else {
                    error_stream.Write("exe_pop Внутренняя ошибка" + ". Строка " + tok.Line_Number + "\n");
                    //FlushStreams();
                    return;
                }
            }
        }
        // возвращает очередной токен
        long next_token() {
            Token temp = tok;
            long result = lex.Next_token(ref tok);
            return result;
        }
    }
}
