# Project Guidelines

## Code Style

- Use Prettier with 100 character line width and single quotes
- Strict TypeScript configuration: strict mode enabled, no implicit returns, etc.
- Component files: Use `.component.ts`, `.component.html`, `.component.css` for consistency
- Services: Name as `service-name.service.ts`
- DTOs: Use `.dto.ts` suffix for API contracts
- Reference: [src/app/app.ts](src/app/app.ts) for standalone component pattern

## Architecture

- Modern Angular 18 with standalone components (no NgModules)
- JWT-based authentication with Google OAuth integration
- Functional HTTP interceptor for automatic token attachment
- Lazy-loaded ReportsModule under `/reports` path
- Services as singletons with `providedIn: 'root'`
- No centralized state management; services maintain local state

## Build and Test

- `npm start` or `ng serve` for development server
- `ng build` for production build
- `ng test` for unit tests with Jasmine

## Conventions

- Reactive Forms with FormBuilder and Validators
- Component-scoped CSS with Angular Material theme
- API calls via HttpClient with base URL from environment
- Role-based access checks in components (no route guards)
- Hebrew comments and RTL support for accounting context

## Potential Pitfalls

- Avoid hardcoded API URLs; use environment configuration
- Implement proper error handling for HTTP calls
- Use consistent service patterns (all via ApiService)
- Fix the wildcard route redirect to '/login' (currently '/log/in')
- Ensure form validators are properly managed in edit modes

## Critical Notes

- Do not use emojis
- Do not add comments
- Prepare code that is suitable for production</content>
<parameter name="filePath">c:\Users\This User\Desktop\פרילנס\חדש חדש בסד\accounting-office-management\Fronted\Angular-app\CLAUDE.md