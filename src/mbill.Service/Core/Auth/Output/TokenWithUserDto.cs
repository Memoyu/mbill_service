namespace mbill.Service.Core.Auth.Output;

public class TokenWithUserDto
{
    public TokenWithUserDto(TokenDto token, LoginUserDto user)
    {
        Token = token ?? throw new ArgumentNullException(nameof(token));
        User = user ?? throw new ArgumentNullException(nameof(user));
    }

    public TokenDto Token { get; set; }

    public LoginUserDto User { get; set; }
}
