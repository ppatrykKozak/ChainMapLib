using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ChainMapLib;

namespace ChainMapTests
{
    [TestClass]
    public class ChainMapTests
    {
        private ChainMap<string, string> chainMap;

        // Metoda wywoływana przed każdym testem w celu przygotowania danych testowych.

        [TestInitialize]
        public void Setup()
        {
            chainMap = new ChainMap<string, string>();

            var slownik1 = new Dictionary<string, string> { { "a", "1" }, { "b", "2" }, { "c", "3" } };
            var slownik2 = new Dictionary<string, string> { { "b", "22" }, { "c", "33" }, { "d", "44" } };
            var slownik3 = new Dictionary<string, string> { { "c", "333" }, { "d", "444" }, { "e", "555" } };

            chainMap.AddDictionary(slownik1);
            chainMap.AddDictionary(slownik2);
            chainMap.AddDictionary(slownik3);
        }
        // Test sprawdzający pobieranie wartości z głównego słownika.

        [TestMethod]
        public void TestGetValueFromMainDictionary()
        {
            Assert.AreEqual("1", chainMap["a"]);
        }
        // Test sprawdzający pobieranie wartości z dodatkowego słownika.
        [TestMethod]
        public void TestGetValueFromSecondaryDictionary()
        {
            Assert.AreEqual("44", chainMap["d"]);
        }


        // Test sprawdzający dodawanie nowej pary klucz-wartość do głównego słownika.
        [TestMethod]
        public void TestAddToMainDictionary()
        {
            chainMap["f"] = "66";
            Assert.AreEqual("66", chainMap["f"]);
        }


        // Test sprawdzający usuwanie pary klucz-wartość z głównego słownika.
        [TestMethod]
        public void TestRemoveFromMainDictionary()
        {
            chainMap["g"] = "77";
            chainMap.Remove("g");
            Assert.IsFalse(chainMap.ContainsKey("g"));
        }


        // Test sprawdzający metodę ContainsKey.
        [TestMethod]
        public void TestContainsKey()
        {
            Assert.IsTrue(chainMap.ContainsKey("b"));
            Assert.IsFalse(chainMap.ContainsKey("z"));
        }


        // Test sprawdzający dodawanie nowego słownika na określonej pozycji.
        [TestMethod]
        public void TestAddDictionary()
        {
            var nowySlownik = new Dictionary<string, string> { { "h", "88" } };
            chainMap.AddDictionary(nowySlownik, 0);
            Assert.AreEqual("88", chainMap["h"]);
        }


        // Test sprawdzający usuwanie słownika z listy na określonej pozycji.
        [TestMethod]
        public void TestRemoveDictionary()
        {
            chainMap.AddDictionary(new Dictionary<string, string> { { "i", "99" } }, 0);
            chainMap.RemoveDictionary(0);
            Assert.IsFalse(chainMap.ContainsKey("i"));
        }


        // Test sprawdzający usuwanie wszystkich słowników z listy.
        [TestMethod]
        public void TestClearDictionaries()
        {
            chainMap.ClearDictionaries();
            Assert.AreEqual(0, chainMap.CountDictionaries);
        }


        // Metoda wywoływana po zakończeniu każdego testu.
        [TestCleanup]
        public void Cleanup()
        {
            chainMap = null;
        }
    }
}
