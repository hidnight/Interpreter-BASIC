using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BASIC_Interpreter_Library.Constants;

namespace BASIC_Interpreter_Library {
    class Stack<T> {
        // массив элементов
        T[] st;
        // счетчик
        int count;
        // глубина
        int depth;
        // нулевое значение класса
        T nill;

        int N = MAX_STACK;
        // очищает объект
        void reset() {
            depth = count = 0;
            for (int i = 0; i < N; i++)
                st[i] = nill;
        }
        // количество элементов
        public int Size {
            get {
                return count;
            }
        }
        // проталкивает элемент
        void push(T value) {
            if (count >= N)
                throw new Exception("ststack::push stack overflow");
            st[count++] = value;
            if (depth < count)
                depth = count;
        }
        // выталкивает элемент
        T pop() {
            if (count < 1)
                throw new Exception("ststack::pop stack is empty");
            T value = st[--count];
            st[count] = nill;
            return value;
        }
        // выталкивает number элементов
        T pop(int number) {
            if (count < number)
                throw new Exception("ststack::pop(int) stack isn't so big as number");
            count -= number;
            return st[count];
        }
        // элемент на вершине
        public T Top {
            get {
                if (count < 1)
                    throw new Exception("ststack::top stack is empty");
                return st[count - 1];
            }
        }
    }
}
