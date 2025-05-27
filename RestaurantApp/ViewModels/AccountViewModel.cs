using RestaurantApp.Commands;
using System.Windows.Input;
using System.Windows;

namespace RestaurantApp.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        private bool _isProfileVisible = true;
        private bool _isSecurityVisible;
        private bool _isPreferencesVisible;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;
        private bool _notificationsEnabled;
        private bool _newsletterEnabled;

        public AccountViewModel()
        {
            // Inițializare comenzi
            ShowProfileCommand = new RelayCommand(ShowProfile);
            ShowSecurityCommand = new RelayCommand(ShowSecurity);
            ShowPreferencesCommand = new RelayCommand(ShowPreferences);
            SaveProfileCommand = new RelayCommand(SaveProfile);
            ChangePasswordCommand = new RelayCommand(ChangePassword);
            SavePreferencesCommand = new RelayCommand(SavePreferences);
            LogoutCommand = new RelayCommand(Logout);

            // Încărcare date utilizator
            LoadUserData();
        }

        // Proprietăți pentru vizibilitate
        public bool IsProfileVisible
        {
            get => _isProfileVisible;
            set => SetProperty(ref _isProfileVisible, value);
        }

        public bool IsSecurityVisible
        {
            get => _isSecurityVisible;
            set => SetProperty(ref _isSecurityVisible, value);
        }

        public bool IsPreferencesVisible
        {
            get => _isPreferencesVisible;
            set => SetProperty(ref _isPreferencesVisible, value);
        }

        // Proprietăți pentru profil
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        // Proprietăți pentru preferințe
        public bool NotificationsEnabled
        {
            get => _notificationsEnabled;
            set => SetProperty(ref _notificationsEnabled, value);
        }

        public bool NewsletterEnabled
        {
            get => _newsletterEnabled;
            set => SetProperty(ref _newsletterEnabled, value);
        }

        // Comenzi
        public ICommand ShowProfileCommand { get; }
        public ICommand ShowSecurityCommand { get; }
        public ICommand ShowPreferencesCommand { get; }
        public ICommand SaveProfileCommand { get; }
        public ICommand ChangePasswordCommand { get; }
        public ICommand SavePreferencesCommand { get; }
        public ICommand LogoutCommand { get; }

        // Metode pentru comenzi
        private void ShowProfile()
        {
            IsProfileVisible = true;
            IsSecurityVisible = false;
            IsPreferencesVisible = false;
        }

        private void ShowSecurity()
        {
            IsProfileVisible = false;
            IsSecurityVisible = true;
            IsPreferencesVisible = false;
        }

        private void ShowPreferences()
        {
            IsProfileVisible = false;
            IsSecurityVisible = false;
            IsPreferencesVisible = true;
        }

        private void SaveProfile()
        {
            // TODO: Implementare salvare profil
            MessageBox.Show("Profilul a fost salvat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ChangePassword()
        {
            // TODO: Implementare schimbare parolă
            MessageBox.Show("Parola a fost schimbată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SavePreferences()
        {
            // TODO: Implementare salvare preferințe
            MessageBox.Show("Preferințele au fost salvate cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Logout()
        {
            // TODO: Implementare deconectare
            MessageBox.Show("V-ați deconectat cu succes!", "Deconectare", MessageBoxButton.OK, MessageBoxImage.Information);
            Application.Current.Shutdown();
        }

        private void LoadUserData()
        {
            // TODO: Implementare încărcare date utilizator din baza de date
            // Pentru moment, setăm niște date de test
            FirstName = "John";
            LastName = "Doe";
            Email = "john.doe@example.com";
            Phone = "0712345678";
            NotificationsEnabled = true;
            NewsletterEnabled = false;
        }
    }
} 