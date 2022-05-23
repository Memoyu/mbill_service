namespace mbill_service.Service.Core.Auth.Output;

public class TokenWithUserDto
{
    public TokenWithUserDto(TokenDto token, UserSimpleDto user)
    {
        Token = token ?? throw new ArgumentNullException(nameof(token));
        User = user ?? throw new ArgumentNullException(nameof(user));
    }

    public TokenDto Token { get; set; }

    public UserSimpleDto User { get; set; }
}
