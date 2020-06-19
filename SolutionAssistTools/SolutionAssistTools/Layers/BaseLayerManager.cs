using Commons.Collections;
using Faker.AssistTools.Modules;
using NVelocity;
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
    public  class BaseLayerManager
    {
        
        /// <summary>
        /// 当前工作目录
        /// </summary>
        public string WorkFolder = System.IO.Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        /// <summary>
        /// VM魔板目录
        /// </summary>
        public string VMFolder = string.Empty;
        /// <summary>
        ///  模板引擎
        /// </summary>
        public VelocityEngine VelocityEngine;

        public BaseLayerManager()
        {

            

            this.VMFolder = string.Format(@"{0}\\VM\\", WorkFolder); // 默认目录为VM
            VelocityEngine = new VelocityEngine();
            ExtendedProperties props = new ExtendedProperties(); //创建模版使用的扩展属性
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, this.VMFolder);  //设置模板所在的路径
            VelocityEngine.Init(props);
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="_BaseDirPath">基础目录</param>
        /// <param name="_DirName">需要新建的目录路径</param>
        protected  void CreateDirectory(string _BaseDirPath, string _DirName)
        {
            //1. 创建领域实体目录 DomainService 在当前实体目录下创建
            var DomainServiceDir = string.Format("{0}\\{1}", _BaseDirPath, _DirName);
            // 不存在这个目录则创建
            if (!Directory.Exists(DomainServiceDir))
            {
                Directory.CreateDirectory(DomainServiceDir);
            }
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="writeFilePath">文件路径</param>
        /// <param name="vmName">VM模板全名称</param>
        /// <param name="model">需要实体</param>
        /// <param name="isWrite">是否写入文件默认为写入</param>
        /// <returns>写入的字符串</returns>
        protected virtual string CreateFile(string writeFilePath,string vmName, object model ,bool isWrite = true) 
        {
           
            // 获取模板并且设置参数
            Template template = VelocityEngine.GetTemplate(vmName);
            VelocityContext context = new VelocityContext();
            context.Put("people", "华威");
            context.Put("model", model);

            StringWriter writer = new StringWriter();
            template.Merge(context, writer);
            var result = writer.GetStringBuilder().ToString();
            //2. 内容写入到指定文件
            if (isWrite)
            {
                // 文件如果不存在
                if (!File.Exists(writeFilePath))
                {
                    File.WriteAllText(writeFilePath, result);
                }
                else if(APP.Configuration.IsCover) // 同意重写才会重写
                {
                    File.WriteAllText(writeFilePath, result);
                }
                
            }
            return result;
        }
    }
}
