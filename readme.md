### Intelligent Authentication & Security Monitoring Platform

A production-grade authentication and security monitoring system built with ASP.NET Core, OAuth 2.0, JWT, Redis, PostgreSQL, QuestDB, and React.
This project goes beyond basic login systems and implements real-time security intelligence, automated threat detection, and risk-based access control similar to modern identity providers.

---

### Key Highlights

Secure authentication with JWT + Refresh Token Rotation

Google OAuth 2.0 integration

Risk-based authentication engine

Automated brute-force and IP attack detection

Real-time security telemetry pipeline (QuestDB)

Live Security Operations Center (SOC) dashboard

Background security monitoring service

This system behaves like a lightweight SIEM + Identity Platform.


---

### System Architecture

Client (React Dashboard)
        |
        v
ASP.NET Core Authentication API
        |
        |-- PostgreSQL  → User Accounts
        |-- Redis       → Refresh Tokens, Risk Scores, IP Blocks
        |-- QuestDB     → Security Event Telemetry
        |
Background Security Monitoring Service
        |
        v
Automated IP Blocking & Threat Detection

---

### Core Authentication Features

1. Local Authentication

Secure password hashing

JWT access tokens (short-lived)

Refresh token rotation stored in Redis

2. Google OAuth 2.0

External authentication using Google

Automatic user provisioning

Unified JWT issuance after OAuth login

3. JWT-Based Authorization

Protected API routes

Role-based authorization support

Stateless access token validation


---

### Advanced Security Engineering

This project implements real-world security mechanisms rarely found in personal projects.

---

### User Risk Scoring Engine

Each user is assigned a dynamic risk score:

Event	Risk Impact
Failed login	+10
Successful login	-5
Suspicious activity	+20

Risk scores stored in Redis

Accounts auto-locked when risk ≥ 70

Mimics Azure AD / Okta sign-in risk policies

---

### IP Reputation & Risk Tracking

Each IP address maintains a risk score:

Behavior	IP Risk Impact
Repeated failed logins	+10
Brute-force pattern	+30

Stored in Redis with TTL

Automatically escalates to blocking

---

### Automated IP Blocking

IPs are automatically blocked when:

IP risk ≥ 50

Detected as brute-force attacker

Blocked IPs are:

Stored in Redis (15 min TTL)

Logged to QuestDB

Enforced at API gateway level

---

### Background Security Monitoring Service

A hosted background service continuously:

Scans QuestDB for suspicious login patterns

Detects brute-force attacks

Automatically blocks attacking IPs

Logs security actions for audit trail

Runs every 2 minutes:

QuestDB → Detect attack → Redis block → Log event

---

### Real-Time Security Telemetry (QuestDB)

All authentication and security events are streamed to QuestDB:

Event Type
LOGIN_SUCCESS
LOGIN_FAILED
IP_BLOCKED
BRUTE_FORCE_BLOCK

---

### Real-time dashboards

SOC Dashboard (React)

A live Security Operations Center dashboard shows:

Live Metrics

Logins (last 5 minutes)

Failed logins (last 5 minutes)

Blocked IPs (last hour)

Threat Intelligence

Top attacking IP addresses

OAuth vs Local login statistics

Live auto-refresh every 5 seconds

---

### Token Lifecycle

User logs in

Receives:

Short-lived JWT access token

Long-lived refresh token

Refresh token stored in Redis

On refresh:

Old token invalidated

New token issued

On logout:

Refresh token revoked server-side

Prevents:

Token replay attacks

Stolen refresh token abuse

---

### Technologies Used

### Backend:	ASP.NET Core Web API

### Authentication:	JWT

### OAuth 2.0

### Database:	PostgreSQL

### Caching & Security State:	Redis

### Telemetry:	QuestDB

### Frontend:	React

### Charts:	Recharts

---

### Security-First Design

This system is designed following Zero Trust principles:

Short-lived access tokens

Server-side token invalidation

Continuous risk evaluation

Automated threat mitigation

Full security audit trail