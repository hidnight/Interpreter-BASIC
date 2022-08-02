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
        TOK_DIM,
        TOK_ID,
        TOK_AS,
        TOK_COMMA,
        // \n
        TOK_LF,
        // =
        TOK_ASS,
        // [
        TOK_LB,
        // ]
        TOK_RB,
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
        TOK_I4,
        // real
        TOK_R8,
        // string
        TOK_QUOTE,
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
        SYM_DECLARATIONS,
        SYM_DECLARATION,
        SYM_DECLARATION_LIST,
        SYM_TYPE,
        SYM_DECLARATION_LIST_,
        SYM_ASSIGNMENT_,
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
        SYM_PRIMITIVE_,
        SYM_LAST = SYM_PRIMITIVE_,
        // операционные символы грамматики для ленты ПОЛИЗ
        OUT_ID,
        OUT_DIM,
        OUT_ASS,
        OUT_ID_Q,
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
