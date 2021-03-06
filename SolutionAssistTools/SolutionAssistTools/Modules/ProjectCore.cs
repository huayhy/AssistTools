﻿using System;
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
