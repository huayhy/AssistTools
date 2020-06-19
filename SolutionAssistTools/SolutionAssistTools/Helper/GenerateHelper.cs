/*
    负责生成功能处理函数
 */
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.AssistTools.Helper
{
    public class GenerateHelper
    {
        public string WorkFolder = System.IO.Directory.GetCurrentDirectory();

        public string VMFolder = string.Empty;

        protected VelocityEngine VelocityEngine;
        

        public GenerateHelper() 
        {
            this.VMFolder = string.Format(@"{0}\VM", WorkFolder);
            VelocityEngine = new VelocityEngine();
            ExtendedProperties props = new ExtendedProperties(); //创建模版使用的扩展属性
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, this.VMFolder);  //设置模板所在的路径
            VelocityEngine.Init(props);
        }


        public string WriteEditDto() 
        {
            Template template = VelocityEngine.GetTemplate("EditDto.vm");
            VelocityContext context = new VelocityContext();
            context.Put("people", "华威");

            StringWriter writer = new StringWriter();
            template.Merge(context, writer);
            return writer.GetStringBuilder().ToString(); 
        }
    }
}
