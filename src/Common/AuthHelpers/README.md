# AuthHelpers

The ResponseHelpers package contains common authentication and authorization code.

## Useful extensions

Use following extension methods to reduce code duplication:

- `AddJwtAuthorization` for `SwaggerGenOptions` class. This method adds authentication scheme to Swagger UI.

- `AddCommonAuthentication` for `IServiceCollection`. This method adds common authentication to your DI container.

## Options

`JwtOptions` is an option class for your JWT configuration that is identical among all microservices.

Jwt configuration JSON scheme: 

```json
"YourJwtSettingsName": {
    "Issuer": "YourIssuer",
    "Audience": "YourAudience",
    "Key": "YourJwtKey"
}
```

