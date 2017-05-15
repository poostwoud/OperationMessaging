# ${1:Project Name}

A request-response approach to unleash libraries for applications using WebAPI like routing.

## Installation

1. Download the repository;

2. Open it in Visual Studio;

3. See the tests.

## Usage

1. Add a reference to OperationMessaging;

2. Add a service class to the library you want to extend with OperationMessaging OR create a facade if you don't have access to the library;

3. Make the service class inherit from OperationService;

4. Add the routes to the service using it's constructor or mapping method (whatever you prefer);

5. Start using your library as an API.

## History

### 2017-05-15, second draft

### 2017-05-14, first draft

## TODO

* OperationService.Execute, support data;

## Credits

* Will Charczuk, RouteParser.cs, https://gist.github.com/wcharczuk/2284226

* markmnl, OperationResult, https://www.codeproject.com/Articles/1022462/Error-Handling-in-SOLID-Csharp-NET-The-Operation-R