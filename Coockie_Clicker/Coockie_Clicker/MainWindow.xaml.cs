using Coockie_Clicker.Classes;
using Coockie_Clicker.Models;
using Coockie_Clicker.Achievements;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using System.Reflection;

namespace Coockie_Clicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Classes, Achievements
        Coockie_Clicker.Achievements.Coockies_Clicked coockies_Clicked = new Coockie_Clicker.Achievements.Coockies_Clicked();

        // Classes, Models
        Models.Cursor CursorClass = new Models.Cursor();
        Farm FarmClass = new Farm();
        Grandma GrandmaClass = new Grandma();
        Factory FactoryClass = new Factory();
        Mine MineClass = new Mine();
        Bank BankClass = new Bank();
        Temple TempleClass = new Temple();

        // Variable
        private double Score, Income, Clicked, Bought, Used;

        // Timer
        private DispatcherTimer timer;
        private DispatcherTimer goldenCookieTimer;

        public MainWindow() // Werkt is timing van 10 Milliseconden
        {
            InitializeComponent();
            ButtonHidden();

            // Timer Tick Voor de Rest van het programma
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);

            timer.Tick += Timer_Tick;
            timer.Start();


            //Timer Tick Voor golden Coockie
            goldenCookieTimer = new DispatcherTimer();
            goldenCookieTimer.Interval = TimeSpan.FromMinutes(1);

            goldenCookieTimer.Tick += GoldenCookieTimer_Tick;
            goldenCookieTimer.Start();
        }

        private void ButtonHidden() // Werkt
        {
            // Normale Store Buttons
            BtnCursor.Visibility = Visibility.Hidden;
            BtnGrandma.Visibility = Visibility.Hidden;
            BtnFarm.Visibility = Visibility.Hidden;
            BtnMine.Visibility = Visibility.Hidden;
            BtnFactory.Visibility = Visibility.Hidden;
            BtnBank.Visibility = Visibility.Hidden;
            BtnTemple.Visibility = Visibility.Hidden;

            //Bonus Buttons
            BtnCursorBonus.Visibility = Visibility.Hidden;
            BtnFactoryBonus.Visibility = Visibility.Hidden;
            BtnFarmBonus.Visibility = Visibility.Hidden;
            BtnGrandmaBonus.Visibility = Visibility.Hidden;
            BtnMineBonus.Visibility = Visibility.Hidden;
            BtnTempleBonus.Visibility = Visibility.Hidden;
            BtnBankBonus.Visibility = Visibility.Hidden;
        }

        private void Timer_Tick(object sender, EventArgs e) // Werkt
        {
            // gaat elke 10 miliseconden alles visual updaten van de client.
            UpdateCounterText();
            UpdateScoreText();
            UpdateInvestmentButtonVisibility(); 
            ControleerAchievements();
            UpdateButtonEnabledState(BtnCursor, CursorClass.Prijs);
            UpdateButtonEnabledState(BtnCursorBonus, CursorClass.PrijsBonus);
            UpdateButtonEnabledState(BtnGrandma, GrandmaClass.Prijs);
            UpdateButtonEnabledState(BtnGrandmaBonus, GrandmaClass.PrijsBonus);
            UpdateButtonEnabledState(BtnFarm, FarmClass.Prijs);
            UpdateButtonEnabledState(BtnFarmBonus, FarmClass.PrijsBonus);
            UpdateButtonEnabledState(BtnMine, MineClass.Prijs);
            UpdateButtonEnabledState(BtnMineBonus, MineClass.PrijsBonus);
            UpdateButtonEnabledState(BtnFactory, FactoryClass.Prijs);
            UpdateButtonEnabledState(BtnFactoryBonus, FactoryClass.PrijsBonus);
            UpdateButtonEnabledState(BtnBank, BankClass.Prijs);
            UpdateButtonEnabledState(BtnBankBonus, BankClass.PrijsBonus);
            UpdateButtonEnabledState(BtnTemple, TempleClass.Prijs);
            UpdateButtonEnabledState(BtnTempleBonus, TempleClass.PrijsBonus);
        }

        private void GoldenCookieTimer_Tick(object sender, EventArgs e) // Werkt
        {
            if (new Random().Next(100) < 30) // 30 procent kans
            {
                ShowGoldenCookie();
            }
        }

        private void ControleerAchievements() //fixen
        {
            // achievements Coockie Clicked
            if (Clicked == coockies_Clicked.RequiredCookies)
            {
                coockies_Clicked.IsUnlocked = true;
            }
        }

        private void ShowGoldenCookie() // Werkt
        {
            // Code om een gouden koekje weer te geven
            double left = new Random().Next((int)ActualWidth);
            double top = new Random().Next((int)ActualHeight);

            // aanmaken van golden cookie
            Image Goldencookie = new Image
            {
                Source = new BitmapImage(new Uri("C:\\Users\\Gebruiker\\Documents\\GitHub\\-coockie-cliker\\Coockie_Clicker\\Coockie_Clicker\\img\\Golden_cookie.png")),
                Width = 20,
                Height = 20,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(left, top, 0, 0)
            };

            Goldencookie.MouseDown += Goldencookie_MouseDown;

            // Voeg het gouden koekje toe aan het scherm
            cookie_Grid.Children.Add(Goldencookie);

            // animatie 
            DoubleAnimation animation = new DoubleAnimation(1.0, 0.0, TimeSpan.FromSeconds(5));
            Goldencookie.BeginAnimation(OpacityProperty, animation);
        }

        private void Goldencookie_MouseDown(object sender, MouseButtonEventArgs e) // Werkt
        {
            double cookieValue = CalculateCookieValue();

            Score += cookieValue; // nieuwe scoren
            cookie_Grid.Children.Remove(sender as UIElement); // delete golden cookie
        }

        private double CalculateCookieValue() // Werkt
        {
            double Totaalincome = Convert.ToDouble(LbIncome.Content); // Huidige hoeveelheid

            double TotaalincomeNa15Min = Totaalincome * 15 * 60; // Bereken het bedrag na 15 minuten
            return TotaalincomeNa15Min;
        }

        private void UpdateCounterText() // Werkt
        {
            if (Income >= 0)
            {
                if(Score >= 0)
                {
                    double newScore = Score + Income;
                    Score = Math.Max(0, newScore); // Zorg ervoor dat de Score nooit onder nul komt
                    LbCounter.Content = Score;
                }
            }
        }

        private void UpdateScoreText() // werkt
        {
            // Lables correct houden met de juiste format en 10 mil update
            // Labels Extras
            LbIncome.Content = FormaatGrootGetal(Income);
            LbCounter.Content = FormaatGrootGetal(Score);
            LbClicked_Text.Content = FormaatLeesbaar_018_short(Clicked);
            LbUpgrades_Text.Content = FormaatLeesbaar_018_short(Bought);
            LbUsed_Text.Content = FormaatLeesbaar_018(Used);

            // labels Prijs Normale Store
            LbPrijsCursor.Content = FormaatGrootGetal_Prijs(CursorClass.Prijs);
            LbPrijsFactory.Content = FormaatGrootGetal_Prijs(FactoryClass.Prijs);
            LbPrijsFarm.Content = FormaatGrootGetal_Prijs(FarmClass.Prijs);
            LbPrijsGrandma.Content = FormaatGrootGetal_Prijs(GrandmaClass.Prijs);
            LbPrijsMine.Content = FormaatGrootGetal_Prijs(MineClass.Prijs);
            LbPrijsBank.Content = FormaatGrootGetal_Prijs(BankClass.Prijs);
            LbPrijsTemple.Content = FormaatGrootGetal_Prijs(TempleClass.Prijs);

            // Labels Prijs Bonus Store
            LbPrijsCursorBonus.Content = FormaatGrootGetal_Prijs(CursorClass.PrijsBonus);
            LbPrijsGrandmaBonus.Content = FormaatGrootGetal_Prijs(GrandmaClass.PrijsBonus);
            LbPrijsFarmBonus.Content = FormaatGrootGetal_Prijs(FarmClass.PrijsBonus);
            LbPrijsMineBonus.Content = FormaatGrootGetal_Prijs(MineClass.PrijsBonus);
            LbPrijsFactoryBonus.Content = FormaatGrootGetal_Prijs(FactoryClass.PrijsBonus);
            LbPrijsBankBonus.Content = FormaatGrootGetal_Prijs(BankClass.PrijsBonus);
            LbPrijsTempleBonus.Content = FormaatGrootGetal_Prijs(TempleClass.PrijsBonus);

            // Labels Multi Bonus Store
            LbCursorMulti.Content = CursorClass.Teller;
            LbGrandmaMulti.Content = GrandmaClass.Teller;
            LbFarmMulti.Content = FarmClass.Teller;
            LbMineMulti.Content = MineClass.Teller;
            LbFactoryMulti.Content = FactoryClass.Teller;
            LbBankMulti.Content = BankClass.Teller;
            LbTempelMulti.Content = TempleClass.Teller;
        }

        private string FormaatLeesbaar_018_short(double number) // Werkt
        {
            if (number < 1000 || number >= 1000000) // controleer als tussen deze waarden 
            {
                return number.ToString();
            }

            string formattedNumber = number.ToString("### ###.00");

            return formattedNumber;
        }

        private string FormaatLeesbaar_018(double number) // Werkt
        {
            if (number < 1000 || number >= 1000000) // controleer als tussen deze waarden 
            {
                return number.ToString("0.00");
            }

            string formattedNumber = number.ToString("### ###.00");

            return formattedNumber;
        }

        private string FormaatGrootGetal(double number) // Werkt
        {
            // Human Readable voor alles boven 1000
            string[] terms = { "", "Duizend", "Miljoen", "Miljard", "Biljoen", "Biljard", "Triljoen" };

            int index = 0;
            while (number >= 1000)
            {
                number /= 1000;
                index++;
            }

            return $"{number:F3} {terms[index]}";
        }

        private string FormaatGrootGetal_Prijs(double number) // Werkt
        {
            // Human Readable voor alles boven 1000
            string[] terms = { "", "Duizend", "Miljoen", "Miljard", "Biljoen", "Biljard", "Triljoen" };

            int index = 0;
            while (number >= 1000)
            {
                number /= 1000;
                index++;
            }

            // Prijs van de items gaat nooit een koma getal moeten voorstellen andere formating
            return $"{number:F1} {terms[index]}";
        }

        private void UpdateButtonEnabledState(Button button, double Value) // Werkt
        {
            // als de button clickable is voor de client op de juiste waarde 
            button.IsEnabled = Score >= Value;
        }

        private void UpdateInvestmentButtonVisibility() // Werkt moet optimized worden
        {
            double totalCookies = Score + Used;

            object[] classes = new object[7] { CursorClass, GrandmaClass, FarmClass, MineClass, FactoryClass, BankClass, TempleClass };

            foreach (object obj in classes)
            {
                if (obj != null)
                {
                    if (obj == CursorClass && totalCookies >= CursorClass.Prijs && !CursorClass.CursorButtonVisible)
                    {
                        BtnCursor.Visibility = Visibility.Visible;
                        BtnCursorBonus.Visibility = Visibility.Visible;
                        CursorClass.CursorButtonVisible = true;

                    }
                    if (obj == GrandmaClass && totalCookies >= GrandmaClass.Prijs && !GrandmaClass.CursorButtonVisible)
                    {
                        BtnGrandma.Visibility = Visibility.Visible;
                        BtnGrandmaBonus.Visibility = Visibility.Visible;
                        GrandmaClass.CursorButtonVisible = true;
                    }
                    if (obj == FarmClass && totalCookies >= FarmClass.Prijs && !FarmClass.CursorButtonVisible)
                    {
                        BtnFarm.Visibility = Visibility.Visible;
                        BtnFactoryBonus.Visibility = Visibility.Visible;
                        CursorClass.CursorButtonVisible = true;
                    }
                    if (obj == FarmClass && totalCookies >= FarmClass.Prijs && !CursorClass.CursorButtonVisible)
                    {
                        BtnMine.Visibility = Visibility.Visible;
                        BtnMineBonus.Visibility = Visibility.Visible;
                        FarmClass.CursorButtonVisible = true;
                    }
                    if (obj == FactoryClass && totalCookies >= FactoryClass.Prijs && !FactoryClass.CursorButtonVisible)
                    {
                        BtnFactory.Visibility = Visibility.Visible;
                        BtnFactoryBonus.Visibility = Visibility.Visible;   
                        FactoryClass.CursorButtonVisible = true;
                    }
                    if (obj == BankClass && totalCookies >= BankClass.Prijs && !BankClass.CursorButtonVisible)
                    {
                        BtnBank.Visibility = Visibility.Visible;
                        BtnBankBonus.Visibility = Visibility.Visible;
                        BankClass.CursorButtonVisible = true;
                    }
                    if (obj == TempleClass && totalCookies >= TempleClass.Prijs && !TempleClass.CursorButtonVisible)
                    {
                        BtnTemple.Visibility = Visibility.Visible;
                        BtnTempleBonus.Visibility = Visibility.Visible; 
                        TempleClass.CursorButtonVisible = true;
                    }
                }
            }
        }

        private void UpdateLabelContent(double Value, double income) // Werkt
        {
            double newScore = Score - Value;
            if (newScore >= 0)
            {
                Score = newScore;
                Income += income;
                Used += Value;
                Bought++;

                UpdateScoreText();
                UpdateInvestmentButtonVisibility();
            }
            else
            {
                MessageBox.Show("Onvoldoende cookies beschikbaar voor deze aankoop.");
            }
        }

        private void UpdateLabelContent(double Value) // Werkt
        {
            double newScore = Score - Value;
            if (newScore >= 0)
            {
                Score = newScore;
                Used += Value;
                Bought++;

                UpdateScoreText();
                UpdateInvestmentButtonVisibility();
            }
            else
            {
                MessageBox.Show("Onvoldoende cookies beschikbaar voor deze aankoop.");
            }
        }

        private void LbName_MouseDown(object sender, MouseButtonEventArgs e) // Werkt
        {
            // gebruik maken van window maken functie
            string NieuweNaam = WijzigNaamCookieClicker.Show("Bakkerij Naam wijzigen", "Voer de nieuwe bakkerij naam in:");

            // controleren als de jusite waarde is ingegeven
            if (!string.IsNullOrWhiteSpace(NieuweNaam))
            {
                LbName.Content = NieuweNaam;
            }
            else
            {
                MessageBox.Show("Ongeldige naam. De naam mag niet leeg zijn of alleen uit witruimte bestaan.");
            }
        }

        public static class WijzigNaamCookieClicker // Werkt
        {
            public static string Show(string title, string prompt)
            {

                // nieuwe Window maken
                Window window = new Window
                {
                    Title = title,
                    Width = 350,
                    Height = 250,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ResizeMode = ResizeMode.CanResize
                };

                //stackpanel voor de content van de label op te vullen met de juiste elementen voor de name change.
                StackPanel stackPanel = new StackPanel();

                System.Windows.Controls.Label label = new System.Windows.Controls.Label { Content = prompt };
                TextBox textBox = new TextBox { Margin = new Thickness(0, 15, 0, 15) };
                Button button = new Button { Content = "Opslaan" };

                button.Click += (sender, e) => window.Close();

                stackPanel.Children.Add(label);
                stackPanel.Children.Add(textBox);
                stackPanel.Children.Add(button);

                window.Content = stackPanel;

                window.ShowDialog();

                return textBox.Text;
            }
        }

        public void stackpanelImage(StackPanel stackPanel, String imageSource) // fix dit de stackpanel helemaal opvult
        {
            // gaat de image ophalen en langs elkaar zetten met een set height en width.
            Image Img = new Image();
            Img.Source = new BitmapImage(new Uri(imageSource));
            Img.Height = 32;
            Img.Width = 37;
            stackPanel.Children.Add(Img);

        }

        private void Cookie_MouseDown(object sender, RoutedEventArgs e) // Werkt 
        {
            Score++;
            Clicked++;
            UpdateScoreText();

            // mousedown op image voor COOKIECLICKER-03
            if (sender is Image image)
            {
                // animatie voor COOKIECLICKER-04 en COOKIECLICKER-05.
                ScaleTransform scaleTransform = new ScaleTransform(1.0, 1.0);
                image.RenderTransform = scaleTransform;

                DoubleAnimation animation = new DoubleAnimation(1.2, 1.0, TimeSpan.FromSeconds(0.3));
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);
            }
        }
        public void UpdateIncome(double income, double teller, double gekocht)
        {
            double nieuweIncome = teller * (gekocht * income);
            double nieuweIncomeAfnemen = gekocht * income;

            Income -= nieuweIncomeAfnemen;
            Income += nieuweIncome;
        }

        //private void Class_Button_Click(object obj, Button button, double income)
        //{
        //    var prijsProperty = obj.GetType().GetProperty("Prijs");
        //    var tellerProperty = obj.GetType().GetProperty("Teller");
        //    double prijs = (double)prijsProperty.GetValue(obj);
        //    int teller = (int)tellerProperty.GetValue(obj);

        //    obj.GetType().GetMethod("GekockteUpgrade");
        //    obj.GetType().GetMethod("PrijsVerhogen");

        //    //if (button == BtnCursor) LbCursor.Content = teller.ToString();
        //    //else if (button == BtnGrandma) LbGrandma.Content = teller.ToString();
        //    //else if (button == BtnFarm) LbFarm.Content = teller.ToString();
        //    //else if (button == BtnMine) LbMine.Content = teller.ToString();
        //    //else if (button == BtnFactory) LbFactory.Content = teller.ToString();

        //    UpdateButtonEnabledState(button, prijs);
        //    UpdateLabelContent(prijs, income);
        //}

        private void BtnCursor_Click(object sender, RoutedEventArgs e)
        {

            //Class_Button_Click(CursorClass, BtnCursor, 0.001);

            double Prijs = (double)CursorClass.Prijs;
            double income = 0.001;

            CursorClass.Gekocht++;
            CursorClass.PrijsVerhogen();

            stackpanelImage(SkpCursor, "C:\\Users\\Gebruiker\\Documents\\GitHub\\-coockie-cliker\\Coockie_Clicker\\Coockie_Clicker\\img\\Cursor.png");
            UpdateButtonEnabledState(BtnCursor, Prijs);
            UpdateLabelContent(Prijs, income);
        }

        private void BtnCursorBonus_Click(object sender, RoutedEventArgs e)
        {
            double BonusPrijs = (double)CursorClass.PrijsBonus;

            UpdateIncome(0.001, CursorClass.Teller, CursorClass.Gekocht);

            CursorClass.PrijsVerhogenBonus();
            CursorClass.GekockteBonusCursor();

            UpdateButtonEnabledState(BtnCursorBonus, BonusPrijs);
            UpdateLabelContent(BonusPrijs);
        }

        private void BtnGrandma_Click(object sender, RoutedEventArgs e)
        {

            double Prijs = (double)GrandmaClass.Prijs;
            double income = 0.01;

            GrandmaClass.Gekocht++;
            GrandmaClass.PrijsVerhogen();

            stackpanelImage(SkpGrandma, "C:\\Users\\12200178\\Desktop\\Computer Science\\Computer_Science\\Jaar_01\\Trimester_01\\Projecten\\Coockie_Clicker\\Coockie_Clicker\\img\\Grandma.png");
            UpdateButtonEnabledState(BtnGrandma, Prijs);
            UpdateLabelContent(Prijs, income);
        }

        private void BtnGrandmaBonus_Click(object sender, RoutedEventArgs e)
        {
            double BonusPrijs = (double)GrandmaClass.PrijsBonus;

            UpdateIncome(0.01, GrandmaClass.Teller, GrandmaClass.Gekocht);

            GrandmaClass.PrijsVerhogenBonus();
            GrandmaClass.GekockteBonusGrandma();

            UpdateButtonEnabledState(BtnGrandmaBonus, BonusPrijs);
            UpdateLabelContent(BonusPrijs);
        }

        private void BtnFarm_Click(object sender, RoutedEventArgs e)
        {
            double Prijs = (double)FarmClass.Prijs;
            double income = 0.08;

            FarmClass.Gekocht++;
            FarmClass.PrijsVerhogen();

            stackpanelImage(SkpFarm, "C:\\Users\\12200178\\Desktop\\Computer Science\\Computer_Science\\Jaar_01\\Trimester_01\\Projecten\\Coockie_Clicker\\Coockie_Clicker\\img\\Farm.png");
            UpdateButtonEnabledState(BtnFactory, Prijs);
            UpdateLabelContent(Prijs, income);

        }

        private void BtnFarmBonus_Click(object sender, RoutedEventArgs e)
        {
            double BonusPrijs = (double)FarmClass.PrijsBonus;

            UpdateIncome(0.08, FarmClass.Teller, FarmClass.Gekocht);

            FarmClass.PrijsVerhogenBonus();
            FarmClass.GekockteBonusFarm();

            UpdateButtonEnabledState(BtnFactoryBonus, BonusPrijs);
            UpdateLabelContent(BonusPrijs);
        }
        private void BtnMine_Click(object sender, RoutedEventArgs e)
        {
            double Prijs = (double)MineClass.Prijs;
            double income = 0.47;

            MineClass.Gekocht++;
            MineClass.PrijsVerhogen();

            stackpanelImage(SkpMine, "C:\\Users\\12200178\\Desktop\\Computer Science\\Computer_Science\\Jaar_01\\Trimester_01\\Projecten\\Coockie_Clicker\\Coockie_Clicker\\img\\Mine.png");
            UpdateButtonEnabledState(BtnFarm, Prijs);
            UpdateLabelContent(Prijs, income);
        }

        private void BtnMineBonus_Click(object sender, RoutedEventArgs e)
        {
            double BonusPrijs = (double)MineClass.PrijsBonus;

            UpdateIncome(0.47, MineClass.Teller, MineClass.Gekocht);

            MineClass.PrijsVerhogenBonus();
            MineClass.GekockteBonusMinea();

            UpdateButtonEnabledState(BtnMineBonus, BonusPrijs);
            UpdateLabelContent(BonusPrijs);
        }

        private void BtnFactory_Click(object sender, RoutedEventArgs e)
        {
            double Prijs = (double)FactoryClass.Prijs;
            double income = 2.60;

            FactoryClass.Gekocht++;
            FactoryClass.PrijsVerhogen();

            stackpanelImage(SkpFactory, "C:\\Users\\12200178\\Desktop\\Computer Science\\Computer_Science\\Jaar_01\\Trimester_01\\Projecten\\Coockie_Clicker\\Coockie_Clicker\\img\\Factory.png");
            UpdateButtonEnabledState(BtnMine, Prijs);
            UpdateLabelContent(Prijs, income);
        }

        private void BtnFactoryBonus_Click(object sender, RoutedEventArgs e)
        {
            double BonusPrijs = (double)FactoryClass.PrijsBonus;

            UpdateIncome(2.60, FactoryClass.Teller, FactoryClass.Gekocht);

            FactoryClass.PrijsVerhogenBonus();
            FactoryClass.GekockteBonusFactory();

            UpdateButtonEnabledState(BtnFactoryBonus, BonusPrijs);
            UpdateLabelContent(BonusPrijs);
        }

        private void BtnBank_Click(object sender, RoutedEventArgs e)
        {
            double Prijs = (double)BankClass.Prijs;
            double income = 14;

            BankClass.Gekocht++;
            BankClass.PrijsVerhogen();

            stackpanelImage(SkpBank, "");
            UpdateButtonEnabledState(BtnBank, Prijs);
            UpdateLabelContent(Prijs, income);
        }

        private void BtnBankBonus_Click(object sender, RoutedEventArgs e)
        {
            double BonusPrijs = (double)BankClass.PrijsBonus;

            UpdateIncome(14, BankClass.Teller, BankClass.Gekocht);

            BankClass.PrijsVerhogenBonus();
            BankClass.GekockteBonusBank();

            UpdateButtonEnabledState(BtnBankBonus, BonusPrijs);
            UpdateLabelContent(BonusPrijs);
        }

        private void BtnTemple_Click(object sender, RoutedEventArgs e)
        {
            double Prijs = (double)TempleClass.Prijs;
            double income = 78;

            TempleClass.Gekocht++;
            TempleClass.PrijsVerhogen();

            stackpanelImage(SkpTemple, "");
            UpdateButtonEnabledState(BtnTemple, Prijs);
            UpdateLabelContent(Prijs, income);
        }

        private void BtnTempleBonus_Click(object sender, RoutedEventArgs e)
        {
            double BonusPrijs = (double)TempleClass.PrijsBonus;

            UpdateIncome(78, BankClass.Teller, BankClass.Gekocht);

            TempleClass.PrijsVerhogenBonus();
            TempleClass.GekockteBonusTemple();

            UpdateButtonEnabledState(BtnTempleBonus, BonusPrijs);
            UpdateLabelContent(BonusPrijs);
        }

        private void Btn_Achievements_Click(object sender, RoutedEventArgs e) // Werkt
        {
            Achievement achievements = new Achievement();
            achievements.Show();
        }

    }
}
