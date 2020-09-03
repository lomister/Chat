# My Project
----
# DiscordTop
Accenture practice website

# Authors - Danilijs Rudāns

# Requirements

ASP.NET last version
ASP.NET MVC - 5.2.7

# Installation

-- $composer require lomister/Chat

# Online manual

https://docs.microsoft.com/en-us/aspnet/core/getting-started/?view=aspnetcore-3.1&tabs=windows
---------------------------------------------------------------------------------------------------------------
# Variables
----
- In general variables should be in camel case: **int xxxXxxxxXxxx** or **string queryString**
- Private variables should start with an underscore and lowercase letter: **private int _xxxxx** or **private string _connectionString**

# Methods
----
- Methods should be in camel case starting with an uppercase letter: **XxxxxXxxxxXxx()** or **CheckIfInputIsValid()**
- Method names should explain what the method does without the need of comments 

# Test methods
----
- Test methods should be in camel case starting with an uppercase letter: **XxxxxXxxxxXxxxxxXxx()** or **TestMessagesWithDatabase()**
- Test methods should start with word **test**
- Unit test method should include the name of the method you are testing
- Test methods should explain what are they testing and if the result of the test is successful or not. 
   - for example if it throw an exception: **TestLogoutGetNullException()**

# Classes
----
- Classes should be in camel case starting with an uppercase letter: **XxxxxXxxxx** or **ChatController**

# Files
----
- Files should be in camel case starting with an uppercase letter: **XxxxxXxxxx.cs** or **RegisterController.cs**
//citus

# Database
----
- Database tables and attribute should be in camel case starting with an uppercase letter: **Xxxxx** or **Message**
- ID in Database tables and attributes should be uppercase: **ID** or **SenderID**

# Personal branches
----
- Each teammate should have their own branch that has their name included and has the correct formatting like in the example: **XxxxDevBranch** or **MartinsDevBranch**
