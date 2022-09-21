using static BASIC_Interpreter_Library.Interpreter_symbol;

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

        // таблица перекодировки
        public static readonly byte[] TOT = new byte[256] { /* (c) 2003 ReVoL */
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
            ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, ALPHA, INVAL, INVAL, INVAL, INVAL, UNDER,

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
        public static readonly Interpreter_symbol[][] RULE = new Interpreter_symbol[55][] {
            new Interpreter_symbol[]/* 0                       */{ TOK_EOT },
            new Interpreter_symbol[]/* 1 SYM_MODULE            */{ SYM_STATEMENT, SYM_MODULE, TOK_EOT },
            new Interpreter_symbol[]/* 2 SYM_MODULE            */{ TOK_EOT },
            new Interpreter_symbol[]/* 3 SYM_STATEMENT         */{ SYM_ASSIGNMENT, TOK_EOT },
            new Interpreter_symbol[]/* 4 SYM_STATEMENT         */{ SYM_BOOLEAN, TOK_EOT },
            new Interpreter_symbol[]/* 5 SYM_STATEMENT         */{ SYM_PRINT, TOK_EOT },
            new Interpreter_symbol[]/* 6 SYM_STATEMENT         */{ SYM_WHILE, TOK_EOT },
            new Interpreter_symbol[]/* 7 SYM_STATEMENT         */{ SYM_EMPTY, TOK_EOT },
            new Interpreter_symbol[]/* 8 SYM_STATEMENT         */{ SYM_DECLARATIONS, TOK_EOT },
            new Interpreter_symbol[]/* 9 SYM_DECLARATIONS      */{ SYM_DECLARATION, SYM_DECLARATIONS, TOK_EOT },
            new Interpreter_symbol[]/*10 SYM_DECLARATION       */{ TOK_DIM, SYM_DECLARATION_LIST, TOK_EOT },
            new Interpreter_symbol[]/*11 SYM_DECLARATION_LIST  */{ TOK_ID, OUT_ID, TOK_AS, SYM_TYPE, OUT_DIM, SYM_DECLARATION_LIST_, TOK_EOT },
            new Interpreter_symbol[]/*12 SYM_DECLARATION_LIST_ */{ TOK_COMMA, TOK_ID, OUT_ID, TOK_AS, SYM_TYPE, OUT_DIM, SYM_DECLARATION_LIST_, TOK_EOT },
            new Interpreter_symbol[]/*13 SYM_DECLARATION_LIST_ */{ TOK_LF, TOK_EOT },
            new Interpreter_symbol[]/*14 SYM_ASSIGNMENT        */{ TOK_ID, OUT_ID, TOK_EQ, SYM_EXPRESSION, OUT_ASS, TOK_LF, TOK_EOT },
            new Interpreter_symbol[]/*15 SYM_BOOLEAN           */{ TOK_IF, SYM_EXPRESSION, TOK_THEN, OUT_PUSH, OUT_BZ, SYM_MODULE, SYM_BOOLEAN_END, TOK_EOT },
            new Interpreter_symbol[]/*16 SYM_BOOLEAN_END       */{ TOK_ELSE, OUT_PUSH, OUT_BR, OUT_SWAP, OUT_DEFL, TOK_LF, SYM_MODULE, OUT_POPL, OUT_DEFL, TOK_END_IF, TOK_LF, TOK_EOT },
            new Interpreter_symbol[]/*17 SYM_BOOLEAN_END       */{ OUT_POPL, OUT_DEFL, TOK_END_IF, TOK_LF, TOK_EOT },
            new Interpreter_symbol[]/*18 SYM_PRINT             */{ TOK_PRINT, SYM_EXPRESSION, OUT_PRINT, TOK_LF, TOK_EOT },
            new Interpreter_symbol[]/*19 SYM_WHILE             */{ TOK_WHILE, SYM_EXPRESSION, OUT_PUSH, OUT_BZ, SYM_MODULE, OUT_POPL, OUT_DEFL, TOK_END_WHILE, TOK_LF, TOK_EOT },
            new Interpreter_symbol[]/*20 SYM_EMPTY             */{ TOK_LF, TOK_EOT },
            new Interpreter_symbol[]/*21 SYM_TYPE              */{ TOK_STRING, OUT_STRING, TOK_EOT },
            new Interpreter_symbol[]/*22 SYM_TYPE              */{ TOK_LONG, OUT_LONG, TOK_EOT },
            new Interpreter_symbol[]/*23 SYM_TYPE              */{ TOK_REAL, OUT_REAL, TOK_EOT },
            new Interpreter_symbol[]/*24 SYM_EXPRESSION        */{ SYM_LOGICAL_AND, SYM_LOGICAL_AND_, TOK_EOT },
            new Interpreter_symbol[]/*25 SYM_LOGICAL_AND_      */{ TOK_OR, SYM_LOGICAL_AND, SYM_LOGICAL_AND_, OUT_OR, TOK_EOT },
            new Interpreter_symbol[]/*26 SYM_LOGICAL_AND_      */{ TOK_EOT },
            new Interpreter_symbol[]/*27 SYM_LOGICAL_AND       */{ SYM_LOGICAL_NOT, SYM_LOGICAL_NOT_, TOK_EOT },
            new Interpreter_symbol[]/*28 SYM_LOGICAL_NOT_      */{ TOK_AND, SYM_LOGICAL_NOT, SYM_LOGICAL_NOT_, OUT_AND, TOK_EOT },
            new Interpreter_symbol[]/*29 SYM_LOGICAL_NOT_      */{ TOK_EOT },
            new Interpreter_symbol[]/*30 SYM_LOGICAL_NOT       */{ TOK_NOT, SYM_LOGICAL_NOT, OUT_NOT, TOK_EOT },
            new Interpreter_symbol[]/*31 SYM_LOGICAL_NOT       */{ SYM_RELATION, SYM_RELATION_, TOK_EOT },
            new Interpreter_symbol[]/*32 SYM_RELATION_         */{ TOK_EQ, SYM_RELATION, SYM_RELATION_, OUT_EQ, TOK_EOT },
            new Interpreter_symbol[]/*33 SYM_RELATION_         */{ TOK_NE, SYM_RELATION, SYM_RELATION_, OUT_NE, TOK_EOT },
            new Interpreter_symbol[]/*34 SYM_RELATION_         */{ TOK_LT, SYM_RELATION, SYM_RELATION_, OUT_LT, TOK_EOT },
            new Interpreter_symbol[]/*35 SYM_RELATION_         */{ TOK_GT, SYM_RELATION, SYM_RELATION_, OUT_GT, TOK_EOT },
            new Interpreter_symbol[]/*36 SYM_RELATION_         */{ TOK_LE, SYM_RELATION, SYM_RELATION_, OUT_LE, TOK_EOT },
            new Interpreter_symbol[]/*37 SYM_RELATION_         */{ TOK_GE, SYM_RELATION, SYM_RELATION_, OUT_GE, TOK_EOT },
            new Interpreter_symbol[]/*38 SYM_RELATION_         */{ TOK_EOT },
            new Interpreter_symbol[]/*39 SYM_RELATION          */{ SYM_TERM, SYM_TERM_, TOK_EOT },
            new Interpreter_symbol[]/*40 SYM_TERM_             */{ TOK_ADD, SYM_TERM, SYM_TERM_, OUT_ADD, TOK_EOT },
            new Interpreter_symbol[]/*41 SYM_TERM_             */{ TOK_SUB, SYM_TERM, SYM_TERM_, OUT_SUB, TOK_EOT },
            new Interpreter_symbol[]/*42 SYM_TERM_             */{ TOK_EOT },
            new Interpreter_symbol[]/*43 SYM_TERM              */{ SYM_UNARY, SYM_UNARY_, TOK_EOT },
            new Interpreter_symbol[]/*44 SYM_UNARY_            */{ TOK_MUL, SYM_UNARY, SYM_UNARY_, OUT_MUL, TOK_EOT },
            new Interpreter_symbol[]/*45 SYM_UNARY_            */{ TOK_DIV, SYM_UNARY, SYM_UNARY_, OUT_DIV, TOK_EOT },
            new Interpreter_symbol[]/*46 SYM_UNARY_            */{ TOK_EOT },
            new Interpreter_symbol[]/*47 SYM_UNARY             */{ SYM_PRIMITIVE, TOK_EOT },
            new Interpreter_symbol[]/*48 SYM_UNARY             */{ TOK_SUB, SYM_PRIMITIVE, OUT_U_SUB, TOK_EOT },
            new Interpreter_symbol[]/*49 SYM_UNARY             */{ TOK_ADD, SYM_PRIMITIVE, OUT_U_ADD, TOK_EOT },
            new Interpreter_symbol[]/*50 SYM_PRIMITIVE         */{ TOK_ID, OUT_ID, TOK_EOT },
            new Interpreter_symbol[]/*51 SYM_PRIMITIVE         */{ TOK_I4, OUT_I4, TOK_EOT },
            new Interpreter_symbol[]/*52 SYM_PRIMITIVE         */{ TOK_R8, OUT_R8, TOK_EOT },
            new Interpreter_symbol[]/*53 SYM_PRIMITIVE         */{ TOK_QUOTE, OUT_QUOTE, TOK_EOT },
            new Interpreter_symbol[]/*54 SYM_PRIMITIVE         */{ TOK_LP, SYM_EXPRESSION, TOK_RP, TOK_EOT }
        };
        /* Max rule index */
        public const int MAX_RULE = 54;
        /* Rule Length */
        public static int[] RLEN = new int[] { 0, 2, 0, 1, 1, 1, 1, 1, 1, 2, 2, 4, 5, 1, 4, 5, 5, 2, 3, 5, 1, 1, 1, 1, 2, 3, 0, 2, 3, 0, 2, 2, 3, 3, 3, 3, 3, 3, 0, 2, 3, 3, 0, 2, 3, 3, 0, 1, 2, 2, 1, 1, 1, 1, 3 };
        /* Evaluates actual rule's length */
        public static void get_rule_len() {
            int rule, count;
            for (rule = 1; rule < RULE.Length; rule++) {
                for (count = 0; count < RULE[rule].Length; count++) {
                    if (RULE[rule][count] == TOK_EOT) {
                        break;
                    }
                }
                    
                /*{
                    if (RULE[rule][count] == TOK_EOT)
                        break;
                }*/
                RLEN[rule] = count;
            }
        }
        /* ACCEPT CODE */
        public const int ACC = 255;
        /* POP CODE */
        public const int POP = 254;
        /*START SYMBOL */
        public const Interpreter_symbol START = SYM_MODULE;
        /* Syntax Table SYNTA (LL-analysis) */
        public static readonly int[][] SYNTA = new int[60][] {
            /*                                   EOT [dim]  [id]  [as]   [,]  [LF]   [=]  [if] [then [else [end_ [prin [whil [end_ [STRI [LONG [REAL  [or] [and] [not]  [==]  [<>]   [<]   [>]  [<=]  [>=]   [+]   [-]   [*]   [/]  [I4]  [R8] [QUOT   [(]   [)] */
            new int[35]/*                   */{  ACC,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*              [dim]*/{    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*               [id]*/{    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*               [as]*/{    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*                [,]*/{    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*               [LF]*/{    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*                [=]*/{    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*               [if]*/{    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*             [then]*/{    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*             [else]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*           [end_if]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*            [print]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*            [while]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*        [end_while]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*           [STRING]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*             [LONG]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*             [REAL]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*               [or]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*              [and]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*              [not]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*               [==]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*               [<>]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*                [<]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*                [>]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*               [<=]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*               [>=]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*                [+]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*                [-]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*                [*]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0 },
            new int[35]/*                [/]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0 },
            new int[35]/*               [I4]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0 },
            new int[35]/*               [R8]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0 },
            new int[35]/*            [QUOTE]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0 },
            new int[35]/*                [(]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0 },
            new int[35]/*                [)]*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP },
            /*                                   EOT [dim]  [id]  [as]   [,]  [LF]   [=]  [if] [then [else [end_ [prin [whil [end_ [STRI [LONG [REAL  [or] [and] [not]  [==]  [<>]   [<]   [>]  [<=]  [>=]   [+]   [-]   [*]   [/]  [I4]  [R8] [QUOT   [(]   [)] */
            new int[35]/*           <Module>*/{    2,    1,    1,    0,    0,    1,    0,    1,    0,    2,    2,    1,    1,    2,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*        <Statement>*/{    0,    8,    3,    0,    0,    7,    0,    4,    0,    0,    0,    5,    6,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*       <Assignment>*/{    0,    0,   14,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*          <Boolean>*/{    0,    0,    0,    0,    0,    0,    0,   15,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*            <Print>*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   18,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*            <While>*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   19,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*            <Empty>*/{    0,    0,    0,    0,    0,   20,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*     <Declarations>*/{    0,    9,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*      <Declaration>*/{    0,   10,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/* <Declaration_list>*/{    0,    0,   11,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*             <Type>*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   21,   22,   23,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*<Declaration_list.>*/{    0,    0,    0,    0,   12,   13,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*       <Expression>*/{    0,    0,   24,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   24,    0,    0,    0,    0,    0,    0,   24,   24,    0,    0,   24,   24,   24,   24,    0 },
            new int[35]/*      <Boolean_end>*/{    0,    0,    0,    0,    0,    0,    0,    0,    0,   16,   17,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
            new int[35]/*      <Logical_AND>*/{    0,    0,   27,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   27,    0,    0,    0,    0,    0,    0,   27,   27,    0,    0,   27,   27,   27,   27,    0 },
            new int[35]/*     <Logical_AND.>*/{   26,   26,   26,    0,    0,   26,    0,   26,   26,   26,   26,   26,   26,   26,    0,    0,    0,   25,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   26 },
            new int[35]/*      <Logical_NOT>*/{    0,    0,   31,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   30,    0,    0,    0,    0,    0,    0,   31,   31,    0,    0,   31,   31,   31,   31,    0 },
            new int[35]/*     <Logical_NOT.>*/{   29,   29,   29,    0,    0,   29,    0,   29,   29,   29,   29,   29,   29,   29,    0,    0,    0,   29,   28,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   29 },
            new int[35]/*         <Relation>*/{    0,    0,   39,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   39,   39,    0,    0,   39,   39,   39,   39,    0 },
            new int[35]/*        <Relation.>*/{   38,   38,   38,    0,    0,   38,    0,   38,   38,   38,   38,   38,   38,   38,    0,    0,    0,   38,   38,    0,   32,   33,   34,   35,   36,   37,    0,    0,    0,    0,    0,    0,    0,    0,   38 },
            new int[35]/*             <Term>*/{    0,    0,   43,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   43,   43,    0,    0,   43,   43,   43,   43,    0 },
            new int[35]/*            <Term.>*/{   42,   42,   42,    0,    0,   42,    0,   42,   42,   42,   42,   42,   42,   42,    0,    0,    0,   42,   42,    0,   42,   42,   42,   42,   42,   42,   40,   41,    0,    0,    0,    0,    0,    0,   42 },
            new int[35]/*            <Unary>*/{    0,    0,   47,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   49,   48,    0,    0,   47,   47,   47,   47,    0 },
            new int[35]/*           <Unary.>*/{   46,   46,   46,    0,    0,   46,    0,   46,   46,   46,   46,   46,   46,   46,    0,    0,    0,   46,   46,    0,   46,   46,   46,   46,   46,   46,   46,   46,   44,   45,    0,    0,    0,    0,   46 },
            new int[35]/*        <Primitive>*/{    0,    0,   50,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   51,   52,   53,   54,    0 }
        };
        // признак присутствия
        public const int ST_EXISTS = -2;
        // признак отсутствия
        public const int ST_NOTFOUND = -3;
    }
}
