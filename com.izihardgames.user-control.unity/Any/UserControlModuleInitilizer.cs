using System;
using IziHardGames.AppConstructor;

namespace IziHardGames.UserControl.ForUnity
{
    public abstract class UserControlModuleInitilizer
    {
        public abstract bool ResolveDependecies(IziAppModuled app);
        public abstract void LoadModule(IziAppModuled app);
        public abstract void LoadModuleEnd();
    }
}