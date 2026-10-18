UrlShortener.Backend/
 ├── UrlShortener.Api/            → API layer (controllers, DI composition root)
 ├── UrlShortener.Domain/         → Entities + Interfaces (pure business)
 ├── UrlShortener.Data/           → Dapper repositories + SQL scripts
 ├── UrlShortener.Service/        → Business services (optional but recommended)
 ├── UrlShortener.Tests/          → Unit + integration tests
 └── README.md


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

Step D — Analytics (UrlHits)
Added
UrlHits table

Dapper hit repository

Redirect endpoint logs each click

New endpoint:

Code
GET /api/urls/{id}/stats
Example Response
json
{
  "id": "guid",
  "code": "abc12345",
  "longUrl": "https://example.com",
  "totalHits": 12,
  "lastAccessed": "2026-10-16T14:22:11Z",
  "hits": [
    {
      "timestamp": "2026-10-16T14:22:11Z",
      "userAgent": "Mozilla/5.0",
      "ipHash": "base64hash"
    }
  ]
}