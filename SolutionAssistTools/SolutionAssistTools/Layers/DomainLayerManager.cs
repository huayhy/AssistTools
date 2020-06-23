/*
    领域服务层生成接口实现
 */
using Faker.AssistTools.Modules;
using NVelocity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.AssistTools.Layers
{

    public class DomainLayerManager : BaseLayerManager, ILayerManager
    {

        public string DomainServiceName = "DomainService";

        public string AuthorizationName = "Authorization";

        // 需要一个变量来配置生成参数
        protected FileEntity FileEntity;

        public  DomainLayerManager(FileEntity _FileEntity) 
        {
            FileEntity = _FileEntity;
        }

        /// <summary>
        /// 创建接口实现
        /// </summary>
        public  void CreateLayer()
        {
            // 这里需要给领域服务层创建所需要的文件目录和数据准备工作
            this.createDirectory();

            // 需要添加一个首次判断
            if (APP.Configuration.IsFirst)
                this.Create_Base_Files();

            // 创建领域服务所需要的文件
            if (APP.Configuration.UseDomainService)
            {
                this.CreateFiles();
            }
        }

        /// <summary>
        /// 输出层文件对象
        /// </summary>
        public void OutputLayer()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 领域层所需要的目录
        /// </summary>
        protected void createDirectory()
        {
            //1. 创建领域实体目录 DomainService 在当前实体目录下创建
            var DomainServiceDir = string.Format("{0}\\{1}", FileEntity.CurrentFile.Directory.FullName, this.DomainServiceName);

            // 不存在这个目录则创建
            if (!Directory.Exists(DomainServiceDir))
            {
                Directory.CreateDirectory(DomainServiceDir);
            }
            // 创建身份验证目录
            this.CreateDirectory(FileEntity.CurrentFile.Directory.FullName, this.AuthorizationName);
        }

        /// <summary>
        /// 创建领域服务所需文件
        /// </summary>
        protected void CreateFiles()
        {
            // 创建接口
            this.createIManager();
            // 创建实现类
            this.createManager();

            if (APP.Configuration.UseDomainService)
            {
                // 创建权限相关
                this.CreateAuthorization();
            }
            // 创建Readme
            this.Create_Readme();
        }

        /// <summary>
        /// 创建处理框架文件
        /// </summary>
        protected void Create_Base_Files() {

            // 创建领域服务框架基类
            this.create_DomainServiceBase();
            this.create_ConstBase();
            // 领域服务的 Authorization 目录要添加一个默认权限 AppLtmPermissions.cs
            this.Create_Base_AppLtmPermissions();
        }

        /// <summary>
        /// 创建身份验证
        /// </summary>
        public void CreateAuthorization()
        {
            // 生成权限
            this.Create_Permissions();
            this.Create_AuthorizationProvider();
        }

        /// <summary>
        /// 基础架构 需要进行首次判断
        /// </summary>
        protected void create_DomainServiceBase() 
        {
            // 1. 当前实体的同级目录下创建领域服务文件夹 DomainService
            var fileName = "DomainServiceBase.cs";
            var filePath = Path.Combine(FileEntity.ProjectCore.BaseDirPath, fileName);
            // 获取模板并且设置参数
            Template template = VelocityEngine.GetTemplate("DomainServiceBase.vm");

            var strs = FileEntity.Solution.Name.Split('.');
            var bName = string.Empty;
            if (strs.Length > 1)
            {
                bName = strs[1];
            }
            else {
                bName = strs[0];
            }

            //var bName = FileEntity.Solution.Name.Split('.')?[1];
            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.Solution.Name, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                BaseName  = bName
            };

            VelocityContext context = new VelocityContext();
            context.Put("people", "华威");
            context.Put("model", model);

            StringWriter writer = new StringWriter();
            template.Merge(context, writer);
            var result = writer.GetStringBuilder().ToString();

            //2. 内容写入到指定文件

            File.WriteAllText(filePath, result);
        }

        /// <summary>
        /// 创建权限常量
        /// </summary>
        public void Create_Base_AppLtmPermissions()
        {
            string path = string.Empty;
            string fileName = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.NameSpace, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                SolutionName = FileEntity.Solution.Name,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                SolutionTxt = FileEntity.SubName,
            };

            fileName = "AppLtmPermissions.cs";
            path = Path.Combine(FileEntity.ProjectCore.BaseDirPath, this.AuthorizationName, fileName);
            this.CreateFile(path, "AppLtmPermissions.vm", model);
        }

        /// <summary>
        /// 创建配置常量
        /// </summary>
        public void create_ConstBase() 
        {
            // 1. 当前实体的同级目录下创建领域服务文件夹 DomainService
            var fileName = "AppCoreConst.cs";
            var filePath = Path.Combine(FileEntity.ProjectCore.BaseDirPath, fileName);
            // 获取模板并且设置参数
            Template template = VelocityEngine.GetTemplate("DomainConst.vm");

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.Solution.Name, // 命名空间
                SolutionName = FileEntity.Solution.Name,
                //EntityName = FileEntity.Name, // 实体类型名称
                //BaseName = bName
            };

            VelocityContext context = new VelocityContext();
            context.Put("people", "华威");
            context.Put("model", model);

            StringWriter writer = new StringWriter();
            template.Merge(context, writer);
            var result = writer.GetStringBuilder().ToString();

            //2. 内容写入到指定文件

            File.WriteAllText(filePath, result);
        }

        /// <summary>
        /// 领域服务接口 IManager.vm
        /// </summary>
        protected void createIManager()
        {

            // 1. 当前实体的同级目录下创建领域服务文件夹 DomainService
            var fileName = string.Format("I{0}Manager.cs", FileEntity.Name);
            var filePath = Path.Combine(FileEntity.CurrentFile.Directory.FullName, this.DomainServiceName, fileName);

            // 获取模板并且设置参数
            Template template = VelocityEngine.GetTemplate("IManager.vm");

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = string.Format("{0}.{1}", FileEntity.NameSpace, this.DomainServiceName), // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称

            };

            VelocityContext context = new VelocityContext();
            context.Put("people", "华威");
            context.Put("model", model);

            StringWriter writer = new StringWriter();
            template.Merge(context, writer);
            var result = writer.GetStringBuilder().ToString();

            //2. 内容写入到指定文件

            File.WriteAllText(filePath, result);
        }

        /// <summary>
        /// 创建领域服务实现类 Manager.vm
        /// </summary>
        protected void createManager()
        {
            //1. 创建领域实体目录 DomainService 在当前实体目录下创建
            var fileName = string.Format("{0}Manager.cs", FileEntity.Name);
            var filePath = Path.Combine(FileEntity.CurrentFile.Directory.FullName, this.DomainServiceName, fileName);
            // 获取模板并且设置参数
            Template template = VelocityEngine.GetTemplate("Manager.vm");
            
            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = string.Format("{0}.{1}", FileEntity.NameSpace, this.DomainServiceName), // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称

            };

            VelocityContext context = new VelocityContext();
            context.Put("people", "华威");
            context.Put("model", model);

            StringWriter writer = new StringWriter();
            template.Merge(context, writer);
            var result = writer.GetStringBuilder().ToString();

            //2. 内容写入到指定文件

            File.WriteAllText(filePath, result);
        }

        


        /// <summary>
        /// 创建映射
        /// </summary>
        protected void Create_Permissions()
        {
            string path = string.Empty;
            string fileName = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.NameSpace, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                SolutionName = FileEntity.Solution.Name,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                SolutionTxt = FileEntity.SubName

            };

            fileName = string.Format("{0}Permissions.cs", FileEntity.Name);
            path = Path.Combine(FileEntity.CurrentFile.Directory.FullName, this.AuthorizationName, fileName);
            this.CreateFile(path, "Permissions.vm", model);
        }

        /// <summary>
        /// 创建映射
        /// </summary>
        protected void Create_AuthorizationProvider()
        {
            string path = string.Empty;
            string fileName = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.NameSpace, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                SolutionName = FileEntity.Solution.Name,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                SolutionTxt = FileEntity.SubName,
               
            };

            fileName = string.Format("{0}AuthorizationProvider.cs", FileEntity.Name);
            path = Path.Combine(FileEntity.CurrentFile.Directory.FullName, this.AuthorizationName, fileName);
            this.CreateFile(path, "AuthorizationProvider.vm", model);
        }

        /// <summary>
        /// 创建说明文档
        /// </summary>
        private void Create_Readme()
        {
            string path = string.Empty;
            string fileName = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.NameSpace, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                SolutionName = FileEntity.Solution.Name,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                SolutionTxt = FileEntity.SubName,

            };

            fileName = "readme.md";
            path = Path.Combine(FileEntity.CurrentFile.Directory.FullName, fileName);
            this.CreateFile(path, "Readme.vm", model);
        }
    }
}
