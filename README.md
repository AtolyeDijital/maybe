# A "lightweight" Maybe Monad for C#

The Maybe monad implemented here provides a way to handle optional values without resorting to explicit null checks, making your C# code cleaner, safer, and more readable.
It aims to prevent the developer from accessing the value without checking it.
This implementation supports async operations.

## NuGet Package

The Atolye.Maybe package is available as a NuGet package. You can find it on the NuGet Gallery:
[Atolye.Maybe](https://www.nuget.org/packages/Atolye.Maybe)


## Features

- **Null Safety**: Avoids null reference exceptions and encourages more functional programming patterns.
- **Access Control**: Prevents direct access to the base value without performing a control operation.
- **Async Support**: Seamlessly integrates with asynchronous programming models in .NET.
- **Simplicity**: Easy to use and understand, with a straightforward API.
- **Flexibility**: Provides methods to chain operations on the optional values, including checks and transformations, without unwrapping them.


## Usage

### Creating a Maybe instance

```csharp
// Create a Maybe containing a value
Maybe<int> maybeInt = Maybe<int>.from(5);

// Create a Maybe containing an object
Maybe<Order> order = Maybe<Order>.from(orderData);
```

### Working with Maybe

```csharp
// Perform an operation if the value exists. In this example if orderData is not null, binds another operation.
Maybe<Order> order = Maybe<Order>.from(orderData)
                            .Bind(ord => AddOrderItems(ord, itemData));


// You can make pipelines.
Maybe<Order> order = Maybe<Order>.from(orderData)
                            .Bind(ord => AddOrderItems(ord, itemData))
                            .Bind(CalculateItemSums);


// You can set error messages if binded function returns null. It throws exception with specified message.
Maybe<Order> order = Maybe<Order>.from(orderData)
                            .Bind(ord => AddOrderItems(ord, itemData), "Throw this message if AddOrderItems returns null!")
                            .Bind(CalculateItemSums);


// Access unrwapped value if it is not null, otherwise throws exception with specified message.
Order order = Maybe<Order>.from(orderData)
                            .Bind(ord => AddOrderItems(ord, itemData))
                            .Bind(CalculateItemSums)
                            .ValueOrThrow("Can not create order!");

```

### Value Checking

```csharp
// Perform checks with data based on a predicate. If check fails it returns an empty Maybe.
Maybe<Order> order = Maybe<Order>.from(orderData)
                            .Check(ord => ord.ItemCount > 0);

// If you specify error message, it will throw exception with the message.
Maybe<Order> order = Maybe<Order>.from(orderData)
                            .Check(ord => ord.ItemCount > 0, "Order must have at least 1 item!");

// You can create a validation pipeline.
Maybe<Order> order = Maybe<Order>.from(orderData)
                            .Check(ord => ord.ItemCount > 0, "Order must have at least 1 item!")
                            .Check(ord => !string.IsNullOrEmpty(ord.CustomerName), "Customer name must be specified!");

// You check null and throw exception before binding functions.
Order order = Maybe<Order>.from(orderData)
                        .CheckNull("Invalid order data!")
                        .Bind(CreateOrderItems)
                        .ValueOrThrow("Can not create order!");

// You validate input and throw exception before binding functions.
Order order = Maybe<Order>.from(orderData)
                        .Check(ValidateOrderData, "Invalid order data!")
                        .Bind(CreateOrderItems)
                        .ValueOrThrow("Can not create order!");

```

### Async Support

The library extends its functionality to asynchronous operations, allowing the `Maybe` type to be used with async/await patterns seamlessly.

```csharp

// You can use async functions for check and bind.
Order order = await Maybe<Order>.from(orderData)
                        .CheckAsync(async ord => await ValidateFromRemote(ord), "Invalid order data!")
                        .BindAsync(async ord => await CreateOrderFromRemoteService(ord))
                        .ValueOrThrow("Can not create order!");

```


## License
Copyright (c) Atölye Dijital 2022

This project is open-sourced under the MIT license. See the LICENSE file for details.
