# Trackflow

Responsive Angular 22 UI for the Track Management API. It includes a sign-in page, a filterable track catalog, and track details with distribution statuses. The app uses Angular signals and Tailwind CSS 4.

## Run locally

1. Start the .NET API using its `https` launch profile (`https://localhost:7106`).
2. From this folder, run `npm start`.
3. Open `http://localhost:4200`.

The Angular dev server proxies `/api` requests to `https://localhost:7106` using `proxy.conf.json`, so API calls stay same-origin in the browser. For deployment, serve the frontend and API on the same origin or configure an appropriate API proxy.

## Build and tests

Run `npm run build` to create the production build, or `npm test` to run the Angular tests.
