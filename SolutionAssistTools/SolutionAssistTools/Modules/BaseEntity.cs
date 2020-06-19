using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.AssistTools.Modules
{
    public class BaseEntity
    {
        /// <summary>
        /// 项目的名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 当前项目层的基础目录
        /// </summary>
        public virtual string BaseDirPath { get; set; }

    }
}
