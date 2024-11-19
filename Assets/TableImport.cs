using System;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class TableImport : MonoBehaviour
{
    [Serializable]
    public class AssetCSVСomparison
    {
        public string SheetName;
        public TextAsset Asset;
    }
        
    private string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
    private string ApplicationName = "fatum";
    private string SpreadsheetId = "1T7HckJJha-BqWqz_zuOQ1W3RNlXl6laQeEq3iRdgrPI";

    public List<AssetCSVСomparison> comparisons;

    public void Import()
    {
        UserCredential credential;

        using (var stream = new FileStream("sheets_credentials.json", FileMode.Open, FileAccess.Read))
        {
            // Путь к файлу credentials.json
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None).Result;
        }

        // Создание Google Sheets API сервиса
        var service = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        foreach (var assetCSVСomparison in comparisons)
        {
            // Запрос к Google Sheets
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, assetCSVСomparison.SheetName);
            var response = request.Execute();
            var values = response.Values;

            if (values is { Count: > 0 })
            {
                var csvContent = string.Empty;
                var rowCount = 0;
                var isCaptionRow = true;

                foreach (var row in values)
                {
                    var csvRow = "";

                    if (isCaptionRow)
                    {
                        rowCount = row.Count;
                        isCaptionRow = false;
                    }
                    
                    //Делаем из всех значений строки
                    csvRow = "\"" + string.Join("\",\"", row) + "\"";

                    if (row.Count < rowCount)
                        //Докидываем пустых полей каждой строке если они не заполнены
                        csvRow += "," + string.Join(",", Enumerable.Repeat("\"\"", rowCount - row.Count + 1));

                    csvContent += csvRow + "\n";;
                }

                var filePath = AssetDatabase.GetAssetPath(assetCSVСomparison.Asset);
                File.WriteAllText(filePath, csvContent);
            }
            else
            {
                Debug.Log("No data found.");
            }
        }
        
        Debug.Log("Import done.");
    }
}