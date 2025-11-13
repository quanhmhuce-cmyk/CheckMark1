using Autodesk.Revit.DB;
using System.IO;
using System.Threading;
using Autodesk.Revit.UI;
namespace WpfExternalApp
{
    public class EventWatcher
    {
        private readonly UIApplication _uiapp;

        public EventWatcher(UIApplication app)
        {
            _uiapp = app;
        }

        public void Start()
        {
            new Thread(() =>
            {
                while (true)
                {
                    if (File.Exists(@"C:\Temp\trigger.txt"))
                    {
                        string cmd = File.ReadAllText(@"C:\Temp\trigger.txt");
                        if (cmd == "HighlightWalls")
                        {
                            HighlightWalls();
                            File.Delete(@"C:\Temp\trigger.txt");
                        }
                    }
                    Thread.Sleep(2000);
                }
            })
            { IsBackground = true }.Start();
        }

        private void HighlightWalls()
        {
            UIDocument uidoc = _uiapp.ActiveUIDocument;
            var walls = new FilteredElementCollector(uidoc.Document)
                .OfCategory(BuiltInCategory.OST_Walls)
                .WhereElementIsNotElementType();

            TaskDialog.Show("Revit API", $"Đã tìm thấy {walls.GetElementCount()} tường trong mô hình!");
        }
    }
}
