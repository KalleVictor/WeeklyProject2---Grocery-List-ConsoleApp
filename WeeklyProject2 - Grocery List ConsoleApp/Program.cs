using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

CultureInfo culture = CultureInfo.InvariantCulture; //CultureInfo for the program


// Generic list added with some products for debugging
var products = new List<GroceryList>
{
   new ("Fruit", "Apples", 12),
   new ("Fruit", "Bananas", 20),
   new ("Fruit", "Oranges", 10),
   new ("Vegetable", "Tomatoes", 15),
   new ("Meat", "Sausages", 12),
};

Start();
AddProduct(products);

//If Q is entered, the program will quit and list all products even in the AddProduct function

while (true)
{
    string? command = Console.ReadLine();
    if (command == "Q")
    {
        ListAllProducts(products);
        break;
    }

    //Call add product function
    else if (command == "P")
    {
        AddProduct(products);
    }

    //Call search function
    else if (command == "S")
    {
        SearchProduct(products);
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid command! Try again!");
        Console.ResetColor();
    }
}

//Start
static void Start()
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("To enter a new product - follow the steps | To quit - enter: \"Q\"");
    Console.ResetColor();
}

//Menu
static void Menu()
{
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("To enter a new product - enter: \"P\" | To search for a product - enter: \"S\" | To quit - enter \"Q\"");
    Console.ResetColor();
}

//Exit procedure 
static void Exit(List<GroceryList> products)
{
    ListAllProducts(products);
    Environment.Exit(0);
}

//List header for the products
static void ListHeader()
{
    MenuBreak();
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine($"Category".PadRight(20) + "Name".PadRight(20) + "Price");
    Console.ResetColor();
}
static void MenuBreak()
{
    Console.WriteLine("__________________________________________________________");
}

//Function 1. Add product function
static void AddProduct(List<GroceryList> products)
{
    // Step 1: Ask for category
    Console.Write("Enter a catergory: ");
    string? category = Console.ReadLine();
    if (string.IsNullOrEmpty(category))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid category! Please enter a category.");
        Console.ResetColor();
        Menu();
        return; // Exit function if name is invalid
    }
    if (category == "Q")
    {
        Exit(products);
    }
    // Step 2: Ask for name
    Console.Write("Enter a name: ");
    string? name = Console.ReadLine();
    if (string.IsNullOrEmpty(name))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid name! Please enter a name.");
        Console.ResetColor();
        Menu();
        return;// Exit function if name is invalid
    }
    if (name == "Q")
    {
        Exit(products);
    }
    // Step 3: Ask for price
    Console.Write("Enter a price: ");
    string? priceInput = Console.ReadLine();

    //Allow exit if Q is entered
    if (priceInput == "Q")
    {
        Exit(products);
    }
    //Replace comma with dot to ensure decimal compatibility
    if (priceInput != null && priceInput.Contains(","))
        priceInput = priceInput.Replace(",", ".");

    //Check if price is a valid number (using InvariantCulture to ensure compatibility with dot as decimal separator)
    if (!double.TryParse(priceInput, NumberStyles.Float, CultureInfo.InvariantCulture, out double price) || price < 0)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid price! Please enter a number.");
        Console.ResetColor();
        Menu();
        return;// Exit function if price is invalid
    }


    GroceryList product = new GroceryList(category, name, price);
    products.Add(new GroceryList(category, name, price));

    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine("Product was successfully added!");
    Console.ResetColor();
    MenuBreak();
    Menu();
}

//Function 2. Search function with a list of found products and a list of remaining products

static void SearchProduct(List<GroceryList> products)
{
    Console.Write("Enter a Product name: ");
    string? searchName = Console.ReadLine();

    //Allow exit if Q is entered
    if (searchName == "Q")
    {
        Exit(products);
    }

    if (string.IsNullOrEmpty(searchName))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid name! Please enter a name.");
        Console.ResetColor();
        Menu();
        return; // Exit function if name is invalid
    }

    List<GroceryList> foundProducts = products.FindAll(product => product.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase));
    List<GroceryList> remainingProducts = products.Except(foundProducts).ToList();

    MenuBreak();
    ListHeader();

    foreach (var product in foundProducts)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(product.Category.PadRight(20) + product.Name.PadRight(20) + product.Price);
        Console.ResetColor();
    }

    foreach (var product in remainingProducts)
    {
        Console.WriteLine(product.Category.PadRight(20) + product.Name.PadRight(20) + product.Price);
    }
    if (foundProducts.Count > 0)
    {
        MenuBreak();
        Menu();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Product was not found!");
        Console.ResetColor();
        MenuBreak();
        Menu();
    }
}

//Function 3. QUIT & List all products function by ascending order upon quitting and summarize the total cost
static void ListAllProducts(List<GroceryList> products)
{
    ListHeader();
    products = products.OrderBy(product => product.Price).ToList();
    foreach (var product in products)
    {
        Console.WriteLine(product.Category.PadRight(20) + product.Name.PadRight(20) + product.Price);
    }
    Console.WriteLine();
    Console.WriteLine("".PadRight(20) + "Total amount: ".PadRight(20) + products.Sum(product => product.Price));
    MenuBreak();
    Menu();
}


Console.ReadLine();

//Class for the groceries
class GroceryList
{
    public GroceryList(string category, string name, double price)
    {
        Category = category;
        Name = name;
        Price = price;
    }

    public string Category { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
}