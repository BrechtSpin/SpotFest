FROM node:20-alpine AS builder-frontend

WORKDIR /app
COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build -- --prod


FROM node:20-alpine AS runtime-frontend
RUN npm install -g serve

WORKDIR /app
COPY --from=builder-frontend /app/dist/*/browser ./

EXPOSE 80
CMD ["serve", "-s", ".", "-l", "80"]
