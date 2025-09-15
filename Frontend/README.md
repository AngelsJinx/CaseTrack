# Frontend
This is my very straightforward UI. It's a little rushed, and lacks tests of any sort. Sorry about that!

It's a pretty standard Quasar project, using VueJS, vue-router, and a Pinia store. \
The important bits are:

* src/components/models.ts - Which houses my front-end equivalents of the backend DTOs
* src/pages/IndexPage.vue - The entrypoint, and main logic of the project - there are some more details below.
* src/stores/taskStore.ts - My pinia store, that handles state management within the app & API calls.
* src/components/*.vue - The core web components.

# IndexPage.vue
This is admittedly a bit messy!

I centralised the logic here so it's largely a one-stop-shop for a code review. The web components emit events when the user tries to do something (edit, delete, save a new one), and this page handles those events - delegating to the store for the API requests.

There is some benefit to this approach, in that our business logic is in a single location. It also keeps the components cleaner, so it would be easier to test them (compared to those components directly accessing the store). \
However as the project continues to grow, parts of this business logic would want to be split up into conceptual models - perhaps using composables.

# What I couldn't get to
I would have loved to add drag-and-drop to move tickets between the different columns.

It would also be good to have tests, perhaps using Puppeteer.