using Faker.AssistTools.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.AssistTools.Layers
{
    public class ApplicationLayerManager: BaseLayerManager, ILayerManager
    {
        // 需要一个变量来配置生成参数
        protected FileEntity FileEntity;

        public ApplicationLayerManager(FileEntity _FileEntity)
        {
            FileEntity = _FileEntity;
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
            // 创建需要的基础对象
            //this.Create_Base_Files();
        }

        /// <summary>
        /// 输出层文件对象
        /// </summary>
        public void OutputLayer()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 应用层所需要的目录
        /// </summary>
        protected void CreateDirectory()
        {
            // 创建Dtos目录
            this.CreateDirectory(this.FileEntity.Application.BaseDirPath, "Dtos");
            // 创建实体对应Dto目录
            this.CreateDirectory(this.FileEntity.Application.DomainPath, "Dtos");
            //  创建 Mapper 目录
            this.CreateDirectory(this.FileEntity.Application.DomainPath, "Mapper");
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CreateFiles()
        {
            // 创建通用的DTO
            this.Create_Dtos();
            // 创建实体Dto
            this.Create_EntityDtos();
            // 创建AutoMapper映射
            this.Create_AutoMapper();
            // 创建接口实现类
            this.Create_EntityObject();
            // 创建说明文件
            this.Create_Readme();
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

        /// <summary>
        /// 创建公共Dto文件
        /// </summary>
        protected void Create_Dtos() 
        {
            string path = string.Empty;

            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = FileEntity.Solution.Name, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                SolutionName = string.Empty
            };

            path = Path.Combine(this.FileEntity.Application.BaseDirPath, "Dtos", "PagedAndFilteredInputDto.cs");
            this.CreateLayerFile(path, "PagedAndFilteredInputDto.vm", model);
            path = Path.Combine(this.FileEntity.Application.BaseDirPath, "Dtos", "PagedAndSortedInputDto.cs");
            this.CreateLayerFile(path, "PagedAndSortedInputDto.vm", model);
            path = Path.Combine(this.FileEntity.Application.BaseDirPath, "Dtos", "PagedInputDto.cs");
            this.CreateLayerFile(path, "PagedInputDto.vm", model);
            path = Path.Combine(this.FileEntity.Application.BaseDirPath, "Dtos", "PagedSortedAndFilteredInputDto.cs");
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
                NameSpace =  FileEntity.NameSpace, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                InheritType = FileEntity.ClassEntity.InheritType,
                SolutionName = FileEntity.Solution.Name,
            };

            fileName = string.Format("{0}ListDto.cs", FileEntity.Name);
            path = Path.Combine(this.FileEntity.Application.DomainPath, "Dtos", fileName);
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
            path = Path.Combine(this.FileEntity.Application.DomainPath, "Dtos", fileName);
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
                InheritDto = FileEntity.ClassEntity.InheritDto,
                InheritType = FileEntity.ClassEntity.InheritType,
            };

            fileName = string.Format("Get{0}sInput.cs", FileEntity.Name);
            path = Path.Combine(this.FileEntity.Application.DomainPath, "Dtos", fileName);
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
                InheritType = FileEntity.ClassEntity.InheritType,

            };
            
            fileName = string.Format("{0}DtoAutoMapper.cs", FileEntity.Name);
            path = Path.Combine(this.FileEntity.Application.DomainPath, "Mapper", fileName);
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
                InheritDto = FileEntity.ClassEntity.InheritDto,
                InheritType = FileEntity.ClassEntity.InheritType,
            };

            fileName = string.Format("CreateOrUpdate{0}Input.cs", FileEntity.Name);
            path = Path.Combine(this.FileEntity.Application.DomainPath, "Dtos", fileName);
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
                InheritDto = FileEntity.ClassEntity.InheritDto,
                InheritType = FileEntity.ClassEntity.InheritType,
            };

            fileName = string.Format("Get{0}ForEditOutput.cs", FileEntity.Name);
            path = Path.Combine(this.FileEntity.Application.DomainPath, "Dtos", fileName);
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
            path = Path.Combine(this.FileEntity.Application.DomainPath, fileName);
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

            };

            fileName = string.Format("{0}AppService.cs", FileEntity.Name);
            path = Path.Combine(this.FileEntity.Application.DomainPath, fileName);
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
                NameSpace = FileEntity.NameSpace, // 命名空间
                EntityName = FileEntity.Name, // 实体类型名称
                List = FileEntity.Fields,
                SolutionName = FileEntity.Solution.Name,
                InheritDto = FileEntity.ClassEntity.InheritDto,
                SolutionTxt = FileEntity.SubName,
                InheritType = FileEntity.ClassEntity.InheritType,
            };

            fileName = "readme.md";
            path = Path.Combine(this.FileEntity.Application.DomainPath, fileName);
            this.CreateFile(path, "Readme.vm", model);
        }
    }
}
