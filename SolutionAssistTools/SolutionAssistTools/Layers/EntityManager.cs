using Faker.AssistTools.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.AssistTools.Layers
{
    public class EntityManager: BaseLayerManager,ILayerManager
    {
        // 需要一个变量来配置生成参数
        protected FileEntity FileEntity;

        public EntityManager(FileEntity _FileEntity)
        {
            FileEntity = _FileEntity;
        }

        /// <summary>
        /// 创建接口实现
        /// </summary>
        public void CreateLayer()
        {
            // 只要调用了就会生成一个领域实体目录，请以后吧领域实体都放这里
            this.CreateDirectory();

            this.createFiles();
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
        /// 领域层所需要的目录
        /// </summary>
        protected void CreateDirectory()
        {
            // 创建 DomainEntities 目录
            this.CreateDirectory(this.FileEntity.ProjectCore.BaseDirPath, "DomainEntities");
        }

        /// <summary>
        /// 创建领域服务层需要的文件
        /// </summary>
        protected void createFiles()
        {
            // EntityMapper目录下创建对应实体文件夹在创建配置
            this.Create_Entity();
        }
        /// <summary>
        /// 输出层文件对象
        /// </summary>
        public void OutputLayer()
        {
            throw new NotImplementedException();
        }


        protected void Create_Entity()
        {
            string path = string.Empty;
            string fileName = string.Empty;
            // SOEI.Solution.DomainEntities.Member
            var model = new
            {
                // 选择文件的命名空间 + DomainService
                NameSpace = string.Format("{0}.DomainEntities.{1}", FileEntity.Solution.Name, FileEntity.FullName), // 命名空间
                EntityName = FileEntity.EntityInfo.Name, // 实体类型名称
                List = FileEntity.Fields,
                Inherit = FileEntity.EntityInfo.Inherit
            };

            fileName = string.Format("{0}.cs", FileEntity.EntityInfo.Name);
            path = Path.Combine(this.FileEntity.FullFileName, fileName);
            this.CreateFile(path, "Entity.vm", model);
            // 处理完成之后吧这两个数据赋值回去
            this.FileEntity.FullFileName = path;
            this.FileEntity.CurrentFile = new FileInfo(path);
            this.FileEntity.Name = Path.GetFileNameWithoutExtension(path);  // 选择项名称
            this.FileEntity.FullName = Path.GetFileName(path);
            this.FileEntity.NameSpace = model.NameSpace;
            this.FileEntity.SubName = this.FileEntity.Solution.Name.Substring(this.FileEntity.Solution.Name.LastIndexOf(".") + 1);
            // 获取方案名称
            //var strs = this.Solution.Name.Split('.');
            //if (strs.Length == 2)
            //{
            //    this.CpmpanyName = strs[0];
            //    this.SubName = strs[1];
            //}
            //if (strs.Length == 1)
            //{
            //    this.CpmpanyName = strs[0];
            //    this.SubName = strs[0];
            //}
        }
    }
}
