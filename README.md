# ProgressPlayMCP API Documentation

## API Gateway

The ProgressPlayMCP API now provides a unified API Gateway that consolidates all data access endpoints into a single controller. This simplifies the API structure and provides a consistent way to access various types of data.

### Base URL

All data access endpoints are now available through the base URL:

```
/api/gateway
```

### Available Endpoints

| Endpoint | HTTP Method | Description |
|----------|-------------|-------------|
| `/api/gateway/daily-actions` | POST | Get players' summarized daily financial activity |
| `/api/gateway/player-details` | POST | Get players' life-time details and stats |
| `/api/gateway/transactions` | POST | Get players' individual transaction details |
| `/api/gateway/player-games` | POST | Get players' summarized play activity |
| `/api/gateway/player-summary` | POST | Get players' summarized financial activity |
| `/api/gateway/income-access` | POST | Get data in Income Access format |

### Authentication

All endpoints require authentication using JWT Bearer tokens. Include your token in the Authorization header:

```
Authorization: Bearer <your-token>
```

You can obtain a token through the `/api/Auth/login` endpoint.

### Request Format

All requests must include:

1. Date ranges in the format YYYY/MM/DD
2. At least one WhiteLabel ID
3. Additional filters as needed for each endpoint

### Permission Filtering

All requests are filtered based on the user's permissions:

1. WhiteLabel permissions - Only data for WhiteLabels the user has access to will be returned
2. Affiliate permissions - When specified, only data for Affiliates the user has access to will be returned

### Response Format

All successful responses will return:

1. HTTP 200 OK status code
2. JSON array containing the requested data

### Error Responses

Possible error responses include:

| Status Code | Description |
|-------------|-------------|
| 400 | Bad Request - Invalid parameters or date format |
| 401 | Unauthorized - Authentication required |
| 403 | Forbidden - User doesn't have permission to access the requested data |
| 500 | Internal Server Error - An error occurred while processing the request |

## Examples

### Get Daily Actions

```json
POST /api/gateway/daily-actions
{
  "dateStart": "2025/01/01",
  "dateEnd": "2025/01/31",
  "whiteLabels": [1, 2],
  "affiliateId": "12345"
}
```

### Get Player Details

```json
POST /api/gateway/player-details
{
  "registrationDateStart": "2024/01/01",
  "registrationDateEnd": "2025/01/01",
  "whiteLabels": [1, 2]
}
```

### Get Transactions

```json
POST /api/gateway/transactions
{
  "dateStart": "2025/04/01",
  "dateEnd": "2025/04/30",
  "whiteLabels": [1, 2],
  "transactionTypes": ["deposit", "withdrawal"]
}
```

## Other APIs

### MCP (Model Context Protocol)

The MCP API remains unchanged and is accessible through the following endpoints:

- `POST /mcp/context` - Store a context for future use
- `GET /mcp/context/{contextId}` - Retrieve a stored context
- `POST /mcp/request` - Process a model request
- `POST /mcp/stream` - Process a streaming model request

### Authentication

The authentication API remains unchanged and is accessible through the following endpoints:

- `POST /api/Auth/login` - Login with username and password
- `POST /api/Auth/refresh-token` - Refresh an expired token