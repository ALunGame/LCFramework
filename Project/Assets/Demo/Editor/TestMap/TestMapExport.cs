using System.IO;
using LCConfig.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Demo.TestMap
{
    public static class TestMapExport
    {
        [MenuItem("测试/地图导入")]
        public static void TestExport()
        {
            string tPath = "../Config/测试关卡.xlsx";
            tPath = Path.GetFullPath(tPath);
            
            FileInfo exFile = new FileInfo(tPath);
            ExcelPackage package = new ExcelPackage(exFile);

            ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
            ExportWorksheet(worksheet);
            
            package.Dispose();
        }

        private static GameObject mapRoot;
        private static GameObject mapCell;
        
        private static void ExportWorksheet(ExcelWorksheet pSheet)
        {
            RuleTile groundTile = AssetDatabase.LoadAssetAtPath<RuleTile>("Assets/Demo/Asset/Maps/101/Tile/map101_groundRule_tile.asset");
            Tilemap tilemap = GameObject.Find("Tilemaps/Ground/GroundTile").GetComponent<Tilemap>();
            tilemap.ClearAllTiles();
            
            // mapCell = GameObject.Find("MapCell");
            //
            // mapRoot = new GameObject("MapRoot");
            // mapRoot.transform.localPosition = Vector3.zero;
            // mapRoot.transform.localEulerAngles = Vector3.zero;
            // mapRoot.transform.localScale = Vector3.one;
            
            Debug.Log($"ExportWorksheet:{pSheet.Dimension.Start.Row} --> {pSheet.Dimension.End.Row}");
            Debug.Log($"ExportWorksheet:{pSheet.Dimension.Start.Column} --> {pSheet.Dimension.End.Column}");
            
            //最大行
            int _MaxRowNum = pSheet.Dimension.End.Row;
            //最小行
            int _MinRowNum = pSheet.Dimension.Start.Row;
            
            //最大列
            int _MaxColumnNum = pSheet.Dimension.End.Column;
            //最小列
            int _MinColumnNum = pSheet.Dimension.Start.Column;

            ExcelRange testCell = pSheet.Cells[4, 1];
            
            //ExcelColor color
            for (int row = _MinRowNum; row <= _MaxRowNum; row++)
            {
                for (int col = _MinColumnNum; col <= _MaxColumnNum; col++)
                {
                    string value = ExcelReader.GetCellValue(pSheet, row, col);
                    ExcelRange cell = pSheet.Cells[row, col];
                    //Debug.Log($"value-->{cell.Value} row:{row} col:{col}");
                    if (cell.Text == "1")
                    {
                        // Debug.LogWarning($"地面-->row:{row} col:{col}");
                        //
                        // GameObject cellGo = GameObject.Instantiate(mapCell);
                        // cellGo.name = $"Ground_{row}_{col}";
                        // cellGo.AddComponent<SpriteRenderer>();
                        //
                        // cellGo.transform.SetParent(mapRoot.transform);
                        // cellGo.transform.localPosition = new Vector3((_MaxColumnNum - col)*-1, _MaxRowNum - row, 0);
                        // cellGo.transform.localEulerAngles = Vector3.zero;
                        // cellGo.transform.localScale = Vector3.one;
                        
                        tilemap.SetTile(new Vector3Int((_MaxColumnNum - col)*-1, _MaxRowNum - row),groundTile);
                    }
                }
            }
            
        }
    }
}