# Musdis.ResponseHelpers

The Musdis.ResponseHelpers package contains useful response models.

## ⚠️ HTTP errors

HTTP errors can easily be converted into [Problem Details response](https://datatracker.ietf.org/doc/html/rfc7807).

Use `ToProblemDetails` method to create Problem Details result for Minimal APIs.

### Predefined errors:

1. `NoContentError` - a no content error, associated with status code 204, cannot be converted into Problem Details. Should only be used as a failure result of some operations (e.g. deletion).

Predefined HTTP errors:

1. `ConflictError` - a conflict with the current state of the target resource, associated with an HTTP status code 409.
1. `ForbiddenError` - an error indicating  that the server understood the request but refuses to authorize it, associated with an HTTP status code 403.
1. `GoneError` - an error indicating that access to the target resource is no longer available, associated with an HTTP status code 410.
1. `InternalServerError` - an error indicating an internal server error, associated with an HTTP status code 500.
1. `NotFoundError` - a resource not found error, associated with an HTTP status code 404.
1. `UnauthorizedError` - an unauthorized error, associated with an HTTP status code 401.
1. `ValidationError` - an error indicating a validation failure, associated with an HTTP status code 400.

### Create custom HTTP errors

If you need an HTTP error which is not available in list above, you can create it by inheriting `HttpError` class.

Override `ToProblemDetails` method if your error need a specific Problem Details response.

## 📨 Response models

`PagedResponse` is a model for pagination responses. Contains data and additional pagination info.

```json
{
    "data": [/*some_data*/],
    "paginationInfo": {
        "currentPage": 2,
        "pageSize": 25,
        "totalCount": 100,
        "totalPages": 4,
        "hasPrevious": true,
        "hasNext": true
    }
}
```
