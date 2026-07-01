# Contracts Layout

This folder contains API contracts (DTOs) grouped by feature. Keep these contracts independent from Entity Framework navigation properties.

## Package structure

- `Contracts/Notes`: note create, update, and response DTOs.
- `Contracts/Users`: user create, update, summary, and response DTOs.
- `Contracts/NoteGroups`: note group create, update, and response DTOs.
- `Contracts/Permissions`: permission grant, update, and response DTOs.

## Design rules

- Use `Create...RequestDto` for POST payloads.
- Use `Update...RequestDto` for PUT/PATCH payloads.
- Use `...ResponseDto` for API outputs.
- Do not expose entity navigation properties in DTOs.
- Server-owned fields (for example identity, owner user ID) should not be writable from request DTOs unless explicitly required.
- Services should consume and return DTOs at boundaries.
