using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLISP
{
    class lispDictionary
    {
        public Dictionary<string, Func<string, definedFunct, string>> dict = new Dictionary<string, Func<string, definedFunct, string>>();
        public lispDictionary()
        {
            Initial_Dict();
        }
        public void Initial_Dict()
        {
            dict.Add("+", Program.lisp.add);
            dict.Add("-", Program.lisp.sub);
            dict.Add("*", Program.lisp.multiply);
            dict.Add("/", Program.lisp.divide);
            dict.Add("=", Program.lisp.equal);
            dict.Add("<", Program.lisp.lessThan);
            dict.Add(">", Program.lisp.greaterThan);
            dict.Add("define", Program.lisp.define);
            dict.Add("set", Program.lisp.set);
            dict.Add("cons", Program.lisp.cons);
            dict.Add("car", Program.lisp.car);
            dict.Add("cdr", Program.lisp.cdr);
            dict.Add("number?", Program.lisp.isNumber);
            dict.Add("symbol?", Program.lisp.isSymbol);
            dict.Add("null?", Program.lisp.isNil);
            dict.Add("NIL?", Program.lisp.isNil);
            dict.Add("list?", Program.lisp.isList);
            dict.Add("EQ?", Program.lisp.equal);
            dict.Add("print", Program.lisp.print);
            //dict.Add("if", Program.lisp.if_func);
            //dict.Add("COND", Program.lisp.cond);
        }
    }
}
