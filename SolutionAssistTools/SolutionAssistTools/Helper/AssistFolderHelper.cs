using EnvDTE;
using EnvDTE80;
using Faker.AssistTools.Modules;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.AssistTools.Helper
{
    public class AssistFolderHelper
    {
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
            // 获取选中目标基础信息
            var FileEntity = new FileEntity
            {
                SrcDir = string.Format("{0}//src/", Path.GetDirectoryName(solution.FullName)),
                Name = Path.GetFileNameWithoutExtension(selectedItem.Name),  // 选择项名称
                FullName = selectedItem.Name,
                FullFileName = selectedItem.FileNames[0], // 当前选中的文件全名称
                CurrentFile = new FileInfo(selectedItem.FileNames[0]) // 选中的文件对象
            };
            // 检查是否ABP项目
            if (!AssistHelper.IsAbpSolution(FileEntity.SrcDir))
            {
                return null;
            }

            // 处理解决方案数据
            FileEntity.Solution.Name = Path.GetFileNameWithoutExtension(solution.FullName); //Path.GetFileName(solution.FullName)
            FileEntity.Solution.BaseDirPath = Path.GetDirectoryName(solution.FullName);//解决方案目录
            // 核心层数据
            var dirCore = AssistHelper.GetLayerDir(FileEntity.SrcDir, ProjectCore.Layer);
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
            return FileEntity;
        }
    }
}
