using System;
using System.Globalization;
using System.IO;
using static BASIC_Interpreter_Library.Constants;
using static BASIC_Interpreter_Library.InterpreterSymbol;

namespace BASIC_Interpreter_Library {
    /* LL */
    public class Syntaxan {
        // входной поток
        private StreamWriter ParseStream;
        // поток ошибок
        private StreamWriter ErrorStream;
        // лексический анализатор
        private Lexan lex;
        // текущий токен
        private Token tok;
        // стек меток
        private Stack<int> stl = new Stack<int>();
        // стек МП-автомата
        private Stack<InterpreterSymbol> sta = new Stack<InterpreterSymbol>();
        // таблица символов
        private Syms syms;
        // исполняющий стек
        private ExStack exe;
        // лента ПОЛИЗ
        private Strip strip;
        // конструктор
        public Syntaxan(ref Lexan lex, ref StreamWriter parse_stream, ref StreamWriter error_stream) {
            this.lex = lex;
            this.ParseStream = parse_stream;
            this.ErrorStream = error_stream;
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
        public int Parse() {
            if (NextToken() == 0)
                return 0;
            // вспомогательные переменные
            // i - итератор
            // n - длина правила
            int i, n;
            InterpreterSymbol at = TOK_EOT, bt = TOK_EOT;
            // символ на стеке
            InterpreterSymbol s = 0;
            // значение из управляющей таблицы
            int t = 0;
            // предварительные действия
            // уточнение длины правил
            GetRuleLen();
            // проталкивание дна стека
            sta.Push(TOK_EOT);
            // проталкивание аксиомы
            sta.Push(START);
            // работа LL автомата
            while (true) {
                // состояние на стеке
                s = ClearStack();
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
                    ErrorStream.Write("\n Ожидалось = " + s.ToString() + "\n");
                    ErrorStream.Write("\n Получено = " + tok.Stt.ToString() + "\n");
                    ErrorStream.Write("\n t = " + t.ToString() + "\n");
                } catch (Exception) {
                    throw;
                }
                // анализ управляющего значения
                if (t <= 0) {
                    // ошибка
                    ErrorStream.Write("\nОшибка синтаксического анализа. =" + t + ".\n\n");
                    ParseStream.Write("Ошибка синтаксического анализа.");
                    //FlushStreams();
                    return 0;
                } else {
                    if (t == ACC) {
                        // допуск
                        break;
                    } else {
                        if (t == POP) {
                            // выброс
                            at = sta.Pop();
                            // выталкиваем операционные символы
                            ClearStack();
                            // следующий токен
                            if (NextToken() == 0) {
                                return 0;
                            }
                        } else {
                            if (t <= MAX_RULE) {
                                // правило
                                at = sta.Pop();
                                n = RLEN[(int)t];
                                for (i = n - 1; i >= 0; i--) {
                                    bt = RULE[(int)t][i];
                                    sta.Push(bt);
                                }
                                ErrorStream.Write("Правило " + t + "->");
                                for (i = 0; i < n; i++) {
                                    ErrorStream.Write(" " + RULE[t][i].ToString() + " ");
                                }
                                if (n == 0) {
                                    ErrorStream.Write("λ");
                                }
                                ErrorStream.WriteLine();
                            } else {
                                // ошибка управляющей таблицы
                                ErrorStream.Write("syntaxan: Неверная синтаксическая таблица");
                                //FlushStreams();
                                return 0;
                            }
                        }
                    }
                }
            }
            
            // записываем конец ленты ПОЛИЗ
            PutOnStrip(OUT_END);
            // выводим содержимое ленты ПОЛИЗ
            ErrorStream.Write("\nЛента ПОЛИЗ\n" + strip.ToString() + "\n");
            Execute();
            ErrorStream.Write("\nsyntaxan: успешно.\n\n");
            //FlushStreams();
            return 1;
        }
        // запись символа на ленту ПОЛИЗ
        private void PutOnStrip(InterpreterSymbol tt) {
            int i = 0, j = 0;
            Token t = (Token)tok.Clone();
            t.Stt = tt;
            switch (tt) {
            case OUT_PUSH: {
                j = strip.NewLabel();
                stl.Push(j);
                t.IntVal = j;
                t.Stt = OUT_LABEL;
                break;
            }
            case OUT_POPL: {
                j = stl.Pop();
                t.IntVal = j;
                t.Stt = OUT_LABEL;
                break;
            }
            case OUT_SWAP: {
                i = stl.Pop();
                j = stl.Pop();
                stl.Push(i);
                t.IntVal = j;
                t.Stt = OUT_LABEL;
                break;
            }
            }
            strip.Add(t);
        }
        // выводит операционные символы со стека
        private InterpreterSymbol ClearStack() {
            InterpreterSymbol s;
            while ((s = sta.Top) > SYM_LAST) {
                s = sta.Pop();
                PutOnStrip(s);
            }
            return s;
        }
        // исполняющая машина ПОЛИЗ
        private void Execute() {
            // счетчик операций ПОЛИЗ
            long it_counter = 1;
            // указатель ленты ПОЛИЗ
            long strip_pointer = 1;
            // текущий элемент ПОЛИЗ
            InterpreterSymbol stt;
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
                    ErrorStream.Write("\nВыполнение завершено.\n\n");
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
                        e.DataType = DataType.STDT_R8;
                        break;
                    }
                    case OUT_I4:
                    case OUT_LONG: {
                        e.DataType = DataType.STDT_I4;
                        break;
                    }
                    case OUT_QUOTE:
                    case OUT_STRING: {
                        e.DataType = DataType.STDT_QUOTE;
                        break;
                    }
                    case OUT_ID: {
                        break;
                    }
                    default: {
                        e.DataType = DataType.STDT_NULL;
                        break;
                    }
                    }
                    exe.Push(e);
                    // следующий элемент ленты
                    strip_pointer++;
                    break;
                }
                case OUT_DIM: {
                    // тип
                    exe.Pop(ref Y);
                    // ID
                    exe.Pop(ref X);
                    j = syms.Insert(X);
                    if (j == ST_EXISTS) {
                        ErrorStream.Write("\nПовторное объявление переменной" +
                            ". Строка " + tok.LineNumber + "\n");
                        return;
                    }
                    syms[(int)j].DataType = Y.DataType;
                    strip_pointer++;
                    break;
                }
                case OUT_ASS: {
                    // значение
                    ExePop(ref Y);
                    // ID
                    exe.Pop(ref X);
                    j = syms.Find(ref X);
                    if (j == ST_NOTFOUND) {
                        ErrorStream.Write("exe_pop Переменная не найдена" + ". Строка " + tok.LineNumber + "\n");
                        return;
                    }
                    if (Y.DataType == DataType.STDT_BOOL) {
                        ErrorStream.Write("Невозможно присвоить булево значение" + ". Строка " + tok.LineNumber + "\n");
                        return;
                    }
                    switch (X.DataType) {
                    case DataType.STDT_I4: {
                        switch (Y.DataType) {
                        case DataType.STDT_I4: {
                            syms[(int)j].IntVal = Y.IntVal;
                            break;
                        }
                        case DataType.STDT_R8: {
                            syms[(int)j].IntVal = (int)Y.DblVal;
                            break;
                        }
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Переменной типа LONG нельзя присвоить значение типа QUOTE. Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }
                    case DataType.STDT_R8: {
                        switch (Y.DataType) {
                        case DataType.STDT_I4: {
                            syms[(int)j].DblVal = Y.IntVal;
                            break;
                        }
                        case DataType.STDT_R8: {
                            syms[(int)j].DblVal = Y.DblVal;
                            break;
                        }
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Переменной типа REAL нельзя присвоить значение типа QUOTE." + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }
                    case DataType.STDT_QUOTE: {
                        switch (Y.DataType) {
                        case DataType.STDT_I4: {
                            syms[(int)j].StrVal = Y.IntVal.ToString();
                            break;
                        }
                        case DataType.STDT_R8: {
                            syms[(int)j].StrVal = Y.DblVal.ToString("R", CultureInfo.InvariantCulture);
                            break;
                        }
                        case DataType.STDT_QUOTE: {
                            syms[(int)j].StrVal = Y.StrVal;
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
                    ExePop(ref Y);
                    // левый операнд
                    ExePop(ref X);
                    switch (stt) {
                    case OUT_ADD: {
                        switch (X.DataType) {
                        case DataType.STDT_I4: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.IntVal += Y.IntVal;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.DataType = DataType.STDT_R8;
                                X.DblVal = X.IntVal + Y.DblVal;
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                X.DataType = DataType.STDT_QUOTE;
                                X.StrVal = X.IntVal + Y.StrVal;
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сложить число и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_R8: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.DblVal += Y.IntVal;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.DblVal += Y.DblVal;
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                X.DataType = DataType.STDT_QUOTE;
                                X.StrVal = X.DblVal + Y.StrVal;
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сложить число и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_QUOTE: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.StrVal += Y.IntVal.ToString();
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.StrVal += Y.DblVal.ToString("R", CultureInfo.InvariantCulture);
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                X.StrVal += Y.StrVal;
                                break;
                            }
                            case DataType.STDT_BOOL: {
                                X.StrVal += Y.BoolVal;
                                break;
                            }
                            default: {
                                ErrorStream.Write("Неверныйй тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_BOOL: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4:
                            case DataType.STDT_R8:
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сложить с булевым значением" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_QUOTE: {
                                X.DataType = DataType.STDT_QUOTE;
                                X.StrVal = X.BoolVal + Y.StrVal;
                                break;
                            }
                            default: {
                                ErrorStream.Write("Неверныйй тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        default: {
                            ErrorStream.Write("Неверныйй тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_SUB: {

                        switch (Y.DataType) {
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Невозможно вычесть число и строку" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_BOOL: {
                            ErrorStream.Write("Невозможно вычесть из булева значения" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }

                        switch (X.DataType) {
                        case DataType.STDT_I4: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.IntVal -= Y.IntVal;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.DataType = DataType.STDT_R8;
                                X.DblVal = X.IntVal - Y.DblVal;
                                break;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_R8: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.DblVal -= Y.IntVal;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.DblVal -= Y.DblVal;
                                break;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Невозможно вычесть из строки" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_BOOL: {
                            ErrorStream.Write("Невозможно вычесть из булева значения" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        default: {
                            ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_MUL: {

                        switch (Y.DataType) {
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Невозможно умножить на строку" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_BOOL: {
                            ErrorStream.Write("Невозможно умножить на булево значение" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }

                        switch (X.DataType) {
                        case DataType.STDT_I4: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.IntVal *= Y.IntVal;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.DataType = DataType.STDT_R8;
                                X.DblVal = X.IntVal * Y.DblVal;
                                break;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_R8: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.DblVal *= Y.IntVal;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.DblVal *= Y.DblVal;
                                break;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Невозможно умножить строку" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_BOOL: {
                            ErrorStream.Write("Невозможно умножить булево значение" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        default: {
                            ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_DIV: {
                        if (Y.DataType == DataType.STDT_I4 && Y.IntVal == 0 ||
                            Y.DataType == DataType.STDT_R8 && Y.DblVal == 0.0) {
                            ErrorStream.Write("Деление на нуль" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        switch (Y.DataType) {
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Невозможно поделить на строку" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_BOOL: {
                            ErrorStream.Write("Невозможно поделить на булево значение" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        switch (X.DataType) {
                        case DataType.STDT_I4: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.IntVal /= Y.IntVal;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.DataType = DataType.STDT_R8;
                                X.DblVal = X.IntVal / Y.DblVal;
                                break;
                            }
                            default: {
                                ErrorStream.Write("Неверныйй тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_R8: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.DblVal /= Y.IntVal;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.DblVal /= Y.DblVal;
                                break;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Невозможно поделить строку" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_BOOL: {
                            ErrorStream.Write("Невозможно поделить булево значение" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        default: {
                            ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_EQ: {
                        switch (X.DataType) {
                        case DataType.STDT_I4: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.BoolVal = X.IntVal == Y.IntVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.BoolVal = ((double)X.IntVal) == Y.DblVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_R8: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.BoolVal = X.DblVal == (double)Y.IntVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.BoolVal = X.DblVal == Y.DblVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_QUOTE: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4:
                            case DataType.STDT_R8: {
                                ErrorStream.Write("Невозможно сравнить строку и число" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_QUOTE: {
                                X.BoolVal = X.StrVal == Y.StrVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сравнить строку и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_BOOL: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4:
                            case DataType.STDT_R8: {
                                ErrorStream.Write("Невозможно сравнить булево значение и число" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно сравнить булево значение и строку" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                X.BoolVal = X.BoolVal == Y.BoolVal;
                                break;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        default: {
                            ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_NE: {
                        switch (X.DataType) {
                        case DataType.STDT_I4: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.BoolVal = X.IntVal != Y.IntVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.BoolVal = X.IntVal != Y.DblVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_R8: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.BoolVal = X.DblVal != Y.IntVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.BoolVal = X.DblVal != Y.DblVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_QUOTE: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4:
                            case DataType.STDT_R8: {
                                ErrorStream.Write("Невозможно сравнить строку и число" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_QUOTE: {
                                X.BoolVal = X.StrVal != Y.StrVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сравнить строку и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_BOOL: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4:
                            case DataType.STDT_R8: {
                                ErrorStream.Write("Невозможно сравнить булево значение и число" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно сравнить булево значение и строку" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                X.BoolVal = X.BoolVal != Y.BoolVal;
                                break;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        default: {
                            ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_LT: {
                        switch (X.DataType) {
                        case DataType.STDT_I4: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.BoolVal = X.IntVal < Y.IntVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.BoolVal = X.IntVal < Y.DblVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_R8: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.BoolVal = X.DblVal < Y.IntVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.BoolVal = X.DblVal < Y.DblVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Невозможно применить < к строке" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_BOOL: {
                            ErrorStream.Write("Невозможно применить < к булевому значению" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        default: {
                            ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_GT: {
                        switch (X.DataType) {
                        case DataType.STDT_I4: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.BoolVal = X.IntVal > Y.IntVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.BoolVal = X.IntVal > Y.DblVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_R8: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.BoolVal = X.DblVal > Y.IntVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.BoolVal = X.DblVal > Y.DblVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Невозможно применить > к строке" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_BOOL: {
                            ErrorStream.Write("Невозможно применить > к булевому значению" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        default: {
                            ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_LE: {
                        switch (X.DataType) {
                        case DataType.STDT_I4: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.BoolVal = X.IntVal <= Y.IntVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.BoolVal = X.IntVal <= Y.DblVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_R8: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.BoolVal = X.DblVal <= Y.IntVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.BoolVal = X.DblVal <= Y.DblVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Невозможно применить <= к строке" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_BOOL: {
                            ErrorStream.Write("Невозможно применить <= к булевому значению" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        default: {
                            ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_GE: {
                        switch (X.DataType) {
                        case DataType.STDT_I4: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.BoolVal = X.IntVal >= Y.IntVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.BoolVal = X.IntVal >= Y.DblVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_R8: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                X.BoolVal = X.DblVal >= Y.IntVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_R8: {
                                X.BoolVal = X.DblVal >= Y.DblVal;
                                X.DataType = DataType.STDT_BOOL;
                                break;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно сравнить число и строку" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                ErrorStream.Write("Невозможно сравнить число и булево значение" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Невозможно применить >= к строке" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_BOOL: {
                            ErrorStream.Write("Невозможно применить >= к булевому значению" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        default: {
                            ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_AND: {
                        switch (X.DataType) {
                        case DataType.STDT_I4: {
                            ErrorStream.Write("Невозможно применить AND к числу" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_R8: {
                            ErrorStream.Write("Невозможно применить AND к числу" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Невозможно применить AND к строке" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_BOOL: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                ErrorStream.Write("Невозможно применить AND к числу" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_R8: {
                                ErrorStream.Write("Невозможно применить AND к числу" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно применить AND к строке" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                X.BoolVal = X.BoolVal && Y.BoolVal;
                                break;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        default: {
                            ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }

                    case OUT_OR: {
                        switch (X.DataType) {
                        case DataType.STDT_I4: {
                            ErrorStream.Write("Невозможно применить AND к числу" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_R8: {
                            ErrorStream.Write("Невозможно применить AND к числу" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Невозможно применить AND к строке" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_BOOL: {
                            switch (Y.DataType) {
                            case DataType.STDT_I4: {
                                ErrorStream.Write("Невозможно применить AND к числу" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_R8: {
                                ErrorStream.Write("Невозможно применить AND к числу" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_QUOTE: {
                                ErrorStream.Write("Невозможно применить AND к строке" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            case DataType.STDT_BOOL: {
                                X.BoolVal = X.BoolVal || Y.BoolVal;
                                break;
                            }
                            default: {
                                ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                                return;
                            }
                            }
                            break;
                        }
                        default: {
                            ErrorStream.Write("Неверный тип данных операнда" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }
                    }
                    exe.Push(X);
                    strip_pointer++;
                    break;
                }

                case OUT_NOT:
                case OUT_U_ADD:
                case OUT_U_SUB: {
                    ExePop(ref X);
                    switch (stt) {
                    case OUT_NOT: {
                        switch (X.DataType) {
                        case DataType.STDT_I4:
                        case DataType.STDT_R8:
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Невозможно применить НЕ к небулевым значениям" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        case DataType.STDT_BOOL: {
                            X.BoolVal = !X.BoolVal;
                            break;
                        }
                        }
                        break;
                    }
                    case OUT_U_SUB: {
                        switch (X.DataType) {
                        case DataType.STDT_I4: {
                            X.IntVal = -X.IntVal;
                            break;
                        }
                        case DataType.STDT_R8: {
                            X.DblVal = -X.DblVal;
                            break;
                        }
                        case DataType.STDT_BOOL:
                        case DataType.STDT_QUOTE: {
                            ErrorStream.Write("Невозможно применить унарным минус к нечисловым значениям" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        }
                        break;
                    }
                    }
                    exe.Push(X);
                    strip_pointer++;
                    break;
                }

                case OUT_LABEL: {
                    // на стек помещаем значение метки
                    X.IntVal = strip[(int)strip_pointer].IntVal;
                    // тип значения - константа
                    X.Stt = OUT_I4;
                    exe.Push(X);
                    strip_pointer++;
                    break;
                }

                case OUT_DEFL: {
                    exe.Pop(ref X);
                    strip_pointer++;
                    break;
                }

                case OUT_BZ: {
                    ExePop(ref Y);
                    ExePop(ref X);
                    if (X.BoolVal == false) {
                        j = strip.FindDEF(Y.IntVal);
                        if (j == -1) {
                            ErrorStream.Write("Метка не найдена" + ". Строка " + tok.LineNumber + "\n");
                            return;
                        }
                        strip_pointer = j;
                    } else {
                        strip_pointer++;
                    }
                    break;
                }

                case OUT_BR: {
                    ExePop(ref Y);
                    j = strip.FindDEF(Y.IntVal);
                    if (j == -1) {
                        ErrorStream.Write("Метка не найдена" + ". Строка " + tok.LineNumber + "\n");
                        return;
                    }
                    strip_pointer = j;
                    break;
                }

                case OUT_PRINT: {
                    ExePop(ref Y);
                    switch (Y.DataType) {
                    case DataType.STDT_I4: {
                        ParseStream.Write(Y.IntVal.ToString());
                        break;
                    }
                    case DataType.STDT_R8: {
                        ParseStream.Write(Y.DblVal.ToString("R", CultureInfo.InvariantCulture));
                        break;
                    }
                    case DataType.STDT_QUOTE: {
                        ParseStream.Write(Y.StrVal);
                        break;
                    }
                    case DataType.STDT_BOOL: {
                        ParseStream.Write(Y.BoolVal);
                        break;
                    }
                    default: {
                        ErrorStream.Write("Неверный операнд" + ". Строка " + tok.LineNumber + "\n");
                        return;
                    }
                    }
                    strip_pointer++;
                    break;
                }
                }

                if (++it_counter > MAX_IT) {
                    ErrorStream.Write("Возможный бесконечный цикл. Строка " + tok.LineNumber + "\n");
                    //FlushStreams();
                    return;
                }
            }
        }
        // извлекает из стека значение
        private void ExePop(ref Token e) {
            exe.Pop(ref e);
            if (e.Stt == OUT_I4 || e.Stt == OUT_R8 || e.Stt == OUT_QUOTE) {
            } else {
                if (e.Stt == OUT_ID) {
                    long j = syms.Find(ref e);
                    if (j == ST_NOTFOUND) {
                        ErrorStream.Write("Идентификатор \"" + e.Name + "\" не найден" + ". Строка " + tok.LineNumber + "\n");
                        //FlushStreams();
                        return;
                    }
                    e.DataType = syms[(int)j].DataType;
                    switch (e.DataType) {
                    case DataType.STDT_I4: {
                        e.IntVal = syms[(int)j].IntVal;
                        e.Stt = OUT_I4;
                        break;
                    }
                    case DataType.STDT_R8: {
                        e.DblVal = syms[(int)j].DblVal;
                        e.Stt = OUT_R8;
                        break;
                    }
                    case DataType.STDT_QUOTE: {
                        e.StrVal = syms[(int)j].StrVal;
                        e.Stt = OUT_QUOTE;
                        break;
                    }
                    }
                } else {
                    ErrorStream.Write("exe_pop Внутренняя ошибка" + ". Строка " + tok.LineNumber + "\n");
                    //FlushStreams();
                    return;
                }
            }
        }
        // возвращает очередной токен
        private long NextToken() {
            Token temp = tok;
            long result = lex.GetNextToken(ref tok);
            return result;
        }
    }
}
