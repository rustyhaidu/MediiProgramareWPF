using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

namespace Haidu_Claudiu_Lab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DoughnutMachine myDoughnutMachine;
        private int m_RaisedGlazed;
        private int m_RaisedSugar;
        private int m_FilledLemon;
        private int m_FilledChocolate;
        private int m_FilledVanilla;

        KeyValuePair<DoughnutType, double>[] PriceList = {
             new KeyValuePair<DoughnutType, double>(DoughnutType.Sugar, 2.5),
             new KeyValuePair<DoughnutType, double>(DoughnutType.Glazed,3),
             new KeyValuePair<DoughnutType, double>(DoughnutType.Chocolate,4.5),
             new KeyValuePair<DoughnutType, double>(DoughnutType.Vanilla,4),
             new KeyValuePair<DoughnutType, double>(DoughnutType.Lemon,3.5)
         };

        DoughnutType selectedDoughnut;

        public MainWindow()
        {
            InitializeComponent();

            //creare obiect binding pentru comanda
            CommandBinding cmd1 = new CommandBinding();
            //asociere comanda
            cmd1.Command = ApplicationCommands.Print;
            //input gesture: I + Alt
            ApplicationCommands.Print.InputGestures.Add(new KeyGesture(Key.I, ModifierKeys.Alt));
            //asociem un handler
            cmd1.Executed += new ExecutedRoutedEventHandler(CtrlP_CommandHandler);
            //adaugam la colectia CommandBindings
            CommandBindings.Add(cmd1);

            //Doughnuts>Stop
            //comanda custom
            CommandBinding cmd2 = new CommandBinding();
            cmd2.Command = CustomCommands.StopCommand.Launch;
            cmd2.Executed += new
            ExecutedRoutedEventHandler(CtrlS_CommandHandler);//asociem handler
            CommandBindings.Add(cmd2);
        }

        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            myDoughnutMachine = new DoughnutMachine();
            myDoughnutMachine.DoughnutComplete += new
            DoughnutMachine.DoughnutCompleteDelegate(DoughnutCompleteHandler);

            cmbType.ItemsSource = PriceList;
            cmbType.DisplayMemberPath = "Key";
            cmbType.SelectedValuePath = "Value";
        }

        private void glazedToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            glazedToolStripMenuItem.IsChecked = true;
            sugarToolStripMenuItem.IsChecked = false;
            myDoughnutMachine.MakeDoughnuts(DoughnutType.Glazed);
        }
        private void sugarToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            glazedToolStripMenuItem.IsChecked = false;
            sugarToolStripMenuItem.IsChecked = true;
            myDoughnutMachine.MakeDoughnuts(DoughnutType.Sugar);
        }

        private void lemonToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            lemonFilledMenuItem.IsChecked = true;
            chocolateFilledMenuItem.IsChecked = false;
            vanillaFilledMenuItem.IsChecked = false;
            myDoughnutMachine.MakeDoughnuts(DoughnutType.Lemon);
        }

        private void chocolateToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            lemonFilledMenuItem.IsChecked = false;
            chocolateFilledMenuItem.IsChecked = true;
            vanillaFilledMenuItem.IsChecked = false;
            myDoughnutMachine.MakeDoughnuts(DoughnutType.Chocolate);
        }

        private void vanillaToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            lemonFilledMenuItem.IsChecked = false;
            chocolateFilledMenuItem.IsChecked = false;
            vanillaFilledMenuItem.IsChecked = true;
            myDoughnutMachine.MakeDoughnuts(DoughnutType.Vanilla);
        }

        private void DoughnutCompleteHandler()
        {
            switch (myDoughnutMachine.Flavor)
            {
                case DoughnutType.Glazed:
                    {
                        m_RaisedGlazed++;
                        txtGlazedRaised.Text = m_RaisedGlazed.ToString();
                        break;
                    }
                case DoughnutType.Sugar:
                    {
                        m_RaisedSugar++;
                        txtSugarRaised.Text = m_RaisedSugar.ToString();
                        break;
                    }

                case DoughnutType.Lemon:
                    {
                        m_FilledLemon++;
                        txtLemonFilled.Text = m_FilledLemon.ToString();
                        break;
                    }
                case DoughnutType.Chocolate:
                    {
                        m_FilledChocolate++;
                        txtChocolateFilled.Text = m_FilledChocolate.ToString();
                        break;
                    }
                case DoughnutType.Vanilla:
                    {
                        m_FilledVanilla++;
                        txtVanillaFilled.Text = m_FilledVanilla.ToString();
                        break;
                    }
                default:
                    {
                        throw new InvalidEnumArgumentException($"Invalid Flavor {myDoughnutMachine.Flavor}");
                    }
            }
        }

        private void stopToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myDoughnutMachine.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void txtQuantity_KeyPress(object sender, KeyEventArgs e)
        {
            if (!(e.Key >= Key.D0 && e.Key <= Key.D9))
            {
                MessageBox.Show("Numai cifre se pot introduce!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilledItems_Click(object sender, RoutedEventArgs e)
        {
            string DoughnutFlavour;

            MenuItem SelectedItem = (MenuItem)e.OriginalSource;
            DoughnutFlavour = SelectedItem.Header.ToString();
            Enum.TryParse(DoughnutFlavour, out DoughnutType myFlavour);
            myDoughnutMachine.MakeDoughnuts(myFlavour);
        }

        private void FilledItemsShow_Click(object sender, RoutedEventArgs e)
        {
            string mesaj;
            MenuItem SelectedItem = (MenuItem)e.OriginalSource;
            mesaj = SelectedItem.Header.ToString() + " doughnuts are being cooked!";
            Title = mesaj;
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtPrice.Text = cmbType.SelectedValue.ToString();
            KeyValuePair<DoughnutType, double> selectedEntry = (KeyValuePair<DoughnutType, double>)cmbType.SelectedItem;
            selectedDoughnut = selectedEntry.Key;
        }

        private int ValidateQuantity(DoughnutType selectedDoughnut)
        {
            int q = int.Parse(txtQuantity.Text);
            int r = 1;

            switch (selectedDoughnut)
            {
                case DoughnutType.Glazed:
                    {
                        if (q > m_RaisedGlazed)
                        {
                            r = 0;
                        }

                        break;
                    }
                case DoughnutType.Sugar:
                    {
                        if (q > m_RaisedSugar)
                        {
                            r = 0;
                        }

                        break;
                    }
                case DoughnutType.Chocolate:
                    {
                        if (q > m_FilledChocolate)
                        {
                            r = 0;
                        }

                        break;
                    }
                case DoughnutType.Lemon:
                    {
                        if (q > m_FilledLemon)
                        {
                            r = 0;
                        }

                        break;
                    }
                case DoughnutType.Vanilla:
                    {
                        if (q > m_FilledVanilla)
                        {
                            r = 0;
                        }

                        break;
                    }
            }
            return r;
        }

        private void btnAddToSale_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateQuantity(selectedDoughnut) > 0)
            {
                lstSale.Items.Add($"{txtQuantity.Text}\t{selectedDoughnut} : {txtPrice.Text}\t{double.Parse(txtQuantity.Text) * double.Parse(txtPrice.Text)}");
            }
            else
            {
                MessageBox.Show("Cantitatea introdusa nu este disponibila in stoc!");
            }
        }

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            lstSale.Items.Remove(lstSale.SelectedItem);
        }

        private void btnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            txtTotal.Text = (double.Parse(txtQuantity.Text) * double.Parse(txtPrice.Text)).ToString();
            foreach (string s in lstSale.Items)
            {
                var doughnutType = Helper.GetDoughnutTypeFromSaleListItem(s);

                switch (doughnutType)
                {
                    case DoughnutType.Glazed:
                        {
                            m_RaisedGlazed -= int.Parse(s.Substring(0, s.IndexOf($"{doughnutType}")));
                            txtGlazedRaised.Text = m_RaisedGlazed.ToString();
                            break;
                        }
                    case DoughnutType.Sugar:
                        {
                            m_RaisedSugar -= int.Parse(s.Substring(0, s.IndexOf(" ")));
                            txtSugarRaised.Text = m_RaisedSugar.ToString();
                            break;
                        }
                    case DoughnutType.Chocolate:
                        {
                            m_FilledChocolate -= int.Parse(s.Substring(0, s.IndexOf(" ")));
                            txtChocolateFilled.Text = m_FilledChocolate.ToString();
                            break;
                        }
                    case DoughnutType.Lemon:
                        {
                            m_FilledLemon -= int.Parse(s.Substring(0, s.IndexOf(" ")));
                            txtLemonFilled.Text = m_FilledLemon.ToString();
                            break;
                        }
                    case DoughnutType.Vanilla:
                        {
                            m_FilledVanilla -= int.Parse(s.Substring(0, s.IndexOf(" ")));
                            txtVanillaFilled.Text = m_FilledVanilla.ToString();
                            break;
                        }
                }
            }
        }

        private void CtrlP_CommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("You have in stock:" + m_RaisedGlazed + " Glazed," + m_RaisedSugar + "Sugar, " + m_FilledLemon +
                " Lemon, " + m_FilledChocolate + " Chocolate, " + m_FilledVanilla + " Vanilla"
           );
        }

        private void CtrlS_CommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            //handler pentru comanda Ctrl+S -> se va executa stopToolStripMenuItem_Click
            MessageBox.Show("Ctrl+S was pressed! The doughnut machine will stop!");
            stopToolStripMenuItem_Click(sender, e);
        }
    }
  }
