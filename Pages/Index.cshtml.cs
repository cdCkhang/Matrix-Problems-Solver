using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace WebAppMatrix.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<IndexModel> _logger;

        // DUONG DAN TOI FILE MAPLE.EXE CHO CHUONG TRINH THAY DOI O DONG 143
        // "C:\\Program Files\\Maple 2022\\bin.X86_64_WINDOWS\\cmaple.exe\"
        //

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public void OnGet(String ketqua)
        {
            ViewData["KETQUA"] = ketqua;
        }

        public IActionResult OnPostTest()
        {
            ViewData["KETQUA"] = "";
            return Page();
        }
        private string ToLatex(List<int> elements, int rows, int cols)
        {
            if (elements.Count != rows * cols)
            {
                throw new ArgumentException("The number of elements does not match the specified dimensions.");
            }

            StringBuilder latexBuilder = new StringBuilder();
            latexBuilder.AppendLine("\\begin{bmatrix}");

            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    latexBuilder.Append(elements[index++]);
                    if (j < cols - 1)
                    {
                        latexBuilder.Append(" & ");
                    }
                }
                if (i < rows - 1)
                {
                    latexBuilder.AppendLine(" \\\\");
                }
            }

            latexBuilder.AppendLine("\\end{bmatrix}");

            return latexBuilder.ToString();
        }
        private String toStringArray(List<int> Elements)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Array([");
            for (int i = 0; i < Elements.Count;i++) 
            {
                if (i<Elements.Count-1)
                {
                    sb.Append(Elements[i].ToString());
                    sb.Append(", ");
                }
                else
                {
                    sb.Append(Elements[Elements.Count-1].ToString());
                    sb.Append("])");
                }
            }            
            return sb.ToString();
        }
        private void ExecuteOperation(string ArrayMatrixA, string ArrayMatrixB, string rowsA, string colsA, string rowsB, string colsB, int operation)
        {
            string filename = generaterRandomName();

            string KnowledgePath = Path.Combine(_webHostEnvironment.ContentRootPath, "KB-IE").Replace(@"\", @"/");
           
            string DataPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Data", filename + "_debai.txt").Replace(@"\", @"/");
            string BatPath = Path.Combine(_webHostEnvironment.ContentRootPath, "BatFiles", filename + "_bat.bat").Replace(@"\", @"/");
            string ResultPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Solved", filename + "_ketqua.html").Replace(@"\", @"/");

            createFileData(ArrayMatrixA, ArrayMatrixB, rowsA, colsA, rowsB, colsB, operation, KnowledgePath, DataPath, ResultPath);
            createFileBat(BatPath, DataPath);
            goToConnectMaple(BatPath);
                    
            string output = System.IO.File.ReadAllText(ResultPath);
            ViewData["KETQUA"] = output;
        }

        private void createFileData(string ArrayMatrixA, string ArrayMatrixB,string rowsA, string colsA, string rowsB,string colsB,
            int operation ,string KnowledgePath, string pathData, string resultPath)
        {
            string selectedProc = "";
            switch (operation)
            {
                case 1:
                    selectedProc = "F_MatrixAddition";
                    break;
                case 2:
                    selectedProc = "F_MatrixSubtraction";
                    break;
                case 3:
                    selectedProc = "F_MatrixMultiplication";
                    break;
            }
            using (FileStream fs = new FileStream(pathData, FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.ASCII))
            {
                sw.WriteLine("restart;");
                sw.WriteLine("libname := libname, \"" + KnowledgePath + "\";");
                sw.WriteLine("with(MatrixModule);");
                sw.WriteLine("RawMatrixA := " + ArrayMatrixA + ";");
                sw.WriteLine("RawMatrixB := " + ArrayMatrixB + ";");
                sw.WriteLine("MatrixA := PartMatrixConverter(RawMatrixA, " + rowsA + ", " + colsA + ");");
                sw.WriteLine("MatrixB := PartMatrixConverter(RawMatrixB, " + rowsB + ", " + colsB + ");");
                sw.WriteLine("filename := \"" + resultPath + "\";");
                sw.WriteLine("Init();");
                sw.WriteLine(selectedProc + "(MatrixA, MatrixB, filename);");
            }
        }

        private void createFileBat(string pathFileBat, string inputPath)
        {
            using (FileStream fs = new FileStream(pathFileBat, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.ASCII))
            {
                sw.WriteLine("\"C:\\Program Files\\Maple 2022\\bin.X86_64_WINDOWS\\cmaple.exe\" " + inputPath + "\"");
            }
        }

        private void goToConnectMaple(string batPath)
        {
            var proc = new System.Diagnostics.Process
            {
                EnableRaisingEvents = true,
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = batPath,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
                }
            };
            proc.Start();
            proc.WaitForExit();
            System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(5));
        }

        private string generaterRandomName()
        { 
            string filename = System.IO.Path.GetRandomFileName();
            filename = filename.Replace(".", "");
            return filename;
        }

        public IActionResult OnPostMatrixAddition()
        {
            string pathData="C:/Resource/TestTxt.txt";
            using (FileStream fs = new FileStream(pathData, FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.ASCII))
            {
                sw.WriteLine("Hello world");
            }
                StringBuilder LatexMatrices = new StringBuilder();
            LatexMatrices.Append("<b><h4>Matrix Addition Equation: </h4></b>");
            List <List<int>> list1 = new List<List<int>>();
            var matrixInputs = HttpContext.Request.Form;

            list1.Add(new List<int>()); 
            list1.Add(new List<int>());            

            foreach (var key in matrixInputs.Keys)
            {
                if (key.StartsWith("a_"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list1[0].Add(parsedValue); 
                    }
                    else
                    {
                        list1[0].Add(1); 
                    }
                }
                else if (key.StartsWith("RowsA"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list1[1].Add(parsedValue); 
                    }
                }
                else if (key.StartsWith("ColsA"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list1[1].Add(parsedValue); 
                    }
                }
            }

            List<List<int>> list2 = new List<List<int>>();            
            list2.Add(new List<int>());
            list2.Add(new List<int>());

            foreach (var key in matrixInputs.Keys)
            {
                if (key.StartsWith("b_"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list2[0].Add(parsedValue);
                    }
                    else
                    {
                        list2[0].Add(1);
                    }
                }
                else if (key.StartsWith("RowsB"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list2[1].Add(parsedValue);
                    }
                }
                else if (key.StartsWith("ColsB"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list2[1].Add(parsedValue);
                    }
                }
            }

            string MatrixA = toStringArray(list1[0]);
            string MatrixB = toStringArray(list2[0]);
             
            LatexMatrices.Append("<h5><div class=\"matrix-container\">\r\n        <div class=\"matrix\">");
            if (list1.Count > 0 || list2.Count > 0 || list2[0].Count > 0 || list2[1].Count > 0)
            {
                string latexMatrixA = ToLatex(list1[0], list1[1][0], list1[1][1]);
                LatexMatrices.Append(latexMatrixA);
                LatexMatrices.Append("</div> + <div class=\"matrix\">");
                string latexMatrixB = ToLatex(list2[0], list2[1][0], list2[1][1]);
                LatexMatrices.Append(latexMatrixB);
                LatexMatrices.Append(" </div>\r\n    </div></h5>");                
            }
            ViewData["DEBAI"] = LatexMatrices;
            ExecuteOperation(MatrixA, MatrixB, list1[1][0].ToString(), list1[1][1].ToString(), list2[1][0].ToString(), list2[1][1].ToString(), 1);
            return Page();
        }

        public IActionResult OnPostMatrixSubtraction()
        {
            List<List<int>> list1 = new List<List<int>>();
            StringBuilder LatexMatrices = new StringBuilder();
            LatexMatrices.Append("<b><h4>Matrix Subtraction Equation: </h4></b>");
            var matrixInputs = HttpContext.Request.Form;

            list1.Add(new List<int>());
            list1.Add(new List<int>());

            foreach (var key in matrixInputs.Keys)
            {
                if (key.StartsWith("a_"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list1[0].Add(parsedValue);
                    }
                    else
                    {
                        list1[0].Add(1);
                    }
                }
                else if (key.StartsWith("RowsA"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list1[1].Add(parsedValue);
                    }
                }
                else if (key.StartsWith("ColsA"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list1[1].Add(parsedValue);
                    }
                }
            }

            List<List<int>> list2 = new List<List<int>>();
            list2.Add(new List<int>());
            list2.Add(new List<int>());

            foreach (var key in matrixInputs.Keys)
            {
                if (key.StartsWith("b_"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list2[0].Add(parsedValue);
                    }
                    else
                    {
                        list2[0].Add(1);
                    }
                }
                else if (key.StartsWith("RowsB"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list2[1].Add(parsedValue);
                    }
                }
                else if (key.StartsWith("ColsB"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list2[1].Add(parsedValue);
                    }
                }
            }

            string MatrixA = toStringArray(list1[0]);
            string MatrixB = toStringArray(list2[0]);

            LatexMatrices.Append("<h5><div class=\"matrix-container\">\r\n        <div class=\"matrix\">");
            if (list1.Count > 0 || list2.Count > 0 || list2[0].Count > 0 || list2[1].Count > 0)
            {
                string latexMatrixA = ToLatex(list1[0], list1[1][0], list1[1][1]);
                LatexMatrices.Append(latexMatrixA);
                LatexMatrices.Append("</div> - <div class=\"matrix\">");
                string latexMatrixB = ToLatex(list2[0], list2[1][0], list2[1][1]);
                LatexMatrices.Append(latexMatrixB);
                LatexMatrices.Append(" </div>\r\n    </div></h5>");              
            }
            ViewData["DEBAI"] = LatexMatrices;
            ExecuteOperation(MatrixA, MatrixB, list1[1][0].ToString(), list1[1][1].ToString(), list2[1][0].ToString(), list2[1][1].ToString(), 2);
            return Page();

        }

        public IActionResult OnPostMatrixMultiplication()
        {
            List<List<int>> list1 = new List<List<int>>();
            StringBuilder LatexMatrices = new StringBuilder();
            LatexMatrices.Append("<b><h4>Matrix Multiplication Equation: </h4></b>");
            var matrixInputs = HttpContext.Request.Form;

            list1.Add(new List<int>());
            list1.Add(new List<int>());

            foreach (var key in matrixInputs.Keys)
            {
                if (key.StartsWith("a_"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list1[0].Add(parsedValue);
                    }
                    else
                    {
                        list1[0].Add(1);
                    }
                }
                else if (key.StartsWith("RowsA"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list1[1].Add(parsedValue);
                    }
                }
                else if (key.StartsWith("ColsA"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list1[1].Add(parsedValue);
                    }
                }
            }

            List<List<int>> list2 = new List<List<int>>();
            list2.Add(new List<int>());
            list2.Add(new List<int>());

            foreach (var key in matrixInputs.Keys)
            {
                if (key.StartsWith("b_"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list2[0].Add(parsedValue);
                    }
                    else
                    {
                        list2[0].Add(1);
                    }
                }
                else if (key.StartsWith("RowsB"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list2[1].Add(parsedValue);
                    }
                }
                else if (key.StartsWith("ColsB"))
                {
                    var value = matrixInputs[key];
                    int parsedValue;
                    if (int.TryParse(value, out parsedValue))
                    {
                        list2[1].Add(parsedValue);
                    }
                }
            }

            string MatrixA = toStringArray(list1[0]);
            string MatrixB = toStringArray(list2[0]);

            LatexMatrices.Append("<h5><div class=\"matrix-container\">\r\n        <div class=\"matrix\">");
            if (list1.Count > 0 || list2.Count > 0 || list2[0].Count > 0 || list2[1].Count > 0)
            {
                string latexMatrixA = ToLatex(list1[0], list1[1][0], list1[1][1]);
                LatexMatrices.Append(latexMatrixA);
                LatexMatrices.Append("</div> x <div class=\"matrix\">");
                string latexMatrixB = ToLatex(list2[0], list2[1][0], list2[1][1]);
                LatexMatrices.Append(latexMatrixB);
                LatexMatrices.Append(" </div>\r\n    </div></h5>");              
            }
            ViewData["DEBAI"] = LatexMatrices;
            ExecuteOperation(MatrixA, MatrixB, list1[1][0].ToString(), list1[1][1].ToString(), list2[1][0].ToString(), list2[1][1].ToString(), 3);
            // Example: Concatenate values from list[0] for ViewData            
            return Page();

        }
        
    }    
}
