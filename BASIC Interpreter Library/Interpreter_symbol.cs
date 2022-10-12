using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASIC_Interpreter_Library {
    public enum Interpreter_symbol {
        OUT_START = 9997,
        // комментарий
        TOK_COMMENT,
        // неизвестно
        TOK_UNKNOWN,
        // токены
        // конец текста
        TOK_EOT = 0,
        TOK_DIM,        // ключевое слово
        TOK_ID,
        TOK_AS,         // ключевое слово
        TOK_COMMA,
        // \n
        TOK_LF,
        // =
        TOK_ASS,
        TOK_IF,         // ключевое слово
        TOK_THEN,       // ключевое слово
        TOK_ELSE,       // ключевое слово
        TOK_END,        // ключевое слово
        TOK_PRINT,      // ключевое слово
        TOK_WHILE,      // ключевое слово
        TOK_STRING,     // ключевое слово
        TOK_LONG,       // ключевое слово
        TOK_REAL,       // ключевое слово
        TOK_OR,
        TOK_AND,
        TOK_NOT,
        // ==
        TOK_EQ,
        // <>
        TOK_NE,
        // <
        TOK_LT,
        // >
        TOK_GT,
        // <=
        TOK_LE,
        // >=
        TOK_GE,
        // +
        TOK_ADD,
        // -
        TOK_SUB,
        // *
        TOK_MUL,
        // /
        TOK_DIV,
        // long
        TOK_I4,     // константа
        // real
        TOK_R8,     // константа
        // string
        TOK_QUOTE,  // константа
        // (
        TOK_LP,
        // )
        TOK_RP,
        TOK_LAST = TOK_RP,
        // нетерминалы
        SYM_MODULE,
        SYM_STATEMENT,
        SYM_ASSIGNMENT,
        SYM_BOOLEAN,
        SYM_PRINT,
        SYM_WHILE,
        SYM_EMPTY,
        SYM_DECLARATION,
        SYM_DECLARATION_LIST,
        SYM_TYPE,
        SYM_DECLARATION_LIST_,
        SYM_EXPRESSION,
        SYM_BOOLEAN_END,
        SYM_LOGICAL_AND,
        SYM_LOGICAL_AND_,
        SYM_LOGICAL_NOT,
        SYM_LOGICAL_NOT_,
        SYM_RELATION,
        SYM_RELATION_,
        SYM_TERM,
        SYM_TERM_,
        SYM_UNARY,
        SYM_UNARY_,
        SYM_PRIMITIVE,
        SYM_LAST = SYM_PRIMITIVE,
        // операционные символы грамматики для ленты ПОЛИЗ
        OUT_ID,
        OUT_DIM,
        OUT_ASS,
        OUT_PUSH,
        OUT_BZ,
        OUT_BR,
        OUT_SWAP,
        OUT_DEFL,
        OUT_POPL,
        OUT_PRINT,
        OUT_STRING,
        OUT_LONG,
        OUT_REAL,
        OUT_OR,
        OUT_AND,
        OUT_NOT,
        OUT_EQ,
        OUT_NE,
        OUT_LT,
        OUT_GT,
        OUT_LE,
        OUT_GE,
        OUT_ADD,
        OUT_SUB,
        OUT_MUL,
        OUT_DIV,
        OUT_U_SUB,
        OUT_U_ADD,
        OUT_I4,
        OUT_R8,
        OUT_QUOTE,
        OUT_LABEL,
        OUT_END
    }
}
