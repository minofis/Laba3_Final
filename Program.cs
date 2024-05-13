namespace Laba;

class Program
{
    public enum Transport{
        NoTransport = 0,
        Bicycle = 1,
        Car = 2,
    }

    public enum OrderStatus{
        Waiting = 0,
        Delivering = 1,
        Delivered = 2,
    }


    public interface IDeliveryManager{
   
    }

    public class Person{
        public string Name { get; protected set; } = string.Empty;
        public string Number { get; protected set; } = string.Empty;
    }

    public class Courier : Person{
        public int Rating { get; private set; }
        public Transport Transport { get; private set; }

        public Courier(string name, string number, int rating, Transport transport){
        Name = name;
        Rating = rating;
        Number = number;
        Transport = transport;
        }
    }


    public class Client : Person{
        public string Address { get; private set; }

        public Client(string name, string number, string address){
            Name = name;
            Number = number;
            Address = address;
        }
    }


    public class Order{

        public Restaurant Restaurant { get; private set; }
        public Client Client { get; private set; }
        public Courier Courier { get; private set; }
        public List<Dish> Dishes { get; private set; }
        public OrderStatus Status { get; private set; }
        public int TotalPrice { get; private set; }

        public Order(Restaurant restaurant, Client client, Courier courier, List<Dish> dishes, OrderStatus status, int totalPrice){
            Restaurant = restaurant;
            Client = client;
            Courier = courier;
            Dishes = dishes;
            Status = status;
        }

        public string GetInfo(Restaurant restaurant, Client client, Courier courier, List<Dish> selectedDishes, OrderStatus orderStatus){
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Info by order:");
            Console.ResetColor();
            Console.WriteLine($"\nRestaurant:\n\n\t{restaurant.Name}\n\t{restaurant.Address}");
            Console.WriteLine($"\nClient:\n\n\t{client.Name}\n\t{client.Address}\n\t{client.Number}");
            Console.WriteLine($"\nCourier:\n\n\t{courier.Name}\n\t{courier.Number}\n\t{courier.Rating}\n\t{courier.Transport}");
            Console.WriteLine($"\nOrder Status:\n\n\t{orderStatus}");
            Console.WriteLine("\nDishes:");
            foreach(Dish dish in selectedDishes){
                Console.WriteLine($"\n\t{dish.Name}");
            }
            int totalPrice = 0;
            foreach (var dish in selectedDishes)
            {
                totalPrice += dish.Price;
            }
            Console.WriteLine($"\nTotal Price:\n\n\t${totalPrice}");

            Console.WriteLine("\nOK (any key) | CANCEL (0)");
            string confirm = Console.ReadLine();
            return confirm;
        }

        public void ChangeStatus(OrderStatus status){
            Status = status;
        }

    }


    public class Restaurant{
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Type { get; private set; }
        public int Rating { get; private set; }
        public List<Dish> Dishes { get; private set; }
        public Restaurant(int id, string name, string address, string type, int rating)
        {
            Id = id;
            Name = name;
            Address = address;
            Type = type;
            Rating = rating;
            Dishes = new List<Dish>();
        }

        public void AddDishes(Dish[] dishes){
            foreach (var dish in dishes)
            {
                Dishes.Add(dish);
            }
        }

        public void PrintMenu(){
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nSelect a dish from {Name}(1, 2, 3) or complete your order(0):");
            Console.ResetColor();
            foreach (var Dish in Dishes)
            {
                Console.WriteLine($"\n Id: {Dish.Id} \n Dish: {Dish.Name} \n Description: {Dish.Description} \n Price: ${Dish.Price}");
            }
        }
    }

    public class Dish{
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public int Price { get; private set; }

        public Dish(int id, string name, string description, int price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }
    }

    public class DeliveryManager : IDeliveryManager{
        public Courier GetBestCourier(Courier[] couriers){
            Courier bestCourier = new(" ", " ", 0, Transport.NoTransport);
            foreach (var courier in couriers)
            {
                if(bestCourier.Rating < courier.Rating){
                    bestCourier = courier;
                }
            }
            return bestCourier;
        }

        public int CalculateDeliveryTime(string clientAddress)
        {
            int deliveryTime = clientAddress.Length + 1;
            Console.WriteLine($"\nDelivery time: {deliveryTime} seconds");
            return deliveryTime;
        }

        public void DeliveryProgress(int deliveryTime){
            for (int i = deliveryTime; i > 0; i--)
            {
                Console.WriteLine($"{i} seconds left");
                Thread.Sleep(1000);
            }
        }
    }

    public class MockTester{
        public static Restaurant GetMockRestaurantData(int RestaurantId){
            if(RestaurantId == 1){
                return new Restaurant(1, "Osama", "Via Monte Carlo 44", "Sushiya", 5);
            }
            else if(RestaurantId == 2){
                return new Restaurant(2, "Dominos", "Via Delo Rosa 24", "Pizzeria", 4);
            }
            else if(RestaurantId == 3){
                return new Restaurant(3, "Aroma Kava", "Via Sala Mandra 51", "Cafe", 3);
            }
            else
            {
                return null;
            }
        }

        public static Dish[] GetMockDishesData(int DishesId){
            if(DishesId == 1){
                return
                [
                    new(1, "Philadelphia", "Sushi made from smoked salmon, cream cheese and cucumber", 7),
                    new(2, "California", "Sushi made with rice turned inside out", 6),
                    new(3, "Maki", "Sushi that consists of fish, vegetables, rice and rolled up in a seaweed", 5),
                ];
            }
            if(DishesId == 2){
                return
                [
                    new(1, "Capricciosa", "Pizza made with mozzarella cheese, baked ham, mushrooms, artichokes and tomatoes", 8),
                    new(2, "Neapolitan", "Pizza made with tomatoes and mozzarella cheese", 7),
                    new(3, "Margarita", "Pizza with crushed and peeled tomatoes, mozzarella, fresh basil leaves, and olive oil", 7),
                ];
            }
            if(DishesId == 3){
                return
                [
                    new(1,"Cappuccino", "Espresso-based coffee drink with added milk", 3),
                    new(2,"Espresso", "Coffee drink based on hot water with ground coffee", 4),
                    new(3,"Latte", "Milk-based coffee drink", 2),
                ];
            }
            else
            {
                return null;
            }
        }

        public static Courier[] GetMockCouriersData(){
            return
            [
                new("Carl", "+380353456789", 3, Transport.Bicycle),
                new("Poco", "+380564563453", 1, Transport.NoTransport),
                new("Sheli", "+380454353534", 2, Transport.NoTransport),
                new("Pablo", "+380123456789", 5, Transport.Car),
                new("Leon", "+380545353543", 4, Transport.Bicycle),
            ];
        }
    }

    public static string ReadInfo(string input){
        string info;
        do
        {
            Console.WriteLine($"Enter your {input}: ");
            info = Console.ReadLine();

            if(info == string.Empty){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Can't be empty here!");
                Console.ResetColor();
                continue;
            }
        } while (info == string.Empty);

        return info;
    }

    public static int ReadRestaurantId(List<Restaurant> restaurants){
        string restaurantId;
        do
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Select a restaurant(1, 2, 3):");
            Console.ResetColor();
            foreach (var restaurant in restaurants)
            {
            Console.WriteLine($"\n{restaurant.Id}) {restaurant.Name}");
            }
            restaurantId = Console.ReadLine();

            if(restaurantId != "1" && restaurantId != "2" && restaurantId != "3"){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error, try again!");
                Console.ResetColor();
                continue;
            }
        } while (restaurantId != "1" && restaurantId != "2" && restaurantId != "3");

        return int.Parse(restaurantId);
    }

    public static int ReadDishId(Restaurant restaurant){
        string dishId;
        do
        {
            restaurant?.PrintMenu();
            
            dishId = Console.ReadLine();

            if(dishId != "1" && dishId != "2" && dishId != "3" && dishId != "0"){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error, try again!");
                Console.ResetColor();
                continue;
            }
        } while (dishId != "1" && dishId != "2" && dishId != "3" && dishId != "0");

        return int.Parse(dishId);
    }

    static void Main(string[] args)
    { 
        // Restaurants with Dishes

        Restaurant sushiya = MockTester.GetMockRestaurantData(1);
        sushiya.AddDishes(MockTester.GetMockDishesData(1));
        Restaurant pizzeria = MockTester.GetMockRestaurantData(2);
        pizzeria.AddDishes(MockTester.GetMockDishesData(2));
        Restaurant cafe = MockTester.GetMockRestaurantData(3);
        cafe.AddDishes(MockTester.GetMockDishesData(3));

        List<Restaurant> restaurants = [sushiya, pizzeria, cafe];


        // New Client

        string name = "name";
        string number = "number";
        string address = "address";


        name = ReadInfo(name);
        number = ReadInfo(number);
        address = ReadInfo(address);

        
        Client client = new Client(name, number, address);


        // Restaurant Selection

        int restaurantId = ReadRestaurantId(restaurants);

        Restaurant restaurant = restaurants.Find(r => r.Id == restaurantId);


        int dishId = 1;
        
        List<Dish> selectedDishes = [];

        while (dishId != 0)
        {


        // Dish Selection

        List<Dish> dishes = restaurant.Dishes;

        dishId = ReadDishId(restaurant);

        if(dishId == 0){
            break;
        }

        Dish dish = dishes.Find(d => d.Id == dishId);


        // Selected Dishes
        
        selectedDishes.Add(dish);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{dish.Name} was added to order");
        Console.ResetColor();
        }


        // Delivery Manager

        DeliveryManager delivery = new();
        
        
        // Courier Search

        Courier[] couriers = MockTester.GetMockCouriersData();
        Courier courier= delivery.GetBestCourier(couriers);


        // Order

        Order order = new(restaurant, client, courier, selectedDishes, OrderStatus.Waiting, 10);

        string confirm = order.GetInfo(restaurant, client, courier, selectedDishes, OrderStatus.Waiting);

        if(confirm == "0"){
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ORDER IS CANCELED");
            Console.ResetColor();
            return;
        }

        order.ChangeStatus(OrderStatus.Delivering);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Order Status:\n\n\t{order.Status}");
        Console.ResetColor();

        // Delivery Progress

        int deliveryTime = delivery.CalculateDeliveryTime(client.Address);

        delivery.DeliveryProgress(deliveryTime);

        order.ChangeStatus(OrderStatus.Delivered);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\nOrder Status:\n\n\t{order.Status}");
        Console.ResetColor();
    }
}
