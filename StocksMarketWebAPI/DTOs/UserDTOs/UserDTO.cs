namespace StocksMarketWebAPI.DTOs.UserDTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string? Email { get; set; }

        public string? Tel { get; set; }

        public string? Role { get; set; }
    }
}
