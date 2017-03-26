﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.CodeDom;
using System.CodeDom.Compiler;
using System;
using System.IO;
public class ScriptBiuld_Service
{


    public static void BuildDll(string[] rootpaths, string outputpath)
    {
        // 设定编译参数,DLL代表需要引入的Assemblies
        CompilerParameters cplist = new CompilerParameters();
        //
        cplist.GenerateExecutable = false;
        //在内存中生成
        cplist.GenerateInMemory = true;
        //生成调试信息
        cplist.IncludeDebugInformation = true;
        cplist.ReferencedAssemblies.Add("System.dll");
        cplist.ReferencedAssemblies.Add("System.Core.dll");
        cplist.ReferencedAssemblies.Add("System.XML.dll");
        cplist.ReferencedAssemblies.Add("System.Data.dll");
        cplist.ReferencedAssemblies.Add(Application.dataPath+ "/../Library/UnityAssemblies/UnityEngine.dll");
        cplist.ReferencedAssemblies.Add(Application.dataPath + "/../Library/UnityAssemblies/UnityEngine.UI.dll");
        //输出path
        cplist.OutputAssembly = outputpath;
        // 编译代理类，C# CSharp都可以
        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

        //文件数组
        List<string> filelist = new List<string>();
        foreach (var rootpath in rootpaths)
        {
            var files = Directory.GetFiles(rootpath, "*.cs", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                //编辑器和插件下的代码不进行编译
                if (files[i].IndexOf("Editor") == -1 && files[i].IndexOf("Resources") == -1&& files[i].IndexOf("Plugins") == -1)
                {
                    filelist.Add(files[i].Replace("\\", "/"));
                }
              
            }
        }
        CompilerResults cr = provider.CompileAssemblyFromFile(cplist, filelist.ToArray());
        if (true == cr.Errors.HasErrors)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
            {
                sb.Append(ce.ToString());
                sb.Append(System.Environment.NewLine);
            }
            throw new Exception(sb.ToString());
        }

        

    }
}