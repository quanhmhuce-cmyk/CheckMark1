using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CheckMark1
{
    // Class phụ trợ để lưu thông tin hiển thị
    public class ElementData
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string MarkValue { get; set; }
    }

    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        // Hàm nhận dữ liệu từ Command.cs đổ vào bảng "Chưa có Mark"
        public void SetNoMarkList(List<ElementData> list)
        {
            dgNoMark.ItemsSource = list;
        }

        // Hàm nhận dữ liệu từ Command.cs đổ vào bảng "Đã có Mark"
        public void SetHasMarkList(List<ElementData> list)
        {
            dgHasMark.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            if (parent != null) parent.Close();
        }
    }
}