﻿# Citolab Persistence for ASP.NET Core
 
This library can be configured to use MongoDB or an in-memory database (using MemoryCache).

Packages can be installed using NuGet:
- Install-Package Citolab.Persistence

## IUnitOfWork Usage

### In-memory database
```C#
var services = new ServiceCollection();
services.AddInMemoryPersistence();

```
### MongoDB
```C#
var services = new ServiceCollection();
services.AddMongoDbPersistence("MyDatabase", Configuration.GetConnectionString("MongoDB"));

```

### Model

An entity that must be stored in the database should inherit from Model.


### API

In the example below a Web API controller that uses all methods:

```C#
public class UserController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    public UserController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    [HttpGet("{id}")]
    public Task<User> Get(Guid id) =>
        _unitOfWork.GetCollection<User>().GetAsync(id);

    [HttpGet]
    public Task<IEnumerable<User>> Get() =>
        Task.Run(() => _unitOfWork.GetCollection<User>().AsQueryable().AsEnumerable());

    [HttpPost]
    public Task<User> Post([FromBody] User user) =>
        _unitOfWork.GetCollection<User>().AddAsync(user);

    [HttpPut]
    public Task<bool> Update([FromBody] User user) =>
        _unitOfWork.GetCollection<User>().UpdateAsync(user);

    [HttpDelete("{id}")]
    public Task<bool> Delete(Guid id) =>
        _unitOfWork.GetCollection<User>().DeleteAsync(id);
}
```
### Caching

Caching can be added for Mongo usage. Add the caching attribute above your model classes; these entities will be kept in the MemoryCache. The cache will be updated when entities are changed.

```C#
[Cache(300)]
public class User : Model
{
	//properties
}
```