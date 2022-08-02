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
        public static readonly Interpreter_symbol[][] RULE = new Interpreter_symbol[59][] {
            /* 0*/new Interpreter_symbol[]{ TOK_EOT },
            /* 1 SYM_MODULE            */new Interpreter_symbol[]{ SYM_STATEMENT, SYM_MODULE, TOK_EOT },
            /* 2 SYM_MODULE            */new Interpreter_symbol[]{ TOK_EOT },
            /* 3 SYM_STATEMENT         */new Interpreter_symbol[]{ SYM_ASSIGNMENT, TOK_EOT },
            /* 4 SYM_STATEMENT         */new Interpreter_symbol[]{ SYM_BOOLEAN, TOK_EOT },
            /* 5 SYM_STATEMENT         */new Interpreter_symbol[]{ SYM_PRINT, TOK_EOT },
            /* 6 SYM_STATEMENT         */new Interpreter_symbol[]{ SYM_WHILE, TOK_EOT },
            /* 7 SYM_STATEMENT         */new Interpreter_symbol[]{ SYM_EMPTY, TOK_EOT },
            /* 8 SYM_STATEMENT         */new Interpreter_symbol[]{ SYM_DECLARATIONS, TOK_EOT },
            /* 9 SYM_DECLARATIONS      */new Interpreter_symbol[]{ SYM_DECLARATION, SYM_DECLARATIONS, TOK_EOT },
            /*10 SYM_DECLARATION       */new Interpreter_symbol[]{ TOK_DIM, SYM_DECLARATION_LIST, TOK_EOT },
            /*11 SYM_DECLARATION_LIST  */new Interpreter_symbol[]{ TOK_ID, OUT_ID, TOK_AS, SYM_TYPE, OUT_DIM, SYM_DECLARATION_LIST_, TOK_EOT },
            /*12 SYM_DECLARATION_LIST_ */new Interpreter_symbol[]{ TOK_COMMA, TOK_ID, OUT_ID, TOK_AS, SYM_TYPE, OUT_DIM, SYM_DECLARATION_LIST_, TOK_EOT },
            /*13 SYM_DECLARATION_LIST_ */new Interpreter_symbol[]{ TOK_LF, TOK_EOT },
            /*14 SYM_ASSIGNMENT        */new Interpreter_symbol[]{ TOK_ID, SYM_ASSIGNMENT_, TOK_EOT },
            /*15 SYM_ASSIGNMENT_       */new Interpreter_symbol[]{ OUT_ID, TOK_ASS, SYM_EXPRESSION, TOK_LF, TOK_EOT },
            /*16 SYM_ASSIGNMENT_       */new Interpreter_symbol[]{ TOK_LB, SYM_EXPRESSION, TOK_RB, OUT_ID_Q, TOK_ASS, SYM_EXPRESSION, TOK_LF, TOK_EOT },
            /*17 SYM_BOOLEAN           */new Interpreter_symbol[]{ TOK_IF, SYM_EXPRESSION, TOK_THEN, TOK_LF, OUT_PUSH, OUT_BZ, SYM_MODULE, SYM_BOOLEAN_END, TOK_EOT },
            /*18 SYM_BOOLEAN_END       */new Interpreter_symbol[]{ TOK_ELSE, TOK_LF, OUT_PUSH, OUT_BR, OUT_SWAP, OUT_DEFL, SYM_MODULE, OUT_POPL, OUT_DEFL, TOK_END_IF, TOK_LF, TOK_EOT },
            /*19 SYM_BOOLEAN_END       */new Interpreter_symbol[]{ OUT_POPL, OUT_DEFL, TOK_END_IF, TOK_LF, TOK_EOT },
            /*20 SYM_PRINT             */new Interpreter_symbol[]{ TOK_PRINT, SYM_EXPRESSION, OUT_PRINT, TOK_LF, TOK_EOT },
            /*21 SYM_WHILE             */new Interpreter_symbol[]{ TOK_WHILE, SYM_EXPRESSION, OUT_PUSH, OUT_BZ, TOK_LF, SYM_MODULE, OUT_POPL, OUT_DEFL, TOK_END_WHILE, TOK_LF, TOK_EOT },
            /*22 SYM_EMPTY             */new Interpreter_symbol[]{ TOK_LF, TOK_EOT },
            /*23 SYM_TYPE              */new Interpreter_symbol[]{ TOK_STRING, OUT_STRING, TOK_EOT },
            /*24 SYM_TYPE              */new Interpreter_symbol[]{ TOK_LONG, OUT_LONG, TOK_EOT },
            /*25 SYM_TYPE              */new Interpreter_symbol[]{ TOK_REAL, OUT_REAL, TOK_EOT },
            /*26 SYM_EXPRESSION        */new Interpreter_symbol[]{ SYM_LOGICAL_AND, SYM_LOGICAL_AND_, TOK_EOT },
            /*27 SYM_LOGICAL_AND_      */new Interpreter_symbol[]{ TOK_OR, SYM_LOGICAL_AND, SYM_LOGICAL_AND_, OUT_OR, TOK_EOT },
            /*28 SYM_LOGICAL_AND_      */new Interpreter_symbol[]{ TOK_EOT },
            /*29 SYM_LOGICAL_AND       */new Interpreter_symbol[]{ SYM_LOGICAL_NOT, SYM_LOGICAL_NOT_, TOK_EOT },
            /*30 SYM_LOGICAL_NOT_      */new Interpreter_symbol[]{ TOK_AND, SYM_LOGICAL_NOT, SYM_LOGICAL_NOT_, OUT_AND, TOK_EOT },
            /*31 SYM_LOGICAL_NOT_      */new Interpreter_symbol[]{ TOK_EOT },
            /*32 SYM_LOGICAL_NOT       */new Interpreter_symbol[]{ TOK_NOT, SYM_LOGICAL_NOT, OUT_NOT, TOK_EOT },
            /*33 SYM_LOGICAL_NOT       */new Interpreter_symbol[]{ SYM_RELATION, SYM_RELATION_, TOK_EOT },
            /*34 SYM_RELATION_         */new Interpreter_symbol[]{ TOK_EQ, SYM_RELATION, SYM_RELATION_, OUT_EQ, TOK_EOT },
            /*35 SYM_RELATION_         */new Interpreter_symbol[]{ TOK_NE, SYM_RELATION, SYM_RELATION_, OUT_NE, TOK_EOT },
            /*36 SYM_RELATION_         */new Interpreter_symbol[]{ TOK_LT, SYM_RELATION, SYM_RELATION_, OUT_LT, TOK_EOT },
            /*37 SYM_RELATION_         */new Interpreter_symbol[]{ TOK_GT, SYM_RELATION, SYM_RELATION_, OUT_GT, TOK_EOT },
            /*38 SYM_RELATION_         */new Interpreter_symbol[]{ TOK_LE, SYM_RELATION, SYM_RELATION_, OUT_LE, TOK_EOT },
            /*39 SYM_RELATION_         */new Interpreter_symbol[]{ TOK_GE, SYM_RELATION, SYM_RELATION_, OUT_GE, TOK_EOT },
            /*40 SYM_RELATION_         */new Interpreter_symbol[]{ TOK_EOT },
            /*41 SYM_RELATION          */new Interpreter_symbol[]{ SYM_TERM, SYM_TERM_, TOK_EOT },
            /*42 SYM_TERM_             */new Interpreter_symbol[]{ TOK_ADD, SYM_TERM, SYM_TERM_, OUT_ADD, TOK_EOT },
            /*43 SYM_TERM_             */new Interpreter_symbol[]{ TOK_SUB, SYM_TERM, SYM_TERM_, OUT_SUB, TOK_EOT },
            /*44 SYM_TERM_             */new Interpreter_symbol[]{ TOK_EOT },
            /*45 SYM_TERM              */new Interpreter_symbol[]{ SYM_UNARY, SYM_UNARY_, TOK_EOT },
            /*46 SYM_UNARY_            */new Interpreter_symbol[]{ TOK_MUL, SYM_UNARY, SYM_UNARY_, OUT_MUL, TOK_EOT },
            /*47 SYM_UNARY_            */new Interpreter_symbol[]{ TOK_DIV, SYM_UNARY, SYM_UNARY_, OUT_DIV, TOK_EOT },
            /*48 SYM_UNARY_            */new Interpreter_symbol[]{ TOK_EOT },
            /*49 SYM_UNARY             */new Interpreter_symbol[]{ SYM_PRIMITIVE, TOK_EOT },
            /*50 SYM_UNARY             */new Interpreter_symbol[]{ TOK_SUB, SYM_PRIMITIVE, OUT_U_SUB, TOK_EOT },
            /*51 SYM_UNARY             */new Interpreter_symbol[]{ TOK_ADD, SYM_PRIMITIVE, OUT_U_ADD, TOK_EOT },
            /*52 SYM_PRIMITIVE         */new Interpreter_symbol[]{ TOK_ID, SYM_PRIMITIVE_, TOK_EOT },
            /*53 SYM_PRIMITIVE         */new Interpreter_symbol[]{ TOK_I4, OUT_I4, TOK_EOT },
            /*54 SYM_PRIMITIVE         */new Interpreter_symbol[]{ TOK_R8, OUT_R8, TOK_EOT },
            /*55 SYM_PRIMITIVE         */new Interpreter_symbol[]{ TOK_QUOTE, OUT_QUOTE, TOK_EOT },
            /*56 SYM_PRIMITIVE         */new Interpreter_symbol[]{ TOK_LP, SYM_EXPRESSION, TOK_RP, TOK_EOT },
            /*57 SYM_PRIMITIVE_        */new Interpreter_symbol[]{ TOK_LB, SYM_EXPRESSION, TOK_RB, OUT_ID_Q, TOK_EOT },
            /*58 SYM_PRIMITIVE_        */new Interpreter_symbol[]{ OUT_ID, TOK_EOT }
        };
        /* Max rule index */
        public const int MAX_RULE = 58;
        /* Rule Length */
        public static int[] RLEN = new int[] { 0, 2, 0, 1, 1, 1, 1, 1, 1, 2, 2, 4, 5, 1, 2, 3, 6, 6, 5, 2, 3, 6, 1, 1, 1, 1, 2, 3, 0, 2, 3, 0, 2, 2, 3, 3, 3, 3, 3, 3, 0, 2, 3, 3, 0, 2, 3, 3, 0, 1, 2, 2, 2, 1, 1, 1, 3, 3, 0 };
        /* Evaluates actual rule's length */
        public static void get_rule_len() {
            int rule, count;
            for (rule = 1; rule <= RULE.Length; rule++) {
                for (count = 0;
                    count < RULE[rule].Length && RULE[rule][count] != TOK_EOT;
                    count++)
                    ;
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
        public static readonly int[][] SYNTA = new int[64][] {
        /*                                 EOT [dim]  [id]  [as]   [,]  [LF]   [=]   [[]   []]  [if] [then [else [end_ [prin [whil [end_ [STRI [LONG [REAL  [or] [and] [not]  [==]  [<>]   [<]   [>]  [<=]  [>=]   [+]   [-]   [*]   [/]  [I4]  [R8] [QUOT   [(]   [)] */
        /*                   */new int[]{  ACC,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*              [dim]*/new int[]{    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*               [id]*/new int[]{    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*               [as]*/new int[]{    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*                [,]*/new int[]{    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*               [LF]*/new int[]{    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*                [=]*/new int[]{    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*                [[]*/new int[]{    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*                []]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*               [if]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*             [then]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*             [else]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*           [end_if]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*            [print]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*            [while]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*        [end_while]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*           [STRING]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*             [LONG]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*             [REAL]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*               [or]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*              [and]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*              [not]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*               [==]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*               [<>]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*                [<]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*                [>]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*               [<=]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*               [>=]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*                [+]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*                [-]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0,    0 },
        /*                [*]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0,    0 },
        /*                [/]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0,    0 },
        /*               [I4]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0,    0 },
        /*               [R8]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0,    0 },
        /*            [QUOTE]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0,    0 },
        /*                [(]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP,    0 },
        /*                [)]*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,  POP },
        /*                                 EOT [dim]  [id]  [as]   [,]  [LF]   [=]   [[]   []]  [if] [then [else [end_ [prin [whil [end_ [STRI [LONG [REAL  [or] [and] [not]  [==]  [<>]   [<]   [>]  [<=]  [>=]   [+]   [-]   [*]   [/]  [I4]  [R8] [QUOT   [(]   [)] */
        /*           <Module>*/new int[]{    2,    1,    1,    0,    0,    1,    0,    0,    0,    1,    0,    2,    2,    1,    1,    2,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*        <Statement>*/new int[]{    0,    8,    3,    0,    0,    7,    0,    0,    0,    4,    0,    0,    0,    5,    6,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*       <Assignment>*/new int[]{    0,    0,   14,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*          <Boolean>*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,   17,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*            <Print>*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   20,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*            <While>*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   21,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*            <Empty>*/new int[]{    0,    0,    0,    0,    0,   22,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*     <Declarations>*/new int[]{    0,    9,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*      <Declaration>*/new int[]{    0,   10,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /* <Declaration_list>*/new int[]{    0,    0,   11,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*             <Type>*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   23,   24,   25,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*<Declaration_list.>*/new int[]{    0,    0,    0,    0,   12,   13,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*      <Assignment.>*/new int[]{    0,    0,    0,    0,    0,    0,   15,   16,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*       <Expression>*/new int[]{    0,    0,   26,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   26,    0,    0,    0,    0,    0,    0,   26,   26,    0,    0,   26,   26,   26,   26,    0 },
        /*      <Boolean_end>*/new int[]{    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   18,   19,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
        /*      <Logical_AND>*/new int[]{    0,    0,   29,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   29,    0,    0,    0,    0,    0,    0,   29,   29,    0,    0,   29,   29,   29,   29,    0 },
        /*     <Logical_AND.>*/new int[]{    0,    0,    0,    0,    0,   28,    0,    0,   28,    0,   28,    0,    0,    0,    0,    0,    0,    0,    0,   27,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   28 },
        /*      <Logical_NOT>*/new int[]{    0,    0,   33,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   32,    0,    0,    0,    0,    0,    0,   33,   33,    0,    0,   33,   33,   33,   33,    0 },
        /*     <Logical_NOT.>*/new int[]{    0,    0,    0,    0,    0,   31,    0,    0,   31,    0,   31,    0,    0,    0,    0,    0,    0,    0,    0,   31,   30,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   31 },
        /*         <Relation>*/new int[]{    0,    0,   41,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   41,   41,    0,    0,   41,   41,   41,   41,    0 },
        /*        <Relation.>*/new int[]{    0,    0,    0,    0,    0,   40,    0,    0,   40,    0,   40,    0,    0,    0,    0,    0,    0,    0,    0,   40,   40,    0,   34,   35,   36,   37,   38,   39,    0,    0,    0,    0,    0,    0,    0,    0,   40 },
        /*             <Term>*/new int[]{    0,    0,   45,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   45,   45,    0,    0,   45,   45,   45,   45,    0 },
        /*            <Term.>*/new int[]{    0,    0,    0,    0,    0,   44,    0,    0,   44,    0,   44,    0,    0,    0,    0,    0,    0,    0,    0,   44,   44,    0,   44,   44,   44,   44,   44,   44,   42,   43,    0,    0,    0,    0,    0,    0,   44 },
        /*            <Unary>*/new int[]{    0,    0,   49,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   51,   50,    0,    0,   49,   49,   49,   49,    0 },
        /*           <Unary.>*/new int[]{    0,    0,    0,    0,    0,   48,    0,    0,   48,    0,   48,    0,    0,    0,    0,    0,    0,    0,    0,   48,   48,    0,   48,   48,   48,   48,   48,   48,   48,   48,   46,   47,    0,    0,    0,    0,   48 },
        /*        <Primitive>*/new int[]{    0,    0,   52,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,   53,   54,   55,   56,    0 },
        /*       <Primitive.>*/new int[]{    0,    0,    0,    0,    0,   58,    0,   57,   58,    0,   58,    0,    0,    0,    0,    0,    0,    0,    0,   58,   58,    0,   58,   58,   58,   58,   58,   58,   58,   58,   58,   58,    0,    0,    0,    0,   58 }
        };
        // признак присутствия
        public const int ST_EXISTS = -2;
        // признак отсутствия
        public const int ST_NOTFOUND = -3;
    }
}
