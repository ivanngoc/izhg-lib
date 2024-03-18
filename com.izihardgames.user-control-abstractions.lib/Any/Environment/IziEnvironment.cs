using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IziHardGames.UserControl.Abstractions.NetStd21;

namespace IziHardGames.UserControl.Abstractions.NetStd21.Environments
{
    public abstract class UserEnvironmentSelectorAbstract
    {
        public abstract UserEnvironmentAbstract this[int id] { get; set; }
        public abstract UserEnvironmentAbstract this[User user] { get; set; }
        public UserEnvironmentAbstract? Current { get; set; }
        public abstract IEnumerable<UserEnvironmentAbstract?> All { get; }
        public abstract void Cleanup();
        public T As<T>() where T : UserEnvironmentSelectorAbstract => this as T ?? throw new InvalidCastException($"Current type:{GetType().FullName}. Target Type:{typeof(T).FullName}");
    }
    public static class IziEnvironment
    {
        /// <summary>
        /// Environments per <see cref="User.id"/>
        /// </summary>
        public static UserEnvironmentSelectorAbstract? Environments { get; set; }
        private static bool isInitilized;
        private static bool isSingleUser;

        public static void SetSingleUser(User user)
        {
            (Environments ?? throw new NullReferenceException()).Current = Environments[user.id];
            Environments = new SingleUserEnvironmentSelector();
            isInitilized = true;
            isSingleUser = true;
        }

        public static UserEnvironmentAbstract? GetSingleUser()
        {
            if (!isInitilized) throw new InvalidOperationException("Must be initilized");
            if (!isSingleUser) throw new InvalidOperationException("Must be initilized as Singleplayer");
            return Environments!.All.First();
        }
        public static void CleanupStatic()
        {
            Environments?.Cleanup();
            Environments = default;
        }

    }

    internal class SingleUserEnvironmentSelector : UserEnvironmentSelectorAbstract, IEnumerable<UserEnvironmentAbstract?>, IEnumerator<UserEnvironmentAbstract?>
    {
        public override UserEnvironmentAbstract this[int id] { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override UserEnvironmentAbstract this[User user] { get => environment ?? throw new NullReferenceException(); set => environment = value; }
        public override IEnumerable<UserEnvironmentAbstract?> All { get => this; }
        private UserEnvironmentAbstract? environment;
        object? IEnumerator.Current { get => environment; }
        private int counter;

        public override void Cleanup()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<UserEnvironmentAbstract?> GetEnumerator()
        {
            counter = default;
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            counter = default;
            return this;
        }

        public bool MoveNext()
        {
            counter++;
            return counter == 1;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }

    internal class MultiUserEnvironmentSelector : UserEnvironmentSelectorAbstract
    {
        private readonly Dictionary<int, UserEnvironmentAbstract> environemtnsPerUserId = new Dictionary<int, UserEnvironmentAbstract>();
        public override UserEnvironmentAbstract this[int id] { get => environemtnsPerUserId[id]; set => SetOrUpdate(id, value); }
        public override UserEnvironmentAbstract this[User user] { get => environemtnsPerUserId[user.id]; set => SetOrUpdate(user.id, value); }
        public override IEnumerable<UserEnvironmentAbstract> All => environemtnsPerUserId.Values;
        private void SetOrUpdate(int id, UserEnvironmentAbstract value)
        {
            if (environemtnsPerUserId.TryGetValue(id, out var user))
            {
                environemtnsPerUserId[id] = value;
            }
            else
            {
                environemtnsPerUserId.Add(id, value);
            }
        }
        public override void Cleanup()
        {
            environemtnsPerUserId.Clear();
        }
    }
}