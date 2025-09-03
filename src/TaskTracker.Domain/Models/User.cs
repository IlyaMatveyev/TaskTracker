namespace TaskTracker.Domain.Models
{
	public class User
	{
		public Guid Id { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		public string UserName { get; set; }

		public User(string userName, string passwordHash, string email)
		{
			Id = Guid.NewGuid();
			UserName = userName;
			PasswordHash = passwordHash;
			Email = email;
		}

		public User()
		{
			Id = Guid.NewGuid();
		}
	}
}
