public struct Vector2{
    public float X;
    public float Y;

    public Vector2(float x, float y){
        this.X = x;
        this.Y = y;
    }

    public static Vector2 operator + (Vector2 _v1, Vector2 _v2){
        return new Vector2(_v1.X + _v2.X, _v1.Y + _v2.Y);
    }

    public static Vector2 operator - (Vector2 _v1, Vector2 _v2){
        return new Vector2(_v1.X - _v2.X, _v1.Y - _v2.Y);
    }

    public static Vector2 operator * (Vector2 _v1, float m){
        return new Vector2(_v1.X * m, _v1.Y * m);
    }

    public static Vector2 operator / (Vector2 _v1, float d){
        return new Vector2(_v1.X / d, _v1.Y / d);
    }

    public static float distance(Vector2 _v1, Vector2 _v2){
        return (float) Math.Sqrt(Math.Pow(_v1.X - _v2.X, 2) + Math.Pow(_v1.Y - _v2.Y, 2));
    }

    public float length(){
        return (float) Math.Sqrt(X * X + Y * Y);
    }
}
