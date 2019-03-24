# Deploy static using nginx
FROM nginx:1.15
#COPY --from=builder /app/build/ /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
