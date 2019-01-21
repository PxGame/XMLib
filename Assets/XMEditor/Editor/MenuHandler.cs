/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/21/2019 9:42:23 PM
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace XMEditor
{
    /// <summary>
    /// 按钮集合
    /// </summary>
    public static class MenuHandler
    {

        /// <summary>
        /// 创建脚本
        /// </summary>
        [MenuItem("Assets/XMLib/Create/脚本")]
        public static void CreateLibScript()
        {
            ScriptCreator.CreateLib();
        }

        /// <summary>
        /// 创建编辑器脚本
        /// </summary>
        [MenuItem("Assets/XMLib/Create/编辑器脚本")]
        public static void CreateEditorScript()
        {
            ScriptCreator.CreateEditor();
        }

        /// <summary>
        /// 创建测试脚本
        /// </summary>

        [MenuItem("Assets/XMLib/Create/测试脚本")]
        public static void CreateLibTestScript()
        {
            ScriptCreator.CreateLibTest();
        }

        /// <summary>
        /// 创建测试运行脚本
        /// </summary>
        [MenuItem("Assets/XMLib/Create/测试运行脚本")]
        public static void CreateLibTestRunnerScript()
        {
            ScriptCreator.CreateLibTestRunner();
        }
    }
}