using Commons.Collections;
using Faker.AssistTools.Helper;
using Faker.AssistTools.Modules;
using NVelocity.App;
using NVelocity.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.AssistTools.Layers
{
    public class ContractsManager : BaseLayerManager, ILayerManager
    {
        // 需要一个变量来配置生成参数
        protected FileEntity FileEntity;

        public ContractsManager(FileEntity _FileEntity)
        {
            this.VMFolder = string.Format(@"{0}\\VM\\Contracts\\", WorkFolder); // 默认目录为VM
            VelocityEngine = new VelocityEngine();
            ExtendedProperties props = new ExtendedProperties(); //创建模版使用的扩展属性
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, this.VMFolder);  //设置模板所在的路径
            VelocityEngine.Init(props);

            FileEntity = _FileEntity;
            FileEntity.Contracts.DomainPath = FileEntity.CurrentFile.DirectoryName.Replace(FileEntity.ProjectCore.Name, FileEntity.Contracts.Name);
            //// 额外的应用服务层数据(如果勾选了生成额外的应用服务)
            //var dirExtraApplication = AssistHelper.GetExtraLayerDir(FileEntity.ServDir, APP.Configuration.ExtraName);
            //FileEntity.ExtraApplication.Name = dirExtraApplication?.Name;
            //FileEntity.ExtraApplication.BaseDirPath = dirExtraApplication?.FullName;

            //string oldval = string.Format("src\\{0}", FileEntity.ProjectCore.Name);
            //string newval = string.Format("serv\\{0}", FileEntity.ExtraApplication.Name);
            //FileEntity.ExtraApplication.DomainPath = FileEntity.CurrentFile.DirectoryName.Replace(oldval, newval);

            this.FileEntity.Contracts.ClassName = "ApplicationContracts";
            // 获取方案名称
            //var strs = FileEntity.Contracts.Name.Split('.');
            //if (strs.Length == 3)
            //{
            //    this.FileEntity.Contracts.CompanyName = strs[0];
            //    this.FileEntity.Contracts.SubName = strs[1];
            //    this.FileEntity.Contracts.ClassName = string.Format("{0}{1}", strs[1], strs[2]);
            //}
            //if (strs.Length == 2)
            //{
            //    this.FileEntity.Contracts.CompanyName = strs[0];
            //    this.FileEntity.Contracts.SubName = strs[0];
            //    this.FileEntity.Contracts.ClassName = string.Format("{0}", strs[1]);
            //}
        }

        /// <summary>
        /// 创建接口实现
        /// </summary>
        public void CreateLayer()
        {
            // 这里需要给领域服务层创建所需要的文件目录和数据准备工作
            this.CreateDirectory();
            // 创建应用服务所需要的文件
            if (APP.Configuration.UseApplication)
            {
                this.CreateFiles();
            }
        }

        /// <summary>
        /// 创建基础层
        /// </summary>
        public void CreateBaseLayer()
        {
            // 先创建对应的目录
            this.CreateDirectory();
        }

        /// <summary>
        /// 输出层文件对象
        /// </summary>
        public  void OutputLayer()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 应用层所需要的目录
        /// </summary>
        protected void CreateDirectory()
        {
            // 创建Dtos目录
            this.CreateDirectory(this.FileEntity.Contracts.BaseDirPath, "Dtos");
            // 创建实体对应Dto目录
            this.CreateDirectory(this.FileEntity.Contracts.DomainPath, "Dtos");
            // 创建 Mapper 目录
            this.CreateDirectory(this.FileEntity.Contracts.DomainPath, "Mapper");
            // 创建 Application 目录
            if (APP.Configuration.UseExtraApplication)
            {
                this.CreateDirectory(this.FileEntity.ExtraApplication.DomainPath);
            }
            else
            {
                this.CreateDirectory(this.FileEntity.Application.DomainPath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CreateFiles()
        {
            // 创建需要的基础对象
            this.Create_Base();
            // 创建通用的DTO
            this.Create_Dtos();
            //// 创建实体Dto
            this.Create_EntityDtos();
            //// 创建AutoMapper映射
            this.Create_AutoMapper();
            //// 创建接口实现类
            this.Create_EntityObject();
            //// 创建说明文件
            this.Create_Readme();
            // 创建合约层说明文件
            this.Create_ContractsReadme();

            if (APP.Configuration.UseExtraApplication)
            {
                this.Create_ExtraApplicationReadme();
            }
        }

        protected void Create_Base()
        {
            this.Create_Module();

            if (APP.Configuration.UseExtraApplication)
            {
                // 是否创建的新的应用服务
                this.Create_ExtraApplicationModule();

                this.Create_ExtraServiceBase();
            }
        }

        /// <summary>
        /// 创建实体DTOS
        /// </summary>
        public void Create_EntityDtos() 
        {
            // ListDto
            this.Create_Entity_ListDtos();
            // EditDto
            this.Create_Entity_EditDtos();
            // GetEntityInput
            this.Create_GetEntityInput();
            // CreateOrUpdateEntityInput
            this.Create_CreateOrUpdateEntityInput();
            // Create_GetEntityForEditOutput
            this.Create_GetEntityForEditOutput();
        }

        /// <summary>
        ///  创建实体对象接口和实现类
        /// </summary>
        protected void Create_EntityObject()
        {
            // 创建实体接口
            this.Create_EntityObject_Interface();
            // 接口实现类
            this.Create_EntityObject_Implement();
        }

        protected void Create_ExtraApplicationModule()
        {
            string path = string.Empty;
            string fileName = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.ExtraApplication.Name, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                SolutionTxt = FileEntity.SubName,
                ProjectName = FileEntity.ExtraApplication.Name,
                SolutionName = FileEntity.Solution.Name,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                InheritType = FileEntity.ClassEntity.InheritType,
                ClassName = FileEntity.ExtraApplication.ClassName,
                CoreClassName = FileEntity.ProjectCore.ClassName
            };

            fileName = string.Format("{0}Module.cs", FileEntity.ExtraApplication.ClassName);
            path = Path.Combine(this.FileEntity.ExtraApplication.BaseDirPath, fileName);
            this.CreateLayerFile(path, "ExtraApplicationModule.vm", model);
        }

        protected void Create_ExtraServiceBase()
        {
            string path = string.Empty;
            string fileName = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.ExtraApplication.Name, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                SolutionTxt = FileEntity.SubName,
                ProjectName = FileEntity.ExtraApplication.Name,
                SolutionName = FileEntity.Solution.Name,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                InheritType = FileEntity.ClassEntity.InheritType,
                ClassName = FileEntity.ExtraApplication.ClassName,
                CoreClassName = FileEntity.ProjectCore.ClassName
            };

            fileName = string.Format("{0}ServiceBase.cs", FileEntity.ExtraApplication.ClassName);
            path = Path.Combine(this.FileEntity.ExtraApplication.BaseDirPath, fileName);
            this.CreateLayerFile(path, "ExtraServiceBase.vm", model);
        }

        protected void Create_Module()
        {
            string path = string.Empty;
            string fileName  = string.Empty;
            
            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.Contracts.Name, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                SolutionTxt = FileEntity.SubName,
                ProjectName = FileEntity.Contracts.Name,
                SolutionName = FileEntity.Solution.Name,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                InheritType = FileEntity.ClassEntity.InheritType,
                ClassName = FileEntity.Contracts.ClassName,
                CoreClassName = FileEntity.ProjectCore.ClassName
            };

            fileName = string.Format("{0}Module.cs", FileEntity.Contracts.ClassName);
            path = Path.Combine(this.FileEntity.Contracts.BaseDirPath, fileName);
            this.CreateLayerFile(path, "ContractsApplicationModule.vm", model);
        }

        /// <summary>
        /// 创建公共Dto文件(把这个归属到首次应用)
        /// </summary>
        protected void Create_Dtos() 
        {
            string path = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace  = FileEntity.Contracts.Name, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                SolutionName = FileEntity.Solution.Name,
            };

            path = Path.Combine(this.FileEntity.Contracts.BaseDirPath, "Dtos", "PagedAndFilteredInputDto.cs");
            this.CreateLayerFile(path, "PagedAndFilteredInputDto.vm", model);
            path = Path.Combine(this.FileEntity.Contracts.BaseDirPath, "Dtos", "PagedAndSortedInputDto.cs");
            this.CreateLayerFile(path, "PagedAndSortedInputDto.vm", model);
            path = Path.Combine(this.FileEntity.Contracts.BaseDirPath, "Dtos", "PagedInputDto.cs");
            this.CreateLayerFile(path, "PagedInputDto.vm", model);
            path = Path.Combine(this.FileEntity.Contracts.BaseDirPath, "Dtos", "PagedSortedAndFilteredInputDto.cs");
            this.CreateLayerFile(path, "PagedSortedAndFilteredInputDto.vm", model);
        }

        /// <summary>
        /// 创建实体ListDto
        /// </summary>
        protected void Create_Entity_ListDtos() 
        {
            string path = string.Empty;
            string fileName = string.Empty;
            
            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.NameSpace, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                InheritType = FileEntity.ClassEntity.InheritType
            };

            fileName = string.Format("{0}ListDto.cs", FileEntity.Name);
            path = Path.Combine(this.FileEntity.Contracts.DomainPath, "Dtos", fileName);
            this.CreateFile(path, "ListDto.vm", model);
        }

        /// <summary>
        /// 创建实体EditDto
        /// </summary>
        protected void Create_Entity_EditDtos()
        {
            string path = string.Empty;
            string fileName = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.NameSpace, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                InheritType = FileEntity.ClassEntity.InheritType,
                SolutionName = FileEntity.Solution.Name,
            };

            fileName = string.Format("{0}EditDto.cs", FileEntity.Name);
            path = Path.Combine(this.FileEntity.Contracts.DomainPath, "Dtos", fileName);
            this.CreateFile(path, "EditDto.vm", model);
        }


        /// <summary>
        /// 创建GetEntityInput Dto
        /// </summary>
        protected void Create_GetEntityInput()
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
                InheritDto = FileEntity.ClassEntity.InheritDto
            };

            fileName = string.Format("Get{0}sInput.cs", FileEntity.Name);
            path = Path.Combine(this.FileEntity.Contracts.DomainPath, "Dtos", fileName);
            this.CreateFile(path, "GetEntityInput.vm", model);
        }


        /// <summary>
        /// 创建映射
        /// </summary>
        protected void Create_AutoMapper()
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
                ProjectName = FileEntity.Contracts.Name

            };
            
            fileName = string.Format("{0}DtoAutoMapper.cs", FileEntity.Name);
            path = Path.Combine(this.FileEntity.Contracts.DomainPath, "Mapper", fileName);
            this.CreateLayerFile(path, "DtoAutoMapper.vm", model);
        }

        /// <summary>
        /// 创建和更新Dto
        /// </summary>
        protected void Create_CreateOrUpdateEntityInput()
        {
            string path = string.Empty;
            string fileName = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.NameSpace, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                InheritDto = FileEntity.ClassEntity.InheritDto
            };

            fileName = string.Format("CreateOrUpdate{0}Input.cs", FileEntity.Name);
            path = Path.Combine(this.FileEntity.Contracts.DomainPath, "Dtos", fileName);
            this.CreateFile(path, "CreateOrUpdateEntityInput.vm", model);
        }

        /// <summary>
        /// GetEntityForEditOutput 
        /// </summary>
        protected void Create_GetEntityForEditOutput()
        {
            string path = string.Empty;
            string fileName = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.NameSpace, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                InheritDto = FileEntity.ClassEntity.InheritDto
            };

            fileName = string.Format("Get{0}ForEditOutput.cs", FileEntity.Name);
            path = Path.Combine(this.FileEntity.Contracts.DomainPath, "Dtos", fileName);
            this.CreateFile(path, "GetEntityForEditOutput.vm", model);
        }

        /// <summary>
        /// 创建应用层接口
        /// </summary>
        protected void Create_EntityObject_Interface()
        {
            string path = string.Empty;
            string fileName = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.NameSpace, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                SolutionName = FileEntity.Solution.Name,
                InheritType = FileEntity.ClassEntity.InheritType,
            };

            fileName = string.Format("I{0}AppService.cs", FileEntity.Name);
            path = Path.Combine(this.FileEntity.Contracts.DomainPath, fileName);
            this.CreateFile(path, "IEntityAppService.vm", model);
        }

        /// <summary>
        /// 创建应用层接口实现类
        /// </summary>
        protected void Create_EntityObject_Implement()
        {
            string path = string.Empty;
            string fileName = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.NameSpace, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                SolutionName = FileEntity.Solution.Name,
                SolutionTxt = FileEntity.SubName,
                InheritType = FileEntity.ClassEntity.InheritType,
                ProjectName = APP.Configuration.UseExtraApplication?FileEntity.ExtraApplication.Name: FileEntity.Application.Name,
                ClassName = APP.Configuration.UseExtraApplication ? FileEntity.ExtraApplication.ClassName : FileEntity.SubName+"App",
            };

            fileName = string.Format("{0}AppService.cs", FileEntity.Name);
            // 最终实现类放在哪里根据是否扩展判断
            // 如果有额外的层
            if (APP.Configuration.UseExtraApplication)
            {
                path = Path.Combine(this.FileEntity.ExtraApplication.DomainPath, fileName);
            }
            else {
                path = Path.Combine(this.FileEntity.Application.DomainPath, fileName);
            }
            
            this.CreateFile(path, "EntityAppService.vm", model);
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
                NameSpace = FileEntity.ExtraApplication.Name, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                SolutionName = FileEntity.Solution.Name,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                SolutionTxt = FileEntity.SubName,

            };

            fileName = "readme.md";
            
            if (APP.Configuration.UseExtraApplication)
            {
                path = Path.Combine(this.FileEntity.ExtraApplication.DomainPath, fileName);
            }
            else
            {
                path = Path.Combine(this.FileEntity.Application.DomainPath, fileName);
            }
            this.CreateFile(path, "Readme.vm", model);
        }

        /// <summary>
        /// 创建内容项目说明文件(只创建一次)
        /// </summary>
        protected void Create_ContractsReadme()
        {
            string path = string.Empty;
            string fileName = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.ExtraApplication.Name, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                SolutionName = FileEntity.Solution.Name,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                SolutionTxt = FileEntity.SubName,
                ClassName = FileEntity.Contracts.ClassName,
                CoreClassName = FileEntity.ProjectCore.ClassName,
                ProjectName = FileEntity.Contracts.Name,

            };

            fileName = "readme.md";
            path = Path.Combine(this.FileEntity.Contracts.BaseDirPath, fileName);
            this.CreateLayerFile(path, "ReadmeContracts.vm", model);
            
        }

        /// <summary>
        /// 创建内容项目说明文件(只创建一次)
        /// </summary>
        protected void Create_ExtraApplicationReadme()
        {
            string path = string.Empty;
            string fileName = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.ExtraApplication.Name, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                SolutionName = FileEntity.Solution.Name,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                SolutionTxt = FileEntity.SubName,
                ClassName = FileEntity.ExtraApplication.ClassName,
                CoreClassName = FileEntity.ProjectCore.ClassName,
                ProjectName = FileEntity.ExtraApplication.Name,

            };

            fileName = "readme.md";
            path = Path.Combine(this.FileEntity.ExtraApplication.BaseDirPath, fileName);
            this.CreateLayerFile(path, "ReadmeExtraApplication.vm", model);

        }
    }
}
