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

## Overview

### Usage

#### Docker
```
docker compose up --build -d
```
#### Kubernates

- Prerequisites 

1. A Kubernetes cluster
2. A Docker Registry

- Set Minikube Environment
  - Install Minikube - [Guide](https://minikube.sigs.k8s.io/docs/start/?arch=%2Fwindows%2Fx86-64%2Fstable%2F.exe+download)
  - Mount Config Volume - [Guide](https://minikube.sigs.k8s.io/docs/handbook/mount/)
  - Enable Docker Registry - [Guide](https://minikube.sigs.k8s.io/docs/handbook/registry/)
  - Accessing Apps - [Guide](https://minikube.sigs.k8s.io/docs/handbook/accessing/)
  - Minikube Dashboard - [Guide](https://minikube.sigs.k8s.io/docs/handbook/dashboard/)

- Step

1. Start Minikube
```
minikube start
```
2. Mount Config Volume
  <span><repo_path></span> is the root of the repo.
```
minikube mount <repo_path>:/mnt
```
3. Enable Docker Registry
```
minikube addons enable registry
kubectl port-forward --namespace kube-system service/registry 5000:80
docker run --rm -it --network=host alpine ash -c "apk add socat && socat TCP-LISTEN:5000,reuseaddr,fork TCP:$(minikube ip):5000"
```
4. Build Image & Push To Minikube Registry
```
./docker/push-images.ps1
```
5. Start App
```
kubectl apply -f ./k8s/namespace.yaml
kubectl apply -f ./k8s/deployment
```
6. Expose Service Port to localhost
```
minikube tunnel
```
7. Open Minikube Dashboard
```
minikube dashboard
```

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
    - .NET API Gateway(Yarp): 20002
    - .NET Service: 20003    
  - Storage: 210{00-99}
    - Redis: 21001
    - Kafka: 21002
    - SQL Server: 21003
  - Exporter: 220{00-99}
    - Nginx of frontend exporter: 22001
    - Redis exporter: 22002
    - Kafka exporter: 22003
    - Elasticsearch exporter: 22004
    - Logstash exorter: 22005
    - zookeeper exporter: 22006
    - SQL Server exporter: 22007
  - Monitor: 230{00-99}
    - Prometheus: 23001
    - Elasticsearch: 23002
    - Logstash Beats: 23003
    - Logstash Monitor API: 23004
    - Kibana: 23005
    - grafana: 23006
    - zookeeper: 23007

### Test Work Flow System

## Step3 Develop App

### Develop Guide

###
