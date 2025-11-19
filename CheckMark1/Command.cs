using System;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows; // Cần cho Window UI

namespace CheckMark1
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // 1. Lấy danh sách đang chọn (Selection)
            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();

            // Nếu không chọn gì cả, thông báo cho người dùng (hoặc có thể sửa để lấy toàn bộ View)
            if (selectedIds.Count == 0)
            {
                TaskDialog.Show("Thông báo", "Vui lòng chọn các đối tượng cần kiểm tra Mark trong mô hình.");
                return Result.Cancelled;
            }

            // 2. Chuẩn bị 2 danh sách chứa dữ liệu
            List<ElementData> noMarkList = new List<ElementData>();
            List<ElementData> hasMarkList = new List<ElementData>();

            // 3. Duyệt qua từng đối tượng để kiểm tra
            foreach (ElementId id in selectedIds)
            {
                Element ele = doc.GetElement(id);

                // Bỏ qua các đối tượng không phải Model (ví dụ View, Level...) nếu lỡ chọn nhầm
                if (ele.Category == null) continue;

                // Lấy tham số Mark
                Parameter paramMark = ele.get_Parameter(BuiltInParameter.ALL_MODEL_MARK);

                // Tạo đối tượng data để hiển thị
                ElementData data = new ElementData
                {
                    Id = ele.Id.ToString(),
                    Category = ele.Category.Name,
                    Name = ele.Name // Thường là Family Type
                };

                // Kiểm tra giá trị Mark
                if (paramMark != null && paramMark.HasValue && !string.IsNullOrEmpty(paramMark.AsString()))
                {
                    data.MarkValue = paramMark.AsString();
                    hasMarkList.Add(data);
                }
                else
                {
                    data.MarkValue = "[Trống]";
                    noMarkList.Add(data);
                }
            }

            // 4. Hiển thị Giao diện
            UserControl1 userControl = new UserControl1();

            // Đổ dữ liệu vào 2 bảng
            userControl.SetNoMarkList(noMarkList);
            userControl.SetHasMarkList(hasMarkList);

            Window window = new Window
            {
                Title = $"Quản lý Mark - Đã chọn {selectedIds.Count} đối tượng",
                Content = userControl,
                Width = 900,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            window.ShowDialog();

            return Result.Succeeded;
        }
    }
}