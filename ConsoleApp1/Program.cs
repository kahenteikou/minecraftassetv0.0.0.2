using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCAssetsDOWNA;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Error\nオプションを指定してください。");
                return;

            }
            if (args.Length == 1)
            {
                Console.WriteLine("Error\nオプションを指定してください。");
                return;

            }
            string outpath3 = args[1].TrimEnd('\\','"');
            Console.Write(outpath3);
            Console.WriteLine();
            Core ASCore = new Core();
            ASCore.Assets(args[0],outpath3);

        }
    }
}
