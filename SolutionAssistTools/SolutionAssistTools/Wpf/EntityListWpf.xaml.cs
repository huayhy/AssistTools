using Faker.AssistTools.Layers;
using Faker.AssistTools.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Faker.AssistTools.Wpf
{
    /// <summary>
    /// EntityListWpf.xaml 的交互逻辑
    /// </summary>
    public partial class EntityListWpf : Window
    {
        public FileEntity FileEntity { get; set; }

        public EntityListWpf()
        {
            InitializeComponent();
        }

        public EntityListWpf(FileEntity _FileEntity)
        {
            InitializeComponent();

            FileEntity = _FileEntity;

            this.DoLoadData();
        }

        private void DoLoadData()
        {
            
            this.ListmuLu.ItemsSource = FileEntity.Fields;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            ////1. 处理领域服务
            ILayerManager lm1 = new DomainLayerManager(this.FileEntity);
            //2. 基础设施
            lm1.CreateLayer();
            ILayerManager lm2 = new FrameworkCoreManager(this.FileEntity);
            lm2.CreateLayer();
            ILayerManager lm3 = new ApplicationLayerManager(this.FileEntity);
            lm3.CreateLayer();
            MessageBox.Show("生成操作完成！","注意", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void butPre_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void butCancal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
