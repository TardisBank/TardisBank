const proxy = require("http-proxy-middleware");

module.exports = function(app) {
  app.use(
    proxy("/api", {
      target: `http://${process.env.PROXY_API || "localhost:5000"}/`,
      pathRewrite: {
        "^/api/": "/" // remove base path
      }
    })
  );
};
