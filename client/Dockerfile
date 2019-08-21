# base image
FROM node:9.6.1 AS builder

# set working directory
RUN mkdir /app
WORKDIR /app

# install and cache app dependencies
COPY ["package.json", "yarn.lock","tsconfig.json", "tsconfig.prod.json", "tslint.json", "./"]
RUN yarn install

COPY /src /app/src 

COPY /public /app/public
RUN yarn build

# Deploy static using nginx
# FROM nginx:1.15
# COPY --from=builder /app/build/ /usr/share/nginx/html
# COPY nginx.conf /etc/nginx/conf.d/default.conf
