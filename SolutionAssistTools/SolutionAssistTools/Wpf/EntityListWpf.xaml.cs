using Faker.AssistTools.Layers;
using Faker.AssistTools.Modules;
using System;
using System.Collections.Generic;
using System.IO;
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

        public Dictionary<int, string> ApplicationListSource = new Dictionary<int, string>();

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
            this.UseExtraApplication.DataContext = APP.Configuration;
            this.ListmuLu.ItemsSource = FileEntity.Fields;
            // 找到Srev目录下所有的服务项目
            DirectoryInfo dir = new DirectoryInfo(FileEntity.ServDir);
            if (dir.Exists) {
                DirectoryInfo[] dirs = dir.GetDirectories();
                int i = 1;
                foreach(var d in dir.GetDirectories()) {
                    this.ApplicationListSource.Add(i, d.Name);
                    i++;
                }
                this.txtExtra.ItemsSource = this.ApplicationListSource;
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // 判断是否生成额外的应用服务
            APP.Configuration.ExtraName = this.txtExtra.Text.Trim();

            //1. 处理领域服务
            ILayerManager lm1 = new DomainLayerManager(this.FileEntity);
            //2. 基础设施
            lm1.CreateLayer();
            ILayerManager lm2 = new FrameworkCoreManager(this.FileEntity);
            lm2.CreateLayer();
            if (APP.Configuration.UseExtraApplication)
            {
                ILayerManager lm4 = new ExtraApplicationLayerManager(this.FileEntity);
                lm4.CreateLayer();
            }
            else {
                ILayerManager lm3 = new ApplicationLayerManager(this.FileEntity);
                lm3.CreateLayer();
            }
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

        private void UseExtraApplication_Checked(object sender, RoutedEventArgs e)
        {
            this.txtExtra.IsReadOnly = !APP.Configuration.UseExtraApplication;
        }

        private void UseExtraApplication_Click(object sender, RoutedEventArgs e)
        {
            this.txtExtra.IsEnabled = APP.Configuration.UseExtraApplication;
            if (!this.txtExtra.IsEnabled) {
                this.txtExtra.Text = string.Empty;
            }
        }
    }
}
