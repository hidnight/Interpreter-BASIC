using System;
using System.IO;
using System.Text;
using static BASIC_Interpreter_Library.Interpreter_symbol;
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
        void Next_char() {
            cc = (sbyte)input_stream.Read();
        }
        // возвращает токен комментария
        // вход по '
        Interpreter_symbol get_comment() {
            while (true) {
                Next_char();
                // \n
                if (cc == '\n' || cc == EOF) {
                    return TOK_COMMENT;
                }
            }
        }
        // принимает идентификатор
        int get_id(ref Token tok) {
            tok.Stt = TOK_ID;
            while (true) {
                // вход только по букве, поэтому
                // нет разницы между буквой и цифрой
                switch (TOT[(byte)cc]) {
                case ALPHA:
                case DIGIT:
                case UNDER: {
                    if (tok.Str_val.Length < MAX_ID)
                        tok.Append((char)(byte)cc);
                    break;
                }
                default: {
                    int result = is_keyword(ref tok);
                    if (tok.Stt == TOK_ID) {
                        tok.Name = tok.Str_val;
                        tok.Str_val = "";
                    }
                    return result;
                }
                }
                Next_char();
            }
        }
        // определяет ключевое слово
        int is_keyword(ref Token tok) {
            if (tok.Str_val.ToLower() == "dim") {
                tok.Stt = TOK_DIM;
                return 1;
            }
            if (tok.Str_val.ToLower() == "as") {
                tok.Stt = TOK_AS;
                return 1;
            }
            if (tok.Str_val.ToLower() == "end") {
                tok.Stt = TOK_END;
                return 1;
            }
            if (tok.Str_val.ToLower() == "or") {
                tok.Stt = TOK_OR;
                return 1;
            }
            if (tok.Str_val.ToLower() == "and") {
                tok.Stt = TOK_AND;
                return 1;
            }
            if (tok.Str_val.ToLower() == "not") {
                tok.Stt = TOK_NOT;
                return 1;
            }
            if (tok.Str_val.ToLower() == "while") {
                tok.Stt = TOK_WHILE;
                return 1;
            }
            if (tok.Str_val.ToLower() == "if") {
                tok.Stt = TOK_IF;
                return 1;
            }
            if (tok.Str_val.ToLower() == "print") {
                tok.Stt = TOK_PRINT;
                return 1;
            }
            if (tok.Str_val.ToLower() == "real") {
                tok.Stt = TOK_REAL;
                return 1;
            }
            if (tok.Str_val.ToLower() == "string") {
                tok.Stt = TOK_STRING;
                return 1;
            }
            if (tok.Str_val.ToLower() == "else") {
                tok.Stt = TOK_ELSE;
                return 1;
            }
            if (tok.Str_val.ToLower() == "then") {
                tok.Stt = TOK_THEN;
                return 1;
            }
            if (tok.Str_val.ToLower() == "long") {
                tok.Stt = TOK_LONG;
                return 1;
            }
            return 1;
        }
        // принимает целое число
        long is_number(ref Token tok) {
            tok.Stt = TOK_I4;
            tok.Data_Type = Data_type.STDT_I4;
            while (true) {
                switch (TOT[(byte)cc]) {
                case DIGIT: {
                    tok.Int_val *= 10;
                    tok.Int_val += cc - '0';
                    break;
                }
                case CHDOT: {
                    return is_real(ref tok, 0);
                }
                default: {
                    if (tok.Dbl_val != 0.0 && tok.Stt != TOK_UNKNOWN && tok.Stt == TOK_I4) {
                        tok.Stt = TOK_R8;
                        tok.Dbl_val = tok.Int_val;
                    }
                    return 1;
                }
                }
                Next_char();
            }
        }
        // разбирает вещественную часть числа
        int is_real(ref Token tok, int must) {
            tok.Stt = TOK_R8;
            tok.Data_Type = Data_type.STDT_R8;
            double number = 0.0;
            int div = 0;
            while (true) {
                // точка уже прочитана
                Next_char();
                switch (TOT[(byte)cc]) {
                case DIGIT: {
                    must = 0;
                    number *= 10;
                    number += cc - '0';
                    div++;
                    break;
                }
                case CHDOT: {
                    error_stream.Write("\nЛишняя запятая в REAL. Строка " + tok.Line_Number);
                    return 0;
                }
                default: {
                    tok.Dbl_val = number / Math.Pow(10, div) + tok.Int_val;
                    return 1;
                }
                }
            }
        }
        // разбирает строковый литерал
        int is_quote(ref Token tok) {
            tok.Stt = TOK_QUOTE;
            tok.Data_Type = Data_type.STDT_QUOTE;
            while (true) {
                Next_char();
                // вход по "
                if ((TOT[(byte)cc]) == QUOTE) {
                    Next_char();
                    if ((TOT[(byte)cc]) != QUOTE) {
                        return 1;
                    } else {
                        if (tok.Str_val.Length < MAX_QUOTE) {
                            tok.Append((char)((byte)cc));
                        }
                    }
                } else {
                    if (cc == EOF) {
                        error_stream.Write("\nНеожиданный конец файла в QOUTE. Строка "+  tok.Line_Number);
                        return 0;
                    }
                    // LF
                    /*if (cc == '\n') {

                    }*/
                    if (cc < 32) {
                        error_stream.Write("\nНепечатаемый символ в строке. Строка " + tok.Line_Number);
                        return 0;
                    }
                    if (tok.Str_val.Length < MAX_QUOTE)
                        tok.Append((char)((byte)cc));
                }
            }
        }
        // принимает операцию
        int is_opera(ref Token tok) {
            while (true) {
                switch ((char)((byte)cc)) {
                case '+': {
                    Next_char();
                    tok.Stt = TOK_ADD;
                    return 1;
                }
                case '-': {
                    Next_char();
                    tok.Stt = TOK_SUB;
                    return 1;
                }
                case '*': {
                    Next_char();
                    tok.Stt = TOK_MUL;
                    return 1;
                }
                case '/': {
                    Next_char();
                    tok.Stt = TOK_DIV;
                    return 1;
                }
                case '(': {
                    Next_char();
                    tok.Stt = TOK_LP;
                    return 1;
                }
                case ')': {
                    Next_char();
                    tok.Stt = TOK_RP;
                    return 1;
                }
                case ',': {
                    Next_char();
                    tok.Stt = TOK_COMMA;
                    return 1;
                }
                case '<': {
                    Next_char();
                    if ((char)((byte)cc) == '=') {
                        tok.Stt = TOK_LE;
                        Next_char();
                    } else {
                        if ((char)((byte)cc) == '>') {
                            tok.Stt = TOK_NE;
                            Next_char();
                        } else {
                            tok.Stt = TOK_LT;
                        }
                    }
                    return 1;
                }
                case '>': {
                    Next_char();
                    if ((char)((byte)cc) == '=') {
                        tok.Stt = TOK_GE;
                        Next_char();
                    } else {
                        tok.Stt = TOK_GT;
                    }
                    return 1;
                }
                case '=': {
                    Next_char();
                    if ((char)((byte)cc) == '=') {
                        tok.Stt = TOK_EQ;
                        Next_char();
                    } else {
                        tok.Stt = TOK_ASS;
                    }
                    return 1;
                }
                default: {
                    tok.Stt = TOK_UNKNOWN;
                    error_stream.Write("\nНеожиданный символ. Строка " + tok.Line_Number);
                    return 0;
                }
                }
            }
        }
        // возвращает очередной токен потока
        public long Next_token(ref Token tok) {
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
                    Next_char();
                    return 1;

                } else if ((char)((byte)cc) == '\'') {
                    Interpreter_symbol result = get_comment();
                    // comment пропускаем
                } else if (cc < 33) {
                    // управляющие символы и пробел пропускаем
                } else {
                    // другой символ разбираем дальше
                    break;
                }
                // следующий символ
                Next_char();
            }
            // подготавливаем токен
            tok.Reset();
            tok.Line_Number = lineNumber;
            // разбираем первый символ лексемы
            switch (TOT[(byte)cc]) {
            case ALPHA:
            case UNDER:
                return get_id(ref tok);
            case DIGIT:
                return is_number(ref tok);
            case CHDOT:
                return is_real(ref tok, 1);
            case QUOTE:
                return is_quote(ref tok);
            default:
                return is_opera(ref tok);
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
            Next_char();
        }
    }
}
