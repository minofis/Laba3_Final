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
        public void DeliveryProgress(int deliveryTime, Order order);
        public int CalculateDeliveryTime(string clientAddress);
        public Courier GetBestCourier(Courier[] couriers);
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

        public void GetInfo(){
            Console.WriteLine($"\nCourier:\n\n\t{Name}\n\t{Number}\n\t{Rating}\n\t{Transport}");
        }
    }


    public class Client : Person{
        public string Address { get; private set; }

        public Client(string name, string number, string address){
            Name = name;
            Number = number;
            Address = address;
        }

        public void GetInfo(){
            Console.WriteLine($"\nClient:\n\n\t{Name}\n\t{Address}\n\t{Number}");
        }
    }


    public class Order{

        public Restaurant Restaurant { get; private set; }
        public Client Client { get; private set; }
        public Courier Courier { get; private set; }
        public List<Dish> Dishes { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Waiting;
        public int TotalPrice { get; private set; }

        public Order(Restaurant restaurant, Client client, Courier courier, List<Dish> dishes, OrderStatus status, int totalPrice){
            Restaurant = restaurant;
            Client = client;
            Courier = courier;
            Dishes = dishes;
            Status = status;
        }

        public string GetInfo(Restaurant restaurant, Client client, Courier courier, List<Dish> selectedDishes, Order order){
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Info by order:");
            Console.ResetColor();
            restaurant.GetInfo();
            client.GetInfo();
            courier.GetInfo();
            order.GetStatus();
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
            string confirm = "1";
            return confirm;
        }

        public void GetStatus(){
            Console.WriteLine($"\nOrder Status:\n\n\t{Status}");
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
            foreach (Dish Dish in Dishes)
            {
                Console.WriteLine($"\n Id: {Dish.Id} \n Dish: {Dish.Name} \n Description: {Dish.Description} \n Price: ${Dish.Price}");
            }
        }

        public void GetInfo(){
            Console.WriteLine($"\nRestaurant:\n\n\t{Id}\n\t{Name}\n\t{Address}\n\t{Rating}\n\t{Type}");
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

        public void DeliveryProgress(int deliveryTime, Order order){
            for (int i = deliveryTime; i > 0; i--)
            {
                Console.WriteLine($"{i} seconds left");
                Thread.Sleep(1000);
            }

            order.ChangeStatus(OrderStatus.Delivered);
            Console.ForegroundColor = ConsoleColor.Yellow;
            order.GetStatus();
            Console.ResetColor();
        }
    }

    public class MockTester{
        public static Restaurant GetMockRestaurantData(int RestaurantId){
            if(RestaurantId == 1){
                return new Restaurant(1, "Osama", "3689 Walton Street", "Sushiya", 5);
            }
            else if(RestaurantId == 2){
                return new Restaurant(2, "Dominos", "3104 Bastin Drive", "Pizzeria", 4);
            }
            else if(RestaurantId == 3){
                return new Restaurant(3, "Aroma Kava", "1558 Washington Avenue", "Cafe", 3);
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

        private static readonly Random random = new();

        public static Restaurant GetRandomRestaurant()
        {        
            Restaurant sushiya = MockTester.GetMockRestaurantData(1);
            sushiya.AddDishes(MockTester.GetMockDishesData(1));
            Restaurant pizzeria = MockTester.GetMockRestaurantData(2);
            pizzeria.AddDishes(MockTester.GetMockDishesData(2));
            Restaurant cafe = MockTester.GetMockRestaurantData(3);
            cafe.AddDishes(MockTester.GetMockDishesData(3));

            List<Restaurant> restaurants = [sushiya, pizzeria, cafe];

            int randomRestaurantId = random.Next(1, 4);
            return restaurants.Find( r => r.Id == randomRestaurantId);
        }

        public static Dish GetRandomDish(Restaurant randomRestaurant)
        {
            return randomRestaurant.Dishes[random.Next(0, 3)];
        }

        public static Order GetRandomOrder()
        {
            DeliveryManager manager = new();

            Restaurant randomRestaurant = GetRandomRestaurant();

            Client randomClient = new("Artem", "+380663421243", "3342 Mozar Ave");

            Courier[] couriers = GetMockCouriersData();
            Courier bestCourier = manager.GetBestCourier(couriers);

            List<Dish> randomDishes = [];
            for (int i = 0; i < 3; i++)
            {
                randomDishes.Add(GetRandomDish(randomRestaurant));
            }

            return new Order(randomRestaurant, randomClient, bestCourier, randomDishes, OrderStatus.Waiting, 10);
        }
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

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Select a restaurant(1, 2, 3):");
        Console.ResetColor();
        foreach (var restaurant in restaurants)
        {
            restaurant.GetInfo();
        }


        // Delivery Manager

        DeliveryManager delivery = new();


        // Order

        Order order = MockTester.GetRandomOrder();

        order.Restaurant.PrintMenu();

        string confirm = order.GetInfo(order.Restaurant, order.Client, order.Courier, order.Dishes, order);

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

        int deliveryTime = delivery.CalculateDeliveryTime(order.Client.Address);

        delivery.DeliveryProgress(deliveryTime, order);
    }
}
