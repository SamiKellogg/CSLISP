using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Linq.Expressions;

namespace CSLISP
{   
    class utility
    {
        
        lispDictionary dict;
        lispFunctions funct;
        private string[] inputString;
        private int countLine = 0;

        public utility(string code, lispDictionary dictionary, lispFunctions funct)
        {
            Processed(code);
            dict = dictionary;
            this.funct= funct;
        }
        void Processed(string code) {//makes code into array of expressions
            Prep(ref code);
            List<string> ls = code.Split(" ").Where(x => !String.IsNullOrWhiteSpace(x)).ToList(); ;
            List<string> final = new List<string>();
            int count = 0;
            string expr = "";
            foreach (string s in ls)
            {
                if (s == "(")
                    count++;
                else if (s == ")")
                    count--;
                expr += s + " ";
                if (count == 0 && expr != "")
                {
                    final.Add(expr);
                    expr = "";
                }
            }
            inputString = final.ToArray();
        }
        public void Atom(int[] x, string line ,definedFunct func)
        {
            if (x[0] == -1 && x[1] == -1)
            {
                return;
            }
            string atoms = line[x[0]..(x[1] + 1)].Trim();
            string argAtoms = atoms[(atoms.IndexOf('(') + 1)..atoms.LastIndexOf(')')].Trim();
            string ans = "";
            try
            {
                var pro = argAtoms.Trim().Split(" ")[0].Trim();
                //Console.WriteLine(argAtoms);
                //Console.WriteLine("p");
                ans = dict.dict[pro](argAtoms, func != null ? func : null);
                Console.WriteLine(ans);
            }
            catch (Exception)
            {
                if (argAtoms == " ") argAtoms = "()";
                line = line[0..(x[0])] + " " + argAtoms + " " + line[(x[1] + 1)..];
                line = line.Trim();
                return;
            }
            line = line[0..(x[0])] + " " + ans + " " + line[(x[1] + 1)..];
            //Console.WriteLine(line);
        }
        public void AtomRef(int[] x, ref string line, definedFunct func)
        {
            if (x[0] == -1 && x[1] == -1)
            {
                return;
            }
            string atoms = line[x[0]..(x[1] + 1)].Trim();
            string argAtoms = atoms[(atoms.IndexOf('(') + 1)..atoms.LastIndexOf(')')].Trim();
            string ans = "";
            try
            {
                var pro = argAtoms.Trim().Split(" ")[0].Trim();
                //Console.WriteLine(argAtoms);
                //Console.WriteLine("p");
                ans = dict.dict[pro](argAtoms, func != null ? func : null);
                Console.WriteLine(ans);
            }
            catch (Exception)
            {
                if (argAtoms == " ") argAtoms = "()";
                line = line[0..(x[0])] + " " + argAtoms + " " + line[(x[1] + 1)..];
                line = line.Trim();
                return;
            }
            line = line[0..(x[0])] + " " + ans + " " + line[(x[1] + 1)..];
            //Console.WriteLine(line);
        }
        //evaluate sub-expressions in given inout
        public void evaluateFunct(ref string input, definedFunct func = null)
        {
            int i = 00;
            while (i<1)
            {
                int[] x = readFirstpart(input);
                //Console.WriteLine(x[0]);

                if (x[0] == -1)
                {
                    input = input.Replace("( )", "()").Trim();
                    //Console.WriteLine("d");
                    //Console.WriteLine("d");

                    //Console.WriteLine(input);
                    return;
                }
                Program.util.AtomRef(x, ref input, func);
                input = input.Trim();
                //Console.WriteLine("heew");
                //Console.WriteLine(input);
                i++;
            }
        }
        //get next full expression from input
        public int[] readFirstpart(string input)
        {
            int[] index = { -1, -1 };
            int i = 0;
            int count = 0;
            //Console.WriteLine(input);
            string[] subArray = getSubArray(input);
            //Console.WriteLine("1");

            // Console.WriteLine(subArray);

            foreach (string c in subArray)
            {
                if ( c== "(" && count++ == 0 && index[0] == -1)
                {
                    index[0] = i;
                }
                else if (c == ")" && --count == 0 && index[1] == -1)
                {
                    index[1] = i;
                }
                if (index[1] != -1 && index[0] != -1)
                {
                    if (index[1] + index[0] < 2)
                    {
                        index[0] = -1;
                        index[1] = -1;
                        count = 0;
                    }
                    else
                    {
                        string s = String.Join(" ", subArray[index[0]..(index[1] + 1)]);
                        int temp = input.IndexOf(s);
                        return new int[] { temp, s.Length + temp - 1 };
                    }
                }
                i++;
            }
            index= new int[] {-1,-1};
            return index;
        }
        public int[] readNextPart(string s, int start)
        {
            //Console.WriteLine("s=");
            //Console.WriteLine(s[start]);
            //Console.WriteLine(start);
            string temp = s[start..];
            int[] index = readFirstpart(temp);
            //Console.WriteLine("index=");
            //Console.WriteLine(index[0]);
            //Console.WriteLine(index[1]);
            index[0] += start;
            index[1] += start;
            return index;
        }
        public void subVarr(ref string input, definedFunct func = null)
        {
            string[] args = getSubArray(input);
            for(int i=0; i < args.Length; i++)
            {
                if(func != null)
                {
                    if (func.vars.ContainsKey(args[i]))
                    {
                        args[i] = func.getVar(args[i], func);
                    }
                    else
                    {
                        if (func.vars.ContainsKey(args[i]))
                            args[i] = funct.getVar(args[i]);
                    }
                }
                else if (funct.variables.ContainsKey(args[i]))
                {
                    args[i] = funct.getVar(args[i]);
                }
            }
            input = string.Join(" ", args);
        }
        public void Prep(ref string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(' || input[i] == ')')
                {
                    if ((i > 0 && i < input.Length - 1) && (input[i - 1] == '\'' || input[i + 1] == '\''))
                    {
                        continue;
                    }
                    else if (i == 0)
                    {
                        input = input[i] + " " + input[(i + 1)..];
                    }
                    else
                    {
                        input = input[..(i)] + " " + input[i] + " " + input[(i + 1)..];
                        i++;
                    }
                }
            }
            input = recompileString(getSubArray(input));
        }
        public string extractKey(ref string input)
        {
            //Console.WriteLine(input);
            string key = input.Trim().Split(" ")[1];
            input = input[(input.IndexOf(key) + key.Length)..].Trim();
            return key;
        }
        public string extractKey1(ref string input)
        {
            //Console.WriteLine(input);
            string key = input.Trim().Split(" ")[0];
            input = input[(input.IndexOf(key) + key.Length)..].Trim();
            return key;
        }
        public string[] getSubArray(string input, int[] index =null, string[]filter = null)
        {
            //Console.WriteLine(input);
            filter ??= new string[] { };
            index ??= new int[] { 0, input.Length - 1 };
            string s = input[index[0]..(index[1] + 1)];
            return s.Split(" ").Where(x => !filter.Contains(x) && !String.IsNullOrWhiteSpace(x)).ToArray();

        }
        public string recompileString(string[] input)
        {
            return String.Join(" ", input);
        }

    }

}
