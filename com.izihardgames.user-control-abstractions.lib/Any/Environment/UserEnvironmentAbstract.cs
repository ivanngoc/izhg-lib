
using System;
using System.Collections.Generic;
using IziHardGames.UserControl.Abstractions.NetStd21.Contexts;
using IziHardGames.UserControl.Abstractions.NetStd21.UserMods;

namespace IziHardGames.UserControl.Abstractions.NetStd21.Environments
{
    /// <summary>
    /// <see cref="IziHardGames.UserControl.Abstractions.NetStd21.Contexts.UserContext"/>
    /// </summary>
    public abstract class UserEnvironmentAbstract : IUserEnvironment
    {
        private uint frameCount;
        /// <summary>
        /// X - Right
        /// </summary>
        private double viewerPosX;
        /// <summary>
        /// Y - Up
        /// </summary>
        private double viewerPosY;
        /// <summary>
        /// Z - Forward
        /// </summary>
        private double viewerPosZ;
        protected User user;
        private readonly Dictionary<int, EnvironmentState> states = new Dictionary<int, EnvironmentState>();


        public EnvironmentState this[Type type] => states[type.GetHashCode()];
        public EnvironmentState this[int key] => states[key];


        public uint FrameCount => frameCount;
        public IEnumerable<EnvironmentState> AllStates => states.Values;
        public User User => user;
        public ContextForUserModesAbstract? Modes { get; set; }
        public double DetectionDistance { get; set; }


        protected UserEnvironmentAbstract(User user)
        {
            this.user = user;
            (IziEnvironment.Environments ?? throw new NullReferenceException())[user.id] = this;
        }

        public void AddState(int key, EnvironmentState state)
        {
            states.Add(key, state);
        }
        public void AddState(Type type, EnvironmentState state)
        {
            states.Add(type.GetHashCode(), state);
        }
        public void AddState<T>(EnvironmentState state)
        {
            states.Add(typeof(T).GetHashCode(), state);
        }
        public T As<T>() where T : UserEnvironmentAbstract => this as T ?? throw new InvalidCastException($"Current type:{GetType().FullName}. Target Type:{typeof(T).FullName}");

        protected void CollectStates()
        {
            foreach (var state in states.Values)
            {
                state.Collect();
            }
        }

        public void SetViewerPositionXYZ(double x, double y, double z)
        {
            this.viewerPosX = x; this.viewerPosY = y; this.viewerPosZ = z;
        }
        public (double, double, double) GetViewerPositionXYZ()
        {
            return (viewerPosX, viewerPosY, viewerPosZ);
        }
        public abstract T GetContext<T>() where T : UserContext;

        public void IncrementFrame()
        {
            frameCount++;
        }
    }
}