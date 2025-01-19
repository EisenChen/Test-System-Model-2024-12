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

# install package first, then "COPY . ." won't reinstall again.
COPY package.json pnpm-lock.yaml ./

RUN npm install -g pnpm && pnpm install

COPY . .

ARG API_URL
ENV VITE_API_URL=${API_URL}

RUN pnpm run build

FROM nginx:alpine AS final

COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
