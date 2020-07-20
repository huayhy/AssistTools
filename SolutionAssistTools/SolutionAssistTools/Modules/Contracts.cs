using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.AssistTools.Modules
{
    public class Contracts : BaseEntity
    {
        public const string Layer = ".Application.Contracts";

        /// <summary>
        ///  是否试用内容层
        /// </summary>
        public bool IsUse = false;
        /// <summary>
        /// 应用服务名称
        /// </summary>
        public override string Name { get; set; }

        /// <summary>
        /// 应用服务层的实体路径和领域服务层相似
        /// </summary>
        public string DomainPath { get; set; }

        /// <summary>
        /// 公司活组织名称 比如 Faker.Solution 的 Faker 部分
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 项目子名称 比如 Faker.Solution 的 Solution 部分
        /// </summary>
        public string SubName { get; set; }

        public string ClassName { get; set; }
    }
}
