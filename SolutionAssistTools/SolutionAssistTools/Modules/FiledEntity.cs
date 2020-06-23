using System;
/*
    解析出来的字段实体
*/
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.AssistTools.Modules
{
    public class FiledEntity
    {
        public FiledEntity() {
            this.TypeName = "long";
            this.UseEditDto = true;
            this.UseListDto = true;
        }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 中文名称
        /// </summary>
        public string CName { get; set; }

        /// <summary>l
        /// 是否必填
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 最小长度
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// 是否使用EditDto
        /// </summary>
        public bool UseEditDto { get; set; }


        /// <summary>
        /// 是否使用ListDto
        /// </summary>
        public bool UseListDto { get; set; }

        
    }
}
