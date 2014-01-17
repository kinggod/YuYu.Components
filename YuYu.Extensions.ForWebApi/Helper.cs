using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    internal class Helper
    {
        internal static object CreateObject(string code, params string[] referencedAssemblies)
        {
            string tempsPath = AppDomain.CurrentDomain.BaseDirectory + "Temps\\";
            if (!Directory.Exists(tempsPath))
                Directory.CreateDirectory(tempsPath);
            string codeFile = tempsPath + "__temp.cs";
            string assemblyFile = tempsPath + "__temp.dll";
            File.Delete(codeFile);
            File.Delete(assemblyFile);
            FileStream fs = File.Open(codeFile, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(code);
            sw.Close();
            sw.Dispose();
            fs.Close();
            fs.Dispose();
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            parameters.OutputAssembly = assemblyFile;
            if (referencedAssemblies != null && referencedAssemblies.Length > 0)
                parameters.ReferencedAssemblies.AddRange(referencedAssemblies);
            CompilerResults results = provider.CompileAssemblyFromFile(parameters, codeFile);
            if (results.Errors.HasErrors || results.Errors.HasWarnings)
                return null;
            else
                return results.CompiledAssembly.CreateInstance("__temp.__temp");
        }
    }
}
