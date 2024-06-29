# SE_UE2_SimpleIdentityManagement
Before starting the application, Docker Desktop must be started.
To start the application, navigate to /inf and run dock-init.ps1 in PowerShell.
## Create a simple service (topics below) with the following features:
- Simple client/server communication
- Authentication
- Two simple views (list view, detail view)
- CRUD operations (create, read, update, delete)
- Simple server-side data storage, e.g., NoSQL-DB, MySQL, file-based
- Technology selection
## Functionality
- EventService – Managing participants of an event.
  1. Simple users: can register for an event with their email address and provide a password to make changes later (unregister, change number of accompanying persons)
  2. Administrator: has a list of all registered participants (including the total number of accompanying persons) and can inspect individual registrations.
## Authentication platform
- Keycloak: https://www.keycloak.org/
- Selection of another platform is permissible.

