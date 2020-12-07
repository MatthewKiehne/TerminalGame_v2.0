using RoslynCSharp;
using RoslynCSharp.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Creator {

    public static Creator I = new Creator();
    private ScriptDomain dom = ScriptDomain.CreateDomain("Creator");

    public Creator() {

        if( I != null) {
            throw new System.Exception("An instance of Creator has already been made");
        }

    }

    public void load(string path) {
        //dom.RoslynCompilerService.ReferenceAssemblies.Add()
        //AssemblyReference.FromAssembly()
        ScriptAssembly scrAss = dom.CompileAndLoadFile(path);
    }
}