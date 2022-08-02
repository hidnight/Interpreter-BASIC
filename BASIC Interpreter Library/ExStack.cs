using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASIC_Interpreter_Library {
    public class ExStack {
        // массив элементов
        private List<Token> st;
        // счетчик
        //private int count;
        // глубина
        private int depth;
        public ExStack() {
            depth = 0;
        }
        public int Size => st.Count;
        // проталкивает элемент
        public void push(ref Token e) {
            st.Add(e);
            if (depth < Size) {
                depth = Size;
            }
        }
        // выталкивает элемент
        public void pop(ref Token e) {
            if (Size < 1)
                throw new Exception("exstack::push exe-stack is empty");
            e = st.Last();
            st.RemoveAt(Size - 1);
        }
    }
}
