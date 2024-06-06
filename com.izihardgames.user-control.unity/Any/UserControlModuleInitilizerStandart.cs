using IziHardGames.AppConstructor;

namespace IziHardGames.UserControl.ForUnity
{
    public class UserControlModuleInitilizerStandart : UserControlModuleInitilizer
    {
        private UserControlMonoService userControlMonoService;
        public override bool ResolveDependecies(IziAppModuled app)
        {
            return
                 app.IsModuleLoaded("eb809b3d-b383-4a46-97ac-a80d84b53ab5") && // ticking module 
                 app.IsModuleLoaded("") // UI
                 ;
        }

        public override void LoadModule(IziAppModuled app)
        {
            this.userControlMonoService = new UserControlMonoService();
        }

        public override void LoadModuleEnd()
        {

        }
    }
}