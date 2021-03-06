﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace at.mschwaig.mped.contribution_sorting
{
    public class CharacterMapping
    {
        public int min_ed { get; }
        public int max_ed { get; }

        private char[] from;
        private char[] to;
        private string s1, s2;
        Dictionary<char, char> mapping;

        public CharacterMapping(char[] from, char[] to, string s1, string s2, Dictionary<char, char> mapping)
        {
            this.from = from;
            this.to = to;
            this.s1 = s1;
            this.s2 = s2;
            this.mapping = mapping;

            min_ed = minimum_ed(s1, s2, mapping);
            max_ed = maximum_ed(s1, s2, mapping);
        }

        public bool intersectsWith(CharacterMapping cm)
        {
            return this.mapping.Keys.Intersect(cm.mapping.Keys).Count() > 0 || this.mapping.Values.Intersect(cm.mapping.Values).Count() > 0;
        }

        public CharacterMapping merge(CharacterMapping cm)
        {
            Dictionary<char, char> merged = new Dictionary<char, char>();
            this.mapping.ToList().ForEach(x => merged.Add(x.Key, x.Value));
            cm.mapping.ToList().ForEach(x => merged.Add(x.Key, x.Value));
            return new CharacterMapping(from, to, s1, s2, merged);
        }

        public bool isComplete() {
            return this.mapping.Keys.Count == from.Length;
        }

        private static int minimum_ed(string a, string b, Dictionary<char, char> mapping)
        {
            int[,] mat = ed(a, b, (c1, c2) => !mapping.ContainsKey(c1) || mapping[c1] == c2);

            return mat[mat.GetLength(0) - 1, mat.GetLength(1) - 1];
        }

        private static int maximum_ed(string a, string b, Dictionary<char, char> mapping)
        {
            int[,] mat = ed(a, b, (c1, c2) => mapping.ContainsKey(c1) && mapping[c1] == c2);

            return mat[mat.GetLength(0) - 1, mat.GetLength(1) - 1];
        }

        private static int[,] ed(string a, string b, Func<char, char, bool> mapping)
        {
            int[,] distance = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i < a.Length + 1; i++)
            {
                distance[i, 0] = i;
            }

            for (int j = 0; j < b.Length + 1; j++)
            {
                distance[0, j] = j;
            }

            for (int j = 0; j < b.Length; j++)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (mapping(a[i], b[j]))
                    {
                        distance[i + 1, j + 1] = distance[i, j];
                    }
                    else
                    {
                        int delete = distance[i, j + 1] + 1;
                        int insert = distance[i + 1, j] + 1;
                        int substitute = distance[i, j] + 1;
                        distance[i + 1, j + 1] = Math.Min(delete, Math.Min(insert, substitute));
                    }
                }
            }

            return distance;
        }

        public static CharacterMapping merge(CharacterMapping[] mappings)
        {
            var first = mappings[0];
            var merged_dictionary = mappings.Select(x => x.mapping)
                .SelectMany(dict => dict)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            return new CharacterMapping(first.from, first.to, first.s1, first.s2, merged_dictionary);
        }
    }

}
