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
    public class FrameworkCoreManager: BaseLayerManager, ILayerManager
    {
        // 需要一个变量来配置生成参数
        protected FileEntity FileEntity;

        public FrameworkCoreManager(FileEntity _FileEntity)
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
            // 当前选中的基础设置需要一个文件 EntityMapper 文件
            this.createFiles();
        }

        /// <summary>
        /// 创建基础层
        /// </summary>
        public void CreateBaseLayer()
        {
            // 先创建对应的目录
            this.CreateDirectory();
            // 创建需要的基础对象
            this.Create_Base_Files();
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
        protected void CreateDirectory()
        {
            // 创建EntityMapper目录
            this.CreateDirectory(this.FileEntity.FrameworkCore.BaseDirPath, "EntityMapper");

        }

        /// <summary>
        /// 创建领域服务层需要的文件
        /// </summary>
        protected void createFiles()
        {
            // EntityMapper目录下创建对应实体文件夹在创建配置
            this.Create_Entity_Mapper();
        }

        /// <summary>
        /// 基础架构 需要进行首次判断
        /// </summary>
        protected void Create_Base_Files()
        {
            
        }

        /// <summary>
        /// 基础架构 需要进行首次判断
        /// </summary>
        protected void Create_Entity_Mapper()
        {
            var dirName = "EntityMapper";
            var strs = FileEntity.Solution.Name.Split('.');
            var bName = string.Empty;
            if (strs.Length > 1)
            {
                bName = strs[1];
            }
            else
            {
                bName = strs[0];
            }
            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = string.Format("{0}.{1}", FileEntity.FrameworkCore.Name, dirName), // 命名空间
                EntityNameSpace = FileEntity.NameSpace,
                SolutionName = FileEntity.Solution.Name,
                EntityName = FileEntity.Name, // 实体类型名称
                BaseName = bName
            };

            var fileName = string.Format("{0}Mapper.cs", FileEntity.Name);
            var path = Path.Combine(this.FileEntity.FrameworkCore.BaseDirPath, dirName, fileName);
            this.CreateFile(path, "EntityMapper.vm", model);
        }
    }
}
