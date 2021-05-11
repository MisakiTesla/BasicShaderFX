using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class ScriptBuildHelper
{
    private StringBuilder _stringBuilder;
    private string _lineBreak = "\r\n";
    private int _currentIndex;
    public int IndentTimes { get; set; }
    public ScriptBuildHelper()
    {
        _stringBuilder = new StringBuilder();
    }
    public void Write(string context, bool isIndent = false)
    {
        if (isIndent)
        {
            context = GetIndent() + context;
        }

        if (_currentIndex == _stringBuilder.Length)
        {
            _stringBuilder.Append(context);
        }
        else
        {
            _stringBuilder.Insert(_currentIndex, context);
        }

        _currentIndex += context.Length;
    }
    private void WriteLine(string context, bool isIndent = false)
    {
        Write(context + _lineBreak, isIndent);
    }
    
    private string GetIndent()
    {
        var indent = "";
        for (int i = 0; i < IndentTimes; i++)
        {
            indent += "    ";
        }
        return indent;
    }
    
    private int WriteCurlyBrackets()
    {
        var start = _lineBreak + GetIndent() + "{" + _lineBreak;
        var end = GetIndent() + "}" + _lineBreak;
        Write(start+end,true);
        return end.Length; 
    }
    
    public void WriteUsing(params string[] namespaceNames)
    {
        foreach (var name in namespaceNames)
        {
            WriteLine($"using {name};");
        }
    }
    
    public void WriteEmptyLine()
    {
        Write("");
    }    
    public void WriteNamespace(string name)
    {
        Write($"namespace {name}");
        var length = WriteCurlyBrackets();
        _currentIndex -= length;
    }
    
    public void WriteClass(string name)
    {
        Write($"public class {name} : MonoBehaviour",true);
        var length = WriteCurlyBrackets();
        _currentIndex -= length;

    }

    public void WriteMethod(string name, params string[] paraNames)
    {
        var tmpSB = new StringBuilder();
        tmpSB.Append($"public void {name}()");
        if (paraNames.Length > 0)
        {
            foreach (var s in paraNames)
            {
                tmpSB.Insert(tmpSB.Length - 1, s + ",");
            }

            tmpSB.Remove(tmpSB.Length - 2, 1);
            
        }
        Write(tmpSB.ToString(), true);
        WriteCurlyBrackets();
    }

    public override string ToString()
    {
        return _stringBuilder.ToString();
    }
}
