using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace CSLISP {
     class Program
    {
        public static utility util; 
        public static lispFunctions lisp = new lispFunctions();
        public static lispDictionary dictionary = new lispDictionary();
        public static string homeDir = Directory.GetCurrentDirectory() + "\\";

        static void Main(string[] args)
        {
            int barLength = 40;

            foreach (string f in Directory.GetFiles(homeDir + "testing", "*.txt"))
            {
                Console.WriteLine(f[(f.LastIndexOf('\\') + 1)..]);
                Console.WriteLine(new string('=', barLength));
                util = new utility(f, dictionary, lisp);
                string hold = "";
                string[] lines = System.IO.File.ReadAllLines(f);
                foreach (string line in lines)
                {
                    //hold = util.readNext().Trim();
                    //Console.WriteLine(line);
                    if (line == "") break;
                    hold= util.recompileString(util.getSubArray(line));
                    //Console.WriteLine(hold);
                    if (hold.IndexOf('(') > -1)
                    {
                        util.Atom(new int[] { 0, hold.Length - 1 }, hold, null);
                        //Console.WriteLine(hold);

                    }
                }
                Console.WriteLine(new string('=', barLength));
                Console.WriteLine();
            }
            Console.WriteLine("Press any key to close the window...");
            Console.ReadKey();
        }
        static string READ(string arg)
        {
            if (arg == "")
            {
                return null;
            }
            util.Prep(ref arg);
            util.Atom(new int[] { 0, arg.Length - 1 },  arg, null);
            return arg;

        }
        static string EVALUATE(string arg) => arg;
        static string PRINT(string arg) => arg;
        static string REP(string arg)
        {
            return PRINT(EVALUATE(READ(arg)));
        }
        internal static int loop()
        {
            for (; ; )//forever loop so that it can read in
            {
                Console.Write("user> ");
                var str = Console.ReadLine();
                if (str == null)
                    break;
                Console.WriteLine(REP(str));
            }
            return 0;
        }

    }
}

