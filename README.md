# RabbitMQ Eventbus

[<img align="right" width="100px" src="https://avatars.githubusercontent.com/u/90261798?s=200&v=4" />](https://github.com/SAI-GH-ORG)

Welcome to the RabbitMQ Eventbus project made for SAI.

The current version of RabbitMQ Eventbus is v0.1.

This library is available as package on [nuget](https://www.nuget.org/packages/RabbitEventbus)

#### Handy Links

* Documentation
  * [Get .NET](https://dotnet.microsoft.com/download/dotnet)
  * [Getting started for ASP.NET Core](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial/intro)
* Samples
  * [ASP.NET Core Sample Web API](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api)
  * [ASP.NET MVC 5 Sample App](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc)
  * [Console Application](https://docs.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio)

#### Building
To build the RabbitMQ Eventbus solution in Visual Studio, you'll need:
- Visual Studio 2019 16.3+ (or the .NET Core 3.x SDK)
  - Note: no extension is needed if building via `build.cmd` or `build.ps1` in the repository root. They pull it in via a package.

After a clone, running `build.cmd`. To create packages, use `build.cmd -CreatePackages $true` and it'll output them in the `.nukpgs\` folder.

#### Used Packages

| Package | Nuget link |
| ------- | ---- |
| Microsoft.Extensions.Logging.Abstractions| [Microsoft.Extensions.Logging.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions/5.0.0) |
| Newtonsoft.Json | [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/13.0.1) |
| Polly | [Polly](https://www.nuget.org/packages/Polly/7.2.2) |
| RabbitMQ.Client | [RabbitMQ.Client](https://www.nuget.org/packages/RabbitMQ.Client/6.2.2) |
<!-- Gen script inspiration: https://gist.github.com/NickCraver/33a825aca1fd0893ea019976a2f98850 -->

#### License
RabbitMQ Eventbus is licensed under the CC0-1.0 license (license file to be provisioned)
