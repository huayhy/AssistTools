/*
    
 */
using Commons.Collections;
using Faker.AssistTools.Helper;
using NVelocity.App;
using NVelocity.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Faker.AssistTools.Modules
{
    /// <summary>
    /// 一个文件实体数据包
    /// </summary>
    public class FileEntity
    {
        //重写数据源规则一个文件实体需要包含 解决方案 应用服务 领域服务 基础设施 4个对象的数据

       /// <summary>
       /// 实体信息
       /// </summary>
        public EntityInfo EntityInfo { get; set; }

        /// <summary>
        /// 解决方案相关数据
        /// </summary>
        public Solution Solution { get; set; }

        /// <summary>
        /// 应用服务相关数据
        /// </summary>
        public Application Application { get; set; }

        /// <summary>
        /// 基础设施层相关数据
        /// </summary>
        public FrameworkCore FrameworkCore { get; set; }

        /// <summary>
        /// 项目核心层相关数据
        /// </summary>
        public ProjectCore ProjectCore { get; set; }

        /// <summary>
        /// 解析出来的字段实体（或者新增的实体）
        /// </summary>
        public ICollection<FiledEntity> Fields { get; set; }

        public FileEntity (){

            this.Solution = new Solution();
            this.Application = new Application();
            this.FrameworkCore = new FrameworkCore();
            this.ProjectCore = new ProjectCore();
            this.ClassEntity = new ClassEntity();
        }

        /// <summary>
        /// 当前实体的命名空间
        /// </summary>
        public string NameSpace { get; set; }
        /// <summary>
        ///  ABP源代码的目录
        /// </summary>
        public string SrcDir { get; set; }

       
        /// <summary>
        /// 当前选择的文件名称 Member.cs
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 当前选择的文件名称 Member
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 当前选择的文件路径 D:\\App\\Demo\\测试工程\\SOEI解决方案框架\\src\\SOEI.Solution.Core\\DomainEntities\\Member\\Member.cs
        /// </summary>
        public string FullFileName { get; set; }
        /// <summary>
        /// 当前选择的实体文件FileInfo
        /// </summary>
        public FileInfo CurrentFile { get; set; }

        /// <summary>
        /// 类的临时属性
        /// </summary>
        public ClassEntity ClassEntity { get; set; }

        /// <summary>
        /// 继承信息
        /// </summary>
        public string Inherit { get; set; }

        /// <summary>
        /// 公司活组织名称 比如 Faker.Solution 的 Faker 部分
        /// </summary>
        public string CpmpanyName  { get; set; }

        /// <summary>
        /// 项目子名称 比如 Faker.Solution 的 Solution 部分
        /// </summary>
        public string SubName { get; set; }

        /// <summary>
        /// 读取当前数据
        /// </summary>
        public void LoadData() {

            string text = File.ReadAllText(FullFileName);
            this.Fields = AssistHelper.GetFields(text);
            // 获取命名空间
            this.NameSpace = AssistHelper.GetValue(text, "namespace", "{").Trim();
            // 获取类名称继承接信息
            this.ClassEntity.Inherit = AssistHelper.GetClassInfo(text);
            // 获取方案名称
            var strs = this.Solution.Name.Split('.');
            if (strs.Length == 2)
            {
                this.CpmpanyName = strs[0];
                this.SubName = strs[1];
            }
            if (strs.Length == 1)
            {
                this.CpmpanyName = strs[0];
                this.SubName = strs[0];
            }
        }
    }
}
