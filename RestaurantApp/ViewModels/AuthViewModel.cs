using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Commands;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System;
using System.Collections.ObjectModel;

namespace RestaurantApp.ViewModels
{
    public class UserRole
    {
        public string Name { get; set; }
        public bool IsEmployee { get; set; }
    }

    public class AuthViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _deliveryAddress;
        private bool _isRegistering;
        private string _errorMessage;
        private User _currentUser;
        private UserRole _selectedRole;

        public static User CurrentUserStatic { get; set; }

        public ObservableCollection<UserRole> UserRoles { get; } = new ObservableCollection<UserRole>
        {
            new UserRole { Name = "Client", IsEmployee = false },
            new UserRole { Name = "Angajat", IsEmployee = true }
        };

        public UserRole SelectedRole
        {
            get => _selectedRole;
            set => SetProperty(ref _selectedRole, value);
        }

        public event EventHandler<User> AuthenticationSuccessful;

        public AuthViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(async () => await Login());
            RegisterCommand = new RelayCommand(async () => await Register());
            ToggleAuthModeCommand = new RelayCommand(() => IsRegistering = !IsRegistering);
            SelectedRole = UserRoles[0]; // Setează rolul implicit la Client
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

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

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public string DeliveryAddress
        {
            get => _deliveryAddress;
            set => SetProperty(ref _deliveryAddress, value);
        }

        public bool IsRegistering
        {
            get => _isRegistering;
            set
            {
                SetProperty(ref _isRegistering, value);
                OnPropertyChanged(nameof(AuthButtonText));
                OnPropertyChanged(nameof(ToggleAuthButtonText));
                OnPropertyChanged(nameof(RegisterFieldsVisibility));
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(ErrorVisibility));
            }
        }

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ToggleAuthModeCommand { get; }

        // Proprietăți pentru UI fără converteri
        public string AuthButtonText => IsRegistering ? "Inregistrare" : "Autentificare";
        public string ToggleAuthButtonText => IsRegistering ? "Ai deja cont? Autentifica-te" : "Nu ai cont? Inregistreaza-te";
        public Visibility RegisterFieldsVisibility => IsRegistering ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ErrorVisibility => !string.IsNullOrEmpty(ErrorMessage) ? Visibility.Visible : Visibility.Collapsed;

        // Notifică UI-ul la schimbare

        private async Task Login()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Va rugam completati toate campurile obligatorii.";
                    return;
                }

                var user = await Task.Run(() => _authService.LoginAsync(Email, Password));
                if (user != null)
                {
                    CurrentUser = user;
                    AuthViewModel.CurrentUserStatic = user;
                    ErrorMessage = null;
                    AuthenticationSuccessful?.Invoke(this, user);
                }
                else
                {
                    ErrorMessage = "Email sau parola incorecta.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "A apărut o eroare la autentificare. Vă rugăm încercați din nou.";
                // Log the exception
            }
        }

        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword) || string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) || SelectedRole == null)
            {
                ErrorMessage = "Va rugam completati toate campurile obligatorii.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Parolele nu coincid.";
                return;
            }

            var user = new User
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = PhoneNumber,
                DeliveryAddress = DeliveryAddress,
                IsEmployee = SelectedRole.IsEmployee
            };

            var result = await _authService.RegisterAsync(user, Password);
            if (result != null)
            {
                CurrentUser = result;
                AuthViewModel.CurrentUserStatic = result;
                ErrorMessage = null;
                AuthenticationSuccessful?.Invoke(this, result);
            }
            else
            {
                ErrorMessage = "Email-ul este deja inregistrat.";
            }
        }
    }
} 