namespace WireGuardApi.Domain.Exceptions;

public class ValidationConfigException(string text = "") : ArgumentNullException(ErrorCode, text)
{
    private const string ErrorCode = "Config";
}