FROM node:20-alpine AS builder

WORKDIR /app
COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build --prod


FROM node:20-alpine AS runtime
RUN npm install -g serve

WORKDIR /app
COPY --from=builder /app/dist/*/browser ./

EXPOSE 80
CMD ["serve", "-s", ".", "-l", "80"]
