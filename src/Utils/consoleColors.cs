static string BOLD(string _str) => $"<b>{_str.ToString()}</b>";

static string UNDERLINE(string _str) => $"<u>{_str.ToString()}</u>";

static string ITALIC(string _str) => $"<i>{_str.ToString()}</i>";

static string RESIZE(string _str, float _size) => $"<size={_size}>{_str.ToString()}</size>";

static string COLOR(string _str, string _color) => $"<color={_color}>{_str.ToString()}</color>";
static string COLOR(string _str, Color _color) => $"<color={_color.toHex()}>{_str.ToString()}</color>";

static string ALIGN(string _str, string _alignment) => $"<align=\"{_alignment}\">{_str.ToString()}";
