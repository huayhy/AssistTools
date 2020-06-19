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
    /// AssistToolsWpf.xaml 的交互逻辑
    /// </summary>
    public partial class AssistToolsWpf : Window
    {
        public string WorkFolder = System.IO.Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);

        public FileEntity FileEntity { get; set; }

        public AssistToolsWpf()
        {
            InitializeComponent();
        }

        public AssistToolsWpf(FileEntity _FileEntity)
        {
            InitializeComponent();
            this.FileEntity = _FileEntity;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateInstance();
            this.entityTextBlock.Text = this.FileEntity.Name;
            //this.entityConfigurationBindingSource.DataSource = APP.Configuration;
            



        }

        /// <summary>
        /// 创建实例（）
        /// </summary>
        private void CreateInstance()
        {
            // 窗体每次打开都会有个新的配置
            APP.Configuration = new EntityConfiguration();

            this.IsCover.DataContext = APP.Configuration;
            this.IsFirst.DataContext = APP.Configuration;
            this.UseApplication.DataContext = APP.Configuration;
            this.UseAuthorization.DataContext = APP.Configuration;
            this.UseDomainService.DataContext = APP.Configuration;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;

            EntityListWpf wpf = new EntityListWpf(this.FileEntity);
            var result = wpf.ShowDialog();

            if (result.Value) {
                this.Visibility = Visibility.Visible;
            }
        }

        private void butCancal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
