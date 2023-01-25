using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncIncomeOutcome
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

/*
Task.Delay(1000).Wait() - Synced, Blocking
await Task.Delay(1000) - Synced, Not Blocking
Task.Delay(1000) - Asynced, Not Blocking
*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            decimal income = 0m, outcome = 0m, total = 0m;
            Stopwatch sw = Stopwatch.StartNew();    
            
            var task1 = Task.Run(async () => //Gets the income and sets it
            {
               async Task<decimal> getIncome()
               {
                    string incomePath = @"C:\Payments\income.txt";
                    var incomeLines = await File.ReadAllLinesAsync(incomePath);
                    decimal totalIncome = 0m;
                    foreach (var line in incomeLines)
                    {
                        
                        totalIncome += decimal.Parse(line);  
                    }
                    return totalIncome;
               }
                income=await getIncome();
            });

            var task2 = Task.Run(async () => //Gets the outcome sets it and does the rest aswell
            {
                async Task<decimal> getOutcome()
                {
                    string outcomePath = @"C:\Payments\outcome.txt";
                    var outcomeLines = await File.ReadAllLinesAsync(outcomePath);
                    decimal totalOutcome = 0m;
                    foreach (var line in outcomeLines)
                    {

                        totalOutcome += decimal.Parse(line);
                    }
                    return totalOutcome;

                }
                outcome = await getOutcome();

                await task1;
                total = income - outcome;
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                Dispatcher.Invoke(()=>
                { 
                    TextBox.Text = total.ToString();
                    StopwatchLabel.Content = "Time Elapsed: " + ts.ToString();
                });
            });
        }

        //This Doesn't Work Yet Ignore!!
        
        //public async Task<string[]> ReadAllLinesInFileAsync(string path)
        //{
        //    string[] emptyString = new string[0];
        //    var readTask = await Task.Run(() =>
        //    {
        //        var lines = File.ReadAllLines(path);
        //        return lines;
        //    });
        //    return emptyString;
        //}


    }
}
