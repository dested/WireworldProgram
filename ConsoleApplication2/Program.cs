using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            var registries = new List<Registry>()
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



            int line = 0;
            bool nextOne = false;
            while (true)
            {

                if (nextOne)
                {
                    programCounter = nextProgramCounter;
                    nextProgramCounter = -1;
                    nextOne = false;
                }
                if (nextProgramCounter != -1)
                {
                    nextOne = true;
                }
//                Console.SetCursorPosition(0, line++);
                Execute(registries);

                int f = 0;
                /*foreach (var item in registries)
                {
                    Console.SetCursorPosition(50, f);
                    Console.Write(f.ToString().PadLeft(2, '0') + ": " + item.Location1.ToString("x2") + item.Location2.ToString("x2") + " " + item.Data);
                    f++;
                }*/
//                Console.ReadLine();

            }
        }

        private static void Execute(List<Registry> registries)
        {
            var counter = programCounter;
            var reg = registries[programCounter++];


            var value = Read(registries, reg.Location2);
            Write(registries, reg.Location1, new Registry(value.ToString("x"), true));

//            Console.Write($"{counter} Move from {reg.Location2}({value.ToString("x")}) to {reg.Location1} ({registries[reg.Location1].Data })");

        }

        private static short display = 0;

        private static short nextProgramCounter = -1;

        private static void Write(List<Registry> registries, byte location, Registry registry)
        {
            switch (location)
            {
                case 0:
                    var cc = Console.CursorTop;
                   Console.SetCursorPosition(40,0);
                    display = registry.Data;
                    Console.Write(display);
                    Console.SetCursorPosition(0, cc);
                    break;
                case 63:
                    nextProgramCounter = registry.Data;

                    break;
                default:
                    registries[location] = registry;
                    break;
            }
        }


        private static short programCounter = 1;
        private static short Read(List<Registry> registries, byte location)
        {
            switch (location)
            {
                case 0:
                    return 0;
                case 53:
                    return (short)(registries[54].Data & ~registries[53].Data);
                case 54:
                    return (short)(registries[53].Data & ~registries[54].Data);
                case 55:
                    return 0;
                case 56:
                    return registries[56].Data == 0 ? registries[57].Data : registries[55].Data;
                case 57:
                    return 0;
                case 58:
                    return (short)((registries[58].Data >> 1) | (registries[58].Data << (15 - 1)));
                case 59:
                    return (short)((registries[59].Data >> 1) | (registries[59].Data << (15 - 1)));
                case 61:
                    return (short)(registries[60].Data + registries[61].Data);
                case 62:
                    return (short)(~registries[62].Data);
                case 63:
                    return programCounter;
                default:
                    return registries[location].Data;
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
