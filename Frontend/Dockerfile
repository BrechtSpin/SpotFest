FROM node:20-alpine

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

RUN npm run build --prod

# Install a lightweight static server
RUN npm install -g http-server

# Set the working dir to the built Angular app
WORKDIR /app/dist/Frontend

EXPOSE 80

CMD ["http-server", "-p", "80"]
