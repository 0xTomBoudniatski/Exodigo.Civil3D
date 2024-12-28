
using CsvHelper;
using CsvHelper.Configuration;
using Exodigo.Civil3D.Contracts.Models;
using Exodigo.Civil3D.Utilities.Models;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Exodigo.Civil3D.Utilities;

public class HandleCsvUtility
{
    public IEnumerable<IManholeSurveyData> LoadManholeSurveyData()
    {
        
        try
        {
            var csvFilePath = PromptUserToSelectCsvFilePath();
            
            if(string.IsNullOrWhiteSpace(csvFilePath))
            {
                throw new Exception("Invalid CSV File Path - Try Again");
            }

            using var reader = new StreamReader(csvFilePath);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
            };

            using var csvReader = new CsvReader(reader, config);

            var result = csvReader.GetRecords<ManholeSurveyDataModel>();
            return result.ToList();
        }
        catch
        {
            throw;
        }
    }

    private static string PromptUserToSelectCsvFilePath()
    {
        var openFileDialog = new OpenFileDialog()
        {
            Title = "Select a CSV file",
            Filter = "CSV Files (*.csv)|*.csv",
            Multiselect = false
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            return openFileDialog.FileName;
        }
        else
        {
            return string.Empty;
        }
    }




}
