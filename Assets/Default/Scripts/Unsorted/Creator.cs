using RoslynCSharp;
using RoslynCSharp.Compiler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Creator {

    public static Creator I = new Creator();
    private ScriptDomain dom = ScriptDomain.CreateDomain("Creator");

    //[ModName][scriptName]
    private Dictionary<string, Dictionary<string, ScriptAssembly>> scripts = new Dictionary<string, Dictionary<string, ScriptAssembly>>();
    private HashSet<Type> typesLoaded = new HashSet<Type>();

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

    /// <summary>
    /// Loads the Type into the doamin
    /// </summary>
    /// <param name="type"></param>
    public void load(Type type) {

        if (!this.typesLoaded.Contains(type)) {
            Assembly assembly = Assembly.GetAssembly(type);
            IMetadataReferenceProvider metaData = AssemblyReference.FromAssembly(assembly);
            this.dom.RoslynCompilerService.ReferenceAssemblies.Add(metaData);
            this.typesLoaded.Add(type);
        }
    }

    /// <summary>
    /// Creates an instance of the script from the given mod
    /// </summary>
    /// <param name="modName"></param>
    /// <param name="scriptName"></param>
    /// <param name="parent"></param>
    /// <param name="inputs"></param>
    /// <returns> Returns an object of the instance from the mod</returns>
    public object Create(string modName, string scriptName, GameObject parent, object[] inputs) {
        return this.scripts[modName][scriptName].MainType.CreateInstance(parent, inputs);
    }

    /// <summary>
    /// Creates an instance of the type passed in from the mod
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="modName"></param>
    /// <param name="scriptName"></param>
    /// <param name="parent"></param>
    /// <param name="inputs"></param>
    /// <returns>Returns an instance of the object of the given type</returns>
    public T Create<T>(string modName, string scriptName, GameObject parent, object[] inputs) {
        return (T)this.Create(modName, scriptName, parent, inputs);
    }


    /// <summary>
    /// Loads the assembly at the path into the domain
    /// </summary>
    /// <param name="path">the path of the Assembly file</param> 
    /// <param name="modName">the name of the mod the Script should go in once loaded</param>
    public void loadAssembly(string path, string modName) {

        ScriptAssembly sa = this.dom.LoadAssembly(path);

        IMetadataReferenceProvider metaData = AssemblyReference.FromAssembly(sa.RawAssembly);
        this.dom.RoslynCompilerService.ReferenceAssemblies.Add(metaData);
        
        string scriptName = sa.MainType.RawType.Name;
        this.addScript(modName, scriptName, sa);    
    }

    /// <summary>
    /// Checks to see if the Type is loaded into the domain
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool isLoaded(Type type) {
        return this.typesLoaded.Contains(type);
    }

    //checks to see if the script is loaded into the mod
    public bool isLoaded(string modName, string scriptName) {
        return this.scripts[modName] != null && this.scripts[modName][scriptName] != null;
    }

    /// <summary>
    /// Adds the scriptAssembly to the dictionary of loadable scripts
    /// </summary>
    /// <param name="modName"></param>
    /// <param name="scriptName"></param>
    /// <param name="scriptAssembly"></param>
    private void addScript(string modName, string scriptName, ScriptAssembly scriptAssembly) {

        if(this.scripts[modName] == null) {
            this.scripts.Add(modName, new Dictionary<string, ScriptAssembly>());
        }
        this.scripts[modName].Add(scriptName, scriptAssembly);
    }

    /// <summary>
    /// Compiles the cSharpScript and returns the CompilationResult of the compisition. See "setCompileOutput" to change the output directory for the assembly file
    /// </summary>
    /// <param name="cSharpScript"></param>
    /// <returns> Returns the CompilationResult of the assembly</returns>
    public CompilationResult compile(string cSharpScript) {
         return dom.RoslynCompilerService.CompileFromSource(cSharpScript);
    }

    /// <summary>
    /// Changes the output directory to the path given in
    /// </summary>
    /// <param name="path">The path where you want the scripts to be compiled at</param>
    public void SetCompileOutput(string path) {
        dom.RoslynCompilerService.OutputDirectory = path;
    }
}