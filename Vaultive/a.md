I am using **Table-Per-Hierarchy (TPH)**, which stores all inherited types in a **single table**. This approach uses a _discriminator column_ — a _hidden field_ that identifies the actual type of each record (e.g., `Movie`, `Series`, `Documentary`).

**EF Core** adds this column automatically unless you configure it manually, and by default, it uses the **class name** as the value. **TPH** avoids creating a separate table for each subclass, which keeps the database schema **simpler** and improves query performance when dealing with **polymorphic types**.
