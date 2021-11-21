# Data class library

Should contain database implementation code e.g.
- Entity Framework DB Context classes (you may use multiple contexts against the same database)
- Repository Implementations
- Database Migrations
- Entity Framework Configurations (Table Configuration via Fluent API)
- Persistence Code e.g. Unit Of Work

You can read more about entity framework configuration here: https://docs.microsoft.com/en-us/ef/core/modeling/
Be sure to follow the fluent API configuration examples and do not include entity framework specific code in the core entity classes.