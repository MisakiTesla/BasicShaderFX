using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Wunder.Editor
{
    public class AutoAddNamespace : UnityEditor.AssetModificationProcessor
    {
        private static void OnWillCreateAsset(string assetPath)
        {
            assetPath = assetPath.Replace(".meta", "");
            if (assetPath.EndsWith(".cs"))
            {
                var text = "";
                text += File.ReadAllText(assetPath);
                var className = GetClassName(text);
                Debug.Log($"{className}");

                var scriptContent = GetNewScriptContent(className);
                File.WriteAllText(assetPath, scriptContent);
                AssetDatabase.Refresh();
            }
        }
        
        /// <summary>
        /// 新脚本内容
        /// </summary>
        private static string GetNewScriptContent(string className)
        {
            var scriptBuildHelper = new ScriptBuildHelper();
            scriptBuildHelper.WriteUsing("UnityEngine");
            scriptBuildHelper.WriteEmptyLine();
            scriptBuildHelper.WriteNamespace("Wunder.UI");
            scriptBuildHelper.IndentTimes++;
            
            scriptBuildHelper.WriteClass(className);
            scriptBuildHelper.IndentTimes++;

            scriptBuildHelper.WriteMethod("Start");
            return scriptBuildHelper.ToString();
        }
        

        private static string GetClassName(string text)
        {
            // var datas = text.Split(' ');
            // var index = 0;
            // for (int i = 0; i < datas.Length; i++)
            // {
            //     if (datas[i].Contains("class"))
            //     {
            //         index = i + 1;
            //         break;
            //     }
            // }
            //
            // if (datas[index].Contains(":"))
            // {
            //     return datas[index].Split(':')[0];
            // }
            // else
            // {
            //     return datas[index];
            // }
            
            // public class AutoAddNamespace : 
            var patterm = "public class ([A-Za-z0-9_]+)\\s*:\\s*MonoBehaviour";
            var match = Regex.Match(text, patterm);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return string.Empty;
            }
        }
        
    }
}
