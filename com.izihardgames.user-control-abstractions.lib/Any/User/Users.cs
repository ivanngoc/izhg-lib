using System.Collections.Generic;
using IziHardGames.UserControl.Abstractions.NetStd21;
using IziHardGames.UserControl.Abstractions.NetStd21.Attributes;

namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    [Registry]
    public static class Users
    {
        public static User Curernt { get; set; }
        public static readonly Dictionary<int, User> users = new Dictionary<int, User>();
        public static IEnumerable<User> All => users.Values;

        public static void RegistUser(User user)
        {
            users.Add(user.id, user);
        }

        public static void SetCurrent(User user)
        {
            Curernt = user;
        }
    }
}