namespace Client.Config
{
    public class ApiGatewayConfig
    {
        public string BaseUrl { get; set; } = "https://localhost:7140/gateway";
        public string ProductsFull { get; set; } = "/products/full";
        public string Category { get; set; } = "/category";
        public string Brand { get; set; } = "/brand";
        public string UsersAll { get; set; } = "/users/all";
        public string UsersCreate { get; set; } = "/users/create";
        public string UserById { get; set; } = "/users/{0}";
    }
}
