

using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Ohj2_Projekti
{
    public struct Logged
    {
        public Users user { get; set; }
        public string eventdesc { get; set; }
        public DateTime time { get; set; }
    }

    public partial class Hubpage : ContentPage
    {
        public Employee current = null;
        public Employment current2 = null;
        public Users user;
        public ObservableCollection<Employee> employee { get; set; }
        public Hubpage(Users u)
        {
            InitializeComponent();

            Title = "Pääsivu";
            employee = new ObservableCollection<Employee>();
            BindingContext = this;
            user = u;
            listaLw.BindingContext = employee;


            fileGet();
            EmploymentDisabler();
            logger("Kirjautui sisaan");
        }


        //Henkilö kentät ja napit -------------------------------------------------------------------------------------------------------------------------------
        private void NewempClicked(object sender, EventArgs e)//uusi käyttäjä tyhjentää kentät
        {
            EmptyFields();
            current = null;
            addBtn.IsEnabled = true;
            updateBtn.IsEnabled = false;
            removeBtn.IsEnabled = false;
            EmploymentDisabler();
            TmReset();
            listaTm.ItemsSource = null;
        }


        private void SaveClicked(object sender, EventArgs e)//lisaa uuden henkilon
        {
            bool b = SsnChecker(ssnEnt.Text);

            if (b)
            {
                Employee ep = new Employee(firstnameEnt.Text, lastnameEnt.Text, nicknameEnt.Text, ssnEnt.Text,
                streetEnt.Text, zipcodeEnt.Text, cityEnt.Text);
                employee.Add(ep);
                EmptyFields();
                listaLw.ItemsSource = employee;
                fileSave();
                EmploymentDisabler();
                logger($"Lisäsi henkilön {firstnameEnt.Text} {lastnameEnt.Text}");
            }
            else
            {
                Warning("Varoitus", "Henkilötunnus ei täyttänyt vaatimuksia", "OK");
                ssnEnt.Text = "";
            }
        }

        private async void removeClicked(object sender, EventArgs e)//poistaa henkilon
        {
            Boolean result = await DisplayAlert("Varoitus", "Haluatko varmasti poistaa henkilön", "Kyllä", "ei");

            if (result)
            {
                EmptyFields();
                employee.Remove(current);
                current = null;
                addBtn.IsEnabled = true;
                updateBtn.IsEnabled = false;
                removeBtn.IsEnabled = false;
                listaLw.ItemsSource = employee;
                fileSave();
                EmploymentDisabler();
                TmReset();
                logger($"Poisti henkilon {current.firstname} {current.lastname}" );
            }
        }

        private void updateClicked(object sender, EventArgs e) //päivittää henkilön tiedot
        {
            bool b = SsnChecker(ssnEnt.Text);

            if (b)
            {
                employee.Remove(current);
                current.firstname = firstnameEnt.Text;
                current.lastname = lastnameEnt.Text;
                current.nickname = nicknameEnt.Text;
                current.ssn = ssnEnt.Text;
                current.street = streetEnt.Text;
                current.zipcode = zipcodeEnt.Text;
                current.city = cityEnt.Text;
                currentLbl.Text = current.firstname + " " + current.lastname;
                logger($"Muokkasi henkilön {current.firstname} {current.lastname} tietoja");
                employee.Add(current);
                listaLw.BindingContext = employee;
                TmReset();
                fileSave();
            }
            else
            {
                Warning("Varoitus", "Henkilötunnus ei täyttänyt vaatimuksia", "OK");
                ssnEnt.Text = "";
            }

        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)//henkilön valinta
        {
            if (e.SelectedItem == null)
                return;
            Employee s = (Employee)e.SelectedItem;

            firstnameEnt.Text = s.firstname;
            lastnameEnt.Text = s.lastname;
            nicknameEnt.Text = s.nickname;
            ssnEnt.Text = s.ssn;
            streetEnt.Text = s.street;
            zipcodeEnt.Text = s.zipcode;
            cityEnt.Text = s.city;
            currentLbl.Text = s.firstname + " " + s.lastname;

            addBtn.IsEnabled = false;
            updateBtn.IsEnabled = true;
            removeBtn.IsEnabled = true;
            EmploymentEnabler();

            current = s;
            TmReset();

            listaLw.SelectedItem = null;
            //listaTm.BindingContext = current.employment;
            listaTm.ItemsSource = current.employment;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = zipcodeEnt.Text;
            if (!string.IsNullOrEmpty(searchText))
            {
                List<Employee> filteredSuggestions = employee
                    .Where(item => item.zipcode.StartsWith(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                suggestionListView.ItemsSource = filteredSuggestions;
                suggestionListView.IsVisible = true;
            }
            else
            {
                suggestionListView.IsVisible = false;
            }

        }

        private void OnSuggestionSelected(object sender, EventArgs e)
        {
            var selectedSuggestion = ((TextCell)sender).Text;
            cityEnt.Text = selectedSuggestion;
            suggestionListView.IsVisible = false;
        }

        //Toimi suhde kentät ja napit ------------------------------------------------------------------------------------------------------------------------------

        private void TmSaveClicked(object sender, EventArgs e)//lisaa uuden henkilon
        {

            Employment empt = new Employment(nimikeEnt.Text, yksikkoEnt.Text, alkamisDp.Date,
                loppumisDp.Date, loppumisSwi.IsToggled);
            current.employment.Add(empt);
            listaTm.ItemsSource = current.employment;
            employee.Remove(current);
            employee.Add(current);
            logger($"Lisasi henkilolle {current.firstname} {current.lastname} Toimisuhteen {nimikeEnt.Text} {yksikkoEnt.Text}");
            TmEmptyFields();
            fileSave();
        }

        private void TmNewempClicked(object sender, EventArgs e)//uusi toimisuhde tyhjentää kentät
        {
            TmReset();
        }

        private void TmupdateClicked(object sender, EventArgs e) //päivittää henkilön tiedot
        {
            current.employment.Remove(current2);
            current2.name = nimikeEnt.Text;
            current2.unit = yksikkoEnt.Text;
            current2.begin = alkamisDp.Date;
            current2.end = loppumisDp.Date;
            current2.endset = loppumisSwi.IsToggled;
            current.employment.Add(current2);
            employee.Remove(current);
            employee.Add(current);
            fileSave();
            TmReset();
            logger($"Paivitti henkilon {current.firstname} {current.lastname}");
        }

        private void TmOnItemSelected(object sender, SelectedItemChangedEventArgs e)//henkilön valinta
        {
            if (e.SelectedItem == null)
                return;
            Employment s = (Employment)e.SelectedItem;

            nimikeEnt.Text = s.name;
            yksikkoEnt.Text = s.unit;
            alkamisDp.Date = s.begin;
            loppumisDp.Date = s.end;
            loppumisSwi.IsToggled = s.endset;


            TmaddBtn.IsEnabled = false;
            TmupdateBtn.IsEnabled = true;
            TmremoveBtn.IsEnabled = true;

            current2 = s;
            curren2tLbl.Text = $"Valittu: {current.firstname} {current.lastname} Tiedot: {current2.name}, {current2.unit}";
            listaTm.SelectedItem = null;
        }

        private async void TmremoveClicked(object sender, EventArgs e)//poistaa toimisuhteen
        {
            Boolean result = await DisplayAlert("Varoitus", "Haluatko varmasti poistaa Toimisuhteen", "Kyllä", "ei");

            if (result)
            {
                employee.Remove(current);
                logger($"Posti henkilön {current.firstname} {current.lastname} Toimisuhteen {current2.name} {current2.unit}");
                current.employment.Remove(current2);
                employee.Add(current);
                TmReset();
                fileSave();
            }

        }

        //Tiedosto funktiot ----------------------------------------------------------------------
        private void fileSave() //Talettaa tiedon txt tiedostoon
        {
            string fileName = @"C:\Temp\HrFiles\employ.txt";
            string folderPath = @"C:\Temp\HrFiles\";

            if (!Directory.Exists(folderPath)) //tekee kansion jos sitä ei ole
            {
                Directory.CreateDirectory(folderPath);
            }
            if (!File.Exists(fileName)) // Luodaan users.json jos siitä ei ole kansio sisältää käyttäjät
            {
                File.WriteAllText(fileName, "");
            }

            try //kokoelma laitetaan tekstitiedostoon
            {
                string jsonString = JsonSerializer.Serialize(employee);
                jsonString = EncryptString("avain", jsonString);
                File.WriteAllText(fileName, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private void fileGet() //Hakee tiedot ohjelmaan
        {
            string fileName = @"C:\Temp\HrFiles\employ.txt";
            string folderPath = @"C:\Temp\HrFiles\";
            string jsonContent = "";

            if (Directory.Exists(folderPath) && File.Exists(fileName))
            {
                try
                {
                    jsonContent = File.ReadAllText(fileName);
                    jsonContent = DecryptString("avain", jsonContent);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                if (jsonContent != "")
                {
                    try
                    {
                        employee = JsonSerializer.Deserialize<ObservableCollection<Employee>>(jsonContent);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }

            try //kokoelma laitetaan tekstitiedostoon
            {
                string jsonString = JsonSerializer.Serialize(employee);
                File.WriteAllText(fileName, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            listaLw.ItemsSource = employee;
        }

        //Napit list järjestämiseen --------------------------------------------------------------------
        private void SortButtonEtunimi(object sender, EventArgs e) //Henkilön etunimella lsita järjestäminen
        {
            String s = TmbtEtunimiorder.Text;
            if (s == "▲")
            {
                employee = new ObservableCollection<Employee>(employee.OrderBy(e => e.firstname));
                TmbtEtunimiorder.Text = "▼";
            }
            else
            {
                employee = new ObservableCollection<Employee>(employee.OrderByDescending(e => e.firstname));
                TmbtEtunimiorder.Text = "▲";
            }

            listaLw.ItemsSource = employee;

        }

        private void SortButtonSukunimi(object sender, EventArgs e) //Henkilön etunimella lsita järjestäminen
        {
            String s = TmbtSukunimiorder.Text;
            if (s == "▲")
            {
                employee = new ObservableCollection<Employee>(employee.OrderBy(e => e.lastname));
                TmbtSukunimiorder.Text = "▼";
            }
            else
            {
                employee = new ObservableCollection<Employee>(employee.OrderByDescending(e => e.lastname));
                TmbtSukunimiorder.Text = "▲";
            }

            listaLw.ItemsSource = employee;

        }

        private void SortButtonNimike(object sender, EventArgs e) //Toimisuhde nimke lista järjestäminen
        {
            String s = Tmbtnnimikeorder.Text;
            if (s == "▲")
            {
                current.employment = new ObservableCollection<Employment>(current.employment.OrderBy(e => e.name));
                Tmbtnnimikeorder.Text = "▼";
            }
            else
            {
                current.employment = new ObservableCollection<Employment>(current.employment.OrderByDescending(e => e.name));
                Tmbtnnimikeorder.Text = "▲";
            }
            employee.Remove(current);
            employee.Add(current);
            listaTm.ItemsSource = current.employment;

        }


        //Kenttä muokkaajat ----------------------------------------------------------------------
        public void EmptyFields()       //tyhjentää kentät henkilöltä
        {
            firstnameEnt.Text = "";
            lastnameEnt.Text = "";
            nicknameEnt.Text = "";
            ssnEnt.Text = "";
            streetEnt.Text = "";
            zipcodeEnt.Text = "";
            cityEnt.Text = "";
            currentLbl.Text = "Uusi Henkilö";
        }

        public void TmReset()       //Tyhjentää kentät ja asettaa ohjelman uutta lisäystä varten
        {
            current2 = null;
            TmEmptyFields();
            TmaddBtn.IsEnabled = true;
            TmupdateBtn.IsEnabled = false;
            TmremoveBtn.IsEnabled = false;
            curren2tLbl.Text = "Uusi Toimisuhde";
            if(current != null)
            {
                listaTm.ItemsSource = current.employment;
            }
        }

        public void TmEmptyFields()       //tyhjentää kentät Työsuhteelata
        {
            nimikeEnt.Text = "";
            yksikkoEnt.Text = "";
            alkamisDp.Date = DateTime.Now;
            loppumisDp.Date = DateTime.Now;
            curren2tLbl.Text = "Uusi Toimisuhde";
            loppumisSwi.IsToggled = false;
        }

        public void EmploymentDisabler() //toimsuhteet otetaan poiskäytöstä
        {
            nimikeEnt.IsEnabled = false;
            yksikkoEnt.IsEnabled = false;
            alkamisDp.IsEnabled = false;
            loppumisDp.IsEnabled = false;
            TmaddBtn.IsEnabled = false;
            TmnewempBtn.IsEnabled = false;
            TmupdateBtn.IsEnabled = false;
            TmremoveBtn.IsEnabled = false;
            loppumisSwi.IsEnabled = false;
            Tmbtnnimikeorder.IsEnabled = false;
        }
        public void EmploymentEnabler() //toimsuhteet otetaan käyttöön
        {
            nimikeEnt.IsEnabled = true;
            yksikkoEnt.IsEnabled = true;
            alkamisDp.IsEnabled = true;
            loppumisDp.IsEnabled = true;
            TmaddBtn.IsEnabled = true;
            TmnewempBtn.IsEnabled = true;
            TmupdateBtn.IsEnabled = true;
            TmremoveBtn.IsEnabled = true;
            loppumisSwi.IsEnabled = true;
            Tmbtnnimikeorder.IsEnabled = true;
        }

        private void OnSwitchToggled(object sender, ToggledEventArgs e) // käsittelee Loppumis päivän Datepickerin säätö
        {
            bool isToggled = e.Value;

            if (isToggled)
            {
                loppumisDp.IsEnabled = false;
            }
            else
            {
                loppumisDp.IsEnabled = true;
            }
        }

        //Loki tieto funktiot--------------------------------------------------------------------------------------------------

        private void logger(String eventdesc) //tallentaa tapathtuma (Lokitiedot)
        {
            string fileName = @"C:\Temp\HrFiles\log.json";
            string folderPath = @"C:\Temp\HrFiles\";
            string jsonContent = "";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            List<Logged> ls = new List<Logged>();
            if (File.Exists(fileName))
            {
                try
                {
                    jsonContent = File.ReadAllText(fileName);
                    ls = JsonSerializer.Deserialize<List<Logged>>(jsonContent);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }

            Logged lg = new Logged();
            lg.user = user;
            lg.eventdesc = eventdesc;
            lg.time = DateTime.Now;
            ls.Add(lg);

            try
            {
                string jsonString = JsonSerializer.Serialize(ls);
                File.WriteAllText(fileName, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           
        }


        //Salaus menetelmät--------------------------------------------------------------------



        private string EncryptString(string key, string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] encryptedBytes = new byte[plainTextBytes.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
            {
                encryptedBytes[i] = (byte)(plainTextBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        private string DecryptString(string key, string cipherText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] decryptedBytes = new byte[cipherTextBytes.Length];

            for (int i = 0; i < cipherTextBytes.Length; i++)
            {
                decryptedBytes[i] = (byte)(cipherTextBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        //Muut funktiot--------------------------------------------------------------------------------------------------

        static bool SsnChecker(String s) //tarkistaa onko henkilö tunnus oikean 
        {
            if(s != null)
            {
                Regex henkilotunnusRegex = new Regex(@"^\d{6}-\d{3}[0-9A-FHJ-NPR-Y]$");
                return henkilotunnusRegex.IsMatch(s);
            }
            else
            {
                return false;
            }

        }

        private async void Warning(String s1, String s2, String s3) //Popup ikkuna
        {
            await DisplayAlert(s1, s2, s3);
        }
    }

}