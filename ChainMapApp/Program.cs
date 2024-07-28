using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainMapLib;

namespace ChainMapApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Program służący użycia struktur ChainMap

            var chainMap = new ChainMap<string, string>();

            var slownik1 = new Dictionary<string, string>
            {
                { "a", "1" },
                { "b", "2" },
                { "c", "3" }
            };

            var slownik2 = new Dictionary<string, string>
            {
                { "b", "22" },
                { "c", "33" },
                { "d", "44" }
            };

            var slownik3 = new Dictionary<string, string>
            {
                { "c", "333" },
                { "d", "444" },
                { "e", "555" }
            };

            chainMap.AddDictionary(slownik1);
            chainMap.AddDictionary(slownik2);
            chainMap.AddDictionary(slownik3);

            Console.WriteLine(chainMap["a"]); // 1
            Console.WriteLine(chainMap["b"]); // 2
            Console.WriteLine(chainMap["c"]); // 3
            Console.WriteLine(chainMap["d"]); // 44
            Console.WriteLine(chainMap["e"]); // 555

            Console.WriteLine(chainMap["a"]); // 11
            Console.WriteLine(chainMap["b"]); // 22
            Console.WriteLine(chainMap["c"]); // 33
            Console.WriteLine(chainMap["d"]); // 44
            Console.WriteLine(chainMap["e"]); // 55

            // remove from main dictionary

            chainMap.Remove("a");
            Console.WriteLine(chainMap["a"]); // 1

            chainMap.Add("f", "66");
            Console.WriteLine(chainMap["f"]); // 66

            chainMap.Remove("f");
            Console.WriteLine(chainMap.ContainsKey("f")); // False

            chainMap.AddDictionary(new Dictionary<string, string> { { "g", "77" } }, 0);
            Console.WriteLine(chainMap["g"]); // 77

            chainMap.RemoveDictionary(0);
            Console.WriteLine(chainMap.ContainsKey("g")); // False

            chainMap.ClearDictionaries();
            Console.WriteLine(chainMap.CountDictionaries); // 0


            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();

        }
    }
}
