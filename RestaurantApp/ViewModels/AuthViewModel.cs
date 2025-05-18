using RestaurantApp.Models;
using RestaurantApp.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantApp.ViewModels
{
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

        public AuthViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(async () => await Login());
            RegisterCommand = new RelayCommand(async () => await Register());
            ToggleAuthModeCommand = new RelayCommand(() => IsRegistering = !IsRegistering);
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
            set => SetProperty(ref _isRegistering, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ToggleAuthModeCommand { get; }

        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Vă rugăm completați toate câmpurile obligatorii.";
                return;
            }

            var user = await _authService.LoginAsync(Email, Password);
            if (user != null)
            {
                CurrentUser = user;
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = "Email sau parolă incorectă.";
            }
        }

        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword) || string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName))
            {
                ErrorMessage = "Vă rugăm completați toate câmpurile obligatorii.";
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
                IsEmployee = false
            };

            var result = await _authService.RegisterAsync(user, Password);
            if (result != null)
            {
                CurrentUser = result;
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = "Email-ul este deja înregistrat.";
            }
        }
    }
} 