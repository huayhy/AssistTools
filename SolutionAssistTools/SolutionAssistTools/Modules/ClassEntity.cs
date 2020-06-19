using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.AssistTools.Modules
{
    public class ClassEntity
    {
        /// <summary>
        /// 继承名称
        /// </summary>
        public string Inherit { get; set; }

        /// <summary>
        /// 获取Dto的继承关系
        /// </summary>
        public string InheritDto {
            get {
                if (string.IsNullOrEmpty(this.Inherit))
                    return string.Empty;

                return ": " + this.Inherit.Trim().Replace("Entity", "EntityDto");
            }
        
        }

    }
}
