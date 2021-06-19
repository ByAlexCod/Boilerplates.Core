namespace ProjectBpl.DAL.Inerfaces
{
    public interface IUserGet
    {
        int UserId { get; set; }
        string Username { get; set; }
        string Email { get; set; }
    }
}