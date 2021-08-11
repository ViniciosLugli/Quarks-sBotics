public struct Vector2 {
	public float x;
	public float y;

	public Vector2(float x, float y) {
		this.x = x;
		this.y = y;
	}

	public static Vector2 operator +(Vector2 _v1, Vector2 _v2) => new Vector2(_v1.x + _v2.x, _v1.y + _v2.y);

	public static Vector2 operator -(Vector2 _v1, Vector2 _v2) => new Vector2(_v1.x - _v2.x, _v1.y - _v2.y);

	public static Vector2 operator *(Vector2 _v1, float m) => new Vector2(_v1.x * m, _v1.y * m);

	public static Vector2 operator /(Vector2 _v1, float d) => new Vector2(_v1.x / d, _v1.y / d);

	public static float distance(Vector2 _v1, Vector2 _v2) => (float)Math.Sqrt(Math.Pow(_v1.x - _v2.x, 2) + Math.Pow(_v1.y - _v2.y, 2));

	public float distanceFrom(Vector2 _v) => (float)Math.Sqrt(Math.Pow(this.x - _v.x, 2) + Math.Pow(this.y - _v.y, 2));

	public float length() => (float)Math.Sqrt(x * x + y * y);
}
