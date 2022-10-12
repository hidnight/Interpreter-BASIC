using System;
using System.IO;
using System.Text;
using static BASIC_Interpreter_Library.InterpreterSymbol;
using static BASIC_Interpreter_Library.Constants;

namespace BASIC_Interpreter_Library {
    public class Lexan {
        // входной поток
        private StreamReader input_stream;
        // поток ошибок
        private StreamWriter error_stream;
        // текущий символ входа
        private sbyte cc;
        private ulong lineNumber = 1;
        // считывает очередной символ входа
        private void NextChar() {
            cc = (sbyte)input_stream.Read();
        }
        // возвращает токен комментария
        // вход по '
        private InterpreterSymbol GetComment() {
            while (true) {
                NextChar();
                // \n
                if (cc == '\n' || cc == EOF) {
                    return TOK_COMMENT;
                }
            }
        }
        // принимает идентификатор
        private int GetId(ref Token tok) {
            tok.Stt = TOK_ID;
            while (true) {
                // вход только по букве, поэтому
                // нет разницы между буквой и цифрой
                switch (TOT[(byte)cc]) {
                case ALPHA:
                case DIGIT:
                case UNDER: {
                    if (tok.StrVal.Length < MAX_ID)
                        tok.Append((char)(byte)cc);
                    break;
                }
                default: {
                    int result = IsKeyword(ref tok);
                    if (tok.Stt == TOK_ID) {
                        tok.Name = tok.StrVal;
                        tok.StrVal = "";
                    }
                    return result;
                }
                }
                NextChar();
            }
        }
        // определяет ключевое слово
        private int IsKeyword(ref Token tok) {
            if (tok.StrVal.ToLower() == "dim") {
                tok.Stt = TOK_DIM;
                return 1;
            }
            if (tok.StrVal.ToLower() == "as") {
                tok.Stt = TOK_AS;
                return 1;
            }
            if (tok.StrVal.ToLower() == "end") {
                tok.Stt = TOK_END;
                return 1;
            }
            if (tok.StrVal.ToLower() == "or") {
                tok.Stt = TOK_OR;
                return 1;
            }
            if (tok.StrVal.ToLower() == "and") {
                tok.Stt = TOK_AND;
                return 1;
            }
            if (tok.StrVal.ToLower() == "not") {
                tok.Stt = TOK_NOT;
                return 1;
            }
            if (tok.StrVal.ToLower() == "while") {
                tok.Stt = TOK_WHILE;
                return 1;
            }
            if (tok.StrVal.ToLower() == "if") {
                tok.Stt = TOK_IF;
                return 1;
            }
            if (tok.StrVal.ToLower() == "print") {
                tok.Stt = TOK_PRINT;
                return 1;
            }
            if (tok.StrVal.ToLower() == "real") {
                tok.Stt = TOK_REAL;
                return 1;
            }
            if (tok.StrVal.ToLower() == "string") {
                tok.Stt = TOK_STRING;
                return 1;
            }
            if (tok.StrVal.ToLower() == "else") {
                tok.Stt = TOK_ELSE;
                return 1;
            }
            if (tok.StrVal.ToLower() == "then") {
                tok.Stt = TOK_THEN;
                return 1;
            }
            if (tok.StrVal.ToLower() == "long") {
                tok.Stt = TOK_LONG;
                return 1;
            }
            return 1;
        }
        // принимает целое число
        private long IsNumber(ref Token tok) {
            tok.Stt = TOK_I4;
            tok.DataType = DataType.STDT_I4;
            while (true) {
                switch (TOT[(byte)cc]) {
                case DIGIT: {
                    tok.IntVal *= 10;
                    tok.IntVal += cc - '0';
                    break;
                }
                case CHDOT: {
                    return IsReal(ref tok, 0);
                }
                default: {
                    if (tok.DblVal != 0.0 && tok.Stt != TOK_UNKNOWN && tok.Stt == TOK_I4) {
                        tok.Stt = TOK_R8;
                        tok.DblVal = tok.IntVal;
                    }
                    return 1;
                }
                }
                NextChar();
            }
        }
        // разбирает вещественную часть числа
        private int IsReal(ref Token tok, int must) {
            tok.Stt = TOK_R8;
            tok.DataType = DataType.STDT_R8;
            double number = 0.0;
            int div = 0;
            while (true) {
                // точка уже прочитана
                NextChar();
                switch (TOT[(byte)cc]) {
                case DIGIT: {
                    must = 0;
                    number *= 10;
                    number += cc - '0';
                    div++;
                    break;
                }
                case CHDOT: {
                    error_stream.Write("\nЛишняя запятая в REAL. Строка " + tok.LineNumber);
                    return 0;
                }
                default: {
                    tok.DblVal = number / Math.Pow(10, div) + tok.IntVal;
                    return 1;
                }
                }
            }
        }
        // разбирает строковый литерал
        private int IsQuote(ref Token tok) {
            tok.Stt = TOK_QUOTE;
            tok.DataType = DataType.STDT_QUOTE;
            while (true) {
                NextChar();
                // вход по "
                if (TOT[(byte)cc] == QUOTE) {
                    NextChar();
                    return 1;
                } else {
                    if (cc == EOF) {
                        error_stream.Write("\nНеожиданный конец файла в QOUTE. Строка " + tok.LineNumber);
                        return 0;
                    }
                    // LF
                    /*if (cc == '\n') {

                    }*/
                    if (TOT[(byte)cc] == BSLASH) {
                        NextChar();
                        switch ((char)cc) {
                        case 'n': {
                            tok.Append('\n');
                            break;
                        }
                        case 't': {
                            tok.Append('\t');
                            break;
                        }
                        case '\'': {
                            tok.Append('\'');
                            break;
                        }
                        case '\"': {
                            tok.Append('\"');
                            break;
                        }
                        case '\\': {
                            tok.Append('\\');
                            break;
                        }
                        default: {
                            error_stream.Write("\nНеверная escape-последовательность. Строка " + tok.LineNumber);
                            return 0;
                        }
                        }
                    } else {
                        if (cc < 32) {
                            error_stream.Write("\nНепечатаемый символ в строке. Строка " + tok.LineNumber);
                            return 0;
                        }
                        if (tok.StrVal.Length < MAX_QUOTE) {
                            tok.Append((char)cc);
                        }
                    }
                }
            }
        }
        // принимает операцию
        private int IsOpera(ref Token tok) {
            while (true) {
                switch ((char)cc) {
                case '+': {
                    NextChar();
                    tok.Stt = TOK_ADD;
                    return 1;
                }
                case '-': {
                    NextChar();
                    tok.Stt = TOK_SUB;
                    return 1;
                }
                case '*': {
                    NextChar();
                    tok.Stt = TOK_MUL;
                    return 1;
                }
                case '/': {
                    NextChar();
                    tok.Stt = TOK_DIV;
                    return 1;
                }
                case '(': {
                    NextChar();
                    tok.Stt = TOK_LP;
                    return 1;
                }
                case ')': {
                    NextChar();
                    tok.Stt = TOK_RP;
                    return 1;
                }
                case ',': {
                    NextChar();
                    tok.Stt = TOK_COMMA;
                    return 1;
                }
                case '<': {
                    NextChar();
                    if ((char)((byte)cc) == '=') {
                        tok.Stt = TOK_LE;
                        NextChar();
                    } else {
                        if ((char)((byte)cc) == '>') {
                            tok.Stt = TOK_NE;
                            NextChar();
                        } else {
                            tok.Stt = TOK_LT;
                        }
                    }
                    return 1;
                }
                case '>': {
                    NextChar();
                    if ((char)((byte)cc) == '=') {
                        tok.Stt = TOK_GE;
                        NextChar();
                    } else {
                        tok.Stt = TOK_GT;
                    }
                    return 1;
                }
                case '=': {
                    NextChar();
                    if ((char)((byte)cc) == '=') {
                        tok.Stt = TOK_EQ;
                        NextChar();
                    } else {
                        tok.Stt = TOK_ASS;
                    }
                    return 1;
                }
                default: {
                    tok.Stt = TOK_UNKNOWN;
                    error_stream.Write("\nНеожиданный символ. Строка " + tok.LineNumber);
                    return 0;
                }
                }
            }
        }
        // возвращает очередной токен потока
        public long GetNextToken(ref Token tok) {
            // ищем первый символ лексемы
            while (true) {
                if (cc == EOF) {
                    // конец текста
                    tok.Stt = TOK_EOT;
                    return 1;
                } else if ((char)((byte)cc) == '\n') {
                    // строку подсчитываем
                    lineNumber++;
                    // в языке есть токен LF
                    tok.Stt = TOK_LF;
                    NextChar();
                    return 1;

                } else if ((char)((byte)cc) == '\'') {
                    InterpreterSymbol result = GetComment();
                    // comment пропускаем
                } else if (cc < 33) {
                    // управляющие символы и пробел пропускаем
                } else {
                    // другой символ разбираем дальше
                    break;
                }
                // следующий символ
                NextChar();
            }
            // подготавливаем токен
            tok.Reset();
            tok.LineNumber = lineNumber;
            // разбираем первый символ лексемы
            switch (TOT[(byte)cc]) {
            case ALPHA:
            case UNDER:
                return GetId(ref tok);
            case DIGIT:
                return IsNumber(ref tok);
            case CHDOT:
                return IsReal(ref tok, 1);
            case QUOTE:
                return IsQuote(ref tok);
            default:
                return IsOpera(ref tok);
            }
        }
        // конструктор
        public Lexan(StreamReader input_stream, ref StreamWriter error_stream) {
            if (input_stream.CurrentEncoding != Encoding.ASCII) {
                throw new Exception("Входные данные не в ASCII формате");
            }
            this.input_stream = input_stream;
            this.error_stream = error_stream;
            // первый символ потока
            NextChar();
        }
    }
}
