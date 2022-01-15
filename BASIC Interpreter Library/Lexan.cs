using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BASIC_Interpreter_Library.Interpreter_symbol;
using static BASIC_Interpreter_Library.Constants;

namespace BASIC_Interpreter_Library {
    public class Lexan {
        private FileStream input_stream;
        private FileStream error_stream;
        // текущий символ входа
        sbyte cc;
        // возвращает очередной символ входа
        void next_char() {
            cc = (sbyte)input_stream.ReadByte();
        }
        Interpreter_symbol get_comment() {
            while (true) {
                next_char();
                if (cc == 10) {
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
                case DIGIT: {
                    if (tok.str_val.Length < MAX_ID)
                        tok.Append((char)((byte)cc));
                    break;
                }
                default: {
                    return is_keyword(ref tok);
                }
                }
                next_char();
            }
        }
        // определяет ключевое слово
        int is_keyword(ref Token tok) {

            if (tok.str_val.ToLower() == "dim") {
                tok.Stt = TOK_DIM;
                return 1;
            }
            if (tok.str_val.ToLower() == "as") {
                tok.Stt = TOK_AS;
                return 1;
            }
            if (tok.str_val.ToLower() == "end_if") {
                tok.Stt = TOK_END_IF;
                return 1;
            }
            if (tok.str_val.ToLower() == "or") {
                tok.Stt = TOK_OR;
                return 1;
            }
            if (tok.str_val.ToLower() == "and") {
                tok.Stt = TOK_AND;
                return 1;
            }
            if (tok.str_val.ToLower() == "not") {
                tok.Stt = TOK_NOT;
                return 1;
            }
            if (tok.str_val.ToLower() == "while") {
                tok.Stt = TOK_WHILE;
                return 1;
            }
            if (tok.str_val.ToLower() == "end_while") {
                tok.Stt = TOK_END_WHILE;
                return 1;
            }
            if (tok.str_val.ToLower() == "if") {
                tok.Stt = TOK_IF;
                return 1;
            }
            if (tok.str_val.ToLower() == "print") {
                tok.Stt = TOK_PRINT;
                return 1;
            }
            if (tok.str_val.ToLower() == "real") {
                tok.Stt = TOK_REAL;
                return 1;
            }
            if (tok.str_val.ToLower() == "string") {
                tok.Stt = TOK_STRING;
                return 1;
            }
            if (tok.str_val.ToLower() == "else") {
                tok.Stt = TOK_ELSE;
                return 1;
            }

            if (tok.str_val.ToLower() == "then") {
                tok.Stt = TOK_THEN;
                return 1;
            }
            return 1;
        }
        // принимает целое число
        int is_number(ref Token tok) {
            tok.Stt = TOK_I4;
            while (true) {
                switch (TOT[(byte)cc]) {
                case DIGIT: {
                    tok.Int_val *= 10;
                    tok.Int_val += cc - 30 /* '0' */;
                    break;
                }
                case CHDOT: {
                    return is_real(ref tok, 0);
                }
                default: {
                    if (tok.Dbl_val == 0.0 && tok.Stt != TOK_UNKNOWN && tok.Stt == TOK_I4) {
                        tok.Stt = TOK_R8;
                        tok.Dbl_val = tok.Int_val;
                    }
                    return 1;
                }
                }
                next_char();
            }
        }
        // разбирает вещественную часть числа
        int is_real(ref Token tok, int must) {
            tok.Stt = TOK_R8;
            double number = 0.0;
            int div = 0;
            while (true) {
                // точка уже прочитана
                next_char();
                switch (TOT[(byte)cc]) {
                case DIGIT: {
                    must = 0;
                    number *= 10;
                    number += cc - 30 /* '0' */;
                    div++;
                    break;
                }
                case CHDOT: {
                    error_stream.Write(Encoding.ASCII.GetBytes("\nlexan: extra dot in float\n\n"), 0, 256);
                    return 0;
                }
                default: {
                    if (must == 1) {
                        error_stream.Write(Encoding.ASCII.GetBytes("\nlexan: single dot in float\n\n"), 0, 256);
                    }
                    tok.Dbl_val = tok.Int_val + number / Math.Pow(10, div);
                    return 1;
                }
                }
            }
        }
        // разбирает строковый литерал
        int is_quote(ref Token tok) {
            tok.Stt = TOK_QUOTE;
            while (true) {
                next_char();
                if ((TOT[(byte)cc]) == QUOTE) {
                    next_char();
                    if ((TOT[(byte)cc]) == QUOTE) {
                        return 1;
                    } else {
                        if (tok.str_val.Length < MAX_QUOTE)
                            tok.Append((char)((byte)cc));
                    }

                } else {
                    if (cc == EOF) {
                        error_stream.Write(Encoding.ASCII.GetBytes("\nlexan: unexpected end of file in quote\n\n"), 0, 256);
                        return 0;
                    }
                    if ((TOT[(byte)cc]) < 32) {
                        error_stream.Write(Encoding.ASCII.GetBytes("\nlexan: extra character in quote\n\n"), 0, 256);
                        return 0;
                    }
                    if (tok.str_val.Length < MAX_QUOTE)
                        tok.Append((char)((byte)cc));
                }
            }
        }
        // принимает операцию
        int is_opera(ref Token tok) {
            while (true) {
                switch ((char)((byte)cc)) {
                case '+': {
                    next_char();
                    tok.Stt = TOK_ADD;
                    return 1;
                }
                case '-': {
                    next_char();
                    tok.Stt = TOK_SUB;
                    return 1;
                }
                case '*': {
                    next_char();
                    tok.Stt = TOK_MUL;
                    return 1;
                }
                case '/': {
                    next_char();
                    tok.Stt = TOK_DIV;
                    return 1;
                }
                case '(': {
                    next_char();
                    tok.Stt = TOK_LP;
                    return 1;
                }
                case ')': {
                    next_char();
                    tok.Stt = TOK_RP;
                    return 1;
                }
                case ',': {
                    next_char();
                    tok.Stt = TOK_COMMA;
                    return 1;
                }
                case '<': {
                        next_char();
                        if ((char)((byte)cc) == '=') {
                            tok.Stt = TOK_LE;
                            next_char();
                        } else {
                            tok.Stt = TOK_LT;
                        }
                        return 1;
                }
                case '>': {
                    next_char();
                    if ((char)((byte)cc) == '=') {
                        tok.Stt = TOK_GE;
                        next_char();
                    } else {
                        tok.Stt = TOK_GT;
                    }
                    return 1;
                }
                case '=': {
                    next_char();
                    if ((char)((byte)cc) == '=') {
                        tok.Stt = TOK_EQ;
                        next_char();
                    } else {
                        tok.Stt = TOK_ASS;
                    }
                    return 1;
                }
                default: {
                    tok.Stt = TOK_UNKNOWN;
                    error_stream.Write(Encoding.ASCII.GetBytes("\nlexan: extra character in line\n\n"), 0, 256);
                    return 0;
                }
                }
            }
        }
        // возвращает очередной токен потока
        public int Next_token(ref Token tok) {
            // ищем первый символ лексемы
            while (true) {
                if (cc == EOF) {
                    // конец текста
                    tok.Stt = TOK_EOT;
                    return 1;
                } else if ((char)((byte)cc) == '\n') {
                    // строку подсчитываем
                    // если в языке есть токен LF
                    tok.Stt = TOK_LF;
                    next_char();
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
                next_char();
            }
            // подготавливаем токен
            tok.reset();
            // разбираем первый символ лексемы
            switch (TOT[(byte)cc]) {
            case ALPHA:
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
        public Lexan(ref FileStream input_stream, ref FileStream error_stream) {
            this.input_stream = input_stream;
            this.error_stream = error_stream;
            // первый символ потока
            next_char();
        }
    }
}
