using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        private static List<Registry> registries;
        static short[] ram = new short[64];
        private static byte[] registriesLocation1 = new byte[64];
        private static byte[] registriesLocation2 = new byte[64];
        static void Main(string[] args)
        {
            registries = new List<Registry>()
            {
                new Registry("0000"),
                new Registry("001e"),
                new Registry("361f"),
                new Registry("2021"),
                new Registry("3c22"),
                new Registry("3d23"),
                new Registry("3c3d"),
                new Registry("3d20"),
                new Registry("3924"),
                new Registry("3725"),
                new Registry("383d"),
                new Registry("3f38"),
                new Registry("3d3d"),
                new Registry("3d3d"),
                new Registry("353d"),
                new Registry("3926"),
                new Registry("3727"),
                new Registry("3836"),
                new Registry("3f38"),
                new Registry("3928"),
                new Registry("0020"),
                new Registry("3d20"),
                new Registry("3c1e"),
                new Registry("3f29"),
                new Registry("203d"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0002"),
                new Registry("7fff"),
                new Registry("0005"),
                new Registry("0003"),
                new Registry("fffe"),
                new Registry("fffd"),
                new Registry("0014"),
                new Registry("000d"),
                new Registry("0009"),
                new Registry("0005"),
                new Registry("0015"),
                new Registry("0004"),
                new Registry("fffc"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
                new Registry("0000"),
            };

            for (int index = 0; index < registries.Count; index++)
            {
                ram[index] = registries[index].Data;
                registriesLocation1[index] = registries[index].Location1;
                registriesLocation2[index] = registries[index].Location2;
            }

            long c = 0;
            int line = 0;
            stopwatch.Start();

            while (true)
            {
                c ++;
                if (c%100000000 == 0)
                {
                    Console.WriteLine("Ms Per Run: "+(((stopwatch.ElapsedTicks / (double)(Stopwatch.Frequency / (double)1000 / 1000 / 1000)) / (double)c).ToString("F10")));
                    stopwatch.Reset();
                    stopwatch.Start();
                }
                if (nextProgramCounter != -1)
                {
                    Execute();
                    programCounter = nextProgramCounter;
                    nextProgramCounter = -1;
                }
                else
                {
                    Execute();

                }                //                Console.SetCursorPosition(0, line++);

                /*int f = 0;
                foreach (var item in registries)
                {
                    Console.SetCursorPosition(50, f);
                    Console.Write(f.ToString().PadLeft(2, '0') + ": " + item.Location1.ToString("x2") + item.Location2.ToString("x2") + " " + item.Data);
                    f++;
                }*/
                //                Console.ReadLine();

            }
        }

        private static Stopwatch stopwatch=new Stopwatch();
        private static void Execute()
        {
            //            var counter = programCounter;
            var s = programCounter++;

            var value = Read(registriesLocation2[s]);
            Write(registriesLocation1[s], value);

            //            Console.Write($"{counter} Move from {reg.Location2}({value.ToString("x")}) to {reg.Location1} ({registries[reg.Location1].Data })");

        }

        private static short display = 0;

        private static short nextProgramCounter = -1;

        private static void Write(byte location, short registry)
        {
            switch (location)
            {
                case 0:
                    //                    var cc = Console.CursorTop;
                    //                   Console.SetCursorPosition(40,0);
                    display = registry;
//                    Console.WriteLine(display);
                    //                    Console.SetCursorPosition(0, cc);
                    break;
                case 63:
                    nextProgramCounter = registry;
                    break;
                default:
                    ram[location] = registry;
                    break;
            }
        }


        private static short programCounter = 1;

        private static short Read(byte location)
        {
            switch (location)
            {
                case 0:
                    return 0;
                case 53:
                    return (short)(ram[54] & ~ram[53]);
                case 54:
                    return (short)(ram[53] & ~ram[54]);
                case 55:
                    return 0;
                case 56:
                    return ram[56] == 0 ? ram[57] : ram[55];
                case 57:
                    return 0;
                case 58:
                    return (short)((ram[58] >> 1) | (ram[58] << (15 - 1)));
                case 59:
                    return (short)((ram[59] >> 1) | (ram[59] << (15 - 1)));
                case 61:
                    return (short)(ram[60] + ram[61]);
                case 62:
                    return (short)(~ram[62]);
                case 63:
                    return programCounter;
                default:
                    return ram[location];
            }
        }
    }


    class Registry
    {
        public short Data;
        public byte Location1;
        public byte Location2;

        public Registry(string i, bool @fixed = false)
        {

            Data = short.Parse(i, NumberStyles.HexNumber);

            if (!@fixed && Data < 0)
            {
                Data += 1;
                i = Data.ToString("x4");
            }

            Location1 = byte.Parse(i.PadLeft(4, '0').Substring(0, 2), NumberStyles.HexNumber);
            Location2 = byte.Parse(i.PadLeft(4, '0').Substring(2, 2), NumberStyles.HexNumber);

        }
    }
}
