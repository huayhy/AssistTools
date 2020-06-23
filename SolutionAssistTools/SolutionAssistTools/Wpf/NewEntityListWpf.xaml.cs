using Faker.AssistTools.Layers;
using Faker.AssistTools.Modules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class NewEntityListWpf : Window
    {
        public FileEntity FileEntity { get; set; }

        public ObservableCollection<FiledEntity> ListEntity = new ObservableCollection<FiledEntity>();

        public Dictionary<int, string> ListSource = new Dictionary<int, string>();

        public Dictionary<int, string> ClassListSource = new Dictionary<int, string>();

        public NewEntityListWpf()
        {
            InitializeComponent();
        }

        public NewEntityListWpf(FileEntity _FileEntity)
        {
            InitializeComponent();

            FileEntity = _FileEntity;

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateInstance();
        }

        private void CreateInstance()
        {
            // 窗体每次打开都会有个新的配置
            APP.Configuration = new EntityConfiguration();

            this.IsCover.DataContext = APP.Configuration;
            this.IsFirst.DataContext = APP.Configuration;
            this.UseApplication.DataContext = APP.Configuration;
            this.UseAuthorization.DataContext = APP.Configuration;
            this.UseDomainService.DataContext = APP.Configuration;

            this.DoLoadData();
        }

        private void DoLoadData()
        {

            ListSource.Add(1, "int");
            ListSource.Add(2, "long");
            ListSource.Add(3, "decimal");
            ListSource.Add(4, "string");
            ListSource.Add(5, "bool");
            ListSource.Add(7, "DateTime");
            
            ClassListSource.Add(1, "AuditedEntity<long>"); // Entity<long>
            ClassListSource.Add(2, "Entity<long>");

            this.cbxInherit.ItemsSource = ClassListSource;
            this.cbxInherit.SelectedIndex = 0;

            this.textBox.Text           = this.FileEntity.ProjectCore.Name;
            this.txtFullPath.Text       = this.FileEntity.FullFileName;
            this.ListmuLu.ItemsSource   = this.ListEntity;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // 在当前目录下生成实体文件
            if (string.IsNullOrEmpty(this.txtEntityName.Text))
            {
                MessageBox.Show("请输入实体名称", "注意", MessageBoxButton.OK, MessageBoxImage.Information);
                this.txtEntityName.Focus();
                return;
            }
            // 创建新的实体数据
            this.FileEntity.EntityInfo = new EntityInfo();
 
            // 准备生成实体
            this.FileEntity.EntityInfo.Name    = this.txtEntityName.Text;
            this.FileEntity.EntityInfo.Inherit = this.cbxInherit.Text;
            // 设置实体生成数据
            this.FileEntity.Fields = this.ListEntity.ToList();
            // 生成实体文件
            ILayerManager lm = new EntityManager(this.FileEntity);
            lm.CreateLayer();


            // 判断实体

            // 新生成的实体已经完成后这里直接生成其他服务

            // 全局配置项

            // 打开

            ////1. 处理领域服务
            ILayerManager lm1 = new DomainLayerManager(this.FileEntity);
            //2. 基础设施5
            lm1.CreateLayer();
            ILayerManager lm2 = new FrameworkCoreManager(this.FileEntity);
            lm2.CreateLayer();
            ILayerManager lm3 = new ApplicationLayerManager(this.FileEntity);
            lm3.CreateLayer();
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
        /// <summary>
        /// 新增实体字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddFiled_Click(object sender, RoutedEventArgs e)
        {
            ListEntity.Add(new FiledEntity());
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbxInstance = sender as ComboBox;
            ComboBoxItem item = cbxInstance.SelectedItem as ComboBoxItem;
            cbxInstance.Text = item.Content.ToString();
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            // ... Get the ComboBox reference.
            var cbImportance = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            cbImportance.ItemsSource = ListSource;

            // ... Make the first item selected.
            //cbImportance.SelectedIndex = 0;
        }

        /// <summary>
        ///  删除时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            var cur = btn.DataContext as FiledEntity;

            this.ListEntity.Remove(cur);
        }

        
    }
}
