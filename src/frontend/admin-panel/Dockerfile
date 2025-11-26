# Build Stage
FROM node:20-alpine as build
WORKDIR /app
COPY package*.json ./
# If npm is not available in the context, we assume package-lock.json might be missing or we just install.
# Since we generated package.json manually, we might not have a lock file.
RUN npm install
COPY . .
RUN npm run build

# Production Stage
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
