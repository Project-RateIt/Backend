using rateit.Helpers;

namespace rateit.User;

[Serializable]
public class SettingsRequestModel : Request
{
    public SettingsRequestModel(int id, SettingsMode mode, object value, string token) : base(token)
    {
        Id = id;
        Mode = mode;
        Value = value;
    }

    public int Id { get; }
    public SettingsMode Mode { get; }
    public object Value { get; }
}