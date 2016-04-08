# DataAccess
.NET library for interacting with databases.

This library is currently being under development.

##What, why, how?
###What?
This library is aimed at unifying access to different database engines from .NET applications.
It achieves this by setting a layer of abstraction over ADO.NET classes.
###Why
In part this project is created for fun and to get to know git. Right now I'm creating some issues just to commit fix for
them just to see, if it will be linked with issue. But still code, that is written should be ready for production (more
or less). At the moment full planned functionality doesn't exist yet, but I plan to finish it, so someday it will be
ready.

I'm writing it in my free time, so I don't promise to finish it fast.
###How?
How to use library?

You derive your data access class from a `ConnectorBase` class. The class you derive should expose `IDbConnector` instance as a parameter for it's constructor and pass it to `ConnectorBase` like below:
```
public class DataAccess : ConnectorBase
{
    public DataAccess(IDbConnector connector)
        : base(connector)
    {
    }
}
```
You can add other parameters to the constructor, but (as a good practice) you should keep this parameter.
`IDbConnector` interface defines functionalities specific to a given database engine. Those functionalities are injected to connector base and used to issue requested commands.

`IDbConnector` acts also as a transaction manager. When you want to execute methods from different classes in one transaction, just put the same `IDbConnector to each instance, and library will do the rest:
```
using (IDbConnector connector = new SqlConnector("%ConnectionString%"))
{
    try
    {
        DataAccess1 dataAccess1 = new DataAccess1(connector);
        connector.BeginTransaction(IsolationLevel.ReadCommited);
        // Execute some methods on dataAccess1 - they are all executed as a part of a single transaction.
        DataAccess2 dataAccess2 = new DataAccess2(connector);
        // Execute some methods on dataAccess2 - they are all executed as a part of the same transaction.
        connector.CommitTransaction();
    }
    catch
    {
        connector.RollbackTransaction();# DataAccess
.NET library for interacting with databases.

This library is currently being under development.

##What, why, how?
###What?
This library is aimed at unifying access to different database engines from .NET applications.
It achieves this by setting a layer of abstraction over ADO.NET classes.
###Why
In part this project is created for fun and to get to know git. Right now I'm creating some issues just to commit fix for
them just to see, if it will be linked with issue. But still code, that is written should be ready for production (more
or less). At the moment full planned functionality doesn't exist yet, but I plan to finish it, so someday it will be
ready.

I'm writing it in my free time, so I don't promise to finish it fast.
###How?
How to use library?

You derive your data access class from a `ConnectorBase` class. The class you derive should expose `IDbConnector` instance as a parameter for it's constructor and pass it to `ConnectorBase` like below:
```
public class DataAccess : ConnectorBase
{
    public DataAccess(IDbConnector connector)
        : base(connector)
    {
    }
}
```
You can add other parameters to the constructor, but (as a good practice) you should keep this parameter.
`IDbConnector` interface defines functionalities specific to a given database engine. Those functionalities are injected to connector base and used to issue requested commands.

`IDbConnector` acts also as a transaction manager. When you want to execute methods from different classes in one transaction, just put the same `IDbConnector to each instance, and library will do the rest:
```
using (IDbConnector connector = new SqlConnector("%ConnectionString%"))
{
    try
    {
        DataAccess1 dataAccess1 = new DataAccess1(connector);
        connector.BeginTransaction(IsolationLevel.ReadCommited);
        // Execute some methods on dataAccess1 - they are all executed as a part of a single transaction.
        DataAccess2 dataAccess2 = new DataAccess2(connector);
        // Execute some methods on dataAccess2 - they are all executed as a part of the same transaction.
        connector.CommitTransaction();
    }
    catch
    {
        connector.RollbackTransaction();
    }
}
```

##Future
For now library supports only Microsoft SQL Server, but I plan on adding some other database engines, when I have more time.

##License
Code of this library is provided under MIT license.
    }
}
```

##License
Code of this library is provided under MIT license.