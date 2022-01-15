using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASIC_Interpreter_Library {
    enum Interpreter_symbol {
        OUT_START = 997,
        // комментарий
        TOK_COMMENT,
        // неизвестно
        TOK_UNKNOWN,
        // токены
        TOK_EOT = 0,
        TOK_LF,
        TOK_DIM,
        TOK_ID,
        TOK_AS,
        TOK_COMMA,
        TOK_ASS,
        TOK_IF,
        TOK_THEN,
        TOK_ELSE,
        TOK_END_IF,
        TOK_PRINT,
        TOK_WHILE,
        TOK_END_WHILE,
        TOK_STRING,
        TOK_LONG,
        TOK_REAL,
        TOK_OR,
        TOK_AND,
        TOK_NOT,
        TOK_EQ,
        TOK_NE,
        TOK_LT,
        TOK_GT,
        TOK_LE,
        TOK_GE,
        TOK_ADD,
        TOK_SUB,
        TOK_MUL,
        TOK_DIV,
        TOK_I4,
        TOK_R8,
        TOK_QUOTE,
        TOK_LP,
        TOK_RP,
        TOK_LAST = TOK_RP,
        // нетерминалы
        SYM_MODULE,
        SYM_STATEMENT,
        SYM_DECLARATION,
        SYM_ASSIGNMENT,
        SYM_BOOLEAN,
        SYM_PRINT,
        SYM_WHILE,
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
        SYM_LAST = SYM_PRIMITIVE
        // конец символов грамматики
    }
}
