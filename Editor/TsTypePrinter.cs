using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CryII.I.EasyHub;
using UnityEngine;

namespace CryII.TsTranslator
{
    public static class TsTypePrinter
    {
        public static Type[] LuaCallCSharp;

        public static void GenCode(IEnumerable<Type> luaCallCSharp, string outFile)
        {
            if (!InitConfig.TryGet(out var easyInitConfig)) return;

            var allTypes = new List<string>();
            LuaCallCSharp = luaCallCSharp.ToArray();
            foreach (var type in LuaCallCSharp)
            {
                if (TsBuilder.TryBuild(type, out var result))
                {
                    allTypes.Add(result);
                }
            }
            
            var tsTypeFilePath = Path.Combine(Application.dataPath, outFile);
            using (var file = new FileStream(tsTypeFilePath, FileMode.OpenOrCreate))
            {
                var bytes = Encoding.UTF8.GetBytes(string.Join("\n", allTypes));
                file.Write(bytes);
            }

            Debug.Log($"declare code generate completed at: {tsTypeFilePath}");
        }
    }
}