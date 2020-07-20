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

        ILayerManager ContractsManager = null; // 这个Layer 已经处理了生成额外的还是非额外的应用服务

        public EntityListWpf()
        {
            InitializeComponent();
        }

        public EntityListWpf(FileEntity _FileEntity)
        {
            InitializeComponent();

            FileEntity = _FileEntity;

            this.DoLoadData();

            // 新模式创建Manager
            this.ContractsManager = new ContractsManager(this.FileEntity);

            this.btnNewSubmit.IsEnabled = this.FileEntity.Contracts.IsUse;
            this.btnSubmit.IsEnabled = !this.FileEntity.Contracts.IsUse;
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

        /// <summary>
        /// 标准模式创建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            
            // 判断是否生成额外的应用服务
            APP.Configuration.ExtraName = this.txtExtra.Text.Trim();

            //1. 处理领域服务
            ILayerManager lm1 = new DomainLayerManager(this.FileEntity);        
            ILayerManager lm2 = new FrameworkCoreManager(this.FileEntity);

            ILayerManager lm3 = new ApplicationLayerManager(this.FileEntity);
            ILayerManager lm4 = new ExtraApplicationLayerManager(this.FileEntity);

            //ILayerManager lm5 = new ContractsManager(this.FileEntity); // 这个Layer 已经处理了生成额外的还是非额外的应用服务

            // 处理基础结构（首次访问）
            if (APP.Configuration.IsFirst)
            {
                // 创建基础结构，目录结构，常量文件等
                lm1.CreateBaseLayer();
                lm2.CreateBaseLayer();
                lm3.CreateBaseLayer();
                lm4.CreateBaseLayer();

                //lm5.CreateBaseLayer();
            }
            //2. 基础设施
            lm1.CreateLayer();
            lm2.CreateLayer();
            // 的模式，这里判断了是否试用扩展额外的应用服务
            if (APP.Configuration.UseExtraApplication)
            {
                // 处理应用服务
                lm4.CreateLayer();
            }
            else
            {
                // 处理应用服务
                lm3.CreateLayer();
            }


            MessageBox.Show("生成操作完成！","注意", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        /// <summary>
        /// 新模式创建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewSubmit_Click(object sender, RoutedEventArgs e)
        {
            // 判断是否生成额外的应用服务
            APP.Configuration.ExtraName = this.txtExtra.Text.Trim();

            //1. 处理领域服务
            ILayerManager lm1 = new DomainLayerManager(this.FileEntity);
            ILayerManager lm2 = new FrameworkCoreManager(this.FileEntity);
            ILayerManager lm3 = new ApplicationLayerManager(this.FileEntity);
            ILayerManager lm4 = new ExtraApplicationLayerManager(this.FileEntity);
            //ILayerManager lm5 = new ContractsManager(this.FileEntity); // 这个Layer 已经处理了生成额外的还是非额外的应用服务

            // 处理基础结构（首次访问）
            if (APP.Configuration.IsFirst)
            {
                // 创建基础结构，目录结构，常量文件等
                lm1.CreateBaseLayer();
                lm2.CreateBaseLayer();
                //lm3.CreateBaseLayer();
                //lm4.CreateBaseLayer();
                this.ContractsManager.CreateBaseLayer();
            }
            // 创建领域层
            lm1.CreateLayer();
            // 创建基础设施层
            lm2.CreateLayer();
            // 创建应用服务
            this.ContractsManager.CreateLayer();

            MessageBox.Show("生成操作完成！", "注意", MessageBoxButton.OK, MessageBoxImage.Information);
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
