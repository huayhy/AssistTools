using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.AssistTools.Modules
{
    public class ProjectCore : BaseEntity
    {
        public const string Layer = ".Core";

        /// <summary>
        /// 基础设施名称
        /// </summary>
        public override string Name { get; set; }
    }
}
