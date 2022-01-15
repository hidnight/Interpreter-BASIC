using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASIC_Interpreter_Library {
    static class Constants {
        public const int MAX_ID = 32;
        public const int MAX_QUOTE = 256;
        public const int MAX_STACK = 64;
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
    }
}
