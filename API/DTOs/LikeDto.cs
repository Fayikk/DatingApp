namespace API.DTOs
{
    public class LikeDto
    {
        //this dto for if user likes other user,if user want will display ,itself like users thanks to class
    
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public string PhotoUrl { get; set; }
        public string City { get; set; }
    
    }
}