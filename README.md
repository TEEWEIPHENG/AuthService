# AuthService

## Authentication
- POST `/api/auth/register`
- POST `/api/auth/login`
- POST `/api/auth/refresh`
- POST `/api/auth/logout`

## User Profile
- GET `/api/auth/me`
- PUT `/api/auth/me`
- POST `/api/auth/change-password`

## Password Reset / Email Verification
- POST `/api/auth/password/forgot`
- POST `/api/auth/password/reset`
- POST `/api/auth/email/verify`
- POST `/api/auth/email/resend`

## Session / Token Management
- GET `/api/auth/sessions`
- DELETE `/api/auth/sessions/{sessionId}`
- DELETE `/api/auth/sessions` (revoke all)

## Admin / User Management (Protected)
- POST `/api/auth/users/{id}/disable`
- POST `/api/auth/users/{id}/enable`
- DELETE `/api/auth/users/{id}` (soft delete)
- POST `/api/auth/users/{id}/unlock`

## Health & Info
- GET `/api/auth/health`
- GET `/api/auth/info`
