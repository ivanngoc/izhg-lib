namespace IziHardGames.Ticking.Lib.ApplicationLevel
{
	public class ConstantsCore
	{
		public const int GROUPE_BOOTSTRAP = int.MinValue;
		public const int GROUPE_CORE = 0;
		public const int GROUPE_INPUT = 500;
		public const int GROUPE_SCENE_ENTERENCE = 10;

		// layers 
		public const int LAYER_NOTHING = 0;                 //Layer 0
		public const int LAYER_EVRYTHING = -1;              //NOT LAYER DISPLAYED

		public const int LAYER_DEFAULT = 1;                 //Layer 0
		public const int LAYER_TRANSPARENT_FX = 2;          //Layer 1
		public const int LAYER_IGNORE_RAYCAST = 4;          //Layer 2
		public const int LAYER_GHOST = 8;                   //Layer 3    
		public const int LAYER_WATER = 16;                  //Layer 4   
		public const int LAYER_UI = 32;                     //Layer 5
		public const int LAYER_IGNORE_UI = 64;              //Layer 6
		public const int LAYER_NO_RENDERING = 128;          //Layer 7


		public const int RENDERERS_CONTROL_CAPACITY = 10000;
	}
}