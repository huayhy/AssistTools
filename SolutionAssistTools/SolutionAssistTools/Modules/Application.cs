using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.AssistTools.Modules
{
    public class Application: BaseEntity
    {
        public const string Layer = ".Application";

        /// <summary>
        /// 应用服务名称
        /// </summary>
        public override string Name { get; set; }

        /// <summary>
        /// 应用服务层的实体路径和领域服务层相似
        /// </summary>
        public string DomainPath { get; set; }
    }
}
