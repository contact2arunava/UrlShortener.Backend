# UrlShortener.Backend
Enterprise‑Grade Backend for Agentic URL Shortener
This backend is built using C#/.NET, SQL Server, and an agentic SDLC orchestration engine designed with enterprise governance, auditability, and reliability in mind.

Step B — URL Domain Model + Basic Shortener API
New Endpoints
POST /api/urls
Creates a short URL.

Request:

json
{
  "longUrl": "https://example.com"
}
Response:

json
{
  "id": "guid",
  "code": "abc12345",
  "shortUrl": "http://localhost:5000/abc12345",
  "longUrl": "https://example.com"
}
GET /{code}
Redirects to the long URL.

Example:

Code
GET http://localhost:5000/abc12345
→ 302 Redirect to https://example.com