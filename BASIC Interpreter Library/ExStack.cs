﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BASIC_Interpreter_Library {
    public class ExStack {
        // массив элементов
        private List<Token> st = new List<Token>();
        // счетчик
        //private int count;
        // глубина
        private int depth;
        public ExStack() {
            depth = 0;
        }
        public int Size => st.Count;
        // проталкивает элемент
        public void push(Token e) {
            st.Add((Token)e.Clone());
            if (depth < Size) {
                depth = Size;
            }
        }
        // выталкивает элемент
        public void pop(ref Token e) {
            if (Size < 1)
                throw new Exception("exstack::pop Стек пуст");
            e = (Token)st.Last().Clone();
            st.RemoveAt(Size - 1);
        }
    }
}
