using System.Text.Json;


namespace Ohj2_Projekti
{
    public struct Users
    {
        public int id {  get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }

    

    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
            Title = "Kirjautumis sivu";
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            bool isToggled = LoginSwi.IsToggled;


            string fileName = @"C:\Temp\HrFiles\users.json";
            string folderPath = @"C:\Temp\HrFiles\";

            if (!Directory.Exists(folderPath)) //luodaan kansio hr temppiin jos sitä ei ole
            {
                Directory.CreateDirectory(folderPath);
            }
            

            if (!File.Exists(fileName)) // Loudaan users.json jos siitä ei ole kabnsio sisältää käyttäjät
            {
                List<Users> l = new List<Users>();
                Users s = new Users();
                s.id = 1;
                s.Username = "admin";
                s.Password = "admin";
                l.Add(s);
                string jsonString = JsonSerializer.Serialize(l);
                File.WriteAllText(fileName, jsonString);
            }

            string jsonContent = File.ReadAllText(fileName);
            List<Users> kayt = JsonSerializer.Deserialize<List<Users>>(jsonContent);

            if (isToggled)
            {
                if (userEnt.Text == passwordEnt.Text && passwordEnt.Text == passwordnewEnt.Text) //tarksitaa että salsanat on samat
                {
                    Users rekis = new Users();
                    rekis.id = UniquedId(kayt);
                    rekis.Username = userEnt.Text;
                    rekis.Password = passwordEnt.Text;

                    if (!CheckDuplicateUser(kayt, rekis)) //tarkistaa ettei luoda samoja käyttäjiä
                    {
                        kayt.Add(rekis);
                        string jsonString2 = JsonSerializer.Serialize(kayt);
                        File.WriteAllText(fileName, jsonString2);
                        userEnt.Text = "";
                        passwordEnt.Text = "";
                        passwordnewEnt.Text = "";
                        LoginSwi.IsToggled = false;
                        Warning("Viesti", "Käyttäjä on luoto", "OK");
                    }
                    else {
                        Warning("Varoitus", "Käyttäjä on jo olemassa", "OK");
                        userEnt.Text = "";
                        passwordEnt.Text = "";
                        passwordnewEnt.Text = "";
                    }
                    
                }
            }
            else
            {
               Boolean b = true;
               foreach (Users user in kayt)
                {
                    if(userEnt.Text == user.Username && passwordEnt.Text == user.Password)
                    {
                        userEnt.Text = "";
                        passwordEnt.Text = "";
                        passwordnewEnt.Text = "";
                        Navigation.PushAsync(new Hubpage(user));
                        b = false;
                    }
                }
               if (b)
               {
                    Warning("Warning", "Käyttäjä ei ole olemassa", "OK");
               }

            }
        }

        private void OnSwitchToggled(object sender, ToggledEventArgs e)
        {
            bool isToggled = e.Value;

            if (isToggled)      //Laittaa Rekisteröitymisen näkyviin
            {
                LoginBtn.Text = "Rekisteröidy";
                passwordnewEnt.IsVisible = true;
                newpasswordLbl.IsVisible = true;
            }
            else
            {
                LoginBtn.Text = "Kirjaudu sisään";
                passwordnewEnt.IsVisible = false;
                newpasswordLbl.IsVisible = false;
            }
        }
        public int UniquedId(List<Users> kayt)      //etsii uniikin id 
        {
            int nextId = 2; // ID 1 on adminille

            int maxId = kayt.Select(u => u.id).Max();

            for (int i = nextId; i <= maxId + 1; i++)
            {
                if (!kayt.Any(u => u.id == i))
                {
                    return i;
                }
            }

            return nextId++;
        }

        public bool CheckDuplicateUser(List<Users> kayt, Users newUser)
        {
            foreach (var existingUser in kayt)
            {
                if (existingUser.Username == newUser.Username && existingUser.Password == newUser.Password)
                {
                    return true;
                }
            }

            return false;
        }

        private async void Warning(String s1, String s2, String s3) //Popup ikkuna
        {
            await DisplayAlert(s1, s2, s3);
        }

    }
}