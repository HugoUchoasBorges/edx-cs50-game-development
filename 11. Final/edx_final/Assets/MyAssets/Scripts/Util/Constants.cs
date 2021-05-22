using UnityEngine;

namespace util
{
    public static class Constants
    {
        public static Vector2 ScreenBounds;
        public static Vector2 PropScreenBounds => ScreenBounds * 1.5f;
        public static Camera Camera;


        // ========================== Prefabs ============================
        public static string PREFAB_BULLET = "Prefabs/Bullet";
        public static string PREFAB_PLAYER_BULLET = "Prefabs/BulletPlayer";
        public static string PREFAB_ENEMY_BULLET = "Prefabs/BulletEnemy";

        public static string PREFAB_WEAPON = "Prefabs/Weapon";

        public static string PREFAB_COLLECTABLE = "Prefabs/Collectable";

        public static string PREFAB_PROP = "Prefabs/Prop";
        public static string PREFAB_PROP_SMALL = "Prefabs/PropSmall";
        public static string PREFAB_PROP_EXPLOSION = "Prefabs/PropExplosion";
        public static string PREFAB_PROP_EXPLOSION_SMALL = "Prefabs/PropExplosionSmall";

        public static string PREFAB_ENEMY = "Prefabs/Enemy";

    }
}