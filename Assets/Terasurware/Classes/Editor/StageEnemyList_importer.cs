using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class StageEnemyList_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/ExcelData/Stage/StageEnemyList.xlsx";
	private static readonly string exportPath = "Assets/ExcelData/Stage/StageEnemyList.asset";
	private static readonly string[] sheetNames = { "Sheet1", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			X_StageEnemyList data = (X_StageEnemyList)AssetDatabase.LoadAssetAtPath (exportPath, typeof(X_StageEnemyList));
			if (data == null) {
				data = ScriptableObject.CreateInstance<X_StageEnemyList> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					X_StageEnemyList.Sheet s = new X_StageEnemyList.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						X_StageEnemyList.Param p = new X_StageEnemyList.Param ();
						
					cell = row.GetCell(0); p.Time = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.EnemyViewId = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.EnemyMoveId = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.BulletSetId = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.AppearViewportX = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.AppearViewportY = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.AppearOffsetX = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.AppearOffsetZ = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.AppearOffsetY = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.AppearRotateY = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.OtherParameters = (cell == null ? "" : cell.StringCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
