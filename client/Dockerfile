# base image
FROM node:9.6.1 AS builder

# set working directory
WORKDIR /app

# install and cache app dependencies
COPY package.json ./
RUN npm install
RUN npm install react-scripts@1.1.1 -g 

# Bundle app source & build
COPY . .
RUN npm run build

# Deploy static using nginx
FROM nginx:1.15
COPY --from=builder /app/build/ /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf