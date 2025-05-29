using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using RestaurantApp.Commands;
using RestaurantApp.Models;
using RestaurantApp.Services;
using System.Windows.Controls; // Add this namespace for Orientation

namespace RestaurantApp.ViewModels
{
    public class AddProductViewModel : ViewModelBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IAllergenService _allergenService;
        private string _name;
        private decimal _weight;
        private decimal _price;
        private string _categoryName;
        private string _newAllergenName;
        private BitmapImage _productImage;
        private string _errorMessage;
        private bool _hasError;
        private bool _hasImage;
        private Category _selectedCategory;
        private Allergen _selectedAllergen;
        private string _ingredients;

        public AddProductViewModel(
            IProductService productService,
            ICategoryService categoryService,
            IAllergenService allergenService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _allergenService = allergenService;

            SaveCommand = new RelayCommand(async () => await SaveProduct());
            CancelCommand = new RelayCommand(() => CloseWindow());
            SelectImageCommand = new RelayCommand(SelectImage);
            AddNewCategoryCommand = new RelayCommand(async () => await AddNewCategory());
            AddNewAllergenCommand = new RelayCommand(async () => await AddNewAllergen());
            RemoveAllergenCommand = new RelayCommand<AllergenViewModel>(RemoveAllergen);

            _ = LoadInitialData();
        }

        public AddProductViewModel(
            IProductService productService,
            ICategoryService categoryService,
            IAllergenService allergenService,
            Product productToEdit)
            : this(productService, categoryService, allergenService)
        {
            if (productToEdit != null)
            {
                Name = productToEdit.Name;
                Weight = productToEdit.Weight;
                Price = productToEdit.Price;
                Ingredients = productToEdit.Ingredients;
                CategoryName = productToEdit.Category?.Name;
                SelectedCategory = productToEdit.Category;
                if (!string.IsNullOrEmpty(productToEdit.ImageUrl))
                {
                    var imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, productToEdit.ImageUrl.TrimStart('/', '\\'));
                    if (File.Exists(imagePath))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(imagePath);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        ProductImage = bitmap;
                    }
                }
                if (productToEdit.Allergens != null)
                {
                    foreach (var allergen in productToEdit.Allergens)
                    {
                        AddSelectedAllergen(allergen);
                    }
                }
            }
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public decimal Weight
        {
            get => _weight;
            set
            {
                if (decimal.TryParse(value.ToString(), out var parsedWeight))
                {
                    SetProperty(ref _weight, parsedWeight);
                    ErrorMessage = string.Empty; // Clear error if parsing is successful
                }
                else
                {
                    // Handle parsing error, maybe set default to 0 or show error message
                    SetProperty(ref _weight, 0); // Set to 0 on invalid input
                    // ErrorMessage = "Gramaj invalid."; // Optionally show error
                }
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                 if (decimal.TryParse(value.ToString(), out var parsedPrice))
                {
                    SetProperty(ref _price, parsedPrice);
                     ErrorMessage = string.Empty; // Clear error if parsing is successful
                }
                else
                {
                    // Handle parsing error, maybe set default to 0 or show error message
                    SetProperty(ref _price, 0); // Set to 0 on invalid input
                    // ErrorMessage = "Pret invalid."; // Optionally show error
                }
            }
        }

        public string CategoryName
        {
            get => _categoryName;
            set => SetProperty(ref _categoryName, value);
        }

        public string NewAllergenName
        {
            get => _newAllergenName;
            set => SetProperty(ref _newAllergenName, value);
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                if (value != null)
                {
                    CategoryName = value.Name;
                }
            }
        }

        public Allergen SelectedAllergen
        {
            get => _selectedAllergen;
            set
            {
                SetProperty(ref _selectedAllergen, value);
                if (value != null)
                {
                    AddSelectedAllergen(value);
                }
            }
        }

        public BitmapImage ProductImage
        {
            get => _productImage;
            set
            {
                SetProperty(ref _productImage, value);
                HasImage = value != null;
            }
        }

        public bool HasImage
        {
            get => _hasImage;
            set => SetProperty(ref _hasImage, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                HasError = !string.IsNullOrEmpty(value);
            }
        }

        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        public string Ingredients
        {
            get => _ingredients;
            set => SetProperty(ref _ingredients, value);
        }

        public ObservableCollection<Category> ExistingCategories { get; } = new ObservableCollection<Category>();
        public ObservableCollection<Allergen> ExistingAllergens { get; } = new ObservableCollection<Allergen>();
        public ObservableCollection<AllergenViewModel> SelectedAllergens { get; } = new ObservableCollection<AllergenViewModel>();

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SelectImageCommand { get; }
        public ICommand AddNewCategoryCommand { get; }
        public ICommand AddNewAllergenCommand { get; }
        public ICommand RemoveAllergenCommand { get; }

        private async Task LoadInitialData()
        {
            await LoadCategories();
            await LoadAllergens();
        }

        private async Task LoadCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ExistingCategories.Clear();
                foreach (var category in categories)
                {
                    ExistingCategories.Add(category);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Eroare la incarcarea categoriilor: " + ex.Message;
            }
        }

        private async Task LoadAllergens()
        {
            try
            {
                var allergens = await _allergenService.GetAllAllergensAsync();
                ExistingAllergens.Clear();
                foreach (var allergen in allergens)
                {
                    ExistingAllergens.Add(allergen);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Eroare la incarcarea alergenilor: " + ex.Message;
            }
        }

        private async Task AddNewCategory()
        {
            if (string.IsNullOrWhiteSpace(CategoryName))
            {
                ErrorMessage = "Va rugam introduceti numele categoriei.";
                return;
            }

            try
            {
                var existingCategory = ExistingCategories.FirstOrDefault(c => 
                    c.Name.Equals(CategoryName, StringComparison.OrdinalIgnoreCase));

                if (existingCategory != null)
                {
                    SelectedCategory = existingCategory;
                    return;
                }

                var newCategory = new Category { Name = CategoryName };
                var createdCategory = await _categoryService.CreateCategoryAsync(newCategory);
                
                if (createdCategory != null)
                {
                    ExistingCategories.Add(createdCategory);
                    SelectedCategory = createdCategory;
                    CategoryName = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Eroare la adaugarea categoriei: " + ex.Message;
            }
        }

        private async Task AddNewAllergen()
        {
            if (string.IsNullOrWhiteSpace(NewAllergenName))
            {
                ErrorMessage = "Va rugam introduceti numele alergenului.";
                return;
            }

            try
            {
                var existingAllergen = ExistingAllergens.FirstOrDefault(a => 
                    a.Name.Equals(NewAllergenName, StringComparison.OrdinalIgnoreCase));

                if (existingAllergen != null)
                {
                    AddSelectedAllergen(existingAllergen);
                    return;
                }

                var newAllergen = new Allergen { Name = NewAllergenName };
                var createdAllergen = await _allergenService.CreateAllergenAsync(newAllergen);
                
                if (createdAllergen != null)
                {
                    ExistingAllergens.Add(createdAllergen);
                    AddSelectedAllergen(createdAllergen);
                    NewAllergenName = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Eroare la adaugarea alergenului: " + ex.Message;
            }
        }

        private void AddSelectedAllergen(Allergen allergen)
        {
            if (!SelectedAllergens.Any(a => a.Name.Equals(allergen.Name, StringComparison.OrdinalIgnoreCase)))
            {
                SelectedAllergens.Add(new AllergenViewModel { Name = allergen.Name });
            }
        }

        private void RemoveAllergen(AllergenViewModel allergen)
        {
            if (allergen != null)
            {
                SelectedAllergens.Remove(allergen);
            }
        }

        private void SelectImage()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Imagini|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                Title = "Selecteaza imaginea produsului"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var image = new BitmapImage(new Uri(dialog.FileName));
                    ProductImage = image;
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Eroare la incarcarea imaginii: " + ex.Message;
                }
            }
        }

        private async Task SaveProduct()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name) || Weight <= 0 || Price <= 0 || SelectedCategory == null)
                {
                    ErrorMessage = "Va rugam completati toate campurile obligatorii (nume, gramaj, pret si categorie).";
                    return;
                }

                var product = new Product
                {
                    Name = Name,
                    Weight = Weight,
                    Price = Price,
                    CategoryId = SelectedCategory.CategoryId,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    Ingredients = Ingredients ?? ""
                };

                product.Allergens.Clear();
                var dbContext = (_productService as ProductService)?.Context;
                if (dbContext != null)
                {
                    foreach (var allergenViewModel in SelectedAllergens)
                    {
                        var existingAllergen = dbContext.Allergens
                            .FirstOrDefault(a => a.Name == allergenViewModel.Name);
                        if (existingAllergen != null && !product.Allergens.Contains(existingAllergen))
                        {
                            product.Allergens.Add(existingAllergen);
                        }
                    }
                }

                if (ProductImage != null)
                {
                    var imagePath = await SaveImage(ProductImage);
                    product.ImageUrl = imagePath;
                }

                var result = await _productService.CreateProductAsync(product);
                if (result != null)
                {
                    MessageBox.Show("Produsul a fost adaugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseWindow();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Eroare la salvarea produsului: " + ex.Message + (ex.InnerException != null ? " | " + ex.InnerException.Message : "");
            }
        }

        private async Task<string> SaveImage(BitmapImage image)
        {
            try
            {
                var imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                var fileName = $"{Guid.NewGuid()}.jpg";
                var filePath = Path.Combine(imagesFolder, fileName);

                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    encoder.Save(stream);
                }

                return $"/Images/{fileName}";
            }
            catch (Exception ex)
            {
                throw new Exception("Eroare la salvarea imaginii: " + ex.Message);
            }
        }

        private void CloseWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            window?.Close();
        }
    }

    public class AllergenViewModel : ViewModelBase
    {
        private string _name;
        private bool _isSelected;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
} 