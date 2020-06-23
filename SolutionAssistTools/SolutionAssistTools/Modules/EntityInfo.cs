/*
 * 如果要追加实体对象
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.AssistTools.Modules
{
    public class EntityInfo
    {
        /// <summary>
        /// 实体名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 继承类名称
        /// </summary>
        public string Inherit { get; set; }
    }
}
