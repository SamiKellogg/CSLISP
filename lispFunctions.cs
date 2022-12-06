using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLISP
{
    class definedFunct
    {
        public List<string> statements = new List<string>();
        public Dictionary<string, string> vars = new Dictionary<string, string>();
        public List<string> varLabels = new List<string>();
        public string getVar(string input, definedFunct func = null)
        {
            string[] filter = { "get" };
            input = Program.util.recompileString(Program.util.getSubArray(input, filter: filter));
            if (func == null)
                return vars[input];
            else
                return func.vars[input];
        }
    }
    class lispFunctions
    {
        public Dictionary<string, string> variables = new Dictionary<string, string>();
        public Dictionary<string, definedFunct> functions = new Dictionary<string, definedFunct>();

        public string define(string name, definedFunct func = null)
        {
            string k = Program.util.extractKey(ref name);
            int[] index = Program.util.readNextPart(name, 0);
            definedFunct newFunc = new definedFunct();
            string[] filter = { "", " ", "(", ")" };
            //Console.WriteLine(name);
            foreach (string s in Program.util.getSubArray(name, index, filter))
            {
                //Console.WriteLine(s);
                newFunc.vars.Add(s, "");
                newFunc.varLabels.Add(s);
            }

            while (true)
            {
                index = Program.util.readNextPart(name, index[1] + 1);
                //Console.WriteLine("index");
                //Console.WriteLine(index[0]);
                if (index[1] - index[0] < 0 || index[0] == index[1]) break;
                newFunc.statements.Add(Program.util.recompileString(Program.util.getSubArray(name, index)));
                //Console.WriteLine("t");


            }
            //Console.WriteLine(k);

            functions.Add(k, newFunc);
            //Console.WriteLine("4");

            Program.dictionary.dict.Add(k, Program.lisp.newfunct);
            //Console.WriteLine("t");
            //Console.WriteLine(Program.dictionary.dict.Keys);
            return "";
        }
        public string newfunct(string input, definedFunct funct = null)
        {
            //Console.WriteLine(input);
            //input = input.Trim();
            string k = Program.util.extractKey1(ref input);
            //Console.WriteLine("k=");
            //Console.WriteLine(input);
            Program.util.subVarr(ref input);
            Program.util.evaluateFunct(ref input, funct);
            input = input.Contains('(') ? input[(input.IndexOf('(') + 1)..(input.LastIndexOf(')'))].Trim() : input;
            definedFunct temp = new definedFunct();
            functions.TryGetValue(k, out temp);
            string[] filter = { ")", "(", " ", "" };
            int i = 0;
            foreach (string s in Program.util.getSubArray(input, filter: filter))
            {
                //Console.WriteLine("s=");
                //SConsole.WriteLine(s);
                temp.vars[temp.varLabels[i]] = s;
                i++;
            }
            string final = "";
            foreach (string s in temp.statements)
            {
                final = s;
                Program.util.Atom(new int[] { 0, s.Length - 1 }, final, temp);
            }
            return final;
        }
        public string add(string input, definedFunct funct = null)
        {
            //Console.WriteLine(input);
            string[] filter = { "+" };
            string[] tempInput = Program.util.getSubArray(input, filter: filter);
            input = Program.util.recompileString(tempInput);
            //Console.WriteLine(input);
            double total = 0;
            Program.util.evaluateFunct(ref input, funct);
            //Console.WriteLine("l");
            Program.util.subVarr(ref input, funct);
            //Console.WriteLine(input);

            string[] args = Program.util.getSubArray(input);
            foreach (string x in args)
            {
                total += Convert.ToDouble(x);
            }
            return total.ToString();
        }
        public string print(string input, definedFunct func = null)
        {
            string[] filter = { "print" };
            input = Program.util.recompileString(Program.util.getSubArray(input, filter: filter));
            //Console.WriteLine(input);
            Program.util.evaluateFunct(ref input, func);
            Program.util.subVarr(ref input, func);

            if (input == "" || input == "()")
                Console.WriteLine("()");
            else if (input == "\\n")
                Console.WriteLine("");
            else
                Console.WriteLine(input.Trim());
            return "";
        }
        public string cond(string input, definedFunct funct)
        {
            //Console.WriteLine("d");
            string[] tempInput = Program.util.getSubArray(input);
            input = Program.util.recompileString(tempInput);
            int[] index = Program.util.readFirstpart(input);
            Program.util.AtomRef(index, ref input, funct);
            
            if (Program.util.getSubArray(input)[1] == "T")
            {
                index = Program.util.readFirstpart(input);
                //Console.WriteLine("not");
                input = input.Substring(index[0], index[1] - index[0] + 1).Trim();
                index = new int[] { 0, input.Length - 1 };
                Program.util.AtomRef(index, ref input, funct);
                return "";
            }
            else
            {
                
                while (true)
                {
                    index = Program.util.readFirstpart(input);
                    input = input.Replace(input.Substring(0, index[1] + 1), "").Trim();
                    //Console.WriteLine(input);
                    index = Program.util.readFirstpart(input);
                    Program.util.AtomRef(index, ref input, funct);
                    //Console.WriteLine(input);
                    if (Program.util.getSubArray(input)[0] == "T")
                    {
                        index = Program.util.readFirstpart(input);
                        //Console.WriteLine("not");
                        input = input.Substring(index[0], index[1] - index[0] + 1).Trim();
                        index = new int[] { 0, input.Length - 1 };
                        Program.util.AtomRef(index, ref input, funct);
                        return "";
                    }
                }
            }
            return "";
        }
        public string isIf(string input, definedFunct funct = null)
        {
            string[] tempInput = Program.util.getSubArray(input);
            input = Program.util.recompileString(tempInput);
            int[] index = Program.util.readFirstpart(input);
            Program.util.AtomRef(index, ref input, funct);
            if (Program.util.getSubArray(input)[1] == "T")
            {
                index = Program.util.readFirstpart(input);
                //Console.WriteLine(input);
                input = input.Substring(index[0], index[1] - index[0] + 1).Trim();
                index = new int[] { 0, input.Length - 1 };
                Program.util.AtomRef(index, ref input, funct);
                return "";
            }
            else
            {
                index = Program.util.readFirstpart(input);
                //Console.WriteLine("i=");
               // Console.WriteLine(input);
                input = input.Replace(input.Substring(0, index[1] + 1), "").Trim();
                //Console.WriteLine("i=");
               // Console.WriteLine(input);
                Program.util.subVarr(ref input, funct);
                index = Program.util.readFirstpart(input);
                //Console.WriteLine(index[0]);
               // Console.WriteLine(index[1]);
                //Console.WriteLine(input);
                input = input.Substring(index[0], index[1] - index[0] + 1).Trim();
                input = Program.util.recompileString(Program.util.getSubArray(input));
                Program.util.AtomRef(new int[] { 0, input.Length - 1 }, ref input, funct);
                return "";
            }

        }


        public string sub(string input, definedFunct funct)
        {
            string[] filter = { "-" };
            string[] tempInput = Program.util.getSubArray(input, filter: filter);
            input = Program.util.recompileString(tempInput);
            Program.util.evaluateFunct(ref input, funct);
            Program.util.subVarr(ref input, funct);
            string[] args = Program.util.getSubArray(input);
            double total = Convert.ToDouble(args[0]);
            total -= Convert.ToDouble(args[1]);
            return total.ToString();
        }
        public string multiply(string input, definedFunct funct)
        {
            string[] filter = { "*" };
            string[] tempInput = Program.util.getSubArray(input, filter: filter);
            input = Program.util.recompileString(tempInput);
            Program.util.evaluateFunct(ref input, funct);
            Program.util.subVarr(ref input, funct);
            string[] args = Program.util.getSubArray(input);
            double total = Convert.ToDouble(args[0]);
            foreach (string s in args[1..])
            {
                total *= Convert.ToDouble(s);
            }

            return total.ToString();
        }
        public string divide(string input, definedFunct funct)
        {
            string[] filter = { "/" };
            string[] tempInput = Program.util.getSubArray(input, filter: filter);
            input = Program.util.recompileString(tempInput);
            Program.util.evaluateFunct(ref input, funct);
            Program.util.subVarr(ref input, funct);
            string[] args = Program.util.getSubArray(input);
            double total = Convert.ToDouble(args[0]);
            total /= Convert.ToDouble(args[1]);
            return total.ToString();
        }
        public string lessThan(string input, definedFunct funct)
        {
            string[] filter = { "<" };
            string[] tempInput = Program.util.getSubArray(input, filter: filter);
            input = Program.util.recompileString(tempInput);
            Program.util.evaluateFunct(ref input, funct);
            Program.util.subVarr(ref input, funct);
            string[] args = Program.util.getSubArray(input);
            //Console.WriteLine("<162");
            if (Convert.ToDouble(args[0]) < Convert.ToDouble(args[1]))
            {
                //Console.WriteLine("T");
                return "T";
            }
            else
            {
                //Console.WriteLine("()");
                return "()";
            }
        }
        public string greaterThan(string input, definedFunct funct)
        {
            string[] filter = { ">" };
            string[] tempInput = Program.util.getSubArray(input, filter: filter);
            input = Program.util.recompileString(tempInput);
            Program.util.evaluateFunct(ref input, funct);
            Program.util.subVarr(ref input, funct);
            string[] args = Program.util.getSubArray(input);
            if (Convert.ToDouble(args[0]) > Convert.ToDouble(args[1]))
            {
                return "T";
            }
            else
            {
                return "()";
            }
        }
        public string equal(string input, definedFunct funct)
        {
            string[] filter = { "=", "EQ?" };
            string[] tempInput = Program.util.getSubArray(input, filter: filter);
            input = Program.util.recompileString(tempInput);
            Program.util.evaluateFunct(ref input, funct);
            Program.util.subVarr(ref input, funct);
            string[] args = Program.util.getSubArray(input);
            if (Convert.ToDouble(args[0]) == Convert.ToDouble(args[1]))
            {
                return "T";
            }
            else
            {
                return "()";
            }
        }
        public string isNumber(string input, definedFunct funct)
        {
            string[] filter = { "number?" };
            string[] temp = Program.util.getSubArray(input, filter: filter);
            try
            {
                Convert.ToDouble(temp[0]);
                return "T";
            }
            catch
            {
                return "()";
            }
        }
        public string isSymbol(string input, definedFunct funct)
        {
            string[] filter = { "symbol?" };
            string[] temp = Program.util.getSubArray(input, filter: filter);
            if (Program.lisp.variables.ContainsKey(temp[0]))
            {
                return "T";
            }
            else
            {
                return "()";
            }
        }
        public string isList(string input, definedFunct funct)
        {
            string[] filter = { "list?" };
            string[] temp = Program.util.getSubArray(input, filter: filter);
            if (temp.Length < 2) return "()";
            foreach (string s in temp)
            {
                if (Program.dictionary.dict.ContainsKey(s)) 
                return "()";
            }
            return "T";
        }
        public string isNil(string input, definedFunct funct = null)
        {
            return (input.Contains("()") || input.Contains("( )") ? "T" : "()");
        }
        public string getVar(string input, definedFunct func = null)
        {
            string[] filter = { "get" };
            input = Program.util.recompileString(Program.util.getSubArray(input, filter: filter));
            if (func == null)
                return variables[input];
            else
                return func.vars[input];
        }
        public string set(string input, definedFunct funct = null)
        {
            string[] filter = { "set" };
            string[] tempInput = Program.util.getSubArray(input, filter: filter);
            input = Program.util.recompileString(tempInput);
            string[] array = new string[2];
            Program.util.evaluateFunct(ref input, funct);
            array = Program.util.getSubArray(input);
            //Console.WriteLine(array[0]);
            if (variables.ContainsKey(array[1]))
            {
                //Console.WriteLine("2");

                array[1] = getVar(array[1]);
            }
            if (variables.ContainsKey(array[0]))
            {
                //Console.WriteLine("3");

                variables.Remove(array[0]);
            }
            variables.Add(array[0], array[1]);
            return "";
        }
        public string car(string input, definedFunct funct)
        {
            string[] filter = { "car" };
            input = input.Replace("car", "").Trim();
            //Console.WriteLine(input);
            //Console.WriteLine(input[1]);
            //Console.WriteLine("p");
            if (input[2] != '(')
            {
                // Console.WriteLine(input.Split(" ")[1]);
                return input.Split(" ")[1];
            }
            else
            {
                string phrase = input[1..(input.Length - 1)].Trim();
                int[] index = Program.util.readFirstpart(phrase);
                return phrase[index[0]..(index[1] + 1)];
            }
        }
        public string cdr(string input, definedFunct funct)
        {
            string[] temp = Program.util.getSubArray(input);
            string replace = String.Join(" ", temp);
            replace = replace.Replace("cdr", "car");
            string getCar = car(replace, null);
            replace = replace.Replace(getCar, " ");
            return replace[replace.IndexOf('(')..];
        }
        public string cons(string input, definedFunct funct)
        {
            int x = input.Where(x => Program.dictionary.dict.Keys.ToArray().ToString().Contains(x)).Count();
            for (; x > 0; x--)
            {
                Program.util.evaluateFunct(ref input, funct);
            }
            string[] filter = { "cons" };
            string[] temp = Program.util.getSubArray(input, filter: filter);
            string restore = "( " + String.Join(" ", temp) + " )";
            return restore;
        }
    }
}
