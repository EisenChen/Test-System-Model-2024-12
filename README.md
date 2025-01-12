# Goal

Build a system model by using following tools.

1. ASP.NET Core
2. Vue.js
3. Microsoft SQL Server
4. GitLab
5. NUnit
6. Vitest
7. K6
8. docker
9. K8s
10. Redis
11. Kafka
12. ELK
13. Grafana
14. Prometheus

## Step1 Categorize tools

- Frontend
  - Framework
    - Vue.js

- Backend
  - Service
    - ASP.NET Core
  - Store
    - Redis: Cache
    - SQL Server: Data Store
  - Message Broker
    - Kafka

- Develop

  - Source Repo
    - Gitlab
  - Test Tools
    - Vitest
      frontend test framework, unit test, integration test
    - NUnit
      C# unit test
    - K6
      stress testing
  - Containerization
    - Docker

- Production
  - Deployment
    - K8s
  - Data collection & analyze
    - ELK (Elasticsearch, Logstash, Kibana)
      Event log, user behavior data collector
    - Prometheus
      system, applications, services data collector.
  - Data Visualization
    - Grafana

## Step2 Build Work Flow

### System Architecture Diagram

![System Architecture](./doc/image/System%20Architecture.png)

### Build Infrastructure

### Create Project

### Work Pipeline

### Connect Work Flow System

- EXPOSE PORTS
  - Service: 200{00-99}
    - Vue Frontend: 20001
    - .NET API Gateway: 2002
    - .NET Service: 20003    
  - Storage: 210{00-99}
    - Redis: 21001
    - Kafka: 21002
  - Exporter: 220{00-99}
    - Nginx of frontend exporter: 22001
    - Redis exporter: 22002
    - Kafka exporter: 22003
    - Elasticsearch exporter: 22004
    - Logstash exoirter: 22005
  - Monitor: 230{00-99}
    - Prometheus: 23001
    - Elasticsearch: 23002
    - Logstash Beats: 23003
    - Logstash Monitor API: 23004
    - Kibana: 23005
    - grafana: 23006

### Test Work Flow System

## Step3 Develop App

### Develop Guide

###
