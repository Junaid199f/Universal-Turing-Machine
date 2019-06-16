using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TuringMachine
{
    class Program
    {
        public static TM findtransition(List<TM> finding,string state,char current){
        for(int i=0;i<finding.Count;i++){
            if (finding[i].initialstate == state && finding[i].current == current) {
                return finding[i];
            }
        }
            return null;
        }
        public static void Main(string[] args)
        {
            List<Tape> tapelist = new List<Tape>();
            string initialstate = "";
            string finalstate = "";
            List<TM> transition = new List<TM>();
            int opt=1;
            while (opt != 7)
            {
                Console.WriteLine("Please Select Options");
                Console.WriteLine("1.Read Input Tape");
                Console.WriteLine("2.Read Turing Machine");
                Console.WriteLine("3.Run Universal Turing Machine");
                opt = Convert.ToInt32(Console.ReadLine());
                if (opt == 1)
                {
                    String line;
                    string inputtape;
                    Console.WriteLine("Enter Input Tape File Name with extension inp");
                    inputtape = Console.ReadLine();
                    string dir = Directory.GetCurrentDirectory();
                    StreamReader sr = new StreamReader(dir + "\\" + inputtape);
                    line = sr.ReadLine();
                    while (line != null)
                    {
                        //write the lie to console window
                        Console.WriteLine(line);
                        char[] words = line.ToCharArray();
                        Tape initial = new Tape(false,'b');
                        tapelist.Add(initial);
                        for (int i = 0; i < words.Length; i++)
                        {
                            Console.WriteLine(words[i]);
                            Tape main = new Tape(false, words[i]);
                            tapelist.Add(main);
                        }
                        Tape final = new Tape(false, 'b');
                        tapelist.Add(final);
                        inputtape = line;
                        //Read the next line
                        line = sr.ReadLine();
                    }
                    sr.Close();
                }

                else if (opt == 2)
                {
                    String line;
                    string filename;
                    Console.WriteLine("Enter Filename");
                    filename = Console.ReadLine();
                    string dir = Directory.GetCurrentDirectory();
                    StreamReader sr = new StreamReader(dir + "\\" + filename);
                    line = sr.ReadLine();
                    while (line != null)
                    {
                        string currentstate = "";
                        string final = "";
                        char currentsymbol;
                        char writesymbol;
                        char move;
                        //write the lie to console window
                        Console.WriteLine(line);
                        char[] words = line.ToCharArray();
                        if (words[0] == 'I')
                        {
                            Console.WriteLine("Input String Detected");
                            for (int i = 2; i < words.Length; i++)
                            {
                                initialstate = initialstate + words[i].ToString();
                            }
                            Console.WriteLine(initialstate);
                        }
                        if (words[0] == 'F')
                        {
                            Console.WriteLine("Input String Detected");
                            for (int i = 2; i < words.Length; i++)
                            {
                                finalstate = finalstate + words[i].ToString();
                            }
                            Console.WriteLine(finalstate);
                        }
                        if (words[0] == '(')
                        {
                            int i = 1;
                            for (; words[i] != ','; i++)
                            {
                                currentstate = currentstate + words[i].ToString();
                            }
                            Console.WriteLine(words[i]);
                            currentsymbol = words[i + 1];
                            writesymbol = words[i + 3];
                            move = words[i + 5];
                            Console.WriteLine("Initial State is " + currentstate + "currrent symbol is" + currentsymbol + "write symbol is " + writesymbol + "move is " + move);
                            for (i = i + 7; words[i] != ')'; i++)
                            {
                                final = final + words[i].ToString();
                            }
                            Console.WriteLine("final state is" + final);
                            TM adddata = new TM(currentstate, currentsymbol, writesymbol, move, final);
                            transition.Add(adddata);
                        }

                        //Read the next line
                        line = sr.ReadLine();
                    }
                    //close the file
                    sr.Close();
                   
                    
                }
                else if (opt == 3)
                {
                    tapelist[1].states = initialstate;
                    tapelist[1].isactive = true;
                    int i = 1;
                    do
                    {
                        for (int k = 0; k < tapelist.Count; k++) {
                            if (tapelist[k].isactive == true)
                            {
                                Console.Write("|" + tapelist[k].states + "|");
                            }
                            else
                            {
                                Console.Write("|" + tapelist[k].data + "|");
                            }
                        }
                        if (tapelist[i].isactive == true)
                        {
                        TM trans = findtransition(transition, tapelist[i].states, tapelist[i].data);
                        if (trans.movement == 'R') {
                            tapelist[i].isactive = false;
                            if (i < tapelist.Count)
                            {
                                i = i + 1;
                                tapelist[i].states = trans.finalstate;
                                if (tapelist[i].states == finalstate)
                                {
                                    Console.WriteLine("Machine Accepted");
                                    break;
                                }
                                tapelist[i].isactive = true;
                            }
                            else
                            {
                                Tape newtape = new Tape(true,trans.next);
                                newtape.states = trans.finalstate;
                                tapelist.Add(newtape);
                            }
                        }
                        else if (trans.movement == 'L') {
                            tapelist[i].isactive = false;
                            if (i >= 0)
                            {
                                i = i - 1;
                                tapelist[i].states = trans.finalstate;
                                if (tapelist[i].states == finalstate)
                                {
                                    Console.WriteLine("Machine Accepted");
                                    break;
                                }
                                else
                                {
                                    Tape newtape = new Tape(true, trans.next);
                                    newtape.states = trans.finalstate;
                                    tapelist.Insert(0, newtape);
                                }
                                tapelist[i].isactive = true;
                            }
                        }
                        else if (trans.movement == 'S') {
                            tapelist[i].states = trans.finalstate;
                            if (tapelist[i].states == finalstate) {
                                Console.WriteLine("Machine Accepted");
                                break;
                            }
                        }
                        }
                        else
                        {
                            Console.WriteLine("Machine Halted");
                            i++;
                        }
                        Console.ReadLine();
                    } while (i >= 0 && i < tapelist.Count);

                        }
                    }
                   
                }


            }
        }
    class States {
        string tape;
    }
    class TM {
       public string initialstate;
       public string finalstate;
       public char current;
       public char next;
       public char movement;
        public TM(string currentstate, char currentsymbol, char writesymbol, char move, string final){
            this.initialstate = currentstate;
            this.finalstate = final;
            this.current = currentsymbol;
            this.movement = move;
            this.next = writesymbol;
        }
    }
    class Tape {
       public bool isactive;
       public char data;
       public string states;
       public Tape(bool isactive,char data) {
            this.isactive = isactive;
            this.data = data;
        }
    }

