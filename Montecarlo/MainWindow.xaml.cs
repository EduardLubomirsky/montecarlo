using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Montecarlo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public class compare
        {
            private int _aNumber; // приватные свойства
            private int _bNumber;

            public int aNumber // свойство 1
            {
                get { return _aNumber; }
                set { _aNumber = value; }
            }
            public int bNumber // свойство 2
            {
                get { return _bNumber; }
                set { _bNumber = value;}
            }
        }

        public MainWindow()
        {

            InitializeComponent();
        }

        long counter = 0;
        TimeSpan time1;
        DateTime initial_time;
        DateTime current_time;


        int WriteToFile(string patch, string str)
        {
            string[] allLines = File.ReadAllLines(patch);
            for (int i = 0; i < allLines.Length; i++)
            {
                if (allLines[i] == str)
                {
                    return 0;
                }

            }
            using (StreamWriter writer = new StreamWriter(patch, true))
            {
                if (!String.IsNullOrEmpty(str))
                {
                    writer.WriteLine(str);
                    counter++;
                    //uniqueSolution.Content = counter.ToString();
                    initial_time = DateTime.Now;
                    time1 = initial_time - current_time;
                    // Dispatcher.BeginInvoke(new ThreadStart(delegate {
                    listbox1.Items.Insert(0, counter + "TIME: " + String.Format("{0:00}:{1:00}:{2:00}",
                        time1.Hours, time1.Minutes, time1.Seconds));
                    //   }));
                }
                //listbox1.Items.Add(counter + "TIME: " + String.Format("{0:00}:{1:00}:{2:00}",
                //    time1.Hours, time1.Minutes, time1.Seconds));
                //}
            }
            return 0;
        }



        Random rand = new Random();
        string GenerateTheBoard(int size) {

            int[,] array = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    array[i, j] = i;
                }
            }








            compare[] newCompare = new compare[0];
            int buff;
            int last;
            int n = size;
            int randomTo = size;
            int randomNumber;

            var knownNumbers = new HashSet<int>();






            string result ="";
            for (int j = 0; j < size; j++)

            {
               // MessageBox.Show(j.ToString());
                knownNumbers.Clear();
                Array.Resize(ref newCompare, newCompare.Length + 1);
                newCompare[j] = new compare();
                randomNumber = rand.Next(0, randomTo);
                buff = array[randomNumber, j];// заносим в буфер ферзя
                newCompare[j].aNumber = buff;
                newCompare[j].bNumber = j;
                bool exit = false;
                if (newCompare.Length > 1)
                    do{
                        exit = false;
                        for (int t = newCompare.Length-1; t > -1; t--)
                        {
                            if (t == j) continue;
                            else
                            if ((newCompare[t].aNumber - newCompare[j].aNumber) == (newCompare[t].bNumber - newCompare[j].bNumber) ||
                                (newCompare[t].aNumber - newCompare[j].aNumber) * -1 == (newCompare[t].bNumber - newCompare[j].bNumber) ||
                                (newCompare[t].aNumber - newCompare[j].aNumber) == (newCompare[t].bNumber - newCompare[j].bNumber) * -1)
                            {
                                exit = true;
                                do
                                {
                                    randomNumber = rand.Next(0, randomTo);
                                } while (!knownNumbers.Add(randomNumber));

                                buff = array[randomNumber, j];// заносим в буфер ферзя
                                newCompare[j].aNumber = buff;
                                // MessageBox.Show("СТОЛБЕЦ " +j+" сколько сгенерировано "+ knownNumbers.Count +"сколько можно: "+ randomTo);
                                if (knownNumbers.Count == randomTo && j != size - 1)
                                {
                                   // knownNumbers.Clear();
                                    WriteToFile(patch, GenerateTheBoard(10));
                                    //MessageBox.Show("вызов");
                                }
                                if (j == size - 1)
                                {
                                    MessageBox.Show("выход");
                                    exit = false;
                                    break;
  
                                }
                            }

                            //    MessageBox.Show(newCompare[j].aNumber + " - " + newCompare[t].aNumber + " = " + (newCompare[j].aNumber - newCompare[t].aNumber).ToString() + "\n"
                            //               + newCompare[j].bNumber + " - " + newCompare[t].bNumber + " = " + (newCompare[j].bNumber - newCompare[t].bNumber).ToString());
                        }

                    } while (exit==true);



                last = array[n - 1, j];
                for (int k = j; k < size; k++)
                    array[randomNumber, k] = last;// помещаем вместо ферзя последний элемент со столбца

                        array[n - 1, j] = buff;
                        n = n - 1;
                        if (randomTo > 1)
                        {
                            randomTo--;
                        }
            }
  

            for (int i = size - 1; i >= 0; i--)
            {
                for (int j = 0; j < size; j++)
                {
                    result += "(" + array[i, j].ToString() + ", " + j + ");";
                    --i;
                }
            }
            //resultLog.Text += result + "\r\n";
            return result;
        }
        string patch = "C:\\Users\\mario\\Desktop\\montecarlo.txt";
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            WriteToFile(patch, GenerateTheBoard(10));
            //            current_time = DateTime.Now;
            //            
            //            for(long i = 0; i < 1; i++)
            //                WriteToFile(patch, GenerateTheBoard(10000));

            //  if (cts != null)
            //      return;
            //  using (cts = new CancellationTokenSource())
            //      await Calculate(cts.Token);
            //  cts = null;
        }


       // CancellationTokenSource cts;


//        Task Calculate(CancellationToken ct)
//        {
//            return Task.Run(() => CalculateImpl(ct), ct);
//        }

        //int CalculateImpl(CancellationToken ct)
        //{
        //    current_time = DateTime.Now;

        //   // for (; ; )
        //    //{
        //        WriteToFile(patch, GenerateTheBoard(6));
        //        if (ct.IsCancellationRequested)
        //            return 0 ;
        //   // }
        //    return 0;
            
        //}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
   //         if (cts != null)
   //             cts.Cancel();
       }
    }
}
