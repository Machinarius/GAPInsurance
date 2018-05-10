# GAPInsurance

This is my implementation of the insurance company test for GAP

## Current Progress
- Most of the functional integration tests have been written
- Business model definition code with unit tests written
- Angular client mostly finished

## TODO
- Dockerize the functional integration tests with SQL Server on Linux Docker image as the database
- Complete the funcional integration test suite
- Apply proper authorization with a [JWT role claim][2] and filter out roles with the [roles property][1]
- Proper client-side form validations using [ngForm][3]
- Proper client-side API error messages

[1]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authorization.authorizeattribute.roles?view=aspnetcore-2.1#Microsoft_AspNetCore_Authorization_AuthorizeAttribute_Roles
[2]: https://auth0.com/rules/roles-creation
[3]: https://angular.io/api/forms/NgForm
