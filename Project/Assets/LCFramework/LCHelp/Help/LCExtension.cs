using UnityEngine;

namespace LCHelp
{
    public static class LCExtension
    {
        #region tostring

        public static string LCToString(this Vector2 vector)
        {
            return string.Format("({0},{1})", vector.x, vector.y);
        }

        public static string LCToString(this Vector2Int vector)
        {
            return string.Format("({0},{1})", vector.x, vector.y);
        }

        public static string LCToString(this Vector3 vector)
        {
            return string.Format("({0},{1},{2})", vector.x, vector.y, vector.z);
        }

        public static string LCToString(this Vector3Int vector)
        {
            return string.Format("({0},{1},{2})", vector.x, vector.y, vector.z);
        }

        public static string LCToString(this Vector4 vector)
        {
            return string.Format("({0},{1},{2},{3})", vector.x, vector.y, vector.z, vector.w);
        }

        #endregion

        #region tovectorInt

        public static Vector2Int ToVectorInt(this Vector2 vector)
        {
            return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
        }

        #endregion

        public static string ToString(object value, string fullName)
        {
            if (fullName == typeof(Vector2).FullName)
            {
                return ((Vector2)value).LCToString();
            }
            else if (fullName == typeof(Vector2Int).FullName)
            {
                return ((Vector2Int)value).LCToString();
            }
            else if (fullName == typeof(Vector3).FullName)
            {
                return ((Vector3)value).LCToString();
            }
            else if (fullName == typeof(Vector3Int).FullName)
            {
                return ((Vector3Int)value).LCToString();
            }
            else if (fullName == typeof(Vector4).FullName)
            {
                return ((Vector4)value).LCToString();
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
