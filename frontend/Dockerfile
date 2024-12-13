# syntax=docker/dockerfile:1

# Comments are provided throughout this file to help you get started.
# If you need more help, visit the Dockerfile reference guide at
# https://docs.docker.com/go/dockerfile-reference/

# Want to help us make this template better? Share your feedback here: https://forms.gle/ybq9Krt8jtBL3iCk7

ARG NODE_VERSION=21.5.0
ARG PNPM_VERSION=9.12.3

################################################################################
# Use node image for base image for all stages.
FROM node:${NODE_VERSION}-alpine AS build

# Set working directory for all build stages.
WORKDIR /app

# 複製 package.json 和 pnpm-lock.yaml 以便安裝依賴
COPY package.json pnpm-lock.yaml ./

# 安裝所有依賴
RUN npm install -g pnpm && pnpm install

# 複製 Vue.js 專案的所有檔案到容器
COPY . .

# 構建應用程式
RUN pnpm run build

# 使用 Nginx 作為運行階段，來提供靜態文件
FROM nginx:alpine AS final

# 複製構建好的文件到 Nginx 服務器的公開目錄
COPY --from=build /app/dist /usr/share/nginx/html

# 暴露 Nginx 的端口 80
EXPOSE 80

# 啟動 Nginx
CMD ["nginx", "-g", "daemon off;"]
