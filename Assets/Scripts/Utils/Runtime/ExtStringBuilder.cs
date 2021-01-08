using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace utils.runtime
{
    public static class ExtStringBuilder
    {
        public static string Build(params string[] words)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < words.Length; i++)
            {
                builder.Append(words[i]);
            }
            return (builder.ToString());
        }
    }
}