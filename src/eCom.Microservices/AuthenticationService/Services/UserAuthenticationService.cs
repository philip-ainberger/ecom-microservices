using AuthenticationService.Database;
using AuthenticationService.Database.Entities;
using AuthenticationService.Dtos;
using AuthenticationService.Metrics;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Base.EntityFrameworkCore;

namespace AuthenticationService.Services;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly AuthDbContext _dbContext;
    private readonly ILogger<UserAuthenticationService> _logger;
    private readonly IAuthenticationMetrics _metrics;

    public UserAuthenticationService(ILogger<UserAuthenticationService> logger, 
        IDbContextProvider<AuthDbContext> dbContextProvider,
        IAuthenticationMetrics metrics)
    {
        _dbContext = dbContextProvider.ProvideContext();
        _logger = logger;
        _metrics = metrics;
    }

    public async Task<Result> SignUpAsync(SignUpDto signUp, CancellationToken cancellationToken)
    {
        var emailInUse = await _dbContext.Users.AnyAsync(c => c.Email == signUp.Email, cancellationToken);

        if (emailInUse)
        {
            _logger.LogInformation("[SignUpAsync] Provided e-mail already in use!");
            return Result.Fail("Email already in use!");
        }

        await _dbContext.Users.AddAsync(signUp.ToUserEntity(), cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _metrics.SignedUp();
        return Result.Ok();
    }

    public async Task<Result> LoginAsync(LoginDto login, CancellationToken cancellationToken)
    {
        var userEntity = await _dbContext.Users.FirstOrDefaultAsync(c => c.Email == login.Email.ToLower(), cancellationToken);

        if (userEntity == null)
            return Result.Fail("");

        var hasher = new PasswordHasher<UserEntity>();
        var passwordResult = hasher.VerifyHashedPassword(userEntity, userEntity.HashedPassword, login.Password);

        _metrics.LoggedIn();

        return passwordResult switch
        {
            PasswordVerificationResult.Failed => Result.Fail(""),
            PasswordVerificationResult.Success => Result.Ok(),
            PasswordVerificationResult.SuccessRehashNeeded => Result.Ok(),
            _ => throw new NotImplementedException()
        };
    }
}
