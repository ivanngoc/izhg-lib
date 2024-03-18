namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    public static class ConstantsForUserControl
    {
        // Raycast layers
        public const uint RESERVED = 1u << 0;
        public const uint LAYER_RAYCASTS = 1u << 1;

        public const uint LAYER_HIT_BY_RAYCAST_UI = 1u << 2;
        public const uint LAYER_HIT_BY_RAYCAST_OBJECTS = 1u << 3;
        public const uint GROUP_HIT_BY_RAY = LAYER_HIT_BY_RAYCAST_UI | LAYER_HIT_BY_RAYCAST_OBJECTS;
    }
}
