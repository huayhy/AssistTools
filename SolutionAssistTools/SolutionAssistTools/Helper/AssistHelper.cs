/*
    辅助工具的辅助功能
 */
using EnvDTE;
using EnvDTE80;
using Faker.AssistTools.Modules;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Faker.AssistTools.Helper
{
    public class AssistHelper
    {
        /// <summary>
        /// 选择清单后的工具初始化入口函数
        /// </summary>
        /// <param name="dte">选择工具dte</param>
        public static FileEntity AssistInitialize(DTE2 dte)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var solution = dte.Solution; // 解决方案
            var selectedItems = dte.ToolWindows.SolutionExplorer.SelectedItems as UIHierarchyItem[];
            if (selectedItems == null || selectedItems.Length == 0)
            {
                return null;
            }
            // 选择对象
            var selectedItem = selectedItems[0]?.Object as ProjectItem;
            if (selectedItem == null)
            {
                return null;
            }
            // 项目
            var project = selectedItem.ContainingProject;
            if (project == null)
            {
                return null;
            }
            // 项目名
            var projectName = project.Name;
            var path = project?.FullName;
            var srcPath = project?.Properties.Item("FullPath").Value?.ToString();

            // 判断一下必须来自领域层
            if (!IsExitAbpCore(projectName, ProjectCore.Layer)) {
                return null;
            }

            // 当前选中的文件
            var FileEntity = new FileEntity
            {
                SrcDir      = string.Format("{0}//src/", Path.GetDirectoryName(solution.FullName)),
                ServDir     = string.Format("{0}//serv/", Path.GetDirectoryName(solution.FullName)),
                Name = Path.GetFileNameWithoutExtension(selectedItem.Name),  // 选择项名称
                FullName = selectedItem.Name,
                FullFileName = selectedItem.FileNames[0], // 当前选中的文件全名称
                CurrentFile = new FileInfo(selectedItem.FileNames[0]) // 选中的文件对象
            };

            // 处理解决方案数据
            FileEntity.Solution.Name        = Path.GetFileNameWithoutExtension(solution.FullName); //Path.GetFileName(solution.FullName)
            FileEntity.Solution.BaseDirPath = Path.GetDirectoryName(solution.FullName);//解决方案目录
            // 核心层数据
            var dirCore = AssistHelper.GetLayerDir(FileEntity.SrcDir,ProjectCore.Layer);
            FileEntity.ProjectCore.Name = dirCore?.Name; // 项目的名称
            FileEntity.ProjectCore.BaseDirPath = dirCore?.FullName; // Src 路径  + 项目名称
            // 应用层数据
            var dirApplication = AssistHelper.GetLayerDir(FileEntity.SrcDir, Application.Layer);
            FileEntity.Application.Name = dirApplication?.Name;
            FileEntity.Application.BaseDirPath = dirApplication?.FullName;
            FileEntity.Application.DomainPath = FileEntity.CurrentFile.DirectoryName.Replace(FileEntity.ProjectCore.Name, FileEntity.Application.Name);
            // 基础设施层数据
            var dirFrameworkCore = AssistHelper.GetLayerDir(FileEntity.SrcDir, FrameworkCore.Layer);
            FileEntity.FrameworkCore.Name = dirFrameworkCore?.Name;
            FileEntity.FrameworkCore.BaseDirPath = dirFrameworkCore?.FullName;
            FileEntity.LoadData();
            return FileEntity;
        }

        /// <summary>
        /// 返回扩展的应用服务目录
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static DirectoryInfo GetExtraLayerDir(string servDir, string layer)
        {
            var dirName = Path.Combine(servDir, layer);
            DirectoryInfo dir = new DirectoryInfo(dirName);
            // 目录不存在顺手创建一个
            if (!dir.Exists)
                dir.Create();
            // 如果存在找一下是否存在指定项目目录layer
            return dir;
        }

        /// <summary>
        /// 找到指定层级的目录
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static DirectoryInfo GetLayerDir(string srcDir, string layer ) {

            DirectoryInfo dir = new DirectoryInfo(srcDir);
            var dirs = dir.GetDirectories(string.Format("*{0}", layer));
            if (dirs.Length > 0) {
                return dirs[0];
            }
            return null;
        }

        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="s">开始</param>
        /// <param name="e">结束</param>
        /// <returns></returns> 
        public static string GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }

        /// <summary>
        /// 解析文本获取所有的属性集合
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ICollection<FiledEntity> GetFields(string str)
        {
            Regex rg = new Regex("(?:public\\s|private\\s|protected\\s)\\s*(?:readonly\\s+)?(?:override\\s+)?(?<type>\\w+)\\s+(?<name>\\w+)", RegexOptions.Multiline | RegexOptions.Singleline);

            var matches = rg.Matches(str);

            var list = new List<FiledEntity>();

            foreach (Match model in matches) {

                if (model.Groups["type"].Value == "class") continue;
                var filed = new FiledEntity
                {
                    Name = model.Groups["name"].Value.Trim(),
                    TypeName = model.Groups["type"].Value.Trim(),
                    CName = "字段" + model.Groups["name"].Value.Trim()
                };

                list.Add(filed);
            }

            return list;
        }

        /// <summary>
        /// 解析类的名称数据信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetClassInfo(string str)
        {
            // 找到类声明这块
            var strClass = AssistHelper.GetValue(str, ":","{");
            

            return strClass.ToString();
        }

        /// <summary>
        /// 判断项目名称是否来自领域层
        /// </summary>
        /// <param name="ProjectName"></param>
        /// <returns></returns>
        public static bool IsExitAbpCore(string ProjectName,string Layer)
        {
            return ProjectName.EndsWith(Layer);
        }

        /// <summary>
        /// 判断当前项目是否为ABP项目
        /// </summary>
        /// <returns></returns>
        public static bool IsAbpSolution(string srcDir)
        {
            // 判断是否存在src目录
            if (!Directory.Exists(srcDir)){
                return false;
            }
            // 检查是否存在应用服务层
            if (Directory.GetDirectories(srcDir, Application.Layer).Length > 0)
            {
                return false;
            }
            // 检查是否存在领域服务层
            if (Directory.GetDirectories(srcDir, ProjectCore.Layer).Length > 0)
            {
                return false;
            }
            // 检查是否存在基础设施层
            if (Directory.GetDirectories(srcDir, FrameworkCore.Layer).Length > 0)
            {
                return false;
            }
            return true;
        }
    }
}
