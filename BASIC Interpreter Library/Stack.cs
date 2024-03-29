﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BASIC_Interpreter_Library {
    public class Stack<T> {
        // массив элементов
        private List<T> st;
        // глубина
        private int depth;
        // конструктор
        public Stack() {
            st = new List<T>();
            Reset();
        }
        // очищает объект
        private void Reset() {
            depth = 0;
            st.Clear();
        }
        // количество элементов
        public int Size => st.Count;
        // проталкивает элемент
        public void Push(T value) {
            st.Add(value);
            if (depth < Size)
                depth = Size;
        }
        // выталкивает элемент
        public T Pop() {
            if (Size < 1)
                throw new Exception("ststack::pop Стек пуст.");
            T value = st.Last();
            st.RemoveAt(Size - 1);
            return value;
        }
        // выталкивает number элементов
        public T pop(int number) {
            if (Size < number)
                throw new Exception("ststack::pop(int) В стеке нет такого количества элементов.");
            T result = st.Last();
            for (int i = number; i > 0; i--) {
                result = st.Last();
                st.Remove(st.Last());
            }
            return result;
        }
        // элемент на вершине
        public T Top {
            get {
                if (Size < 1)
                    throw new Exception("ststack::top Стек пуст.");
                return st.Last();
            }
        }
    }
}
