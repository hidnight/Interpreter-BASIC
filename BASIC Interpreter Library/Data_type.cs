using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace BASIC_Interpreter_Library {
    public enum Data_type {
        STDT_NULL = VarEnum.VT_NULL,
        STDT_I4 = VarEnum.VT_I4,
        STDT_R8 = VarEnum.VT_R8,
        STDT_QUOTE = VarEnum.VT_LPWSTR,
        STDT_BOOL = VarEnum.VT_BOOL
    }
}
