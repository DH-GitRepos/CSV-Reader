using System.Collections.Generic;
using System.Windows;
using System.IO;
using System;

namespace csv_reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
 
    public partial class MainWindow : Window
    {
        public List<SaleItem> saleItemsList { get; private set; }
        public string dataFilePath { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            // set a variable to point to the project root directory
            string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            // set the file path to the data file
            this.dataFilePath = Path.Combine(projectRoot, "data", "Data.csv");
        }


        // DISPLAY THE CSV DATA IN DATE ORDER
        private void Click_ShowDateOrderedData(object sender, RoutedEventArgs e)
        {            
            // read the csv file
            List<string> CSVData = CSV_fileReader(this.dataFilePath);
            // Parse the CSV data
            Parse_CSV_Data(CSVData);

            // Sort the saleItemsList by Date
            this.saleItemsList.Sort((x, y) => x.Date.CompareTo(y.Date));

            // Display the sorted data
            DisplayOrderedOutput(this.saleItemsList);
        }


        // DISPLAY A SUMMARY OF THE ITEMS SOLD, TOTALLING THE QUANTITIES SOLD PER PRODUCT
        private void Click_ShowItemSummary(object sender, RoutedEventArgs e)
        {
            // read the csv file
            List<string> CSVData = CSV_fileReader(this.dataFilePath);
            // Parse the CSV data
            Parse_CSV_Data(CSVData);

            Dictionary<string, int> itemQuantities = new Dictionary<string, int>();

            foreach (var saleItem in this.saleItemsList)
            {
                if (itemQuantities.ContainsKey(saleItem.Product))
                {
                    // Add the quantity to the existing quantity
                    itemQuantities[saleItem.Product] += (int)saleItem.Quantity;
                }
                else
                {
                    // Add the product to the dictionary
                    itemQuantities.Add(saleItem.Product, (int)saleItem.Quantity);
                    // Add the quantity to the product
                    itemQuantities[saleItem.Product] = (int)saleItem.Quantity;
                }
            }

            // Display the formatted data
            DisplaySummaryOutput(itemQuantities);
        }


        // DISPLAY THE ORDERED OUTPUT
        private void DisplayOrderedOutput(List<SaleItem> output)
        {
            // Display the output to Output_Box
            Output_Box.Text = "";
            string outputText = "";

            outputText += String.Format("{0,-12}\t{1,-20}\t\t{2,-10}\n", "Date", "Product", "Quantity");

            foreach (var saleItem in output)
            {
                outputText += String.Format("{0,-12:dd-MM-yyyy}\t{1,-20}\t\t{2,-10:N0}\n", saleItem.Date, saleItem.Product, saleItem.Quantity);
            }

            Output_Box.Text = outputText;
        }


        // DISPLAY THE SUMMARY OUTPUT
        private void DisplaySummaryOutput(Dictionary<string, int> output)
        {
            // Display the output to Output_Box
            Output_Box.Text = "";
            string outputText = "";

            outputText += String.Format("{0,-20}\t\t{1,-10}\n", "Product", "Quantity");

            foreach (var item in output)
            {
                outputText += String.Format("{0,-20}\t\t{1,-10:N0}\n", item.Key, item.Value);
            }

            Output_Box.Text = outputText;
        }


        // PARSE THE CSV DATA INTO A LIST OF SaleItem OBJECTS
        private void Parse_CSV_Data(List<string> CSVData)
        {
            // Clear the saleItemsList
            this.saleItemsList = new List<SaleItem>();

            // Parse the fileRows array and create a SaleItem object for each row.
            foreach (var row in CSVData)
            {
                string[] columns = row.Split(',');
                try
                {
                    SaleItem saleItem = new SaleItem(System.DateTime.Parse(columns[0]), columns[1], long.Parse(columns[2]));
                    this.saleItemsList.Add(saleItem);
                }
                catch(FormatException)
                {
                    // if the row is not in the correct format, skip it (header row)
                    continue;
                }
            }
        }


        // READ THE CSV FILE AND RETURN THE DATA AS A LIST OF STRINGS
        private List<string> CSV_fileReader(string filePath)
        {
            // filePath = "C:\\Users\\user\\Desktop\\sales.csv";
            List<string> fileRows = new List<string>();

            try {
                // Read the file and add each read line to the fileRows array.
                using (var reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        fileRows.Add(reader.ReadLine());
                    }
                }                
            }
            catch (FileNotFoundException)
            {
                fileRows.Add("ERROR: Data file not found.");
            }
            catch (IOException)
            {
                fileRows.Add("ERROR: Unable to read data file.");
            }

            return fileRows;
        }
    }
}