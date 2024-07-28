using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ChainMapLib
{
    public class ChainMap<TKey, TValue> : IDictionary<TKey, TValue>
    {
        // Główny słownik, do którego można dodawać, usuwać i modyfikować elementy
        private readonly IDictionary<TKey, TValue> mainDictionary = new Dictionary<TKey, TValue>();

        // Lista słowników tylko do odczytu
        private readonly List<IDictionary<TKey, TValue>> dictionaries = new List<IDictionary<TKey, TValue>>();

        // Dodaje słownik do listy na określonej pozycji (domyślnie na końcu)

        public void AddDictionary(IDictionary<TKey, TValue> dictionary, int index = -1)
        {
            if (index < 0 || index >= dictionaries.Count)
            {
                dictionaries.Add(dictionary);
            }
            else
            {
                dictionaries.Insert(index, dictionary);
            }
        }


        // Usuwa słownik z listy na określonej pozycji
        public void RemoveDictionary(int index)
        {
            if (index >= 0 && index < dictionaries.Count)
            {
                dictionaries.RemoveAt(index);
            }
        }


        // Usuwa wszystkie słowniki z listy
        public void ClearDictionaries()
        {
            dictionaries.Clear();
        }


        // Zwraca liczbę słowników w liście
        public int CountDictionaries
        {
            get
            {
                return dictionaries.Count;
            }
        }

        // Zwraca listę słowników jako listę tylko do odczytu
        public IReadOnlyList<IDictionary<TKey, TValue>> GetDictionaries()
        {
            return dictionaries.AsReadOnly();
        }


        // Dodaje nową parę klucz-wartość do głównego słownika
        public void Add(TKey key, TValue value)
        {
            if (mainDictionary.ContainsKey(key))
            {
                throw new ArgumentException("W słowniku głównym istnieje już wpis o tym  kluczu.");
            }

            mainDictionary.Add(key, value);
        }


        // Usuwa parę klucz-wartość z głównego słownika
        public bool Remove(TKey key)
        {
            return mainDictionary.Remove(key);
        }


        // Próbuje pobrać wartość dla zadanego klucza, przeszukując główny słownik oraz wszystkie dodatkowe słowniki
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (mainDictionary.TryGetValue(key, out value))
            {
                return true;
            }

            foreach (var dictionary in dictionaries)
            {
                if (dictionary.TryGetValue(key, out value))
                {
                    return true;
                }
            }

            value = default(TValue);
            return false;
        }


        // Sprawdza, czy klucz istnieje w głównym słowniku lub w którymkolwiek z dodatkowych słowników
        public bool ContainsKey(TKey key)
        {
            if (mainDictionary.ContainsKey(key))
            {
                return true;
            }

            foreach (var dictionary in dictionaries)
            {
                if (dictionary.ContainsKey(key))
                {
                    return true;
                }
            }

            return false;
        }

        // Sprawdza, czy wartość istnieje w głównym słowniku lub w którymkolwiek z dodatkowych słowników
        public bool ContainsValue(TValue value)
        {
            if (mainDictionary.Values.Contains(value))
            {
                return true;
            }

            foreach (var dictionary in dictionaries)
            {
                if (dictionary.Values.Contains(value))
                {
                    return true;
                }
            }

            return false;
        }


        // Zwraca wszystkie klucze ze wszystkich słowników
        public ICollection<TKey> Keys
        {
            get
            {
                var keys = new HashSet<TKey>(mainDictionary.Keys);
                foreach (var dictionary in dictionaries)
                {
                    foreach (var key in dictionary.Keys)
                    {
                        keys.Add(key);
                    }
                }
                return keys;
            }
        }


        // Zwraca wszystkie wartości ze wszystkich słowników
        public ICollection<TValue> Values
        {
            get
            {
                var values = new List<TValue>(mainDictionary.Values);
                foreach (var dictionary in dictionaries)
                {
                    foreach (var value in dictionary.Values)
                    {
                        values.Add(value);
                    }
                }
                return values;
            }
        }


        // Indeksator umożliwiający dostęp do wartości na podstawie klucza
        public TValue this[TKey key]
        {
            get
            {
                return GetValueByKey(key);
            }
            set
            {
                SetValueByKey(key, value);
            }
        }
        // Metoda pobierająca wartość na podstawie klucza
        private TValue GetValueByKey(TKey key)
        {
            if (mainDictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            foreach (var dictionary in dictionaries)
            {
                if (dictionary.TryGetValue(key, out value))
                {
                    return value;
                }
            }

            throw new KeyNotFoundException();
        }

        // Metoda ustawiająca wartość na podstawie klucza
        private void SetValueByKey(TKey key, TValue value)
        {
            if (mainDictionary.ContainsKey(key))
            {
                mainDictionary[key] = value;
            }
            else
            {
                Add(key, value);
            }
        }


        // Zwraca łączną liczbę elementów we wszystkich słownikach
        public int Count
        {
            get
            {
                int count = mainDictionary.Count;
                foreach (var dictionary in dictionaries)
                {
                    count += dictionary.Count;
                }
                return count;
            }
        }

        // Wskazuje, czy słownik jest tylko do odczytu (zawsze false dla głównego słownika)
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }


        // Dodaje parę klucz-wartość do głównego słownika (dla interfejsu ICollection<KeyValuePair<TKey, TValue>>)
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }


        // Usuwa wszystkie elementy z głównego słownika
        public void Clear()
        {
            mainDictionary.Clear();
        }


        // Sprawdza, czy para klucz-wartość istnieje w głównym słowniku lub w którymkolwiek z dodatkowych słowników
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (mainDictionary.Contains(item))
            {
                return true;
            }

            foreach (var dictionary in dictionaries)
            {
                if (dictionary.Contains(item))
                {
                    return true;
                }
            }

            return false;
        }

        // Kopiuje elementy słownika do tablicy
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var kvp in mainDictionary)
            {
                array[arrayIndex++] = kvp;
            }
            foreach (var dictionary in dictionaries)
            {
                foreach (var kvp in dictionary)
                {
                    array[arrayIndex++] = kvp;
                }
            }
        }


        // Zwraca enumerator dla wszystkich par klucz-wartość we wszystkich słownikach
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return mainDictionary.Concat(dictionaries.SelectMany(d => d)).GetEnumerator();
        }


        // Zwraca enumerator (dla interfejsu IEnumerable)
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        // Usuwa parę klucz-wartość z głównego słownika (dla interfejsu ICollection<KeyValuePair<TKey, TValue>>)
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return mainDictionary.Remove(item.Key);
        }
    }
}
