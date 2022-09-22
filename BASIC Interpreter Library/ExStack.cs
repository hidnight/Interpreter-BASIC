using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Token temp = new Token() {
                Stt = e.Stt,
                Int_val = e.Int_val,
                Dbl_val = e.Dbl_val,
                Str_val = e.Str_val,
                Bool_val = e.Bool_val,
                Data_Type = e.Data_Type,
                Line_Number = e.Line_Number,
                Name = e.Name
            };
            st.Add(temp);
            if (depth < Size) {
                depth = Size;
            }
        }
        // выталкивает элемент
        public void pop(ref Token e) {
            if (Size < 1)
                throw new Exception("exstack::pop exe-stack is empty");
            e.Stt = st.Last().Stt;
            e.Int_val = st.Last().Int_val;
            e.Dbl_val = st.Last().Dbl_val;
            e.Bool_val = st.Last().Bool_val;
            e.Data_Type = st.Last().Data_Type;
            e.Str_val = st.Last().Str_val;
            e.Name = st.Last().Name;
            st.RemoveAt(Size - 1);
        }
    }
}
