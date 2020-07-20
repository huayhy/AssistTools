/*
    层创建的接口
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.AssistTools.Layers
{
    public interface ILayerManager
    {
        /// <summary>
        /// 创建层
        /// </summary>
        void CreateLayer();

        /// <summary>
        /// 创建层
        /// </summary>
        void CreateBaseLayer();

        /// <summary>
        /// 输出层目录
        /// </summary>
        void OutputLayer();
    }
}
