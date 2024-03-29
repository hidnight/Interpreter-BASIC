﻿using static BASIC_Interpreter_Library.InterpreterSymbol;

namespace BASIC_Interpreter_Library {
    static class Constants {
        public const int MAX_ID = 32;
        public const int MAX_QUOTE = 256;
        public const short EOF = -1;
        // группы символов
        public const byte INVAL = 0; // недопустимый
        public const byte DIGIT = 1; // цифра
        public const byte ALPHA = 2; // буква
        public const byte OPERA = 3; // операция
        public const byte CHDOT = 4; // точка
        public const byte QUOTE = 5; // двойная кавычка
        public const byte SINQU = 6; // одинарная кавычка
        public const byte UNDER = 7; // знак подчеркивания
        public const byte SLASH = 8; // слеш
        public const byte WHITE = 9; // пробельный символ
        public const byte BSLASH = 10; // обратный слеш
        // таблица перекодировки
        public static readonly byte[] TOT = new byte[256] {
            /* управляющие символы */
            /* 00     01     02     03     04     05     06     07     08     TAB    LF     0B     0C     CR     0E     0F */
            INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, WHITE, INVAL, INVAL, INVAL, WHITE, INVAL, INVAL,
            /* 10     11     12     13     14     15     16     17     18     19     1A     1B     1C     1D     1E     1F */
            INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL,

            /* пробел, знаки, цифры */
            /* 20     !      "      #      $      %      &      '      (      )      *      +      ,      -      .      /  */
            WHITE, INVAL, QUOTE, INVAL, INVAL, INVAL, INVAL, SINQU, OPERA, OPERA, OPERA, OPERA, OPERA, OPERA, CHDOT, SLASH,
            /*  0       1      2      3      4      5      6      7      8      9     :      ;      <      =      >      ? */
            DIGIT, DIGIT, DIGIT, DIGIT, DIGIT, DIGIT, DIGIT, DIGIT, DIGIT, DIGIT, INVAL, INVAL, OPERA, OPERA, OPERA, INVAL,

            /* прописные буквы, знаки */
            /*  @      A      B      C      D      E      F      G      H      I      J      K      L      M      N      O */
            INVAL, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA,
            /*  P      Q      R      S      T      U      V      W      X      Y      Z      [      \      ]      ^      _ */
            ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, INVAL, BSLASH, INVAL, INVAL, UNDER,

            /* строчные буквы, знаки */
            /*  `      a      b      c      d      e      f      g      h      i      j      k      l      m      n      o */
            INVAL, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA,
            /*  p      q      r      s      t      u      v      w      x      y      z      {      |      }      ~        */
            ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, INVAL, INVAL, INVAL, INVAL, INVAL,

            /* вторая половина ASCII - русские или другие интернациональные символы */
            INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL,
            INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL,
            INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL,
            INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL,
            INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL,
            INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL,
            INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL,
            INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL, INVAL
        };
        // размер стека МП-автомата
        public const int MAX_STACK = 64;
        // размер вычислительного стека
        public const int MAX_EXEST = 64;
        // размер таблицы символов
        public const int MAX_SYMS = 16;
        // размер стека меток
        public const int MAX_LABEL = 16;
        // размер ленты
        public const int MAX_STRIP = 64;
        // предел числа итераций
        public const int MAX_IT = 999;
        /* Generated for C by 2016 ReVoL SYNAX */
        /* Adapted to C# by Ivan K.*/
        public const byte ST_MAX_RULE_LEN = 16;
        /* Rules */
        public static readonly InterpreterSymbol[][] RULE = new InterpreterSymbol[54][] {
            new InterpreterSymbol[]/* 0                       */{ TOK_EOT },
            new InterpreterSymbol[]/* 1 SYM_MODULE            */{ SYM_STATEMENT, SYM_MODULE, TOK_EOT },
            new InterpreterSymbol[]/* 2 SYM_MODULE            */{ TOK_EOT },
            new InterpreterSymbol[]/* 3 SYM_STATEMENT         */{ SYM_ASSIGNMENT, TOK_EOT },
            new InterpreterSymbol[]/* 4 SYM_STATEMENT         */{ SYM_BOOLEAN, TOK_EOT },
            new InterpreterSymbol[]/* 5 SYM_STATEMENT         */{ SYM_PRINT, TOK_EOT },
            new InterpreterSymbol[]/* 6 SYM_STATEMENT         */{ SYM_WHILE, TOK_EOT },
            new InterpreterSymbol[]/* 7 SYM_STATEMENT         */{ SYM_EMPTY, TOK_EOT },
            new InterpreterSymbol[]/* 8 SYM_STATEMENT         */{ SYM_DECLARATION, TOK_EOT },
            new InterpreterSymbol[]/* 9 SYM_DECLARATION       */{ TOK_DIM, SYM_DECLARATION_LIST, TOK_EOT },
            new InterpreterSymbol[]/*10 SYM_DECLARATION_LIST  */{ TOK_ID, OUT_ID, TOK_AS, SYM_TYPE, OUT_DIM, SYM_DECLARATION_LIST_, TOK_EOT },
            new InterpreterSymbol[]/*11 SYM_DECLARATION_LIST_ */{ TOK_COMMA, TOK_ID, OUT_ID, TOK_AS, SYM_TYPE, OUT_DIM, SYM_DECLARATION_LIST_, TOK_EOT },
            new InterpreterSymbol[]/*12 SYM_DECLARATION_LIST_ */{ TOK_LF, TOK_EOT },
            new InterpreterSymbol[]/*13 SYM_ASSIGNMENT        */{ TOK_ID, OUT_ID, TOK_ASS, SYM_EXPRESSION, OUT_ASS, TOK_LF, TOK_EOT },
            new InterpreterSymbol[]/*14 SYM_BOOLEAN           */{ TOK_IF, SYM_EXPRESSION, TOK_THEN, TOK_LF, OUT_PUSH, OUT_BZ, SYM_MODULE, SYM_BOOLEAN_END, TOK_EOT },
            new InterpreterSymbol[]/*15 SYM_BOOLEAN_END       */{ TOK_ELSE, TOK_LF, OUT_PUSH, OUT_BR, OUT_SWAP, OUT_DEFL, SYM_MODULE, OUT_POPL, OUT_DEFL, TOK_END, TOK_IF, TOK_LF, TOK_EOT },
            new InterpreterSymbol[]/*16 SYM_BOOLEAN_END       */{ OUT_POPL, OUT_DEFL, TOK_END, TOK_IF, TOK_LF, TOK_EOT },
            new InterpreterSymbol[]/*17 SYM_PRINT             */{ TOK_PRINT, SYM_EXPRESSION,OUT_PRINT,  TOK_LF, TOK_EOT },
            new InterpreterSymbol[]/*18 SYM_WHILE             */{ TOK_WHILE, OUT_PUSH, OUT_DEFL, SYM_EXPRESSION, OUT_PUSH, OUT_BZ, TOK_LF, SYM_MODULE, OUT_SWAP, OUT_BR, OUT_POPL, OUT_DEFL, TOK_END, TOK_WHILE, TOK_LF, TOK_EOT },
            new InterpreterSymbol[]/*19 SYM_EMPTY             */{ TOK_LF, TOK_EOT },
            new InterpreterSymbol[]/*20 SYM_TYPE              */{ TOK_STRING, OUT_STRING, TOK_EOT },
            new InterpreterSymbol[]/*21 SYM_TYPE              */{ TOK_LONG, OUT_LONG, TOK_EOT },
            new InterpreterSymbol[]/*22 SYM_TYPE              */{ TOK_REAL, OUT_REAL, TOK_EOT },
            new InterpreterSymbol[]/*23 SYM_EXPRESSION        */{ SYM_LOGICAL_AND, SYM_LOGICAL_AND_, TOK_EOT },
            new InterpreterSymbol[]/*24 SYM_LOGICAL_AND_      */{ TOK_OR, SYM_LOGICAL_AND, OUT_OR, SYM_LOGICAL_AND_, TOK_EOT },
            new InterpreterSymbol[]/*25 SYM_LOGICAL_AND_      */{ TOK_EOT },
            new InterpreterSymbol[]/*26 SYM_LOGICAL_AND       */{ SYM_LOGICAL_NOT, SYM_LOGICAL_NOT_, TOK_EOT },
            new InterpreterSymbol[]/*27 SYM_LOGICAL_NOT_      */{ TOK_AND, SYM_LOGICAL_NOT, OUT_AND, SYM_LOGICAL_NOT_, TOK_EOT },
            new InterpreterSymbol[]/*28 SYM_LOGICAL_NOT_      */{ TOK_EOT },
            new InterpreterSymbol[]/*29 SYM_LOGICAL_NOT       */{ TOK_NOT, SYM_LOGICAL_NOT, OUT_NOT, TOK_EOT },
            new InterpreterSymbol[]/*30 SYM_LOGICAL_NOT       */{ SYM_RELATION, SYM_RELATION_, TOK_EOT },
            new InterpreterSymbol[]/*31 SYM_RELATION_         */{ TOK_EQ, SYM_RELATION, OUT_EQ, SYM_RELATION_, TOK_EOT },
            new InterpreterSymbol[]/*32 SYM_RELATION_         */{ TOK_NE, SYM_RELATION, OUT_NE, SYM_RELATION_, TOK_EOT },
            new InterpreterSymbol[]/*33 SYM_RELATION_         */{ TOK_LT, SYM_RELATION, OUT_LT, SYM_RELATION_, TOK_EOT },
            new InterpreterSymbol[]/*34 SYM_RELATION_         */{ TOK_GT, SYM_RELATION, OUT_GT, SYM_RELATION_, TOK_EOT },
            new InterpreterSymbol[]/*35 SYM_RELATION_         */{ TOK_LE, SYM_RELATION, OUT_LE, SYM_RELATION_, TOK_EOT },
            new InterpreterSymbol[]/*36 SYM_RELATION_         */{ TOK_GE, SYM_RELATION, OUT_GE, SYM_RELATION_, TOK_EOT },
            new InterpreterSymbol[]/*37 SYM_RELATION_         */{ TOK_EOT },
            new InterpreterSymbol[]/*38 SYM_RELATION          */{ SYM_TERM, SYM_TERM_, TOK_EOT },
            new InterpreterSymbol[]/*39 SYM_TERM_             */{ TOK_ADD, SYM_TERM, OUT_ADD, SYM_TERM_, TOK_EOT },
            new InterpreterSymbol[]/*40 SYM_TERM_             */{ TOK_SUB, SYM_TERM, OUT_SUB, SYM_TERM_, TOK_EOT },
            new InterpreterSymbol[]/*41 SYM_TERM_             */{ TOK_EOT },
            new InterpreterSymbol[]/*42 SYM_TERM              */{ SYM_UNARY, SYM_UNARY_, TOK_EOT },
            new InterpreterSymbol[]/*43 SYM_UNARY_            */{ TOK_MUL, SYM_UNARY, OUT_MUL, SYM_UNARY_, TOK_EOT },
            new InterpreterSymbol[]/*44 SYM_UNARY_            */{ TOK_DIV, SYM_UNARY, OUT_DIV, SYM_UNARY_, TOK_EOT },
            new InterpreterSymbol[]/*45 SYM_UNARY_            */{ TOK_EOT },
            new InterpreterSymbol[]/*46 SYM_UNARY             */{ SYM_PRIMITIVE, TOK_EOT },
            new InterpreterSymbol[]/*47 SYM_UNARY             */{ TOK_SUB, SYM_PRIMITIVE, OUT_U_SUB, TOK_EOT },
            new InterpreterSymbol[]/*48 SYM_UNARY             */{ TOK_ADD, SYM_PRIMITIVE, OUT_U_ADD, TOK_EOT },
            new InterpreterSymbol[]/*49 SYM_PRIMITIVE         */{ TOK_ID, OUT_ID, TOK_EOT },
            new InterpreterSymbol[]/*50 SYM_PRIMITIVE         */{ TOK_I4, OUT_I4, TOK_EOT },
            new InterpreterSymbol[]/*51 SYM_PRIMITIVE         */{ TOK_R8, OUT_R8, TOK_EOT },
            new InterpreterSymbol[]/*52 SYM_PRIMITIVE         */{ TOK_QUOTE, OUT_QUOTE, TOK_EOT },
            new InterpreterSymbol[]/*53 SYM_PRIMITIVE         */{ TOK_LP, SYM_EXPRESSION, TOK_RP, TOK_EOT }
        };
        /* Max rule index */
        public const int MAX_RULE = 53;
        /* Rule Length */
        public static int[] RLEN = new int[] { 0, 2, 0, 1, 1, 1, 1, 1, 1, 2, 4, 5, 1, 4, 6, 6, 2, 3, 7, 1, 1, 1, 1, 2, 3, 0, 2, 3, 0, 2, 2, 3, 3, 3, 3, 3, 3, 0, 2, 3, 3, 0, 2, 3, 3, 0, 1, 2, 2, 1, 1, 1, 1, 3 };
        /* Evaluates actual rule's length */
        public static void GetRuleLen() {
            int rule, count;
            for (rule = 1; rule < RULE.Length; rule++) {
                for (count = 0; count < RULE[rule].Length; count++) {
                    if (RULE[rule][count] == TOK_EOT) {
                        break;
                    }
                }
                RLEN[rule] = count;
            }
        }
        /* ACCEPT CODE */
        public const int ACC = 255;
        /* POP CODE */
        public const int POP = 254;
        /*START SYMBOL */
        public const InterpreterSymbol START = SYM_MODULE;
        /* Syntax Table SYNTA (LL-analysis) */
        public static readonly int[][] SYNTA = new int[][] {
                     /*                        EOT [dim]  [id]  [as]   [,]  [LF]   [=]  [if] [then [else [end] [prin [whil [STRI [LONG [REAL  [or] [and] [not]  [==]  [<>]   [<]   [>]  [<=]  [>=]   [+]   [-]   [*]   [/]  [I4]  [R8] [QUOT   [(]   [)] */
            new int[]/*           <Module>*/{    2,    1,    1,    0,    0,    1,    0,    1,    0,    2,    2,    1,    1,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[]/*        <Statement>*/{    0,    8,    3,    0,    0,    7,    0,    4,    0,    0,    0,    5,    6,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[]/*       <Assignment>*/{    0,    0,   13,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[]/*          <Boolean>*/{    0,    0,    0,    0,    0,    0,    0,   14,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[]/*            <Print>*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   17,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[]/*            <While>*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   18,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[]/*            <Empty>*/{    0,    0,    0,    0,    0,   19,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[]/*      <Declaration>*/{    0,    9,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[]/* <Declaration_list>*/{    0,    0,   10,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[]/*             <Type>*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   20,   21,   22,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[]/*<Declaration_list.>*/{    0,    0,    0,    0,   11,   12,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[]/*       <Expression>*/{    0,    0,   23,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   23,    0,    0,    0,    0,    0,    0,   23,   23,    0,    0,   23,   23,   23,   23,    0 },
            new int[]/*      <Boolean_end>*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,   15,   16,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[]/*      <Logical_AND>*/{    0,    0,   26,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   26,    0,    0,    0,    0,    0,    0,   26,   26,    0,    0,   26,   26,   26,   26,    0 },
            new int[]/*     <Logical_AND.>*/{    0,    0,    0,    0,    0,   25,    0,    0,   25,    0,    0,    0,    0,    0,    0,    0,   24,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   25 },
            new int[]/*      <Logical_NOT>*/{    0,    0,   30,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   29,    0,    0,    0,    0,    0,    0,   30,   30,    0,    0,   30,   30,   30,   30,    0 },
            new int[]/*     <Logical_NOT.>*/{    0,    0,    0,    0,    0,   28,    0,    0,   28,    0,    0,    0,    0,    0,    0,    0,   28,   27,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   28 },
            new int[]/*         <Relation>*/{    0,    0,   38,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   38,   38,    0,    0,   38,   38,   38,   38,    0 },
            new int[]/*        <Relation.>*/{    0,    0,    0,    0,    0,   37,    0,    0,   37,    0,    0,    0,    0,    0,    0,    0,   37,   37,    0,   31,   32,   33,   34,   35,   36,    0,    0,    0,    0,    0,    0,    0,    0,   37 },
            new int[]/*             <Term>*/{    0,    0,   42,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   42,   42,    0,    0,   42,   42,   42,   42,    0 },
            new int[]/*            <Term.>*/{    0,    0,    0,    0,    0,   41,    0,    0,   41,    0,    0,    0,    0,    0,    0,    0,   41,   41,    0,   41,   41,   41,   41,   41,   41,   39,   40,    0,    0,    0,    0,    0,    0,   41 },
            new int[]/*            <Unary>*/{    0,    0,   46,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   48,   47,    0,    0,   46,   46,   46,   46,    0 },
            new int[]/*           <Unary.>*/{    0,    0,    0,    0,    0,   45,    0,    0,   45,    0,    0,    0,    0,    0,    0,    0,   45,   45,    0,   45,   45,   45,   45,   45,   45,   45,   45,   43,   44,    0,    0,    0,    0,   45 },
            new int[]/*        <Primitive>*/{    0,    0,   49,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   50,   51,   52,   53,    0 }
        };
        // признак присутствия
        public const int ST_EXISTS = -2;
        // признак отсутствия
        public const int ST_NOTFOUND = -3;
    }
}
