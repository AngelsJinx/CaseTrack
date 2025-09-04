FROM node as build

WORKDIR /app
COPY package*.json ./
RUN yarn global add @quasar/cli
COPY . .
RUN npm install
RUN yarn build

FROM nginx:stable-alpine as final
COPY --from=build /app/dist/spa /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]